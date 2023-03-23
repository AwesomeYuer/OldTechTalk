<%@ Page language="c#" AutoEventWireup="false"%>
	<%@ Import Namespace="System.Threading" %>
	<%@ Import Namespace="System.Data" %>
	<%@ Import Namespace="System.Data.SqlClient" %>
	<%@ Import Namespace="Microshaoft.DataAccess" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
 <head>
  <title> new document </title>
  <meta name="generator" content="editplus" />
  <meta name="author" content="" />
  <meta name="keywords" content="" />
  <meta name="description" content="" />
 </head>

 <body>
<%
	int l = (1024 * 1024 * 10);
	//Response.AddHeader("Content-Length", l.ToString());
	int i = 0;
	Response.Buffer = false;
	byte[] buffer = new byte[]{1};
	while (1==1)
	{
		Thread.Sleep(300);
		bool b = false;
		DataTable dt = Class1.ExecDataTable();
		foreach (DataRow r in dt.Rows)
		{
			Response.Write(r[0].ToString()+"<br>");
			b = true;
			Response.Flush();
		}
//		if (!b)
//		{
//			Response.BinaryWrite(buffer);
//			Response.Write("no data");
//		}
	}
%>

  
 </body>
</html>