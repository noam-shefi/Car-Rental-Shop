<%@ Page Title="פרטי רכב" Language="C#" MasterPageFile="~/MasterPageVisitor.master" AutoEventWireup="true" CodeFile="CarDetails.aspx.cs" Inherits="CarDetails" %>

<asp:Content ID="CarDetailsContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="bookingScriptManager" runat="server" />
    <main class="container py-4" dir="rtl">
        <asp:UpdatePanel ID="bookingUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
        <asp:Panel ID="pnlNotFound" runat="server" Visible="false" CssClass="alert alert-warning">הרכב המבוקש לא נמצא.</asp:Panel>
        <asp:Panel ID="pnlDetails" runat="server" Visible="false">
            <div class="row g-4">
                <div class="col-lg-6"><asp:Image ID="imgCar" runat="server" CssClass="img-fluid rounded bg-dark p-3 w-100" Style="height:360px;object-fit:contain" AlternateText="תמונת הרכב" /></div>
                <div class="col-lg-6 text-white">
                    <h1><asp:Literal ID="litTitle" runat="server" /></h1>
                    <p class="text-secondary car-description"><asp:Literal ID="litDescription" runat="server" /></p>
                    <div class="fs-4 fw-bold">₪<asp:Literal ID="litPrice" runat="server" /> <small class="fs-6">ליום</small></div>
                    <p class="mt-2">דירוג: <asp:Literal ID="litRating" runat="server" /> (<asp:Literal ID="litReviewCount" runat="server" /> ביקורות)</p>
                    <dl class="row car-specifications">
                        <dt class="col-4">פרטי הרכב</dt><dd class="col-8">שנה: <asp:Literal ID="litYear" runat="server" /> · קטגוריה: <asp:Literal ID="litCategory" runat="server" /></dd>
                        <dt class="col-4">נוחות ומטען</dt><dd class="col-8">מושבים: <asp:Literal ID="litSeats" runat="server" /> · מקום למטען: <asp:Literal ID="litLuggage" runat="server" /></dd>
                        <dt class="col-4">מפרט טכני</dt><dd class="col-8">דלק: <asp:Literal ID="litFuel" runat="server" /> · טווח: <asp:Literal ID="litRange" runat="server" /> · תיבת הילוכים: <asp:Literal ID="litTransmission" runat="server" /></dd>
                        <dt class="col-4">מיקום וזמינות</dt><dd class="col-8">סניף: <asp:Literal ID="litBranch" runat="server" /> · מלאי: <asp:Literal ID="litStock" runat="server" /></dd>
                    </dl>
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-warning d-block mb-2" />
                    <div class="d-flex gap-2 mb-3">
                        <asp:Button ID="btnFavorite" runat="server" Text="הוספה למועדפים" CssClass="btn btn-outline-light" OnClick="btnFavorite_Click" Visible="false" />
                        <asp:HyperLink ID="lnkLogin" runat="server" CssClass="btn btn-outline-light" Visible="false">התחברות להזמנה ולשמירה במועדפים</asp:HyperLink>
                    </div>
                    <asp:Panel ID="pnlBooking" runat="server" Visible="false" CssClass="card bg-dark border-secondary p-3">
                        <h2 class="h5">בדיקת זמינות והזמנה</h2>
                        <div class="row g-2">
                            <div class="col"><label for="txtStartDate">תאריך איסוף</label><asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="Date" /></div>
                            <div class="col"><label for="txtEndDate">תאריך החזרה</label><asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="Date" /></div>
                        </div>
                        <div class="row g-3 mt-2">
                            <div class="col-md-6"><h3 class="h6">בחירת איסוף</h3><asp:Calendar ID="calStart" runat="server" ClientIDMode="Static" OnDayRender="Calendar_DayRender" OnSelectionChanged="calStart_SelectionChanged" /></div>
                            <div class="col-md-6"><h3 class="h6">בחירת החזרה</h3><asp:Calendar ID="calEnd" runat="server" ClientIDMode="Static" OnDayRender="Calendar_DayRender" OnSelectionChanged="calEnd_SelectionChanged" /></div>
                        </div>
                        <asp:Button ID="btnCheckAvailability" runat="server" Text="בדיקת זמינות" CssClass="btn btn-primary mt-3" OnClick="btnCheckAvailability_Click" />
                        <asp:Panel ID="pnlConfirmation" runat="server" ClientIDMode="Static" Visible="false" CssClass="mt-3">
                            <p>סה״כ לתקופה: ₪<asp:Literal ID="litTotal" runat="server" />. הבקשה תישלח לאישור ותופיע כבקשה ממתינה.</p>
                            <asp:CheckBox ID="chkEmailReceipt" runat="server" Text="שליחת קבלה בדוא״ל (השירות אינו מוגדר כרגע)" />
                            <asp:Button ID="btnConfirmRental" runat="server" Text="אישור ושליחת בקשה" CssClass="btn btn-success d-block mt-2" OnClick="btnConfirmRental_Click" />
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Label ID="lblSuccess" runat="server" CssClass="text-success d-block mt-2" />
                </div>
            </div>
            <section class="mt-5">
                <h2 class="h3 text-white">ביקורות לקוחות</h2>
                <asp:Repeater ID="rptReviews" runat="server">
                    <ItemTemplate>
                        <article class="card bg-dark text-white border-secondary my-2 p-3">
                            <div class="d-flex justify-content-between"><strong><%#: Eval("MyUsername") %></strong><span>★ <%#: Eval("MyRating") %>/5</span></div>
                            <p class="mb-1"><%#: Eval("MyRev") %></p>
                            <small class="text-secondary"><%# Eval("MyReviewDate", "{0:dd/MM/yyyy}") %></small>
                        </article>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoReviews" runat="server" CssClass="text-secondary" Text="עדיין אין ביקורות לרכב זה." />
            </section>
        </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </main>
    <style>
        .rental-date-selected { outline: 2px solid #a855f7; outline-offset: -2px; }
        .car-description {
            display: -webkit-box;
            -webkit-box-orient: vertical;
            -webkit-line-clamp: 3;
            overflow: hidden;
        }
        .car-specifications dt,
        .car-specifications dd { margin-bottom: .65rem; }
    </style>
    <script>
        (function () {
            function setupCalendar(calendarId, inputId) {
                var calendar = document.getElementById(calendarId);
                var input = document.getElementById(inputId);
                if (!calendar || !input) return;

                function highlight() {
                    calendar.querySelectorAll('[data-rental-date]').forEach(function (cell) {
                        cell.classList.toggle('rental-date-selected', cell.getAttribute('data-rental-date') === input.value);
                    });
                }

                function dateChanged() {
                    var confirmation = document.getElementById('pnlConfirmation');
                    if (confirmation) confirmation.style.display = 'none';
                    highlight();
                }

                calendar.addEventListener('click', function (event) {
                    var link = event.target.closest('a');
                    var cell = link && link.closest('td[data-rental-date]');
                    if (!cell || !calendar.contains(cell)) return;
                    event.preventDefault();
                    input.value = cell.getAttribute('data-rental-date');
                    dateChanged();
                });
                input.addEventListener('change', dateChanged);
                highlight();
            }

            Sys.Application.add_load(function () {
                setupCalendar('calStart', 'txtStartDate');
                setupCalendar('calEnd', 'txtEndDate');
            });
        }());
    </script>
</asp:Content>
