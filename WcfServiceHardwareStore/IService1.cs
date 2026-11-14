using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using BusinessModelHardwareStore;

namespace WcfServiceHardwareStore
{
    
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        
        // Brand
        [OperationContract]
        bool isExistBrand(int BrandID);
        [OperationContract]
        void AddBrand(string BrandName, string CountryOfOrigin, string WebsiteURL, string Description);
        [OperationContract]
        void DeleteBrand(int BrandID);
        [OperationContract]
        void UpdateBrand(string BrandName, string CountryOfOrigin, string WebsiteURL, string Description, int BrandID);
        [OperationContract]
        List<Brand> showAllBrand();
        [OperationContract]
        Brand showBrand(int BrandID);

        // Cart
        [OperationContract]
        bool isExistCart(string Username);
        [OperationContract]
        void AddCart(int ProductID, string Username, int Quantity);
        [OperationContract]
        void DeleteCart(string Username);
        [OperationContract]
        List<Cart> showAllCart();
        [OperationContract]
        List<Cart> showCart(string Username);
        [OperationContract]
        void DeletePruductFromCart(int ProductID, string Username);
        [OperationContract]
        bool isExistProductInCart(int ProductID, string Username);
        [OperationContract]
        void CopyData(string Username, decimal Price);
        [OperationContract]
        bool UpdateCartQuantity(string Username, int ProductID, int newQuantity);
        [OperationContract]
        decimal GetCartTotalPrice(string Username);
        [OperationContract]
        int CountItemsInCart(string Username);
        [OperationContract]
        List<Cart> ListCartWithProductDetails(string Username);
        [OperationContract]
        decimal GetProductTotalPriceInCart(string Username, int ProductID);
        [OperationContract]
        bool UpdateCartPrice(string Username, int ProductID, decimal NewPrice);
        [OperationContract]
        Cart showCartWithoutUsername(string Username);
        [OperationContract]
        Cart showProductInCart(int ProductID, string Username);
        [OperationContract]
        void IncreaseQuantityByOne(string Username, int ProductID);
        [OperationContract]
        void DecreaseQuantityByOne(string Username, int ProductID);

        // Category
        [OperationContract]
        bool isExistCategory(int CategoryID);
        [OperationContract]
        void AddCategory(string CategoryName, string Description);
        [OperationContract]
        void DeleteCategory(int CategoryID);
        [OperationContract]
        void UpdateCategory(string CategoryName, string Description, int CategoryID);
        [OperationContract]
        List<Category> showAllCategory();
        [OperationContract]
        Category showCategory(int CategoryID);
        [OperationContract]
        void AddProductFromCategory(int CategoryID);

        // Contact
        [OperationContract]
        bool isExistContact(int ContactID);
        [OperationContract]
        void AddContact(string Username, string FullName, string Email, string Subject, string Message, DateTime DateSent);

        [OperationContract]
        void UpdateContactStatus(int ContactID, bool IsTreated);

        [OperationContract]
        void DeleteContact(int ContactID);

        [OperationContract]
        List<Contact> showAllContact();

        [OperationContract]
        Contact ShowContact(int ContactID);

        [OperationContract]
        List<Contact> GetContactsByUser(string Username);

        // Order
        [OperationContract]
        bool isExistOrder(int OrderID);
        [OperationContract]
        void AddOrder(string Username, DateTime OrderDate, decimal TotalPrice, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions);
        [OperationContract]
        void DeleteOrder(int OrderID);
        [OperationContract]
        void UpdateOrder(string Username, DateTime OrderDate, decimal TotalPrice, string Address, string AdditionalInstructions, bool Delivered, int OrderID);
        [OperationContract]
        List<Order> showAllOrders();
        [OperationContract]
        Order showOrder(int OrderID);
        [OperationContract]
        void UpdateOrderStatus(int OrderID, bool Delivered);
        [OperationContract]
        List<Order> showPendingOrders();
        [OperationContract]
        List<Order> showOrdersByUsername(string Username);

        // OrderDetails
        [OperationContract]
        bool isExistOrderDetails(int OrderDetailsID);
        [OperationContract]
        void AddOrderDetails(int OrderID, int ProductID, int Quantity, decimal UnitPrice);
        [OperationContract]
        void DeleteOrderDetails(int OrderDetailsID);
        [OperationContract]
        void UpdateOrderDetails(int OrderID, int ProductID, int Quantity, decimal UnitPrice, int OrderDetailsID);
        [OperationContract]
        List<OrderDetails> showAllOrderDetails();
        [OperationContract]
        OrderDetails showOrderDetails(int OrderDetailsID);

