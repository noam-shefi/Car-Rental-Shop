using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using BusinessModelCarRental;

namespace ViewModelCarRental
{
    public class UserDB
    {
        public UserDB() { }

        public DBfunction db = new DBfunction();

        // isExistUser
        public bool isExistUser(string Username)
        {
            string sql = "SELECT * FROM [User] WHERE [Username] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);

            DataSet ds = db.SelectcommandInjection(cmd);

            return ds.Tables[0].Rows.Count > 0;
        }


        // AddUser
        public void AddUser(string Username, string Password, string PhoneNumber, string Email, bool Admin, DateTime JoinDate)
        {
            string sql = @"INSERT INTO [User] 
                          ([Username], [Password], [PhoneNumber], [Email], [Admin], [JoinDate]) 
                          VALUES (?, ?, ?, ?, ?, ?)";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", Password);
            cmd.Parameters.AddWithValue("?", PhoneNumber);
            cmd.Parameters.AddWithValue("?", Email);
            cmd.Parameters.AddWithValue("?", Admin);
            cmd.Parameters.Add("?", OleDbType.Date).Value = JoinDate;

            db.DoCommandInjection(cmd);
        }


        // DeleteUser
        public void DeleteUser(string Username)
        {
            string sql = "DELETE FROM [User] WHERE [Username] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", Username);

