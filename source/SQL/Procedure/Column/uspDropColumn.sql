USE master;
GO

IF OBJECT_ID(N'uspDropColumn', N'P') IS NOT NULL
    DROP PROCEDURE uspDropColumn;
GO

CREATE PROCEDURE uspDropColumn
    @DatabaseName       NVARCHAR(128),
    @SchemaName         NVARCHAR(128),
    @TableName          NVARCHAR(128),
    @ColumnName         NVARCHAR(128),
    @IgnoreIfNotExists  BIT             = 1,
    @ForceDropDependent BIT             = 0
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL       NVARCHAR(MAX);
    DECLARE @Exists    BIT = 0;
    DECLARE @FullTable NVARCHAR(300);

    SET @FullTable = QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspDropColumn] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    IF NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@ColumnName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspDropColumn] SchemaName, TableName, and ColumnName are required.', 16, 1);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- CHECK COLUMN EXISTS
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
        USE [' + @DatabaseName + N'];
        SELECT @E = 1
        FROM   sys.columns c
        JOIN   sys.tables t ON c.object_id = t.object_id
        JOIN   sys.schemas s ON t.schema_id = s.schema_id
        WHERE  c.name = @ColName
          AND  t.name = @TblName
          AND  s.name = @SchName;
    ';
    EXEC sp_executesql @SQL,
        N'@ColName NVARCHAR(128), @TblName NVARCHAR(128), @SchName NVARCHAR(128), @E BIT OUTPUT',
        @ColumnName, @TableName, @SchemaName, @Exists OUTPUT;

    IF @Exists = 0
    BEGIN
        IF @IgnoreIfNotExists = 1
        BEGIN
            PRINT N'[uspDropColumn] Column [' + @ColumnName + N'] does not exist. Skipping.';
            RETURN;
        END
        ELSE
        BEGIN
            RAISERROR(N'[uspDropColumn] Column [%s] not found.', 16, 1, @ColumnName);
            RETURN;
        END
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- FORCE DROP DEPENDENT OBJECTS
    -- ═══════════════════════════════════════════════════════════════════
    IF @ForceDropDependent = 1
    BEGIN
        -- ───────────────────────────────────────────────────────────────
        -- 1. DROP FOREIGN KEYS REFERENCING THIS COLUMN (from other tables)
        -- ───────────────────────────────────────────────────────────────
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @fkName NVARCHAR(128);
            DECLARE @refSchema NVARCHAR(128);
            DECLARE @refTable NVARCHAR(128);
            DECLARE fk_ref_cur CURSOR FOR
                SELECT 
                    fk.name,
                    SCHEMA_NAME(ref_t.schema_id) AS RefSchema,
                    ref_t.name AS RefTable
                FROM   sys.foreign_key_columns fkc
                JOIN   sys.foreign_keys fk ON fkc.constraint_object_id = fk.object_id
                JOIN   sys.tables ref_t ON fk.parent_object_id = ref_t.object_id
                JOIN   sys.columns c ON fkc.referenced_object_id = c.object_id
                                     AND fkc.referenced_column_id = c.column_id
                WHERE  fkc.referenced_object_id = OBJECT_ID(' + QUOTENAME(@FullTable, '''') + N')
                  AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';
            OPEN fk_ref_cur;
            FETCH NEXT FROM fk_ref_cur INTO @fkName, @refSchema, @refTable;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                DECLARE @dropFK NVARCHAR(500);
                SET @dropFK = ''ALTER TABLE '' + QUOTENAME(@refSchema) + ''.'' + QUOTENAME(@refTable) + '' DROP CONSTRAINT '' + QUOTENAME(@fkName);
                EXEC(@dropFK);
                PRINT ''[uspDropColumn] FK dropped from ['' + @refSchema + ''].['' + @refTable + '']: '' + @fkName;
                FETCH NEXT FROM fk_ref_cur INTO @fkName, @refSchema, @refTable;
            END
            CLOSE fk_ref_cur;
            DEALLOCATE fk_ref_cur;
        ';
        EXEC sp_executesql @SQL;

        -- ───────────────────────────────────────────────────────────────
        -- 2. DROP FOREIGN KEYS FROM THIS COLUMN (to other tables)
        -- ───────────────────────────────────────────────────────────────
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @fkName NVARCHAR(128);
            DECLARE fk_cur CURSOR FOR
                SELECT fk.name
                FROM   sys.foreign_key_columns fkc
                JOIN   sys.foreign_keys fk ON fkc.constraint_object_id = fk.object_id
                JOIN   sys.columns c ON fkc.parent_object_id = c.object_id
                                     AND fkc.parent_column_id = c.column_id
                WHERE  fkc.parent_object_id = OBJECT_ID(' + QUOTENAME(@FullTable, '''') + N')
                  AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';
            OPEN fk_cur;
            FETCH NEXT FROM fk_cur INTO @fkName;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC(''ALTER TABLE ' + @FullTable + N' DROP CONSTRAINT ['' + @fkName + '']'');
                PRINT ''[uspDropColumn] FK dropped: '' + @fkName;
                FETCH NEXT FROM fk_cur INTO @fkName;
            END
            CLOSE fk_cur;
            DEALLOCATE fk_cur;
        ';
        EXEC sp_executesql @SQL;

        -- ───────────────────────────────────────────────────────────────
        -- 3. DROP PRIMARY KEY (if column is part of PK)
        -- ───────────────────────────────────────────────────────────────
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @pkName NVARCHAR(128);
            SELECT @pkName = kc.name
            FROM   sys.key_constraints kc
            JOIN   sys.index_columns ic ON ic.object_id = kc.parent_object_id
                                        AND ic.index_id = kc.unique_index_id
            JOIN   sys.columns c ON c.object_id = ic.object_id
                                 AND c.column_id = ic.column_id
            WHERE  kc.parent_object_id = OBJECT_ID(' + QUOTENAME(@FullTable, '''') + N')
              AND  kc.type = ''PK''
              AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';

            IF @pkName IS NOT NULL
            BEGIN
                EXEC(''ALTER TABLE ' + @FullTable + N' DROP CONSTRAINT ['' + @pkName + '']'');
                PRINT ''[uspDropColumn] PK dropped: '' + @pkName;
            END
        ';
        EXEC sp_executesql @SQL;

        -- ───────────────────────────────────────────────────────────────
        -- 4. DROP UNIQUE CONSTRAINTS
        -- ───────────────────────────────────────────────────────────────
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @uqName NVARCHAR(128);
            DECLARE uq_cur CURSOR FOR
                SELECT DISTINCT kc.name
                FROM   sys.key_constraints kc
                JOIN   sys.index_columns ic ON ic.object_id = kc.parent_object_id
                                            AND ic.index_id = kc.unique_index_id
                JOIN   sys.columns c ON c.object_id = ic.object_id
                                     AND c.column_id = ic.column_id
                WHERE  kc.parent_object_id = OBJECT_ID(' + QUOTENAME(@FullTable, '''') + N')
                  AND  kc.type = ''UQ''
                  AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';
            OPEN uq_cur;
            FETCH NEXT FROM uq_cur INTO @uqName;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC(''ALTER TABLE ' + @FullTable + N' DROP CONSTRAINT ['' + @uqName + '']'');
                PRINT ''[uspDropColumn] UNIQUE constraint dropped: '' + @uqName;
                FETCH NEXT FROM uq_cur INTO @uqName;
            END
            CLOSE uq_cur;
            DEALLOCATE uq_cur;
        ';
        EXEC sp_executesql @SQL;

        -- ───────────────────────────────────────────────────────────────
        -- 5. DROP INDEXES
        -- ───────────────────────────────────────────────────────────────
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @ixName NVARCHAR(128);
            DECLARE ix_cur CURSOR FOR
                SELECT DISTINCT i.name
                FROM   sys.index_columns ic
                JOIN   sys.indexes i ON ic.object_id = i.object_id
                                     AND ic.index_id = i.index_id
                JOIN   sys.columns c ON ic.object_id = c.object_id
                                     AND ic.column_id = c.column_id
                WHERE  ic.object_id = OBJECT_ID(' + QUOTENAME(@FullTable, '''') + N')
                  AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N'
                  AND  i.is_primary_key = 0
                  AND  i.is_unique_constraint = 0
                  AND  i.type > 0;
            OPEN ix_cur;
            FETCH NEXT FROM ix_cur INTO @ixName;
            WHILE @@FETCH_STATUS = 0
            BEGIN
                EXEC(''DROP INDEX ['' + @ixName + ''] ON ' + @FullTable + N''');
                PRINT ''[uspDropColumn] Index dropped: '' + @ixName;
                FETCH NEXT FROM ix_cur INTO @ixName;
            END
            CLOSE ix_cur;
            DEALLOCATE ix_cur;
        ';
        EXEC sp_executesql @SQL;

        -- ───────────────────────────────────────────────────────────────
        -- 6. DROP DEFAULT CONSTRAINT
        -- ───────────────────────────────────────────────────────────────
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @dfName NVARCHAR(128);
            SELECT @dfName = dc.name
            FROM   sys.default_constraints dc
            JOIN   sys.columns c ON dc.parent_object_id = c.object_id
                                 AND dc.parent_column_id = c.column_id
            WHERE  dc.parent_object_id = OBJECT_ID(' + QUOTENAME(@FullTable, '''') + N')
              AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';

            IF @dfName IS NOT NULL
            BEGIN
                EXEC(''ALTER TABLE ' + @FullTable + N' DROP CONSTRAINT ['' + @dfName + '']'');
                PRINT ''[uspDropColumn] Default constraint dropped: '' + @dfName;
            END
        ';
        EXEC sp_executesql @SQL;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- DROP COLUMN
    -- ═══════════════════════════════════════════════════════════════════
    BEGIN TRY
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            ALTER TABLE ' + @FullTable + N' DROP COLUMN ' + QUOTENAME(@ColumnName) + N';
        ';
        EXEC sp_executesql @SQL;
        PRINT N'[uspDropColumn] ✓ Column [' + @ColumnName + N'] dropped successfully.';
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(N'[uspDropColumn] Failed to drop column: %s', 16, 1, @ErrMsg);
    END CATCH
END
GO