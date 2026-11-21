using System;
using System.ServiceModel;
using System.Windows;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            try
            {
                using (var ws = new ServiceReference1.Service1Client("BasicHttpBinding_IService1"))
                {
                    if (!ws.IsExistUserAndPass(username, password))
                    {
                        ErrorText.Text = "Invalid username or password.";
                        ErrorText.Visibility = Visibility.Visible;
                        return;
                    }

                    var user = ws.showUser(username);
                    bool isAdmin = (user != null && user.MyAdmin) || ws.IsUserAdmin(username);

                    if (!isAdmin)
                    {
                        ErrorText.Text = "You must be an admin.";
                        ErrorText.Visibility = Visibility.Visible;
                        return;
                    }

                    MainWindow mainWindow = new MainWindow(username);
                    Application.Current.MainWindow = mainWindow;
                    mainWindow.Show();
                    Close();
                }
            }
            catch (EndpointNotFoundException)
            {
                ErrorText.Text = "WCF service is not running. Start WcfServiceCarRental and try again.";
                ErrorText.Visibility = Visibility.Visible;
            }
            catch (TimeoutException)
            {
                ErrorText.Text = "WCF service timeout. Make sure WcfServiceCarRental is running.";
                ErrorText.Visibility = Visibility.Visible;
            }
            catch (CommunicationException ex)
            {
                ErrorText.Text = "WCF error: " + ex.Message;
                ErrorText.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ErrorText.Text = "Unexpected error: " + ex.Message;
                ErrorText.Visibility = Visibility.Visible;
            }
        }
    }
}
