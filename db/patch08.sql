create table SpellbookItemNote
(
	Id INT Primary key IDENTITY,
	IdSpellbookItem int,
	IdCharacter int,
	Note text
)
go


