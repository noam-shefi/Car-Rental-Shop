using BusinessModelHardwareStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ViewModelHardwareStore;


namespace WcfServiceHardwareStore
{
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        // Brand
        public bool isExistBrand(int BrandID)
        {
            BrandDB b = new BrandDB();
            return b.isExistBrand(BrandID);
        }
        public void AddBrand(string BrandName, string CountryOfOrigin, string WebsiteURL, string Description)
        {
            BrandDB b = new BrandDB();
            b.AddBrand(BrandName, CountryOfOrigin, WebsiteURL, Description);
        }
        public void DeleteBrand(int BrandID)
        {
            BrandDB b = new BrandDB();
            b.DeleteBrand(BrandID);
        }
        public void UpdateBrand(string BrandName, string CountryOfOrigin, string WebsiteURL, string Description, int BrandID)
        {
            BrandDB b = new BrandDB();
            b.UpdateBrand(BrandName, CountryOfOrigin, WebsiteURL, Description, BrandID);
        }
        public List<Brand> showAllBrand()
        {
            BrandDB b = new BrandDB();
            return b.showAllBrand();
        }
        public Brand showBrand(int BrandID)
        {
            BrandDB b = new BrandDB();
            return b.showBrand(BrandID);
        }

        // Cart
        public bool isExistCart(string Username)
        {
            CartDB c = new CartDB();
            return c.isExistCart(Username);
        }
        public void AddCart(int ProductID, string Username, int Quantity)
        {
            CartDB c = new CartDB();
            c.AddCart(ProductID, Username, Quantity);
        }
        public void DeleteCart(string Username)
        {
            CartDB c = new CartDB();
            c.DeleteCart(Username);
        }
        public List<Cart> showAllCart()
        {
            CartDB c = new CartDB();
            return c.showAllCart();
        }
        public List<Cart> showCart(string Username)
        {
            CartDB c = new CartDB();
            return c.showCart(Username);
        }
        public void DeletePruductFromCart(int ProductID, string Username)
        {
            CartDB c = new CartDB();
            c.DeletePruductFromCart(ProductID, Username);
        }
        public bool isExistProductInCart(int ProductID, string Username)
        {
            CartDB c = new CartDB();
            return c.isExistProductInCart(ProductID, Username);
        }
        public void CopyData(string Username, decimal Price)
        {
            CartDB c = new CartDB();
            c.CopyData(Username, Price);
        }
        public bool UpdateCartQuantity(string Username, int ProductID, int newQuantity)
        {
            CartDB c = new CartDB();
            return c.UpdateCartQuantity(Username, ProductID, newQuantity);
        }
        public decimal GetCartTotalPrice(string Username)
        {
            CartDB c = new CartDB();
            return c.GetCartTotalPrice(Username);
        }
        public int CountItemsInCart(string Username)
        {
            CartDB c = new CartDB();
            return c.CountItemsInCart(Username);
        }
        public List<Cart> ListCartWithProductDetails(string Username)
        {
            CartDB c = new CartDB();
            return c.ListCartWithProductDetails(Username);
        }
        public decimal GetProductTotalPriceInCart(string Username, int ProductID)
        {
            CartDB c = new CartDB();
            return c.GetProductTotalPriceInCart(Username, ProductID);
        }
        public bool UpdateCartPrice(string Username, int ProductID, decimal NewPrice)
        {
            CartDB c = new CartDB();
            return c.UpdateCartPrice(Username, ProductID, NewPrice);
        }
        public Cart showCartWithoutUsername(string Username)
        {
            CartDB c = new CartDB();
            return c.showCartWithoutUsername(Username);
        }
        public Cart showProductInCart(int ProductID, string Username)
        {
            CartDB c = new CartDB();
            return c.showProductInCart(ProductID, Username);
        }
        public void IncreaseQuantityByOne(string Username, int ProductID)
        {
            CartDB c = new CartDB();
            c.IncreaseQuantityByOne(Username, ProductID);
        }
        public void DecreaseQuantityByOne(string Username, int ProductID)
        {
            CartDB c = new CartDB();
            c.DecreaseQuantityByOne(Username, ProductID);
        }

