CREATE function [dbo].[zufn_SplitStringToTable]
(
	@Text   varchar(8000),  --���ֲ���ַ���
	@Separator varchar(10) = ','     --���ݷָ���
)RETURNS @Table TABLE(id int,F varchar(100))
AS
/*
select *
from zufn_SplitStringToTable(',,44,,,55,77,77,',',')
*/

BEGIN
set @Text = replace(@Text,' ','')
set @Separator = ','
DECLARE @SeparatorLen int
SET @SeparatorLen=LEN(@Separator+'$')-2
set @Text = replace(@Text,' ','')
declare @i int
set @i = 1
WHILE CHARINDEX(@Separator,@Text )>0
BEGIN
	declare @v varchar(100)
	set @v = (LEFT(@Text ,CHARINDEX(@Separator,@Text )-1))
	INSERT @Table (id,F)
	select @i,@v
	where rtrim(ltrim(@v)) != '' 
			and not exists (select 1 from @Table  where F = @v)
	if @@rowcount > 0
	begin
		set @i = @i + 1
	end
	SET @Text = STUFF(@Text ,1,CHARINDEX(@Separator,@Text )+@SeparatorLen,'')
END
INSERT @Table  (id,F)
select @i,@Text
where rtrim(ltrim(@Text)) != ''
		and not exists (select 1 from @Table where F = @Text)
return
end