USE master;
GO

IF OBJECT_ID(N'uspCreateDatabase', N'P') IS NOT NULL
    DROP PROCEDURE uspCreateDatabase;
GO

CREATE PROCEDURE uspCreateDatabase
    @DatabaseName       NVARCHAR(128),
    @DataFilePath       NVARCHAR(500),
    @LogFilePath        NVARCHAR(500)    = NULL,
    @InitialSizeMB      INT              = 8,
    @MaxSizeMB          INT              = NULL,
    @FileGrowthMB       INT              = 64,
    @LogInitialSizeMB   INT              = 8,
    @LogMaxSizeMB       INT              = NULL,
    @LogFileGrowthMB    INT              = 64,
    @Collation          NVARCHAR(128)    = NULL,
    @RecoveryModel      NVARCHAR(20)     = 'SIMPLE',
    @CompatibilityLevel TINYINT          = NULL,
    @IsReadOnly         BIT              = 0,
    @AutoShrink         BIT              = 0,
    @AutoClose          BIT              = 0,
    @PageVerify         NVARCHAR(20)     = 'CHECKSUM'
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL           NVARCHAR(MAX) = N'';
    DECLARE @MaxSizeStr    NVARCHAR(50);
    DECLARE @LogMaxSizeStr NVARCHAR(50);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspCreateDatabase] Database name cannot be empty.', 16, 1);
        RETURN;
    END

    IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspCreateDatabase] Database [%s] already exists.', 16, 1, @DatabaseName);
        RETURN;
    END

    IF NULLIF(LTRIM(RTRIM(@DataFilePath)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspCreateDatabase] DataFilePath cannot be empty.', 16, 1);
        RETURN;
    END

    IF @RecoveryModel NOT IN (N'SIMPLE', N'FULL', N'BULK_LOGGED')
    BEGIN
        RAISERROR(N'[uspCreateDatabase] RecoveryModel must be SIMPLE, FULL, or BULK_LOGGED.', 16, 1);
        RETURN;
    END

    IF @PageVerify NOT IN (N'CHECKSUM', N'TORN_PAGE_DETECTION', N'NONE')
    BEGIN
        RAISERROR(N'[uspCreateDatabase] PageVerify must be CHECKSUM, TORN_PAGE_DETECTION, or NONE.', 16, 1);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- PREPARE FILE PATHS
    -- ═══════════════════════════════════════════════════════════════════
    IF @LogFilePath IS NULL
        SET @LogFilePath = @DataFilePath;

    SET @DataFilePath = RTRIM(@DataFilePath);
    SET @LogFilePath  = RTRIM(@LogFilePath);

    IF RIGHT(@DataFilePath, 1) = N'\'
        SET @DataFilePath = LEFT(@DataFilePath, LEN(@DataFilePath) - 1);

    IF RIGHT(@LogFilePath, 1) = N'\'
        SET @LogFilePath = LEFT(@LogFilePath, LEN(@LogFilePath) - 1);

    SET @DataFilePath = @DataFilePath + N'\' + @DatabaseName + N'.mdf';
    SET @LogFilePath  = @LogFilePath  + N'\' + @DatabaseName + N'_log.ldf';

    SET @MaxSizeStr    = CASE WHEN @MaxSizeMB    IS NULL THEN N'UNLIMITED' ELSE CAST(@MaxSizeMB    AS NVARCHAR(20)) + N'MB' END;
    SET @LogMaxSizeStr = CASE WHEN @LogMaxSizeMB IS NULL THEN N'UNLIMITED' ELSE CAST(@LogMaxSizeMB AS NVARCHAR(20)) + N'MB' END;

    -- ═══════════════════════════════════════════════════════════════════
    -- CREATE DATABASE
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'
        CREATE DATABASE ' + QUOTENAME(@DatabaseName) + N'
        ON PRIMARY
        (
            NAME       = ' + QUOTENAME(@DatabaseName, '''') + N',
            FILENAME   = ' + QUOTENAME(@DataFilePath, '''') + N',
            SIZE       = ' + CAST(@InitialSizeMB AS NVARCHAR(20)) + N'MB,
            MAXSIZE    = ' + @MaxSizeStr + N',
            FILEGROWTH = ' + CAST(@FileGrowthMB AS NVARCHAR(20)) + N'MB
        )
        LOG ON
        (
            NAME       = ' + QUOTENAME(@DatabaseName + N'_log', '''') + N',
            FILENAME   = ' + QUOTENAME(@LogFilePath, '''') + N',
            SIZE       = ' + CAST(@LogInitialSizeMB AS NVARCHAR(20)) + N'MB,
            MAXSIZE    = ' + @LogMaxSizeStr + N',
            FILEGROWTH = ' + CAST(@LogFileGrowthMB AS NVARCHAR(20)) + N'MB
        )';

    IF @Collation IS NOT NULL
        SET @SQL = @SQL + N' COLLATE ' + @Collation;

    EXEC sp_executesql @SQL;

    -- ═══════════════════════════════════════════════════════════════════
    -- APPLY SETTINGS
    -- ═══════════════════════════════════════════════════════════════════
    -- Recovery Model
    SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET RECOVERY ' + @RecoveryModel + N' WITH NO_WAIT';
    EXEC sp_executesql @SQL;

    -- Read Only / Read Write
    IF @IsReadOnly = 1
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET READ_ONLY WITH NO_WAIT';
    ELSE
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET READ_WRITE WITH NO_WAIT';
    EXEC sp_executesql @SQL;

    -- Auto Shrink
    SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET AUTO_SHRINK ' + CASE WHEN @AutoShrink = 1 THEN N'ON' ELSE N'OFF' END;
    EXEC sp_executesql @SQL;

    -- Auto Close
    SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET AUTO_CLOSE ' + CASE WHEN @AutoClose = 1 THEN N'ON' ELSE N'OFF' END;
    EXEC sp_executesql @SQL;

    -- Page Verify
    SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET PAGE_VERIFY ' + @PageVerify + N' WITH NO_WAIT';
    EXEC sp_executesql @SQL;

    -- Compatibility Level
    IF @CompatibilityLevel IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET COMPATIBILITY_LEVEL = ' + CAST(@CompatibilityLevel AS NVARCHAR(10));
        EXEC sp_executesql @SQL;
    END

    PRINT N'[uspCreateDatabase] ✓ Database [' + @DatabaseName + N'] created successfully.';
END
GO