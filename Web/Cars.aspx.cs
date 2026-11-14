using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;

public partial class Cars : Page
{
    private bool isAdministrator;
    private bool adminStatusUnavailable;
    protected bool IsAdministrator { get { return isAdministrator; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        isAdministrator = IsCurrentUserAdministrator();
        pnlAdmin.Visible = isAdministrator;
        if (!IsPostBack)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["search"]))
                txtSearch.Text = Request.QueryString["search"];
            BindCars();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindCars();
    }

    private void BindCars()
    {
        decimal maximum;
        decimal? maximumPrice = null;
        if (!string.IsNullOrWhiteSpace(txtMaximumPrice.Text))
        {
            if (!decimal.TryParse(txtMaximumPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out maximum) || maximum < 0)
            {
                lblMessage.Text = "יש להזין מחיר מרבי תקין שאינו שלילי.";
                return;
            }
            maximumPrice = maximum;
        }

        DateTime startDate;
        DateTime endDate;
        DateTime? start = null;
        DateTime? end = null;
        bool hasStart = !string.IsNullOrWhiteSpace(txtStartDate.Text);
        bool hasEnd = !string.IsNullOrWhiteSpace(txtEndDate.Text);
        if (hasStart != hasEnd)
        {
            lblMessage.Text = "יש לבחור גם תאריך איסוף וגם תאריך החזרה.";
            return;
        }
        if (hasStart)
        {
            if (!DateTime.TryParseExact(txtStartDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) ||
                !DateTime.TryParseExact(txtEndDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) ||
                startDate.Date < DateTime.Today || endDate.Date <= startDate.Date)
            {
                lblMessage.Text = "תאריך האיסוף חייב להיות מהיום ואילך ותאריך ההחזרה חייב להיות מאוחר ממנו.";
                return;
            }
            start = startDate.Date;
            end = endDate.Date;
        }

        if (!adminStatusUnavailable)
            lblMessage.Text = string.Empty;
        try
        {
            using (var service = new CarRentalServiceClient())
                rptCars.DataSource = service.SearchCars(txtSearch.Text.Trim(), ddlCategory.SelectedValue, maximumPrice, start, end);
            rptCars.DataBind();
            pnlNoResults.Visible = rptCars.Items.Count == 0;
        }
        catch (CommunicationException ex)
        {
            lblMessage.Text = "לא ניתן לטעון את קטלוג הרכבים: " + HttpUtility.HtmlEncode(ex.Message);
            rptCars.DataSource = null;
            rptCars.DataBind();
            pnlNoResults.Visible = false;
        }
        catch (TimeoutException ex)
        {
            lblMessage.Text = "טעינת קטלוג הרכבים חרגה מזמן ההמתנה: " + HttpUtility.HtmlEncode(ex.Message);
            rptCars.DataSource = null;
            rptCars.DataBind();
            pnlNoResults.Visible = false;
        }
    }

    protected void rptCars_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "DeleteCar")
            return;
        if (!IsCurrentUserAdministrator())
        {
            Response.Redirect("HomeVisitor.aspx");
            return;
        }

        try
        {
            using (var service = new CarRentalServiceClient())
                service.DeleteCar(Convert.ToInt32(e.CommandArgument));
            BindCars();
        }
        catch (CommunicationException ex)
        {
            lblMessage.Text = "מחיקת הרכב נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
        catch (TimeoutException ex)
        {
            lblMessage.Text = "מחיקת הרכב חרגה מזמן ההמתנה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected string GetImageUrl(object pictureData)
    {
        string picture = Convert.ToString(pictureData);
        if (string.IsNullOrWhiteSpace(picture))
            return ResolveUrl("~/img/no-image.svg");
        if (picture.StartsWith("data:image", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("https://", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("~/", StringComparison.Ordinal))
            return picture.StartsWith("~/", StringComparison.Ordinal) ? ResolveUrl(picture) : picture;
        return "data:image/jpeg;base64," + picture;
    }

    private bool IsCurrentUserAdministrator()
    {
        string username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
            return false;
        try
        {
            using (var client = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
                return client.IsUserAdmin(username);
        }
        catch (CommunicationException ex)
        {
            adminStatusUnavailable = true;
            lblMessage.Text = "לא ניתן לאמת הרשאות מנהל כרגע: " + HttpUtility.HtmlEncode(ex.Message);
            return false;
        }
        catch (TimeoutException ex)
        {
            adminStatusUnavailable = true;
            lblMessage.Text = "אימות הרשאות מנהל חרג מזמן ההמתנה: " + HttpUtility.HtmlEncode(ex.Message);
            return false;
        }
    }
}