        // Product
        [OperationContract]
        bool isExistProduct(int ProductID);
        [OperationContract]
        int GetNextProductID(int CategoryID);
        [OperationContract]
        void AddProduct(int ProductID, string ProductName, int CategoryID, int BrandID, decimal Price, int StockQuantity, string Description, string Picture);
        [OperationContract]
        void DeleteProduct(int ProductID);
        [OperationContract]
        void UpdateProduct(string ProductName, int CategoryID, int BrandID, decimal Price, int StockQuantity, string Description, string Picture, int ProductID);
        [OperationContract]
        List<Product> showAllProduct();
        [OperationContract]
        Product showProduct(int ProductID);
        [OperationContract]
        List<Product> showAllProductsInnerJoin();
        [OperationContract]
        List<Product> showAllProductByCategoryID(int CategoryID);
        [OperationContract]
        List<Product> showAllProductByBrandID(int BrandID);
        [OperationContract]
        List<Product> showAllProductsPriceDesc();
        [OperationContract]
        List<Product> showAllProductsPriceAsc();
        [OperationContract]
        List<Product> SearchProducts(string SearchTerm);
        [OperationContract]
        List<Product> FilterProductsByPriceRange(decimal MinPrice, decimal MaxPrice);
        [OperationContract]
        List<Product> GetProductsUnder50();
        [OperationContract]
        List<Product> GetProductsBetween51And100();
        [OperationContract]
        List<Product> GetProductsBetween101And200();
        [OperationContract]
        List<Product> GetProductsBetween201And500();
        [OperationContract]
        List<Product> GetProductsAbove500();

        // Review
        [OperationContract]
        bool isExistReview(int ReviewID);
        [OperationContract]
        void AddReview(string Username, int ProductID, string Rev, int Rating);
        [OperationContract]
        void DeleteReview(int ReviewID);
        [OperationContract]
        void UpdateReview(string Username, int ProductID, string Rev, int Rating, int ReviewID);
        [OperationContract]
        List<Review> ShowAllReview();
        [OperationContract]
        Review ShowReview(int ReviewID);
        [OperationContract]
        List<Review> ShowAllUserReview(string Username);
        [OperationContract]
        List<Review> ShowAllProductReview(int ProductID);

        // ShippingAddress
        [OperationContract]
        bool isExistShippingAddress(string Username);
        [OperationContract]
        void AddShippingAddress(string Username, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions);
        [OperationContract]
        void UpdateShippingAddress(string Username, string CountryRegion, string City, string StreetAddress, string Floor, string ZipCode, string AdditionalInstructions);
        [OperationContract]
        void DeleteShippingAddress(string Username);
        [OperationContract]
        List<ShippingAddress> ShowAllShippingAddress();
        [OperationContract]
        ShippingAddress ShowShippingAddress(string Username);

        // User
        [OperationContract]
        bool isExistUser(string Username);
        [OperationContract]
        void AddUser(string Username, string Password, string PhoneNumber, string Email, bool Admin, DateTime JoinDate);
        [OperationContract]
        void DeleteUser(string Username);
        [OperationContract]
        void UpdateUser(string Password, string PhoneNumber, string Email, bool Admin, DateTime JoinDate, string Username);
        [OperationContract]
        List<User> showAllUsers();
        [OperationContract]
        User showUser(string Username);
        [OperationContract]
        bool IsExistUserAndPass(string Username, string Password);
        [OperationContract]
        bool IsUserAdmin(string username);
        [OperationContract]
        User ShowUserByEmail(string Email);

        [OperationContract]
        List<Product> GetCars();
        [OperationContract]
        List<Product> SearchCars(string search, string category, decimal? maximumPrice, DateTime? startDate, DateTime? endDate);
        [OperationContract]
        Product GetCar(int productID);
        [OperationContract]
        int AddCar(Product car);
        [OperationContract]
        void UpdateCar(Product car);
        [OperationContract]
        void DeleteCar(int productID);
        [OperationContract]
        void SetCarStock(int productID, int quantity);
        [OperationContract]
        List<Branch> GetBranches();
        [OperationContract]
        void AddBranch(Branch branch);
        [OperationContract]
        void DeleteBranch(int branchID);
        [OperationContract]
        List<Product> GetFavorites(string username);
        [OperationContract]
        void SetFavorite(string username, int productID, bool isFavorite);
        [OperationContract]
        bool IsRentalAvailable(int carID, DateTime startDate, DateTime endDate, int requiredStock);
        [OperationContract]
        List<DateTime> GetUnavailableRentalDates(int carID, DateTime firstDate, DateTime lastDate, int stock);
        [OperationContract]
        int CreateRentalOrder(string username, int carID, DateTime startDate, DateTime endDate);
        [OperationContract]
        List<Order> GetRentalOrders(string username);
        [OperationContract]
        bool RequestRentalCancellation(int orderID, string username);
        [OperationContract]
        bool SetRentalStatus(int orderID, string status, bool cancellationRequested, string requiredUsername);
        [OperationContract]
        List<Review> GetRentalReviewsForCar(int productID);
        [OperationContract]
        bool CanReviewRental(string username, int orderID);
        [OperationContract]
        void AddRentalReview(string username, int orderID, int rating, string comment);
        [OperationContract]
        bool IsEmailRegistered(string email, string exceptUsername);
        [OperationContract]
        void RegisterCustomer(string username, string password, string phoneNumber, string email, string firstName, string lastName);
        [OperationContract]
        void UpdateProfile(string username, string firstName, string lastName, string email, string phoneNumber);
        [OperationContract]
        User GetCustomerProfile(string username);
        [OperationContract]
        bool SetUserAdmin(string targetUsername, bool admin, string administratorUsername);
        [OperationContract]
        bool DeleteManagedUser(string targetUsername, string administratorUsername);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
