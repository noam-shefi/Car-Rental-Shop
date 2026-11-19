using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using BusinessModelCarRental;

namespace ViewModelCarRental
{
    public class VehicleCategoryDB
    {
        public VehicleCategoryDB() { }
        public DBfunction db = new DBfunction();

        // VehicleCategoryExists
        public bool VehicleCategoryExists(int VehicleCategoryID)
        {
            string sql = "SELECT * FROM [Category] WHERE [CategoryID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);

            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // AddVehicleCategory
        public void AddVehicleCategory(string VehicleCategoryName, string Description)
        {
            string sql = "INSERT INTO [Category] ([CategoryName], [Description]) VALUES (?, ?)";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryName);
            cmd.Parameters.AddWithValue("?", Description);

            db.DoCommandInjection(cmd);
        }

        // DeleteVehicleCategory
        public void DeleteVehicleCategory(int VehicleCategoryID)
        {
            string sql = "DELETE FROM [Category] WHERE [CategoryID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);

            db.DoCommandInjection(cmd);
        }

        // UpdateVehicleCategory
        public void UpdateVehicleCategory(string VehicleCategoryName, string Description, int VehicleCategoryID)
        {
            string sql = "UPDATE [Category] SET [CategoryName] = ?, [Description] = ? WHERE [CategoryID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryName);
            cmd.Parameters.AddWithValue("?", Description);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);

            db.DoCommandInjection(cmd);
        }

        // GetAllVehicleCategories
        public List<VehicleCategory> GetAllVehicleCategories()
        {
            string sql = "SELECT * FROM [Category]";
            OleDbCommand cmd = new OleDbCommand(sql);

            DataSet ds = db.SelectcommandInjection(cmd);

            List<VehicleCategory> list = new List<VehicleCategory>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new VehicleCategory
                {
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyVehicleCategoryName = row["CategoryName"].ToString(),
                    MyDescription = row["Description"].ToString()
                });
            }
            return list;
        }

        // GetVehicleCategory
        public VehicleCategory GetVehicleCategory(int VehicleCategoryID)
        {
            string sql = "SELECT * FROM [Category] WHERE [CategoryID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);

            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];
            return new VehicleCategory
            {
                MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                MyVehicleCategoryName = row["CategoryName"].ToString(),
                MyDescription = row["Description"].ToString()
            };
        }

        // AddCarFromVehicleCategory
        public void AddCarFromVehicleCategory(int VehicleCategoryID)
        {
            string sql = "INSERT INTO [Product] " +
                         "([ProductID], [ProductName], [CategoryID], [BrandID], [Price], [StockQuantity], [Description], [Picture]) " +
                         "VALUES (?, 'Fake', ?, 2, 0, 0, 'None', 'None')";

            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", -VehicleCategoryID);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);

            db.DoCommandInjection(cmd);
        }
    }
}