        // Category
        public bool isExistCategory(int CategoryID)
        {
            CategoryDB c = new CategoryDB();
            return c.isExistCategory(CategoryID);
        }
        public void AddCategory(string CategoryName, string Description)
        {
            CategoryDB c = new CategoryDB();
            c.AddCategory(CategoryName, Description);
        }
        public void DeleteCategory(int CategoryID)
        {
            CategoryDB c = new CategoryDB();
            c.DeleteCategory(CategoryID);
        }
        public void UpdateCategory(string CategoryName, string Description, int CategoryID)
        {
            CategoryDB c = new CategoryDB();
            c.UpdateCategory(CategoryName, Description, CategoryID);
        }
        public List<Category> showAllCategory()
        {
            CategoryDB c = new CategoryDB();
            return c.showAllCategory();
        }
        public Category showCategory(int CategoryID)
        {
            CategoryDB c = new CategoryDB();
            return c.showCategory(CategoryID);
        }
        public void AddProductFromCategory(int CategoryID)
        {
            CategoryDB c = new CategoryDB();
            c.AddProductFromCategory(CategoryID);
        }

        // Contact
        public bool isExistContact(int ContactID)
        {
            ContactDB c = new ContactDB();
            return c.isExistContact(ContactID);
        }

        public void AddContact(string Username, string FullName, string Email, string Subject, string Message, DateTime DateSent)
        {
            ContactDB c = new ContactDB();
            c.AddContact(Username, FullName, Email, Subject, Message, DateSent);
        }

        public void UpdateContactStatus(int ContactID, bool IsTreated)
        {
            ContactDB c = new ContactDB();
            c.UpdateContactStatus(ContactID, IsTreated);
        }

        public void DeleteContact(int ContactID)
        {
            ContactDB c = new ContactDB();
            c.DeleteContact(ContactID);
        }

        public List<Contact> showAllContact()
        {
            ContactDB c = new ContactDB();
            return c.showAllContact();
        }

        public Contact ShowContact(int ContactID)
        {
            ContactDB c = new ContactDB();
            return c.ShowContact(ContactID);
        }

        public List<Contact> GetContactsByUser(string Username)
        {
            ContactDB c = new ContactDB();
            return c.GetContactsByUser(Username);
        }

        // Order
        public bool isExistOrder(int OrderID)
        {
            OrderDB o = new OrderDB();
            return o.isExistOrder(OrderID);
        }
        public void AddOrder(string Username, DateTime OrderDate, decimal TotalPrice, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions)
        {
            OrderDB o = new OrderDB();
            o.AddOrder(Username, OrderDate, TotalPrice, CountryRegion, City, StreetAddress, Floor, ZipCode, AdditionalInstructions);
        }
        public void DeleteOrder(int OrderID)
        {
            OrderDB o = new OrderDB();
            o.DeleteOrder(OrderID);
        }
        public void UpdateOrder(string Username, DateTime OrderDate, decimal TotalPrice, string Address, string AdditionalInstructions, bool Delivered, int OrderID)
        {
            OrderDB o = new OrderDB();
            o.UpdateOrder(Username, OrderDate, TotalPrice, Address, AdditionalInstructions, Delivered, OrderID);
        }
        public List<Order> showAllOrders()
        {
            OrderDB o = new OrderDB();
            return o.showAllOrders();
        }
        public Order showOrder(int OrderID)
        {
            OrderDB o = new OrderDB();
            return o.showOrder(OrderID);
        }
        public void UpdateOrderStatus(int OrderID, bool Delivered)
        {
            OrderDB o = new OrderDB();
            o.UpdateOrderStatus(OrderID, Delivered);
        }
        public List<Order> showPendingOrders()
        {
            OrderDB o = new OrderDB();
            return o.showPendingOrders();
        }
        public List<Order> showOrdersByUsername(string Username)
        {
            OrderDB o = new OrderDB();
            return o.showOrdersByUsername(Username);
        }

