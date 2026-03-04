/* =========================================================
   KioskDB2_full.sql
   Order:
   1) CREATE DATABASE + USE
   2) TABLES
   3) VIEWS + FUNCTIONS
   4) STORED PROCEDURES
   ========================================================= */

------------------------------------------------------------
-- 1) CREATE DATABASE + USE
------------------------------------------------------------
IF DB_ID('KioskDB2') IS NULL
BEGIN
    CREATE DATABASE KioskDB2;
END
GO

USE KioskDB2;
GO

------------------------------------------------------------
-- 2) TABLES
------------------------------------------------------------

-- USERS
IF OBJECT_ID('dbo.Users','U') IS NOT NULL DROP TABLE dbo.Users;
GO
CREATE TABLE dbo.Users (
    id_user       INT IDENTITY(1,1) PRIMARY KEY,
    nom           NVARCHAR(60)  NOT NULL,
    prenom        NVARCHAR(60)  NOT NULL,
    role          NVARCHAR(20)  NOT NULL,
    login         NVARCHAR(80)  NOT NULL,
    mot_de_passe  NVARCHAR(255) NOT NULL,
    CONSTRAINT UQ_Users_Login UNIQUE (login),
    CONSTRAINT CK_Users_Role CHECK (role IN ('Manager', 'Caissier'))
);
GO

-- CATEGORIES
IF OBJECT_ID('dbo.Categories','U') IS NOT NULL DROP TABLE dbo.Categories;
GO
CREATE TABLE dbo.Categories (
    id_categorie INT IDENTITY(1,1) PRIMARY KEY,
    libelle      NVARCHAR(80) NOT NULL,
    CONSTRAINT UQ_Categories_Libelle UNIQUE (libelle)
);
GO

-- SUPPLIERS
IF OBJECT_ID('dbo.Suppliers','U') IS NOT NULL DROP TABLE dbo.Suppliers;
GO
CREATE TABLE dbo.Suppliers (
    id_supplier INT IDENTITY(1,1) PRIMARY KEY,
    nom         NVARCHAR(120) NOT NULL,
    telephone   NVARCHAR(30)  NULL,
    email       NVARCHAR(120) NULL
);
GO

-- PRODUCTS
IF OBJECT_ID('dbo.Products','U') IS NOT NULL DROP TABLE dbo.Products;
GO
CREATE TABLE dbo.Products (
    id_produit     INT IDENTITY(1,1) PRIMARY KEY,
    nom            NVARCHAR(120) NOT NULL,
    prix_vente     DECIMAL(10,2) NOT NULL,
    stock_minimum  INT NOT NULL CONSTRAINT DF_Products_StockMin DEFAULT 0,
    actif          BIT NOT NULL CONSTRAINT DF_Products_Actif DEFAULT 1,

    id_categorie   INT NOT NULL,
    id_supplier    INT NOT NULL,

    CONSTRAINT FK_Products_Categories
        FOREIGN KEY (id_categorie) REFERENCES dbo.Categories(id_categorie),

    CONSTRAINT FK_Products_Suppliers
        FOREIGN KEY (id_supplier) REFERENCES dbo.Suppliers(id_supplier),

    CONSTRAINT CK_Products_Prix CHECK (prix_vente >= 0),
    CONSTRAINT CK_Products_StockMin CHECK (stock_minimum >= 0)
);
GO

-- INVENTORY (1-1 with PRODUCTS)
IF OBJECT_ID('dbo.Inventory','U') IS NOT NULL DROP TABLE dbo.Inventory;
GO
CREATE TABLE dbo.Inventory (
    id_inventory      INT IDENTITY(1,1) PRIMARY KEY,
    id_produit        INT NOT NULL,
    quantite_actuelle INT NOT NULL CONSTRAINT DF_Inventory_Qte DEFAULT 0,
    date_maj          DATETIME2 NOT NULL CONSTRAINT DF_Inventory_DateMaj DEFAULT SYSDATETIME(),

    CONSTRAINT FK_Inventory_Products
        FOREIGN KEY (id_produit) REFERENCES dbo.Products(id_produit),

    CONSTRAINT UQ_Inventory_Produit UNIQUE (id_produit),

    CONSTRAINT CK_Inventory_Qte CHECK (quantite_actuelle >= 0)
);
GO

