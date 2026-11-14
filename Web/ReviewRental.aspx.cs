using System;
using System.Linq;
using System.Web;
using System.Web.UI;

public partial class ReviewRental : Page
{
    private string username;
    private int orderID;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            return;
        }
        if (!int.TryParse(Request.QueryString["orderId"], out orderID) || orderID <= 0)
        {
            Response.Redirect("MyOrders.aspx");
            return;
        }
        if (!IsPostBack)
            LoadReviewForm();
    }

    private void LoadReviewForm()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
            {
                if (!service.CanReviewRental(username, orderID))
                {
                    lblMessage.Text = "ההשכרה אינה זכאית לביקורת או שכבר נכתבה עליה ביקורת.";
                    return;
                }
                var order = service.GetRentalOrders(username).FirstOrDefault(item => item.MyOrderID == orderID);
                if (order == null)
                {
                    lblMessage.Text = "ההזמנה לא נמצאה.";
                    return;
                }
                lblCar.Text = HttpUtility.HtmlEncode(order.MyCarName);
                pnlReview.Visible = true;
            }
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת טופס הביקורת נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int rating;
        if (!int.TryParse(ddlRating.SelectedValue, out rating) || rating < 1 || rating > 5)
        {
            lblMessage.Text = "יש לבחור דירוג בין 1 ל-5.";
            return;
        }
        try
        {
            using (var service = new CarRentalServiceClient())
                service.AddRentalReview(username, orderID, rating, txtComment.Text.Trim());
            Response.Redirect("MyOrders.aspx");
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "שליחת הביקורת נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }
}
