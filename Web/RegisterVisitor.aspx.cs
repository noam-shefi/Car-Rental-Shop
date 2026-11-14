using System;
using System.Web;
using System.Web.UI;

public partial class Register : Page
{
    protected void Button1_Click(object sender, EventArgs e)
    {
        string username = txtusername.Text.Trim();
        string firstName = txtFirstName.Text.Trim();
        string lastName = txtLastName.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPass.Text;

        if (username.Length < 3 || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email) || password.Length < 6 || password != txtConfirmPass.Text)
        {
            ShowMessage("יש להזין שם משתמש בן 3 תווים לפחות, שם פרטי ושם משפחה, דוא״ל, סיסמה בת 6 תווים לפחות ואישור סיסמה תואם.");
            return;
        }
        if (email.IndexOf('@') < 0)
        {
            ShowMessage("יש להזין כתובת דוא״ל הכוללת @.");
            return;
        }

        try
        {
            using (var accountService = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
            {
                if (accountService.isExistUser(username))
                {
                    ShowMessage("שם המשתמש כבר רשום.");
                    return;
                }
            }

            using (var rentalService = new CarRentalServiceClient())
            {
                if (rentalService.IsEmailRegistered(email, string.Empty))
                {
                    ShowMessage("כתובת הדוא״ל כבר רשומה.");
                    return;
                }
                rentalService.RegisterCustomer(username, password, txtPhoneNumber.Text.Trim(), email, firstName, lastName);
            }

            Session["username"] = username;
            Session["Username"] = username;
            Session["isAdmin"] = false;
            Session["Email"] = email;
            Session["PhoneNumber"] = txtPhoneNumber.Text.Trim();
            Response.Redirect("Cars.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (System.ServiceModel.CommunicationException ex)
        {
            ShowMessage("הרישום נכשל: " + HttpUtility.HtmlEncode(ex.Message));
        }
    }

    private void ShowMessage(string message)
    {
        lblInfo.Visible = true;
        lblInfo.Text = message;
    }
}