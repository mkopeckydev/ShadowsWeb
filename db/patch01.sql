alter table Character add IdCharacterClass2 int;
go
alter table ItemRow add IdSkillLevel int;
go
update Character set IdCharacterClass2 = 0;
update ItemRow set IdSkillLevel = 1;
go