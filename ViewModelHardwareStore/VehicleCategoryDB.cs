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
    public class VehicleCategoryDB
    {
        public VehicleCategoryDB() { }
        public DBfunction db = new DBfunction();

        // VehicleCategoryExists
        public bool VehicleCategoryExists(int VehicleCategoryID)
        {
            string sql = string.Format("select * from [Category] where [CategoryID]={0}", VehicleCategoryID);
            DataSet ds = db.Selectcommand(sql);
            return ds.Tables[0].Rows.Count >= 1;
        }

        // AddVehicleCategory
        public void AddVehicleCategory(string VehicleCategoryName, string Description)
        {
            string sql = string.Format(
                "insert into [Category] ([CategoryName], [Description]) values ('{0}','{1}')",
                VehicleCategoryName, Description);
            db.DoCommand(sql);
        }

        // DeleteVehicleCategory
        public void DeleteVehicleCategory(int VehicleCategoryID)
        {
            string sql = string.Format("delete from [Category] where [CategoryID]={0}", VehicleCategoryID);
            db.DoCommand(sql);
        }

        // UpdateVehicleCategory
        public void UpdateVehicleCategory(string VehicleCategoryName, string Description, int VehicleCategoryID)
        {
            string sql = string.Format(
                "update [Category] set [CategoryName]='{0}', [Description]='{1}' where [CategoryID]={2}",
                VehicleCategoryName, Description, VehicleCategoryID);
            db.DoCommand(sql);
        }

        // GetAllVehicleCategories
        public DataSet GetAllVehicleCategories()
        {
            string sql = "select * from [Category]";
            return db.Selectcommand(sql);
        }

        // GetVehicleCategory
        public DataSet GetVehicleCategory(int VehicleCategoryID)
        {
            string sql = string.Format("select * from [Category] where [CategoryID]={0}", VehicleCategoryID);
            return db.Selectcommand(sql);
        }
    }
}