-- SHIFT  (table name = Shift)
IF OBJECT_ID('dbo.[Shift]','U') IS NOT NULL DROP TABLE dbo.[Shift];
GO
CREATE TABLE dbo.[Shift] (
    id_shift          INT IDENTITY(1,1) PRIMARY KEY,
    id_user           INT NOT NULL,

    date_ouverture    DATETIME2 NOT NULL,
    date_fermeture    DATETIME2 NULL,

    montant_theorique DECIMAL(10,2) NOT NULL CONSTRAINT DF_Shift_Theorique DEFAULT 0,
    montant_reel      DECIMAL(10,2) NOT NULL CONSTRAINT DF_Shift_Reel DEFAULT 0,
    ecart             DECIMAL(10,2) NOT NULL CONSTRAINT DF_Shift_Ecart DEFAULT 0,

    CONSTRAINT FK_Shift_Users
        FOREIGN KEY (id_user) REFERENCES dbo.Users(id_user),

    CONSTRAINT CK_Shift_Montants CHECK (
        montant_theorique >= 0 AND montant_reel >= 0
    ),

    CONSTRAINT CK_Shift_Dates CHECK (
        date_fermeture IS NULL OR date_fermeture >= date_ouverture
    )
);
GO

-- SALES
IF OBJECT_ID('dbo.Sales','U') IS NOT NULL DROP TABLE dbo.Sales;
GO
CREATE TABLE dbo.Sales (
    id_sale       INT IDENTITY(1,1) PRIMARY KEY,
    id_user       INT NOT NULL,
    date_heure    DATETIME2 NOT NULL CONSTRAINT DF_Sales_DateHeure DEFAULT SYSDATETIME(),
    montant_total DECIMAL(10,2) NOT NULL CONSTRAINT DF_Sales_Total DEFAULT 0,

    CONSTRAINT FK_Sales_Users
        FOREIGN KEY (id_user) REFERENCES dbo.Users(id_user),

    CONSTRAINT CK_Sales_Total CHECK (montant_total >= 0)
);
GO

-- SALEDETAILS
IF OBJECT_ID('dbo.SaleDetails','U') IS NOT NULL DROP TABLE dbo.SaleDetails;
GO
CREATE TABLE dbo.SaleDetails (
    id_sale_detail INT IDENTITY(1,1) PRIMARY KEY,
    id_sale        INT NOT NULL,
    id_produit     INT NOT NULL,

    quantite       INT NOT NULL,
    prix_unitaire  DECIMAL(10,2) NOT NULL,
    sous_total     DECIMAL(10,2) NOT NULL,

    CONSTRAINT FK_SaleDetails_Sales
        FOREIGN KEY (id_sale) REFERENCES dbo.Sales(id_sale),

    CONSTRAINT FK_SaleDetails_Products
        FOREIGN KEY (id_produit) REFERENCES dbo.Products(id_produit),

    CONSTRAINT UQ_SaleDetails_Sale_Product UNIQUE (id_sale, id_produit),

    CONSTRAINT CK_SaleDetails_Qte CHECK (quantite > 0),
    CONSTRAINT CK_SaleDetails_Prix CHECK (prix_unitaire >= 0),
    CONSTRAINT CK_SaleDetails_SousTotal CHECK (sous_total >= 0),
    CONSTRAINT CK_SaleDetails_SousTotalCalc CHECK (sous_total = quantite * prix_unitaire)
);
GO

-- PURCHASES
IF OBJECT_ID('dbo.Purchases','U') IS NOT NULL DROP TABLE dbo.Purchases;
GO
CREATE TABLE dbo.Purchases (
    id_purchase INT IDENTITY(1,1) PRIMARY KEY,
    id_produit  INT NOT NULL,
    date_achat  DATETIME2 NOT NULL CONSTRAINT DF_Purchases_DateAchat DEFAULT SYSDATETIME(),
    quantite    INT NOT NULL,

    CONSTRAINT FK_Purchases_Products
        FOREIGN KEY (id_produit) REFERENCES dbo.Products(id_produit),

    CONSTRAINT CK_Purchases_Qte CHECK (quantite > 0)
);
GO

