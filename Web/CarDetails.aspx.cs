using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using BusinessModelCarRental;

public partial class CarDetails : Page
{
    private int carID;
    private Car car;
    private readonly HashSet<DateTime> unavailableDates = new HashSet<DateTime>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!int.TryParse(Request.QueryString["id"], out carID) || carID <= 0)
        {
            Response.Redirect("Cars.aspx");
            return;
        }

        if (!IsPostBack)
        {
            calStart.VisibleDate = DateTime.Today;
            calEnd.VisibleDate = DateTime.Today;
        }
        LoadCar();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (pnlConfirmation.Visible &&
            (!string.Equals(ViewState["CheckedStartDate"] as string, txtStartDate.Text, StringComparison.Ordinal) ||
             !string.Equals(ViewState["CheckedEndDate"] as string, txtEndDate.Text, StringComparison.Ordinal)))
            pnlConfirmation.Visible = false;
    }

    private void LoadCar()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                car = service.GetCar(carID);

            if (car == null)
            {
                pnlNotFound.Visible = true;
                pnlDetails.Visible = false;
                return;
            }

            pnlDetails.Visible = true;
            pnlNotFound.Visible = false;
            imgCar.ImageUrl = GetImageUrl(car.MyPicture);
            litTitle.Text = HttpUtility.HtmlEncode((car.MyManufacturer + " " + car.MyModelName).Trim());
            litDescription.Text = HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(car.MyDescription) ? "ללא תיאור נוסף." : car.MyDescription);
            litPrice.Text = car.MyPrice.ToString("0.##", CultureInfo.InvariantCulture);
            litRating.Text = car.MyAverageRating.ToString("0.0", CultureInfo.InvariantCulture);
            litReviewCount.Text = car.MyReviewCount.ToString(CultureInfo.InvariantCulture);
            litYear.Text = car.MyYear > 0 ? car.MyYear.ToString() : "לא צוין";
            litCategory.Text = HttpUtility.HtmlEncode(CarRentalDisplay.VehicleCategory(car.MyCarCategory));
            litBranch.Text = string.IsNullOrWhiteSpace(car.MyBranchName) ? "לא משויך לסניף" : HttpUtility.HtmlEncode(car.MyBranchName + " — " + car.MyBranchCity);
            litSeats.Text = FormatOptional(car.MySeats);
            litFuel.Text = HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(car.MyFuelType) ? "לא צוין" : car.MyFuelType);
            litRange.Text = car.MyRangeKm.HasValue ? car.MyRangeKm.Value + " ק״מ" : "לא צוין";
            litTransmission.Text = HttpUtility.HtmlEncode(string.IsNullOrWhiteSpace(car.MyTransmission) ? "לא צוין" : car.MyTransmission);
            litLuggage.Text = FormatOptional(car.MyLuggageCapacity);
            litStock.Text = car.MyFleetQuantity.ToString();

            string username = CurrentUsername;
            bool signedIn = !string.IsNullOrWhiteSpace(username);
            btnFavorite.Visible = signedIn;
            lnkLogin.Visible = !signedIn;
            lnkLogin.NavigateUrl = "SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode("CarDetails.aspx?id=" + carID);
            if (signedIn)
            {
                using (var service = new CarRentalServiceClient())
                    btnFavorite.Text = service.GetFavorites(username).Any(favorite => favorite.MyCarID == carID) ? "הסרה מהמועדפים" : "הוספה למועדפים";
            }

            pnlBooking.Visible = signedIn && car.MyFleetQuantity > 0;
            btnCheckAvailability.Visible = car.MyFleetQuantity > 0;
            using (var service = new CarRentalServiceClient())
            {
                var unavailable = service.GetUnavailableRentalDates(carID, DateTime.Today, DateTime.Today.AddYears(1), car.MyFleetQuantity);
                unavailableDates.Clear();
                foreach (DateTime date in unavailable)
                    unavailableDates.Add(date.Date);

                var reviews = service.GetRentalReviewsForCar(carID);
                rptReviews.DataSource = reviews;
                rptReviews.DataBind();
                lblNoReviews.Visible = reviews == null || reviews.Count == 0;
            }
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            pnlDetails.Visible = false;
            pnlNotFound.Visible = false;
            lblMessage.Text = "טעינת פרטי הרכב נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void Calendar_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
    {
        if (e.Day.Date.Date < DateTime.Today || unavailableDates.Contains(e.Day.Date.Date))
        {
            e.Day.IsSelectable = false;
            e.Cell.CssClass = "text-muted";
        }
        else if (e.Day.IsSelectable)
        {
            e.Cell.Attributes["data-rental-date"] = e.Day.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }
    }

    protected void calStart_SelectionChanged(object sender, EventArgs e)
    {
        txtStartDate.Text = calStart.SelectedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    protected void calEnd_SelectionChanged(object sender, EventArgs e)
    {
        txtEndDate.Text = calEnd.SelectedDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    protected void btnFavorite_Click(object sender, EventArgs e)
    {
        string username = CurrentUsername;
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode("CarDetails.aspx?id=" + carID));
            return;
        }

        try
        {
            using (var service = new CarRentalServiceClient())
            {
                bool exists = service.GetFavorites(username).Any(favorite => favorite.MyCarID == carID);
                service.SetFavorite(username, carID, !exists);
            }
            LoadCar();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "עדכון המועדפים נכשל: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void btnCheckAvailability_Click(object sender, EventArgs e)
    {
        pnlConfirmation.Visible = false;
        DateTime startDate;
        DateTime endDate;
        if (car == null || !TryGetRentalDates(out startDate, out endDate))
            return;

        try
        {
            using (var service = new CarRentalServiceClient())
            {
                if (!service.IsRentalAvailable(carID, startDate, endDate, car.MyFleetQuantity))
                {
                    lblMessage.Text = "הרכב אינו זמין בתאריכים שנבחרו.";
                    return;
                }
            }
            litTotal.Text = (car.MyPrice * Convert.ToInt32((endDate - startDate).TotalDays)).ToString("0.00", CultureInfo.InvariantCulture);
            ViewState["CheckedStartDate"] = txtStartDate.Text;
            ViewState["CheckedEndDate"] = txtEndDate.Text;
            pnlConfirmation.Visible = true;
            lblMessage.Text = string.Empty;
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "בדיקת הזמינות נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void btnConfirmRental_Click(object sender, EventArgs e)
    {
        string username = CurrentUsername;
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode("CarDetails.aspx?id=" + carID));
            return;
        }

        DateTime startDate;
        DateTime endDate;
        if (!TryGetRentalDates(out startDate, out endDate))
            return;

        try
        {
            using (var service = new CarRentalServiceClient())
                service.CreateRentalOrder(username, carID, startDate, endDate);
            pnlConfirmation.Visible = false;
            lblMessage.Text = string.Empty;
            lblSuccess.Text = "בקשת ההשכרה נשלחה בהצלחה וממתינה לאישור. ניתן לעקוב אחריה בעמוד ההזמנות שלי.";
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            pnlConfirmation.Visible = false;
            lblMessage.Text = "לא ניתן ליצור את בקשת ההשכרה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    private bool TryGetRentalDates(out DateTime startDate, out DateTime endDate)
    {
        startDate = DateTime.MinValue;
        endDate = DateTime.MinValue;
        bool valid = DateTime.TryParseExact(txtStartDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) &&
                     DateTime.TryParseExact(txtEndDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
        if (!valid || startDate.Date < DateTime.Today || endDate.Date <= startDate.Date)
        {
            lblMessage.Text = "יש לבחור תאריך איסוף מהיום ואילך ותאריך החזרה מאוחר ממנו.";
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;
            return false;
        }
        startDate = startDate.Date;
        endDate = endDate.Date;
        return true;
    }

    private static string FormatOptional(int? value)
    {
        return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "לא צוין";
    }

    private string GetImageUrl(string picture)
    {
        if (string.IsNullOrWhiteSpace(picture))
            return ResolveUrl("~/img/no-image.svg");
        if (picture.StartsWith("data:image", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return picture;
        if (picture.StartsWith("~/", StringComparison.Ordinal))
            return ResolveUrl(picture);
        return "data:image/jpeg;base64," + picture;
    }

    private string CurrentUsername
    {
        get { return Convert.ToString(Session["username"] ?? Session["Username"]); }
    }
}
