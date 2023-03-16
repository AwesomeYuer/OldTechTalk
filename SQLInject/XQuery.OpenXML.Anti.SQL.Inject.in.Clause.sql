--建测试表
create table #testTable (UserID int,UserName varchar(50))
--插测试数据
insert into #testTable(UserID,UserName) values (1,'aaa')
insert into #testTable(UserID,UserName) values (2,'aaa')
insert into #testTable(UserID,UserName) values (3,'aaa')
insert into #testTable(UserID,UserName) values (4,'aaa')

declare @ids xml
set @ids = cast('<ids><id>1</id><id>2</id><id>3</id></ids>' as xml)

select A.* from TestUser A
	inner join
	(
		select X.c.value('.', 'int') as UserID 
			from @ids.nodes('/ids/id') as X(c)
	) B on A.UserID = B.UserID 
	
	
drop table #testTable