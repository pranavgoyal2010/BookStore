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

