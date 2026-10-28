
IF DB_ID(N'LoginDB') IS NULL
    CREATE DATABASE LoginDB;
GO

USE LoginDB;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(256) NOT NULL,
        PasswordSalt NVARCHAR(256) NOT NULL
    );
END;
GO
