using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyFavorites : Page
{
    private string username;

    protected void Page_Load(object sender, EventArgs e)
    {
        username = Convert.ToString(Session["username"] ?? Session["Username"]);
        if (string.IsNullOrWhiteSpace(username))
        {
            Response.Redirect("SignInVisitor.aspx?ReturnUrl=" + HttpUtility.UrlEncode("MyFavorites.aspx"));
            return;
        }
        if (!IsPostBack)
            BindFavorites();
    }

    private void BindFavorites()
    {
        try
        {
            using (var service = new CarRentalServiceClient())
                rptFavorites.DataSource = service.GetFavorites(username);
            rptFavorites.DataBind();
            pnlEmpty.Visible = rptFavorites.Items.Count == 0;
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "טעינת המועדפים נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected void rptFavorites_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "RemoveFavorite")
            return;
        try
        {
            using (var service = new CarRentalServiceClient())
                service.SetFavorite(username, Convert.ToInt32(e.CommandArgument), false);
            BindFavorites();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            lblMessage.Text = "הסרת הרכב מהמועדפים נכשלה: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }

    protected string GetImageUrl(object pictureData)
    {
        string picture = Convert.ToString(pictureData);
        if (string.IsNullOrWhiteSpace(picture)) return ResolveUrl("~/img/no-image.svg");
        if (picture.StartsWith("data:image", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || picture.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return picture;
        if (picture.StartsWith("~/", StringComparison.Ordinal)) return ResolveUrl(picture);
        return "data:image/jpeg;base64," + picture;
    }
}
