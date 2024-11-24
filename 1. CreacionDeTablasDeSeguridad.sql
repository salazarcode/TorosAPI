CREATE TABLE Identifiers (
    ID INT IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(512) NOT NULL,
    PasswordSalt NVARCHAR(512) NOT NULL,
    PrimaryGroupID INT,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_Identifiers PRIMARY KEY (ID),
    CONSTRAINT UQ_Identifiers_Username UNIQUE (Username),
    CONSTRAINT UQ_Identifiers_Email UNIQUE (Email)
);

CREATE TABLE Groups (
    ID INT IDENTITY(1,1),
	UniqueKey nvarchar(100) not null,
    Description NVARCHAR(300),
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_Groups PRIMARY KEY (ID),
    CONSTRAINT UQ_Groups_UniqueKey UNIQUE (UniqueKey)
);

CREATE TABLE IdentifierGroup (
    IdentifierID INT NOT NULL,
    GroupID INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CreatedBy INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT PK_IdentifierGroup PRIMARY KEY (IdentifierID, GroupID)
);

-- 2. Agregar los foreign keys 
ALTER TABLE Identifiers
ADD CONSTRAINT FK_Identifiers_PrimaryGroupID
    FOREIGN KEY (PrimaryGroupID) REFERENCES Groups(ID);

ALTER TABLE Identifiers
ADD CONSTRAINT FK_Identifiers_CreatedBy
    FOREIGN KEY (CreatedBy) REFERENCES Identifiers(ID);

ALTER TABLE Groups
ADD CONSTRAINT FK_Groups_CreatedBy
    FOREIGN KEY (CreatedBy) REFERENCES Identifiers(ID);

ALTER TABLE IdentifierGroup
ADD CONSTRAINT FK_IdentifierGroup_IdentifierID
    FOREIGN KEY (IdentifierID) REFERENCES Identifiers(ID);

ALTER TABLE IdentifierGroup
ADD CONSTRAINT FK_IdentifierGroup_GroupID
    FOREIGN KEY (GroupID) REFERENCES Groups(ID);

ALTER TABLE IdentifierGroup
ADD CONSTRAINT FK_IdentifierGroup_CreatedBy
    FOREIGN KEY (CreatedBy) REFERENCES Identifiers(ID);