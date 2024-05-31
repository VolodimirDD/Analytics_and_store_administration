-- "Товарна група" 
CREATE TABLE Товарна_група (
    [ID_товар_групи] INT IDENTITY(1,1) PRIMARY KEY,
    [Найменування] NVARCHAR(255)
);

-- "Товар по порам року"
CREATE TABLE Товар_по_порам_року (
    [ID_пори_року] INT IDENTITY(1,1) PRIMARY KEY,
    [Найменування] NVARCHAR(255)
);

-- "Товар по категоріям"
CREATE TABLE Товар_по_категоріям (
    [ID_категорії] INT IDENTITY(1,1) PRIMARY KEY,
    [Найменування] NVARCHAR(255)
);

-- "Матеріали"
CREATE TABLE Матеріали (
    [ID_матеріалу] INT IDENTITY(1,1) PRIMARY KEY,
    [Найменування] NVARCHAR(255)
);

-- "Магазини"
CREATE TABLE Магазини (
    [ID_магазину] INT IDENTITY(1,1) PRIMARY KEY,
    [Адреса] NVARCHAR(255),
    [Телефон] NVARCHAR(20)
);

-- "Реалізація" 
CREATE TABLE Реалізація (
    [ID_покупки] INT IDENTITY(1,1) PRIMARY KEY,
    [Дата] DATE,
    [ID_магазину] INT,
    FOREIGN KEY ([ID_магазину]) REFERENCES Магазини([ID_магазину]) ON DELETE CASCADE
);

-- "Товар" 
CREATE TABLE Товар (
    [ID_товару] INT IDENTITY(1,1) PRIMARY KEY,
    [Назва] NVARCHAR(255),
    [Ціна] INT,
    [Розмір] NVARCHAR(20),
    [ID_товарної_групи] INT,
    [ID_пори_року] INT,
    [ID_категорії] INT,
    [ID_матеріалу] INT,
    FOREIGN KEY ([ID_товарної_групи]) REFERENCES Товарна_група([ID_товар_групи]) ON DELETE CASCADE,
    FOREIGN KEY ([ID_пори_року]) REFERENCES Товар_по_порам_року([ID_пори_року]) ON DELETE CASCADE,
    FOREIGN KEY ([ID_категорії]) REFERENCES Товар_по_категоріям([ID_категорії]) ON DELETE CASCADE,
    FOREIGN KEY ([ID_матеріалу]) REFERENCES Матеріали([ID_матеріалу]) ON DELETE CASCADE
);

-- "Зміст реалізації"
CREATE TABLE Зміст_реалізації (
    [ID_зміст_реал] INT IDENTITY(1,1) PRIMARY KEY,
    [ID_покупки] INT,
    [Кількість] INT,
    [ID_товару] INT,
    FOREIGN KEY ([ID_покупки]) REFERENCES Реалізація([ID_покупки]) ON DELETE CASCADE,
    FOREIGN KEY ([ID_товару]) REFERENCES Товар([ID_товару]) ON DELETE CASCADE
);

-- "Користувачі" 
CREATE TABLE Користувачі (
    [ID_користувача] INT IDENTITY(1,1) PRIMARY KEY,
    [Логін] NVARCHAR(255) UNIQUE NOT NULL,
    [Захешований_пароль] NVARCHAR(255) NOT NULL
);