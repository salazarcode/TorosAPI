-- 3. Insertar datos iniciales
INSERT INTO Groups (Name, UniqueKey, Description, CreatedBy)
VALUES ('sudo', 'sudo', 'System Administrators', NULL);

INSERT INTO Users (
    Username, 
    Email, 
    PasswordHash, 
    PasswordSalt, 
    PrimaryGroupID,
    CreatedBy
)
VALUES (
    'root',
    'salazarcode@gmail.com',
    'CHANGE_ME_HASH',
    'CHANGE_ME_SALT',
    (SELECT ID FROM Groups WHERE Name = 'Administrators'),
    NULL
);

-- 4. Actualizar el CreatedBy del grupo Administrators
UPDATE Groups 
SET CreatedBy = (SELECT ID FROM Users WHERE Username = 'root')
WHERE Name = 'Administrators';

-- 5. Actualizar el CreatedBy del admin
UPDATE Users
SET CreatedBy = ID
WHERE Username = 'root';