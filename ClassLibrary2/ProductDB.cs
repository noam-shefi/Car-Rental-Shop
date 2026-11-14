using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using BusinessModelHardwareStore;

namespace ViewModelHardwareStore
{
    public class ProductDB
    {
        public ProductDB() { }
        public DBfunction db = new DBfunction();

        // isExistProduct
        public bool isExistProduct(int ProductID)
        {
            string sql = "SELECT * FROM [Product] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ProductID);

            DataSet ds = db.SelectcommandInjection(cmd);
            return ds.Tables[0].Rows.Count > 0;
        }

        // GetNextProductID
        public int GetNextProductID(int CategoryID)
        {
            int baseNumber = CategoryID * 10000;
            int nextID = baseNumber + 1;

            string sql = "SELECT MAX(ProductID) FROM [Product] WHERE [CategoryID] = ? AND ProductID >= ? AND ProductID < ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CategoryID);
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

        // AddProduct
        public void AddProduct(int ProductID, string ProductName, int CategoryID, int BrandID, decimal Price, int StockQuantity, string Description, string Picture)
        {
            string sql = "INSERT INTO [Product] ([ProductID], [ProductName], [CategoryID], [BrandID], [Price], [StockQuantity], [Description], [Picture]) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ProductID);
            cmd.Parameters.AddWithValue("?", ProductName);
            cmd.Parameters.AddWithValue("?", CategoryID);
            cmd.Parameters.AddWithValue("?", BrandID);
            cmd.Parameters.AddWithValue("?", Price);
            cmd.Parameters.AddWithValue("?", StockQuantity);
            cmd.Parameters.AddWithValue("?", Description);
            cmd.Parameters.AddWithValue("?", Picture);

            db.DoCommandInjection(cmd);
        }

        // DeleteProduct
        public void DeleteProduct(int ProductID)
        {
            string sql = "DELETE FROM [Product] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ProductID);

            db.DoCommandInjection(cmd);
        }

        // UpdateProduct
        public void UpdateProduct(string ProductName, int CategoryID, int BrandID, decimal Price, int StockQuantity, string Description, string Picture, int ProductID)
        {
            string sql = "UPDATE [Product] SET [ProductName] = ?, [CategoryID] = ?, [BrandID] = ?, [Price] = ?, [StockQuantity] = ?, [Description] = ?, [Picture] = ? WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ProductName);
            cmd.Parameters.AddWithValue("?", CategoryID);
            cmd.Parameters.AddWithValue("?", BrandID);
            cmd.Parameters.AddWithValue("?", Price);
            cmd.Parameters.AddWithValue("?", StockQuantity);
            cmd.Parameters.AddWithValue("?", Description);
            cmd.Parameters.AddWithValue("?", Picture);
            cmd.Parameters.AddWithValue("?", ProductID);

            db.DoCommandInjection(cmd);
        }

        // showAllProduct
        public List<Product> showAllProduct()
        {
            string sql = "SELECT * FROM [Product]";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }

            return list;
        }

        // showProduct
        public Product showProduct(int ProductID)
        {
            string sql = "SELECT * FROM [Product] WHERE [ProductID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", ProductID);
            DataSet ds = db.SelectcommandInjection(cmd);

            if (ds.Tables[0].Rows.Count == 0) return null;

            DataRow row = ds.Tables[0].Rows[0];
            return new Product
            {
                MyProductID = Convert.ToInt32(row["ProductID"]),
                MyProductName = row["ProductName"].ToString(),
                MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                MyBrandID = Convert.ToInt32(row["BrandID"]),
                MyPrice = Convert.ToDecimal(row["Price"]),
                MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                MyDescription = row["Description"].ToString(),
                MyPicture = row["Picture"].ToString()
            };
        }

        // showAllProductsInnerJoin
        public List<Product> showAllProductsInnerJoin()
        {
            string sql =
                "SELECT Product.ProductID, Product.ProductName, Category.CategoryName, Brand.BrandName, " +
                "Product.Price, Product.Description, Product.StockQuantity, Product.Picture " +
                "FROM (Product INNER JOIN Category ON Product.CategoryID = Category.CategoryID) " +
                "INNER JOIN Brand ON Product.BrandID = Brand.BrandID";

            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }

