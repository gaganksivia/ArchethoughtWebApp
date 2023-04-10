using SignalChat.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalChat.Pages
{
    public partial class Chat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ChatInfo"] != null)
            {
                lblusername.InnerText = Session["ChatInfo"].ToString().Split('$')[0];
            }
            else if (Session["AdminInfo"] != null)
            {
                lblusername.InnerText = Session["AdminInfo"].ToString().Split('$')[0];
            }
        }
    }
}