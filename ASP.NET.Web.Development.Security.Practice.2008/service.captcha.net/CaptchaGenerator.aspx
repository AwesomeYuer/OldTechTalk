//CaptchaGenerator.aspx
<%@ Page ContentType="text/x-javascript" Language="C#" AutoEventWireup="true" Inherits="Microshaoft.WebSecurity.CaptchaGenerator" %>
var <%= _ImgObjVar%> = document.getElementById("<%= _ImgID%>");
<%= _ImgObjVar%>.src="<%= _UrlPrefix%>Captcha.aspx?text=<%= _InternalTripleDESEncryptedVerifyCode%>&sign=<%= _RSASignMode%>&w=<%= _W%>&h=<%= _H%>&r=" + Math.random();
<%= _CallbackName%>("<%= _ClientID%>","<%= _EncryptedMode%>","<%= _EncryptedVerifyCode%>"/*, "<%= _ResponseRSAPublicKey%>"*/, "<%= _ResponseRSASignature%>");