------------------------------------------------------------
-- 3) VIEWS + FUNCTIONS
------------------------------------------------------------

-- V1: Current Stock
CREATE OR ALTER VIEW dbo.vw_CurrentStock
AS
SELECT p.id_produit,
       p.nom AS produit,
       p.prix_vente,
       p.stock_minimum,
       p.actif,
       c.libelle AS categorie,
       s.nom AS supplier,
       i.quantite_actuelle,
       i.date_maj,
       IIF(i.quantite_actuelle <= 0, 'RUPTURE',
           IIF(i.quantite_actuelle < p.stock_minimum, 'FAIBLE', 'OK')) AS stock_status
FROM dbo.Products p
INNER JOIN dbo.Inventory i ON i.id_produit = p.id_produit
INNER JOIN dbo.Categories c ON c.id_categorie = p.id_categorie
INNER JOIN dbo.Suppliers  s ON s.id_supplier  = p.id_supplier;
GO

-- V2: Daily Sales Summary
CREATE OR ALTER VIEW dbo.vw_DailySalesSummary
AS
SELECT CONVERT(date, s.date_heure) AS jour,
       COUNT(*) AS nb_ventes,
       SUM(s.montant_total) AS total_jour
FROM dbo.Sales s
GROUP BY CONVERT(date, s.date_heure);
GO

-- F1: Sales between dates
CREATE OR ALTER FUNCTION dbo.fn_SalesBetweenDates
(
  @date_from DATETIME2,
  @date_to   DATETIME2
)
RETURNS TABLE
AS
RETURN
(
  SELECT s.id_sale, s.date_heure, s.montant_total,
         s.id_user, u.nom, u.prenom, u.role
  FROM dbo.Sales s
  INNER JOIN dbo.Users u ON u.id_user = s.id_user
  WHERE s.date_heure >= @date_from AND s.date_heure < @date_to
);
GO

-- F2: Low stock products
CREATE OR ALTER FUNCTION dbo.fn_LowStockProducts()
RETURNS TABLE
AS
RETURN
(
  SELECT p.id_produit, p.nom, p.stock_minimum,
         i.quantite_actuelle,
         (p.stock_minimum - i.quantite_actuelle) AS manque
  FROM dbo.Products p
  INNER JOIN dbo.Inventory i ON i.id_produit = p.id_produit
  WHERE i.quantite_actuelle < p.stock_minimum
);
GO

-- F3: Top selling products between dates
CREATE OR ALTER FUNCTION dbo.fn_TopSellingProducts
(
  @date_from DATETIME2,
  @date_to   DATETIME2
)
RETURNS TABLE
AS
RETURN
(
  SELECT p.id_produit, p.nom,
         SUM(sd.quantite)   AS qte_vendue,
         SUM(sd.sous_total) AS total_vente
  FROM dbo.Sales s
  INNER JOIN dbo.SaleDetails sd ON sd.id_sale = s.id_sale
  INNER JOIN dbo.Products p     ON p.id_produit = sd.id_produit
  WHERE s.date_heure >= @date_from AND s.date_heure < @date_to
  GROUP BY p.id_produit, p.nom
);
GO

-- F4: Purchases by supplier between dates
CREATE OR ALTER FUNCTION dbo.fn_PurchasesBySupplierBetweenDates
(
  @date_from DATETIME2,
  @date_to   DATETIME2
)
RETURNS TABLE
AS
RETURN
(
  SELECT sup.id_supplier, sup.nom AS supplier,
         SUM(pu.quantite) AS qte_achetee
  FROM dbo.Purchases pu
  INNER JOIN dbo.Products pr   ON pr.id_produit = pu.id_produit
  INNER JOIN dbo.Suppliers sup ON sup.id_supplier = pr.id_supplier
  WHERE pu.date_achat >= @date_from AND pu.date_achat < @date_to
  GROUP BY sup.id_supplier, sup.nom
);
GO

------------------------------------------------------------
-- 4) STORED PROCEDURES
------------------------------------------------------------

----------------------------
-- AUTH (MISSING IN YOUR SCRIPT ✅)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Auth_Login
  @login NVARCHAR(80),
  @mot_de_passe NVARCHAR(255)
