USE master;
GO

IF OBJECT_ID(N'uspDropForeignKey', N'P') IS NOT NULL
    DROP PROCEDURE uspDropForeignKey;
GO

CREATE PROCEDURE uspDropForeignKey
    @DatabaseName      NVARCHAR(128),
    @SchemaName        NVARCHAR(128),
    @TableName         NVARCHAR(128),
    @ColumnName        NVARCHAR(128)   = NULL,
    @RefSchemaName     NVARCHAR(128)   = NULL,
    @RefTableName      NVARCHAR(128)   = NULL,
    @RefColumnName     NVARCHAR(128)   = NULL,
    @FKName            NVARCHAR(300)   = NULL,
    @IgnoreIfNotExists BIT             = 1
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL             NVARCHAR(MAX);
    DECLARE @Exists          BIT = 0;
    DECLARE @ResolvedFKName  NVARCHAR(300);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspDropForeignKey] DatabaseName, SchemaName and TableName are required.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspDropForeignKey] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- RESOLVE FK NAME
    -- ═══════════════════════════════════════════════════════════════════
    IF @FKName IS NOT NULL
    BEGIN
        SET @ResolvedFKName = @FKName;
    END
    ELSE IF @ColumnName IS NOT NULL AND @RefSchemaName IS NOT NULL AND @RefTableName IS NOT NULL AND @RefColumnName IS NOT NULL
    BEGIN
        SET @ResolvedFKName = N'FK_' + @SchemaName + N'_' + @TableName + N'_' + @ColumnName + N'_2_' + @RefSchemaName + N'_' + @RefTableName + N'_' + @RefColumnName;
    END
    ELSE
    BEGIN
        RAISERROR(N'[uspDropForeignKey] Provide either @FKName or the combination of @ColumnName, @RefSchemaName, @RefTableName and @RefColumnName.', 16, 1);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- CHECK FK EXISTS
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'SELECT @E = 1 FROM ' + QUOTENAME(@DatabaseName) + N'.sys.foreign_keys fk
                 INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.tables t ON fk.parent_object_id = t.object_id
                 INNER JOIN ' + QUOTENAME(@DatabaseName) + N'.sys.schemas s ON t.schema_id = s.schema_id
                 WHERE fk.name = @FKName
                   AND s.name = @SchemaName
                   AND t.name = @TableName';
    
    EXEC sp_executesql @SQL, 
        N'@FKName NVARCHAR(300), @SchemaName NVARCHAR(128), @TableName NVARCHAR(128), @E BIT OUTPUT',
        @ResolvedFKName, @SchemaName, @TableName, @Exists OUTPUT;

    IF @Exists = 0 OR @Exists IS NULL
    BEGIN
        IF @IgnoreIfNotExists = 1
        BEGIN
            PRINT N'[uspDropForeignKey] FK [' + @ResolvedFKName + N'] does not exist. Skipping.';
            RETURN;
        END
        ELSE
        BEGIN
            RAISERROR(N'[uspDropForeignKey] Foreign key [%s] was not found on table [%s].[%s].', 16, 1, @ResolvedFKName, @SchemaName, @TableName);
            RETURN;
        END
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- DROP FK
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'USE ' + QUOTENAME(@DatabaseName) + N';
                 ALTER TABLE ' + QUOTENAME(@SchemaName) + N'.' + QUOTENAME(@TableName) + N'
                 DROP CONSTRAINT ' + QUOTENAME(@ResolvedFKName) + N';';
    EXEC sp_executesql @SQL;
    PRINT N'[uspDropForeignKey] ✓ FK dropped: [' + @ResolvedFKName + N']';

END
GO