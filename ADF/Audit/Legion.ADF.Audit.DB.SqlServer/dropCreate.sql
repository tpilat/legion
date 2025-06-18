BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = '#TargetDatabase#';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [#TargetDatabase#];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE #CurrentDatabase#;
GO

DROP DATABASE IF EXISTS #TargetDatabase#;
GO

CREATE DATABASE #TargetDatabase#
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::#TargetDatabase# TO #AdminUser#;
GO

DROP USER IF EXISTS #TargetDbUsername#;
GO

IF EXISTS(
	SELECT name 
	FROM [#CurrentDatabase#].[sys].[syslogins]
	WHERE NAME = '#TargetDbUsername#')

BEGIN 
	DROP LOGIN #TargetDbUsername#;
END
GO

CREATE LOGIN #TargetDbUsername#
WITH PASSWORD = '#TargetDbPassword#';
GO

USE #TargetDatabase#;
GO

CREATE USER #TargetDbUsername# FOR LOGIN #TargetDbUsername#;
GO
