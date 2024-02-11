CREATE PROCEDURE AddOrder
    @CustomerId int,
    @OrderDate datetime2 = NULL,
    @ShippedDate datetime2 = NULL,
    @TotalAmount money = NULL,
    @OrderItemId int OUTPUT, -- Optional: if you want to return the OrderItemId
    @Result nvarchar(max) OUTPUT
AS
BEGIN
    -- Check if the customer exists
    IF EXISTS (SELECT 1 FROM Customers WHERE CustomerId = @CustomerId)
    BEGIN
        -- Check if the order already exists
        IF EXISTS (SELECT 1 FROM Orders WHERE CustomerId = @CustomerId AND OrderDate = @OrderDate AND ShippedDate = @ShippedDate AND TotalAmount = @TotalAmount)
        BEGIN
            SET @Result = 'Order already exists for CustomerId ' + CONVERT(nvarchar(50), @CustomerId) + ' with OrderDate ' + CONVERT(nvarchar(50), @OrderDate) + ', ShippedDate ' + CONVERT(nvarchar(50), @ShippedDate) + ', and TotalAmount ' + CONVERT(nvarchar(50), @TotalAmount)
            RETURN
        END

        BEGIN TRANSACTION
        BEGIN TRY
            -- Set OrderDate to current date and time if not provided
            IF @OrderDate IS NULL
                SET @OrderDate = GETDATE()

            -- Set ShippedDate to OrderDate if not provided
            IF @ShippedDate IS NULL
                SET @ShippedDate = @OrderDate

            -- Insert the order
            INSERT INTO Orders (CustomerId, OrderDate, ShippedDate, TotalAmount)
            VALUES (@CustomerId, @OrderDate, @ShippedDate, @TotalAmount)

            -- Get the newly inserted OrderId
            DECLARE @OrderId int = SCOPE_IDENTITY()

            -- Call AddOrderItem procedure to add order items
            -- Example: AddOrderItem @OrderId, 'ArticleNumber', ProductPriceId, Quantity, UnitPrice, @OrderItemId OUTPUT, @Result OUTPUT

            -- Commit the transaction
            COMMIT TRANSACTION

            -- Set success message
            SET @Result = 'Order was created successfully with OrderId: ' + CONVERT(nvarchar(50), @OrderId)
        END TRY
        BEGIN CATCH
            -- Rollback the transaction in case of an error
            ROLLBACK TRANSACTION

            -- Set error message
            SET @Result = 'Error: ' + ERROR_MESSAGE()
        END CATCH
    END
    ELSE
    BEGIN
        -- Customer does not exist
        SET @Result = 'Customer with CustomerId ' + CONVERT(nvarchar(50), @CustomerId) + ' does not exist.'
    END
END