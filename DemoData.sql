insert into classes(uniquekey, isprimitive, name) 
    values
        ('string', true, 'Cadena de texto'),
        ('integer', true, 'Número entero'),
        ('real', true, 'Número real'),
        ('char', true, 'Caracter'),
        ('datetime', true, 'Fecha y hora'),
        ('user', false, 'Usuario');

insert into properties(classid, propertyclassid, uniquekey, name) 
    values
        (6, 1, 'first-name', 'Nombre'),
        (6, 1, 'last-name', 'Apellido'),
        (6, 2, 'age', 'Edad'),
        (6, 5, 'birth-date', 'Fecha de nacimiento'),
        (6, 5, 'father', 'Padre');

insert into DeleteBehaviours(name) 
values
    ('ON_CASCADE'),
    ('NULL'),
    ('RESTRICT');    

insert into AbstractPropertyDetails(PropertyID, Min, Max, DeleteBehaviour) 
    VALUES
        (5, 0, 1, 'ON_CASCADE');

insert into objects(classid, createdat) values(6, now());
insert into stringvalues(objectid, propertyid, value) 
values
    (1,1, 'Pedro'),
    (1,2, 'Salazar'),
    (1,3, '56'),
    (1,4, '19680713');

insert into objects(classid, createdat) values(6, now());
insert into stringvalues(objectid, propertyid, value) 
values
    (2,1, 'Andrés'),
    (2,2, 'Salazar'),
    (2,3, '35'),
    (2,4, '19890517'),
    (2,5, '1');