using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAspApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(config.ConnStr))
            {
                using(MySqlCommand cmd = new MySqlCommand())
                {
                    conn.Open();

                    cmd.Connection = conn;

                    cmd.CommandText = "select * from `member` order by name;";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                    da.Fill(dt);

                    conn.Close();
                }
            }

            StringBuilder  sb = new StringBuilder();
            sb.Append(@"
<table>
<tr>
<th></th>
<th>ID</th>
<th>Code</th>
<th>Name</th>
<th>Phone</th>
<th>Gender</th>
</tr>
");
            if(dt.Rows.Count==0)
            {
                sb.Append("<tr><td colspan='6'>No data</td></tr>");
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("<tr>");

                    sb.AppendFormat("<td><a href='MemberEdit.aspx?id={0}'>Edit</a></td>", dr["id"]);

                    sb.AppendFormat("<td>{0}</td>", dr["id"]);
                    sb.AppendFormat("<td>{0}</td>", dr["code"]);
                    sb.AppendFormat("<td>{0}</td>", dr["name"]);
                    sb.AppendFormat("<td>{0}</td>", dr["phone"]);

                    string g = dr["gender"] + "";

                    switch(g)
                    {
                        case "1":
                            sb.Append("<td>Male</td>");
                            break;
                        case "2":
                            sb.Append("<td>Female</td>");
                            break;
                        default:
                            sb.Append("<td></td>");
                            break;

                    }

                    sb.Append("</tr>");
                }
            }

            sb.Append("</table>");

            ph1.Controls.Add(new LiteralControl(sb.ToString()));

        }
    }
}