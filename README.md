# Sistema BPMS para Gestión de Calidad

## Descripción
Sistema de Gestión de Procesos de Negocio (BPMS) especializado en Gestión de Calidad empresarial, diseñado para implementar, mantener y mejorar sistemas de gestión basados en normas internacionales como ISO 9001. El sistema utiliza una arquitectura EAV (Entity-Attribute-Value) fuertemente tipada que permite una gran flexibilidad en la definición de procesos y documentación.

## Objetivos del Sistema
- Facilitar la implementación y mantenimiento de Sistemas de Gestión de Calidad
- Automatizar la gestión documental de calidad
- Controlar y mejorar procesos empresariales
- Gestionar no conformidades, acciones correctivas y preventivas
- Realizar seguimiento de indicadores de calidad
- Gestionar auditorías internas y externas

## Arquitectura

### Tecnologías
- **Base de Datos**: SQL Server con modelo EAV
- **Backend**: API REST con .NET 8
- **Frontend**: Blazor WebAssembly PWA
- **Arquitectura**: Diseño modular orientado a microservicios

### Estructura de la Base de Datos (EAV)

#### Modelo de Datos Core

##### Classes
Definición de tipos de entidades del sistema
```sql
CREATE TABLE Classes (
    ID INT PRIMARY KEY,
    Name NVARCHAR(255),
    UniqueKey NVARCHAR(255) UNIQUE,
    IsPrimitive BIT,
    CreatedAt DATETIME,
    CreatedBy INT,
    IsActive BIT
)
```

##### Properties
Definición de propiedades para las clases
```sql
CREATE TABLE Properties (
    ID INT PRIMARY KEY,
    ClassID INT,
    PropertyClassID INT,
    UniqueKey NVARCHAR(255),
    Name NVARCHAR(255),
    CreatedAt DATETIME,
    CreatedBy INT,
    IsActive BIT,
    FOREIGN KEY (ClassID) REFERENCES Classes(ID),
    FOREIGN KEY (PropertyClassID) REFERENCES Classes(ID)
)
```

##### AbstractProperties
Metadatos adicionales para propiedades
```sql
CREATE TABLE AbstractProperties (
    PropertyID INT PRIMARY KEY,
    Min DECIMAL,
    Max DECIMAL,
    DeleteBehaviour INT,
    FOREIGN KEY (PropertyID) REFERENCES Properties(ID)
)
```

##### Ancestries
Manejo de herencia entre clases
```sql
CREATE TABLE Ancestries (
    ParentClassID INT,
    ChildClassID INT,
    CreatedAt DATETIME,
    CreatedBy INT,
    IsActive BIT,
    PRIMARY KEY (ParentClassID, ChildClassID),
    FOREIGN KEY (ParentClassID) REFERENCES Classes(ID),
    FOREIGN KEY (ChildClassID) REFERENCES Classes(ID)
)
```

##### Objects
Instancias de las clases
```sql
CREATE TABLE Objects (
    ID INT PRIMARY KEY,
    ClassID INT,
    Permissions CHAR(9),  -- Estilo Linux (rwxrwxrwx)
    CreatedAt DATETIME,
    CreatedBy INT,
    IsActive BIT,
    FOREIGN KEY (ClassID) REFERENCES Classes(ID)
)
```

##### Values
Valores de las propiedades para cada objeto
```sql
CREATE TABLE Values (
    ID INT PRIMARY KEY,
    ObjectID INT,
    PropertyID INT,
    Value NVARCHAR(MAX),
    IsActive BIT,
    CreatedBy INT,
    CreatedAt DATETIME,
    FOREIGN KEY (ObjectID) REFERENCES Objects(ID),
    FOREIGN KEY (PropertyID) REFERENCES Properties(ID)
)
```

#### Sistema de Usuarios

##### Users
```sql
CREATE TABLE Users (
    ID INT PRIMARY KEY,
    Username NVARCHAR(255),
    Email NVARCHAR(255),
    CreatedAt DATETIME,
    CreatedBy INT,
    IsActive BIT,
    PasswordHash NVARCHAR(255),
    PrimaryGroupID INT,
    FOREIGN KEY (PrimaryGroupID) REFERENCES Groups(ID)
)
```

##### Groups
```sql
CREATE TABLE Groups (
    ID INT PRIMARY KEY,
    Name NVARCHAR(255)
)
```

##### GroupUser
```sql
CREATE TABLE GroupUser (
    GroupID INT,
    UserID INT,
    PRIMARY KEY (GroupID, UserID),
    FOREIGN KEY (GroupID) REFERENCES Groups(ID),
    FOREIGN KEY (UserID) REFERENCES Users(ID)
)
```

## Módulos del Sistema

### 1. Gestión Documental de Calidad
- Control de documentos y registros
- Versionamiento de documentos
- Flujos de aprobación
- Matriz de documentos
- Control de cambios

### 2. Gestión de Procesos
- Modelador visual de procesos
- Indicadores de proceso (KPIs)
- Mapeo de procesos
- Fichas de proceso
- Matrices de riesgo

### 3. Gestión de No Conformidades
- Registro y seguimiento
- Acciones correctivas
- Acciones preventivas
- Análisis de causa raíz
- Planes de acción

### 4. Auditorías
- Programación de auditorías
- Listas de verificación
- Gestión de hallazgos
- Planes de acción
- Seguimiento de acciones

### 5. Mejora Continua
- Objetivos de calidad
- Seguimiento de indicadores
- Gestión de proyectos de mejora
- Evaluación de efectividad
- Análisis de tendencias

### 6. Gestión de Competencias
- Perfiles de puesto
- Matrices de competencias
- Planes de capacitación
- Evaluaciones de desempeño
- Registros de formación

## Características Técnicas Destacadas
- Sistema de permisos granular a nivel de objeto
- Modelo de datos flexible y extensible
- Arquitectura orientada a eventos
- API RESTful documentada con Swagger
- Interfaz PWA para acceso móvil
- Sistema de notificaciones en tiempo real

# Cryptographic Keys

This directory stores the RSA key pair used for JWT signing:
- private.key - Private key for signing tokens
- public.key - Public key for token verification

These files are not included in the repository for security reasons and must be generated locally.

## Roadmap de Desarrollo

### Fase 1: Fundamentos
- [ ] Setup inicial del proyecto
- [ ] Implementación del modelo EAV
- [ ] Sistema base de autenticación y autorización
- [ ] Componentes base de UI

### Fase 2: Módulos Core
- [ ] Gestión documental
- [ ] Modelador de procesos
- [ ] Gestión de no conformidades
- [ ] Dashboard de indicadores

### Fase 3: Módulos Avanzados
- [ ] Sistema de auditorías
- [ ] Gestión de competencias
- [ ] Análisis de datos y reportes
- [ ] Integración con sistemas externos

### Fase 4: Optimización
- [ ] Mejoras de rendimiento
- [ ] Optimización de UX
- [ ] Implementación de PWA
- [ ] Testing y documentación

## Requisitos del Sistema
- SQL Server 2019+
- .NET 8 Runtime
- Navegador moderno con soporte para WebAssembly

## Contribución
[Pendiente definir guías de contribución]

## Licencia
[Pendiente definir licencia]
