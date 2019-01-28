create procedure DeletePubliser @PublisherId int
as
begin
	delete from Book where PublisherId = @PublisherId;
	delete from Publisher where PublisherId = @PublisherId;
end