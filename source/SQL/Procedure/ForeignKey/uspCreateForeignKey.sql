USE master;
GO

IF OBJECT_ID(N'uspCreateForeignKey', N'P') IS NOT NULL
    DROP PROCEDURE uspCreateForeignKey;
GO

CREATE PROCEDURE uspCreateForeignKey
    @DatabaseName        NVARCHAR(128),
    @SchemaName          NVARCHAR(128),
    @TableName           NVARCHAR(128),
    @ColumnName          NVARCHAR(128),
    @RefSchemaName       NVARCHAR(128),
    @RefTableName        NVARCHAR(128),
    @RefColumnName       NVARCHAR(128),
    @OnDelete            NVARCHAR(20)    = N'NO ACTION',
    @OnUpdate            NVARCHAR(20)    = N'NO ACTION',
    @IsNotForReplication BIT             = 0,
    @Enabled             BIT             = 1
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL            NVARCHAR(MAX);
    DECLARE @FKName         NVARCHAR(300);
    DECLARE @FullTableName  NVARCHAR(400);
    DECLARE @FKExists       BIT = 0;

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@ColumnName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@RefSchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@RefTableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@RefColumnName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] DatabaseName, SchemaName, TableName, ColumnName, RefSchemaName, RefTableName and RefColumnName are required.', 16, 1);
        RETURN;
    END

    IF @OnDelete NOT IN (N'NO ACTION', N'CASCADE', N'SET NULL', N'SET DEFAULT')
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] Invalid OnDelete value. Allowed: NO ACTION, CASCADE, SET NULL, SET DEFAULT.', 16, 1);
        RETURN;
    END

    IF @OnUpdate NOT IN (N'NO ACTION', N'CASCADE', N'SET NULL', N'SET DEFAULT')
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] Invalid OnUpdate value. Allowed: NO ACTION, CASCADE, SET NULL, SET DEFAULT.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    -- Check table exists
    SET @SQL = N'SELECT @E = 1 FROM ' + QUOTENAME(@DatabaseName) + N'.sys.tables t
                 INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
                 WHERE s.name = @SchemaName AND t.name = @TableName';
    EXEC sp_executesql @SQL, N'@SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @E BIT OUTPUT',
                       @SchemaName, @TableName, @FKExists OUTPUT;

    IF @FKExists = 0
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] Table [%s].[%s] does not exist in database [%s].', 16, 1, @SchemaName, @TableName, @DatabaseName);
        RETURN;
    END

    -- Check referenced table exists
    SET @FKExists = 0;
    SET @SQL = N'SELECT @E = 1 FROM ' + QUOTENAME(@DatabaseName) + N'.sys.tables t
                 INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
                 WHERE s.name = @RefSchemaName AND t.name = @RefTableName';
    EXEC sp_executesql @SQL, N'@RefSchemaName NVARCHAR(128), @RefTableName NVARCHAR(128), @E BIT OUTPUT',
                       @RefSchemaName, @RefTableName, @FKExists OUTPUT;

    IF @FKExists = 0
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] Referenced table [%s].[%s] does not exist in database [%s].', 16, 1, @RefSchemaName, @RefTableName, @DatabaseName);
        RETURN;
    END

    -- Build standard FK name
    SET @FKName = N'FK_' + @SchemaName + N'_' + @TableName + N'_' + @ColumnName + N'_2_' + @RefSchemaName + N'_' + @RefTableName + N'_' + @RefColumnName;

    -- Check FK already exists
    SET @FKExists = 0;
    SET @FullTableName = QUOTENAME(@DatabaseName) + N'.' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName);
    SET @SQL = N'SELECT @E = 1 FROM ' + QUOTENAME(@DatabaseName) + N'.sys.foreign_keys
                 WHERE name = @FKName
                   AND parent_object_id = OBJECT_ID(@FullTableName)';
    EXEC sp_executesql @SQL, N'@FKName NVARCHAR(300), @FullTableName NVARCHAR(400), @E BIT OUTPUT',
                       @FKName, @FullTableName, @FKExists OUTPUT;

    IF @FKExists = 1
    BEGIN
        RAISERROR(N'[uspCreateForeignKey] Foreign key [%s] already exists.', 16, 1, @FKName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- CREATE FOREIGN KEY
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                 ADD CONSTRAINT ' + QUOTENAME(@FKName) + N'
                 FOREIGN KEY (' + QUOTENAME(@ColumnName) + N')
                 REFERENCES ' + QUOTENAME(@RefSchemaName) + N'.' + QUOTENAME(@RefTableName) + N' (' + QUOTENAME(@RefColumnName) + N')
                 ON DELETE ' + @OnDelete + N'
                 ON UPDATE ' + @OnUpdate;

    IF @IsNotForReplication = 1
        SET @SQL = @SQL + N' NOT FOR REPLICATION';

    SET @SQL = @SQL + N';';
    EXEC sp_executesql @SQL;
    PRINT N'[uspCreateForeignKey] ✓ FK created: [' + @FKName + N']';

    -- ═══════════════════════════════════════════════════════════════════
    -- ENABLE OR DISABLE FK
    -- ═══════════════════════════════════════════════════════════════════
    IF @Enabled = 0
    BEGIN
        SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                     ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                     NOCHECK CONSTRAINT ' + QUOTENAME(@FKName) + N';';
        EXEC sp_executesql @SQL;
        PRINT N'[uspCreateForeignKey] FK disabled.';
    END
    ELSE
    BEGIN
        SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                     ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                     WITH CHECK CHECK CONSTRAINT ' + QUOTENAME(@FKName) + N';';
        EXEC sp_executesql @SQL;
        PRINT N'[uspCreateForeignKey] FK enabled and validated.';
    END

END
GO