namespace Microshaoft.WebSecurity.AntiXSS.HttpModules
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Reflection;
    public class WebControlsPropertysFilter : IHttpModule
    {
        private HttpApplication _contextApplication;
        public void Init(HttpApplication context)
        {
            _contextApplication = context;
            _contextApplication.PostMapRequestHandler += new EventHandler(_contextApplication_PostMapRequestHandlerProcess);
        }
        public void Dispose()
        {
            _contextApplication = null;
            _contextApplication.Dispose();
        }
        public void _contextApplication_PostMapRequestHandlerProcess(object sender, EventArgs e)
        {
            IHttpHandler handler = null;
            if (_contextApplication == null)
            {
                return;
            }
            if (_contextApplication.Context.Handler is Page)
            {
                handler = _contextApplication.Context.Handler;
            }
            if (handler != null)
            {
                Page page = handler as Page;
                //page.PreRender += new EventHandler(page_PreRender);
                page.PreRenderComplete += new EventHandler(page_PreRender);
            }
        }
        private void page_PreRender(object sender, EventArgs e)
        {
            Page page = sender as Page;
            ControlCollection cc = page.Controls;
            RecursiveProcessControls(cc);
        }
        private static void RecursiveProcessControls(ControlCollection cc)
        {
            foreach (Control c in cc)
            {
                Type t = c.GetType();
                PropertyInfo pi = t.GetProperty("Text"); //Server WebControls
                if (pi != null)
                {
                    string s = (string) (pi.GetValue(c, null));
                    s = string.Format("AntiXSS HttpModule Filter: [{0}]", s);
                    //s = HttpUtility.HtmlEncode(s); //AntiXSS
                    pi.SetValue(c, s, null);
                }
                pi = t.GetProperty("Value"); //Server HtmlControls
                if (pi != null)
                {
                    string s = (string) (pi.GetValue(c, null));
                    s = HttpUtility.HtmlEncode(s); //AntiXSS
                    pi.SetValue(c, s, null);
                }
                if (c.HasControls())
                {
                    RecursiveProcessControls(c.Controls);
                }
            }
        }
    }
}
/*
<?xml version="1.0" encoding="utf-8" ?>
<!-- Web.Config -->
<configuration>
    <system.web>
        <httpModules>
            <add name="ControlsPropertyFilterHttpModule" type="Microshaoft.ControlsPropertyFilterHttpModule, ControlsPropertyFilterHttpModule" />
        </httpModules>
    </system.web>
</configuration>
/*

*/