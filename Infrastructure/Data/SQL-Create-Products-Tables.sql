DROP TABLE OrderItems;
DROP TABLE Orders;
DROP TABLE ProductCategories;
DROP TABLE ProductPrices;
DROP TABLE Products;
DROP TABLE CustomersAddresses;
DROP TABLE CustomersPhoneNumbers;
DROP TABLE ProductImages;
DROP TABLE Categories;
DROP TABLE Addresses;
DROP TABLE Customers;
DROP TABLE Suppliers;

CREATE TABLE Suppliers
(
	SupplierId int not null identity primary key,
	SupplierName nvarchar(50) not null,
	SupplierNumber nvarchar(50) not null,
	ContactPerson nvarchar(50) not null,
	SupplierEmail nvarchar(50) not null,
	SupplierPhone varchar(50) not null
);

CREATE TABLE Customers
(
	CustomerId int not null identity primary key,
	FirstName nvarchar(50) not null,
	LastName nvarchar(50) not null,
	Email nvarchar(200) not null unique,
    	PhoneNumber varchar(30) null
);

CREATE TABLE Addresses
(
	AddressId int not null identity primary key,
	StreetName nvarchar(200) not null,
	StreetNumber varchar(20) not null,
	PostalCode varchar(20) not null,
	City nvarchar(50) not null
);

CREATE TABLE Categories
(
	CategoryId int not null identity primary key,
	CategoryName nvarchar(100) not null unique
);

CREATE TABLE ProductImages
(
	ProductImageId int not null identity primary key,
	ImagePath nvarchar(max)
);

CREATE TABLE CustomersPhoneNumbers
(
    CustomerId int not null,
    PhoneNumber varchar(30) not null,
    primary key (CustomerId, PhoneNumber)
);


CREATE TABLE CustomersAddresses
(
	CustomerAddressId int not null identity primary key,
	CustomerId int not null references Customers(CustomerId),
	AddressId int not null references Addresses(AddressId),
	unique (CustomerId, AddressId)
);

CREATE TABLE Products
(
	ArticleNumber nvarchar(200) not null primary key,
	ProductName nvarchar(200) not null,
	Ingress nvarchar(200) null,
	Description nvarchar(max) null,
	UnitPrice money not null,
	CategoryId int not null references Categories(CategoryId),
	SupplierId int not null references Suppliers(SupplierId),
	ProductImageId int references ProductImages(ProductImageId) null
);

CREATE TABLE ProductPrices
(
	ProductPriceId int not null primary key identity,
	ArticleNumber nvarchar(200) not null references Products(ArticleNumber),
	UnitPrice money not null,
	DiscountPrice money null
);

CREATE TABLE ProductCategories
(
    ProductCategoryId int not null identity primary key,
    ArticleNumber nvarchar(200) not null references Products(ArticleNumber),
    CategoryId int not null references Categories(CategoryId),
    unique (ArticleNumber, CategoryId)
);


CREATE TABLE Orders
(
	OrderId int not null primary key identity,
	CustomerId int not null references Customers(CustomerId),
	OrderDate datetime2 null default GETDATE(),
	ShippedDate datetime2 null default GETDATE(),
	TotalAmount money null
);

CREATE TABLE OrderItems
(
	OrderItemId int not null identity,
	OrderId int not null references Orders(OrderId),
	ArticleNumber nvarchar(200) not null references Products(ArticleNumber),
	ProductPriceId int not null references ProductPrices(ProductPriceId),
	Quantity int not null,
	UnitPrice money not null,
	primary key (OrderItemId, OrderId)
);