            db.DoCommandInjection(cmd);
        }


        // UpdateUser
        public void UpdateUser(string Password, string PhoneNumber, string Email, bool Admin, DateTime JoinDate, string Username)
        {
            string sql = @"UPDATE [User] 
                          SET [Password] = ?, 
                              [PhoneNumber] = ?, 
                              [Email] = ?, 
                              [Admin] = ?, 
                              [JoinDate] = ? 
                          WHERE [Username] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", Password);
            cmd.Parameters.AddWithValue("?", PhoneNumber);
            cmd.Parameters.AddWithValue("?", Email);
            cmd.Parameters.AddWithValue("?", Admin);
            cmd.Parameters.AddWithValue("?", JoinDate);
            cmd.Parameters.AddWithValue("?", Username);

            db.DoCommandInjection(cmd);
        }


        // showAllUsers
        public List<User> showAllUsers()
        {
            string sql = "SELECT * FROM [User] ";

            OleDbCommand cmd = new OleDbCommand(sql);

            DataSet ds = db.SelectcommandInjection(cmd);

            List<User> users = new List<User>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                users.Add(new User
                {
                    MyUsername = row["Username"].ToString(),
                    MyPassword = row["Password"].ToString(),
                    MyPhoneNumber = row["PhoneNumber"].ToString(),
                    MyEmail = row["Email"].ToString(),
                    MyAdmin = Convert.ToBoolean(row["Admin"]),
                    MyJoinDate = Convert.ToDateTime(row["JoinDate"]),
                    MyFirstName = row["FirstName"].ToString(),
                    MyLastName = row["LastName"].ToString()
                });
            }

            return users;
        }


        // showUser
        public User showUser(string Username)
        {
            string sql = "SELECT * FROM [User] WHERE [Username] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", Username);

            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

                       
            return new User
            {
                MyUsername = row["Username"].ToString(),
                MyPassword = row["Password"].ToString(),
                MyPhoneNumber = row["PhoneNumber"].ToString(),
                MyEmail = row["Email"].ToString(),
                MyAdmin = Convert.ToBoolean(row["Admin"]),
                MyJoinDate = Convert.ToDateTime(row["JoinDate"]),
                MyFirstName = row["FirstName"].ToString(),
                MyLastName = row["LastName"].ToString()
            };
        }


        // IsExistUserAndPass
        public bool IsExistUserAndPass(string Username, string Password)
        {
            string sql = "SELECT * FROM [User] WHERE [Username] = ? AND [Password] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", Password);

            DataSet ds = db.SelectcommandInjection(cmd);

            return ds.Tables[0].Rows.Count > 0;
        }


        // IsUserAdmin
        public bool IsUserAdmin(string username)
        {
            string sql = "SELECT [Admin] FROM [User] WHERE [Username] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", username);

            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return false;

            return Convert.ToBoolean(ds.Tables[0].Rows[0]["Admin"]);
        }


        // ShowUserByEmail
        public User ShowUserByEmail(string Email)
        {
            string sql = "SELECT * FROM [User] WHERE [Email] = ?";

            OleDbCommand cmd = new OleDbCommand(sql);

            cmd.Parameters.AddWithValue("?", Email);

            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];

            return new User
            {
                MyUsername = row["Username"].ToString(),
                MyPassword = row["Password"].ToString(),
                MyPhoneNumber = row["PhoneNumber"].ToString(),
                MyEmail = row["Email"].ToString(),
                MyAdmin = Convert.ToBoolean(row["Admin"]),
                MyJoinDate = Convert.ToDateTime(row["JoinDate"]),
                MyFirstName = row["FirstName"].ToString(),
                MyLastName = row["LastName"].ToString()
            };
        }

        public bool IsEmailRegistered(string email, string exceptUsername)
        {
            OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM [User] WHERE [Email]=? AND [Username]<>?");
            command.Parameters.AddWithValue("?", email);
            command.Parameters.AddWithValue("?", exceptUsername ?? string.Empty);
            DataSet ds = db.SelectcommandInjection(command);
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
        }

        public void RegisterCustomer(string username, string password, string phoneNumber, string email, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Trim().Length < 3 || string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email) || email.IndexOf('@') < 0 || string.IsNullOrEmpty(password) || password.Length < 6)
                throw new ArgumentException("Registration details do not meet the required rules.");
            if (isExistUser(username) || IsEmailRegistered(email, string.Empty))
                throw new InvalidOperationException("The username or email address is already registered.");

            OleDbCommand command = new OleDbCommand("INSERT INTO [User] ([Username],[Password],[PhoneNumber],[Email],[Admin],[JoinDate],[FirstName],[LastName]) VALUES (?,?,?,?,false,?,?,?)");
            command.Parameters.AddWithValue("?", username);
            command.Parameters.AddWithValue("?", password);
            command.Parameters.AddWithValue("?", phoneNumber ?? string.Empty);
            command.Parameters.AddWithValue("?", email);
            command.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
            command.Parameters.AddWithValue("?", firstName);
            command.Parameters.AddWithValue("?", lastName);
            db.DoCommandInjection(command);
        }

        public void UpdateProfile(string username, string firstName, string lastName, string email, string phoneNumber)
        {
            OleDbCommand command = new OleDbCommand("UPDATE [User] SET [FirstName]=?,[LastName]=?,[Email]=?,[PhoneNumber]=? WHERE [Username]=?");
            command.Parameters.AddWithValue("?", firstName);
            command.Parameters.AddWithValue("?", lastName);
            command.Parameters.AddWithValue("?", email);
            command.Parameters.AddWithValue("?", phoneNumber ?? string.Empty);
            command.Parameters.AddWithValue("?", username);
            db.DoCommandInjection(command);
        }

        public bool SetUserAdmin(string targetUsername, bool admin, string administratorUsername)
        {
            if (string.IsNullOrWhiteSpace(administratorUsername) || string.Equals(targetUsername, administratorUsername, StringComparison.OrdinalIgnoreCase) || !IsUserAdmin(administratorUsername))
                return false;
            OleDbCommand command = new OleDbCommand("UPDATE [User] SET [Admin]=? WHERE [Username]=?");
            command.Parameters.AddWithValue("?", admin);
            command.Parameters.AddWithValue("?", targetUsername);
            db.DoCommandInjection(command);
            return true;
        }

        public bool DeleteManagedUser(string targetUsername, string administratorUsername)
        {
            if (string.IsNullOrWhiteSpace(administratorUsername) || string.Equals(targetUsername, administratorUsername, StringComparison.OrdinalIgnoreCase) || !IsUserAdmin(administratorUsername))
                return false;
            DeleteUser(targetUsername);
            return true;
        }
    }
}