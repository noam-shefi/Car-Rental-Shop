using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls; 

public partial class MasterPageUser : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));

        if (Session["username"] == null)
        {
            Response.Redirect("SignInVisitor.aspx");
        }

        if (Session["isAdmin"] != null)
            phAdminLinks.Visible = Convert.ToBoolean(Session["isAdmin"]);

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