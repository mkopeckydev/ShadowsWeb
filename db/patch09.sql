drop table CustomCounter
go

create table CustomCounter
(
	Id INT Primary key IDENTITY,
	IdCharacter int,
	Caption nvarchar(64),
	ActualValue int,
	MaxValue int,
	Color nvarchar(32)
)
go


