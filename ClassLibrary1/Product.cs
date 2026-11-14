using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModelHardwareStore
{
    // Product
    public class Product
    {
        // ProductID
        private int ProductID;
        public int MyProductID { get { return this.ProductID; } set { this.ProductID = value; } }

        // ProductName
        private string ProductName;
        public string MyProductName { get { return this.ProductName; } set { this.ProductName = value; } }

        // CategoryID
        private int CategoryID;
        public int MyCategoryID { get { return this.CategoryID; } set { this.CategoryID = value; } }

        // BrandName
        private int BrandID;
        public int MyBrandID { get { return this.BrandID; } set { this.BrandID = value; } }

        // Price
        private decimal Price;
        public decimal MyPrice { get { return this.Price; } set { this.Price = value; } }

        // StockQuantity
        private int StockQuantity;
        public int MyStockQuantity { get { return this.StockQuantity; } set { this.StockQuantity = value; } }

        // Description
        private string Description;
        public string MyDescription { get { return this.Description; } set { this.Description = value; } }

        // Description
        private string Picture;
        public string MyPicture { get { return this.Picture; } set { this.Picture = value; } }

        public bool MyIsCar { get; set; }
        public int MyYear { get; set; }
        public string MyCarCategory { get; set; }
        public int? MyBranchID { get; set; }
        public string MyBranchName { get; set; }
        public string MyBranchCity { get; set; }
        public int? MySeats { get; set; }
        public string MyFuelType { get; set; }
        public int? MyRangeKm { get; set; }
        public string MyTransmission { get; set; }
        public int? MyLuggageCapacity { get; set; }
        public string MyManufacturer { get; set; }
        public DateTime? MyFavoriteDateAdded { get; set; }
        public double MyAverageRating { get; set; }
        public int MyReviewCount { get; set; }
    }

    public class Branch
    {
        public int MyBranchID { get; set; }
        public string MyBranchName { get; set; }
        public string MyCity { get; set; }
        public string MyAddress { get; set; }
        public string MyPhone { get; set; }
    }
}
