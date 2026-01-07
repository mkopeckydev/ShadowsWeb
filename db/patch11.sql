/*

alter table Character add IsActive bit;
go
update Character set IsActive = 0;
go

*/
go





drop table Activity
go

create table Activity
(
	Id INT Primary key IDENTITY,
	Caption nvarchar(64)
)
go

insert into Activity(Caption) values ('Léèení kouzlem #1');
insert into Activity(Caption) values ('Léèení kouzlem #2');
insert into Activity(Caption) values ('Léèení zázrakem #1');
insert into Activity(Caption) values ('Léèení zázrakem #2');
insert into Activity(Caption) values ('Èerpání z rostlin');
insert into Activity(Caption) values ('Èerpání z lesù');
insert into Activity(Caption) values ('Èerpání z talismanu');
go

drop table CharacterActivity
go

create table CharacterActivity
(
	Id INT Primary key IDENTITY,
	IdCharacter int,
	IdActivity int,
	IsSelected bit
)
go


