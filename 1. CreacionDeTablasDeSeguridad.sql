---- Eliminar FKs de GroupUser
--ALTER TABLE GroupUser
--DROP CONSTRAINT IF EXISTS FK_GroupUser_UserID;

--ALTER TABLE GroupUser
--DROP CONSTRAINT IF EXISTS FK_GroupUser_GroupID;

--ALTER TABLE GroupUser
--DROP CONSTRAINT IF EXISTS FK_GroupUser_CreatedBy;

---- Eliminar FKs de Identifiers
--ALTER TABLE Users
--DROP CONSTRAINT IF EXISTS FK_Users_PrimaryGroupID;
							 
--ALTER TABLE Users		 
--DROP CONSTRAINT IF EXISTS FK_Users_CreatedBy;

---- Eliminar FKs de Groups
--ALTER TABLE Groups
--DROP CONSTRAINT IF EXISTS FK_Groups_CreatedBy;

---- Ahora puedes eliminar las tablas
--DROP TABLE IF EXISTS GroupUser;
--DROP TABLE IF EXISTS Users;
--DROP TABLE IF EXISTS Groups;

-- 1. Crear las tablas sin foreign keys
CREATE TABLE Users (
    ID INT IDENTITY(1,1),
    PublicId UNIQUEIDENTIFIER DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(512) NOT NULL,
    PasswordSalt NVARCHAR(512) NOT NULL,
    PrimaryGroupID INT,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_Users PRIMARY KEY (ID),
    CONSTRAINT UQ_Users_PublicId UNIQUE (PublicId),
    CONSTRAINT UQ_Users_Username UNIQUE (Username),
    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);

CREATE TABLE Groups (
    ID INT IDENTITY(1,1),
    PublicId UNIQUEIDENTIFIER DEFAULT NEWID(),
	UniqueKey nvarchar(100) not null,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(300),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_Groups PRIMARY KEY (ID),
    CONSTRAINT UQ_Groups_UniqueKey UNIQUE (UniqueKey),
    CONSTRAINT UQ_Groups_PublicId UNIQUE (PublicId)
);


CREATE TABLE GroupUser (
    UserID INT NOT NULL,
    GroupID INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_GroupUser PRIMARY KEY (UserID, GroupID)
);

-- 2. Agregar los foreign keys después
ALTER TABLE Users
ADD CONSTRAINT FK_Users_PrimaryGroupID 
    FOREIGN KEY (PrimaryGroupID) REFERENCES Groups(ID);

ALTER TABLE Users
ADD CONSTRAINT FK_Users_CreatedBy 
    FOREIGN KEY (CreatedBy) REFERENCES Users(ID);

ALTER TABLE Groups
ADD CONSTRAINT FK_Groups_CreatedBy 
    FOREIGN KEY (CreatedBy) REFERENCES Users(ID);

ALTER TABLE GroupUser
ADD CONSTRAINT FK_GroupUser_UserID 
    FOREIGN KEY (UserID) REFERENCES Users(ID);

ALTER TABLE GroupUser
ADD CONSTRAINT FK_GroupUser_GroupID 
    FOREIGN KEY (GroupID) REFERENCES Groups(ID);

ALTER TABLE GroupUser
ADD CONSTRAINT FK_GroupUser_CreatedBy 
    FOREIGN KEY (CreatedBy) REFERENCES Users(ID);


	
