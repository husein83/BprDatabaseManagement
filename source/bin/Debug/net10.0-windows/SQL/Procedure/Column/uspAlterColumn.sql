USE master;
GO

IF OBJECT_ID(N'uspAlterColumn', N'P') IS NOT NULL
    DROP PROCEDURE uspAlterColumn;
GO

CREATE PROCEDURE uspAlterColumn
    @DatabaseName       NVARCHAR(128),
    @SchemaName         NVARCHAR(128),
    @TableName          NVARCHAR(128),
    @ColumnName         NVARCHAR(128),
    @DataType           NVARCHAR(50)    = NULL,
    @Length             INT             = NULL,
    @Precision          INT             = NULL,
    @Scale              INT             = NULL,
    @IsNullable         BIT             = NULL,
    @DefaultValue       NVARCHAR(500)   = NULL,
    @DefaultName        NVARCHAR(128)   = NULL,
    @Collation          NVARCHAR(128)   = NULL,
    @Description        NVARCHAR(500)   = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL           NVARCHAR(MAX);
    DECLARE @ColumnExists  BIT = 0;
    DECLARE @TypeDef       NVARCHAR(200);
    DECLARE @NullDef       NVARCHAR(20);
    DECLARE @FullTableName NVARCHAR(300);
    DECLARE @TblExists     BIT = 0;
    DECLARE @ExistingPKName NVARCHAR(128);
    DECLARE @IsComputed    BIT = 0;

    -- Validation
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@ColumnName)), N'') IS NULL
    BEGIN
        RAISERROR(N'DatabaseName, SchemaName, TableName, and ColumnName are required.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    SET @FullTableName = QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName);

    -- Check table exists
    SET @SQL = N'
        SELECT @E = 1
        FROM   [' + @DatabaseName + N'].sys.tables t
        JOIN   [' + @DatabaseName + N'].sys.schemas s ON t.schema_id = s.schema_id
        WHERE  t.name = @TblName AND s.name = @SchName;
    ';
    EXEC sp_executesql @SQL,
        N'@TblName NVARCHAR(128), @SchName NVARCHAR(128), @E BIT OUTPUT',
        @TableName, @SchemaName, @TblExists OUTPUT;

    IF @TblExists = 0
    BEGIN
        RAISERROR(N'Table [%s].[%s] does not exist in database [%s].', 16, 1, @SchemaName, @TableName, @DatabaseName);
        RETURN;
    END

    -- Check column exists
    SET @SQL = N'
        SELECT @E = 1, @IsComp = c.is_computed
        FROM   [' + @DatabaseName + N'].sys.columns c
        JOIN   [' + @DatabaseName + N'].sys.tables t ON c.object_id = t.object_id
        JOIN   [' + @DatabaseName + N'].sys.schemas s ON t.schema_id = s.schema_id
        WHERE  c.name = @ColName AND t.name = @TblName AND s.name = @SchName;
    ';
    EXEC sp_executesql @SQL,
        N'@ColName NVARCHAR(128), @TblName NVARCHAR(128), @SchName NVARCHAR(128), @E BIT OUTPUT, @IsComp BIT OUTPUT',
        @ColumnName, @TableName, @SchemaName, @ColumnExists OUTPUT, @IsComputed OUTPUT;

    IF @ColumnExists = 0
    BEGIN
        RAISERROR(N'Column [%s] does not exist in table [%s].[%s].', 16, 1, @ColumnName, @SchemaName, @TableName);
        RETURN;
    END

    IF @IsComputed = 1
    BEGIN
        RAISERROR(N'Column [%s] is a computed column. Altering computed columns is not supported. Drop and recreate the column.', 16, 1, @ColumnName);
        RETURN;
    END

    -- If DataType is provided, alter the column
    IF @DataType IS NOT NULL
    BEGIN
        -- Build type definition
        SET @TypeDef = @DataType;

        IF @DataType IN (N'NVARCHAR', N'VARCHAR', N'NCHAR', N'CHAR', N'BINARY', N'VARBINARY')
        BEGIN
            IF @Length IS NULL OR @Length = -1
                SET @TypeDef = @TypeDef + N'(MAX)';
            ELSE
                SET @TypeDef = @TypeDef + N'(' + CAST(@Length AS NVARCHAR(10)) + N')';
        END
        ELSE IF @DataType IN (N'DECIMAL', N'NUMERIC')
        BEGIN
            IF @Precision IS NOT NULL AND @Scale IS NOT NULL
                SET @TypeDef = @TypeDef + N'(' + CAST(@Precision AS NVARCHAR(10)) + N',' + CAST(@Scale AS NVARCHAR(10)) + N')';
        END
        ELSE IF @DataType IN (N'FLOAT', N'REAL') AND @Precision IS NOT NULL
            SET @TypeDef = @TypeDef + N'(' + CAST(@Precision AS NVARCHAR(10)) + N')';
        ELSE IF @DataType = N'DATETIME2' AND @Scale IS NOT NULL
            SET @TypeDef = @TypeDef + N'(' + CAST(@Scale AS NVARCHAR(10)) + N')';

        IF @Collation IS NOT NULL AND @DataType IN (N'NVARCHAR', N'VARCHAR', N'NCHAR', N'CHAR', N'TEXT', N'NTEXT')
            SET @TypeDef = @TypeDef + N' COLLATE ' + @Collation;

        IF @IsNullable IS NOT NULL
            SET @NullDef = CASE WHEN @IsNullable = 1 THEN N' NULL' ELSE N' NOT NULL' END;
        ELSE
            SET @NullDef = N' NULL'; -- Default

        SET @TypeDef = @TypeDef + @NullDef;

        -- Check if column is part of a PK
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            SELECT @PKNameOut = kc.name
            FROM   sys.key_constraints kc
            JOIN   sys.index_columns ic ON ic.object_id = kc.parent_object_id
                                        AND ic.index_id = kc.unique_index_id
            JOIN   sys.columns c ON c.object_id = ic.object_id
                                 AND c.column_id = ic.column_id
            WHERE  kc.parent_object_id = OBJECT_ID(' + QUOTENAME(@FullTableName, '''') + N')
              AND  kc.type = ''PK''
              AND  c.name = @ColName;
        ';
        EXEC sp_executesql @SQL,
            N'@ColName NVARCHAR(128), @PKNameOut NVARCHAR(128) OUTPUT',
            @ColumnName, @ExistingPKName OUTPUT;

        -- Drop PK if exists
        IF @ExistingPKName IS NOT NULL
        BEGIN
            SET @SQL = N'
                USE [' + @DatabaseName + N'];
                ALTER TABLE ' + @FullTableName + N' DROP CONSTRAINT ' + QUOTENAME(@ExistingPKName) + N';
            ';
            EXEC sp_executesql @SQL;
            PRINT N'Dropped existing PK [' + @ExistingPKName + N'] to allow column alteration.';
        END

        -- Drop existing default constraint
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @DFName NVARCHAR(128);
            SELECT @DFName = dc.name
            FROM   sys.default_constraints dc
            JOIN   sys.columns c ON dc.parent_object_id = c.object_id
                                 AND dc.parent_column_id = c.column_id
            WHERE  c.object_id = OBJECT_ID(' + QUOTENAME(@FullTableName, '''') + N')
              AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';
            IF @DFName IS NOT NULL
                EXEC(''ALTER TABLE ' + @FullTableName + N' DROP CONSTRAINT ['' + @DFName + '']'');
        ';
        EXEC sp_executesql @SQL;

        -- Alter column
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            ALTER TABLE ' + @FullTableName + N' ALTER COLUMN ' + QUOTENAME(@ColumnName) + N' ' + @TypeDef + N';
        ';
        EXEC sp_executesql @SQL;
        PRINT N'Column [' + @ColumnName + N'] altered successfully.';

        -- Recreate PK if it was dropped
        IF @ExistingPKName IS NOT NULL
        BEGIN
            SET @SQL = N'
                USE [' + @DatabaseName + N'];
                ALTER TABLE ' + @FullTableName + N' ADD CONSTRAINT ' + QUOTENAME(@ExistingPKName) + N' PRIMARY KEY (' + QUOTENAME(@ColumnName) + N');
            ';
            EXEC sp_executesql @SQL;
            PRINT N'Recreated PK [' + @ExistingPKName + N'].';
        END
    END

    -- Update default constraint if provided
    IF @DefaultValue IS NOT NULL
    BEGIN
        -- Drop existing default
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            DECLARE @DFName NVARCHAR(128);
            SELECT @DFName = dc.name
            FROM   sys.default_constraints dc
            JOIN   sys.columns c ON dc.parent_object_id = c.object_id
                                 AND dc.parent_column_id = c.column_id
            WHERE  c.object_id = OBJECT_ID(' + QUOTENAME(@FullTableName, '''') + N')
              AND  c.name = ' + QUOTENAME(@ColumnName, '''') + N';
            IF @DFName IS NOT NULL
                EXEC(''ALTER TABLE ' + @FullTableName + N' DROP CONSTRAINT ['' + @DFName + '']'');
        ';
        EXEC sp_executesql @SQL;

        -- Add new default
        IF @DefaultName IS NULL
            SET @DefaultName = N'DF_' + @SchemaName + N'_' + @TableName + N'_' + @ColumnName;

        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            ALTER TABLE ' + @FullTableName + N' ADD CONSTRAINT ' + QUOTENAME(@DefaultName) + N' DEFAULT (' + @DefaultValue + N') FOR ' + QUOTENAME(@ColumnName) + N';
        ';
        EXEC sp_executesql @SQL;
        PRINT N'Default constraint updated.';
    END

    -- Update description
    IF @Description IS NOT NULL
    BEGIN
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            IF EXISTS (
                SELECT 1 FROM sys.extended_properties
                WHERE major_id = OBJECT_ID(' + QUOTENAME(@FullTableName, '''') + N')
                  AND minor_id = (
                      SELECT column_id FROM sys.columns
                      WHERE object_id = OBJECT_ID(' + QUOTENAME(@FullTableName, '''') + N')
                        AND name = ' + QUOTENAME(@ColumnName, '''') + N'
                  )
                  AND name = N''MS_Description''
            )
                EXEC sys.sp_updateextendedproperty
                    @name  = N''MS_Description'',
                    @value = ' + QUOTENAME(@Description, '''') + N',
                    @level0type = N''SCHEMA'', @level0name = ' + QUOTENAME(@SchemaName, '''') + N',
                    @level1type = N''TABLE'',  @level1name = ' + QUOTENAME(@TableName, '''') + N',
                    @level2type = N''COLUMN'', @level2name = ' + QUOTENAME(@ColumnName, '''') + N';
            ELSE
                EXEC sys.sp_addextendedproperty
                    @name  = N''MS_Description'',
                    @value = ' + QUOTENAME(@Description, '''') + N',
                    @level0type = N''SCHEMA'', @level0name = ' + QUOTENAME(@SchemaName, '''') + N',
                    @level1type = N''TABLE'',  @level1name = ' + QUOTENAME(@TableName, '''') + N',
                    @level2type = N''COLUMN'', @level2name = ' + QUOTENAME(@ColumnName, '''') + N';
        ';
        EXEC sp_executesql @SQL;
        PRINT N'Column description updated.';
    END
END
GO