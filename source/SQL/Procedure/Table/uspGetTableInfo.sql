USE master;
GO

IF OBJECT_ID(N'uspGetTableInfo', N'P') IS NOT NULL
    DROP PROCEDURE uspGetTableInfo;
GO

CREATE PROCEDURE uspGetTableInfo
    @DatabaseName   NVARCHAR(128),
    @SchemaName     NVARCHAR(128),
    @TableName      NVARCHAR(128)
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);

    -- Validations
    IF @DatabaseName IS NULL OR LTRIM(RTRIM(@DatabaseName)) = ''
    BEGIN RAISERROR('DatabaseName is required.', 16, 1); RETURN; END

    IF @SchemaName IS NULL OR LTRIM(RTRIM(@SchemaName)) = ''
    BEGIN RAISERROR('SchemaName is required.', 16, 1); RETURN; END

    IF @TableName IS NULL OR LTRIM(RTRIM(@TableName)) = ''
    BEGIN RAISERROR('TableName is required.', 16, 1); RETURN; END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN RAISERROR('The specified database does not exist.', 16, 1); RETURN; END

    -- Query table info
    SET @SQL = N'
        USE ' + QUOTENAME(@DatabaseName) + N';

        SELECT
            s.name                  AS SchemaName,
            t.name                  AS TableName,
            fg.name                 AS FileGroup,
            CAST(ep.value AS NVARCHAR(500)) AS Description,
            t.create_date           AS CreateDate,
            t.modify_date           AS ModifyDate
        FROM sys.tables t
        INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
        LEFT JOIN sys.filegroups fg ON t.lob_data_space_id = fg.data_space_id
        LEFT JOIN sys.extended_properties ep
            ON ep.major_id = t.object_id
           AND ep.minor_id = 0
           AND ep.name = N''MS_Description''
        WHERE s.name = ' + QUOTENAME(@SchemaName, '''') + N'
          AND t.name = ' + QUOTENAME(@TableName, '''') + N';';

    EXEC sp_executesql @SQL;
END
GO