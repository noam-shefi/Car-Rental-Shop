using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageFleet : Page
{
    private string administrator;

    protected void Page_Load(object sender, EventArgs e)
    {
        administrator = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (!RequireAdministrator()) return;
        if (!IsPostBack) BindCars();
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

    private void BindCars()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                rptCars.DataSource = service.GetCars();
            rptCars.DataBind();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת הצי נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void rptCars_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "SaveStock") return;
        RepeaterItem item = (RepeaterItem)((Control)e.CommandSource).NamingContainer;
        TextBox quantityBox = (TextBox)item.FindControl("txtStock");
        int quantity;
        if (!int.TryParse(quantityBox.Text, out quantity) || quantity < 0)
        {
            lblMessage.Text = "יש להזין מספר שלם שאינו שלילי.";
            return;
        }
        try
        {
            using (var service = new CarRentalServiceClient())
                service.SetCarFleetQuantity(Convert.ToInt32(e.CommandArgument), quantity);
            lblMessage.Text = "כמות הצי עודכנה.";
            BindCars();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "עדכון המלאי נכשל: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }
}
