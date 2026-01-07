alter table Character add IsDead bit;
go
update Character set IsDead = 0;
go