using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessModelCarRental;

public partial class ManageBranches : Page
{
    private string administrator;

    protected void Page_Load(object sender, EventArgs e)
    {
        administrator = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (!RequireAdministrator()) return;
        if (!IsPostBack) BindBranches();
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

    private void BindBranches()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                rptBranches.DataSource = service.GetBranches();
            rptBranches.DataBind();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת הסניפים נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCity.Text))
        {
            lblMessage.Text = "שם ועיר הם שדות חובה.";
            return;
        }
        try
        {
            using (var service = new CarRentalServiceClient())
                service.AddBranch(new Branch { MyBranchName = txtName.Text.Trim(), MyCity = txtCity.Text.Trim(), MyAddress = txtAddress.Text.Trim(), MyPhone = txtPhone.Text.Trim() });
            txtName.Text = txtCity.Text = txtAddress.Text = txtPhone.Text = string.Empty;
            lblMessage.Text = "הסניף נוסף.";
            BindBranches();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "הוספת הסניף נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void rptBranches_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "DeleteBranch") return;
        try
        {
            using (var service = new CarRentalServiceClient())
                service.DeleteBranch(Convert.ToInt32(e.CommandArgument));
            lblMessage.Text = "הסניף הוסר; הרכבים שלו נשארו בצי ללא שיוך.";
            BindBranches();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "מחיקת הסניף נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }
}
