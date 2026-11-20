using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using BusinessModelCarRental;

public partial class CarMatch : Page
{
    protected async void btnRecommend_Click(object sender, EventArgs e)
    {
        pnlRecommendation.Visible = false;
        lblMessage.Text = string.Empty;

        string description = txtTrip.Text == null ? string.Empty : txtTrip.Text.Trim();
        if (string.IsNullOrWhiteSpace(description))
        {
            lblMessage.Text = "נא לתאר את הטיול לפני בקשת המלצה.";
            return;
        }

        try
        {
            Dictionary<int, Car> availableCars = LoadAvailableCars();
            if (availableCars.Count == 0)
            {
                lblMessage.Text = "אין כרגע רכבים זמינים במלאי.";
                return;
            }

            List<CarRecommendationCandidate> candidates = availableCars.Values
                .Select(ToCandidate)
                .ToList();

            var groq = new GroqClient();
            GroqRecommendationResponse recommendation = await groq.RecommendCarAsync(description, candidates);

            Car selected;
            if (!availableCars.TryGetValue(recommendation.CarId, out selected))
            {
                lblMessage.Text = "התקבלה המלצה שאינה תואמת לרכבים הזמינים כרגע. אנא נסו שוב.";
                return;
            }

            string carDetailsUrl = "CarDetails.aspx?id=" + selected.MyCarID;

            litCar.Text = HttpUtility.HtmlEncode((selected.MyManufacturer + " " + selected.MyModelName).Trim());
            litReason.Text = HttpUtility.HtmlEncode(recommendation.Reason) +
                " <a class='text-info text-decoration-underline' href='" + HttpUtility.HtmlAttributeEncode(carDetailsUrl) + "'>למעבר לעמוד הרכב המומלץ</a>";
            litSpecs.Text = HttpUtility.HtmlEncode(BuildSpecsSummary(selected));
            litMissingInfo.Text = string.IsNullOrWhiteSpace(recommendation.MissingInfo)
                ? string.Empty
                : HttpUtility.HtmlEncode("הערת מידע: " + recommendation.MissingInfo);
            lnkCar.NavigateUrl = carDetailsUrl;
            pnlRecommendation.Visible = true;
            lblMessage.Text = "ההמלצה מבוססת על רכבים זמינים במלאי בלבד.";
        }
        catch (GroqClientException ex)
        {
            lblMessage.Text = HttpUtility.HtmlEncode(ex.Message);
        }
        catch (CommunicationException)
        {
            lblMessage.Text = "לא ניתן לגשת לנתוני הרכבים כרגע. נסו שוב מאוחר יותר.";
        }
        catch (TimeoutException)
        {
            lblMessage.Text = "הבקשה חרגה מזמן ההמתנה. נסו שוב.";
        }
        catch
        {
            lblMessage.Text = "אירעה שגיאה זמנית בעת יצירת ההמלצה.";
        }
    }

    [WebMethod]
    public static VoiceTranscriptionResponse TranscribeAudio(string base64Audio, string fileExt)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(base64Audio))
                return VoiceTranscriptionResponse.Fail("לא התקבלו נתוני שמע.");

            byte[] audioBytes;
            try
            {
                audioBytes = Convert.FromBase64String(base64Audio);
            }
            catch
            {
                return VoiceTranscriptionResponse.Fail("פורמט השמע אינו תקין.");
            }

            if (audioBytes.Length < 1000)
                return VoiceTranscriptionResponse.Fail("ההקלטה ריקה או קצרה מדי.");

            var groq = new GroqClient();
            string text = groq.TranscribeAudioAsync(audioBytes, fileExt).GetAwaiter().GetResult();
            if (string.IsNullOrWhiteSpace(text))
                return VoiceTranscriptionResponse.Fail("לא זוהה דיבור ברור. נסו שוב.");

            return VoiceTranscriptionResponse.Ok(text.Trim());
        }
        catch (GroqClientException ex)
        {
            return VoiceTranscriptionResponse.Fail(ex.Message);
        }
        catch
        {
            return VoiceTranscriptionResponse.Fail("אירעה שגיאה בתמלול הקול.");
        }
    }

    private static Dictionary<int, Car> LoadAvailableCars()
    {
        using (var service = new CarRentalServiceClient())
        {
            List<Car> allCars = service.GetCars() ?? new List<Car>();
            return allCars
                .Where(car => car != null && car.MyFleetQuantity > 0)
                .GroupBy(car => car.MyCarID)
                .Select(group => group.First())
                .ToDictionary(car => car.MyCarID, car => car);
        }
    }

    private static CarRecommendationCandidate ToCandidate(Car car)
    {
        return new CarRecommendationCandidate
        {
            CarId = car.MyCarID,
            Manufacturer = car.MyManufacturer,
            Model = car.MyModelName,
            PricePerDay = car.MyPrice,
            Category = car.MyCarCategory,
            Seats = car.MySeats,
            LuggageCapacity = car.MyLuggageCapacity,
            FuelType = car.MyFuelType,
            Transmission = car.MyTransmission,
            RangeKm = car.MyRangeKm,
            Year = car.MyYear,
            Stock = car.MyFleetQuantity,
            Description = car.MyDescription
        };
    }

    private static string BuildSpecsSummary(Car car)
    {
        string seats = car.MySeats.HasValue ? car.MySeats.Value.ToString() : "לא צוין";
        string luggage = car.MyLuggageCapacity.HasValue ? car.MyLuggageCapacity.Value.ToString() : "לא צוין";
        string category = string.IsNullOrWhiteSpace(car.MyCarCategory) ? "לא צוין" : CarRentalDisplay.VehicleCategory(car.MyCarCategory);
        return "קטגוריה: " + category + " | מחיר ליום: ₪" + car.MyPrice.ToString("0.##") + " | מושבים: " + seats + " | מטען: " + luggage;
    }

    public sealed class VoiceTranscriptionResponse
    {
        public bool Success { get; set; }
        public string Text { get; set; }
        public string Message { get; set; }

        public static VoiceTranscriptionResponse Ok(string text)
        {
            return new VoiceTranscriptionResponse { Success = true, Text = text, Message = string.Empty };
        }

        public static VoiceTranscriptionResponse Fail(string message)
        {
            return new VoiceTranscriptionResponse { Success = false, Text = string.Empty, Message = message };
        }
    }
}
