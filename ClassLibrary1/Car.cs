using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModelCarRental
{
    // Car
    public class Car
    {
        // CarID
        private int CarID;
        public int MyCarID { get { return this.CarID; } set { this.CarID = value; } }

        // ModelName
        private string ModelName;
        public string MyModelName { get { return this.ModelName; } set { this.ModelName = value; } }

        // MakeName
        private int MakeID;
        public int MyMakeID { get { return this.MakeID; } set { this.MakeID = value; } }

        // Price
        private decimal Price;
        public decimal MyPrice { get { return this.Price; } set { this.Price = value; } }

        // FleetQuantity
        private int FleetQuantity;
        public int MyFleetQuantity { get { return this.FleetQuantity; } set { this.FleetQuantity = value; } }

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
