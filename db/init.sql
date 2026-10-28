-- Creates the legacy catalog schema and seed data for the Docker SQL Server.
-- Schema and data mirror the legacy EF6 database (see docs/seed-data.md). Safe to run more than once.

IF DB_ID(N'Microsoft.eShopOnContainers.Services.CatalogDb') IS NULL
    CREATE DATABASE [Microsoft.eShopOnContainers.Services.CatalogDb];
GO

USE [Microsoft.eShopOnContainers.Services.CatalogDb];
GO

IF OBJECT_ID(N'dbo.CatalogBrand') IS NULL
CREATE TABLE dbo.CatalogBrand (
    Id    INT           NOT NULL PRIMARY KEY,
    Brand NVARCHAR(100) NOT NULL
);

IF OBJECT_ID(N'dbo.CatalogType') IS NULL
CREATE TABLE dbo.CatalogType (
    Id   INT           NOT NULL PRIMARY KEY,
    Type NVARCHAR(100) NOT NULL
);

IF OBJECT_ID(N'dbo.Catalog') IS NULL
CREATE TABLE dbo.Catalog (
    Id                INT            NOT NULL PRIMARY KEY,
    Name              NVARCHAR(50)   NOT NULL,
    Description       NVARCHAR(MAX)  NULL,
    Price             DECIMAL(18, 2) NOT NULL,
    PictureFileName   NVARCHAR(MAX)  NOT NULL,
    CatalogTypeId     INT            NOT NULL REFERENCES dbo.CatalogType (Id),
    CatalogBrandId    INT            NOT NULL REFERENCES dbo.CatalogBrand (Id),
    AvailableStock    INT            NOT NULL,
    RestockThreshold  INT            NOT NULL,
    MaxStockThreshold INT            NOT NULL,
    OnReorder         BIT            NOT NULL
);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.CatalogBrand)
INSERT INTO dbo.CatalogBrand (Id, Brand) VALUES
    (1, N'Azure'), (2, N'.NET'), (3, N'Visual Studio'), (4, N'SQL Server'), (5, N'Other');

IF NOT EXISTS (SELECT 1 FROM dbo.CatalogType)
INSERT INTO dbo.CatalogType (Id, Type) VALUES
    (1, N'Mug'), (2, N'T-Shirt'), (3, N'Sheet'), (4, N'USB Memory Stick');

IF NOT EXISTS (SELECT 1 FROM dbo.Catalog)
INSERT INTO dbo.Catalog (Id, Name, Description, Price, PictureFileName, CatalogTypeId, CatalogBrandId,
                         AvailableStock, RestockThreshold, MaxStockThreshold, OnReorder) VALUES
    (1,  N'.NET Bot Black Hoodie',   N'.NET Bot Black Hoodie',   19.50, N'1.png',  2, 2, 100, 0, 0, 0),
    (2,  N'.NET Black & White Mug',  N'.NET Black & White Mug',   8.50, N'2.png',  1, 2, 100, 0, 0, 0),
    (3,  N'Prism White T-Shirt',     N'Prism White T-Shirt',     12.00, N'3.png',  2, 5, 100, 0, 0, 0),
    (4,  N'.NET Foundation T-shirt', N'.NET Foundation T-shirt', 12.00, N'4.png',  2, 2, 100, 0, 0, 0),
    (5,  N'Roslyn Red Sheet',        N'Roslyn Red Sheet',         8.50, N'5.png',  3, 5, 100, 0, 0, 0),
    (6,  N'.NET Blue Hoodie',        N'.NET Blue Hoodie',        12.00, N'6.png',  2, 2, 100, 0, 0, 0),
    (7,  N'Roslyn Red T-Shirt',      N'Roslyn Red T-Shirt',      12.00, N'7.png',  2, 5, 100, 0, 0, 0),
    (8,  N'Kudu Purple Hoodie',      N'Kudu Purple Hoodie',       8.50, N'8.png',  2, 5, 100, 0, 0, 0),
    (9,  N'Cup<T> White Mug',        N'Cup<T> White Mug',        12.00, N'9.png',  1, 5, 100, 0, 0, 0),
    (10, N'.NET Foundation Sheet',   N'.NET Foundation Sheet',   12.00, N'10.png', 3, 2, 100, 0, 0, 0),
    (11, N'Cup<T> Sheet',            N'Cup<T> Sheet',             8.50, N'11.png', 3, 2, 100, 0, 0, 0),
    (12, N'Prism White TShirt',      N'Prism White TShirt',      12.00, N'12.png', 2, 5, 100, 0, 0, 0);
GO
