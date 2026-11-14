<%@ Page Title="התאמת רכב לטיול" Language="C#" MasterPageFile="~/MasterPageVisitor.master" AutoEventWireup="true" CodeFile="CarMatch.aspx.cs" Inherits="CarMatch" %>

<asp:Content ID="MatcherContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">איזה רכב מתאים לטיול שלכם?</h1>
        <p class="text-secondary">תארו את הנסיעה, מספר הנוסעים והציוד. ההתאמה מבוססת על מאפייני הרכבים והמלאי הרשום ואינה מבטיחה זמינות לתאריכים מסוימים.</p>
        <div class="card bg-dark text-white border-secondary p-4">
            <label for="txtTrip">תיאור הטיול</label>
            <asp:TextBox ID="txtTrip" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ClientIDMode="Static" />
            <div class="d-flex gap-2 mt-3">
                <button id="btnRecord" type="button" class="btn btn-outline-light">הקלטת תיאור בקול</button>
                <asp:Button ID="btnRecommend" runat="server" Text="המליצו לי על רכב" CssClass="btn btn-primary" OnClick="btnRecommend_Click" />
            </div>
            <span id="speechStatus" class="small text-secondary mt-2"></span>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mt-3" />
        <asp:Panel ID="pnlRecommendation" runat="server" Visible="false" CssClass="card bg-dark text-white border-info p-4 mt-4">
            <h2 class="h4">ההמלצה שלנו: <asp:Literal ID="litCar" runat="server" /></h2>
            <p><asp:Literal ID="litReason" runat="server" /></p>
            <asp:HyperLink ID="lnkCar" runat="server" CssClass="btn btn-outline-info align-self-start">לפרטי הרכב</asp:HyperLink>
        </asp:Panel>
    </main>
    <script>
        (function () {
            var button = document.getElementById('btnRecord');
            var status = document.getElementById('speechStatus');
            var Recognition = window.SpeechRecognition || window.webkitSpeechRecognition;
            if (!Recognition) {
                button.disabled = true;
                status.textContent = 'הדפדפן אינו תומך בתמלול קולי.';
                return;
            }
            var recognition = new Recognition();
            recognition.lang = 'he-IL';
            recognition.interimResults = false;
            button.addEventListener('click', function () {
                if (button.dataset.recording === 'true') {
                    recognition.stop();
                    button.dataset.recording = 'false';
                    button.textContent = 'הקלטת תיאור בקול';
                } else {
                    recognition.start();
                    button.dataset.recording = 'true';
                    button.textContent = 'עצירת הקלטה';
                    status.textContent = 'ממתינים להרשאת מיקרופון ולדיבור…';
                }
            });
            recognition.onresult = function (event) {
                document.getElementById('txtTrip').value = event.results[0][0].transcript;
                status.textContent = 'התמלול הועתק לשדה התיאור.';
            };
            recognition.onerror = function (event) {
                status.textContent = 'לא ניתן לתמלל: ' + event.error;
                button.dataset.recording = 'false';
                button.textContent = 'הקלטת תיאור בקול';
            };
            recognition.onend = function () {
                button.dataset.recording = 'false';
                button.textContent = 'הקלטת תיאור בקול';
            };
        }());
    </script>
</asp:Content>
