using System;

public static class CarRentalDisplay
{
    public static string VehicleCategory(object value)
    {
        string vehicleCategory = Convert.ToString(value);
        switch (vehicleCategory.ToLowerInvariant())
        {
            case "sedan": return "סדאן";
            case "suv": return "ג׳יפ / SUV";
            case "sports": return "ספורט";
            case "family": return "משפחתי";
            default: return string.IsNullOrWhiteSpace(vehicleCategory) ? "לא צוין" : vehicleCategory;
        }
    }
}
