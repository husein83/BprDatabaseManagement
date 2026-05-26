USE master;
GO

IF OBJECT_ID(N'uspAlterDatabase', N'P') IS NOT NULL
    DROP PROCEDURE uspAlterDatabase;
GO

CREATE PROCEDURE uspAlterDatabase
    @DatabaseName       NVARCHAR(128),
    @Collation          NVARCHAR(128)    = NULL,
    @RecoveryModel      NVARCHAR(20)     = NULL,
    @CompatibilityLevel TINYINT          = NULL,
    @IsReadOnly         BIT              = NULL,
    @AutoShrink         BIT              = NULL,
    @AutoClose          BIT              = NULL,
    @PageVerify         NVARCHAR(20)     = NULL
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @SQL NVARCHAR(MAX);

    -- ═══════════════════════════════════════════════════════════════════
    -- VALIDATE
    -- ═══════════════════════════════════════════════════════════════════
    IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL
    BEGIN
        RAISERROR(N'[uspAlterDatabase] Database name cannot be empty.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        RAISERROR(N'[uspAlterDatabase] Database [%s] does not exist.', 16, 1, @DatabaseName);
        RETURN;
    END

    IF @RecoveryModel IS NOT NULL AND @RecoveryModel NOT IN (N'SIMPLE', N'FULL', N'BULK_LOGGED')
    BEGIN
        RAISERROR(N'[uspAlterDatabase] RecoveryModel must be SIMPLE, FULL, or BULK_LOGGED.', 16, 1);
        RETURN;
    END

    IF @PageVerify IS NOT NULL AND @PageVerify NOT IN (N'CHECKSUM', N'TORN_PAGE_DETECTION', N'NONE')
    BEGIN
        RAISERROR(N'[uspAlterDatabase] PageVerify must be CHECKSUM, TORN_PAGE_DETECTION, or NONE.', 16, 1);
        RETURN;
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- APPLY SETTINGS
    -- ═══════════════════════════════════════════════════════════════════
    -- Collation
    IF @Collation IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' COLLATE ' + @Collation;
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Collation updated.';
    END

    -- Recovery Model
    IF @RecoveryModel IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET RECOVERY ' + @RecoveryModel + N' WITH NO_WAIT';
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Recovery model updated.';
    END

    -- Read Only / Read Write
    IF @IsReadOnly IS NOT NULL
    BEGIN
        IF @IsReadOnly = 1
            SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET READ_ONLY WITH NO_WAIT';
        ELSE
            SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET READ_WRITE WITH NO_WAIT';
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Read-only mode updated.';
    END

    -- Auto Shrink
    IF @AutoShrink IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET AUTO_SHRINK ' + CASE WHEN @AutoShrink = 1 THEN N'ON' ELSE N'OFF' END;
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Auto-shrink updated.';
    END

    -- Auto Close
    IF @AutoClose IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET AUTO_CLOSE ' + CASE WHEN @AutoClose = 1 THEN N'ON' ELSE N'OFF' END;
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Auto-close updated.';
    END

    -- Page Verify
    IF @PageVerify IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET PAGE_VERIFY ' + @PageVerify + N' WITH NO_WAIT';
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Page verify updated.';
    END

    -- Compatibility Level
    IF @CompatibilityLevel IS NOT NULL
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET COMPATIBILITY_LEVEL = ' + CAST(@CompatibilityLevel AS NVARCHAR(10));
        EXEC sp_executesql @SQL;
        PRINT N'[uspAlterDatabase] Compatibility level updated.';
    END

    PRINT N'[uspAlterDatabase] ✓ Database [' + @DatabaseName + N'] updated successfully.';
END
GO