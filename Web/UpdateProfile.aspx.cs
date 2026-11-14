using System;
using System.Web;
using System.Web.UI;

public partial class UpdateProfile : Page
{
    private string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode("UpdateProfile.aspx"));
            return;
        }
        if (!IsPostBack)
            LoadProfile();
    }

    private void LoadProfile()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
            {
                var user = service.GetCustomerProfile(username);
                if (user == null)
                {
                    lblMessage.CssClass = "d-block text-danger mb-3";
                    lblMessage.Text = "החשבון לא נמצא.";
                    return;
                }
                txtFirstName.Text = user.MyFirstName;
                txtLastName.Text = user.MyLastName;
                txtEmail.Text = user.MyEmail;
                txtPhone.Text = user.MyPhoneNumber;
            }
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.CssClass = "d-block text-danger mb-3";
            lblMessage.Text = "טעינת הפרופיל נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                service.UpdateProfile(username, txtFirstName.Text.Trim(), txtLastName.Text.Trim(), txtEmail.Text.Trim(), txtPhone.Text.Trim());
            lblMessage.CssClass = "d-block text-success mb-3";
            lblMessage.Text = "הפרופיל עודכן בהצלחה.";
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.CssClass = "d-block text-danger mb-3";
            lblMessage.Text = "עדכון הפרופיל נכשל: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }
}
