//test.aspx
<%@ Page language="c#" AutoEventWireup ="true" debug="true"%>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
    <HEAD>
        <title>WebForm1</title>
        <meta name="generator" content="editplus" />
        <meta name="author" content="" />
        <meta name="keywords" content="" />
        <meta name="description" content="" />
    <script language="C#" runat="server">
    protected void Page_Load(object sender, EventArgs ea) 
    {
		Response.Write("\u003cscript\u003ealert\u0028\u0022HL29999 Encode script XSS \u0041ttack\u0022\u0029\u003c/script\u003e");
        //h1.Text += "<script>alert('HL Xss Attack')" + "</scr" + "ipt>";
        h1.Text += //HttpUtility.HtmlEncode
                     //               (
                                        "\u003cscript\u003ealert\u0028\u0022HL255555 Encode script XSS \u0041ttack\u0022\u0029\u003c/script\u003e"
                       //             )
                                    ;
        DataTable dt = MakeTable("F1","F2");
        string expression = "1 = 1 or f1 = '2'"; 
        DataView dv = dt.DefaultView; 
        dv.Sort = "f1 desc"; 
        dv.RowFilter = expression; 
        datagrid1.DataSource = dv;
        datagrid1.DataBind();
        gridview1.DataSource = dv;
        gridview1.DataBind();
        TextBox tb = new TextBox();
        tb.Text = "Dynamic TextBox";
        p1.Controls.Add(tb);
    }
    void datagrid1_ItemDataBound(object sender, DataGridItemEventArgs e) 
    {
        foreach (TableCell cell in e.Item.Cells)
        {
            if (cell.Text != string.Empty)
            {
                //cell.Text = HttpUtility.HtmlEncode(cell.Text);
            }
        }
    }
    void gridview1_RowDataBound(object sender, GridViewRowEventArgs e) 
    {
///        foreach (TableCell cell in e.Row.Cells)
///        {
///            if (cell.Text != string.Empty)
///            {
///                cell.Text = HttpUtility.HtmlEncode(cell.Text);
///            }
///        }
    }
    private static DataTable MakeTable
                        (
                            string c1Name
                            , string c2Name
                        )
    {
        DataTable table= new DataTable();
        DataColumn column = new DataColumn(c1Name, typeof(int));
        table.Columns.Add(column);
        column = new DataColumn(c2Name, typeof(string));
        table.Columns.Add(column);
        table.Rows.Add(1,"<script>alert('datagrid xss attack')</scr" + "ipt>");
        table.Rows.Add(2, "\u003c" + "scr" + "ipt\u003ealert\u0028\u0022gridview XSS \u0041ttack\u0022\u0029\u003c/script\u003e");
        return table;
    }
    </script>
    </HEAD>
    <body>
        <form id="Form1" method="post" runat="server">
            <asp:TextBox ID="tb1" Text="Static TextBox" runat="server" />
            <asp:Label ID="l1" Text="Static Label" runat="server" />
            <asp:Panel id="p1" runat="server"/>
            <asp:Hyperlink ID="h1" NavigateUrl="http://www.bing.com" runat="server">
                     www.Bing.com Hyperlink<script>alert('Hyper Link Control Xss Attack')</script>
            </asp:Hyperlink>
            <input ID="Value2" Type="Text" Value="Static HtmlControls HtmlInputText" runat="server"/>
            <asp:ListBox ID="lb1" Width="" runat="server">
                <asp:ListItem><script>alert('ListBox Xss Attack')</script></asp:ListItem>
                <asp:ListItem></asp:ListItem>
            </asp:ListBox>
            <ASP:DataGrid ID="datagrid1" runat="server"
                AutoGenerateColumns="True"
                OnItemDataBound = "datagrid1_ItemDataBound"
            />
            <ASP:GridView ID="gridview1" runat="server"
                AutoGenerateColumns="True"
                OnRowDataBound = "gridview1_RowDataBound"
            />
        </form>
    </body>
</HTML>