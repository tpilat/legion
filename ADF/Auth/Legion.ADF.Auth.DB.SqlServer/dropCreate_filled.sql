BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = 'legion_adf_auth';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [legion_adf_auth];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE master;
GO

DROP DATABASE IF EXISTS legion_adf_auth;
GO

CREATE DATABASE legion_adf_auth
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::legion_adf_auth TO sa;
GO

DROP USER IF EXISTS authusr;
GO

IF EXISTS(
	SELECT name 
	FROM [master].[sys].[syslogins]
	WHERE NAME = 'authusr')

BEGIN 
	DROP LOGIN authusr;
END
GO

CREATE LOGIN authusr
WITH PASSWORD = 'auth_Pwd1.';
GO

USE legion_adf_auth;
GO

CREATE USER authusr FOR LOGIN authusr;
GO
