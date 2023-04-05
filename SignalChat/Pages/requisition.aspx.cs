using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace SignalChat.Pages
{
    public partial class requisition : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUserLogin_Click(object sender, EventArgs e)
        {
            var str = hfRequestType.Value.ToString();

            SqlConnection cnn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString);
            SqlCommand cmd = cnn.CreateCommand();
            cmd.CommandText = "select convert(nvarchar,id)+'$'+name from tbl_User where email=@email";
            cmd.Parameters.AddWithValue("@email", txtemail.Value.Trim());
            if (cnn.State != ConnectionState.Open)
                cnn.Open();
            var strname = cmd.ExecuteScalar();
            if (strname == null)
            {

                cmd.Parameters.Clear();
                cmd.CommandText = "Insert into tbl_User      ([name]" +
                                                           ",[email]" +
                                                           ",[birthdate]" +
                                                           ",[usercategory])" +
                                                           " output INSERTED.id " +
                                                           " Values (@name,@email,@birthdate,@usercategory)";
                cmd.Parameters.AddWithValue("@name", txtname.Value.Trim());
                cmd.Parameters.AddWithValue("@email", txtemail.Value.Trim());
                cmd.Parameters.AddWithValue("@birthdate", txtdob.Value.Trim());
                cmd.Parameters.AddWithValue("@usercategory", 5);
                int id = (int)cmd.ExecuteScalar();
                strname = txtname.Value.ToString().Trim() + "$" + id.ToString();
            }
            cmd.CommandText = "insert into tbl_Request " +
           "([request_date]" +
           ",[id_user]" +
           ",[id_request_type]) values (getdate(), @id_user,@id_request_type)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id_user", strname.ToString().Split('$')[1]);
            cmd.Parameters.AddWithValue("@id_request_type", hfRequestType.Value.ToString().Split('$')[1]);
            cmd.ExecuteNonQuery();
            Session["ChatInfo"] = strname + "$" + hfRequestType.Value.ToString();
            Response.Redirect("Chat");
        }
    }
}