drop table CharacterClass;
drop table Skill;
drop table SkillLevel;

create table CharacterClass
(
	Id INT Primary key,
	Caption nvarchar(32)
)
go
create table Skill
(
	Id INT Primary key,
	Caption nvarchar(32)
)
go
create table SkillLevel
(
	Id INT Primary key,
	Caption nvarchar(32),
	Bonus int
)
go

INSERT INTO CharacterClass (Id,Caption) VALUES (1,'Váleèník');
INSERT INTO CharacterClass (Id,Caption) VALUES (2,'Kouzelník');
INSERT INTO CharacterClass (Id,Caption) VALUES (3,'Zlodìj');
INSERT INTO CharacterClass (Id,Caption) VALUES (4,'Hranièáø');
INSERT INTO CharacterClass (Id,Caption) VALUES (5,'Klerik');
INSERT INTO CharacterClass (Id,Caption) VALUES (6,'Alchymista');

INSERT INTO Skill (Id,Caption) VALUES (1,'Botanika');
INSERT INTO Skill (Id,Caption) VALUES (2,'Zoologie');
INSERT INTO Skill (Id,Caption) VALUES (3,'Okultismus');
INSERT INTO Skill (Id,Caption) VALUES (4,'Sociologie');
INSERT INTO Skill (Id,Caption) VALUES (5,'Bestiologie');

INSERT INTO SkillLevel (Id,Caption,Bonus) VALUES (1,'Vùbec', -10);
INSERT INTO SkillLevel (Id,Caption,Bonus) VALUES (2,'Špatnì', -7);
INSERT INTO SkillLevel (Id,Caption,Bonus) VALUES (3,'Prùmìrnì', -4);
INSERT INTO SkillLevel (Id,Caption,Bonus) VALUES (4,'Dobøe', 0);
INSERT INTO SkillLevel (Id,Caption,Bonus) VALUES (5,'Velmi dobøe', +3);
INSERT INTO SkillLevel (Id,Caption,Bonus) VALUES (6,'Dokonale', +5);
go 

drop table Character;
drop table CharacterSkillLevel;
go

create table Character
(
	Id INT IDENTITY Primary key,
	Name nvarchar(32),
	Password nvarchar(32),
	IdCharacterClass INT,
	IntelligenceBonus INT,
	Administrator BIT
)
go

create table CharacterSkillLevel
(
	Id INT IDENTITY Primary key,
	IdCharacter INT,
	IdSkill INT,
	IdSkillLevel INT,
	BonusLevel INT
)
go

drop table Item;
go


create table Item
(
	Id INT IDENTITY Primary key,
	IdSkill INT,
	ImageName nvarchar(128),
	Caption nvarchar(128)
)
go

/*insert into Item(IdSkill, ImageName, Caption) values (1, '', 'Pampeliška');
insert into Item(IdSkill, ImageName, Caption) values (1, '', 'Jitrocel');
insert into Item(IdSkill, ImageName, Caption) values (1, '', 'Ostropestøec');*/
go


drop table ItemRow;
go

create table ItemRow
(
	Id INT IDENTITY Primary key,
	IdItem INT,
	InfoLevel int,
	InfoCaption nvarchar(256),
	InfoText text,
	IdCharacterClass int
)
go

/*insert into ItemRow(IdItem, InfoLevel, InfoText, IdCharacterClass) values (1, 3, 'Pampeliška 1', 0);
insert into ItemRow(IdItem, InfoLevel, InfoText, IdCharacterClass) values (1, 6, 'Pampeliška 2a', 2);
insert into ItemRow(IdItem, InfoLevel, InfoText, IdCharacterClass) values (1, 6, 'Pampeliška 2b', 2);
insert into ItemRow(IdItem, InfoLevel, InfoText, IdCharacterClass) values (1, 9, 'Pampeliška 3', 0);*/
go

drop table CharacterItemValue;
go

create table CharacterItemValue
(
	Id INT IDENTITY Primary key,
	IdCharacter INT,
	IdItem int,
	CharacterValue int,
	CharacterBonus int
)
go

--insert into CharacterItemValue(IdCharacter, IdItem, CharacterValue, CharacterBonus) values (1, 1, 4, 2);
go