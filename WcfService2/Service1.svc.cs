using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ViewModelCarRental;

namespace WcfService2
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        // Make
        public bool MakeExists(int MakeID)
        {
            MakeDB b = new MakeDB();
            return b.MakeExists(MakeID);
        }
        public void AddMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description)
        {
            MakeDB b = new MakeDB();
            b.AddMake(MakeName, CountryOfOrigin, WebsiteURL, Description);
        }
        public void DeleteMake(int MakeID)
        {
            MakeDB b = new MakeDB();
            b.DeleteMake(MakeID);
        }
        public void UpdateMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description, int MakeID)
        {
            MakeDB b = new MakeDB();
            b.UpdateMake(MakeName, CountryOfOrigin, WebsiteURL, Description, MakeID);
        }
        public DataSet GetAllMakes()
        {
            MakeDB b = new MakeDB();
            return b.GetAllMakes();
        }
        public DataSet GetMake(int MakeID)
        {
            MakeDB b = new MakeDB();
            return b.GetMake(MakeID);
        }

        // Cart
        public bool isExistCart(int CarID, string Username)
        {
            CartDB c = new CartDB();
            return c.isExistCart(CarID, Username);
        }
        public void AddCart(int CarID, string Username, decimal Price, string Picture, int Quantity)
        {
            CartDB c = new CartDB();
            c.AddCart(CarID, Username, Price, Picture, Quantity);
        }
        public void DeleteCart(string Username)
        {
            CartDB c = new CartDB();
            c.DeleteCart(Username);
        }
        public void UpdateCart(int Quantity, string CarID, string Username)
        {
            CartDB c = new CartDB();
            c.UpdateCart(Quantity, CarID, Username);
        }
        public DataSet showAllCart()
        {
            CartDB c = new CartDB();
            return c.showAllCart();
        }
        public DataSet showCart(int CarID, string Username)
        {
            CartDB c = new CartDB();
            return c.showCart(CarID, Username);
        }
        public void DeleteCarFromCart(string CarID, string Username)
        {
            CartDB c = new CartDB();
            c.DeleteCarFromCart(CarID, Username);
        }
        public bool CarExistsInCart(int CarID, string Username)
        {
            CartDB c = new CartDB();
            return c.CarExistsInCart(CarID, Username);
        }
        public void CopyData(string Username, decimal Price)
        {
            CartDB c = new CartDB();
            c.CopyData(Username, Price);
        }
        public string OrderInfo(string Username, decimal Price)
        {
            CartDB c = new CartDB();
            return c.OrderInfo(Username, Price);
        }
        public bool UpdateCartQuantity(string Username, string CarID, int newQuantity)
        {
            CartDB c = new CartDB();
            return c.UpdateCartQuantity(Username, CarID, newQuantity);
        }
        public decimal GetCartTotalPrice(string Username)
        {
            CartDB c = new CartDB();
            return c.GetCartTotalPrice(Username);
        }
        public int CountItemsInCart(string Username)
        {
            CartDB c = new CartDB();
            return c.CountItemsInCart(Username);
        }
        public DataSet ListCartWithCarDetails(string Username)
        {
            CartDB c = new CartDB();
            return c.ListCartWithCarDetails(Username);
        }

    }
}
