using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Data;
using BusinessModelCarRental;

namespace WpfApp1
{
    public partial class CarInventoryWindow : Window
    {
        private ServiceReference1.Service1Client ws = new ServiceReference1.Service1Client("BasicHttpBinding_IService1");
        private int selectedProductIDFromGrid = -1;
        private string stImage = "";

        public CarInventoryWindow()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var list = ws.GetAllFleetCars();
                if (list != null)
                    CarGrid.ItemsSource = list.ToList();
            }
            catch (Exception ex) { MessageBox.Show("Error loading vehicle inventory: " + ex.Message); }
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    stImage = Convert.ToBase64String(imageBytes);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(imageBytes);
                    bitmap.EndInit();
                    ImgPreview.Source = bitmap;
                }
                catch (Exception ex) { MessageBox.Show("Image Error: " + ex.Message); }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TxtModelName.Text) || string.IsNullOrWhiteSpace(TxtCategoryID.Text))
                {
                    MessageBox.Show("Please fill Vehicle Name and Category ID.");
                    return;
                }

                int catID = int.Parse(TxtCategoryID.Text);
                int makeID = int.Parse(TxtBrandID.Text);
                decimal price = decimal.Parse(TxtPrice.Text);
                int fleetQuantity = int.Parse(TxtStock.Text);
                int nextID = ws.GetNextCarID(catID);

                ws.CreateFleetCar(nextID, TxtModelName.Text, catID, makeID, price, fleetQuantity, TxtDescription.Text, stImage);

                MessageBox.Show($"Legacy catalog item {nextID} added. Add rental cars on the website.");
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex) { MessageBox.Show("Add Error: " + ex.Message); }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int idToUpdate;
            if (!int.TryParse(TxtTargetID.Text, out idToUpdate))
                idToUpdate = selectedProductIDFromGrid;

            if (idToUpdate == -1) { MessageBox.Show("Select a vehicle."); return; }

            try
            {
                var current = ws.GetFleetCarByID(idToUpdate);
                if (current == null) return;

                string name = string.IsNullOrWhiteSpace(TxtModelName.Text) ? current.MyModelName : TxtModelName.Text;
                int catID = string.IsNullOrWhiteSpace(TxtCategoryID.Text) ? current.MyVehicleCategoryID : int.Parse(TxtCategoryID.Text);
                int makeID = string.IsNullOrWhiteSpace(TxtBrandID.Text) ? current.MyMakeID : int.Parse(TxtBrandID.Text);
                decimal price = string.IsNullOrWhiteSpace(TxtPrice.Text) ? current.MyPrice : decimal.Parse(TxtPrice.Text);
                int fleetQuantity = string.IsNullOrWhiteSpace(TxtStock.Text) ? current.MyFleetQuantity : int.Parse(TxtStock.Text);
                string desc = string.IsNullOrWhiteSpace(TxtDescription.Text) ? current.MyDescription : TxtDescription.Text;

                string picToSave = string.IsNullOrEmpty(stImage) ? current.MyPicture : stImage;

                ws.UpdateFleetCar(name, catID, makeID, price, fleetQuantity, desc, picToSave, idToUpdate);

                MessageBox.Show("Updated!");
                LoadProducts();
                ClearFields();
            }
            catch (Exception ex) { MessageBox.Show("Update Error: " + ex.Message); }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            int idToDelete;
            if (!int.TryParse(TxtTargetID.Text, out idToDelete))
                idToDelete = selectedProductIDFromGrid;

            if (idToDelete == -1) return;

            if (MessageBox.Show($"Delete vehicle ID {idToDelete}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    ws.DeleteFleetCar(idToDelete);
                    LoadProducts();
                    ClearFields();
                }
                catch (Exception ex) { MessageBox.Show("Delete Error: " + ex.Message); }
            }
        }

        private void CarGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarGrid.SelectedItem is Car selected)
            {
                selectedProductIDFromGrid = selected.MyCarID;
                TxtTargetID.Text = selected.MyCarID.ToString();
                TxtModelName.Text = selected.MyModelName;
                TxtCategoryID.Text = selected.MyVehicleCategoryID.ToString();
                TxtBrandID.Text = selected.MyMakeID.ToString();
                TxtPrice.Text = selected.MyPrice.ToString();
                TxtStock.Text = selected.MyFleetQuantity.ToString();
                TxtDescription.Text = selected.MyDescription;

                if (!string.IsNullOrEmpty(selected.MyPicture))
                {
                    try
                    {
                        byte[] imageBytes = Convert.FromBase64String(selected.MyPicture);
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = new MemoryStream(imageBytes);
                        bitmap.EndInit();
                        ImgPreview.Source = bitmap;
                        stImage = selected.MyPicture;
                    }
                    catch { ImgPreview.Source = null; stImage = ""; }
                }
                else { ImgPreview.Source = null; stImage = ""; }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => ClearFields();

        private void ClearFields()
        {
            selectedProductIDFromGrid = -1;
            TxtTargetID.Clear();
            TxtModelName.Clear();
            TxtCategoryID.Clear();
            TxtBrandID.Clear();
            TxtPrice.Clear();
            TxtStock.Clear();
            TxtDescription.Clear();
            stImage = "";
            ImgPreview.Source = null;
            CarGrid.SelectedItem = null;
        }
    }

    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string base64String = value as string;
            if (string.IsNullOrEmpty(base64String)) return null;

            try
            {
                if (base64String.Contains(","))
                {
                    base64String = base64String.Substring(base64String.IndexOf(",") + 1);
                }

                byte[] imageBytes = System.Convert.FromBase64String(base64String);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(imageBytes);
                bitmap.EndInit();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}