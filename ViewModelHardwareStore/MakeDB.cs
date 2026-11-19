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
    public class MakeDB
    {
        public MakeDB() { }
        public DBfunction db = new DBfunction();

        // MakeExists
        public bool MakeExists(int MakeID)
        {
            string sql = string.Format("select * from [Brand] where [BrandID]={0}", MakeID);
            DataSet ds = db.Selectcommand(sql);
            return ds.Tables[0].Rows.Count >= 1;
        }

        // AddMake
        public void AddMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description)
        {
            string sql = string.Format(
                "insert into [Brand] ([BrandName], [CountryOfOrigin], [WebsiteURL], [Description]) values ('{0}', '{1}', '{2}', '{3}')",
                MakeName, CountryOfOrigin, WebsiteURL, Description);
            db.DoCommand(sql);
        }

        // DeleteMake
        public void DeleteMake(int MakeID)
        {
            string sql = string.Format("delete from [Brand] where [BrandID]={0}", MakeID);
            db.DoCommand(sql);
        }

        // UpdateMake
        public void UpdateMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description, int MakeID)
        {
            string sql = string.Format(
                "update [Brand] set [BrandName]='{0}', [CountryOfOrigin]='{1}', [WebsiteURL]='{2}', [Description]='{3}' where [BrandID]='{3}'",
                MakeName, CountryOfOrigin, WebsiteURL, Description, MakeID);
            db.DoCommand(sql);
        }

        // GetAllMakes
        public DataSet GetAllMakes()
        {
            string sql = "select * from [Brand]";
            return db.Selectcommand(sql);
        }

        // GetMake
        public DataSet GetMake(int MakeID)
        {
            string sql = string.Format("select * from [Brand] where [BrandID]={0}", MakeID);
            return db.Selectcommand(sql);
        }
    }
}
