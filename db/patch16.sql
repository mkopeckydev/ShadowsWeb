alter table Character add ClientMessage nvarchar(64);
go
update Character set ClientMessage = '';
go
