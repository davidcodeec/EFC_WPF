CREATE PROCEDURE GetOrCreateCustomerPhoneNumberId 
    @CustomerId int,
    @PhoneNumber varchar(30),
    @PhoneId int OUTPUT
AS
BEGIN
    -- Check if the phone number already exists for the customer
    SELECT @PhoneId = CustomerId 
    FROM CustomersPhoneNumbers 
    WHERE CustomerId = @CustomerId 
        AND PhoneNumber = @PhoneNumber

    -- If the phone number doesn't exist, create a new one
    IF @PhoneId IS NULL
    BEGIN
        INSERT INTO CustomersPhoneNumbers (CustomerId, PhoneNumber)  
        VALUES (@CustomerId, @PhoneNumber)
    END
END