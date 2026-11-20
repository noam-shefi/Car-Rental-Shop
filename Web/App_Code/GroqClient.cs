using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public sealed class GroqClient
{
    private const string DefaultBaseUrl = "https://api.groq.com/openai/v1";
    private const string DefaultChatModel = "llama-3.3-70b-versatile";
    private const string DefaultWhisperModel = "whisper-large-v3";
    private const int DefaultTimeoutSeconds = 30;

    private readonly string apiKey;
    private readonly string baseUrl;
    private readonly string chatModel;
    private readonly string whisperModel;
    private readonly string chatFallbackModelsCsv;
    private readonly int timeoutMilliseconds;

    public GroqClient()
    {
        apiKey = (ConfigurationManager.AppSettings["GroqApiKey"] ?? string.Empty).Trim();
        baseUrl = (ConfigurationManager.AppSettings["GroqBaseUrl"] ?? DefaultBaseUrl).Trim().TrimEnd('/');
        chatModel = (ConfigurationManager.AppSettings["GroqChatModel"] ?? DefaultChatModel).Trim();
        whisperModel = (ConfigurationManager.AppSettings["GroqWhisperModel"] ?? DefaultWhisperModel).Trim();
        chatFallbackModelsCsv = (ConfigurationManager.AppSettings["GroqChatFallbackModels"] ?? "llama-3.1-8b-instant,llama3-8b-8192,mixtral-8x7b-32768").Trim();

        int timeoutSeconds;
        if (!int.TryParse(ConfigurationManager.AppSettings["GroqTimeoutSeconds"], NumberStyles.Integer, CultureInfo.InvariantCulture, out timeoutSeconds) || timeoutSeconds < 5)
            timeoutSeconds = DefaultTimeoutSeconds;

        timeoutMilliseconds = timeoutSeconds * 1000;
    }

    public Task<GroqRecommendationResponse> RecommendCarAsync(string tripDescription, ICollection<CarRecommendationCandidate> availableCars)
    {
        return Task.Run(() => RecommendCar(tripDescription, availableCars));
    }

    public Task<string> TranscribeAudioAsync(byte[] audioBytes, string fileExtension)
    {
        return Task.Run(() => TranscribeAudio(audioBytes, fileExtension));
    }

    private GroqRecommendationResponse RecommendCar(string tripDescription, ICollection<CarRecommendationCandidate> availableCars)
    {
        EnsureReadyForApiCall();

        if (string.IsNullOrWhiteSpace(tripDescription))
            throw new GroqClientException("נא להזין תיאור נסיעה.");
        if (availableCars == null || availableCars.Count == 0)
            throw new GroqClientException("אין רכבים זמינים להמלצה כרגע.");

        JObject systemMessage = new JObject();
        systemMessage["role"] = "system";
        systemMessage["content"] = BuildSystemPrompt();

        JObject userMessage = new JObject();
        userMessage["role"] = "user";
        userMessage["content"] = BuildUserPrompt(tripDescription, availableCars);

        JArray messages = new JArray();
        messages.Add(systemMessage);
        messages.Add(userMessage);

        List<string> modelsToTry = BuildModelsToTry();
        string lastError = string.Empty;

        foreach (string currentModel in modelsToTry)
        {
            JObject payload = new JObject();
            payload["model"] = currentModel;
            payload["temperature"] = 0.2;
            payload["messages"] = messages;

            try
            {
                string responseText = SendJsonRequest(baseUrl + "/chat/completions", payload.ToString());
                string content = ParseChatMessageContent(responseText);
                GroqRecommendationResponse result = ParseRecommendationJson(content);
                if (result == null || result.CarId <= 0 || string.IsNullOrWhiteSpace(result.Reason))
                    throw new GroqClientException("לא התקבלה תשובת המלצה תקינה מהשירות.");

                return result;
            }
            catch (GroqHttpException httpEx)
            {
                lastError = httpEx.RawDetails;
                if (IsModelSelectionError(httpEx))
                    continue;

                throw ConvertHttpError(httpEx);
            }
        }

        throw new GroqClientException("לא נמצא מודל צ'אט זמין בחשבון Groq. יש לעדכן את GroqChatModel או GroqChatFallbackModels.", lastError);
    }

    private string TranscribeAudio(byte[] audioBytes, string fileExtension)
    {
        EnsureReadyForApiCall();

        if (audioBytes == null || audioBytes.Length < 1000)
            throw new GroqClientException("ההקלטה ריקה או קצרה מדי.");

        string normalizedExt = NormalizeExtension(fileExtension);
        string mimeType = normalizedExt == "mp4" ? "audio/mp4" : "audio/webm";
        string fileName = "trip_voice." + normalizedExt;
        string boundary = "----GroqBoundary" + DateTime.UtcNow.Ticks.ToString(CultureInfo.InvariantCulture);

        byte[] bodyBytes = BuildMultipartBody(boundary, audioBytes, fileName, mimeType);
        string responseText = SendMultipartRequest(baseUrl + "/audio/transcriptions", bodyBytes, boundary);

        JObject parsed = JObject.Parse(responseText);
        string text = (string)parsed["text"];
        if (string.IsNullOrWhiteSpace(text))
            throw new GroqClientException("לא זוהה דיבור ברור בהקלטה.");

        return text.Trim();
    }

    private void EnsureReadyForApiCall()
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            throw new GroqClientException("תצורת שירות ה-AI אינה זמינה כרגע.");

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    }

    private string SendJsonRequest(string url, string payload)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(payload);
        HttpWebRequest request = CreateRequest(url, "application/json");
        request.ContentLength = bytes.Length;
        using (Stream requestStream = request.GetRequestStream())
            requestStream.Write(bytes, 0, bytes.Length);

        return ReadResponse(request);
    }

    private string SendMultipartRequest(string url, byte[] bodyBytes, string boundary)
    {
        HttpWebRequest request = CreateRequest(url, "multipart/form-data; boundary=" + boundary);
        request.ContentLength = bodyBytes.Length;
        using (Stream requestStream = request.GetRequestStream())
            requestStream.Write(bodyBytes, 0, bodyBytes.Length);

        return ReadResponse(request);
    }

    private HttpWebRequest CreateRequest(string url, string contentType)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = contentType;
        request.Timeout = timeoutMilliseconds;
        request.ReadWriteTimeout = timeoutMilliseconds;
        request.KeepAlive = false;
        request.ServicePoint.Expect100Continue = false;
        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        request.Headers["Authorization"] = "Bearer " + apiKey;
        request.UserAgent = "CarRentalShop/1.0";
        return request;
    }

    private static string ReadResponse(HttpWebRequest request)
    {
        try
        {
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream ?? Stream.Null, Encoding.UTF8))
                return reader.ReadToEnd();
        }
        catch (WebException webEx)
        {
            string details = string.Empty;
            HttpStatusCode? statusCode = null;
            if (webEx.Response != null)
            {
                var httpResponse = webEx.Response as HttpWebResponse;
                if (httpResponse != null)
                    statusCode = httpResponse.StatusCode;

                using (var stream = webEx.Response.GetResponseStream())
                using (var reader = new StreamReader(stream ?? Stream.Null, Encoding.UTF8))
                    details = reader.ReadToEnd();
            }

            throw new GroqHttpException(webEx.Status, statusCode, details, webEx);
        }
    }

    private List<string> BuildModelsToTry()
    {
        var models = new List<string>();
        AddModelIfMissing(models, chatModel);

        if (!string.IsNullOrWhiteSpace(chatFallbackModelsCsv))
        {
            string[] fallbackModels = chatFallbackModelsCsv.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string candidate in fallbackModels)
                AddModelIfMissing(models, candidate);
        }

        return models;
    }

    private static void AddModelIfMissing(List<string> models, string model)
    {
        string normalized = (model ?? string.Empty).Trim();
        if (normalized.Length == 0)
            return;

        foreach (string existing in models)
            if (string.Equals(existing, normalized, StringComparison.OrdinalIgnoreCase))
                return;

        models.Add(normalized);
    }

    private static bool IsModelSelectionError(GroqHttpException ex)
    {
        if (ex == null)
            return false;

        if (ex.StatusCode != HttpStatusCode.BadRequest && ex.StatusCode != HttpStatusCode.NotFound && (int)(ex.StatusCode ?? 0) != 422)
            return false;

        string details = (ex.RawDetails ?? string.Empty).ToLowerInvariant();
        return details.Contains("model") || details.Contains("not found") || details.Contains("does not exist") || details.Contains("decommission") || details.Contains("unsupported");
    }

    private static GroqClientException ConvertHttpError(GroqHttpException ex)
    {
        if (ex == null)
            return new GroqClientException("שירות ה-AI אינו זמין כרגע. נסו שוב בעוד כמה דקות.");

        if (ex.WebStatus == WebExceptionStatus.Timeout)
            return new GroqClientException("שירות ההמלצות לא הגיב בזמן. נסו שוב בעוד רגע.", ex.RawDetails);

        if (ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.Forbidden)
            return new GroqClientException("מפתח ה-API של Groq לא תקין או חסרה הרשאה בחשבון.", ex.RawDetails);

        if ((int)(ex.StatusCode ?? 0) == 429)
            return new GroqClientException("הגעתם למגבלת בקשות מול Groq. נסו שוב בעוד כמה דקות.", ex.RawDetails);

        return new GroqClientException("שירות ה-AI אינו זמין כרגע. נסו שוב בעוד כמה דקות.", ex.RawDetails);
    }

    private byte[] BuildMultipartBody(string boundary, byte[] audioBytes, string fileName, string mimeType)
    {
        string lineBreak = "\r\n";
        byte[] boundaryBytes = Encoding.UTF8.GetBytes("--" + boundary + lineBreak);

        using (var stream = new MemoryStream())
        {
            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WriteString(stream, "Content-Disposition: form-data; name=\"file\"; filename=\"" + fileName + "\"" + lineBreak);
            WriteString(stream, "Content-Type: " + mimeType + lineBreak + lineBreak);
            stream.Write(audioBytes, 0, audioBytes.Length);
            WriteString(stream, lineBreak);

            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WriteString(stream, "Content-Disposition: form-data; name=\"model\"" + lineBreak + lineBreak);
            WriteString(stream, whisperModel + lineBreak);

            stream.Write(boundaryBytes, 0, boundaryBytes.Length);
            WriteString(stream, "Content-Disposition: form-data; name=\"language\"" + lineBreak + lineBreak);
            WriteString(stream, "he" + lineBreak);

            WriteString(stream, "--" + boundary + "--" + lineBreak);
            return stream.ToArray();
        }
    }

    private static void WriteString(Stream stream, string value)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        stream.Write(bytes, 0, bytes.Length);
    }

    private static string BuildSystemPrompt()
    {
        return "אתה יועץ השכרת רכבים. בחר רכב אחד בלבד מתוך רשימת הרכבים שקיבלת. " +
               "אסור לבחור מזהה שלא הופיע ברשימה. אסור להמציא מפרט שלא קיים בקלט. " +
               "אם חסר מידע מהותי, ציין זאת. החזר תשובה בפורמט JSON בלבד: " +
               "{\"carId\":123,\"reason\":\"...\",\"missingInfo\":\"...\"}. " +
               "השדה reason חייב להיות בעברית.";
    }

    private static string BuildUserPrompt(string tripDescription, ICollection<CarRecommendationCandidate> availableCars)
    {
        JArray carsArray = new JArray();
        foreach (CarRecommendationCandidate car in availableCars)
        {
            JObject carObject = new JObject();
            carObject["id"] = car.CarId;
            carObject["manufacturer"] = car.Manufacturer;
            carObject["model"] = car.Model;
            carObject["pricePerDay"] = car.PricePerDay;
            carObject["category"] = car.Category;
            carObject["seats"] = car.Seats.HasValue ? (JToken)car.Seats.Value : JValue.CreateNull();
            carObject["luggageCapacity"] = car.LuggageCapacity.HasValue ? (JToken)car.LuggageCapacity.Value : JValue.CreateNull();
            carObject["fuelType"] = car.FuelType;
            carObject["transmission"] = car.Transmission;
            carObject["rangeKm"] = car.RangeKm.HasValue ? (JToken)car.RangeKm.Value : JValue.CreateNull();
            carObject["year"] = car.Year > 0 ? (JToken)car.Year : JValue.CreateNull();
            carObject["stock"] = car.Stock;
            carObject["description"] = car.Description;
            carsArray.Add(carObject);
        }

        JObject body = new JObject();
        body["tripDescription"] = tripDescription;
        body["availableCars"] = carsArray;
        return body.ToString();
    }

    private static string ParseChatMessageContent(string responseText)
    {
        JObject parsed = JObject.Parse(responseText);
        JToken contentToken = parsed.SelectToken("choices[0].message.content");
        string content = contentToken == null ? string.Empty : contentToken.ToString();
        if (string.IsNullOrWhiteSpace(content))
            throw new GroqClientException("לא התקבל תוכן מהשירות.");
        return content.Trim();
    }

    private static GroqRecommendationResponse ParseRecommendationJson(string content)
    {
        JObject parsed;
        try
        {
            parsed = JObject.Parse(content);
        }
        catch
        {
            int start = content.IndexOf('{');
            int end = content.LastIndexOf('}');
            if (start < 0 || end <= start)
                throw new GroqClientException("פורמט תשובת ההמלצה אינו תקין.");
            parsed = JObject.Parse(content.Substring(start, end - start + 1));
        }

        return new GroqRecommendationResponse
        {
            CarId = (int?)parsed["carId"] ?? 0,
            Reason = ((string)parsed["reason"] ?? string.Empty).Trim(),
            MissingInfo = ((string)parsed["missingInfo"] ?? string.Empty).Trim()
        };
    }

    private static string NormalizeExtension(string fileExtension)
    {
        string ext = (fileExtension ?? string.Empty).Trim().TrimStart('.').ToLowerInvariant();
        if (ext == "mp4" || ext == "m4a")
            return "mp4";
        return "webm";
    }
}

