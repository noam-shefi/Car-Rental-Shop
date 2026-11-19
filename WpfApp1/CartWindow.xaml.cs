using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessModelCarRental;

namespace WpfApp1
{
    public partial class CartWindow : Window
    {
        private ServiceReference1.Service1Client ws = new ServiceReference1.Service1Client("BasicHttpBinding_IService1");
        private string selectedUsernameFromGrid = string.Empty;
        private int selectedProductIDFromGrid = -1;

        public CartWindow()
        {
            InitializeComponent();
            LoadCarts();
        }

        private void LoadCarts()
        {
            try
            {
                var list = ws.showAllCart();
                if (list != null)
                {
                    var cartList = list.ToList();
                    CartGrid.ItemsSource = cartList;

                    if (cartList.Count > 0)
                    {
                        int grandTotalQuantity = cartList.Sum(c => c.MyQuantity);
                        decimal grandTotalValue = cartList.Sum(c => c.MyPrice * c.MyQuantity);

                        LblTotalItems.Text = grandTotalQuantity.ToString();
                        LblTotalValue.Text = string.Format("{0:C}", grandTotalValue);
                    }
                    else
                    {
                        LblTotalItems.Text = "0";
                        LblTotalValue.Text = "$0.00";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shopping carts data: " + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = TxtUsername.Text.Trim();
                if (!int.TryParse(TxtProductID.Text.Trim(), out int carId) || string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show("Please enter a valid Username and Product ID.");
                    return;
                }

                int qty = int.TryParse(TxtQuantity.Text, out int q) ? q : 1;

                ws.AddCart(carId, username, qty);

                decimal realPrice = -1;

                if (decimal.TryParse(TxtPrice.Text, out decimal customPrice) && customPrice >= 0)
                {
                    realPrice = customPrice;
                }
                else
                {
                    var allProducts = ws.GetAllFleetCars();
                    if (allProducts != null)
                    {
                        var car = allProducts.FirstOrDefault(p => p.MyCarID == carId);
                        if (car != null)
                        {
                            realPrice = car.MyPrice;
                        }
                    }
                }

                if (realPrice >= 0)
                {
                    ws.UpdateCartPrice(username, carId, realPrice);
                }
                else
                {
                    MessageBox.Show("Warning: Item added, but could not retrieve the real price from Product table. Kept as -1.");
                }

                MessageBox.Show("Item added to cart successfully!");
                LoadCarts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Cart Error: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            if (!int.TryParse(TxtProductID.Text.Trim(), out int carId) || string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Missing valid Username and Product ID keys for update.");
                return;
            }

            if (!int.TryParse(TxtQuantity.Text, out int newQty))
            {
                MessageBox.Show("Please enter a valid integer for quantity.");
                return;
            }

            try
            {
                ws.UpdateCartQuantity(username, carId, newQty);

                if (decimal.TryParse(TxtPrice.Text, out decimal price))
                {
                    ws.UpdateCartPrice(username, carId, price);
                }

                MessageBox.Show("Cart line quantity updated.");
                LoadCarts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Cart Error: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            if (!int.TryParse(TxtProductID.Text.Trim(), out int carId) || string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please select a valid row to remove from cart.");
                return;
            }

            try
            {
                ws.DeleteCarFromCart(carId, username);
                MessageBox.Show("Product removed from this user's cart.");
                LoadCarts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Remove Item Error: " + ex.Message);
            }
        }

        private void BtnPurgeFromAllCarts_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TxtPurgeProductID.Text.Trim(), out int productIdToPurge))
            {
                MessageBox.Show("Please specify a valid Product ID to purge from all carts.");
                return;
            }

            string msg = $"Are you sure you want to completely erase Product ID [{productIdToPurge}] from ALL active customer carts?\n" +
                         "Use this only if the product has been discontinued and removed from sale.";

            if (MessageBox.Show(msg, "Global Purge Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    var allCarts = ws.showAllCart();
                    int counter = 0;

                    if (allCarts != null)
                    {
                        foreach (var cart in allCarts)
                        {
                            if (cart.MyCarID == productIdToPurge)
                            {
                                ws.DeleteCarFromCart(productIdToPurge, cart.MyUsername);
                                counter++;
                            }
                        }
                    }

                    MessageBox.Show($"Purge complete! Product [{productIdToPurge}] was removed from {counter} active user carts.");
                    LoadCarts();
                    TxtPurgeProductID.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Purge Error: " + ex.Message);
                }
            }
        }

        private void BtnWipeUserCart_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please type or select a user to empty their entire cart.");
                return;
            }

            if (MessageBox.Show($"Clear all items inside '{username}'s shopping cart?", "Empty Cart", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    ws.DeleteCart(username);
                    MessageBox.Show("Customer's cart is now completely empty.");
                    LoadCarts();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wipe Error: " + ex.Message);
                }
            }
        }

        private void BtnPlusOne_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            if (int.TryParse(TxtProductID.Text.Trim(), out int pid) && !string.IsNullOrWhiteSpace(username))
            {
                ws.IncreaseQuantityByOne(username, pid);
                LoadCarts();
                RefreshFormAfterQuickAction(username, pid);
            }
        }

        private void BtnMinusOne_Click(object sender, RoutedEventArgs e)
        {
            string username = TxtUsername.Text.Trim();
            if (int.TryParse(TxtProductID.Text.Trim(), out int pid) && !string.IsNullOrWhiteSpace(username))
            {
                ws.DecreaseQuantityByOne(username, pid);
                LoadCarts();
                RefreshFormAfterQuickAction(username, pid);
            }
        }

        private void RefreshFormAfterQuickAction(string user, int pid)
        {
            var updated = ws.GetCarInCart(pid, user);
            if (updated != null)
            {
                TxtQuantity.Text = updated.MyQuantity.ToString();
            }
            else
            {
                ClearFields();
            }
        }

        private void CartGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CartGrid.SelectedItem != null)
            {
                try
                {
                    dynamic selectedCart = CartGrid.SelectedItem;

                    selectedUsernameFromGrid = selectedCart.MyUsername;
                    selectedProductIDFromGrid = selectedCart.MyCarID;

                    TxtUsername.Text = selectedCart.MyUsername;
                    TxtProductID.Text = selectedCart.MyCarID.ToString();
                    TxtQuantity.Text = selectedCart.MyQuantity.ToString();
                    TxtPrice.Text = selectedCart.MyPrice.ToString();
                }
                catch
                {
                    MessageBox.Show("Error reading selected cart item data.");
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => ClearFields();

        private void ClearFields()
        {
            CartGrid.SelectionChanged -= CartGrid_SelectionChanged;

            selectedUsernameFromGrid = string.Empty;
            selectedProductIDFromGrid = -1;
            TxtUsername.Clear();
            TxtProductID.Clear();
            TxtQuantity.Clear();
            TxtPrice.Clear();
            CartGrid.SelectedItem = null;

            CartGrid.SelectionChanged += CartGrid_SelectionChanged;
        }
    }
}