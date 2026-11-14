using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ManageOrders : Page
{
    private string administrator;

    protected void Page_Load(object sender, EventArgs e)
    {
        administrator = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (!RequireAdministrator())
            return;
        if (!IsPostBack)
            BindOrders();
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
            if (client.IsUserAdmin(administrator))
                return true;
        }
        Response.Redirect("HomeVisitor.aspx");
        return false;
    }

    private void BindOrders()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                rptOrders.DataSource = service.GetRentalOrders(null);
            rptOrders.DataBind();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת ההזמנות נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            return;
        BusinessModelHardwareStore.Order order = (BusinessModelHardwareStore.Order)e.Item.DataItem;
        bool pending = order.MyRentalStatus == "pending";
        bool cancellationRequested = order.MyRentalStatus == "cancellation requested";
        bool approved = order.MyRentalStatus == "approved";
        e.Item.FindControl("btnApprove").Visible = pending;
        e.Item.FindControl("btnReject").Visible = pending;
        e.Item.FindControl("btnCancel").Visible = pending || approved;
        e.Item.FindControl("btnApproveCancellation").Visible = cancellationRequested;
        e.Item.FindControl("btnRejectCancellation").Visible = cancellationRequested;
    }

    protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string status;
        if (e.CommandName == "Approve") status = "approved";
        else if (e.CommandName == "Reject") status = "rejected";
        else if (e.CommandName == "Cancel" || e.CommandName == "ApproveCancellation") status = "canceled";
        else if (e.CommandName == "RejectCancellation") status = "approved";
        else return;

        try
        {
            bool updated;
            using (var service = new CarRentalServiceClient())
                updated = service.SetRentalStatus(Convert.ToInt32(e.CommandArgument), status, false, string.Empty);
            lblMessage.Text = updated ? "סטטוס ההזמנה עודכן." : "לא ניתן לבצע את שינוי הסטטוס המבוקש.";
            BindOrders();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "עדכון ההזמנה נכשל: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected string GetStatus(object value)
    {
        string status = Convert.ToString(value);
        if (status == "pending") return "ממתינה לאישור";
        if (status == "approved") return "אושרה";
        if (status == "rejected") return "נדחתה";
        if (status == "canceled") return "בוטלה";
        if (status == "cancellation requested") return "בקשת ביטול";
        return HttpUtility.HtmlEncode(status);
    }
}
