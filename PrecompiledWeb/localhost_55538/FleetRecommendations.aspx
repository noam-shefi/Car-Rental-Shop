<%@ page language="C#" autoeventwireup="true" inherits="FleetRecommendations, App_Web_nq41q5ek" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Explore Our Fleet</title>
    <style>
        body { font-family: Arial, sans-serif; line-height: 1.55; margin: 2rem auto; max-width: 860px; padding: 0 1rem; color: #172033; }
        h1, h2 { color: #164e8a; }
        .card { border: 1px solid #cbd5e1; border-radius: 8px; margin: 1rem 0; padding: 1rem; }
        .recommendation { font-size: 1.1rem; }
        .actions { display: flex; gap: 1rem; align-items: center; }
        .button { padding: 0.6rem 1rem; cursor: pointer; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Find a car for your next trip</h1>
        <p>Browse our fleet and choose a vehicle that fits your plans.</p>

        <section class="card">
            <h2>A few ideas for your journey</h2>
            <p class="recommendation"><asp:Literal ID="litRecommendation" runat="server" /></p>
            <div class="actions">
                <a href="Cars.aspx">Browse available cars</a>
                <asp:Button ID="btnRefresh" runat="server" Text="Show popular picks" CssClass="button" OnClick="btnRefresh_Click" />
                <asp:Button ID="btnClearPreferences" runat="server" Text="Clear preferences" CssClass="button" OnClick="btnClearPreferences_Click" />
            </div>
        </section>
    </form>
</body>
</html>