            return list;
        }

        // showAllProductByCategoryID
        public List<Product> showAllProductByCategoryID(int CategoryID)
        {
            string sql = "SELECT * FROM [Product] WHERE [CategoryID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", CategoryID);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // showAllProductByBrandID
        public List<Product> showAllProductByBrandID(int BrandID)
        {
            string sql = "SELECT * FROM [Product] WHERE [BrandID] = ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", BrandID);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // showAllProductsPriceDesc
        public List<Product> showAllProductsPriceDesc()
        {
            string sql = "SELECT * FROM [Product] ORDER BY [Price] DESC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // showAllProductsPriceAsc
        public List<Product> showAllProductsPriceAsc()
        {
            string sql = "SELECT * FROM [Product] ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // SearchProducts
        public List<Product> SearchProducts(string SearchTerm)
        {
            string sql = "SELECT * FROM [Product] WHERE [ProductName] LIKE ? OR [Description] LIKE ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", "%" + SearchTerm + "%");
            cmd.Parameters.AddWithValue("?", "%" + SearchTerm + "%");
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // FilterProductsByPriceRange
        public List<Product> FilterProductsByPriceRange(decimal MinPrice, decimal MaxPrice)
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] BETWEEN ? AND ?";
            OleDbCommand cmd = new OleDbCommand(sql);
            cmd.Parameters.AddWithValue("?", MinPrice);
            cmd.Parameters.AddWithValue("?", MaxPrice);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetProductsUnder50
        public List<Product> GetProductsUnder50()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] <= 50 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetProductsBetween51And100
        public List<Product> GetProductsBetween51And100()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 50 AND [Price] <= 100 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetProductsBetween101And200
        public List<Product> GetProductsBetween101And200()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 100 AND [Price] <= 200 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetProductsBetween201And500
        public List<Product> GetProductsBetween201And500()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 200 AND [Price] <= 500 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
                    MyDescription = row["Description"].ToString(),
                    MyPicture = row["Picture"].ToString()
                });
            }
            return list;
        }

        // GetProductsAbove500
        public List<Product> GetProductsAbove500()
        {
            string sql = "SELECT * FROM [Product] WHERE [Price] > 500 ORDER BY [Price] ASC";
            OleDbCommand cmd = new OleDbCommand(sql);
            DataSet ds = db.SelectcommandInjection(cmd);

            List<Product> list = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                list.Add(new Product
                {
                    MyProductID = Convert.ToInt32(row["ProductID"]),
                    MyProductName = row["ProductName"].ToString(),
                    MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                    MyBrandID = Convert.ToInt32(row["BrandID"]),
                    MyPrice = Convert.ToDecimal(row["Price"]),
                    MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
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

        public List<Product> GetCars()
        {
            DataSet ds = db.Selectcommand(CarSelectSql + " ORDER BY p.ProductName");
            List<Product> cars = new List<Product>();
            foreach (DataRow row in ds.Tables[0].Rows)
                cars.Add(MapCar(row));
            return cars;
        }

        public Product GetCar(int productID)
        {
            OleDbCommand command = new OleDbCommand(CarSelectSql + " AND p.ProductID = ?");
            command.Parameters.AddWithValue("?", productID);
            DataSet ds = db.SelectcommandInjection(command);
            return ds.Tables[0].Rows.Count == 0 ? null : MapCar(ds.Tables[0].Rows[0]);
        }

        public List<Product> SearchCars(string search, string category, decimal? maximumPrice, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<Product> query = GetCars();
            if (!string.IsNullOrWhiteSpace(search))
            {
                string term = search.Trim();
                query = query.Where(car => (car.MyManufacturer ?? string.Empty).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0 || (car.MyProductName ?? string.Empty).IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(car => string.Equals(car.MyCarCategory, category, StringComparison.OrdinalIgnoreCase));
            if (maximumPrice.HasValue)
                query = query.Where(car => car.MyPrice <= maximumPrice.Value);

            List<Product> cars = query.ToList();
            if (cars.Count > 0 && startDate.HasValue && endDate.HasValue && endDate.Value.Date > startDate.Value.Date)
            {
                Dictionary<int, int> reservations = CountOverlappingReservations(startDate.Value.Date, endDate.Value.Date);
                cars = cars.Where(car => (!reservations.ContainsKey(car.MyProductID) ? 0 : reservations[car.MyProductID]) < car.MyStockQuantity).ToList();
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

        public int AddCar(Product car)
        {
            int productID = GetNextProductID(car.MyCategoryID);
            const string sql = "INSERT INTO [Product] ([ProductID], [ProductName], [CategoryID], [BrandID], [Price], [StockQuantity], [Description], [Picture], [IsCar], [CarYear], [CarCategory], [BranchID], [Seats], [FuelType], [RangeKm], [Transmission], [LuggageCapacity]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, true, ?, ?, ?, ?, ?, ?, ?, ?)";
            OleDbCommand command = new OleDbCommand(sql);
            command.Parameters.AddWithValue("?", productID);
            command.Parameters.AddWithValue("?", car.MyProductName);
            command.Parameters.AddWithValue("?", car.MyCategoryID);
            command.Parameters.AddWithValue("?", car.MyBrandID);
            command.Parameters.AddWithValue("?", car.MyPrice);
            command.Parameters.AddWithValue("?", car.MyStockQuantity);
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
            return productID;
        }

        public void UpdateCar(Product car)
        {
            const string sql = "UPDATE [Product] SET [ProductName]=?, [CategoryID]=?, [BrandID]=?, [Price]=?, [StockQuantity]=?, [Description]=?, [Picture]=?, [CarYear]=?, [CarCategory]=?, [BranchID]=?, [Seats]=?, [FuelType]=?, [RangeKm]=?, [Transmission]=?, [LuggageCapacity]=? WHERE [ProductID]=? AND [IsCar]=true";
            OleDbCommand command = new OleDbCommand(sql);
            command.Parameters.AddWithValue("?", car.MyProductName);
            command.Parameters.AddWithValue("?", car.MyCategoryID);
            command.Parameters.AddWithValue("?", car.MyBrandID);
            command.Parameters.AddWithValue("?", car.MyPrice);
            command.Parameters.AddWithValue("?", car.MyStockQuantity);
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
            command.Parameters.AddWithValue("?", car.MyProductID);
            db.DoCommandInjection(command);
        }

        public void SetCarStock(int productID, int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException("quantity");
            OleDbCommand command = new OleDbCommand("UPDATE [Product] SET [StockQuantity]=? WHERE [ProductID]=? AND [IsCar]=true");
            command.Parameters.AddWithValue("?", quantity);
            command.Parameters.AddWithValue("?", productID);
            db.DoCommandInjection(command);
        }

        public void DeleteCar(int productID)
        {
            OleDbCommand favoriteCleanup = new OleDbCommand("DELETE FROM [Favorite] WHERE [ProductID]=?");
            favoriteCleanup.Parameters.AddWithValue("?", productID);
            db.DoCommandInjection(favoriteCleanup);
            OleDbCommand command = new OleDbCommand("DELETE FROM [Product] WHERE [ProductID]=? AND [IsCar]=true");
            command.Parameters.AddWithValue("?", productID);
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

        public List<Product> GetFavorites(string username)
        {
            OleDbCommand command = new OleDbCommand("SELECT [ProductID], [DateAdded] FROM [Favorite] WHERE [Username]=? ORDER BY [DateAdded] DESC");
            command.Parameters.AddWithValue("?", username);
            DataSet ds = db.SelectcommandInjection(command);
            List<Product> result = new List<Product>();
            if (ds.Tables[0].Rows.Count == 0)
                return result;
            Dictionary<int, Product> cars = GetCars().ToDictionary(car => car.MyProductID);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Product car;
                if (cars.TryGetValue(Convert.ToInt32(row["ProductID"]), out car))
                {
                    car.MyFavoriteDateAdded = row.IsNull("DateAdded") ? (DateTime?)null : Convert.ToDateTime(row["DateAdded"]);
                    result.Add(car);
                }
            }
            return result;
        }

        public void SetFavorite(string username, int productID, bool isFavorite)
        {
            if (isFavorite)
            {
                OleDbCommand command = new OleDbCommand("INSERT INTO [Favorite] ([Username], [ProductID], [DateAdded]) VALUES (?, ?, ?)");
                command.Parameters.AddWithValue("?", username);
                command.Parameters.AddWithValue("?", productID);
                command.Parameters.Add("?", OleDbType.Date).Value = DateTime.Now;
                db.DoCommandInjection(command);
            }
            else
            {
                OleDbCommand command = new OleDbCommand("DELETE FROM [Favorite] WHERE [Username]=? AND [ProductID]=?");
                command.Parameters.AddWithValue("?", username);
                command.Parameters.AddWithValue("?", productID);
                db.DoCommandInjection(command);
            }
        }

        private Product MapCar(DataRow row)
        {
            var car = new Product
            {
                MyProductID = Convert.ToInt32(row["ProductID"]),
                MyProductName = row["ProductName"].ToString(),
                MyCategoryID = Convert.ToInt32(row["CategoryID"]),
                MyBrandID = Convert.ToInt32(row["BrandID"]),
                MyPrice = Convert.ToDecimal(row["Price"]),
                MyStockQuantity = Convert.ToInt32(row["StockQuantity"]),
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