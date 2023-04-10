using SignalChat.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SignalChat.Pages
{
    public partial class SelectAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["ChatInfo"] != null && UserDetail.ConnectedUsers != null)
            {
                string strChatInfo = (string)Session["ChatInfo"];

                var OnlineAdmins = UserDetail.ConnectedUsers.Where(x => x.RequestTypeID == int.Parse(strChatInfo.Split('$')[3]) && x.Status == true && x.UserCategoryID != 5).ToList();
                ltOnlineAdmin.Text = "<ul class=\"list-group\">";
                foreach (var item in OnlineAdmins)
                {
                    ltOnlineAdmin.Text += " <li class=\"list-group-item\" >"+
                        "<input class=\"form-check-input me-1\" type=\"radio\" name=\"listGroupRadio\" onclick='gotochatbox(\"" + item.UserName + "\")'  id=\"" + item.UserName + "\" >"+
                        "<label class=\"form-check-label\" for=\"" + item.UserName + "\">" + item.UserName + "</label></li>";
                }
                ltOnlineAdmin.Text += "</ul>";
            }
        }

        protected void btnStartChat_Click(object sender, EventArgs e)
        {
            if (hfselectedAdmin.Value != "" && Session["ChatInfo"] != null)
            {
                UserDetail.ConnectedUsers.Where(x => x.UserName == ((string)Session["ChatInfo"]).Split('$')[0]).ToList().ForEach(x => x.SelectedAdminToChat = hfselectedAdmin.Value);
                Response.Redirect("Chat");
            }
        }
    }
}