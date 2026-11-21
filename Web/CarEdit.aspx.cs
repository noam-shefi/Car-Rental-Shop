using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessModelCarRental;

public partial class CarEdit : Page
{
    private int carID;
    private bool isEdit;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!RequireAdministrator())
            return;

        isEdit = int.TryParse(Request.QueryString["id"], out carID) && carID > 0;
        if (!IsPostBack)
        {
            LoadOptions();
            if (isEdit)
                LoadCar();
        }
    }

    private bool RequireAdministrator()
    {
        string username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            return false;
        }
        using (var client = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
        {
            if (client.IsUserAdmin(username))
                return true;
        }
        Response.Redirect("HomeVisitor.aspx");
        return false;
    }

    private void LoadOptions()
    {
        ddlBranch.Items.Clear();
        ddlBranch.Items.Add(new ListItem("ללא שיוך", string.Empty));
        using (var service = new CarRentalServiceClient())
        {
            foreach (Branch branch in service.GetBranches())
                ddlBranch.Items.Add(new ListItem(branch.MyBranchName + " — " + branch.MyCity, branch.MyBranchID.ToString()));
        }
    }

    private void LoadCar()
    {
        try
        {
            Car car;
            using (var service = new CarRentalServiceClient())
                car = service.GetCar(carID);
            if (car == null)
            {
                lblMessage.Text = "הרכב לא נמצא.";
                btnSave.Enabled = false;
                return;
            }
            litHeading.Text = "עריכת רכב";
            txtManufacturer.Text = car.MyManufacturer;
            txtModel.Text = car.MyModelName;
            txtYear.Text = car.MyYear.ToString();
            ddlCarCategory.SelectedValue = car.MyCarCategory;
            txtPrice.Text = car.MyPrice.ToString(CultureInfo.InvariantCulture);
            txtStock.Text = car.MyFleetQuantity.ToString();
            txtImage.Text = car.MyPicture;
            txtDescription.Text = car.MyDescription;
            txtSeats.Text = car.MySeats.HasValue ? car.MySeats.Value.ToString() : string.Empty;
            txtFuel.Text = car.MyFuelType;
            txtRange.Text = car.MyRangeKm.HasValue ? car.MyRangeKm.Value.ToString() : string.Empty;
            txtTransmission.Text = car.MyTransmission;
            txtLuggage.Text = car.MyLuggageCapacity.HasValue ? car.MyLuggageCapacity.Value.ToString() : string.Empty;
            if (car.MyBranchID.HasValue && ddlBranch.Items.FindByValue(car.MyBranchID.Value.ToString()) != null)
                ddlBranch.SelectedValue = car.MyBranchID.Value.ToString();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת הרכב נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int year, fleetQuantity;
        decimal price;
        int? seats, range, luggage;
        if ((!isEdit && (string.IsNullOrWhiteSpace(txtManufacturer.Text) || string.IsNullOrWhiteSpace(txtModel.Text))) ||
            !int.TryParse(txtYear.Text, out year) || !decimal.TryParse(txtPrice.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out price) ||
            !int.TryParse(txtStock.Text, out fleetQuantity) || (!isEdit && fleetQuantity < 0) ||
            !TryOptionalInt(txtSeats.Text, out seats) || !TryOptionalInt(txtRange.Text, out range) || !TryOptionalInt(txtLuggage.Text, out luggage))
        {
            lblMessage.Text = "יש למלא יצרן, דגם, שנה, מחיר וכמות תקינים; מפרטים מספריים אופציונליים חייבים להיות מספרים.";
            return;
        }

        try
        {
            int makeID = GetOrCreateBrand(txtManufacturer.Text.Trim());

            var car = new Car
            {
                MyCarID = carID,
                MyModelName = txtModel.Text.Trim(),
                MyMakeID = makeID,
                MyPrice = price,
                MyFleetQuantity = fleetQuantity,
                MyDescription = txtDescription.Text.Trim(),
                MyPicture = txtImage.Text.Trim(),
                MyIsCar = true,
                MyYear = year,
                MyCarCategory = ddlCarCategory.SelectedValue,
                MyBranchID = string.IsNullOrEmpty(ddlBranch.SelectedValue) ? (int?)null : Convert.ToInt32(ddlBranch.SelectedValue),
                MySeats = seats,
                MyFuelType = txtFuel.Text.Trim(),
                MyRangeKm = range,
                MyTransmission = txtTransmission.Text.Trim(),
                MyLuggageCapacity = luggage
            };
            using (var service = new CarRentalServiceClient())
            {
                if (isEdit)
                    service.UpdateCar(car);
                else
                    carID = service.AddCar(car);
            }
            Response.Redirect("Cars.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "שמירת הרכב נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            lblMessage.Text = "שמירת הרכב נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    private int GetOrCreateBrand(string manufacturer)
    {
        using (var client = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
        {
            var make = client.GetAllMakes().FirstOrDefault(item => string.Equals(item.MyMakeName, manufacturer, StringComparison.OrdinalIgnoreCase));
            if (make != null)
                return make.MyMakeID;
            client.AddMake(manufacturer, string.Empty, string.Empty, string.Empty);
            make = client.GetAllMakes().FirstOrDefault(item => string.Equals(item.MyMakeName, manufacturer, StringComparison.OrdinalIgnoreCase));
            if (make == null)
                throw new InvalidOperationException("לא ניתן ליצור את היצרן שנבחר.");
            return make.MyMakeID;
        }
    }

    private static bool TryOptionalInt(string value, out int? number)
    {
        number = null;
        if (string.IsNullOrWhiteSpace(value))
            return true;
        int parsed;
        if (!int.TryParse(value, out parsed))
            return false;
        number = parsed;
        return true;
    }
}
