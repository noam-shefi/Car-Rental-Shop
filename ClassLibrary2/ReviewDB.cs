using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using BusinessModelCarRental;

namespace ViewModelCarRental
{
    public class ReviewDB
    {
        public ReviewDB() { }
        public DBfunction db = new DBfunction();

        // isExistReview
        public bool isExistReview(int ReviewID)
        {
            string sql = "SELECT * FROM [Review] WHERE [ReviewID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ReviewID);
            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // AddCarReview
        public void AddCarReview(string Username, int CarID, string Rev, int Rating)
        {
            string sql = "INSERT INTO [Review] ([Username], [ProductID], [Rev], [Rating]) VALUES (?, ?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Rev);
            cmd.Parameters.AddWithValue("?", Rating);
            db.DoCommandInjection(cmd);
        }

        // DeleteReview
        public void DeleteReview(int ReviewID)
        {
            string sql = "DELETE FROM [Review] WHERE [ReviewID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ReviewID);
            db.DoCommandInjection(cmd);
        }

        // UpdateCarReview
        public void UpdateCarReview(string Username, int CarID, string Rev, int Rating, int ReviewID)
        {
            string sql = "UPDATE [Review] SET [Username] = ?, [ProductID] = ?, [Rev] = ?, [Rating] = ? WHERE [ReviewID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", Rev);
            cmd.Parameters.AddWithValue("?", Rating);
            cmd.Parameters.AddWithValue("?", ReviewID);
            db.DoCommandInjection(cmd);
        }

        // ShowAllReview
        public List<Review> ShowAllReview()
        {
            string sql = "SELECT * FROM [Review]";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Review> list = new List<Review>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Review
                {
                    MyReviewID = Convert.ToInt32(row["ReviewID"]),
                    MyUsername = row["Username"].ToString(),
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyRev = row["Rev"].ToString(),
                    MyRating = Convert.ToInt32(row["Rating"])
                });
            }
            return list;
        }

        // ShowReview
        public Review ShowReview(int ReviewID)
        {
            string sql = "SELECT * FROM [Review] WHERE [ReviewID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ReviewID);
            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0) return null;

            DataRow row = ds.Tables[0].Rows[0];
            return new Review
            {
                MyReviewID = Convert.ToInt32(row["ReviewID"]),
                MyUsername = row["Username"].ToString(),
                MyCarID = Convert.ToInt32(row["ProductID"]),
                MyRev = row["Rev"].ToString(),
                MyRating = Convert.ToInt32(row["Rating"])
            };
        }

        // ShowAllUserReview
        public List<Review> ShowAllUserReview(string Username)
        {
            string sql = "SELECT * FROM [Review] WHERE [Username] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Review> list = new List<Review>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Review
                {
                    MyReviewID = Convert.ToInt32(row["ReviewID"]),
                    MyUsername = row["Username"].ToString(),
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyRev = row["Rev"].ToString(),
                    MyRating = Convert.ToInt32(row["Rating"])
                });
            }
            return list;
        }

        // ShowAllCarReviews
        public List<Review> ShowAllCarReviews(int CarID)
        {
            string sql = "SELECT * FROM [Review] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Review> list = new List<Review>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Review
                {
                    MyReviewID = Convert.ToInt32(row["ReviewID"]),
                    MyUsername = row["Username"].ToString(),
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyRev = row["Rev"].ToString(),
                    MyRating = Convert.ToInt32(row["Rating"])
                });
            }
            return list;
        }

        public List<Review> GetRentalReviewsForCar(int carID)
        {
            OleDbCommand command = new OleDbCommand("SELECT * FROM [Review] WHERE [ProductID]=? AND [RentalOrderID] IS NOT NULL ORDER BY [ReviewDate] DESC");
            command.Parameters.AddWithValue("?", carID);
            DataSet ds = db.SelectcommandInjection(command);
            List<Review> reviews = new List<Review>();
            foreach (DataRow row in ds.Tables[0].Rows)
                reviews.Add(MapRentalReview(row));
            return reviews;
        }

        public bool CanReviewRental(string username, int orderID)
        {
            OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM [Order] AS o LEFT JOIN [Review] AS r ON o.OrderID=r.RentalOrderID WHERE o.OrderID=? AND o.Username=? AND o.IsRental=true AND o.RentalStatus='approved' AND o.RentalEndDate<Date() AND r.ReviewID IS NULL");
            command.Parameters.AddWithValue("?", orderID);
            command.Parameters.AddWithValue("?", username);
            DataSet ds = db.SelectcommandInjection(command);
            return Convert.ToInt32(ds.Tables[0].Rows[0][0]) == 1;
        }

        public void AddRentalReview(string username, int orderID, int rating, string comment)
        {
            if (rating < 1 || rating > 5 || !CanReviewRental(username, orderID))
                throw new InvalidOperationException("This rental is not eligible for a review.");

            OleDbCommand command = new OleDbCommand("INSERT INTO [Review] ([Username],[ProductID],[Rev],[Rating],[RentalOrderID],[ReviewDate]) SELECT ?, [CarID], ?, ?, ?, ? FROM [Order] WHERE [OrderID]=?");
            command.Parameters.AddWithValue("?", username);
            command.Parameters.AddWithValue("?", comment ?? string.Empty);
            command.Parameters.AddWithValue("?", rating);
            command.Parameters.AddWithValue("?", orderID);
            command.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
            command.Parameters.AddWithValue("?", orderID);
            db.DoCommandInjection(command);
        }

        private Review MapRentalReview(DataRow row)
        {
            return new Review
            {
                MyReviewID = Convert.ToInt32(row["ReviewID"]),
                MyUsername = row["Username"].ToString(),
                MyCarID = Convert.ToInt32(row["ProductID"]),
                MyRev = row["Rev"].ToString(),
                MyRating = Convert.ToInt32(row["Rating"]),
                MyRentalOrderID = row.IsNull("RentalOrderID") ? (int?)null : Convert.ToInt32(row["RentalOrderID"]),
                MyReviewDate = row.IsNull("ReviewDate") ? (DateTime?)null : Convert.ToDateTime(row["ReviewDate"])
            };
        }
    }
}