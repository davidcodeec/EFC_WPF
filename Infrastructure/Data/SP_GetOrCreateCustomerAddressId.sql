CREATE PROCEDURE GetOrCreateCustomerAddressId 
    @StreetName nvarchar(200),
    @StreetNumber varchar(20),
    @PostalCode nvarchar(20),
    @City nvarchar(50),
    @AddressId int OUTPUT
AS
BEGIN
    -- Check if the address already exists
    SELECT @AddressId = AddressId 
    FROM Addresses 
    WHERE StreetName = @StreetName 
        AND StreetNumber = @StreetNumber 
        AND PostalCode = @PostalCode 
        AND City = @City

    -- If the address doesn't exist, create a new one
    IF @AddressId IS NULL
    BEGIN
        INSERT INTO Addresses (StreetName, StreetNumber, PostalCode, City)  
        VALUES (@StreetName, @StreetNumber, @PostalCode, @City)
        SET @AddressId = SCOPE_IDENTITY() 
    END
END