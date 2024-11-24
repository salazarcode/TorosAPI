
-- 3. Insertar datos iniciales
INSERT INTO Groups (UniqueKey, Description, CreatedBy)
VALUES ('sudo', 'SuperUsers DO', NULL);

INSERT INTO Identifiers (
    Username,
    Email,
    PasswordHash,
    PasswordSalt,
    PrimaryGroupID,
    CreatedBy
)
VALUES (
    'root',
    'salarzarcode@gmail.com',
    'CHANGE_ME_HASH',
    'CHANGE_ME_SALT',
    (SELECT ID FROM Groups WHERE UniqueKey = 'sudo'),
    NULL
);

-- 4. Actualizar el CreatedBy del grupo Administrators
UPDATE Groups
SET CreatedBy = (SELECT ID FROM Identifiers WHERE Username = 'root')
WHERE UniqueKey = 'sudo';

-- 5. Actualizar el CreatedBy del admin
UPDATE Identifiers
SET CreatedBy = ID
WHERE Username = 'root';