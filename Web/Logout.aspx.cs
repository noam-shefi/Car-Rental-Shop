using System;
using System.Web;
using System.Web.UI;

public partial class RentalLogout : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();

        HttpCookie sessionCookie = new HttpCookie("ASP.NET_SessionId", string.Empty)
        {
            Expires = DateTime.UtcNow.AddYears(-1),
            HttpOnly = true,
            Secure = true,
            Path = "/"
        };
        sessionCookie.SameSite = SameSiteMode.Lax;
        Response.Cookies.Set(sessionCookie);

        Response.Redirect("HomeVisitor.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }
}
