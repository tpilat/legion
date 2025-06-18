BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = 'legion_adf_conf';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [legion_adf_conf];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE master;
GO

DROP DATABASE IF EXISTS legion_adf_conf;
GO

CREATE DATABASE legion_adf_conf
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::legion_adf_conf TO sa;
GO

DROP USER IF EXISTS confusr;
GO

IF EXISTS(
	SELECT name 
	FROM [master].[sys].[syslogins]
	WHERE NAME = 'confusr')

BEGIN 
	DROP LOGIN confusr;
END
GO

CREATE LOGIN confusr
WITH PASSWORD = 'conf_Pwd1.';
GO

USE legion_adf_conf;
GO

CREATE USER confusr FOR LOGIN confusr;
GO
