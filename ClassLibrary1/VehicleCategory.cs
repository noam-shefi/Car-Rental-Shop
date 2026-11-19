using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModelCarRental
{
    // VehicleCategory
    public class VehicleCategory
    {
        // VehicleCategoryID
        private int VehicleCategoryID;
        public int MyVehicleCategoryID { get { return this.VehicleCategoryID; } set { this.VehicleCategoryID = value; } }

        // VehicleCategoryName
        private string VehicleCategoryName;
        public string MyVehicleCategoryName { get { return this.VehicleCategoryName; } set { this.VehicleCategoryName = value; } }

        // Description
        private string Description;
        public string MyDescription { get { return this.Description; } set { this.Description = value; } }
    }
}
