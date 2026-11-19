using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using BusinessModelCarRental;

[ServiceContract(Name = "IService1", Namespace = "http://tempuri.org/")]
public interface ICarRentalServiceContract
{
    [OperationContract] List<Car> GetCars();
    [OperationContract] List<Car> SearchCars(string search, string vehicleCategory, decimal? maximumPrice, DateTime? startDate, DateTime? endDate);
    [OperationContract] Car GetCar(int carID);
    [OperationContract] int AddCar(Car car);
    [OperationContract] void UpdateCar(Car car);
    [OperationContract] void DeleteCar(int carID);
    [OperationContract] void SetCarFleetQuantity(int carID, int quantity);
    [OperationContract] List<Branch> GetBranches();
    [OperationContract] void AddBranch(Branch branch);
    [OperationContract] void DeleteBranch(int branchID);
    [OperationContract] List<Car> GetFavorites(string username);
    [OperationContract] void SetFavorite(string username, int carID, bool isFavorite);
    [OperationContract] bool IsRentalAvailable(int carID, DateTime startDate, DateTime endDate, int requiredStock);
    [OperationContract] List<DateTime> GetUnavailableRentalDates(int carID, DateTime firstDate, DateTime lastDate, int fleetQuantity);
    [OperationContract] int CreateRentalOrder(string username, int carID, DateTime startDate, DateTime endDate);
    [OperationContract] List<Order> GetRentalOrders(string username);
    [OperationContract] bool RequestRentalCancellation(int orderID, string username);
    [OperationContract] bool SetRentalStatus(int orderID, string status, bool cancellationRequested, string requiredUsername);
    [OperationContract] List<Review> GetRentalReviewsForCar(int carID);
    [OperationContract] bool CanReviewRental(string username, int orderID);
    [OperationContract] void AddRentalReview(string username, int orderID, int rating, string comment);
    [OperationContract] bool IsEmailRegistered(string email, string exceptUsername);
    [OperationContract] void RegisterCustomer(string username, string password, string phoneNumber, string email, string firstName, string lastName);
    [OperationContract] void UpdateProfile(string username, string firstName, string lastName, string email, string phoneNumber);
    [OperationContract] User GetCustomerProfile(string username);
    [OperationContract] bool SetUserAdmin(string targetUsername, bool admin, string administratorUsername);
    [OperationContract] bool DeleteManagedUser(string targetUsername, string administratorUsername);
}

public sealed class CarRentalServiceClient : IDisposable
{
    private readonly ChannelFactory<ICarRentalServiceContract> factory;
    private readonly ICarRentalServiceContract service;

    public CarRentalServiceClient()
    {
        string address = ConfigurationManager.AppSettings["CarRentalServiceAddress"];
        if (string.IsNullOrWhiteSpace(address))
            throw new ConfigurationErrorsException("CarRentalServiceAddress is not configured.");

        factory = new ChannelFactory<ICarRentalServiceContract>(new BasicHttpBinding(), new EndpointAddress(address));
        service = factory.CreateChannel();
    }

    public List<Car> GetCars() { return service.GetCars(); }
    public List<Car> SearchCars(string search, string vehicleCategory, decimal? maximumPrice, DateTime? startDate, DateTime? endDate) { return service.SearchCars(search, vehicleCategory, maximumPrice, startDate, endDate); }
    public Car GetCar(int id) { return service.GetCar(id); }
    public int AddCar(Car car) { return service.AddCar(car); }
    public void UpdateCar(Car car) { service.UpdateCar(car); }
    public void DeleteCar(int id) { service.DeleteCar(id); }
    public void SetCarFleetQuantity(int id, int quantity) { service.SetCarFleetQuantity(id, quantity); }
    public List<Branch> GetBranches() { return service.GetBranches(); }
    public void AddBranch(Branch branch) { service.AddBranch(branch); }
    public void DeleteBranch(int id) { service.DeleteBranch(id); }
    public List<Car> GetFavorites(string username) { return service.GetFavorites(username); }
    public void SetFavorite(string username, int id, bool value) { service.SetFavorite(username, id, value); }
    public bool IsRentalAvailable(int id, DateTime start, DateTime end, int fleetQuantity) { return service.IsRentalAvailable(id, start, end, fleetQuantity); }
    public List<DateTime> GetUnavailableRentalDates(int id, DateTime first, DateTime last, int fleetQuantity) { return service.GetUnavailableRentalDates(id, first, last, fleetQuantity); }
    public int CreateRentalOrder(string username, int id, DateTime start, DateTime end) { return service.CreateRentalOrder(username, id, start, end); }
    public List<Order> GetRentalOrders(string username) { return service.GetRentalOrders(username); }
    public bool RequestRentalCancellation(int id, string username) { return service.RequestRentalCancellation(id, username); }
    public bool SetRentalStatus(int id, string status, bool cancellationRequested, string requiredUsername) { return service.SetRentalStatus(id, status, cancellationRequested, requiredUsername); }
    public List<Review> GetRentalReviewsForCar(int id) { return service.GetRentalReviewsForCar(id); }
    public bool CanReviewRental(string username, int id) { return service.CanReviewRental(username, id); }
    public void AddRentalReview(string username, int id, int rating, string comment) { service.AddRentalReview(username, id, rating, comment); }
    public bool IsEmailRegistered(string email, string exceptUsername) { return service.IsEmailRegistered(email, exceptUsername); }
    public void RegisterCustomer(string username, string password, string phone, string email, string firstName, string lastName) { service.RegisterCustomer(username, password, phone, email, firstName, lastName); }
    public void UpdateProfile(string username, string firstName, string lastName, string email, string phone) { service.UpdateProfile(username, firstName, lastName, email, phone); }
    public User GetCustomerProfile(string username) { return service.GetCustomerProfile(username); }
    public bool SetUserAdmin(string username, bool admin, string administratorUsername) { return service.SetUserAdmin(username, admin, administratorUsername); }
    public bool DeleteManagedUser(string username, string administratorUsername) { return service.DeleteManagedUser(username, administratorUsername); }

    public void Dispose()
    {
        IClientChannel channel = service as IClientChannel;
        if (channel == null)
            return;

        try
        {
            channel.Close();
            factory.Close();
        }
        catch (CommunicationException)
        {
            channel.Abort();
            factory.Abort();
        }
        catch (TimeoutException)
        {
            channel.Abort();
            factory.Abort();
        }
    }
}
