using BusinessModelCarRental;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ViewModelCarRental;


namespace WcfServiceCarRental
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

        // Make
        public bool MakeExists(int MakeID)
        {
            MakeDB b = new MakeDB();
            return b.MakeExists(MakeID);
        }
        public void AddMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description)
        {
            MakeDB b = new MakeDB();
            b.AddMake(MakeName, CountryOfOrigin, WebsiteURL, Description);
        }
        public void DeleteMake(int MakeID)
        {
            MakeDB b = new MakeDB();
            b.DeleteMake(MakeID);
        }
        public void UpdateMake(string MakeName, string CountryOfOrigin, string WebsiteURL, string Description, int MakeID)
        {
            MakeDB b = new MakeDB();
            b.UpdateMake(MakeName, CountryOfOrigin, WebsiteURL, Description, MakeID);
        }
        public List<Make> GetAllMakes()
        {
            MakeDB b = new MakeDB();
            return b.GetAllMakes();
        }
        public Make GetMake(int MakeID)
        {
            MakeDB b = new MakeDB();
            return b.GetMake(MakeID);
        }

        // Cart
        public bool isExistCart(string Username)
        {
            CartDB c = new CartDB();
            return c.isExistCart(Username);
        }
        public void AddCart(int CarID, string Username, int Quantity)
        {
            CartDB c = new CartDB();
            c.AddCart(CarID, Username, Quantity);
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
        public void DeleteCarFromCart(int CarID, string Username)
        {
            CartDB c = new CartDB();
            c.DeleteCarFromCart(CarID, Username);
        }
        public bool CarExistsInCart(int CarID, string Username)
        {
            CartDB c = new CartDB();
            return c.CarExistsInCart(CarID, Username);
        }
        public void CopyData(string Username, decimal Price)
        {
            CartDB c = new CartDB();
            c.CopyData(Username, Price);
        }
        public bool UpdateCartQuantity(string Username, int CarID, int newQuantity)
        {
            CartDB c = new CartDB();
            return c.UpdateCartQuantity(Username, CarID, newQuantity);
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
        public List<Cart> ListCartWithCarDetails(string Username)
        {
            CartDB c = new CartDB();
            return c.ListCartWithCarDetails(Username);
        }
        public decimal GetCarTotalPriceInCart(string Username, int CarID)
        {
            CartDB c = new CartDB();
            return c.GetCarTotalPriceInCart(Username, CarID);
        }
        public bool UpdateCartPrice(string Username, int CarID, decimal NewPrice)
        {
            CartDB c = new CartDB();
            return c.UpdateCartPrice(Username, CarID, NewPrice);
        }
        public Cart showCartWithoutUsername(string Username)
        {
            CartDB c = new CartDB();
            return c.showCartWithoutUsername(Username);
        }
        public Cart GetCarInCart(int CarID, string Username)
        {
            CartDB c = new CartDB();
            return c.GetCarInCart(CarID, Username);
        }
        public void IncreaseQuantityByOne(string Username, int CarID)
        {
            CartDB c = new CartDB();
            c.IncreaseQuantityByOne(Username, CarID);
        }
        public void DecreaseQuantityByOne(string Username, int CarID)
        {
            CartDB c = new CartDB();
            c.DecreaseQuantityByOne(Username, CarID);
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

        // Car
        public bool FleetCarExists(int CarID)
        {
            CarDB p = new CarDB();
            return p.FleetCarExists(CarID);
        }
        public int GetNextCarID()
        {
            CarDB p = new CarDB();
            return p.GetNextCarID();
        }
        public void CreateFleetCar(int CarID, string ModelName, int MakeID, decimal Price, int FleetQuantity, string Description, string Picture)
        {
            CarDB p = new CarDB();
            p.CreateFleetCar(CarID, ModelName, MakeID, Price, FleetQuantity, Description, Picture);
        }
        public void DeleteFleetCar(int CarID)
        {
            CarDB p = new CarDB();
            p.DeleteFleetCar(CarID);
        }
        public void UpdateFleetCar(string ModelName, int MakeID, decimal Price, int FleetQuantity, string Description, string Picture, int CarID)
        {
            CarDB p = new CarDB();
            p.UpdateFleetCar(ModelName, MakeID, Price, FleetQuantity, Description, Picture, CarID);
        }
        public List<Car> GetAllFleetCars()
        {
            CarDB p = new CarDB();
            return p.GetAllFleetCars();
        }
        public Car GetFleetCarByID(int CarID)
        {
            CarDB p = new CarDB();
            return p.GetFleetCarByID(CarID);
        }
        public List<Car> GetFleetCarsByMakeID(int MakeID)
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsByMakeID(MakeID);
        }
        public List<Car> GetFleetCarsByRateDescending()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsByRateDescending();
        }
        public List<Car> GetFleetCarsByRateAscending()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsByRateAscending();
        }
        public List<Car> SearchFleetCars(string SearchTerm)
        {
            CarDB p = new CarDB();
            return p.SearchFleetCars(SearchTerm);
        }
        public List<Car> FilterFleetCarsByRateRange(decimal MinPrice, decimal MaxPrice)
        {
            CarDB p = new CarDB();
            return p.FilterFleetCarsByRateRange(MinPrice, MaxPrice);
        }
        public List<Car> GetFleetCarsUnder50()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsUnder50();
        }
        public List<Car> GetFleetCarsBetween51And100()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsBetween51And100();
        }
        public List<Car> GetFleetCarsBetween101And200()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsBetween101And200();
        }
        public List<Car> GetFleetCarsBetween201And500()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsBetween201And500();
        }
        public List<Car> GetFleetCarsAbove500()
        {
            CarDB p = new CarDB();
            return p.GetFleetCarsAbove500();
        }

        // Review
        public bool isExistReview(int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            return r.isExistReview(ReviewID);
        }
        public void AddCarReview(string Username, int CarID, string Rev, int Rating)
        {
            ReviewDB r = new ReviewDB();
            r.AddCarReview(Username, CarID, Rev, Rating);
        }
        public void DeleteReview(int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            r.DeleteReview(ReviewID);
        }
        public void UpdateCarReview(string Username, int CarID, string Rev, int Rating, int ReviewID)
        {
            ReviewDB r = new ReviewDB();
            r.UpdateCarReview(Username, CarID, Rev, Rating, ReviewID);
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
        public List<Review> ShowAllCarReviews(int CarID)
        {
            ReviewDB r = new ReviewDB();
            return r.ShowAllCarReviews(CarID);
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

        public List<Car> GetCars() { return new CarDB().GetCars(); }
        public List<Car> SearchCars(string search, string vehicleCategory, decimal? maximumPrice, DateTime? startDate, DateTime? endDate) { return new CarDB().SearchCars(search, vehicleCategory, maximumPrice, startDate, endDate); }
        public Car GetCar(int carID) { return new CarDB().GetCar(carID); }
        public int AddCar(Car car) { return new CarDB().AddCar(car); }
        public void UpdateCar(Car car) { new CarDB().UpdateCar(car); }
        public void DeleteCar(int carID) { new CarDB().DeleteCar(carID); }
        public void SetCarFleetQuantity(int carID, int quantity) { new CarDB().SetCarFleetQuantity(carID, quantity); }
        public List<Branch> GetBranches() { return new CarDB().GetBranches(); }
        public void AddBranch(Branch branch) { new CarDB().AddBranch(branch); }
        public void DeleteBranch(int branchID) { new CarDB().DeleteBranch(branchID); }
        public List<Car> GetFavorites(string username) { return new CarDB().GetFavorites(username); }
        public void SetFavorite(string username, int carID, bool isFavorite) { new CarDB().SetFavorite(username, carID, isFavorite); }
        public bool IsRentalAvailable(int carID, DateTime startDate, DateTime endDate, int requiredStock) { return new OrderDB().IsRentalAvailable(carID, startDate, endDate, requiredStock); }
        public List<DateTime> GetUnavailableRentalDates(int carID, DateTime firstDate, DateTime lastDate, int fleetQuantity) { return new OrderDB().GetUnavailableRentalDates(carID, firstDate, lastDate, fleetQuantity); }
        public int CreateRentalOrder(string username, int carID, DateTime startDate, DateTime endDate) { return new OrderDB().CreateRentalOrder(username, carID, startDate, endDate); }
        public List<Order> GetRentalOrders(string username) { return new OrderDB().GetRentalOrders(username); }
        public bool RequestRentalCancellation(int orderID, string username) { return new OrderDB().RequestRentalCancellation(orderID, username); }
        public bool SetRentalStatus(int orderID, string status, bool cancellationRequested, string requiredUsername) { return new OrderDB().SetRentalStatus(orderID, status, cancellationRequested, requiredUsername); }
        public List<Review> GetRentalReviewsForCar(int carID) { return new ReviewDB().GetRentalReviewsForCar(carID); }
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