        // OrderDetails
        public bool isExistOrderDetails(int OrderDetailsID)
        {
            OrderDetailsDB o = new OrderDetailsDB();
            return o.isExistOrderDetails(OrderDetailsID);
        }
        public void AddOrderDetails(int OrderID, int ProductID, int Quantity, decimal UnitPrice)
        {
            OrderDetailsDB o = new OrderDetailsDB();
            o.AddOrderDetails(OrderID, ProductID, Quantity, UnitPrice);
        }
        public void DeleteOrderDetails(int OrderDetailsID)
        {
            OrderDetailsDB o = new OrderDetailsDB();
            o.DeleteOrderDetails(OrderDetailsID);
        }
        public void UpdateOrderDetails(int OrderID, int ProductID, int Quantity, decimal UnitPrice, int OrderDetailsID)
        {
            OrderDetailsDB o = new OrderDetailsDB();
            o.UpdateOrderDetails(OrderID, ProductID, Quantity, UnitPrice, OrderDetailsID);
        }
        public List<OrderDetails> showAllOrderDetails()
        {
            OrderDetailsDB o = new OrderDetailsDB();
            return o.showAllOrderDetails();
        }
        public OrderDetails showOrderDetails(int OrderDetailsID)
        {
            OrderDetailsDB o = new OrderDetailsDB();
            return o.showOrderDetails(OrderDetailsID);
        }

        // Product
        public bool isExistProduct(int ProductID)
        {
            ProductDB p = new ProductDB();
            return p.isExistProduct(ProductID);
        }
        public int GetNextProductID(int CategoryID)
        {
            ProductDB p = new ProductDB();
            return p.GetNextProductID(CategoryID);
        }
        public void AddProduct(int ProductID, string ProductName, int CategoryID, int BrandID, decimal Price, int StockQuantity, string Description, string Picture)
        {
            ProductDB p = new ProductDB();
            p.AddProduct(ProductID, ProductName, CategoryID, BrandID, Price, StockQuantity, Description, Picture);
        }
        public void DeleteProduct(int ProductID)
        {
            ProductDB p = new ProductDB();
            p.DeleteProduct(ProductID);
        }
        public void UpdateProduct(string ProductName, int CategoryID, int BrandID, decimal Price, int StockQuantity, string Description, string Picture, int ProductID)
        {
            ProductDB p = new ProductDB();
            p.UpdateProduct(ProductName, CategoryID, BrandID, Price, StockQuantity, Description, Picture, ProductID);
        }
        public List<Product> showAllProduct()
        {
            ProductDB p = new ProductDB();
            return p.showAllProduct();
        }
        public Product showProduct(int ProductID)
        {
            ProductDB p = new ProductDB();
            return p.showProduct(ProductID);
        }
        public List<Product> showAllProductsInnerJoin()
        {
            ProductDB p = new ProductDB();
            return p.showAllProductsInnerJoin();
        }
        public List<Product> showAllProductByCategoryID(int CategoryID)
        {
            ProductDB p = new ProductDB();
            return p.showAllProductByCategoryID(CategoryID);
        }
        public List<Product> showAllProductByBrandID(int BrandID)
        {
            ProductDB p = new ProductDB();
            return p.showAllProductByBrandID(BrandID);
        }
        public List<Product> showAllProductsPriceDesc()
        {
            ProductDB p = new ProductDB();
            return p.showAllProductsPriceDesc();
        }
        public List<Product> showAllProductsPriceAsc()
        {
            ProductDB p = new ProductDB();
            return p.showAllProductsPriceAsc();
        }
        public List<Product> SearchProducts(string SearchTerm)
        {
            ProductDB p = new ProductDB();
            return p.SearchProducts(SearchTerm);
        }
        public List<Product> FilterProductsByPriceRange(decimal MinPrice, decimal MaxPrice)
        {
            ProductDB p = new ProductDB();
            return p.FilterProductsByPriceRange(MinPrice, MaxPrice);
        }
        public List<Product> GetProductsUnder50()
        {
            ProductDB p = new ProductDB();
            return p.GetProductsUnder50();
        }
        public List<Product> GetProductsBetween51And100()
        {
            ProductDB p = new ProductDB();
            return p.GetProductsBetween51And100();
        }
        public List<Product> GetProductsBetween101And200()
        {
            ProductDB p = new ProductDB();
            return p.GetProductsBetween101And200();
        }
        public List<Product> GetProductsBetween201And500()
        {
            ProductDB p = new ProductDB();
            return p.GetProductsBetween201And500();
        }
        public List<Product> GetProductsAbove500()
        {
            ProductDB p = new ProductDB();
            return p.GetProductsAbove500();
        }

