using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAspApp
{
    public partial class MemberEdit : System.Web.UI.Page
    {
        int id
        {
            get
            {
                int i = 0;
                int.TryParse(ViewState["id"] + "", out i);
                return i;
            }

            set
            {
                ViewState["id"] = value;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string istr = Request.QueryString["id"] + "";
                if(istr.Length>0)
                {
                    int i = 0;
                    if(int.TryParse(istr, out i))
                    {
                        id = i;
                    }
                }

                LoadData();

            }
        }

        void LoadData()
        {
            DataTable dt = new DataTable();

            if(id>0)
            {
                using (MySqlConnection conn = new MySqlConnection(config.ConnStr))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        conn.Open();

                        cmd.Connection = conn;

                        cmd.CommandText = string.Format("select * from `member` where id={0} limit 0,1;",id);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                        da.Fill(dt);

                        conn.Close();
                    }
                }
            }

            if(id==0 || dt.Rows.Count==0)
            {
                lbId.Text = "[new data]";
                txtCode.Text = "";
                txtName.Text = "";
                txtPhone.Text = "";
                dropGender.SelectedIndex = 0;
            }
            else
            {
                lbId.Text = id.ToString();
                txtCode.Text = dt.Rows[0]["code"] + "";
                txtName.Text = dt.Rows[0]["name"] + "";
                txtPhone.Text = dt.Rows[0]["phone"] + "";

                try
                {
                    dropGender.SelectedValue = dt.Rows[0]["gender"] + "";
                }
                catch
                {

                }
            }
        }
        protected void btSave_Click(object sender, EventArgs e)
        {

            

           using (MySqlConnection conn = new MySqlConnection(config.ConnStr))
            {
                using(MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();

                    cmd.Connection = conn;

                    if(id==0)
                    {
                        cmd.CommandText = "insert into `member` (code,name,phone,gender) values (@code,@name,@phone,@gender) ;";
                    }
                    else
                    {
                        cmd.CommandText = "update `member` set code=@code, name=@code, phone=@phone, gender=@gender where id=@id;";
                        cmd.Parameters.AddWithValue("@id", id);
                    }

                    cmd.Parameters.AddWithValue("@code", txtCode.Text);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@gender", dropGender.SelectedValue);

                    cmd.ExecuteNonQuery();

                    if(id==0)
                    {
                        cmd.CommandText = "select last_insert_id();";
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    conn.Close();
                }
            }

            ph1.Controls.Add(new LiteralControl("<spon style='color: darkgreen;'>Data saved</span>"));
            LoadData();

        }

        protected void btDelete_Click(object sender, EventArgs e)
        {
            if (id == 0)
                return;

           using (MySqlConnection conn = new MySqlConnection(config.ConnStr))
            {
                using(MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();

                    cmd.Connection = conn;

                    cmd.CommandText = string.Format("delete from `member` where id={0} limit 1;", id);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }

            Response.Redirect("Default.aspx");

        }
    }
}