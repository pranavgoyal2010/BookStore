CREATE TABLE [User] (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(255),
    LastName VARCHAR(255),
    MobileNo VARCHAR(20),
    Email VARCHAR(255),
    Password VARCHAR(100),
    AddedOn DATETIME,
    UpdatedOn DATETIME,
    Role VARCHAR(50)
);

CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    BookName VARCHAR(255),
    Description VARCHAR(MAX),
    Author VARCHAR(255),
    Price DECIMAL(10, 2),
    BookImage VARCHAR(MAX),
    Quantity INT,
    AddedOn DATETIME,
    UpdatedOn DATETIME
);

CREATE TABLE Cart (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT,
    FOREIGN KEY (UserId) REFERENCES [User](UserId)
)

CREATE TABLE CartItem (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT,
    BookId INT,
    Quantity INT,
    Price DECIMAL(10, 2),
    FOREIGN KEY (CartId) REFERENCES Cart(CartId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId)
)