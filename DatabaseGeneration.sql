create database MyEavBpms;

use MyEavBpms;

CREATE TABLE Classes (
    ID SERIAL PRIMARY KEY,
    UniqueKey VARCHAR(100) UNIQUE NOT NULL,
    IsPrimitive BOOLEAN NOT NULL,
    Name VARCHAR(100) NOT NULL
);
-- Índice no agrupado para UniqueKey
CREATE INDEX idx_classes_uniquekey ON Classes (UniqueKey);
-- Índice no agrupado para Name
CREATE INDEX idx_classes_name ON Classes (Name);

insert into classes(uniquekey, isprimitive, name) 
    values
        ('string', true, 'Cadena de texto'),
        ('integer', true, 'Número entero'),
        ('real', true, 'Número real'),
        ('char', true, 'Caracter'),
        ('datetime', true, 'Fecha y hora'),
        ('user', false, 'Usuario');

CREATE TABLE Properties (
    ID SERIAL PRIMARY KEY,
    ClassID INT NOT NULL,
    PropertyClassID INT NOT NULL,
    UniqueKey VARCHAR(100) UNIQUE NOT NULL,
    Name VARCHAR(100) NOT NULL,

    FOREIGN KEY(ClassID) REFERENCES Classes(ID) ON DELETE CASCADE,
    FOREIGN KEY(PropertyClassID) REFERENCES Classes(ID) ON DELETE CASCADE
);
-- Índice no agrupado para UniqueKey
CREATE INDEX idx_properties_uniquekey ON Properties (UniqueKey);
-- Índice no agrupado para Name
CREATE INDEX idx_properties_name ON Properties (Name);

insert into properties(classid, propertyclassid, uniquekey, name) 
    values
        (6, 1, 'first-name', 'Nombre'),
        (6, 1, 'last-name', 'Apellido'),
        (6, 2, 'age', 'Edad'),
        (6, 5, 'birth-date', 'Fecha de nacimiento'),
        (6, 5, 'father', 'Padre');


CREATE TABLE DeleteBehaviours (
    Name VARCHAR(20) PRIMARY KEY
);

insert into DeleteBehaviours(name) 
values
    ('ON_CASCADE'),
    ('NULL'),
    ('RESTRICT');

CREATE TABLE AbstractPropertyDetails (
    PropertyID INT NOT NULL,
    Min INT,
    Max INT,
    DeleteBehaviour VARCHAR(20),
    
    FOREIGN KEY (PropertyID) REFERENCES Properties(ID) ON DELETE CASCADE,
    FOREIGN KEY (DeleteBehaviour) REFERENCES DeleteBehaviours(Name) ON DELETE SET NULL
);

select * from properties

insert into AbstractPropertyDetails(PropertyID, Min, Max, DeleteBehaviour) 
    VALUES
        (5, 0, 1, 'ON_CASCADE');

CREATE TABLE Ancestries (
    ClassID INT NOT NULL,
    ParentID INT NOT NULL,

    PRIMARY KEY (ClassID, ParentID),
    FOREIGN KEY (ClassID) REFERENCES Classes(ID) ON DELETE CASCADE,
    FOREIGN KEY (ParentID) REFERENCES Classes(ID) ON DELETE CASCADE
);

CREATE TABLE Objects (
    ID SERIAL PRIMARY KEY,
    ClassID INT NOT NULL,
    CreatedAt TIMESTAMP,

    FOREIGN KEY (ClassID) REFERENCES Classes(ID) ON DELETE CASCADE
);

insert into objects(classid, TIMESTAMP) values()

CREATE TABLE StringValues (
    ID SERIAL PRIMARY KEY,
    ObjectID INT NOT NULL,
    PropertyID INT NOT NULL,
    Value VARCHAR(255),

    FOREIGN KEY (ObjectID) REFERENCES Objects(ID) ON DELETE CASCADE,
    FOREIGN KEY (PropertyID) REFERENCES Properties(ID) ON DELETE CASCADE
);

-- Índice no agrupado para Name
CREATE INDEX idx_string_values_value ON StringValues (Value);

