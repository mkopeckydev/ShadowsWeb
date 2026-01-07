

drop table Activity
go

create table Activity
(
	Id INT Primary key IDENTITY,
	Caption nvarchar(64),
	Color nvarchar(1)
)
go

delete from Activity;
insert into Activity(Caption,Color) values ('Léèeno kouzlem 1','G');
insert into Activity(Caption,Color) values ('Léèeno kouzlem 2','G');
insert into Activity(Caption,Color) values ('Léèeno zázrakem 1','G');
insert into Activity(Caption,Color) values ('Léèeno zázrakem 2','G');
insert into Activity(Caption,Color) values ('Èerpání z rostlin','B');
insert into Activity(Caption,Color) values ('Èerpání z lesù','B');
insert into Activity(Caption,Color) values ('Èerpání z talismanu','B');
insert into Activity(Caption,Color) values ('Ošetøení lehkých zranìní 1','R');
insert into Activity(Caption,Color) values ('Ošetøení lehkých zranìní 1','R');


go



