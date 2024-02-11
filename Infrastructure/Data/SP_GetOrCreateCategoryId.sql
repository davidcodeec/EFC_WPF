CREATE PROCEDURE GetOrCreateCategoryId 
	@CategoryName nvarchar(50),
	@CategoryId int OUTPUT
AS

BEGIN
	-- For Categories
	
	SELECT @CategoryId = CategoryId FROM Categories WHERE CategoryName = @CategoryName

	IF @CategoryId IS NULL
	BEGIN
		INSERT INTO Categories (CategoryName)  
		VALUES (@CategoryName)
		SELECT @CategoryId = SCOPE_IDENTITY()
	END
END