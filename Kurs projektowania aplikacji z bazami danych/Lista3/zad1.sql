CREATE PROCEDURE dbo.backup_standart AS
BEGIN
    CREATE TABLE ProductDescription_Backup (
        ProductDescriptionID int NOT NULL,
        Description nvarchar(400) NOT NULL,
        rowguid uniqueidentifier NOT NULL,
        ModifiedDate datetime NOT NULL
    );

    DECLARE @StartTime datetime = GETDATE();

    INSERT INTO ProductDescription_Backup
    SELECT *
    FROM SalesLT.ProductDescription;

    DECLARE @EndTime datetime = GETDATE();
    SELECT DATEDIFF(ms, @StartTime, @EndTime) AS 'miliseconds_standart';
END
GO

CREATE PROCEDURE dbo.backup_cursor AS
BEGIN
    CREATE TABLE ProductDescription_Backup (
        ProductDescriptionID int NOT NULL,
        Description nvarchar(400) NOT NULL,
        rowguid uniqueidentifier NOT NULL,
        ModifiedDate datetime NOT NULL
    );

    DECLARE @StartTime datetime = GETDATE();

    DECLARE @ProductDescriptionID int;
    DECLARE @Description nvarchar(400);
    DECLARE @rowguid uniqueidentifier;
    DECLARE @ModifiedDate datetime;

    DECLARE i CURSOR FOR
    SELECT *
    FROM SalesLT.ProductDescription;

    OPEN i;
    FETCH NEXT FROM i INTO @ProductDescriptionID, @Description, @rowguid, @ModifiedDate;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO ProductDescription_Backup (ProductDescriptionID, Description, rowguid, ModifiedDate)
        VALUES (@ProductDescriptionID, @Description, @rowguid, @ModifiedDate);

      FETCH NEXT FROM i INTO @ProductDescriptionID, @Description, @rowguid, @ModifiedDate;
    END;

    CLOSE i;
    DEALLOCATE i;

    DECLARE @EndTime datetime = GETDATE();
    SELECT DATEDIFF(ms, @StartTime, @EndTime) AS 'milliseconds_cursor';
END
GO



EXEC backup_standart;
DROP TABLE ProductDescription_Backup;
GO

EXEC backup_cursor;
DROP TABLE ProductDescription_Backup;
GO