CREATE PROCEDURE GetOrCreateProductImageId 
	@ImagePath nvarchar(max),
	@ProductImageId int OUTPUT
AS

BEGIN
	-- For Product Images
	
	SELECT @ProductImageId = ProductImageId FROM ProductImages WHERE ImagePath = @ImagePath

	IF @ProductImageId IS NULL
	BEGIN
		INSERT INTO ProductImages (ImagePath)  
		VALUES (@ImagePath)
		SELECT @ProductImageId = SCOPE_IDENTITY()
	END
END