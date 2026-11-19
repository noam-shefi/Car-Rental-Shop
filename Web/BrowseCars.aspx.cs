using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

public partial class BrowseCars : System.Web.UI.Page
{
    private readonly ServiceReference1.Service1Client _ws = new ServiceReference1.Service1Client("BasicHttpBinding_IService1");

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect("Cars.aspx");
    }

    private void LoadFilterData()
    {
        cblCategories.DataSource = _ws.GetAllVehicleCategories();
        cblCategories.DataTextField = "MyCategoryName";
        cblCategories.DataValueField = "MyCategoryID";
        cblCategories.DataBind();

        cblBrands.DataSource = _ws.GetAllMakes();
        cblBrands.DataTextField = "MyBrandName";
        cblBrands.DataValueField = "MyBrandID";
        cblBrands.DataBind();
    }

    private void BindAllProducts()
    {
        var allProducts = _ws.GetAllFleetCars();

        if (allProducts == null || allProducts.Length == 0)
        {
            divNoProducts.Visible = true;
            rptProducts.DataSource = null;
        }
        else
        {
            divNoProducts.Visible = false;
            rptProducts.DataSource = allProducts;
        }
        rptProducts.DataBind();
    }

    protected void ApplyAllFilters(object sender, EventArgs e)
    {
        var allProductsRaw = _ws.GetAllFleetCars();
        if (allProductsRaw == null)
        {
            divNoProducts.Visible = true;
            rptProducts.DataSource = null;
            rptProducts.DataBind();
            return;
        }

        var filteredProducts = allProductsRaw.ToList();

        string globalSearchTerm = Request.QueryString["search"];

        if (!IsPostBack && !string.IsNullOrEmpty(globalSearchTerm))
        {
            foreach (ListItem item in cblCategories.Items) item.Selected = false;
            foreach (ListItem item in cblBrands.Items) item.Selected = false;
            foreach (ListItem item in cblPriceFilter.Items) item.Selected = false;
        }

        if (!string.IsNullOrEmpty(globalSearchTerm))
        {
            string term = globalSearchTerm.Trim().ToLower();
            filteredProducts = filteredProducts.Where(p =>
                (p.MyModelName != null && p.MyModelName.ToLower().Contains(term)) ||
                (p.MyDescription != null && p.MyDescription.ToLower().Contains(term))
            ).ToList();
        }

        var selectedCategories = cblCategories.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => Convert.ToInt32(i.Value)).ToList();
        if (selectedCategories.Count > 0)
        {
            var categoryResults = new List<object>();
            foreach (int catId in selectedCategories)
            {
                var productsFromCat = _ws.GetFleetCarsByCategoryID(catId);
                if (productsFromCat != null) categoryResults.AddRange(productsFromCat);
            }
            filteredProducts = filteredProducts.Where(p => categoryResults.Any(cr => ((dynamic)cr).MyCarID == p.MyCarID)).ToList();
        }

        var selectedBrands = cblBrands.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => Convert.ToInt32(i.Value)).ToList();
        if (selectedBrands.Count > 0)
        {
            var brandResults = new List<object>();
            foreach (int makeId in selectedBrands)
            {
                var productsFromBrand = _ws.GetFleetCarsByMakeID(makeId);
                if (productsFromBrand != null) brandResults.AddRange(productsFromBrand);
            }
            filteredProducts = filteredProducts.Where(p => brandResults.Any(br => ((dynamic)br).MyCarID == p.MyCarID)).ToList();
        }

        var selectedPrices = cblPriceFilter.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
        if (selectedPrices.Count > 0)
        {
            filteredProducts = filteredProducts.Where(p =>
                (selectedPrices.Contains("under50") && p.MyPrice <= 50) ||
                (selectedPrices.Contains("51-100") && p.MyPrice > 50 && p.MyPrice <= 100) ||
                (selectedPrices.Contains("101-200") && p.MyPrice > 100 && p.MyPrice <= 200) ||
                (selectedPrices.Contains("201-500") && p.MyPrice > 200 && p.MyPrice <= 500) ||
                (selectedPrices.Contains("above500") && p.MyPrice > 500)
            ).ToList();
        }

        string sortSelection = ddlSort.SelectedValue;
        if (sortSelection == "low-high")
        {
            filteredProducts = filteredProducts.OrderBy(p => p.MyPrice).ToList();
        }
        else if (sortSelection == "high-low")
        {
            filteredProducts = filteredProducts.OrderByDescending(p => p.MyPrice).ToList();
        }

        if (filteredProducts.Count == 0)
        {
            divNoProducts.Visible = true; 
            rptProducts.DataSource = null;
        }
        else
        {
            divNoProducts.Visible = false; 
            rptProducts.DataSource = filteredProducts;
        }

        rptProducts.DataBind();
    }

    protected void rptProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "AddToCart")
        {
            string username = Session["Username"] != null ? Session["Username"].ToString() : "guest";
            int carID = Convert.ToInt32(e.CommandArgument);

            try
            {
                if (_ws.CarExistsInCart(carID, username))
                {
                    var cartItems = _ws.ListCartWithCarDetails(username);

                    var existingItem = cartItems.FirstOrDefault(p => p.MyCarID == carID);
                    if (existingItem != null)
                    {
                        int newQuantity = existingItem.MyQuantity + 1;

                        _ws.UpdateCartQuantity(username, carID, newQuantity);
                    }
                }
                else
                {
                    var allProducts = _ws.GetAllFleetCars();
                    var car = allProducts.FirstOrDefault(p => p.MyCarID == carID);

                    if (car != null)
                    {
                        _ws.AddCart(car.MyCarID, username, 1);
                    }
                }

                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error adding product to cart: " + ex.Message + "');</script>");
            }
        }
    }

    public string GetImageUrl(object pictureData)
    {
        if (pictureData == null || string.IsNullOrEmpty(pictureData.ToString())) return "img/no-image.png";
        string base64String = pictureData.ToString();
        if (base64String.StartsWith("data:image")) return base64String;
        return "data:image/png;base64," + base64String;
    }
}