--LibraryApp DB script
-------------------------------------------------------

--drop fk
alter table [Book] drop constraint fk_book_publisherid;
go

--drop tables
drop table [Publisher];
go

drop table [Book];
go

--Publisher Table:
CREATE TABLE [Publisher](
    [PublisherId] [int] NOT NULL IDENTITY(1,1),
    [Name] [varchar](50) NULL,
    CONSTRAINT [pk_publisher] PRIMARY KEY CLUSTERED
    (
        [PublisherId] ASC
    ) WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
);
GO

--Book Table:
CREATE TABLE [Book](
       [BookId] [int] NOT NULL PRIMARY KEY IDENTITY(1,1),
       [Title] [varchar](50) NULL,
       [PublisherId] [int] NULL,
       [Year] [int] NULL,
       [Price] [decimal](18, 0) NULL
       )
GO
ALTER TABLE [Book]  WITH CHECK ADD  CONSTRAINT [fk_book_publisherid] FOREIGN KEY([PublisherId])
REFERENCES [Publisher] ([PublisherId])
ON DELETE CASCADE;
GO
ALTER TABLE [Book] CHECK CONSTRAINT [fk_book_publisherid];
GO

--Insert some data in those tables
insert into Publisher (Name) values ('Polirom');
go
insert into Publisher (Name) values ('For you');
go
insert into Publisher (Name) values ('Litera');
go
insert into Publisher (Name) values ('Nemira');
go
insert into Publisher (Name) values ('Rao');
go
insert into Publisher (Name) values ('Trei');
go
insert into Publisher (Name) values ('Asa');
go
insert into Publisher (Name) values ('Art');
go


insert into Book (Title, PublisherId, [Year], Price) values ('De veghe in lanul de secara', 1, 2016, 17);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Fluturi', 2, 2016, 13);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Proza', 3, 2011, 8);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Portretul lui Dorian Grey', 1, 2013, 18);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Urzeala tronurilor', 4, 2017, 30);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Numele vantului', 5, 2017, 35);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Cartea vietii', 3, 2017, 24);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Chimista', 6, 2016, 33);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Baltagul', 7, 2014, 22);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Harry Potter vol. 5', 8, 2017, 64);
go
insert into Book (Title, PublisherId, [Year], Price) values ('Puterea armelor', 6, 2017, 35);
go
