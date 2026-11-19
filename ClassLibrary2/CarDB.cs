using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using BusinessModelCarRental;

namespace ViewModelCarRental
{
    public class CarDB
    {
        public CarDB() { }
        public DBfunction db = new DBfunction();

        // FleetCarExists
        public bool FleetCarExists(int CarID)
        {
            string sql = "SELECT * FROM [Product] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);

            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // GetNextCarID
        public int GetNextCarID(int VehicleCategoryID)
        {
            int baseNumber = VehicleCategoryID * 10000;
            int nextID = baseNumber + 1;

            string sql = "SELECT MAX(ProductID) FROM [Product] WHERE [CategoryID] = ? AND ProductID >= ? AND ProductID < ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);
            cmd.Parameters.AddWithValue("?", baseNumber);
            cmd.Parameters.AddWithValue("?", baseNumber + 10000);

            DataSet ds = db.SelectcommandInjection(cmd);
            if (ds.Tables[0].Rows[0][0] != DBNull.Value)
            {
                int maxID = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                if (maxID >= baseNumber)
                    nextID = maxID + 1;
            }

            return nextID;
        }

        // CreateFleetCar
        public void CreateFleetCar(int CarID, string ModelName, int VehicleCategoryID, int MakeID, decimal Price, int FleetQuantity, string Description, string Picture)
        {
            string sql = "INSERT INTO [Product] ([ProductID], [ProductName], [CategoryID], [BrandID], [Price], [StockQuantity], [Description], [Picture]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);
            cmd.Parameters.AddWithValue("?", ModelName);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);
            cmd.Parameters.AddWithValue("?", MakeID);
            cmd.Parameters.AddWithValue("?", Price);
            cmd.Parameters.AddWithValue("?", FleetQuantity);
            cmd.Parameters.AddWithValue("?", Description);
            cmd.Parameters.AddWithValue("?", Picture);

            db.DoCommandInjection(cmd);
        }

        // DeleteFleetCar
        public void DeleteFleetCar(int CarID)
        {
            string sql = "DELETE FROM [Product] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);

            db.DoCommandInjection(cmd);
        }

        // UpdateFleetCar
        public void UpdateFleetCar(string ModelName, int VehicleCategoryID, int MakeID, decimal Price, int FleetQuantity, string Description, string Picture, int CarID)
        {
            string sql = "UPDATE [Product] SET [ProductName] = ?, [CategoryID] = ?, [BrandID] = ?, [Price] = ?, [StockQuantity] = ?, [Description] = ?, [Picture] = ? WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ModelName);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);
            cmd.Parameters.AddWithValue("?", MakeID);
            cmd.Parameters.AddWithValue("?", Price);
            cmd.Parameters.AddWithValue("?", FleetQuantity);
            cmd.Parameters.AddWithValue("?", Description);
            cmd.Parameters.AddWithValue("?", Picture);
            cmd.Parameters.AddWithValue("?", CarID);

            db.DoCommandInjection(cmd);
        }

        // GetAllFleetCars
        public List<Car> GetAllFleetCars()
        {
            string sql = "SELECT * FROM [Product]";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }

            return list;
        }

        // GetFleetCarByID
        public Car GetFleetCarByID(int CarID)
        {
            string sql = "SELECT * FROM [Product] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CarID);
            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0) return null;

