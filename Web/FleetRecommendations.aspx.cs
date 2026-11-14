using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;

public partial class FleetRecommendations : Page
{
    private const string TrackingCookieName = "TrackingId";
    private const string DefaultTrackingId = "browse-home";
    private static readonly DataTable RecommendationGroups = CreateRecommendationGroups();
    private static readonly Regex TrainingCookie = new Regex(
        @"\A(?:browse-home|route-48a|route-93c)(?:' AND [0-9]{1,3}=[0-9]{1,3} AND 'a'='a)?\z",
        RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    private string TrainingMode
    {
        get { return ConfigurationManager.AppSettings["FleetRecommendationTrainingMode"] ?? "disabled"; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (TrainingMode != "disabled" && TrainingMode != "unsafe" && TrainingMode != "secure")
            throw new ConfigurationErrorsException("Invalid FleetRecommendationTrainingMode.");
        if (TrainingMode != "disabled" && !Request.IsLocal)
            throw new HttpException(404, "Not found");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.Cookies[TrackingCookieName] == null)
        {
            SetTrackingCookie(DefaultTrackingId);
        }

        ShowRecommendation(Request.Cookies[TrackingCookieName] == null
            ? DefaultTrackingId
            : Request.Cookies[TrackingCookieName].Value);
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        SetTrackingCookie(DefaultTrackingId);
        ShowRecommendation(DefaultTrackingId);
    }

    protected void btnClearPreferences_Click(object sender, EventArgs e)
    {
        HttpCookie expiredCookie = new HttpCookie(TrackingCookieName, string.Empty)
        {
            Expires = DateTime.UtcNow.AddYears(-1),
            HttpOnly = true,
            Secure = Request.IsSecureConnection,
            Path = "/"
        };
        expiredCookie.SameSite = SameSiteMode.Lax;
        Response.Cookies.Set(expiredCookie);
        ShowRecommendation(DefaultTrackingId);
    }

    private void ShowRecommendation(string trackingId)
    {
        bool hasSeasonalPick;
        if (TrainingMode == "disabled")
        {
            DataRow record = RecommendationGroups.Rows.Find(trackingId)
                ?? RecommendationGroups.Rows.Find(DefaultTrackingId);
            if (record == null)
                throw new InvalidOperationException("The recommendation groups have not been initialized.");
            hasSeasonalPick = Convert.ToBoolean(record["HasSeasonalPick"]);
        }
        else
        {
            hasSeasonalPick = FindTrainingRecommendation(trackingId);
        }

        litRecommendation.Text = hasSeasonalPick
            ? "Planning a weekend away? Explore our compact cars for an easy city trip."
            : "Taking the scenic route? Explore our spacious vehicles for extra room on the way.";
    }

    private bool FindTrainingRecommendation(string trackingId)
    {
        // Restrict the exercise to numeric comparisons; ACE can otherwise access external databases.
        if (trackingId == null || !TrainingCookie.IsMatch(trackingId))
            return false;

        string databasePath = Server.MapPath("~/App_Data/RecommendationTraining.accdb");
        if (!File.Exists(databasePath))
            throw new InvalidOperationException("The recommendation training database has not been created.");

        var builder = new OleDbConnectionStringBuilder();
        builder.Provider = "Microsoft.ACE.OLEDB.12.0";
        builder.DataSource = databasePath;
        builder["Mode"] = "Read";
        using (var connection = new OleDbConnection(builder.ConnectionString))
        using (var command = connection.CreateCommand())
        {
            const string prefix = "SELECT COUNT(*) FROM [RecommendationGroups] WHERE [HasSeasonalPick]=True AND [TrackingId]=";
            if (TrainingMode == "unsafe")
                command.CommandText = prefix + "'" + trackingId + "'";
            else
            {
                command.CommandText = prefix + "?";
                command.Parameters.Add("?", OleDbType.VarWChar, 100).Value = trackingId;
            }
            connection.Open();
            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
    }

    private void SetTrackingCookie(string value)
    {
        HttpCookie cookie = new HttpCookie(TrackingCookieName, value)
        {
            HttpOnly = true,
            Secure = Request.IsSecureConnection,
            Path = "/"
        };
        cookie.SameSite = SameSiteMode.Lax;
        Response.Cookies.Set(cookie);
    }

    private static DataTable CreateRecommendationGroups()
    {
        DataTable records = new DataTable("RecommendationGroups");
        DataColumn trackingIdColumn = records.Columns.Add("TrackingId", typeof(string));
        records.PrimaryKey = new[] { trackingIdColumn };
        records.Columns.Add("HasSeasonalPick", typeof(bool));
        records.Rows.Add("route-48a", true);
        records.Rows.Add("route-93c", false);
        records.Rows.Add(DefaultTrackingId, false);
        return records;
    }
}
