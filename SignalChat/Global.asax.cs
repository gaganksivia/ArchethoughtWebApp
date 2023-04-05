using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
namespace SignalChat
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapPageRoute("Index", "Index", "~/Index.aspx");
            RouteTable.Routes.MapPageRoute("admin-login", "admin-login", "~/Pages/admin-login.aspx");
            RouteTable.Routes.MapPageRoute("requisition", "requisition", "~/Pages/requisition.aspx");
            RouteTable.Routes.MapPageRoute("Chat", "Chat", "~/Pages/Chat.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}