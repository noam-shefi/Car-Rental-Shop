using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace ViewModelCarRental
{
    public class CarDB
    {
        public CarDB() { }
        public DBfunction db = new DBfunction();

        // FleetCarExists
        public bool FleetCarExists(int CarID)
        {
            string sql = string.Format("select * from [Product] where [ProductID]={0}", CarID);
            DataSet ds = db.Selectcommand(sql);
            return ds.Tables[0].Rows.Count >= 1;
        }

        // GetNextCarID
        public int GetNextCarID(int VehicleCategoryID)
        {
            int baseNumber = VehicleCategoryID * 10000;
            int nextID = baseNumber + 1;

            string sql = string.Format(
                "SELECT MAX(ProductID) FROM Product WHERE CategoryID = {0} AND ProductID >= {1} AND ProductID < {2}",
                VehicleCategoryID, baseNumber, baseNumber + 10000);

            DataSet ds = db.Selectcommand(sql);
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                int maxID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (maxID >= baseNumber)
                {
                    nextID = maxID + 1;
                }
            }

            return nextID;
        }

        // CreateFleetCar
        public void CreateFleetCar(int CarID, string ModelName, int VehicleCategoryID, int MakeID, decimal Price, int FleetQuantity, string Description)
        {
            string sql = string.Format(
                "insert into [Product] ([ProductID], [ProductName], [CategoryID], [BrandID], [Price], [StockQuantity], [Description]) " +
                "values ({0}, '{1}', {2}, {3}, {4}, {5}, '{6}')",
                CarID, ModelName, VehicleCategoryID, MakeID, Price, FleetQuantity, Description);
            db.DoCommand(sql);
        }

        // DeleteFleetCar
        public void DeleteFleetCar(int CarID)
        {
            string sql = string.Format("delete from [Product] where [ProductID]={0}", CarID);
            db.DoCommand(sql);
        }

        // UpdateFleetCar
        public void UpdateFleetCar(string ModelName, int VehicleCategoryID, int MakeID, decimal Price, int FleetQuantity, string Description, int CarID)
        {
            string sql = string.Format(
                "update [Product] set [ProductName]='{0}', [CategoryID]={1}, [BrandID]={2}, [Price]={3}, [StockQuantity]={4}, [Description]='{5}' where [ProductID]={6}",
                ModelName, VehicleCategoryID, MakeID, Price, FleetQuantity, Description, CarID);
            db.DoCommand(sql);
        }

        // GetAllFleetCars
        public DataSet GetAllFleetCars()
        {
            string sql = "select * from [Product]";
            return db.Selectcommand(sql);
        }

        // GetFleetCarByID
        public DataSet GetFleetCarByID(int CarID)
        {
            string sql = string.Format("select * from [Product] where [ProductID]={0}", CarID);
            return db.Selectcommand(sql);
        }

        // GetFleetCarsWithCategoryAndMake
        public DataSet GetFleetCarsWithCategoryAndMake()
        {
            string sql = string.Format(
                "SELECT Product.ProductID, Product.ProductName, Category.CategoryName, Brand.BrandName, " +
                "Product.Price, Product.Description, Product.StockQuantity, Product.Picture " +
                "FROM Category " +
                "INNER JOIN Product ON Category.CategoryID = Product.CategoryID " +
                "INNER JOIN Brand ON Brand.BrandID = Product.BrandID");

            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // GetFleetCarsByCategoryID
        public DataSet GetFleetCarsByCategoryID(int VehicleCategoryID)
        {
            string sql = string.Format("select * from [Product] where [CategoryID]={0}", VehicleCategoryID);
            return db.Selectcommand(sql);
        }

        // GetFleetCarsByMakeID
        public DataSet GetFleetCarsByMakeID(int MakeID)
        {
            string sql = string.Format("select * from [Product] where [BrandID]={0}", MakeID);
            return db.Selectcommand(sql);
        }

        // GetFleetCarsByRateDescending
        public DataSet GetFleetCarsByRateDescending()
        {
            string sql = "select * from [Product] order by [Price] DESC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // GetFleetCarsByRateAscending
        public DataSet GetFleetCarsByRateAscending()
        {
            string sql = "select * from [Product] order by [Price] ASC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // SearchFleetCars
        public DataSet SearchFleetCars(string SearchTerm)
        {
            string sql = string.Format("SELECT * FROM [Product] WHERE [ProductName] LIKE '%{0}%' OR [Description] LIKE '%{0}%'", SearchTerm);
            return db.Selectcommand(sql);
        }

        // FilterFleetCarsByRateRange
        public DataSet FilterFleetCarsByRateRange(decimal MinPrice, decimal MaxPrice)
        {
            string sql = string.Format("SELECT * FROM [Product] WHERE [Price] BETWEEN {0} AND {1}", MinPrice, MaxPrice);
            return db.Selectcommand(sql);
        }

        // GetFleetCarsUnder50
        public DataSet GetFleetCarsUnder50()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] <= 50 ORDER BY [Price] ASC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // GetFleetCarsBetween51And100
        public DataSet GetFleetCarsBetween51And100()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 50 AND [Price] <= 100 ORDER BY [Price] ASC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // GetFleetCarsBetween101And200
        public DataSet GetFleetCarsBetween101And200()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 100 AND [Price] <= 200 ORDER BY [Price] ASC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // GetFleetCarsBetween201And500
        public DataSet GetFleetCarsBetween201And500()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 200 AND [Price] <= 500 ORDER BY [Price] ASC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }

        // GetFleetCarsAbove500
        public DataSet GetFleetCarsAbove500()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 500 ORDER BY [Price] ASC";
            DataSet ds = db.Selectcommand(sql);
            return ds;
        }
    }
}