        // Review
        public bool isExistReview(int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            return r.isExistReview(ReviewID);
        }
        public void AddReview(string Username, int ProductID, string Rev, int Rating)
        {
            ReviewDB r = new ReviewDB();
            r.AddReview(Username, ProductID, Rev, Rating);
        }
        public void DeleteReview(int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            r.DeleteReview(ReviewID);
        }
        public void UpdateReview(string Username, int ProductID, string Rev, int Rating, int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            r.UpdateReview(Username, ProductID, Rev, Rating, ReviewID);
        }
        public List<Review> ShowAllReview()
        {
            ReviewDB r = new ReviewDB();
            return r.ShowAllReview();
        }
        public Review ShowReview(int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            return r.ShowReview(ReviewID);
        }
        public List<Review> ShowAllUserReview(string Username)
        {
            ReviewDB r = new ReviewDB();
            return r.ShowAllUserReview(Username);
        }
        public List<Review> ShowAllProductReview(int ProductID)
        {
            ReviewDB r = new ReviewDB();
            return r.ShowAllProductReview(ProductID);
        }

        // ShippingAddress
        public bool isExistShippingAddress(string Username)
        {
            ShippingAddressDB db = new ShippingAddressDB();
            return db.IsExistShippingAddress(Username);
        }
        public void AddShippingAddress(string Username, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions)
        {
            ShippingAddressDB db = new ShippingAddressDB();
            db.AddShippingAddress(Username, CountryRegion, City, StreetAddress, Floor, ZipCode, AdditionalInstructions);
        }
        public void UpdateShippingAddress(string Username, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions)
        {
            ShippingAddressDB db = new ShippingAddressDB();
            db.UpdateShippingAddress(Username, CountryRegion, City, StreetAddress, Floor, ZipCode, AdditionalInstructions);
        }
        public void DeleteShippingAddress(string Username)
        {
            ShippingAddressDB db = new ShippingAddressDB();
            db.DeleteShippingAddress(Username);
        }
        public List<ShippingAddress> ShowAllShippingAddress()
        {
            ShippingAddressDB db = new ShippingAddressDB();
            return db.ShowAllShippingAddress();
        }
        public ShippingAddress ShowShippingAddress(string Username)
        {
            ShippingAddressDB db = new ShippingAddressDB();
            return db.ShowShippingAddress(Username);
        }

        // User
        public bool isExistUser(string Username)
        {
            UserDB u = new UserDB();
            return u.isExistUser(Username);
        }
        public void AddUser(string Username, string Password, string PhoneNumber, string Email, bool Admin, DateTime JoinDate)
        {
            UserDB u = new UserDB();
            u.AddUser(Username, Password, PhoneNumber, Email, Admin, JoinDate);
        }
        public void DeleteUser(string Username)
        {
            UserDB u = new UserDB();
            u.DeleteUser(Username);
        }
        public void UpdateUser(string Password, string PhoneNumber, string Email, bool Admin, DateTime JoinDate, string Username)
        {
            UserDB u = new UserDB();
            u.UpdateUser(Password, PhoneNumber, Email, Admin, JoinDate, Username);
        }
        public List<User> showAllUsers()
        {
            UserDB u = new UserDB();
            return u.showAllUsers();
        }
        public User showUser(string Username)
        {
            UserDB u = new UserDB();
            return u.showUser(Username);
        }
        public bool IsExistUserAndPass(string Username, string Password)
        {
            UserDB u = new UserDB();
            return u.IsExistUserAndPass(Username, Password);
        }
        public bool IsUserAdmin(string username)
        {
            UserDB u = new UserDB();
            return u.IsUserAdmin(username);
        }
        public User ShowUserByEmail(string Email)
        {
            UserDB u = new UserDB();
            return u.ShowUserByEmail(Email);
        }

