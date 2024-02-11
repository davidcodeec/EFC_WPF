CREATE PROCEDURE AddOrderItem
    @OrderId int,
    @ArticleNumber nvarchar(200),
    @ProductPriceId int,
    @Quantity int,
    @UnitPrice money,
    @OrderItemId int OUTPUT,
    @Result nvarchar(max) OUTPUT
AS
BEGIN
    -- Check if the order exists
    IF NOT EXISTS (SELECT 1 FROM Orders WHERE OrderId = @OrderId)
    BEGIN
        -- Order does not exist
        SET @Result = 'Order with OrderId ' + CONVERT(nvarchar(50), @OrderId) + ' does not exist.'
        RETURN  -- Exit the stored procedure
    END

    -- Check if the order item already exists
    IF EXISTS (SELECT 1 FROM OrderItems WHERE OrderId = @OrderId AND ArticleNumber = @ArticleNumber)
    BEGIN
        -- Order item already exists
        SET @Result = 'Order item already exists for OrderId ' + CONVERT(nvarchar(50), @OrderId) + ' and ArticleNumber ' + @ArticleNumber
        RETURN  -- Exit the stored procedure
    END

    -- Proceed with adding the order item
    BEGIN TRANSACTION
    BEGIN TRY
        -- Insert the order item
        INSERT INTO OrderItems (OrderId, ArticleNumber, ProductPriceId, Quantity, UnitPrice)
        VALUES (@OrderId, @ArticleNumber, @ProductPriceId, @Quantity, @UnitPrice)

        -- Get the newly inserted OrderItemId
        SET @OrderItemId = SCOPE_IDENTITY()

        -- Commit the transaction
        COMMIT TRANSACTION

        -- Set success message
        SET @Result = 'Order item was added successfully with OrderItemId: ' + CONVERT(nvarchar(50), @OrderItemId)
    END TRY
    BEGIN CATCH
        -- Rollback the transaction in case of an error
        ROLLBACK TRANSACTION

        -- Set error message
        SET @Result = 'Error: ' + ERROR_MESSAGE()
    END CATCH
END