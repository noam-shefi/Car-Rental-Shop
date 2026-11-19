using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CarCatalogDetails : System.Web.UI.Page
{
    private readonly ServiceReference1.Service1Client _ws = new ServiceReference1.Service1Client("BasicHttpBinding_IService1");
    private int _productID;

    protected void Page_Load(object sender, EventArgs e)
    {
        int.TryParse(Request.QueryString["id"], out _productID);
        Response.Redirect("Cars.aspx");
    }

    private void LoadProductDetails()
    {
        var allProducts = _ws.GetAllFleetCars();
        if (allProducts == null)
        {
            Response.Redirect("ShopUser.aspx");
            return;
        }

        var car = allProducts.FirstOrDefault(p => p.MyCarID == _productID);
        if (car == null)
        {
            Response.Redirect("ShopUser.aspx");
            return;
        }

        lblProductName.InnerText = car.MyModelName;
        lblDescription.InnerText = car.MyDescription;
        lblPrice.InnerText = "₪" + car.MyPrice;
        imgProduct.ImageUrl = GetImageUrl(car.MyPicture);

        var allCategories = _ws.GetAllVehicleCategories();
        if (allCategories != null)
        {
            var vehicleCategory = allCategories.FirstOrDefault(c => c.MyVehicleCategoryID == car.MyVehicleCategoryID);
            litCategory.Text = vehicleCategory != null ? vehicleCategory.MyVehicleCategoryName : "General";
        }
        else
        {
            litCategory.Text = "General";
        }

        var allBrands = _ws.GetAllMakes();
        if (allBrands != null)
        {
            var make = allBrands.FirstOrDefault(b => b.MyMakeID == car.MyMakeID);
            litBrand.Text = make != null ? make.MyMakeName : "Generic";
        }
        else
        {
            litBrand.Text = "Generic";
        }
    }

    private void LoadReviews()
    {
        string currentUsername = Session["Username"].ToString();

        var reviewsArray = _ws.ShowAllCarReviews(_productID);

        rptReviews.DataSource = reviewsArray;
        rptReviews.DataBind();

        bool userAlreadyReviewed = false;
        BusinessModelCarRental.Review userReview = null;

        if (reviewsArray != null && reviewsArray.Length > 0)
        {
            userReview = reviewsArray.FirstOrDefault(r => r.MyUsername.Equals(currentUsername, StringComparison.OrdinalIgnoreCase));
            if (userReview != null)
            {
                userAlreadyReviewed = true;
            }
        }

        if (userAlreadyReviewed && ViewState["IsEditing"] == null)
        {
            lblFormTitle.InnerText = "Your Review";
            lblFormSubtitle.InnerText = "You have already reviewed this product. You can update or delete your review below.";
            txtReviewContent.Text = userReview.MyRev;
            ddlRating.SelectedValue = userReview.MyRating.ToString();

            txtReviewContent.Enabled = false;
            ddlRating.Enabled = false;
            btnSubmitReview.Visible = false;
        }
        else if (ViewState["IsEditing"] == null)
        {
            lblFormTitle.InnerText = "Add a Review";
            lblFormSubtitle.InnerText = "Share your thoughts about this product with other users.";
            txtReviewContent.Text = "";
            ddlRating.SelectedIndex = 0;

            txtReviewContent.Enabled = true;
            ddlRating.Enabled = true;
            btnSubmitReview.Visible = true;
            btnSubmitReview.Text = "Submit Review";
            btnCancelEdit.Visible = false;
        }
    }

    protected void btnSubmitReview_Click(object sender, EventArgs e)
    {
        string username = Session["Username"].ToString();
        string reviewText = txtReviewContent.Text.Trim();
        int rating = int.Parse(ddlRating.SelectedValue);

        if (string.IsNullOrEmpty(reviewText))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please write some review content before submitting.');", true);
            return;
        }

        try
        {
            if (ViewState["IsEditing"] != null && ViewState["EditReviewID"] != null)
            {
                int reviewID = (int)ViewState["EditReviewID"];
                _ws.UpdateCarReview(username, _productID, reviewText, rating, reviewID);

                ViewState.Remove("IsEditing");
                ViewState.Remove("EditReviewID");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Review updated successfully!'); window.location='" + Request.RawUrl + "';", true);
            }
            else
            {
                _ws.AddCarReview(username, _productID, reviewText, rating);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Review added successfully!'); window.location='" + Request.RawUrl + "';", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error saving review: " + ex.Message + "');", true);
        }
    }

    protected void rptReviews_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var reviewItem = (BusinessModelCarRental.Review)e.Item.DataItem;
            string reviewOwner = reviewItem.MyUsername;
            string currentUsername = Session["Username"].ToString();

            if (reviewOwner.Equals(currentUsername, StringComparison.OrdinalIgnoreCase))
            {
                LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");
                LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

                if (lnkEdit != null) lnkEdit.Visible = true;
                if (lnkDelete != null) lnkDelete.Visible = true;
            }
        }
    }

    protected void rptReviews_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int reviewID = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "DeleteReview")
        {
            try
            {
                _ws.DeleteReview(reviewID);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Review deleted successfully!'); window.location='" + Request.RawUrl + "';", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error deleting review: " + ex.Message + "');", true);
            }
        }
        else if (e.CommandName == "EditReview")
        {
            var review = _ws.ShowReview(reviewID);
            if (review != null)
            {
                ViewState["IsEditing"] = true;
                ViewState["EditReviewID"] = reviewID;

                lblFormTitle.InnerText = "Edit Your Review";
                lblFormSubtitle.InnerText = "Modify your comment or rating for this product.";
                txtReviewContent.Text = review.MyRev;
                ddlRating.SelectedValue = review.MyRating.ToString();

                txtReviewContent.Enabled = true;
                ddlRating.Enabled = true;
                btnSubmitReview.Visible = true;
                btnSubmitReview.Text = "Update Review";
                btnCancelEdit.Visible = true;
            }
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        ViewState.Remove("IsEditing");
        ViewState.Remove("EditReviewID");
        Response.Redirect(Request.RawUrl);
    }

    protected void btnAddToCart_Click(object sender, EventArgs e)
    {
        string username = Session["Username"] != null ? Session["Username"].ToString() : "guest";

        try
        {
            if (_ws.CarExistsInCart(_productID, username))
            {
                var cartItems = _ws.ListCartWithCarDetails(username);
                if (cartItems != null)
                {
                    var existingItem = cartItems.FirstOrDefault(p => p.MyCarID == _productID);
                    if (existingItem != null)
                    {
                        int newQuantity = existingItem.MyQuantity + 1;
                        _ws.UpdateCartQuantity(username, _productID, newQuantity);
                    }
                }
            }
            else
            {
                _ws.AddCart(_productID, username, 1);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert",
                "alert('Product added to cart successfully!'); window.location='" + Request.RawUrl + "';", true);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('Error adding product to cart: " + ex.Message + "');</script>");
        }
    }

    private string GetImageUrl(object pictureData)
    {
        if (pictureData == null || string.IsNullOrEmpty(pictureData.ToString())) return "img/no-image.png";
        string base64String = pictureData.ToString();
        if (base64String.StartsWith("data:image")) return base64String;
        return "data:image/png;base64," + base64String;
    }
}