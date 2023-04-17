using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SignalChat.Class;
using System.Security.Cryptography;
using System.Text;

namespace SignalChat.Pages
{
    public partial class requisition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUserLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var str = hfRequestType.Value.ToString();
                //////////
                string hashPass = "";
                using (var sha256 = SHA256.Create())
                {
                    // Send a sample text to hash.  
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Value.Trim()));
                    // Get the hashed string.  
                    hashPass = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                }
                //////////
                SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString);
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "select name+'$'+convert(nvarchar,id) from tbl_User where name=@name and password=@password";
                cmd.Parameters.AddWithValue("@name", txtName.Value.Trim());
                cmd.Parameters.AddWithValue("@password", hashPass);
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                var strname = cmd.ExecuteScalar();
                if (strname == null)
                {

                    cmd.Parameters.Clear();
                    cmd.CommandText = "Insert into tbl_User      ([name]" +
                                                               ",[email]" +
                                                               ",[birthdate]" +
                                                               ",[password]" +
                                                               ",[usercategory])" +
                                                               " output INSERTED.id " +
                                                               " Values (@name,@email,@birthdate,@password,@usercategory)";
                    cmd.Parameters.AddWithValue("@name", txtName.Value.Trim());
                    cmd.Parameters.AddWithValue("@email", txtemail.Value.Trim());
                    cmd.Parameters.AddWithValue("@password", hashPass);
                    cmd.Parameters.AddWithValue("@birthdate", txtdob.Value.Trim());
                    cmd.Parameters.AddWithValue("@usercategory", 5);
                    int id = (int)cmd.ExecuteScalar();
                    strname = txtName.Value.ToString().Trim() + "$" + id.ToString();
                }

                Session["ChatInfo"] = strname + "$" + hfRequestType.Value.ToString();
                UserDetail user = UserDetail.ConnectedUsers.Where(u => u.UserName == strname.ToString()).FirstOrDefault();
                if (user == null)
                {
                    UserDetail.ConnectedUsers.Add(new UserDetail
                    {
                        UserName = txtName.Value.ToString().Trim().ToLower(),
                        RequestTypeID = int.Parse(hfRequestType.Value.ToString().Split('$')[1]),
                        UserCategoryID = 5,
                        UserID = int.Parse(strname.ToString().Split('$')[1]),
                        Status = false
                    });
                }
                Response.Redirect("SelectAdmin");

            }
            catch { }
        }
    }
}