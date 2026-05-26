USE master;
GO

IF OBJECT_ID(N'uspGetDatabaseInfo', N'P') IS NOT NULL
    DROP PROCEDURE uspGetDatabaseInfo;
GO

CREATE OR ALTER PROCEDURE uspGetDatabaseInfo
    @DatabaseName NVARCHAR(128) = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.name                                AS DatabaseName,
        d.database_id                         AS DatabaseID,
        d.collation_name                      AS Collation,
        d.compatibility_level                 AS CompatibilityLevel,
        d.recovery_model_desc                 AS RecoveryModel,
        d.page_verify_option_desc             AS PageVerify,
        d.is_read_only                        AS IsReadOnly,
        d.is_auto_shrink_on                   AS AutoShrink,
        d.is_auto_close_on                    AS AutoClose,
        d.state_desc                          AS [State],
        d.create_date                         AS CreateDate,
        mf_data.physical_name                 AS DataFilePath,
        mf_data.size * 8 / 1024               AS DataFileSizeMB,
        CASE 
            WHEN mf_data.max_size = -1 THEN N'UNLIMITED'
            ELSE CAST(mf_data.max_size * 8 / 1024 AS NVARCHAR(20)) + N' MB'
        END                                   AS DataFileMaxSize,
        mf_data.growth * 8 / 1024             AS DataFileGrowthMB,
        mf_log.physical_name                  AS LogFilePath,
        mf_log.size * 8 / 1024                AS LogFileSizeMB,
        CASE 
            WHEN mf_log.max_size = -1 THEN N'UNLIMITED'
            ELSE CAST(mf_log.max_size / 128 AS NVARCHAR(20)) + N' MB'
        END                                   AS LogFileMaxSize,
        mf_log.growth * 8 / 1024              AS LogFileGrowthMB
    FROM sys.databases d
    LEFT JOIN sys.master_files mf_data ON d.database_id = mf_data.database_id AND mf_data.type = 0 AND mf_data.file_id = 1
    LEFT JOIN sys.master_files mf_log  ON d.database_id = mf_log.database_id  AND mf_log.type = 1  AND mf_log.file_id = 2
    WHERE (@DatabaseName IS NULL OR d.name = @DatabaseName)
    ORDER BY d.name;
END
GO