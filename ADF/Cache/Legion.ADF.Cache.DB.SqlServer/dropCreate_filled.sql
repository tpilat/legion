BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = 'legion_adf_cache';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [legion_adf_cache];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE master;
GO

DROP DATABASE IF EXISTS legion_adf_cache;
GO

CREATE DATABASE legion_adf_cache
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::legion_adf_cache TO sa;
GO

DROP USER IF EXISTS cacheusr;
GO

IF EXISTS(
	SELECT name 
	FROM [master].[sys].[syslogins]
	WHERE NAME = 'cacheusr')

BEGIN 
	DROP LOGIN cacheusr;
END
GO

CREATE LOGIN cacheusr
WITH PASSWORD = 'cache_Pwd1.';
GO

USE legion_adf_cache;
GO

CREATE USER cacheusr FOR LOGIN cacheusr;
GO
