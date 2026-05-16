CREATE PROCEDURE SaveOrUpdateCompany
    @Id INT,
    @Name NVARCHAR(200),
    @Address NVARCHAR(200),
    @TaxNumber NVARCHAR(50),
    @Industry NVARCHAR(100),
    @Website NVARCHAR(200),
    @ContactPerson NVARCHAR(100),
    @Telephone NVARCHAR(50),
    @PrimaryEmail NVARCHAR(100),
    @PAN NVARCHAR(20),
    @VAT NVARCHAR(20),
    @TimeZone NVARCHAR(20),
    @Language NVARCHAR(50),
    @Address1 NVARCHAR(200),
    @Address2 NVARCHAR(200),
    @CreatedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM Companies WHERE Id = @Id AND @Id > 0)
    BEGIN
        UPDATE Companies
        SET
            Name = @Name,
            Address = @Address,
            TaxNumber = @TaxNumber,
            Industry = @Industry,
            Website = @Website,
            ContactPerson = @ContactPerson,
            Telephone = @Telephone,
            PrimaryEmail = @PrimaryEmail,
            PAN = @PAN,
            VAT = @VAT,
            TimeZone = @TimeZone,
            Language = @Language,
            Address1 = @Address1,
            Address2 = @Address2
        WHERE Id = @Id
    END
    ELSE
    BEGIN
        INSERT INTO Companies
        (
            Name, Address, TaxNumber, Industry, Website, ContactPerson, Telephone,
            PrimaryEmail, PAN, VAT, TimeZone, Language, Address1, Address2, CreatedAt
        )
        VALUES
        (
            @Name, @Address, @TaxNumber, @Industry, @Website, @ContactPerson, @Telephone,
            @PrimaryEmail, @PAN, @VAT, @TimeZone, @Language, @Address1, @Address2, @CreatedAt
        )
        SET @Id = SCOPE_IDENTITY()
    END
    SELECT @Id AS Id
END
