using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using BusinessModelHardwareStore;

public partial class CarMatch : Page
{
    protected void btnRecommend_Click(object sender, EventArgs e)
    {
        string description = txtTrip.Text.Trim();
        if (string.IsNullOrWhiteSpace(description))
        {
            lblMessage.Text = "תארו בקצרה את הטיול כדי לקבל המלצה.";
            pnlRecommendation.Visible = false;
            return;
        }

        try
        {
            Product[] available;
            using (var service = new CarRentalServiceClient())
                available = service.GetCars().Where(car => car.MyStockQuantity > 0).ToArray();
            if (available.Length == 0)
            {
                lblMessage.Text = "אין כרגע רכבים עם מלאי זמין במערכת.";
                pnlRecommendation.Visible = false;
                return;
            }

            string category = InferCategory(description);
            Product selected = available.FirstOrDefault(car => string.Equals(car.MyCarCategory, category, StringComparison.OrdinalIgnoreCase)) ?? available.OrderByDescending(car => car.MyAverageRating).First();
            litCar.Text = HttpUtility.HtmlEncode((selected.MyManufacturer + " " + selected.MyProductName).Trim());
            litReason.Text = HttpUtility.HtmlEncode(BuildReason(category, selected));
            lnkCar.NavigateUrl = "CarDetails.aspx?id=" + selected.MyProductID;
            pnlRecommendation.Visible = true;
            lblMessage.Text = "ההתאמה מבוססת על מילות מפתח ותכונות הרכב. לא בוצעה בדיקת זמינות לתאריכי הטיול.";
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            pnlRecommendation.Visible = false;
            lblMessage.Text = "קבלת ההמלצה נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    private static string InferCategory(string description)
    {
        string text = description.ToLowerInvariant();
        if (HasAny(text, "ילדים", "משפחה", "משפחתי", "family", "children")) return "family";
        if (HasAny(text, "שטח", "ג׳יפ", "ג'יפ", "טבע", "הר", "suv", "off-road")) return "suv";
        if (HasAny(text, "ספורט", "מהירות", "מירוץ", "sports", "sport")) return "sports";
        return "sedan";
    }

    private static bool HasAny(string text, params string[] terms)
    {
        return terms.Any(term => text.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private static string BuildReason(string category, Product car)
    {
        string reason;
        if (category == "family") reason = "הקטגוריה מתאימה לנסיעה משפחתית ולנוסעים נוספים.";
        else if (category == "suv") reason = "רכב SUV מתאים יותר למסלול שמדגיש טבע או נסיעה מחוץ לעיר.";
        else if (category == "sports") reason = "רכב הספורט מתאים לנסיעה שבה חוויית הנהיגה חשובה במיוחד.";
        else reason = "רכב סדאן הוא בחירה שימושית לנסיעה יומיומית ולמסלולים עירוניים.";
        return reason + " ברכב יש " + car.MyStockQuantity + " יחידות רשומות במלאי ודירוג ממוצע " + car.MyAverageRating.ToString("0.0") + ".";
    }
}
