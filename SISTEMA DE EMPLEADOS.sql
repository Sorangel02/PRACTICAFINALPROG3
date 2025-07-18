CREATE DATABASE SistemaDeEmpleados;
GO


USE SistemaDeEmpleados;
GO

CREATE TABLE Departamentos (
    DepartamentoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);
GO


CREATE TABLE Cargos (
    CargoID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
);
GO


CREATE TABLE Empleados (
    EmpleadoID INT PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    DepartamentoID INT NOT NULL,
    CargoID INT NOT NULL,
    FechaInicio DATE NOT NULL,
    Salario DECIMAL(18,2) NOT NULL CHECK (Salario >= 0),
    Estado BIT NOT NULL, -- 1 = Vigente, 0 = No vigente

  
    FOREIGN KEY (DepartamentoID) REFERENCES Departamentos(DepartamentoID),
    FOREIGN KEY (CargoID) REFERENCES Cargos(CargoID)
);
GO


CREATE TABLE VacacionesDisponibles (
    VacacionesID INT PRIMARY KEY IDENTITY(1,1),
    EmpleadoID INT NOT NULL,
    Anio INT NOT NULL CHECK (Anio >= 2000),
    DiasAsignados INT NOT NULL CHECK (DiasAsignados >= 0),
    DiasTomados INT NOT NULL DEFAULT 0 CHECK (DiasTomados >= 0),
    DiasRestantes AS (DiasAsignados - DiasTomados) PERSISTED,

    FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID)
);
GO