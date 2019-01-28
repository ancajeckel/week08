----------------------------------------
-----         Exercitii          -------
----------------------------------------

--Afisati toate produsele
select * from Product;
go

--Afisati toti clientii care au in continutul email-ului @wantsome.com
select * from Customer where Email like '%wantsome.com%';
go

--Afisati suma preturilor pentru fiecare categorie in parte
select p.CategoryId, c.Name as CategoryName, sum(Price) as SumPricePerCategory
  from Product p
  join Category c on (p.CategoryId = c.Id)
 group by p.CategoryId, c.Name;
go

--Afisati clientii care au mai mult de 10 comenzi
select c.Id as CustomerId, c.Name as CustomerName, count(o.Id) as CountOrder
  from Customer c
  join [Order] o on c.Id = o.CustomerId
  group by c.Id, c.Name
  having count(o.Id) > 10;
go

--Creati un view care va afisa toti clientii si produsele comandate de acestia
if object_id(N'VIWCUSTOMERORDEREDPRODUCTS',N'V') is not null
	drop view ViwCustomerOrderedProducts;
go

create view ViwCustomerOrderedProducts as
 select o.CustomerId, c.Name as CustomerName, o.Id as OrderId, o.OrderDate, o.TotalPrice, op.ProductId, p.Name as ProductName, p.CategoryId, g.Name as CategoryName, op.NumberOfProducts
   from Customer c
   join [Order] o on c.Id = o.CustomerId
   join OrderProduct op on o.Id = op.OrderId
   join Product p on op.ProductId = p.Id
   join Category g on p.CategoryId = g.Id;
go

--Folositi view-ul de la punctul precedent pentru a afisa clientii care au comandat produse in primele trei luni ale anului
select *
  from ViwCustomerOrderedProducts
 where OrderDate between '2019-01-01' and '2019-03-31';
go

--Folositi view-ul de la punctul precedent pentru a afisa clientii care au comandat produse dintr-o anumita categorie
select *
  from ViwCustomerOrderedProducts
 where CategoryName = 'Electronics';

--Creati o procedura care va modifica statusul unui Order; aceasta procedura va updata si LastModifiedDate
if object_id(N'PRCUPDATEORDERSTATUS',N'P') is not null
	drop procedure PrcUpdateOrderStatus;
go

create procedure PrcUpdateOrderStatus @OrderId int, @NewStatus varchar(20)
as
begin 
	begin tran T1
	declare @TotPrice decimal;
	select @TotPrice = sum(op.NumberOfProducts * p.Price)
	  from OrderProduct op
	  join [Order] o on op.OrderId = o.Id
	  join Product p on op.ProductId = p.Id
	 where op.OrderId = @OrderId;
	update [Order] set Status = @NewStatus, TotalPrice = @TotPrice, LastModifiedDate = GETDATE() where Id = @OrderId;
	commit tran T1 
end
go

exec PrcUpdateOrderStatus 1, 'confirmed';
go

select * from [Order] where Id = 1;
go

--Creati un raport (select cu group by) pentru a afisa vanzarile pentru fiecare produs in parte
  select p.Id as ProductId, p.Name as ProductName, 
         sum(coalesce(opo.NumberOfProducts,0)) as TotalNumberOfProducts, 
		 sum(p.Price * coalesce(opo.NumberOfProducts,0)) as TotalValue
    from Product p
	left join (select * from OrderProduct op join [Order] o on op.OrderId = o.Id) opo on opo.ProductId = p.Id
   group by p.Id, p.Name
   order by 1;

--Creati o functie care va calcula pretul total pentru o anumita comanda
if object_id(N'FCTGETORDERTOTALPRICE',N'FN') is not null
	drop function FctGetOrderTotalPrice;
go

create function FctGetOrderTotalPrice (@OrderId int)
returns decimal
as
begin
	declare @ret decimal;
	select @ret = sum(op.NumberOfProducts * p.Price)
	  from OrderProduct op
	  join [Order] o on op.OrderId = o.Id
	  join Product p on op.ProductId = p.Id
	 where op.OrderId = @OrderId;
	if (@ret is null)   
		set @ret = 0;  
	return @ret;
end
go

select dbo.FctGetOrderTotalPrice(1);
go


--Order Audit Table - OrderId, CustomerId, CreateDate, ApprovedDate -> insert trigger
if object_id(N'ORDERAUDIT',N'U') is not null
	drop table OrderAudit;
go

create table OrderAudit (
	OrderId int not null,
	CustomerId int,
	CreateDate date,
	ApprovedDate date,
	constraint pk_orderaudit primary key clustered (OrderId) 
);
go

if object_id(N'TRGORDERINS',N'TR') is not null
	drop trigger TrgOrderIns;
go

create trigger TrgOrderIns on [Order]
for insert
as 
begin
	insert into OrderAudit(OrderId, CustomerId, CreateDate)
		select i.Id, i.CustomerId, GETDATE() from inserted i;
end
go

insert into [Order] (Name, OrderDate, CustomerId, Status, LastModifiedDate) select 'Order B01', GETDATE(), Id, 'new', GETDATE() from Customer where Name = 'Tibi';
go

select * from OrderAudit;
go


--Order Audit: cand order-ul are status approved = update pe coloana approvedat in audit table -> update trigger
if object_id(N'TRGORDERUPD',N'TR') is not null
	drop trigger TrgOrderUpd;
go

create trigger TrgOrderUpd on [Order]
for update
as 
begin
	declare @updStatus varchar(50);
	select @updStatus = [Status] from inserted i;
	if lower(@updStatus) = 'approved'
		update OrderAudit set ApprovedDate = GETDATE()
		  from inserted i 
		  join [Order] o on o.Id = i.Id;
end
go

update [Order] set Status = 'Approved' where Id = (select Id from [Order] where Name = 'Order B01');
go

select * from OrderAudit;
go
