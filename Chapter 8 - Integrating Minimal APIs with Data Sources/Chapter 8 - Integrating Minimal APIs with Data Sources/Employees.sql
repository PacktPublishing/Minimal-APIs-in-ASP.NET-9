CREATE TABLE dbo.Employees 
( 
	Id int NOT NULL IDENTITY (1, 1), 
	Name varchar(MAX) NOT NULL, 
	Salary decimal(10, 2) NOT NULL, 
	Address varchar(MAX) NOT NULL, 
	City varchar(50) NOT NULL, 
	Region varchar(50) NOT NULL, 
	Country varchar(50) NOT NULL, 
	Phone varchar(200) NOT NULL 
)   