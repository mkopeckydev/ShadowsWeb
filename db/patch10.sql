drop table FightNumber
go

create table FightNumber
(
	Id INT Primary key IDENTITY,
	IdCharacter int,
	Caption nvarchar(64),
	BaseValueCaption1 nvarchar(16),
	BaseValue1 int,
	BaseValueCaption2 nvarchar(16),
	BaseValue2 int,
	BaseValueCaption3 nvarchar(16),
	BaseValue3 int,
	BaseValueCaption4 nvarchar(16),
	BaseValue4 int,
	BaseValueCaption5 nvarchar(16),
	BaseValue5 int,
	Ordering int
)
go

drop table FightNumberBonus
go

create table FightNumberBonus
(
	Id INT Primary key IDENTITY,
	IdFightNumber int,
	Caption nvarchar(64),
	Bonus1 int,
	Bonus2 int,
	Bonus3 int,
	Bonus4 int,
	Bonus5 int,
	IsSelected bit
)
go


