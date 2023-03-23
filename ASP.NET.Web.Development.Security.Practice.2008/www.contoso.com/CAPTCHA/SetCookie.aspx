<%@ Page language="c#" AutoEventWireup="true"%>
	<%@ Import Namespace="System.Threading" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>test</title>
		<meta name="generator" content="editplus" />
		<meta name="author" content="" />
		<meta name="keywords" content="" />
		<meta name="description" content="" />
		
	<script language="C#" runat="server">
	protected void Page_Load(object sender, EventArgs ea) 
	{
		//Response.AddHeader("P3P","CP=NON DSP COR CURa ADMa DEVa TAIa PSAa PSDa IVAa IVDa CONa HISa TELa OTPa OUR UNRa IND UNI COM NAV INT DEM CNT PRE LOC"); 
		//Response.AddHeader("P3P","CP=CAO PSA OUR");
		//Response.AddHeader("P3P","CP=\"IDC DSP COR CURa ADMa OUR IND PHY ONL COM STA\"");
		//Response.AddHeader("Content-Length", (1024 * 1024 * 10).ToString()); 

///		while (true)
///		{
///			Thread.Sleep(500);
///			Response.Write("aaaaa<br>");
///		}
		
		string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
		Response.Write(now + "<br>");
		string cookieName = "yxy";
		if (Request.Cookies[cookieName] == null)
		{
			string s = Request.QueryString["a"] + " at " + now;
			HttpCookie cookie = new HttpCookie(cookieName, s);
			cookie.Domain = ".contoso.com";
			//cookie.Domain = "service.captcha.net";
			Response.Cookies.Add(cookie);
		}

	}
	</script>
	</HEAD>
	<body>
	<br>
	<a href="readcookies.aspx">Same domain ReadCookies</a><br>
	<a href="readcookies.aspx" target="_blank">new win Same domain ReadCookies</a><br>
	<a href="http://site1.contoso.com/captcha/readcookies.aspx">Same main domain ReadCookies</a><br>
	<a href="http://site1.contoso.com/captcha/readcookies.aspx" target="_blank">new win Same main domain ReadCookies</a><br>
	<a href="http://service.captcha.net/ReadCookies.aspx">Cross domain ReadCookies</a><br>
	<a href="http://service.captcha.net/ReadCookies.aspx" target="_blank">new win Cross domain ReadCookies</a><br>
	<iframe cols="100" rows="100" src="readcookies.aspx" /><br>
	<iframe cols="100" rows="100" src="http://service.captcha.net/readcookies.aspx" /><br>
	

	</body>
</HTML>