-----------------------------------------
--- script to create Customer database ---
-----------------------------------------

--drop fk (if available)
if object_id(N'FK_CATEGORY_EMPLOYEEID',N'F') is not null
	alter table Category drop constraint fk_category_employeeid;
go

if object_id(N'FK_PRODUCT_CATEGORYID',N'F') is not null
	alter table Product drop constraint fk_product_categoryid;
go

if object_id(N'FK_ORDER_CUSTOMERID',N'F') is not null
	alter table [Order] drop constraint fk_order_customerid;
go

if object_id(N'FK_ORDERPRODUCT_ORDERID',N'F') is not null
	alter table OrderProduct drop constraint fk_orderproduct_orderid;
go

if object_id(N'FK_ORDERPRODUCT_PRODUCTID',N'F') is not null
	alter table OrderProduct drop constraint fk_orderproduct_productid;
go


--drop tables (if available)
if object_id(N'CUSTOMER',N'U') is not null
	drop table customer;
go

if object_id(N'EMPLOYEE',N'U') is not null
	drop table Employee;
go

if object_id(N'CATEGORY',N'U') is not null
	drop table Category;
go

if object_id(N'PRODUCT',N'U') is not null
	drop table Product;
go

if object_id(N'ORDER',N'U') is not null
	drop table [Order];
go

if object_id(N'ORDERPRODUCT',N'U') is not null
	drop table OrderProduct;
go


--create tables
create table Customer (
	Id int identity (1,1) not null,
	Name varchar(50),
	Email varchar(50),
	constraint pk_customer primary key clustered (Id) 
);
go

create table Employee (
	Id int identity (1,1) not null,
	Name varchar(50),
	Email varchar(50),
	constraint pk_employee primary key clustered (Id) 
);
go

create table Category (
	Id int identity (1,1) not null,
	Name varchar(50),
	EmployeeId int,
	constraint pk_category primary key clustered (Id) 
);
go

create table Product (
	Id int identity (1,1) not null,
	Name varchar(50),
	CategoryId int,
	Description varchar(100),
	Price decimal,
	constraint pk_product primary key clustered (Id) 
);
go

create table [Order] (
	Id int identity (1,1) not null,
	Name varchar(50),
	OrderDate date,
	CustomerId int,
	Status varchar(30),
	TotalPrice decimal,
	LastModifiedDate date,
	constraint pk_order primary key clustered (Id) 
);
go

create table OrderProduct (
	OrderId int not null,
	ProductId int not null,
	NumberOfProducts int,
	constraint pk_orderproduct primary key clustered (OrderId, ProductId) 
);
go

--create fk
alter table Category add constraint fk_category_employeeid foreign key ( EmployeeId ) references Employee(Id) on delete cascade;
go

alter table Product add constraint fk_product_categoryid foreign key ( CategoryId ) references Category(Id) on delete cascade;
go

alter table [Order] add constraint fk_order_customerid foreign key ( CustomerId ) references Customer(Id) on delete cascade;
go

alter table OrderProduct add constraint fk_orderproduct_orderid foreign key ( OrderId ) references [Order](Id) on delete cascade;
go

alter table OrderProduct add constraint fk_orderproduct_productid foreign key ( ProductId ) references Product(Id) on delete cascade;
go

--fill in values
insert into Customer (Name, Email) values ('Alex', 'alex@wantsome.com');
go
insert into Customer (Name, Email) values ('Tibi', 'tibi@gmail.com');
go
insert into Customer (Name, Email) values ('John', 'john@wantsome.com');
go
insert into Customer (Name, Email) values ('Vali', 'vali@yahoo.com');
go

insert into Employee (Name, Email) values ('Emp A', 'empa@wantsome.com');
go
insert into Employee (Name, Email) values ('Emp B', 'empb@wantsome.com');
go
insert into Employee (Name, Email) values ('Emp C', 'empc@gmail.com');
go
insert into Employee (Name, Email) values ('Emp D', 'empd@wantsome.com');
go
insert into Employee (Name, Email) values ('Emp E', 'empe@yahoo.com');
go

insert into Category (Name, EmployeeId) select 'Clothes', Id from Employee where Name = 'Emp B';
go
insert into Category (Name, EmployeeId) select 'Electronics', Id from Employee where Name = 'Emp D';
go
insert into Category (Name, EmployeeId) select 'Furniture', Id from Employee where Name = 'Emp E';
go

insert into Product (Name, CategoryId, Description, Price) select 'TV Samsung LCD2400', Id, 'TV Samsung LCD2400 desc', 350 from Category where Name = 'Electronics';
go
insert into Product (Name, CategoryId, Description, Price) select 'Mobile Huawei P9', Id, 'Mobile Huawei P9 desc', 280 from Category where Name = 'Electronics';
go
insert into Product (Name, CategoryId, Description, Price) select 'Sony Headphones Y15 White', Id, 'Sony Headphones Y15 White desc', 40 from Category where Name = 'Electronics';
go
insert into Product (Name, CategoryId, Description, Price) select 'Laptop Acer Ideapad5200', Id, 'Laptop Acer Ideapad5200 desc', 930 from Category where Name = 'Electronics';
go
insert into Product (Name, CategoryId, Description, Price) select 'Shirt A5420 Black S', Id, 'Shirt A5420 Black S desc', 38 from Category where Name = 'Clothes';
go
insert into Product (Name, CategoryId, Description, Price) select 'Jacket R2451 Red XL', Id, 'Jacket R2451 Red XL', 85 from Category where Name = 'Clothes';
go

insert into [Order] (Name, OrderDate, CustomerId, Status, LastModifiedDate) select 'Order A01', GETDATE(), Id, 'new', GETDATE() from Customer where Name = 'Alex';
go
insert into [Order] (Name, OrderDate, CustomerId, Status, LastModifiedDate) select 'Order A02', GETDATE(), Id, 'new', GETDATE() from Customer where Name = 'John';
go
insert into [Order] (Name, OrderDate, CustomerId, Status, LastModifiedDate) select 'Order A03', GETDATE(), Id, 'new', GETDATE() from Customer where Name = 'Vali';
go

insert into OrderProduct (OrderId, ProductId, NumberOfProducts) select o.Id, p.Id, 1 from [Order] o join Product p on (1=1) where o.Name = 'Order A01' and p.Name = 'TV Samsung LCD2400';
go
insert into OrderProduct (OrderId, ProductId, NumberOfProducts) select o.Id, p.Id, 3 from [Order] o join Product p on (1=1) where o.Name = 'Order A01' and p.Name = 'Mobile Huawei P9';
go
insert into OrderProduct (OrderId, ProductId, NumberOfProducts) select o.Id, p.Id, 7 from [Order] o join Product p on (1=1) where o.Name = 'Order A01' and p.Name = 'Sony Headphones Y15 White';
go
insert into OrderProduct (OrderId, ProductId, NumberOfProducts) select o.Id, p.Id, 2 from [Order] o join Product p on (1=1) where o.Name = 'Order A02' and p.Name = 'TV Samsung LCD2400';
go
