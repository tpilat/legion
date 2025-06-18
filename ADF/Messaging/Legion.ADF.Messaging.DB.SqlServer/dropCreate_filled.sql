BEGIN
	DECLARE @DatabaseName NVARCHAR(128) = 'legion_adf_msg';
	DECLARE @SQL NVARCHAR(MAX);
	DECLARE @SQL2 NVARCHAR(MAX);

	IF EXISTS (SELECT 1 FROM sys.databases WHERE name = @DatabaseName)
	BEGIN	

		SET @SQL = 'USE [legion_adf_msg];';

		EXEC sp_executesql @SQL;

		SET @SQL = 'ALTER DATABASE [' + @DatabaseName + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;';

		EXEC sp_executesql @SQL;
	END
END
GO

USE master;
GO

DROP DATABASE IF EXISTS legion_adf_msg;
GO

CREATE DATABASE legion_adf_msg
COLLATE Slovak_100_CS_AS_KS_WS_SC_UTF8;
GO

ALTER AUTHORIZATION ON DATABASE::legion_adf_msg TO sa;
GO

DROP USER IF EXISTS msgusr;
GO

IF EXISTS(
	SELECT name 
	FROM [master].[sys].[syslogins]
	WHERE NAME = 'msgusr')

BEGIN 
	DROP LOGIN msgusr;
END
GO

CREATE LOGIN msgusr
WITH PASSWORD = 'msg_Pwd1.';
GO

USE legion_adf_msg;
GO

CREATE USER msgusr FOR LOGIN msgusr;
GO