AS
BEGIN
  SET NOCOUNT ON;

  SELECT TOP 1
         id_user, nom, prenom, role, login
  FROM dbo.Users
  WHERE login = @login
    AND mot_de_passe = @mot_de_passe;
END
GO

----------------------------
-- USERS (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Users_Insert
  @nom NVARCHAR(60),
  @prenom NVARCHAR(60),
  @role NVARCHAR(20),
  @login NVARCHAR(80),
  @mot_de_passe NVARCHAR(255),
  @new_id_user INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Users(nom, prenom, role, login, mot_de_passe)
  VALUES(@nom, @prenom, @role, @login, @mot_de_passe);
  SET @new_id_user = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Users_Update
  @id_user INT,
  @nom NVARCHAR(60),
  @prenom NVARCHAR(60),
  @role NVARCHAR(20),
  @login NVARCHAR(80),
  @mot_de_passe NVARCHAR(255)
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Users
  SET nom=@nom, prenom=@prenom, role=@role, login=@login, mot_de_passe=@mot_de_passe
  WHERE id_user=@id_user;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Users_Delete
  @id_user INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Users WHERE id_user=@id_user;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Users_SelectOne
  @id_user INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT id_user, nom, prenom, role, login
  FROM dbo.Users
  WHERE id_user=@id_user;
END
GO

----------------------------
-- CATEGORIES (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Categories_Insert
  @libelle NVARCHAR(80),
  @new_id_categorie INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Categories(libelle) VALUES(@libelle);
  SET @new_id_categorie = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categories_Update
  @id_categorie INT,
  @libelle NVARCHAR(80)
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Categories SET libelle=@libelle
  WHERE id_categorie=@id_categorie;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categories_Delete
  @id_categorie INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Categories WHERE id_categorie=@id_categorie;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Categories_SelectOne
  @id_categorie INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT id_categorie, libelle
  FROM dbo.Categories
  WHERE id_categorie=@id_categorie;
END
GO

----------------------------
-- SUPPLIERS (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Suppliers_Insert
  @nom NVARCHAR(120),
  @telephone NVARCHAR(30)=NULL,
  @email NVARCHAR(120)=NULL,
  @new_id_supplier INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Suppliers(nom, telephone, email)
  VALUES(@nom, @telephone, @email);
  SET @new_id_supplier = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Suppliers_Update
  @id_supplier INT,
  @nom NVARCHAR(120),
  @telephone NVARCHAR(30)=NULL,
  @email NVARCHAR(120)=NULL
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Suppliers
  SET nom=@nom, telephone=@telephone, email=@email
  WHERE id_supplier=@id_supplier;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Suppliers_Delete
  @id_supplier INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Suppliers WHERE id_supplier=@id_supplier;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Suppliers_SelectOne
  @id_supplier INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT id_supplier, nom, telephone, email
  FROM dbo.Suppliers
  WHERE id_supplier=@id_supplier;
END
GO

----------------------------
-- PRODUCTS (CRUD)
-- NOTE: small enhancement: create Inventory row on insert ✅
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Products_Insert
  @nom NVARCHAR(120),
  @prix_vente DECIMAL(10,2),
  @stock_minimum INT,
  @actif BIT,
  @id_categorie INT,
  @id_supplier INT,
  @new_id_produit INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO dbo.Products(nom, prix_vente, stock_minimum, actif, id_categorie, id_supplier)
  VALUES(@nom, @prix_vente, @stock_minimum, @actif, @id_categorie, @id_supplier);

  SET @new_id_produit = CONVERT(INT, SCOPE_IDENTITY());

  -- Ensure Inventory exists for this product
  IF NOT EXISTS (SELECT 1 FROM dbo.Inventory WHERE id_produit = @new_id_produit)
  BEGIN
      INSERT INTO dbo.Inventory(id_produit, quantite_actuelle, date_maj)
      VALUES(@new_id_produit, 0, SYSDATETIME());
  END
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Products_Update
  @id_produit INT,
  @nom NVARCHAR(120),
  @prix_vente DECIMAL(10,2),
  @stock_minimum INT,
  @actif BIT,
  @id_categorie INT,
  @id_supplier INT
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE dbo.Products
  SET nom=@nom,
      prix_vente=@prix_vente,
      stock_minimum=@stock_minimum,
      actif=@actif,
      id_categorie=@id_categorie,
      id_supplier=@id_supplier
  WHERE id_produit=@id_produit;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Products_Delete
  @id_produit INT
AS
BEGIN
  SET NOCOUNT ON;

  -- If you want safe delete when no references:
  DELETE FROM dbo.Inventory WHERE id_produit = @id_produit;
  DELETE FROM dbo.Products  WHERE id_produit = @id_produit;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Products_SelectOne
  @id_produit INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT p.id_produit, p.nom, p.prix_vente, p.stock_minimum, p.actif,
         p.id_categorie, c.libelle AS categorie,
         p.id_supplier, s.nom AS supplier
  FROM dbo.Products p
  INNER JOIN dbo.Categories c ON c.id_categorie = p.id_categorie
  INNER JOIN dbo.Suppliers  s ON s.id_supplier  = p.id_supplier
  WHERE p.id_produit=@id_produit;
END
GO

----------------------------
-- INVENTORY (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Inventory_Insert
  @id_produit INT,
  @quantite_actuelle INT,
  @date_maj DATETIME2 = NULL,
  @new_id_inventory INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Inventory(id_produit, quantite_actuelle, date_maj)
  VALUES(@id_produit, @quantite_actuelle, ISNULL(@date_maj, SYSDATETIME()));
  SET @new_id_inventory = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inventory_Update
  @id_inventory INT,
  @quantite_actuelle INT,
  @date_maj DATETIME2 = NULL
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Inventory
  SET quantite_actuelle=@quantite_actuelle,
      date_maj = ISNULL(@date_maj, SYSDATETIME())
  WHERE id_inventory=@id_inventory;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inventory_Delete
  @id_inventory INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Inventory WHERE id_inventory=@id_inventory;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Inventory_SelectOne
  @id_inventory INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT i.id_inventory, i.id_produit, p.nom AS produit,
         i.quantite_actuelle, i.date_maj
  FROM dbo.Inventory i
  INNER JOIN dbo.Products p ON p.id_produit = i.id_produit
  WHERE i.id_inventory=@id_inventory;
END
GO

----------------------------
-- INVENTORY MOVES (MISSING IN YOUR SCRIPT ✅)
-- Used by: POS + Restock
----------------------------

-- Increase stock on Purchase (Restock)
CREATE OR ALTER PROCEDURE dbo.usp_Inventory_IncreaseOnPurchase
  @id_produit INT,
  @quantite   INT
AS
BEGIN
  SET NOCOUNT ON;

  IF @quantite <= 0
  BEGIN
      RAISERROR('Quantite must be > 0', 16, 1);
      RETURN;
  END

  -- Upsert inventory
  IF EXISTS (SELECT 1 FROM dbo.Inventory WHERE id_produit = @id_produit)
  BEGIN
      UPDATE dbo.Inventory
      SET quantite_actuelle = quantite_actuelle + @quantite,
          date_maj = SYSDATETIME()
      WHERE id_produit = @id_produit;
  END
  ELSE
  BEGIN
      INSERT INTO dbo.Inventory(id_produit, quantite_actuelle, date_maj)
      VALUES(@id_produit, @quantite, SYSDATETIME());
  END
END
GO

-- Decrease stock on Sale (POS)
CREATE OR ALTER PROCEDURE dbo.usp_Inventory_DecreaseOnSale
  @id_produit INT,
  @quantite   INT
AS
BEGIN
  SET NOCOUNT ON;

  IF @quantite <= 0
  BEGIN
      RAISERROR('Quantite must be > 0', 16, 1);
      RETURN;
  END

  DECLARE @stock INT;

  SELECT @stock = quantite_actuelle
  FROM dbo.Inventory
  WHERE id_produit = @id_produit;

  IF @stock IS NULL
  BEGIN
      RAISERROR('Inventory row not found for this product.', 16, 1);
      RETURN;
  END

  IF @stock < @quantite
  BEGIN
      RAISERROR('Stock insuffisant.', 16, 1);
      RETURN;
  END

  UPDATE dbo.Inventory
  SET quantite_actuelle = quantite_actuelle - @quantite,
      date_maj = SYSDATETIME()
  WHERE id_produit = @id_produit;
END
GO

----------------------------
-- SHIFT (CRUD + Open/Close/GetOpenByUser)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Shift_Insert
  @id_user INT,
  @date_ouverture DATETIME2,
  @date_fermeture DATETIME2 = NULL,
  @montant_theorique DECIMAL(10,2),
  @montant_reel DECIMAL(10,2),
  @ecart DECIMAL(10,2),
  @new_id_shift INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.[Shift](id_user, date_ouverture, date_fermeture, montant_theorique, montant_reel, ecart)
  VALUES(@id_user, @date_ouverture, @date_fermeture, @montant_theorique, @montant_reel, @ecart);
  SET @new_id_shift = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Shift_Update
  @id_shift INT,
  @id_user INT,
  @date_ouverture DATETIME2,
  @date_fermeture DATETIME2 = NULL,
  @montant_theorique DECIMAL(10,2),
  @montant_reel DECIMAL(10,2),
  @ecart DECIMAL(10,2)
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.[Shift]
  SET id_user=@id_user,
      date_ouverture=@date_ouverture,
      date_fermeture=@date_fermeture,
      montant_theorique=@montant_theorique,
      montant_reel=@montant_reel,
      ecart=@ecart
  WHERE id_shift=@id_shift;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Shift_Delete
  @id_shift INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.[Shift] WHERE id_shift=@id_shift;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Shift_SelectOne
  @id_shift INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT sh.id_shift, sh.id_user, u.nom, u.prenom, u.role,
         sh.date_ouverture, sh.date_fermeture,
         sh.montant_theorique, sh.montant_reel, sh.ecart
  FROM dbo.[Shift] sh
  INNER JOIN dbo.Users u ON u.id_user = sh.id_user
  WHERE sh.id_shift=@id_shift;
END
GO

-- ✅ FIXED: Shift Open (NO NULL inserts)
CREATE OR ALTER PROCEDURE dbo.usp_Shift_Open
  @id_user INT,
  @new_id_shift INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO dbo.[Shift](id_user, date_ouverture, date_fermeture, montant_theorique, montant_reel, ecart)
  VALUES(@id_user, SYSDATETIME(), NULL, 0, 0, 0);

  SET @new_id_shift = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Shift_Close
  @id_shift INT,
  @montant_reel DECIMAL(10,2)
AS
BEGIN
  SET NOCOUNT ON;

  DECLARE @open DATETIME2;

  SELECT @open = date_ouverture
  FROM dbo.[Shift]
  WHERE id_shift = @id_shift;

  DECLARE @theorique DECIMAL(10,2) =
  (
    SELECT COALESCE(SUM(montant_total), 0)
    FROM dbo.Sales
    WHERE date_heure >= @open
      AND date_heure < SYSDATETIME()
  );

  UPDATE dbo.[Shift]
  SET date_fermeture = SYSDATETIME(),
      montant_theorique = @theorique,
      montant_reel = @montant_reel,
      ecart = (@montant_reel - @theorique)
  WHERE id_shift = @id_shift;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Shift_GetOpenByUser
  @id_user INT
AS
BEGIN
  SET NOCOUNT ON;

  SELECT TOP 1 *
  FROM dbo.[Shift]
  WHERE id_user = @id_user
    AND date_fermeture IS NULL
  ORDER BY date_ouverture DESC;
END
GO

----------------------------
-- SALES (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Sales_Insert
  @id_user INT,
  @date_heure DATETIME2 = NULL,
  @montant_total DECIMAL(10,2),
  @new_id_sale INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Sales(id_user, date_heure, montant_total)
  VALUES(@id_user, ISNULL(@date_heure, SYSDATETIME()), @montant_total);
  SET @new_id_sale = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Sales_Update
  @id_sale INT,
  @id_user INT,
  @date_heure DATETIME2,
  @montant_total DECIMAL(10,2)
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Sales
  SET id_user=@id_user,
      date_heure=@date_heure,
      montant_total=@montant_total
  WHERE id_sale=@id_sale;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Sales_Delete
  @id_sale INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Sales WHERE id_sale=@id_sale;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Sales_SelectOne
  @id_sale INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT s.id_sale, s.date_heure, s.montant_total,
         s.id_user, u.nom, u.prenom
  FROM dbo.Sales s
  INNER JOIN dbo.Users u ON u.id_user = s.id_user
  WHERE s.id_sale=@id_sale;
END
GO

----------------------------
-- SALEDETAILS (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_SaleDetails_Insert
  @id_sale INT,
  @id_produit INT,
  @quantite INT,
  @prix_unitaire DECIMAL(10,2),
  @new_id_sale_detail INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @sous_total DECIMAL(10,2) = @quantite * @prix_unitaire;

  INSERT INTO dbo.SaleDetails(id_sale, id_produit, quantite, prix_unitaire, sous_total)
  VALUES(@id_sale, @id_produit, @quantite, @prix_unitaire, @sous_total);

  SET @new_id_sale_detail = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_SaleDetails_Update
  @id_sale_detail INT,
  @id_sale INT,
  @id_produit INT,
  @quantite INT,
  @prix_unitaire DECIMAL(10,2)
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @sous_total DECIMAL(10,2) = @quantite * @prix_unitaire;

  UPDATE dbo.SaleDetails
  SET id_sale=@id_sale,
      id_produit=@id_produit,
      quantite=@quantite,
      prix_unitaire=@prix_unitaire,
      sous_total=@sous_total
  WHERE id_sale_detail=@id_sale_detail;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_SaleDetails_Delete
  @id_sale_detail INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.SaleDetails WHERE id_sale_detail=@id_sale_detail;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_SaleDetails_SelectOne
  @id_sale_detail INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT sd.id_sale_detail, sd.id_sale, sd.id_produit,
         p.nom AS produit,
         sd.quantite, sd.prix_unitaire, sd.sous_total
  FROM dbo.SaleDetails sd
  INNER JOIN dbo.Products p ON p.id_produit = sd.id_produit
  WHERE sd.id_sale_detail=@id_sale_detail;
END
GO

----------------------------
-- PURCHASES (CRUD)
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Purchases_Insert
  @id_produit INT,
  @date_achat DATETIME2 = NULL,
  @quantite INT,
  @new_id_purchase INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO dbo.Purchases(id_produit, date_achat, quantite)
  VALUES(@id_produit, ISNULL(@date_achat, SYSDATETIME()), @quantite);
  SET @new_id_purchase = CONVERT(INT, SCOPE_IDENTITY());
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Purchases_Update
  @id_purchase INT,
  @id_produit INT,
  @date_achat DATETIME2,
  @quantite INT
AS
BEGIN
  SET NOCOUNT ON;
  UPDATE dbo.Purchases
  SET id_produit=@id_produit,
      date_achat=@date_achat,
      quantite=@quantite
  WHERE id_purchase=@id_purchase;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Purchases_Delete
  @id_purchase INT
AS
BEGIN
  SET NOCOUNT ON;
  DELETE FROM dbo.Purchases WHERE id_purchase=@id_purchase;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Purchases_SelectOne
  @id_purchase INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT pu.id_purchase, pu.date_achat, pu.quantite,
         pu.id_produit, p.nom AS produit
  FROM dbo.Purchases pu
  INNER JOIN dbo.Products p ON p.id_produit = pu.id_produit
  WHERE pu.id_purchase=@id_purchase;
END
GO

----------------------------
-- REPORTING SPs
----------------------------
CREATE OR ALTER PROCEDURE dbo.usp_Report_SalesSummary
  @date_from DATETIME2,
  @date_to   DATETIME2
AS
BEGIN
  SET NOCOUNT ON;
  SELECT CONVERT(date, f.date_heure) AS jour,
         COUNT(*) AS nb_ventes,
         SUM(f.montant_total) AS total_jour
  FROM dbo.fn_SalesBetweenDates(@date_from, @date_to) f
  GROUP BY CONVERT(date, f.date_heure)
  ORDER BY jour;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_Report_TopProducts
  @date_from DATETIME2,
  @date_to   DATETIME2,
  @topN      INT = 10
AS
BEGIN
  SET NOCOUNT ON;
  SELECT TOP (@topN)
         t.id_produit, t.nom, t.qte_vendue, t.total_vente
  FROM dbo.fn_TopSellingProducts(@date_from, @date_to) t
  ORDER BY t.qte_vendue DESC, t.total_vente DESC;
END
GO