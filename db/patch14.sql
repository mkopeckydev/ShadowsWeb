drop table Activity
go

drop table CharacterActivity
go

create table CharacterActivity
(
	Id INT Primary key IDENTITY,
	IdCharacter int,
	Caption nvarchar(64),
	Color nvarchar(64),
	CheckCount int,
	Checked1 bit,
	Checked2 bit,
	Checked3 bit,
	Checked4 bit,
	Checked5 bit
)
go


