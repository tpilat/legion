BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = 'legion_adf_audit';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [legion_adf_audit];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE master;
GO

DROP DATABASE IF EXISTS legion_adf_audit;
GO

CREATE DATABASE legion_adf_audit
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::legion_adf_audit TO sa;
GO

DROP USER IF EXISTS auditusr;
GO

IF EXISTS(
	SELECT name 
	FROM [master].[sys].[syslogins]
	WHERE NAME = 'auditusr')

BEGIN 
	DROP LOGIN auditusr;
END
GO

CREATE LOGIN auditusr
WITH PASSWORD = 'audit_Pwd1.';
GO

USE legion_adf_audit;
GO

CREATE USER auditusr FOR LOGIN auditusr;
GO
