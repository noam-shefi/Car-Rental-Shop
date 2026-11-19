using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using BusinessModelCarRental;

namespace ViewModelCarRental
{
    public class MakeDB
    {
        public MakeDB() { }

        public DBfunction db = new DBfunction();


        // MakeExists
        public bool MakeExists(int MakeID)
        {
            string sql = "SELECT * FROM [Brand] WHERE [BrandID] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", MakeID);

            DataSet ds = db.SelectcommandInjection(cmd);

            return ds.Tables[0].Rows.Count > 0;
        }


        // AddMake
        public void AddMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description)
        {
            string sql = @"INSERT INTO [Brand] 
                          ([BrandName], [CountryOfOrigin], [WebsiteURL], [Description]) 
                          VALUES (?, ?, ?, ?)";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", MakeName);
            cmd.Parameters.AddWithValue("?", CountryOfOrigin);
            cmd.Parameters.AddWithValue("?", WebsiteURL);
            cmd.Parameters.AddWithValue("?", Description);

            db.DoCommandInjection(cmd);
        }


        // DeleteMake
        public void DeleteMake(int MakeID)
        {
            string sql = "DELETE FROM [Brand] WHERE [BrandID] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", MakeID);

            db.DoCommandInjection(cmd);
        }


        // UpdateMake
        public void UpdateMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description, int MakeID)
        {
            string sql = @"UPDATE [Brand] 
                          SET [BrandName] = ?, 
                              [CountryOfOrigin] = ?, 
                              [WebsiteURL] = ?, 
                              [Description] = ? 
                          WHERE [BrandID] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", MakeName);
            cmd.Parameters.AddWithValue("?", CountryOfOrigin);
            cmd.Parameters.AddWithValue("?", WebsiteURL);
            cmd.Parameters.AddWithValue("?", Description);
            cmd.Parameters.AddWithValue("?", MakeID);

            db.DoCommandInjection(cmd);
        }


        // GetAllMakes
        public List<Make> GetAllMakes()
        {
            string sql = "SELECT * FROM [Brand]";

            OleDbCommand cmd = new OleDbCommand(sql);

            DataSet ds = db.SelectcommandInjection(cmd);

            List<Make> makes = new List<Make>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                makes.Add(new Make
                {
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyMakeName = row["BrandName"].ToString(),
                    MyCountryOfOrigin = row["CountryOfOrigin"].ToString(),
                    MyWebsiteURL = row["WebsiteURL"].ToString(),
                    MyDescription = row["Description"].ToString()
                });
            }

            return makes;
        }


        // GetMake
        public Make GetMake(int MakeID)
        {
            string sql = "SELECT * FROM [Brand] WHERE [BrandID] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", MakeID);

            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new Make
            {
                MyMakeID = Convert.ToInt32(row["BrandID"]),
                MyMakeName = row["BrandName"].ToString(),
                MyCountryOfOrigin = row["CountryOfOrigin"].ToString(),
                MyWebsiteURL = row["WebsiteURL"].ToString(),
                MyDescription = row["Description"].ToString()
            };
        }
    }
}