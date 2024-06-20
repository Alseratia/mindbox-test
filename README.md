# Тестовое задание Mindbox
## Задание 2

Выполненное задание находится в папках Task2 и Task2.Tests.

Юнит-тесты можно запустить командой ``dotnet test``
## Задание 3

### Создание таблиц:

```tsql
CREATE TABLE Categories (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(255) NOT NULL
);

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(255) NOT NULL
);

-- Создание связующей таблицы для отношения "многие ко многим"
CREATE TABLE ProductCategories (
    ProductId UNIQUEIDENTIFIER NOT NULL,
    CategoryId UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY (ProductId, CategoryId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);


INSERT INTO Categories VALUES
    ('02028FA5-CB78-432F-A5C2-6D4650999A61', 'Electronics'),
    ('E55C3D85-78BA-4D70-99BE-5BA6131CC3B9', 'Clothes'),
    ('EB246D57-8341-47C9-B145-D1524835AF2C', 'Food');

INSERT INTO Products VALUES
    ('8EC8A234-0364-46BC-854F-AFBA2B2CA9AE', 'Phone'),
    ('4FE90DD4-6A17-41E9-ACCC-69699EEA3E45', 'Jeans'),
    ('A9C49974-928C-47D3-BD00-31A07E53FBCE', 'Bread'),
    ('28EB8578-9BD3-4704-B774-B613EA2237D2', 'Shampoo');

INSERT INTO ProductCategories VALUES
    ('8EC8A234-0364-46BC-854F-AFBA2B2CA9AE', '02028FA5-CB78-432F-A5C2-6D4650999A61'),
    ('4FE90DD4-6A17-41E9-ACCC-69699EEA3E45', 'E55C3D85-78BA-4D70-99BE-5BA6131CC3B9'),
    ('A9C49974-928C-47D3-BD00-31A07E53FBCE', 'EB246D57-8341-47C9-B145-D1524835AF2C');
```

### Решение задания:

```tsql
SELECT p.Name AS ProductName, c.Name AS CategoryName FROM Products AS p
LEFT JOIN ProductCategories AS pc ON pc.ProductId = p.Id
LEFT JOIN Categories AS c ON pc.CategoryId = c.Id
```
