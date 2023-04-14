using SignalChat.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
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
            try
            {
                //////////
                string hashPass = "";
                using (var sha256 = SHA256.Create())
                {
                    // Send a sample text to hash.  
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password.Value.Trim()));
                    // Get the hashed string.  
                    hashPass = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }
                //////////
                SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString);
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select  name + '$' + CONVERT(nvarchar,tbl_Admin_request.id_request) + '$' + CONVERT(nvarchar,id) from tbl_User " +
                                  " inner join tbl_Admin_request  on tbl_Admin_request.id_admin=tbl_User.id" +
                                  " where username=@username and password=@password";
                cmd.Parameters.AddWithValue("@username", username.Value.Trim());
                cmd.Parameters.AddWithValue("@password", hashPass);
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                var AdminInfo = cmd.ExecuteScalar();
                if (AdminInfo != null)
                {
                    UserDetail admin = UserDetail.ConnectedUsers.Where(x => x.UserName.ToLower() == username.Value.ToLower()).FirstOrDefault();
                    if (admin == null)
                    {
                        UserDetail.ConnectedUsers.Add(new UserDetail
                        {
                            UserName = username.Value.Trim().ToLower(),
                            RequestTypeID = int.Parse(AdminInfo.ToString().Split('$')[1]),
                            UserCategoryID = int.Parse(AdminInfo.ToString().Split('$')[1]),
                            UserID = int.Parse(AdminInfo.ToString().Split('$')[2]),
                            SelectedAdminToChat = "",
                            Status = false
                        });
                    }
                    Session["AdminInfo"] = AdminInfo;
                    Response.Redirect("Chat");
                }
            }
            catch { }
        }
    }
}