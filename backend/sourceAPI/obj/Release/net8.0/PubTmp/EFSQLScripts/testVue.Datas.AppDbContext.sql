IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241023072019_InitialCreate'
)
BEGIN
    CREATE TABLE [Users] (
        [UserId] int NOT NULL IDENTITY,
        [FullName] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Role] nvarchar(max) NOT NULL,
        [Avatar] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241023072019_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241023072019_InitialCreate', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241029131510_addTableFood'
)
BEGIN
    CREATE TABLE [CustomizableItem] (
        [CustomItemId] int NOT NULL IDENTITY,
        [CustomItemName] nvarchar(max) NOT NULL,
        [CustomItemPrice] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_CustomizableItem] PRIMARY KEY ([CustomItemId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241029131510_addTableFood'
)
BEGIN
    CREATE TABLE [FoodCategory] (
        [CategoryId] int NOT NULL IDENTITY,
        [CategoryName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_FoodCategory] PRIMARY KEY ([CategoryId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241029131510_addTableFood'
)
BEGIN
    CREATE TABLE [FoodCustomizable] (
        [FoodCustomId] int NOT NULL IDENTITY,
        [FoodItemId] int NOT NULL,
        [CustomItemId] int NOT NULL,
        CONSTRAINT [PK_FoodCustomizable] PRIMARY KEY ([FoodCustomId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241029131510_addTableFood'
)
BEGIN
    CREATE TABLE [FoodItem] (
        [FoodItemId] int NOT NULL IDENTITY,
        [FoodName] nvarchar(max) NOT NULL,
        [PriceListed] decimal(18,2) NOT NULL,
        [PriceCustom] decimal(18,2) NULL,
        [ImageUrl] nvarchar(max) NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        [CategoryId] int NULL,
        [Status] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_FoodItem] PRIMARY KEY ([FoodItemId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241029131510_addTableFood'
)
BEGIN
    CREATE TABLE [FoodPriceType] (
        [PriceTypeId] int NOT NULL IDENTITY,
        [FoodItemId] int NOT NULL,
        [Size] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_FoodPriceType] PRIMARY KEY ([PriceTypeId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241029131510_addTableFood'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241029131510_addTableFood', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241118080033_addTables'
)
BEGIN
    ALTER TABLE [FoodCategory] ADD [Icon] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241118080033_addTables'
)
BEGIN
    CREATE TABLE [AdditionalFood] (
        [Id] int NOT NULL IDENTITY,
        [FoodName] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [CategoryId] int NULL,
        CONSTRAINT [PK_AdditionalFood] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241118080033_addTables'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [OrderItemId] int NOT NULL IDENTITY,
        [OrderId] int NOT NULL,
        [FoodItemId] int NOT NULL,
        [Quantity] int NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        [FoodName] nvarchar(max) NULL,
        [IsMainItem] int NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        [Note] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([OrderItemId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241118080033_addTables'
)
BEGIN
    CREATE TABLE [Orders] (
        [OrderId] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [OrderTime] datetime2 NOT NULL,
        [TableId] int NOT NULL,
        [TotalAmount] decimal(18,2) NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [Discount] decimal(18,2) NULL,
        [Tax] decimal(18,2) NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241118080033_addTables'
)
BEGIN
    CREATE TABLE [Tables] (
        [TableId] int NOT NULL IDENTITY,
        [TableName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Tables] PRIMARY KEY ([TableId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241118080033_addTables'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241118080033_addTables', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241129062042_Add table Material'
)
BEGIN
    ALTER TABLE [OrderItems] ADD [CategoryId] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241129062042_Add table Material'
)
BEGIN
    ALTER TABLE [OrderItems] ADD [OrderTime] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241129062042_Add table Material'
)
BEGIN
    ALTER TABLE [AdditionalFood] ADD [Quantity] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241129062042_Add table Material'
)
BEGIN
    CREATE TABLE [Materials] (
        [MaterialId] int NOT NULL IDENTITY,
        [MaterialName] nvarchar(100) NOT NULL,
        [MaterialType] nvarchar(50) NOT NULL,
        [Quantity] float NOT NULL,
        [MinQuantity] float NOT NULL,
        [Unit] nvarchar(20) NOT NULL,
        [ImportDate] datetime2 NOT NULL,
        [ExpirationDate] datetime2 NULL,
        [ImportPrice] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Materials] PRIMARY KEY ([MaterialId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241129062042_Add table Material'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241129062042_Add table Material', N'8.0.10');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250113160835_UpdateColumnFoodItem'
)
BEGIN
    ALTER TABLE [FoodItem] ADD [CreateDate] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250113160835_UpdateColumnFoodItem'
)
BEGIN
    ALTER TABLE [FoodItem] ADD [UpdateDate] datetime2 NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250113160835_UpdateColumnFoodItem'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250113160835_UpdateColumnFoodItem', N'8.0.10');
END;
GO

COMMIT;
GO

