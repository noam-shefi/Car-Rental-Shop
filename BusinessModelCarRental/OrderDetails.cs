using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModelCarRental
{
    // OrderDetails
    public class OrderDetails
    {
        // OrderDetailID
        private int OrderDetailsID;
        public int MyOrderDetailsID { get { return this.OrderDetailsID; } set { this.OrderDetailsID = value; } }

        // OrderID
        private int OrderID;
        public int MyOrderID { get { return this.OrderID; } set { this.OrderID = value; } }

        // CarID
        private int CarID;
        public int MyCarID { get { return this.CarID; } set { this.CarID = value; } }

        // Quantity
        private int Quantity;
        public int MyQuantity { get { return this.Quantity; } set { this.Quantity = value; } }
    }
}
