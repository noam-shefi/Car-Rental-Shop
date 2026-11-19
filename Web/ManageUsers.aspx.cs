using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageUsers : Page
{
    private string administrator;

    protected void Page_Load(object sender, EventArgs e)
    {
        administrator = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (!RequireAdministrator()) return;
        if (!IsPostBack) BindUsers();
    }

    private bool RequireAdministrator()
    {
        if (string.IsNullOrWhiteSpace(administrator))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            return false;
        }
        using (var client = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
        {
            if (client.IsUserAdmin(administrator)) return true;
        }
        Response.Redirect("HomeVisitor.aspx");
        return false;
    }

    private void BindUsers()
    {
        try
        {
            using (var client = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
                rptUsers.DataSource = client.showAllUsers();
            rptUsers.DataBind();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת המשתמשים נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
        BusinessModelCarRental.User user = (BusinessModelCarRental.User)e.Item.DataItem;
        LinkButton role = (LinkButton)e.Item.FindControl("btnRole");
        LinkButton delete = (LinkButton)e.Item.FindControl("btnDelete");
        bool self = string.Equals(user.MyUsername, administrator, StringComparison.OrdinalIgnoreCase);
        role.Visible = !self;
        delete.Visible = !self;
        role.Text = user.MyAdmin ? "הסרת הרשאת מנהל" : "קידום למנהל";
    }

    protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string target = Convert.ToString(e.CommandArgument);
        if (string.Equals(target, administrator, StringComparison.OrdinalIgnoreCase))
        {
            lblMessage.Text = "לא ניתן לשנות או למחוק את חשבון המנהל הנוכחי.";
            return;
        }
        try
        {
            bool changed;
            using (var service = new CarRentalServiceClient())
            {
                if (e.CommandName == "ToggleAdmin")
                {
                    bool currentlyAdmin;
                    using (var accountClient = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
                        currentlyAdmin = accountClient.IsUserAdmin(target);
                    changed = service.SetUserAdmin(target, !currentlyAdmin, administrator);
                }
                else if (e.CommandName == "DeleteUser")
                    changed = service.DeleteManagedUser(target, administrator);
                else
                    return;
            }
            lblMessage.Text = changed ? "פעולת ניהול המשתמש הושלמה." : "לא ניתן לבצע את הפעולה.";
            BindUsers();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "ניהול המשתמש נכשל: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }
}
