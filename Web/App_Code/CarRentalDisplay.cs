using System;

public static class CarRentalDisplay
{
    public static string Category(object value)
    {
        string category = Convert.ToString(value);
        switch (category.ToLowerInvariant())
        {
            case "sedan": return "סדאן";
            case "suv": return "ג׳יפ / SUV";
            case "sports": return "ספורט";
            case "family": return "משפחתי";
            default: return string.IsNullOrWhiteSpace(category) ? "לא צוין" : category;
        }
    }
}
