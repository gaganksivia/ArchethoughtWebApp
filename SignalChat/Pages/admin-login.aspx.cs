using SignalChat.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SignalChat.Pages
{
    public partial class admin_login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "select  name + '$' + CONVERT(nvarchar,tbl_Admin_request.id_request) + '$' + CONVERT(nvarchar,id) from tbl_User " +
                              " inner join tbl_Admin_request  on tbl_Admin_request.id_admin=tbl_User.id" +
                              " where username=@username and password=@password";
            cmd.Parameters.AddWithValue("@username", username.Value.Trim());
            cmd.Parameters.AddWithValue("@password", password.Value.Trim());
            if (cnn.State != ConnectionState.Open)
                cnn.Open();
            var AdminInfo = cmd.ExecuteScalar();
            if (AdminInfo != null)
            {
                UserDetail admin = UserDetail.ConnectedUsers.Where(x => x.UserName == username.Value).FirstOrDefault();
                if (admin == null)
                {
                    UserDetail.ConnectedUsers.Add(new UserDetail
                    {
                        UserName = username.Value.Trim(),
                        RequestTypeID = int.Parse(AdminInfo.ToString().Split('$')[1]),
                        UserCategoryID = int.Parse(AdminInfo.ToString().Split('$')[1]),
                        UserID = int.Parse(AdminInfo.ToString().Split('$')[2]),
                        Status = false
                    });
                }
                Session["AdminInfo"] = AdminInfo;
                Response.Redirect("Chat");
            }
        }
    }
}