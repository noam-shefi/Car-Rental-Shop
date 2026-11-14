# Running the web application locally

The web site needs its local WCF service, and the older checkout pages also use the local payment test service. The addresses must match the IIS Express ports:

- Web: `http://localhost:55538/`
- WCF service: `http://localhost:56829/Service1.svc`
- Payment test service: `http://localhost:53701/visa.asmx`

In Visual Studio, select **Web and local services** in the startup profile dropdown and press F5. The profile starts the WCF and payment services before the web site. If the profile is not available, enable **Multi-Project Launch Profiles** under Tools > Options > Preview Features, then reload the solution. Alternatively, use the solution's **Configure Startup Projects** dialog to start `WcfServiceHardwareStore`, `visa`, and `Web`, in that order.

The Access database and Microsoft Access Database Engine (ACE OLE DB provider) must also be available. The database path is configured in `ClassLibrary2\DBfunction.cs` and must point to the local `HardwareStore.accdb` file. IIS Express and its process architecture must be compatible with the installed provider.

If a service does not start, check whether another application is using its port and whether the corresponding URL opens. Changing a service port requires updating its IIS Express settings and all matching client addresses in `Web\Web.config` (and `WpfApp1\App.config` if using the WPF app).

## Local recommendation exercise

The recommendation page defaults to the existing in-memory behavior. The optional exercise uses `Web\App_Data\RecommendationTraining.accdb`, with a `RecommendationGroups` table containing three dummy records. If that file does not exist, run `powershell -ExecutionPolicy Bypass -File .\Web\SetupRecommendationTraining.ps1` from the solution directory, with a PowerShell architecture matching the installed ACE provider. The script refuses to overwrite an existing file. Do not copy real data into it.

Set `FleetRecommendationTrainingMode` in `Web\Web.config` to `unsafe` for the exercise, or `secure` for the parameterized comparison. Set it back to `disabled` when finished. Both training modes accept only local HTTP requests; keep the application and any proxy bound to localhost. Do not deploy the exercise. The restrictive cookie grammar is an isolation boundary, not a general-purpose SQL injection defense: it permits only known dummy identifiers and simple numeric equality conditions. ACE can otherwise query external databases, even when the current database is separate.

Open `http://localhost:55538/FleetRecommendations.aspx` in Burp's browser, send its GET request to Repeater, and edit only `TrackingId` in the Cookie header. Compare `route-48a' AND 1=1 AND 'a'='a` with `route-48a' AND 1=2 AND 'a'='a`. Keep the rest of the request unchanged and enter the values literally: this page does not URL-decode cookie values. The recommendation text, rather than a true/false indicator, differs in unsafe mode. Compare the same requests after switching to `secure`; both inputs are then literal values and produce the default recommendation. The separate `ASP.NET_SessionId` cookie is not part of this exercise. **Clear preferences** removes only `TrackingId`.

The intentionally unsafe concatenation and the fixed SQL with a positional `?` parameter are in `FleetRecommendations.aspx.cs`. The exercise never calls the main database helper. `DBfunction.DoCommandInjection` attaches the application's Access connection and executes a supplied command; despite its name, its safety depends on callers using fixed SQL and properly ordered parameters. Parameterization, not cookie filtering, is the appropriate production fix.
