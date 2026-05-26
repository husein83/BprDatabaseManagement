USE master;
GO

IF OBJECT_ID(N'uspCreateColumn', N'P') IS NOT NULL
    DROP PROCEDURE uspCreateColumn;
GO

CREATE PROCEDURE uspCreateColumn
    @DatabaseName       NVARCHAR(128),
    @SchemaName         NVARCHAR(128),
    @TableName          NVARCHAR(128),
    @ColumnName         NVARCHAR(128),
    @DataType           NVARCHAR(50),
    @Length             INT             = NULL,
    @Precision          INT             = NULL,
    @Scale              INT             = NULL,
    @IsNullable         BIT             = 1,
    @DefaultValue       NVARCHAR(500)   = NULL,
    @DefaultName        NVARCHAR(128)   = NULL,
    @IsIdentity         BIT             = 0,
    @IdentitySeed       INT             = 1,
    @IdentityIncrement  INT             = 1,
    @IsPrimaryKey       BIT             = 0,
    @PKName             NVARCHAR(128)   = NULL,
    @Collation          NVARCHAR(128)   = NULL,
    @ComputedFormula    NVARCHAR(500)   = NULL,
    @IsPersisted        BIT             = 0,
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

    -- Validation
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@SchemaName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@TableName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@ColumnName)), N'') IS NULL OR
       NULLIF(LTRIM(RTRIM(@DataType)), N'') IS NULL
    BEGIN
        RAISERROR(N'DatabaseName, SchemaName, TableName, ColumnName and DataType are required.', 16, 1);
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

    -- Check column already exists
    SET @SQL = N'
        SELECT @E = 1
        FROM   [' + @DatabaseName + N'].sys.columns c
        JOIN   [' + @DatabaseName + N'].sys.tables t ON c.object_id = t.object_id
        JOIN   [' + @DatabaseName + N'].sys.schemas s ON t.schema_id = s.schema_id
        WHERE  c.name = @ColName AND t.name = @TblName AND s.name = @SchName;
    ';
    EXEC sp_executesql @SQL,
        N'@ColName NVARCHAR(128), @TblName NVARCHAR(128), @SchName NVARCHAR(128), @E BIT OUTPUT',
        @ColumnName, @TableName, @SchemaName, @ColumnExists OUTPUT;

    IF @ColumnExists = 1
    BEGIN
        RAISERROR(N'Column [%s] already exists in table [%s].[%s].', 16, 1, @ColumnName, @SchemaName, @TableName);
        RETURN;
    END

    -- Build data type definition
    IF @ComputedFormula IS NOT NULL
    BEGIN
        SET @TypeDef = N'AS (' + @ComputedFormula + N')'
                     + CASE WHEN @IsPersisted = 1 THEN N' PERSISTED' ELSE N'' END;
    END
    ELSE
    BEGIN
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

        IF @IsIdentity = 1
            SET @TypeDef = @TypeDef + N' IDENTITY(' + CAST(@IdentitySeed AS NVARCHAR(10)) + N',' + CAST(@IdentityIncrement AS NVARCHAR(10)) + N')';

        SET @NullDef = CASE WHEN @IsNullable = 1 THEN N' NULL' ELSE N' NOT NULL' END;
        SET @TypeDef = @TypeDef + @NullDef;
    END

    -- Create column
    IF @ComputedFormula IS NOT NULL
    BEGIN
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            ALTER TABLE ' + @FullTableName + N' ADD ' + QUOTENAME(@ColumnName) + N' ' + @TypeDef + N';
        ';
    END
    ELSE
    BEGIN
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            ALTER TABLE ' + @FullTableName + N' ADD ' + QUOTENAME(@ColumnName) + N' ' + @TypeDef;

        IF @DefaultValue IS NOT NULL
        BEGIN
            IF @DefaultName IS NULL
                SET @DefaultName = N'DF_' + @SchemaName + N'_' + @TableName + N'_' + @ColumnName;
            SET @SQL = @SQL + N' CONSTRAINT ' + QUOTENAME(@DefaultName) + N' DEFAULT (' + @DefaultValue + N')';
        END
        SET @SQL = @SQL + N';';
    END

    EXEC sp_executesql @SQL;
    PRINT N'Column [' + @ColumnName + N'] created successfully.';

    -- Add primary key if requested
    IF @IsPrimaryKey = 1
    BEGIN
        IF @PKName IS NULL
            SET @PKName = N'PK_' + @SchemaName + N'_' + @TableName + N'_' + @ColumnName;

        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            IF NOT EXISTS (
                SELECT 1 FROM sys.key_constraints
                WHERE parent_object_id = OBJECT_ID(' + QUOTENAME(@FullTableName, '''') + N')
                  AND type = ''PK''
            )
            BEGIN
                ALTER TABLE ' + @FullTableName + N'
                ADD CONSTRAINT ' + QUOTENAME(@PKName) + N' PRIMARY KEY (' + QUOTENAME(@ColumnName) + N');
                PRINT ''Primary key [' + @PKName + N'] created.'';
            END
            ELSE
                PRINT ''INFO: Table already has a primary key.'';
        ';
        EXEC sp_executesql @SQL;
    END

    -- Add description
    IF @Description IS NOT NULL
    BEGIN
        SET @SQL = N'
            USE [' + @DatabaseName + N'];
            EXEC sys.sp_addextendedproperty
                @name  = N''MS_Description'',
                @value = ' + QUOTENAME(@Description, '''') + N',
                @level0type = N''SCHEMA'', @level0name = ' + QUOTENAME(@SchemaName, '''') + N',
                @level1type = N''TABLE'',  @level1name = ' + QUOTENAME(@TableName, '''') + N',
                @level2type = N''COLUMN'', @level2name = ' + QUOTENAME(@ColumnName, '''') + N';
        ';
        EXEC sp_executesql @SQL;
        PRINT N'Column description saved.';
    END
END
GO