USE master;

IF OBJECT_ID('uspBackupDatabase', 'P') IS NOT NULL
    DROP PROCEDURE uspBackupDatabase;
GO

CREATE PROCEDURE uspBackupDatabase
    @DatabaseName   NVARCHAR(128),
    @BackupPath     NVARCHAR(500)
WITH ENCRYPTION
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

        -- Validate inputs
        IF NULLIF(LTRIM(RTRIM(@DatabaseName)), N'') IS NULL
        BEGIN
            RAISERROR(N'[uspBackupDatabase] @DatabaseName is required.', 16, 1);
            RETURN;
        END

        IF NULLIF(LTRIM(RTRIM(@BackupPath)), N'') IS NULL
        BEGIN
            RAISERROR(N'[uspBackupDatabase] @BackupPath is required.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
        BEGIN
            RAISERROR(N'[uspBackupDatabase] Database [%s] does not exist.', 16, 1, @DatabaseName);
            RETURN;
        END

        DECLARE @BackupFile NVARCHAR(1000);
        DECLARE @BackupName NVARCHAR(300);
        DECLARE @DateTime   NVARCHAR(50) = FORMAT(GETUTCDATE(), N'dd-MMM-yyyy__HH-mm-ss');
        DECLARE @SQL        NVARCHAR(MAX);

        -- Ensure path ends with backslash
        SET @BackupPath = RTRIM(@BackupPath);
        IF RIGHT(@BackupPath, 1) <> N'\'
            SET @BackupPath = @BackupPath + N'\';

        -- Construct full backup file path
        SET @BackupName = @DatabaseName + N'__Backup__' + @DateTime + N'.bak';
        SET @BackupFile = @BackupPath + @BackupName;

        -- Build and execute backup command (escape single quotes in path)
        SET @SQL =
            N'BACKUP DATABASE ' + QUOTENAME(@DatabaseName) +
            N' TO DISK = N''' + REPLACE(@BackupFile, N'''', N'''''') +
            N''' WITH INIT, NAME = N''Full Backup'', STATS = 10;';

        EXEC sp_executesql @SQL;

        PRINT N'[uspBackupDatabase] SUCCESS: Backup completed at: ' + @BackupFile;

    END TRY
    BEGIN CATCH
        RAISERROR(
            N'[uspBackupDatabase] Backup failed for database [%s]. Error: %s',
            16, 1,
            @DatabaseName
        );
    END CATCH
END
GO