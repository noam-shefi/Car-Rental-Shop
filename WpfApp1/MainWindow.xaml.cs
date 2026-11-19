using System;
using System.Windows;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string adminUsername;

        public MainWindow(string username)
        {
            InitializeComponent();
            this.adminUsername = username;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WelcomeHeader.Text = $"Hello {adminUsername}";

            StatusLabel.Content = $"Logged in as Administrator: {adminUsername} | Connected to Web Service | {DateTime.Now.ToShortDateString()}";
        }


        // 1. Products
        private void OpenCars_Click(object sender, RoutedEventArgs e)
        {
            CarInventoryWindow win = new CarInventoryWindow();
            win.Show();
        }

        // 2. Brands
        private void OpenBrands_Click(object sender, RoutedEventArgs e)
        {
            MakeWindow win = new MakeWindow();
            win.Show();
        }

        // 3. Categories
        private void OpenCategories_Click(object sender, RoutedEventArgs e)
        {
            VehicleCategoryWindow win = new VehicleCategoryWindow();
            win.Show();
        }

        // 4. Contact
        private void OpenContacts_Click(object sender, RoutedEventArgs e)
        {
            ContactWindow win = new ContactWindow();
            win.Show();
        }

        // 5. Users
        private void OpenUsers_Click(object sender, RoutedEventArgs e)
        {
            UserWindow win = new UserWindow();
            win.Show();
        }

        // 6. Orders
        private void OpenOrders_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow win = new OrderWindow();
            win.Show();
        }

        // 7. Order Details
        private void OpenOrderDetails_Click(object sender, RoutedEventArgs e)
        {
            OrderDetailWindow win = new OrderDetailWindow();
            win.Show();
        }

        // 8. Cart
        private void OpenCarts_Click(object sender, RoutedEventArgs e)
        {
            CartWindow win = new CartWindow();
            win.Show();
        }

        // 9. Reviews
        private void OpenReviews_Click(object sender, RoutedEventArgs e)
        {
            ReviewWindow win = new ReviewWindow();
            win.Show();
        }

        // 10. Shipping Addresses
        private void OpenShippingAddresses_Click(object sender, RoutedEventArgs e)
        {
            ShippingAddressWindow win = new ShippingAddressWindow();
            win.Show();
        }
    }
}