        public List<Product> GetCars() { return new ProductDB().GetCars(); }
        public List<Product> SearchCars(string search, string category, decimal? maximumPrice, DateTime? startDate, DateTime? endDate) { return new ProductDB().SearchCars(search, category, maximumPrice, startDate, endDate); }
        public Product GetCar(int productID) { return new ProductDB().GetCar(productID); }
        public int AddCar(Product car) { return new ProductDB().AddCar(car); }
        public void UpdateCar(Product car) { new ProductDB().UpdateCar(car); }
        public void DeleteCar(int productID) { new ProductDB().DeleteCar(productID); }
        public void SetCarStock(int productID, int quantity) { new ProductDB().SetCarStock(productID, quantity); }
        public List<Branch> GetBranches() { return new ProductDB().GetBranches(); }
        public void AddBranch(Branch branch) { new ProductDB().AddBranch(branch); }
        public void DeleteBranch(int branchID) { new ProductDB().DeleteBranch(branchID); }
        public List<Product> GetFavorites(string username) { return new ProductDB().GetFavorites(username); }
        public void SetFavorite(string username, int productID, bool isFavorite) { new ProductDB().SetFavorite(username, productID, isFavorite); }
        public bool IsRentalAvailable(int carID, DateTime startDate, DateTime endDate, int requiredStock) { return new OrderDB().IsRentalAvailable(carID, startDate, endDate, requiredStock); }
        public List<DateTime> GetUnavailableRentalDates(int carID, DateTime firstDate, DateTime lastDate, int stock) { return new OrderDB().GetUnavailableRentalDates(carID, firstDate, lastDate, stock); }
        public int CreateRentalOrder(string username, int carID, DateTime startDate, DateTime endDate) { return new OrderDB().CreateRentalOrder(username, carID, startDate, endDate); }
        public List<Order> GetRentalOrders(string username) { return new OrderDB().GetRentalOrders(username); }
        public bool RequestRentalCancellation(int orderID, string username) { return new OrderDB().RequestRentalCancellation(orderID, username); }
        public bool SetRentalStatus(int orderID, string status, bool cancellationRequested, string requiredUsername) { return new OrderDB().SetRentalStatus(orderID, status, cancellationRequested, requiredUsername); }
        public List<Review> GetRentalReviewsForCar(int productID) { return new ReviewDB().GetRentalReviewsForCar(productID); }
        public bool CanReviewRental(string username, int orderID) { return new ReviewDB().CanReviewRental(username, orderID); }
        public void AddRentalReview(string username, int orderID, int rating, string comment) { new ReviewDB().AddRentalReview(username, orderID, rating, comment); }
        public bool IsEmailRegistered(string email, string exceptUsername) { return new UserDB().IsEmailRegistered(email, exceptUsername); }
        public void RegisterCustomer(string username, string password, string phoneNumber, string email, string firstName, string lastName) { new UserDB().RegisterCustomer(username, password, phoneNumber, email, firstName, lastName); }
        public void UpdateProfile(string username, string firstName, string lastName, string email, string phoneNumber) { new UserDB().UpdateProfile(username, firstName, lastName, email, phoneNumber); }
        public User GetCustomerProfile(string username) { return new UserDB().showUser(username); }
        public bool SetUserAdmin(string targetUsername, bool admin, string administratorUsername) { return new UserDB().SetUserAdmin(targetUsername, admin, administratorUsername); }
        public bool DeleteManagedUser(string targetUsername, string administratorUsername) { return new UserDB().DeleteManagedUser(targetUsername, administratorUsername); }
    }
}
