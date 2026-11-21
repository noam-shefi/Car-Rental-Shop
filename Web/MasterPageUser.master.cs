using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ServiceModel;
using ServiceReference1;

public partial class MasterPageUser : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));

        string username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }

        Session["username"] = username;
        Session["Username"] = username;

        phAdminLinks.Visible = ResolveAdminVisibility(username);

        if (!IsPostBack)
        {
            SetActiveClass();

            if (Request.QueryString["search"] != null)
            {
                txtMasterSearch.Text = Request.QueryString["search"];
            }
        }
    }

    protected void GlobalSearch_Click(object sender, EventArgs e)
    {
        string searchTerm = txtMasterSearch.Text.Trim();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            Response.Redirect("Cars.aspx?search=" + HttpUtility.UrlEncode(searchTerm));
        }
        else
        {
            Response.Redirect("Cars.aspx");
        }
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("Logout.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    private bool ResolveAdminVisibility(string username)
    {
        bool isAdmin;
        if (bool.TryParse(Convert.ToString(Session["isAdmin"]), out isAdmin))
        {
            try
            {
                using (var client = new Service1Client("BasicHttpBinding_IService1"))
                {
                    bool freshAdminState = client.IsUserAdmin(username);
                    Session["isAdmin"] = freshAdminState;
                    return freshAdminState;
                }
            }
            catch (CommunicationException)
            {
                return isAdmin;
            }
            catch (TimeoutException)
            {
                return isAdmin;
            }
        }

        try
        {
            using (var client = new Service1Client("BasicHttpBinding_IService1"))
            {
                bool freshAdminState = client.IsUserAdmin(username);
                Session["isAdmin"] = freshAdminState;
                return freshAdminState;
            }
        }
        catch (CommunicationException)
        {
            return false;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }

    private void SetActiveClass()
    {
        string pageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

        if (navbarLinksContainer == null) return;

        foreach (Control ctrl in navbarLinksContainer.Controls)
        {
            if (ctrl is HtmlAnchor)
            {
                HtmlAnchor link = (HtmlAnchor)ctrl;

                if (link.HRef.ToLower().Contains(pageName.ToLower()))
                {
                    link.Attributes["class"] += " active";
                    break;
                }
            }
            else if (ctrl is LinkButton)
            {
                LinkButton btn = (LinkButton)ctrl;
                if (btn.PostBackUrl.ToLower().Contains(pageName.ToLower()))
                {
                    btn.CssClass += " active";
                    break;
                }
            }
        }
    }
}