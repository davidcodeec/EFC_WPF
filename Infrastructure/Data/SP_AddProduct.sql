CREATE PROCEDURE AddProduct 
    @ArticleNumber nvarchar(200),
    @ProductName nvarchar(200),
    @Ingress nvarchar(200),
    @Description nvarchar(max),
    @UnitPrice money,
    @CategoryName nvarchar(100),
    @SupplierName nvarchar(50),
    @SupplierNumber nvarchar(50),
    @ContactPerson nvarchar(50), 
    @SupplierEmail nvarchar(50), 
    @SupplierPhone nvarchar(50),
    @ImagePath nvarchar(max),
    @Result nvarchar(max) OUTPUT
AS
BEGIN
    PRINT 'Starting AddProduct procedure...'
    
    IF NOT EXISTS (SELECT 1 FROM Products WHERE ArticleNumber = @ArticleNumber)
    BEGIN
        PRINT 'ArticleNumber does not exist, proceeding with product creation...'
        
        BEGIN TRANSACTION
        BEGIN TRY
            DECLARE @SupplierId int
            EXEC GetOrCreateSupplierId @SupplierName, @SupplierNumber, @ContactPerson, @SupplierEmail, @SupplierPhone, @SupplierId OUTPUT
            PRINT 'SupplierId: ' + CONVERT(nvarchar(50), @SupplierId)

            DECLARE @CategoryId int
            EXEC GetOrCreateCategoryId @CategoryName, @CategoryId OUTPUT
            PRINT 'CategoryId: ' + CONVERT(nvarchar(50), @CategoryId)

            DECLARE @ProductImageId int
            EXEC GetOrCreateProductImageId @ImagePath, @ProductImageId OUTPUT
            PRINT 'ProductImageId: ' + CONVERT(nvarchar(50), @ProductImageId)

            INSERT INTO Products (ArticleNumber, ProductName, Ingress, Description, UnitPrice, CategoryId, SupplierId, ProductImageId)
            VALUES (@ArticleNumber, @ProductName, @Ingress, @Description, @UnitPrice, @CategoryId, @SupplierId, @ProductImageId)

            SET @Result = 'Product was created successfully!'
            PRINT 'Product creation successful!'
            
            COMMIT TRANSACTION
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION
            SET @Result = 'Something went wrong, No product was created. Try again!'
            PRINT 'Error: ' + ERROR_MESSAGE()
        END CATCH
    END
    ELSE
    BEGIN
        SET @Result = 'Product already exists with article number ' + @ArticleNumber + ' !'
        PRINT 'Product already exists!'
    END
END