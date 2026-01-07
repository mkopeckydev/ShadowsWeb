update ItemRow set InfoText = REPLACE(cast(InfoText as nvarchar(max)), CHAR(09), '') where InfoText like '%' + CHAR(09) + '%';

update SpellbookItem set Attributes = REPLACE(cast(Attributes as nvarchar(max)), CHAR(09), '') where Attributes like '%' + CHAR(09) + '%';
update SpellbookItem set Description = REPLACE(cast(Description as nvarchar(max)), CHAR(09), '') where Description like '%' + CHAR(09) + '%';
update SpellbookItem set Modification = REPLACE(cast(Modification as nvarchar(max)), CHAR(09), '') where Modification like '%' + CHAR(09) + '%';
update SpellbookItem set ModificationRuneMage = REPLACE(cast(ModificationRuneMage as nvarchar(max)), CHAR(09), '') where ModificationRuneMage like '%' + CHAR(09) + '%';


