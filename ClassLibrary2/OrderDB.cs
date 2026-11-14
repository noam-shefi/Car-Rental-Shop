using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using BusinessModelHardwareStore;

namespace ViewModelHardwareStore
{
    public class OrderDB
    {
        private static readonly object bookingLock = new object();

        public OrderDB() { }
        public DBfunction db = new DBfunction();

        // isExistOrder
        public bool isExistOrder(int OrderID)
        {
            string sql = "SELECT * FROM [Order] WHERE [OrderID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", OrderID);

            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // AddOrder
        public void AddOrder(string Username, DateTime OrderDate, decimal TotalPrice, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions)
        {
            string Address = $"{CountryRegion}, {City}, {StreetAddress}, {Floor}, {ZipCode}";

            string sql = "INSERT INTO [Order] ([Username], [OrderDate], [TotalPrice], [Address], [AdditionalInstructions], [Delivered]) " +
                         "VALUES (?, ?, ?, ?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", OrderDate);
            cmd.Parameters.AddWithValue("?", TotalPrice);
            cmd.Parameters.AddWithValue("?", Address);
            cmd.Parameters.AddWithValue("?", AdditionalInstructions);
            cmd.Parameters.AddWithValue("?", false);

            db.DoCommandInjection(cmd);
        }

        // DeleteOrder
        public void DeleteOrder(int OrderID)
        {
            string sql = "DELETE FROM [Order] WHERE [OrderID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", OrderID);

