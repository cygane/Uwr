-- INSTED OF trigger: pozwala ominąć insert, delete, update i zamiast tego wykonuje inne instrukcje zdefiniowane w triggerze
--                  : badipuje insert, update, delete na widoku

CREATE TABLE dbo.brands(
    brand_id INT IDENTITY PRIMARY KEY,
    brand_name VARCHAR(255) NOT NULL
);
GO

INSERT INTO dbo.brands(brand_name)
VALUES('Brand1'), ('Brand2'), ('Brand3');
GO

CREATE TABLE dbo.brand_approvals(
    brand_id INT IDENTITY PRIMARY KEY,
    brand_name VARCHAR(255) NOT NULL
);
GO

-- tworzenie widoku
CREATE VIEW dbo.vw_brands 
AS
SELECT
    brand_name,
    'Approved' approval_status
FROM
    dbo.brands
UNION
SELECT
    brand_name,
    'Pending Approval' approval_status
FROM dbo.brand_approvals;
GO

-- przekierowanie do brand_approvals
-- trigger wstawia nową nazwę marki do dbo.brand_aprovals, jeśli nazwa marki nie istnieje w dbo.brands
CREATE TRIGGER dbo.trg_vw_brands 
ON dbo.vw_brands
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.brand_approvals (brand_name)
    SELECT i.brand_name
    FROM inserted i
    WHERE i.brand_name NOT IN (
            SELECT brand_name
            FROM dbo.brands
        );
END
GO

-- nowy wiersz zostaje włozony do dbo.brand_approvals za pomocą odpalonego triggera
INSERT INTO dbo.vw_brands(brand_name)
VALUES('Eddy Merckx');
GO

-- pojawił się nowy wiersz
SELECT brand_name, approval_status
FROM dbo.vw_brands;
GO


SELECT * FROM dbo.brands;
SELECT * FROM dbo.brand_approvals;
GO


DROP TABLE dbo.brand_approvals;
DROP TABLE dbo.brands;
DROP VIEW IF EXISTS dbo.vw_brands;
DROP TRIGGER IF EXISTS dbo.trg_vw_brands;