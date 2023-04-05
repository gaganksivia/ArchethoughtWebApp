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
            SqlConnection cnn = new SqlConnection("Data Source=WIN-5FMBPR6ABEV\\SQLEXPRESS;Initial Catalog=Archethought;Integrated Security=True");
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "select name + '$' + CONVERT(nvarchar,usercategory)+ '$' +tbl_UserCategory.category_name from tbl_User " +
                              " inner join tbl_UserCategory  on tbl_UserCategory.id_user_category=tbl_User.usercategory and tbl_User.usercategory <> 5 " +
                              " where username=@username and password=@password";
            cmd.Parameters.AddWithValue("@username", username.Value.Trim());
            cmd.Parameters.AddWithValue("@password", password.Value.Trim());
            if (cnn.State != ConnectionState.Open)
                cnn.Open();
            var AdminInfo = cmd.ExecuteScalar();
            if (AdminInfo != null)
            {
                Session["AdminInfo"] = AdminInfo;
                Response.Redirect("Chat");
            }
        }
    }
}