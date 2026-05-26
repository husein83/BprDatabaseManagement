USE master;

IF OBJECT_ID(N'uspRestoreDatabase', N'P') IS NOT NULL
    DROP PROCEDURE uspRestoreDatabase;
GO

USE master;
GO

CREATE PROCEDURE uspRestoreDatabase
    @DatabaseName   NVARCHAR(128),
    @RestorePath    NVARCHAR(500)
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

        -- SECTION 1: VALIDATE INPUTS
        IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL
        BEGIN
            RAISERROR(N'[uspRestoreDatabase] @DatabaseName is required.', 16, 1);
            RETURN;
        END

        IF NULLIF(LTRIM(RTRIM(@RestorePath)), N'') IS NULL
        BEGIN
            RAISERROR(N'[uspRestoreDatabase] @RestorePath is required.', 16, 1);
            RETURN;
        END

        DECLARE @SQL NVARCHAR(MAX);
        DECLARE @SafePath NVARCHAR(500) = REPLACE(@RestorePath, N'''', N'''''');

        -- SECTION 2: SET SINGLE_USER (only if DB exists)
        IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
        BEGIN
            SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) +
                       N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';
            EXEC sp_executesql @SQL;
        END

        -- SECTION 3: RESTORE
        SET @SQL = N'RESTORE DATABASE ' + QUOTENAME(@DatabaseName) +
                   N' FROM DISK = N''' + @SafePath +
                   N''' WITH REPLACE, RECOVERY, STATS = 10;';
        EXEC sp_executesql @SQL;

        -- SECTION 4: RETURN TO MULTI_USER
        SET @SQL = N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) +
                   N' SET MULTI_USER;';
        EXEC sp_executesql @SQL;

        PRINT N'[uspRestoreDatabase] SUCCESS: Restore completed from: ' + @RestorePath;

    END TRY
    BEGIN CATCH

        -- SAFETY NET: If restore failed, try to put DB back to MULTI_USER
        BEGIN TRY
            IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
            BEGIN
                DECLARE @RecoverSQL NVARCHAR(MAX) =
                    N'ALTER DATABASE ' + QUOTENAME(@DatabaseName) + N' SET MULTI_USER;';
                EXEC sp_executesql @RecoverSQL;
            END
        END TRY
        BEGIN CATCH
            -- If even this fails, just log and continue to main error
            PRINT N'[uspRestoreDatabase] WARNING: Could not restore MULTI_USER mode on ['
                + @DatabaseName + N']. Manual intervention may be required.';
        END CATCH

        RAISERROR(
            N'[uspRestoreDatabase] Restore failed for database [%s]. Error: %s',
            16, 1,
            @DatabaseName
        );

    END CATCH
END
GO
