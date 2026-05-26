USE master;

IF OBJECT_ID(N'uspDropDatabase', N'P') IS NOT NULL
    DROP PROCEDURE uspDropDatabase;
GO

CREATE PROCEDURE uspDropDatabase
    @DatabaseName      NVARCHAR(128),
    @IgnoreIfNotExists BIT = 1,
    @ForceDisconnect   BIT = 1
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
        RAISERROR(N'[uspDropDatabase] Database name cannot be empty.', 16, 1);
        RETURN;
    END

    -- Prevent dropping system databases
    IF @DatabaseName IN (N'master', N'model', N'msdb', N'tempdb')
    BEGIN
        RAISERROR(N'[uspDropDatabase] Dropping system databases is not allowed.', 16, 1);
        RETURN;
    END

    -- Check database exists
    IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
    BEGIN
        IF @IgnoreIfNotExists = 1
        BEGIN
            PRINT N'[uspDropDatabase] Database [' + @DatabaseName + N'] does not exist. Skipping.';
            RETURN;
        END
        ELSE
        BEGIN
            RAISERROR(N'[uspDropDatabase] Database [%s] does not exist.', 16, 1, @DatabaseName);
            RETURN;
        END
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- FORCE DISCONNECT
    -- ═══════════════════════════════════════════════════════════════════
    IF @ForceDisconnect = 1
    BEGIN
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';
        EXEC sp_executesql @SQL;
        PRINT N'[uspDropDatabase] All active connections to [' + @DatabaseName + N'] terminated.';
    END

    -- ═══════════════════════════════════════════════════════════════════
    -- DROP DATABASE
    -- ═══════════════════════════════════════════════════════════════════
    SET @SQL = N'DROP DATABASE ' + QUOTENAME(@DatabaseName) + N';';
    EXEC sp_executesql @SQL;
    PRINT N'[uspDropDatabase] ✓ Database [' + @DatabaseName + N'] dropped successfully.';

END
GO