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
    public class DBfunction
    {
        private static readonly object schemaLock = new object();
        private static bool schemaInitialized;
        private string connectionstr;

        public DBfunction()
        {
            //C:\Users\Magsihim_AI\Downloads\CarRental\ViewModelCarRental\App_Data\CarRental.accdb

            //this.connectionstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory+"/App_Data/Database4.accdb";
            this.connectionstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ResolveDatabasePath();

            lock (schemaLock)
            {
                if (!schemaInitialized)
                {
                    CarRentalSchema.Ensure(connectionstr);
                    schemaInitialized = true;
                }
            }
        }

        private static string ResolveDatabasePath()
        {
            const string fileName = "HardwareStore.accdb";
            string overridePath = Environment.GetEnvironmentVariable("CARRENTAL_DB_PATH");
            if (!string.IsNullOrEmpty(overridePath) && System.IO.File.Exists(overridePath))
                return overridePath;

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var dir = new System.IO.DirectoryInfo(baseDir);
            while (dir != null)
            {
                string candidate = System.IO.Path.Combine(dir.FullName, "ClassLibrary2", "App_Data", fileName);
                if (System.IO.File.Exists(candidate))
                    return candidate;
                dir = dir.Parent;
            }

            return System.IO.Path.Combine(baseDir, "App_Data", fileName);
        }

        // update, insert , delete
        public void DoCommand(string mysql)
        {
            OleDbConnection connectObj = new OleDbConnection(connectionstr);
            OleDbCommand myCommandObj = new OleDbCommand(mysql, connectObj);
            connectObj.Open();
            myCommandObj.ExecuteNonQuery();
            connectObj.Close();
        }

        public void DoCommandInjection(OleDbCommand cmd)
        {
            OleDbConnection connectObj = new OleDbConnection(connectionstr);

            cmd.Connection = connectObj;

            connectObj.Open();

            cmd.ExecuteNonQuery();

            connectObj.Close();
        }


        // select
        public DataSet Selectcommand(string mysql)
        {
            OleDbConnection connectObj = new OleDbConnection(connectionstr);
            OleDbDataAdapter da = new OleDbDataAdapter(mysql, connectObj);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }


        public DataSet SelectcommandInjection(OleDbCommand cmd)
        {
            OleDbConnection connectObj = new OleDbConnection(connectionstr);

            cmd.Connection = connectObj;

            OleDbDataAdapter da = new OleDbDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        public int InsertAndGetIdentity(OleDbCommand command)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionstr))
            {
                command.Connection = connection;
                connection.Open();
                command.ExecuteNonQuery();
                using (OleDbCommand identityCommand = new OleDbCommand("SELECT @@IDENTITY", connection))
                    return Convert.ToInt32(identityCommand.ExecuteScalar());
            }
        }
    }

    internal static class CarRentalSchema
    {
        public static void Ensure(string connectionString)
        {
            using (var connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                AddColumnIfMissing(connection, "Product", "IsCar", "YESNO");
                AddColumnIfMissing(connection, "Product", "CarYear", "INTEGER");
                AddColumnIfMissing(connection, "Product", "CarCategory", "TEXT(40)");
                AddColumnIfMissing(connection, "Product", "BranchID", "LONG");
                AddColumnIfMissing(connection, "Product", "Seats", "INTEGER");
                AddColumnIfMissing(connection, "Product", "FuelType", "TEXT(40)");
                AddColumnIfMissing(connection, "Product", "RangeKm", "INTEGER");
                AddColumnIfMissing(connection, "Product", "Transmission", "TEXT(40)");
                AddColumnIfMissing(connection, "Product", "LuggageCapacity", "INTEGER");

                AddColumnIfMissing(connection, "Order", "IsRental", "YESNO");
                AddColumnIfMissing(connection, "Order", "CarID", "LONG");
                AddColumnIfMissing(connection, "Order", "RentalStartDate", "DATETIME");
                AddColumnIfMissing(connection, "Order", "RentalEndDate", "DATETIME");
                AddColumnIfMissing(connection, "Order", "RentalStatus", "TEXT(40)");
                AddColumnIfMissing(connection, "Order", "CancellationRequested", "YESNO");

                AddColumnIfMissing(connection, "User", "FirstName", "TEXT(100)");
                AddColumnIfMissing(connection, "User", "LastName", "TEXT(100)");
                AddColumnIfMissing(connection, "Review", "RentalOrderID", "LONG");
                AddColumnIfMissing(connection, "Review", "ReviewDate", "DATETIME");

                CreateTableIfMissing(connection, "Branch",
                    "CREATE TABLE [Branch] (" +
                    "[BranchID] COUNTER CONSTRAINT [PK_Branch] PRIMARY KEY, " +
                    "[BranchName] TEXT(100) NOT NULL, " +
                    "[City] TEXT(100) NOT NULL, " +
                    "[Address] TEXT(255), " +
                    "[Phone] TEXT(50))");

                CreateTableIfMissing(connection, "Favorite",
                    "CREATE TABLE [Favorite] (" +
                    "[Username] TEXT(255) NOT NULL, " +
                    "[ProductID] LONG NOT NULL, " +
                    "[DateAdded] DATETIME, " +
                    "CONSTRAINT [PK_Favorite] PRIMARY KEY ([Username], [ProductID]))");

                CreateIndexIfMissing(connection, "Review", "UX_Review_RentalOrder",
                    "CREATE UNIQUE INDEX [UX_Review_RentalOrder] ON [Review] ([RentalOrderID])");
            }
        }

        private static void AddColumnIfMissing(OleDbConnection connection, string tableName, string columnName, string columnType)
        {
            DataTable columns = connection.GetOleDbSchemaTable(
                OleDbSchemaGuid.Columns,
                new object[] { null, null, tableName, null });

            foreach (DataRow row in columns.Rows)
            {
                if (string.Equals(Convert.ToString(row["COLUMN_NAME"]), columnName, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            Execute(connection, "ALTER TABLE [" + tableName + "] ADD COLUMN [" + columnName + "] " + columnType);
        }

        private static void CreateTableIfMissing(OleDbConnection connection, string tableName, string commandText)
        {
            DataTable tables = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            foreach (DataRow row in tables.Rows)
            {
                if (string.Equals(Convert.ToString(row["TABLE_NAME"]), tableName, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            Execute(connection, commandText);
        }

        private static void CreateIndexIfMissing(OleDbConnection connection, string tableName, string indexName, string commandText)
        {
            DataTable indexes = connection.GetOleDbSchemaTable(
                OleDbSchemaGuid.Indexes,
                new object[] { null, null, null, null, tableName });

            foreach (DataRow row in indexes.Rows)
            {
                if (string.Equals(Convert.ToString(row["INDEX_NAME"]), indexName, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            Execute(connection, commandText);
        }

        private static void Execute(OleDbConnection connection, string commandText)
        {
            using (var command = new OleDbCommand(commandText, connection))
                command.ExecuteNonQuery();
        }
    }
}
