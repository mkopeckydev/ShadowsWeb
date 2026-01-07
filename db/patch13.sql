

drop table Activity
go

create table Activity
(
	Id INT Primary key IDENTITY,
	Caption nvarchar(64),
	Color nvarchar(1)
)
go

insert into Activity(Caption,Color) values ('Èerpání z živoèichù','B');
insert into Activity(Caption,Color) values ('Èerpání z harmonie','B');
insert into Activity(Caption,Color) values ('Èerpání z lesù','B');
insert into Activity(Caption,Color) values ('Èerpání z totemu','B');
insert into Activity(Caption,Color) values ('Èerpání z tance','B');
insert into Activity(Caption,Color) values ('Èerpání z rostlin','B');
insert into Activity(Caption,Color) values ('Èerpání z modliteb','B');
insert into Activity(Caption,Color) values ('Èerpání z odevzdání se','B');
insert into Activity(Caption,Color) values ('Èerpání z rituálù','B');
insert into Activity(Caption,Color) values ('Èerpání z odøíkání','B');
insert into Activity(Caption,Color) values ('Èerpání ze vzduchu','B');
insert into Activity(Caption,Color) values ('Èerpání ze zloby','B');
insert into Activity(Caption,Color) values ('Znamení Temnoty','B');
insert into Activity(Caption,Color) values ('Rituály Temnoty','B');
insert into Activity(Caption,Color) values ('Èerpání z hlubin','B');
insert into Activity(Caption,Color) values ('Krvavé èerpání','B');
insert into Activity(Caption,Color) values ('Èerpání z mysli','B');
insert into Activity(Caption,Color) values ('Èerpání ze siloèar','B');
insert into Activity(Caption,Color) values ('Èerpání z ohnì','B');
insert into Activity(Caption,Color) values ('Èerpání z vody','B');
insert into Activity(Caption,Color) values ('Èerpání z tìla','B');
insert into Activity(Caption,Color) values ('Èerpání z anomálií','B');
insert into Activity(Caption,Color) values ('Èerpání ze souznìní','B');
insert into Activity(Caption,Color) values ('Èerpání z pùstu','B');



go

delete from CharacterActivity;
go



