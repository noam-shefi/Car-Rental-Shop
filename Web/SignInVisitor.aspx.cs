using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceReference1;
using BusinessModelHardwareStore;

public partial class Login : System.Web.UI.Page
{
    ServiceReference1.Service1Client ws = new ServiceReference1.Service1Client("BasicHttpBinding_IService1");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.lblInfo.Visible = false;
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string username = this.txtusername.Text.Trim();
        if (ws.IsExistUserAndPass(username, this.txtpass.Text))
        {
            Session["username"] = username;
            Session["Username"] = username;
            Session["isAdmin"] = ws.IsUserAdmin(username);

            User user = ws.showUser(username);
            if (user != null)
                Session["email"] = user.MyEmail;

            string returnUrl = Request.QueryString["ReturnUrl"];
            Response.Redirect(IsSafeLocalReturnUrl(returnUrl) ? returnUrl : "Cars.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.lblInfo.Visible = true;
            this.lblInfo.Text = "שם המשתמש או הסיסמה אינם נכונים.";
        }
    }

    private bool IsSafeLocalReturnUrl(string returnUrl)
    {
        if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl.StartsWith("//", StringComparison.Ordinal) || returnUrl.StartsWith("\\\\", StringComparison.Ordinal))
            return false;
        Uri target;
        return Uri.TryCreate(returnUrl, UriKind.Relative, out target) && !returnUrl.StartsWith("/", StringComparison.Ordinal) && !returnUrl.Contains("\\");
    }
}