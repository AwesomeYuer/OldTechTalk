--In('a','b','c') ×ª»»Îª in (select colname from @Table)

D-- ================================
-- Create User-defined Table Type
-- ================================
USE Test
GO
-- Create the data type
CREATE TYPE dbo.MyType AS TABLE 
(
	col1 int NOT NULL, 
	col2 varchar(20) NULL, 
	col3 datetime NULL, 
	PRIMARY KEY (col1)
)
GO


DECLARE @MyTable MyType
INSERT INTO @MyTable(col1,col2,col3)
VALUES (1,'abc','1/1/2000'),
	(2,'def','1/1/2001'),
	(3,'ghi','1/1/2002'),
	(4,'jkl','1/1/2003'),
	(5,'mno','1/1/2004')
	
SELECT * FROM @MyTable 

go
CREATE PROC usp_test @MyTableParam MyType READONLY
as
begin
	select *
	from [Table_1]
	where f2 in(select col1 FROM @MyTableParam)
end	
GO

'Create a local table
Dim table As New DataTable("temp")
Dim col1 As New DataColumn("col1", System.Type.GetType("System.Int32"))
Dim col2 As New DataColumn("col2", System.Type.GetType("System.String"))
Dim col3 As New DataColumn("col3", System.Type.GetType("System.DateTime"))
table.Columns.Add(col1)
table.Columns.Add(col2)
table.Columns.Add(col3)
		
'Populate the table
For i As Integer = 20 To 30
	Dim vals(2) As Object
	vals(0) = i
	vals(1) = Chr(i + 90)
	vals(2) = System.DateTime.Now
	table.Rows.Add(vals)
Next

'Code
'Create a command object that calls the stored proc
Dim command As New SqlCommand("usp_AddRowsToMyTable", conn)
command.CommandType = CommandType.StoredProcedure

'Create a parameter using the new type
Dim param As SqlParameter = command.Parameters.Add("@MyTableParam", SqlDbType.Structured)
command.Parameters.AddWithValue("@UserID", "Kathi")

Code
'Set the value of the parameter
param.Value = table

'Execute the query
command.ExecuteNonQuery()