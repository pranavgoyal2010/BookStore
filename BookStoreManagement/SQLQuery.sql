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
