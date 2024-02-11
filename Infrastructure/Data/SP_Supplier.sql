CREATE PROCEDURE GetOrCreateSupplierId 
	@SupplierName nvarchar(50),
	@SupplierNumber nvarchar(50),
	@ContactPerson nvarchar(50),
    @ContactEmail nvarchar(50),
    @ContactPhone nvarchar(50),
	@SupplierId int OUTPUT
AS

BEGIN
	-- For Suppliers
	
	SELECT @SupplierId = SupplierId FROM Suppliers WHERE SupplierName = @SupplierName

	IF @SupplierId IS NULL
	BEGIN
		INSERT INTO Suppliers (SupplierName, SupplierNumber, ContactPerson, ContactEmail, ContactPhone)  
		VALUES (@SupplierName, @SupplierNumber, @ContactPerson, @ContactEmail, @ContactPhone)
		SELECT @SupplierId = SCOPE_IDENTITY()
	END
END