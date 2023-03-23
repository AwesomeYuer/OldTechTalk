	<%@ Page ValidateRequest="false"  %>
	<script language="C#" runat="server">
	protected void Page_Load(object sender, EventArgs ea) 
	{
		Response.Write("Request.Cookies:<br>");
		foreach (string var in Request.Cookies.AllKeys)
		{
			Response.Write(string.Format("&nbsp;&nbsp;&nbsp;&nbsp;{0}:{1}<br>", var, Request.Cookies[var].Value));
		}
	}
	</script>