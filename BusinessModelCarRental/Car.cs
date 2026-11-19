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

        // VehicleCategoryID
        private int VehicleCategoryID;
        public int MyVehicleCategoryID { get { return this.VehicleCategoryID; } set { this.VehicleCategoryID = value; } }

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
    }
}
