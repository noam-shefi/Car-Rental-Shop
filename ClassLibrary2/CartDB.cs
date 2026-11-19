using BusinessModelCarRental;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace ViewModelCarRental
{
    public class CartDB
    {
        public DBfunction db = new DBfunction();

        public CartDB() { }

        // isExistCart
        public bool isExistCart(string Username)
        {
            string sql = "SELECT * FROM Cart WHERE Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // AddCart
        public void AddCart(int CarID, string Username, int Quantity)
        {
            string getPriceSql = "SELECT Price FROM Product WHERE ProductID = ?";
            OleDbCommand getPriceCmd = new OleDbCommand(getPriceSql);
            getPriceCmd.Parameters.AddWithValue("?", CarID);

            DataSet ds = db.SelectcommandInjection(getPriceCmd);

            if (ds.Tables[0].Rows.Count == 0)
                return;

            decimal price = Convert.ToDecimal(ds.Tables[0].Rows[0]["Price"]);

            string sql = @"INSERT INTO Cart (ProductID, Username, Price, Quantity)
                   VALUES (?, ?, ?, ?)";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", price);
            cmd.Parameters.AddWithValue("?", Quantity);

            db.DoCommandInjection(cmd);
        }

        // DeleteCart
        public void DeleteCart(string Username)
        {
            string sql = "DELETE FROM Cart WHERE Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            db.DoCommandInjection(cmd);
        }

        // showAllCart
        public List<Cart> showAllCart()
        {
            string sql = "SELECT * FROM Cart";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);
            List<Cart> list = new List<Cart>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Cart
                {
                    MyUsername = row["Username"].ToString(),
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyQuantity = Convert.ToInt32(row["Quantity"]),
                    MyPrice = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0
                });
            }
            return list;
        }

        // showCart
        public List<Cart> showCart(string Username)
        {
            string sql = "SELECT * FROM Cart WHERE Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            List<Cart> list = new List<Cart>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Cart
                {
                    MyUsername = row["Username"].ToString(),
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyQuantity = Convert.ToInt32(row["Quantity"]),
                    MyPrice = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0
                });
            }
            return list;
        }

        // DeleteCarFromCart
        public void DeleteCarFromCart(int CarID, string Username)
        {
            string sql = "DELETE FROM Cart WHERE ProductID = ? AND Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Username);
            db.DoCommandInjection(cmd);
        }

        // CarExistsInCart
        public bool CarExistsInCart(int CarID, string Username)
        {
            string sql = "SELECT * FROM Cart WHERE ProductID = ? AND Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // CopyData
        public void CopyData(string Username, decimal Price)
        {
            // דוגמה למימוש: הכנסה לטבלת Summary או Orders
            string sql = "INSERT INTO OrdersSummary (Username, TotalPrice, OrderDate) VALUES (?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", Price);
            cmd.Parameters.AddWithValue("?", DateTime.Now);
            db.DoCommandInjection(cmd);
        }

        // UpdateCartQuantity
        public bool UpdateCartQuantity(string Username, int CarID, int newQuantity)
        {
            string sql = "UPDATE Cart SET Quantity = ? WHERE ProductID = ? AND Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", newQuantity);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Username);
            db.DoCommandInjection(cmd);
            return true;
        }

        // GetCartTotalPrice
        public decimal GetCartTotalPrice(string Username)
        {
            string sql = "SELECT SUM(Price * Quantity) FROM Cart WHERE Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
            }
            return 0;
        }

        // CountItemsInCart
        public int CountItemsInCart(string Username)
        {
            string sql = "SELECT SUM(Quantity) FROM Cart WHERE Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            return 0;
        }

        // ListCartWithCarDetails
        public List<Cart> ListCartWithCarDetails(string Username)
        {
            string sql = @"SELECT Cart.ProductID, Product.Price, Cart.Quantity
                           FROM Cart
                           INNER JOIN Product ON Cart.ProductID = Product.ProductID
                           WHERE Cart.Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            List<Cart> list = new List<Cart>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Cart
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyQuantity = Convert.ToInt32(row["Quantity"]),
                    MyUsername = Username
                });
            }
            return list;
        }

        // GetCarTotalPriceInCart
        public decimal GetCarTotalPriceInCart(string Username, int CarID)
        {
            string sql = @"SELECT Product.Price * Cart.Quantity
                           FROM Cart
                           INNER JOIN Product ON Cart.ProductID = Product.ProductID
                           WHERE Cart.Username = ? AND Cart.ProductID = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", CarID);
            DataSet ds = db.SelectcommandInjection(cmd);
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                return Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
            }
            return 0;
        }

        // UpdateCartPrice
        public bool UpdateCartPrice(string Username, int CarID, decimal NewPrice)
        {
            string sql = "UPDATE Cart SET Price = ? WHERE ProductID = ? AND Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", NewPrice);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Username);
            db.DoCommandInjection(cmd);
            return true;
        }

        // showCartWithoutUsername
        public Cart showCartWithoutUsername(string Username)
        {
            string sql = "SELECT TOP 1 * FROM Cart WHERE Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return new Cart
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyQuantity = Convert.ToInt32(row["Quantity"]),
                    MyPrice = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0
                    // MyUsername לא מושם כאן לבקשת השם
                };
            }
            return null;
        }

        // GetCarInCart
        public Cart GetCarInCart(int CarID, string Username)
        {
            string sql = "SELECT * FROM Cart WHERE ProductID = ? AND Username = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return new Cart
                {
                    MyUsername = row["Username"].ToString(),
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyQuantity = Convert.ToInt32(row["Quantity"]),
                    MyPrice = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0
                };
            }
            return null;
        }

        // IncreaseQuantityByOne
        public void IncreaseQuantityByOne(string Username, int CarID)
        {
            string sql = "UPDATE Cart SET Quantity = Quantity + 1 WHERE Username = ? AND ProductID = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", CarID);
            db.DoCommandInjection(cmd);
        }

        // DecreaseQuantityByOne
        public void DecreaseQuantityByOne(string Username, int CarID)
        {
            string sql = "UPDATE Cart SET Quantity = Quantity - 1 WHERE Username = ? AND ProductID = ? AND Quantity > 0";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", CarID);
            db.DoCommandInjection(cmd);
        }
    }
}