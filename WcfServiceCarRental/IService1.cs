using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using BusinessModelCarRental;

namespace WcfServiceCarRental
{
    
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here

        
        // Make
        [OperationContract]
        bool MakeExists(int MakeID);
        [OperationContract]
        void AddMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description);
        [OperationContract]
        void DeleteMake(int MakeID);
        [OperationContract]
        void UpdateMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description, int MakeID);
        [OperationContract]
        List<Make> GetAllMakes();
        [OperationContract]
        Make GetMake(int MakeID);

        // Cart
        [OperationContract]
        bool isExistCart(string Username);
        [OperationContract]
        void AddCart(int CarID, string Username, int Quantity);
        [OperationContract]
        void DeleteCart(string Username);
        [OperationContract]
        List<Cart> showAllCart();
        [OperationContract]
        List<Cart> showCart(string Username);
        [OperationContract]
        void DeleteCarFromCart(int CarID, string Username);
        [OperationContract]
        bool CarExistsInCart(int CarID, string Username);
        [OperationContract]
        void CopyData(string Username, decimal Price);
        [OperationContract]
        bool UpdateCartQuantity(string Username, int CarID, int newQuantity);
        [OperationContract]
        decimal GetCartTotalPrice(string Username);
        [OperationContract]
        int CountItemsInCart(string Username);
        [OperationContract]
        List<Cart> ListCartWithCarDetails(string Username);
        [OperationContract]
        decimal GetCarTotalPriceInCart(string Username, int CarID);
        [OperationContract]
        bool UpdateCartPrice(string Username, int CarID, decimal NewPrice);
        [OperationContract]
        Cart showCartWithoutUsername(string Username);
        [OperationContract]
        Cart GetCarInCart(int CarID, string Username);
        [OperationContract]
        void IncreaseQuantityByOne(string Username, int CarID);
        [OperationContract]
        void DecreaseQuantityByOne(string Username, int CarID);

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

        // Car
        [OperationContract]
        bool FleetCarExists(int CarID);
        [OperationContract]
        int GetNextCarID();
        [OperationContract]
        void CreateFleetCar(int CarID, string ModelName, int MakeID, decimal Price, int FleetQuantity, string Description, string Picture);
        [OperationContract]
        void DeleteFleetCar(int CarID);
        [OperationContract]
        void UpdateFleetCar(string ModelName, int MakeID, decimal Price, int FleetQuantity, string Description, string Picture, int CarID);
        [OperationContract]
        List<Car> GetAllFleetCars();
        [OperationContract]
        Car GetFleetCarByID(int CarID);
        [OperationContract]
        List<Car> GetFleetCarsByMakeID(int MakeID);
        [OperationContract]
        List<Car> GetFleetCarsByRateDescending();
        [OperationContract]
        List<Car> GetFleetCarsByRateAscending();
        [OperationContract]
        List<Car> SearchFleetCars(string SearchTerm);
        [OperationContract]
        List<Car> FilterFleetCarsByRateRange(decimal MinPrice, decimal MaxPrice);
        [OperationContract]
        List<Car> GetFleetCarsUnder50();
        [OperationContract]
        List<Car> GetFleetCarsBetween51And100();
        [OperationContract]
        List<Car> GetFleetCarsBetween101And200();
        [OperationContract]
        List<Car> GetFleetCarsBetween201And500();
        [OperationContract]
        List<Car> GetFleetCarsAbove500();

        // Review
        [OperationContract]
        bool isExistReview(int ReviewID);
        [OperationContract]
        void AddCarReview(string Username, int CarID, string Rev, int Rating);
        [OperationContract]
        void DeleteReview(int ReviewID);
        [OperationContract]
        void UpdateCarReview(string Username, int CarID, string Rev, int Rating, int ReviewID);
        [OperationContract]
        List<Review> ShowAllReview();
        [OperationContract]
        Review ShowReview(int ReviewID);
        [OperationContract]
        List<Review> ShowAllUserReview(string Username);
        [OperationContract]
        List<Review> ShowAllCarReviews(int CarID);

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
        List<Car> GetCars();
        [OperationContract]
        List<Car> SearchCars(string search, string vehicleCategory, decimal? maximumPrice, DateTime? startDate, DateTime? endDate);
        [OperationContract]
        Car GetCar(int carID);
        [OperationContract]
        int AddCar(Car car);
        [OperationContract]
        void UpdateCar(Car car);
        [OperationContract]
        void DeleteCar(int carID);
        [OperationContract]
        void SetCarFleetQuantity(int carID, int quantity);
        [OperationContract]
        List<Branch> GetBranches();
        [OperationContract]
        void AddBranch(Branch branch);
        [OperationContract]
        void DeleteBranch(int branchID);
        [OperationContract]
        List<Car> GetFavorites(string username);
        [OperationContract]
        void SetFavorite(string username, int carID, bool isFavorite);
        [OperationContract]
        bool IsRentalAvailable(int carID, DateTime startDate, DateTime endDate, int requiredStock);
        [OperationContract]
        List<DateTime> GetUnavailableRentalDates(int carID, DateTime firstDate, DateTime lastDate, int fleetQuantity);
        [OperationContract]
        int CreateRentalOrder(string username, int carID, DateTime startDate, DateTime endDate);
        [OperationContract]
        List<Order> GetRentalOrders(string username);
        [OperationContract]
        bool RequestRentalCancellation(int orderID, string username);
        [OperationContract]
        bool SetRentalStatus(int orderID, string status, bool cancellationRequested, string requiredUsername);
        [OperationContract]
        List<Review> GetRentalReviewsForCar(int carID);
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
