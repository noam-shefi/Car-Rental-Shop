using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string username = Convert.ToString(Session["username"] ?? Session["Username"]);
        litWelcome.Text = string.IsNullOrWhiteSpace(username)
            ? "Welcome!"
            : "Welcome back, " + HttpUtility.HtmlEncode(username) + "!";
    }
}
