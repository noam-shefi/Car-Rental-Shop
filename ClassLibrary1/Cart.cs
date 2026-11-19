using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessModelCarRental
{
    // Cart
    public class Cart
    {
        // CarID
        private int CarID;
        public int MyCarID { get { return this.CarID; } set { this.CarID = value; } }

        // Username
        private string Username;
        public string MyUsername { get { return this.Username; } set { this.Username = value; } }

        // Price
        private decimal Price;
        public decimal MyPrice { get { return this.Price; } set { this.Price = value; } }

        // Quantity
        private int Quantity;
        public int MyQuantity { get { return this.Quantity; } set { this.Quantity = value; } }
    }
}
