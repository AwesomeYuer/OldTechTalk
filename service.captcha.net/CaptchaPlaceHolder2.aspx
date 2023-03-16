//CaptchaPlaceHolder.aspx
<%@ Page ContentType="text/x-javascript" ValidateRequest="false" Language="C#" AutoEventWireup="true" Inherits="Microshaoft.WebSecurity.CaptchaPlaceHolder" %>
<%@ Import Namespace="Microshaoft" %>

document.write('<div id="<%= _DivID%>" style="display:none">');
function <%= _FunctionName%>()
{
    var <%= _DivChildrenNodesObjVar%> = document.getElementById("<%= _DivID%>").childNodes;
    if (<%= _DivChildrenNodesObjVar%>)
    {
        for (var <%= _iVar%> = <%= _DivChildrenNodesObjVar%>.length - 1; <%= _iVar%> >= 0; <%= _iVar%> --)
        {
            var <%= _DivChildNodeObjVar%> = <%= _DivChildrenNodesObjVar%>[<%= _iVar%>];
            <%= _DivChildNodeObjVar%>.parentNode.removeChild(<%= _DivChildNodeObjVar%>);
        }
    }
    var <%= _ScriptObjVar%> = document.createElement("SCRIPT");
    <%= _ScriptObjVar%>.src= "<%= _UrlPrefix%>CaptchaGenerator.aspx?imgid=<%= _ImgID%>&callback=<%= _CallbackName%>&refresh=<%= _RefreshCallName%>&id=<%= _ClientID%>&sign=<%= _RSASignMode%>&l=<%= _L%>&w=<%= _W%>&h=<%= _H%>&rsa=<%= _RequestRSAPublicKey%>&r=" + Math.random();
    document.getElementById("<%= _DivID%>").appendChild(<%= _ScriptObjVar%>);
    //alert(<%= _DivChildrenNodesObjVar%>.length);
}
document.write('</div>');
document.write('<table border="0"><tr><td align="center">');
document.write('<Img id="<%= _ImgID%>" width="<%= _W%>" height="<%= _H%>" />');
document.write('</td></tr>');
<%
if (!StringHelper.IsValidString(_RefreshCallName))
{
%>
    document.write('<tr><td align="center">');1
    document.write('<button onclick="<%= _FunctionName%>()">Refresh</button>');
    document.write('</td></tr>');
<%
}
%>
document.write('</table>');
<%= _FunctionName%>();
<%
if (StringHelper.IsValidString(_RefreshCallName))
{
%>
    function <%= _RefreshCallName%>()
    {
        <%= _FunctionName%>();
    }
<%
}
%>