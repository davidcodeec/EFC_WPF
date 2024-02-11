CREATE PROCEDURE AddCustomer
    @FirstName nvarchar(50),
    @LastName nvarchar(50),
    @Email nvarchar(200),
    @StreetName nvarchar(200),
    @StreetNumber varchar(20),
    @PostalCode nvarchar(20),
    @City nvarchar(50),
    @PhoneNumber varchar(30),
    @OrderDate datetime2 = NULL,
    @ShippedDate datetime2 = NULL,
    @TotalAmount money = NULL,
    @Result nvarchar(max) OUTPUT
AS
BEGIN
    DECLARE @CustomerId int
    DECLARE @AddressId int
    DECLARE @PhoneId int
    DECLARE @OrderId int

    -- Check if customer already exists
    IF NOT EXISTS (SELECT 1 FROM Customers WHERE Email = @Email)
    BEGIN
        -- Customer does not exist, proceed with customer creation
        BEGIN TRANSACTION
        BEGIN TRY
            -- Get or create address
            EXEC GetOrCreateCustomerAddressId @StreetName, @StreetNumber, @PostalCode, @City, @AddressId OUTPUT

            -- Insert customer information into Customers table
            INSERT INTO Customers (FirstName, LastName, Email)
            VALUES (@FirstName, @LastName, @Email)

            -- Get the newly inserted CustomerId
            SET @CustomerId = SCOPE_IDENTITY()

            -- Insert customer-address relationship into CustomersAddresses table
            INSERT INTO CustomersAddresses (CustomerId, AddressId)
            VALUES (@CustomerId, @AddressId)

            -- Get or create phone number
            EXEC GetOrCreateCustomerPhoneNumberId @CustomerId, @PhoneNumber, @PhoneId OUTPUT

            -- Insert order for the customer
            EXEC AddOrder @CustomerId, @OrderDate, @ShippedDate, @TotalAmount, @OrderId OUTPUT, @Result OUTPUT

            -- Commit the transaction
            COMMIT TRANSACTION
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
        -- Customer already exists with the provided email
        SET @Result = 'Customer already exists with email: '
    END
END