            DataRow row = ds.Tables[0].Rows[0];
            return new Car
            {
                MyCarID = Convert.ToInt32(row["ProductID"]),
                MyModelName = row["ProductName"].ToString(),
                MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                MyMakeID = Convert.ToInt32(row["BrandID"]),
                MyPrice = Convert.ToDecimal(row["Price"]),
                MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                MyDescription = row["Description"].ToString(),
                MyPicture = row["Picture"].ToString()
            };
        }

        // GetFleetCarsWithCategoryAndMake
        public List<Car> GetFleetCarsWithCategoryAndMake()
        {
            string sql =
                "SELECT Product.ProductID, Product.ProductName, Category.CategoryName, Brand.BrandName, " +
                "Product.Price, Product.Description, Product.StockQuantity, Product.Picture " +
                "FROM (Product INNER JOIN Category ON Product.CategoryID = Category.CategoryID) " +
                "INNER JOIN Brand ON Product.BrandID = Brand.BrandID";

            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }

            return list;
        }

        // GetFleetCarsByCategoryID
        public List<Car> GetFleetCarsByCategoryID(int VehicleCategoryID)
        {
            string sql = "SELECT * FROM [Product] WHERE [CategoryID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", VehicleCategoryID);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsByMakeID
        public List<Car> GetFleetCarsByMakeID(int MakeID)
        {
            string sql = "SELECT * FROM [Product] WHERE [BrandID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", MakeID);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsByRateDescending
        public List<Car> GetFleetCarsByRateDescending()
        {
            string sql = "SELECT * FROM [Product] ORDER BY [Price] DESC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsByRateAscending
        public List<Car> GetFleetCarsByRateAscending()
        {
            string sql = "SELECT * FROM [Product] ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // SearchFleetCars
        public List<Car> SearchFleetCars(string SearchTerm)
        {
            string sql = "SELECT * FROM [Product] WHERE [ProductName] LIKE ? OR [Description] LIKE ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", "%" + SearchTerm + "%");
            cmd.Parameters.AddWithValue("?", "%" + SearchTerm + "%");
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // FilterFleetCarsByRateRange
        public List<Car> FilterFleetCarsByRateRange(decimal MinPrice, decimal MaxPrice)
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] BETWEEN ? AND ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", MinPrice);
            cmd.Parameters.AddWithValue("?", MaxPrice);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsUnder50
        public List<Car> GetFleetCarsUnder50()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] <= 50 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsBetween51And100
        public List<Car> GetFleetCarsBetween51And100()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 50 AND [Price] <= 100 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsBetween101And200
        public List<Car> GetFleetCarsBetween101And200()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 100 AND [Price] <= 200 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsBetween201And500
        public List<Car> GetFleetCarsBetween201And500()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 200 AND [Price] <= 500 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetFleetCarsAbove500
        public List<Car> GetFleetCarsAbove500()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 500 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Car> list = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Car
                {
                    MyCarID = Convert.ToInt32(row["ProductID"]),
                    MyModelName = row["ProductName"].ToString(),
                    MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyMakeID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        private const string CarSelectSql = "SELECT p.*, b.BrandName, br.BranchName, br.City AS BranchCity, r.AverageRating, r.ReviewCount " +
            "FROM (([Product] AS p LEFT JOIN [Brand] AS b ON p.BrandID = b.BrandID) " +
            "LEFT JOIN [Branch] AS br ON p.BranchID = br.BranchID) " +
            "LEFT JOIN (SELECT [ProductID], AVG([Rating]) AS AverageRating, COUNT(*) AS ReviewCount " +
            "FROM [Review] WHERE [RentalOrderID] IS NOT NULL GROUP BY [ProductID]) AS r ON p.ProductID = r.ProductID " +
            "WHERE p.IsCar = true AND p.Picture IS NOT NULL AND Trim(p.Picture) <> ''";

        public List<Car> GetCars()
        {
            DataSet ds = db.Selectcommand(CarSelectSql + " ORDER BY p.ProductName");
            List<Car> cars = new List<Car>();
            foreach (DataRow row in ds.Tables[0].Rows)
                cars.Add(MapCar(row));
            return cars;
        }

        public Car GetCar(int carID)
        {
            OleDbCommand command = new OleDbCommand(CarSelectSql + " AND p.ProductID = ?");
            command.Parameters.AddWithValue("?", carID);
            DataSet ds = db.SelectcommandInjection(command);
            return ds.Tables[0].Rows.Count == 0 ? null : MapCar(ds.Tables[0].Rows[0]);
        }

        public List<Car> SearchCars(string search, string vehicleCategory, decimal? maximumPrice, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Car> query = GetCars();
            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();
                query = query.Where(car => (car.MyManufacturer ?? string.Empty).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 || (car.MyModelName ?? string.Empty).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(vehicleCategory))
                query = query.Where(car => string.Equals(car.MyCarCategory, vehicleCategory, StringComparison.OrdinalIgnoreCase));
            if (maximumPrice.HasValue)
                query = query.Where(car => car.MyPrice <= maximumPrice.Value);

            List<Car> cars = query.ToList();
            if (cars.Count > 0 && startDate.HasValue && endDate.HasValue && endDate.Value.Date > startDate.Value.Date)
            {
                Dictionary<int, int> reservations = CountOverlappingReservations(startDate.Value.Date, endDate.Value.Date);
                cars = cars.Where(car => (!reservations.ContainsKey(car.MyCarID) ? 0 : reservations[car.MyCarID]) < car.MyFleetQuantity).ToList();
            }
            return cars;
        }

        private Dictionary<int, int> CountOverlappingReservations(DateTime startDate, DateTime endDate)
        {
            OleDbCommand command = new OleDbCommand("SELECT [CarID], COUNT(*) AS ReservationCount FROM [Order] WHERE [IsRental]=true AND [RentalStartDate]<=? AND [RentalEndDate]>=? AND [RentalStatus] NOT IN ('canceled','rejected') GROUP BY [CarID]");
            command.Parameters.Add("?", OleDbType.Date).Value = endDate;
            command.Parameters.Add("?", OleDbType.Date).Value = startDate;
            DataSet ds = db.SelectcommandInjection(command);
            Dictionary<int, int> counts = new Dictionary<int, int>();
            foreach (DataRow row in ds.Tables[0].Rows)
                if (!row.IsNull("CarID"))
                    counts[Convert.ToInt32(row["CarID"])] = Convert.ToInt32(row["ReservationCount"]);
            return counts;
        }

        public int AddCar(Car car)
        {
            int carID = GetNextCarID(car.MyVehicleCategoryID);
            const string sql = "INSERT INTO [Product] ([ProductID], [ProductName], [CategoryID], [BrandID], [Price], [StockQuantity], [Description], [Picture], [IsCar], [CarYear], [CarCategory], [BranchID], [Seats], [FuelType], [RangeKm], [Transmission], [LuggageCapacity]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, true, ?, ?, ?, ?, ?, ?, ?, ?)";
            OleDbCommand command = new OleDbCommand(sql);
            command.Parameters.AddWithValue("?", carID);
            command.Parameters.AddWithValue("?", car.MyModelName);
            command.Parameters.AddWithValue("?", car.MyVehicleCategoryID);
            command.Parameters.AddWithValue("?", car.MyMakeID);
            command.Parameters.AddWithValue("?", car.MyPrice);
            command.Parameters.AddWithValue("?", car.MyFleetQuantity);
            command.Parameters.AddWithValue("?", car.MyDescription ?? string.Empty);
            command.Parameters.AddWithValue("?", car.MyPicture ?? string.Empty);
            command.Parameters.AddWithValue("?", car.MyYear);
            command.Parameters.AddWithValue("?", car.MyCarCategory);
            command.Parameters.AddWithValue("?", car.MyBranchID.HasValue ? (object)car.MyBranchID.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", car.MySeats.HasValue ? (object)car.MySeats.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", (object)car.MyFuelType ?? DBNull.Value);
            command.Parameters.AddWithValue("?", car.MyRangeKm.HasValue ? (object)car.MyRangeKm.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", (object)car.MyTransmission ?? DBNull.Value);
            command.Parameters.AddWithValue("?", car.MyLuggageCapacity.HasValue ? (object)car.MyLuggageCapacity.Value : DBNull.Value);
            db.DoCommandInjection(command);
            return carID;
        }

        public void UpdateCar(Car car)
        {
            const string sql = "UPDATE [Product] SET [ProductName]=?, [CategoryID]=?, [BrandID]=?, [Price]=?, [StockQuantity]=?, [Description]=?, [Picture]=?, [CarYear]=?, [CarCategory]=?, [BranchID]=?, [Seats]=?, [FuelType]=?, [RangeKm]=?, [Transmission]=?, [LuggageCapacity]=? WHERE [ProductID]=? AND [IsCar]=true";
            OleDbCommand command = new OleDbCommand(sql);
            command.Parameters.AddWithValue("?", car.MyModelName);
            command.Parameters.AddWithValue("?", car.MyVehicleCategoryID);
            command.Parameters.AddWithValue("?", car.MyMakeID);
            command.Parameters.AddWithValue("?", car.MyPrice);
            command.Parameters.AddWithValue("?", car.MyFleetQuantity);
            command.Parameters.AddWithValue("?", car.MyDescription ?? string.Empty);
            command.Parameters.AddWithValue("?", car.MyPicture ?? string.Empty);
            command.Parameters.AddWithValue("?", car.MyYear);
            command.Parameters.AddWithValue("?", car.MyCarCategory);
            command.Parameters.AddWithValue("?", car.MyBranchID.HasValue ? (object)car.MyBranchID.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", car.MySeats.HasValue ? (object)car.MySeats.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", (object)car.MyFuelType ?? DBNull.Value);
            command.Parameters.AddWithValue("?", car.MyRangeKm.HasValue ? (object)car.MyRangeKm.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", (object)car.MyTransmission ?? DBNull.Value);
            command.Parameters.AddWithValue("?", car.MyLuggageCapacity.HasValue ? (object)car.MyLuggageCapacity.Value : DBNull.Value);
            command.Parameters.AddWithValue("?", car.MyCarID);
            db.DoCommandInjection(command);
        }

        public void SetCarFleetQuantity(int carID, int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException("quantity");
            OleDbCommand command = new OleDbCommand("UPDATE [Product] SET [StockQuantity]=? WHERE [ProductID]=? AND [IsCar]=true");
            command.Parameters.AddWithValue("?", quantity);
            command.Parameters.AddWithValue("?", carID);
            db.DoCommandInjection(command);
        }

        public void DeleteCar(int carID)
        {
            OleDbCommand favoriteCleanup = new OleDbCommand("DELETE FROM [Favorite] WHERE [ProductID]=?");
            favoriteCleanup.Parameters.AddWithValue("?", carID);
            db.DoCommandInjection(favoriteCleanup);
            OleDbCommand command = new OleDbCommand("DELETE FROM [Product] WHERE [ProductID]=? AND [IsCar]=true");
            command.Parameters.AddWithValue("?", carID);
            db.DoCommandInjection(command);
        }

        public List<Branch> GetBranches()
        {
            DataSet ds = db.Selectcommand("SELECT [BranchID], [BranchName], [City], [Address], [Phone] FROM [Branch] ORDER BY [City], [BranchName]");
            List<Branch> branches = new List<Branch>();
            foreach (DataRow row in ds.Tables[0].Rows)
                branches.Add(new Branch { MyBranchID = Convert.ToInt32(row["BranchID"]), MyBranchName = row["BranchName"].ToString(), MyCity = row["City"].ToString(), MyAddress = row["Address"].ToString(), MyPhone = row["Phone"].ToString() });
            return branches;
        }

        public void AddBranch(Branch branch)
        {
            OleDbCommand command = new OleDbCommand("INSERT INTO [Branch] ([BranchName], [City], [Address], [Phone]) VALUES (?, ?, ?, ?)");
            command.Parameters.AddWithValue("?", branch.MyBranchName);
            command.Parameters.AddWithValue("?", branch.MyCity);
            command.Parameters.AddWithValue("?", branch.MyAddress ?? string.Empty);
            command.Parameters.AddWithValue("?", branch.MyPhone ?? string.Empty);
            db.DoCommandInjection(command);
        }

        public void DeleteBranch(int branchID)
        {
            OleDbCommand clearAssignments = new OleDbCommand("UPDATE [Product] SET [BranchID]=NULL WHERE [BranchID]=?");
            clearAssignments.Parameters.AddWithValue("?", branchID);
            db.DoCommandInjection(clearAssignments);
            OleDbCommand command = new OleDbCommand("DELETE FROM [Branch] WHERE [BranchID]=?");
            command.Parameters.AddWithValue("?", branchID);
            db.DoCommandInjection(command);
        }

        public List<Car> GetFavorites(string username)
        {
            OleDbCommand command = new OleDbCommand("SELECT [ProductID], [DateAdded] FROM [Favorite] WHERE [Username]=? ORDER BY [DateAdded] DESC");
            command.Parameters.AddWithValue("?", username);
            DataSet ds = db.SelectcommandInjection(command);
            List<Car> result = new List<Car>();
            if (ds.Tables[0].Rows.Count == 0)
                return result;
            Dictionary<int, Car> cars = GetCars().ToDictionary(car => car.MyCarID);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Car car;
                if (cars.TryGetValue(Convert.ToInt32(row["ProductID"]), out car))
                {
                    car.MyFavoriteDateAdded = row.IsNull("DateAdded") ? (DateTime?)null : Convert.ToDateTime(row["DateAdded"]);
                    result.Add(car);
                }
            }
            return result;
        }

        public void SetFavorite(string username, int carID, bool isFavorite)
        {
            if (isFavorite)
            {
                OleDbCommand command = new OleDbCommand("INSERT INTO [Favorite] ([Username], [ProductID], [DateAdded]) VALUES (?, ?, ?)");
                command.Parameters.AddWithValue("?", username);
                command.Parameters.AddWithValue("?", carID);
                command.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                db.DoCommandInjection(command);
            }
            else
            {
                OleDbCommand command = new OleDbCommand("DELETE FROM [Favorite] WHERE [Username]=? AND [ProductID]=?");
                command.Parameters.AddWithValue("?", username);
                command.Parameters.AddWithValue("?", carID);
                db.DoCommandInjection(command);
            }
        }

        private Car MapCar(DataRow row)
        {
            var car = new Car
            {
                MyCarID = Convert.ToInt32(row["ProductID"]),
                MyModelName = row["ProductName"].ToString(),
                MyVehicleCategoryID = Convert.ToInt32(row["CategoryID"]),
                MyMakeID = Convert.ToInt32(row["BrandID"]),
                MyPrice = Convert.ToDecimal(row["Price"]),
                MyFleetQuantity = Convert.ToInt32(row["StockQuantity"]),
                MyDescription = row["Description"].ToString(),
                MyPicture = row["Picture"].ToString(),
                MyIsCar = true,
                MyYear = row.IsNull("CarYear") ? 0 : Convert.ToInt32(row["CarYear"]),
                MyCarCategory = row["CarCategory"].ToString(),
                MyBranchID = row.IsNull("BranchID") ? (int?)null : Convert.ToInt32(row["BranchID"]),
                MyBranchName = row["BranchName"].ToString(),
                MyBranchCity = row["BranchCity"].ToString(),
                MySeats = row.IsNull("Seats") ? (int?)null : Convert.ToInt32(row["Seats"]),
                MyFuelType = row["FuelType"].ToString(),
                MyRangeKm = row.IsNull("RangeKm") ? (int?)null : Convert.ToInt32(row["RangeKm"]),
                MyTransmission = row["Transmission"].ToString(),
                MyLuggageCapacity = row.IsNull("LuggageCapacity") ? (int?)null : Convert.ToInt32(row["LuggageCapacity"]),
                MyManufacturer = row["BrandName"].ToString()
            };
            if (!row.IsNull("AverageRating"))
                car.MyAverageRating = Convert.ToDouble(row["AverageRating"]);
            if (!row.IsNull("ReviewCount"))
                car.MyReviewCount = Convert.ToInt32(row["ReviewCount"]);
            return car;
        }
    }
}