public sealed class CarRecommendationCandidate
{
    public int CarId { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public decimal PricePerDay { get; set; }
    public string Category { get; set; }
    public int? Seats { get; set; }
    public int? LuggageCapacity { get; set; }
    public string FuelType { get; set; }
    public string Transmission { get; set; }
    public int? RangeKm { get; set; }
    public int Year { get; set; }
    public int Stock { get; set; }
    public string Description { get; set; }
}

public sealed class GroqRecommendationResponse
{
    public int CarId { get; set; }
    public string Reason { get; set; }
    public string MissingInfo { get; set; }
}

public sealed class GroqClientException : Exception
{
    public string RawDetails { get; private set; }

    public GroqClientException(string message)
        : base(message)
    {
    }

    public GroqClientException(string message, string rawDetails)
        : base(message)
    {
        RawDetails = rawDetails;
    }
}

public sealed class GroqHttpException : Exception
{
    public WebExceptionStatus WebStatus { get; private set; }
    public HttpStatusCode? StatusCode { get; private set; }
    public string RawDetails { get; private set; }

    public GroqHttpException(WebExceptionStatus webStatus, HttpStatusCode? statusCode, string rawDetails, Exception innerException)
        : base("Groq HTTP request failed.", innerException)
    {
        WebStatus = webStatus;
        StatusCode = statusCode;
        RawDetails = rawDetails;
    }
}