            db.DoCommandInjection(cmd);
        }

        // UpdateOrder
        public void UpdateOrder(string Username, DateTime OrderDate, decimal TotalPrice, string Address, string AdditionalInstructions, bool Delivered, int OrderID)
        {
            string sql = "UPDATE [Order] SET [Username]=?, [OrderDate]=?, [TotalPrice]=?, [Address]=?, [AdditionalInstructions]=?, [Delivered]=? WHERE [OrderID]=?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);
            cmd.Parameters.AddWithValue("?", OrderDate);
            cmd.Parameters.AddWithValue("?", TotalPrice);
            cmd.Parameters.AddWithValue("?", Address);
            cmd.Parameters.AddWithValue("?", AdditionalInstructions);
            cmd.Parameters.AddWithValue("?", Delivered);
            cmd.Parameters.AddWithValue("?", OrderID);

            db.DoCommandInjection(cmd);
        }

        // showAllOrders
        public List<Order> showAllOrders()
        {
            string sql = "SELECT * FROM [Order]";
            OleDbCommand cmd = new OleDbCommand(sql);

            DataSet ds = db.SelectcommandInjection(cmd);
            List<Order> list = new List<Order>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Order
                {
                    MyOrderID = Convert.ToInt32(row["OrderID"]),
                    MyUsername = row["Username"].ToString(),
                    MyOrderDate = Convert.ToDateTime(row["OrderDate"]),
                    MyTotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                    MyAddress = row["Address"].ToString(),
                    MyAdditionalInstructions = row["AdditionalInstructions"].ToString(),
                    MyDelivered = Convert.ToBoolean(row["Delivered"])
                });
            }

            return list;
        }

        // showOrder
        public Order showOrder(int OrderID)
        {
            string sql = "SELECT * FROM [Order] WHERE [OrderID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", OrderID);

            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow row = ds.Tables[0].Rows[0];
            return new Order
            {
                MyOrderID = Convert.ToInt32(row["OrderID"]),
                MyUsername = row["Username"].ToString(),
                MyOrderDate = Convert.ToDateTime(row["OrderDate"]),
                MyTotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                MyAddress = row["Address"].ToString(),
                MyAdditionalInstructions = row["AdditionalInstructions"].ToString(),
                MyDelivered = Convert.ToBoolean(row["Delivered"])
            };
        }

        // UpdateOrderStatus
        public void UpdateOrderStatus(int OrderID, bool Delivered)
        {
            string sql = "UPDATE [Order] SET [Delivered]=? WHERE [OrderID]=?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Delivered);
            cmd.Parameters.AddWithValue("?", OrderID);

            db.DoCommandInjection(cmd);
        }

        // showPendingOrders
        public List<Order> showPendingOrders()
        {
            string sql = "SELECT * FROM [Order] WHERE [Delivered] = false";
            OleDbCommand cmd = new OleDbCommand(sql);

            DataSet ds = db.SelectcommandInjection(cmd);
            List<Order> list = new List<Order>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Order
                {
                    MyOrderID = Convert.ToInt32(row["OrderID"]),
                    MyUsername = row["Username"].ToString(),
                    MyOrderDate = Convert.ToDateTime(row["OrderDate"]),
                    MyTotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                    MyAddress = row["Address"].ToString(),
                    MyAdditionalInstructions = row["AdditionalInstructions"].ToString(),
                    MyDelivered = false
                });
            }
            return list;
        }

        // showOrdersByUsername
        public List<Order> showOrdersByUsername(string Username)
        {
            string sql = "SELECT * FROM [Order] WHERE [Username] = ? ORDER BY [OrderDate] DESC";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", Username);

            DataSet ds = db.SelectcommandInjection(cmd);
            List<Order> list = new List<Order>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Order
                {
                    MyOrderID = Convert.ToInt32(row["OrderID"]),
                    MyUsername = row["Username"].ToString(),
                    MyOrderDate = Convert.ToDateTime(row["OrderDate"]),
                    MyTotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                    MyAddress = row["Address"].ToString(),
                    MyAdditionalInstructions = row["AdditionalInstructions"].ToString(),
                    MyDelivered = Convert.ToBoolean(row["Delivered"])
                });
            }
            return list;
        }

        public bool IsRentalAvailable(int carID, DateTime startDate, DateTime endDate, int requiredStock)
        {
            if (startDate.Date < DateTime.Today || endDate.Date <= startDate.Date || requiredStock <= 0)
                return false;

            OleDbCommand command = new OleDbCommand("SELECT [RentalStartDate],[RentalEndDate] FROM [Order] WHERE [IsRental]=true AND [CarID]=? AND [RentalStartDate]<=? AND [RentalEndDate]>=? AND [RentalStatus] NOT IN ('canceled','rejected')");
            command.Parameters.AddWithValue("?", carID);
            command.Parameters.Add("?", OleDbType.Date).Value = endDate.Date;
            command.Parameters.Add("?", OleDbType.Date).Value = startDate.Date;
            DataSet ds = db.SelectcommandInjection(command);

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                int reserved = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (Convert.ToDateTime(row["RentalStartDate"]) <= date && Convert.ToDateTime(row["RentalEndDate"]) >= date && ++reserved >= requiredStock)
                        return false;
                }
            }
            return true;
        }

        public List<DateTime> GetUnavailableRentalDates(int carID, DateTime firstDate, DateTime lastDate, int stock)
        {
            DateTime first = firstDate.Date;
            DateTime last = lastDate.Date;
            if (last < first)
                throw new ArgumentException("The date range is invalid.");

            List<DateTime> unavailable = new List<DateTime>();
            if (stock <= 0)
            {
                for (DateTime date = first; date <= last; date = date.AddDays(1))
                    unavailable.Add(date);
                return unavailable;
            }

            OleDbCommand command = new OleDbCommand("SELECT [RentalStartDate],[RentalEndDate] FROM [Order] WHERE [IsRental]=true AND [CarID]=? AND [RentalStartDate]<=? AND [RentalEndDate]>=? AND [RentalStatus] NOT IN ('canceled','rejected')");
            command.Parameters.AddWithValue("?", carID);
            command.Parameters.Add("?", OleDbType.Date).Value = last;
            command.Parameters.Add("?", OleDbType.Date).Value = first;
            DataSet ds = db.SelectcommandInjection(command);
            Dictionary<DateTime, int> reservationsByDate = new Dictionary<DateTime, int>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime start = Convert.ToDateTime(row["RentalStartDate"]).Date;
                DateTime end = Convert.ToDateTime(row["RentalEndDate"]).Date;
                if (start < first) start = first;
                if (end > last) end = last;
                for (DateTime date = start; date <= end; date = date.AddDays(1))
                {
                    if (!reservationsByDate.ContainsKey(date)) reservationsByDate[date] = 0;
                    reservationsByDate[date]++;
                }
            }

            for (DateTime date = first; date <= last; date = date.AddDays(1))
            {
                int reserved;
                if (reservationsByDate.TryGetValue(date, out reserved) && reserved >= stock)
                    unavailable.Add(date);
            }
            return unavailable;
        }

        public int CreateRentalOrder(string username, int carID, DateTime startDate, DateTime endDate)
        {
            lock (bookingLock)
            {
                Product car = new ProductDB { db = this.db }.GetCar(carID);
                if (car == null || !IsRentalAvailable(carID, startDate, endDate, car.MyStockQuantity))
                    throw new InvalidOperationException("The selected car is not available for those dates.");

                // The original Order table requires every customer to have a shipping-address row.
                ShippingAddressDB addressDB = new ShippingAddressDB { db = this.db };
                if (!addressDB.IsExistShippingAddress(username))
                    addressDB.AddShippingAddress(username, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                decimal total = car.MyPrice * Convert.ToInt32((endDate.Date - startDate.Date).TotalDays);
                OleDbCommand command = new OleDbCommand("INSERT INTO [Order] ([Username],[OrderDate],[TotalPrice],[Address],[AdditionalInstructions],[Delivered],[IsRental],[CarID],[RentalStartDate],[RentalEndDate],[RentalStatus],[CancellationRequested]) VALUES (?,?,?,'','',false,true,?,?,?,'pending',false)");
                command.Parameters.AddWithValue("?", username);
                command.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                command.Parameters.AddWithValue("?", total);
                command.Parameters.AddWithValue("?", carID);
                command.Parameters.Add("?", OleDbType.Date).Value = startDate.Date;
                command.Parameters.Add("?", OleDbType.Date).Value = endDate.Date;
                return db.InsertAndGetIdentity(command);
            }
        }

        public List<Order> GetRentalOrders(string username)
        {
            string sql = "SELECT * FROM [Order] WHERE [IsRental]=true";
            OleDbCommand command = new OleDbCommand(sql);
            if (!string.IsNullOrEmpty(username))
            {
                sql += " AND [Username]=?";
                command = new OleDbCommand(sql);
                command.Parameters.AddWithValue("?", username);
            }
            sql += " ORDER BY [OrderDate] DESC";
            command.CommandText = sql;
            DataSet ds = db.SelectcommandInjection(command);
            List<Order> orders = new List<Order>();
            foreach (DataRow row in ds.Tables[0].Rows)
                orders.Add(MapRentalOrder(row));
            return orders;
        }

        public Order GetRentalOrder(int orderID)
        {
            OleDbCommand command = new OleDbCommand("SELECT * FROM [Order] WHERE [OrderID]=? AND [IsRental]=true");
            command.Parameters.AddWithValue("?", orderID);
            DataSet ds = db.SelectcommandInjection(command);
            return ds.Tables[0].Rows.Count == 0 ? null : MapRentalOrder(ds.Tables[0].Rows[0]);
        }

        public bool RequestRentalCancellation(int orderID, string username)
        {
            Order order = GetRentalOrder(orderID);
            if (order == null || !string.Equals(order.MyUsername, username, StringComparison.OrdinalIgnoreCase))
                return false;

            if (string.Equals(order.MyRentalStatus, "pending", StringComparison.OrdinalIgnoreCase) && DateTime.Now <= order.MyOrderDate.AddMinutes(10))
                return SetRentalStatus(orderID, "canceled", false, username);

            if (string.Equals(order.MyRentalStatus, "pending", StringComparison.OrdinalIgnoreCase) || string.Equals(order.MyRentalStatus, "approved", StringComparison.OrdinalIgnoreCase))
                return SetRentalStatus(orderID, "cancellation requested", true, username);
            return false;
        }

        public bool SetRentalStatus(int orderID, string status, bool cancellationRequested, string requiredUsername)
        {
            Order order = GetRentalOrder(orderID);
            if (order == null || (!string.IsNullOrEmpty(requiredUsername) && !string.Equals(order.MyUsername, requiredUsername, StringComparison.OrdinalIgnoreCase)))
                return false;

            bool allowed =
                (order.MyRentalStatus == "pending" && (status == "approved" || status == "rejected" || status == "canceled" || status == "cancellation requested")) ||
                (order.MyRentalStatus == "approved" && (status == "canceled" || status == "cancellation requested")) ||
                (order.MyRentalStatus == "cancellation requested" && (status == "approved" || status == "canceled"));
            if (!allowed)
                return false;

            OleDbCommand command = new OleDbCommand("UPDATE [Order] SET [RentalStatus]=?, [CancellationRequested]=? WHERE [OrderID]=? AND [IsRental]=true");
            command.Parameters.AddWithValue("?", status);
            command.Parameters.AddWithValue("?", cancellationRequested);
            command.Parameters.AddWithValue("?", orderID);
            db.DoCommandInjection(command);
            return true;
        }

        private Order MapRentalOrder(DataRow row)
        {
            int carID = Convert.ToInt32(row["CarID"]);
            Product car = new ProductDB { db = this.db }.GetCar(carID);
            return new Order
            {
                MyOrderID = Convert.ToInt32(row["OrderID"]),
                MyUsername = row["Username"].ToString(),
                MyOrderDate = Convert.ToDateTime(row["OrderDate"]),
                MyTotalPrice = Convert.ToDecimal(row["TotalPrice"]),
                MyIsRental = true,
                MyCarID = carID,
                MyCarName = car == null ? "" : (car.MyManufacturer + " " + car.MyProductName),
                MyRentalStartDate = row.IsNull("RentalStartDate") ? (DateTime?)null : Convert.ToDateTime(row["RentalStartDate"]),
                MyRentalEndDate = row.IsNull("RentalEndDate") ? (DateTime?)null : Convert.ToDateTime(row["RentalEndDate"]),
                MyRentalStatus = row["RentalStatus"].ToString(),
                MyCancellationRequested = !row.IsNull("CancellationRequested") && Convert.ToBoolean(row["CancellationRequested"])
            };
        }
    }
}