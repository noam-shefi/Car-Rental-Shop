using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyOrders : Page
{
    private string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode("MyOrders.aspx"));
            return;
        }
        if (!IsPostBack)
            BindOrders();
    }

    private void BindOrders()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                rptOrders.DataSource = service.GetRentalOrders(username);
            rptOrders.DataBind();
            pnlEmpty.Visible = rptOrders.Items.Count == 0;
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

        BusinessModelCarRental.Order order = (BusinessModelCarRental.Order)e.Item.DataItem;
        LinkButton cancel = (LinkButton)e.Item.FindControl("btnCancel");
        cancel.Visible = order.MyRentalStatus == "pending" || order.MyRentalStatus == "approved";
        HyperLink review = (HyperLink)e.Item.FindControl("lnkReview");
        try
        {
            using (var service = new CarRentalServiceClient())
            {
                review.Visible = service.CanReviewRental(username, order.MyOrderID);
                review.NavigateUrl = "ReviewRental.aspx?orderId=" + order.MyOrderID;
            }
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "לא ניתן לבדוק זכאות לביקורת: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void rptOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "CancelRental")
            return;
        try
        {
            bool changed;
            using (var service = new CarRentalServiceClient())
                changed = service.RequestRentalCancellation(Convert.ToInt32(e.CommandArgument), username);
            lblMessage.Text = changed ? "בקשת הביטול עודכנה." : "לא ניתן לבטל את ההזמנה במצבה הנוכחי.";
            BindOrders();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "עדכון הביטול נכשל: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected string GetStatus(object value)
    {
        string status = Convert.ToString(value);
        if (status == "pending") return "ממתינה לאישור";
        if (status == "approved") return "אושרה";
        if (status == "rejected") return "נדחתה";
        if (status == "canceled") return "בוטלה";
        if (status == "cancellation requested") return "בקשת ביטול ממתינה לטיפול";
        return HttpUtility.HtmlEncode(status);
    }
}
