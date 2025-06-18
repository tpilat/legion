BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = 'legion_adf_logs';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [legion_adf_logs];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE master;
GO

DROP DATABASE IF EXISTS legion_adf_logs;
GO

CREATE DATABASE legion_adf_logs
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::legion_adf_logs TO sa;
GO

DROP USER IF EXISTS logsusr;
GO

IF EXISTS(
	SELECT name 
	FROM [master].[sys].[syslogins]
	WHERE NAME = 'logsusr')

BEGIN 
	DROP LOGIN logsusr;
END
GO

CREATE LOGIN logsusr
WITH PASSWORD = 'logs_Pwd1.';
GO

USE legion_adf_logs;
GO

CREATE USER logsusr FOR LOGIN logsusr;
GO
