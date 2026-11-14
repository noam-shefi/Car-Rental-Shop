<%@ Page Title="השכרת רכבים" Language="C#" MasterPageFile="~/MasterPageVisitor.master" AutoEventWireup="true" CodeFile="Cars.aspx.cs" Inherits="Cars" %>

<asp:Content ID="CarsContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="container py-4" dir="rtl">
        <h1 class="text-white mb-2">רכבים להשכרה</h1>
        <p class="text-secondary mb-4">חפשו רכב, השוו מחירים ובדקו זמינות לפי תאריכים.</p>
        <div class="row g-2 align-items-end mb-4">
            <div class="col-lg-3"><label class="form-label text-white" for="txtSearch">יצרן או דגם</label><asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" ClientIDMode="Static" /></div>
            <div class="col-lg-2"><label class="form-label text-white" for="ddlCategory">קטגוריה</label><asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"><asp:ListItem Text="כל הקטגוריות" Value="" /><asp:ListItem Text="סדאן" Value="sedan" /><asp:ListItem Text="ג׳יפ / SUV" Value="suv" /><asp:ListItem Text="ספורט" Value="sports" /><asp:ListItem Text="משפחתי" Value="family" /></asp:DropDownList></div>
            <div class="col-lg-2"><label class="form-label text-white" for="txtMaximumPrice">מחיר יומי מרבי</label><asp:TextBox ID="txtMaximumPrice" runat="server" CssClass="form-control" TextMode="Number" /></div>
            <div class="col-lg-2"><label class="form-label text-white" for="txtStartDate">איסוף</label><asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
            <div class="col-lg-2"><label class="form-label text-white" for="txtEndDate">החזרה</label><asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
            <div class="col-lg-1"><asp:Button ID="btnSearch" runat="server" Text="חיפוש" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" /></div>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-warning d-block mb-3" />
        <asp:Panel ID="pnlNoResults" runat="server" CssClass="alert alert-secondary" Visible="false">לא נמצאו רכבים המתאימים לחיפוש. נסו לשנות את המסננים.</asp:Panel>
        <div id="liveNoResults" class="alert alert-secondary" style="display:none" aria-live="polite">לא נמצאו רכבים המתאימים לחיפוש. נסו לשנות את החיפוש.</div>
        <div class="row g-4">
            <asp:Repeater ID="rptCars" runat="server" OnItemCommand="rptCars_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-6 col-xl-4">
                        <article class="car-card card h-100 bg-dark text-white border-secondary">
                            <a href='<%# "CarDetails.aspx?id=" + Eval("MyProductID") %>' class="text-decoration-none text-white">
                                <img src='<%# GetImageUrl(Eval("MyPicture")) %>' class="card-img-top p-3" style="height:220px;object-fit:contain" alt="תמונת רכב" />
                                <div class="card-body">
                                    <h2 class="h5 car-name"><%#: Eval("MyManufacturer") %> <%#: Eval("MyProductName") %></h2>
                                    <p class="text-secondary car-description"><%#: Eval("MyDescription") %></p>
                                </div>
                            </a>
                            <div class="card-body pt-0">
                                <div class="d-flex justify-content-between"><span><%#: CarRentalDisplay.Category(Eval("MyCarCategory")) %></span><strong>₪<%#: Eval("MyPrice", "{0:0.##}") %> ליום</strong></div>
                                <div class="small mt-2">דירוג: <%#: Eval("MyAverageRating", "{0:0.0}") %> (<%#: Eval("MyReviewCount") %> ביקורות)</div>
                                <div class="small"><%#: Eval("MyBranchCity") %></div>
                                <div class="mt-3 d-flex gap-2">
                                    <a class="btn btn-outline-light" href='<%# "CarDetails.aspx?id=" + Eval("MyProductID") %>'>פרטים והזמנה</a>
                                    <asp:LinkButton ID="btnEdit" runat="server" Visible='<%# IsAdministrator %>' CssClass="btn btn-outline-info" PostBackUrl='<%# "CarEdit.aspx?id=" + Eval("MyProductID") %>'>עריכה</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" Visible='<%# IsAdministrator %>' CssClass="btn btn-outline-danger" CommandName="DeleteCar" CommandArgument='<%# Eval("MyProductID") %>' OnClientClick="return confirm('למחוק את הרכב מהקטלוג?');">מחיקה</asp:LinkButton>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <asp:Panel ID="pnlAdmin" runat="server" Visible="false" CssClass="mt-4"><a class="btn btn-success" href="CarEdit.aspx">הוספת רכב</a></asp:Panel>
    </section>
    <style>
        .car-description {
            display: -webkit-box;
            -webkit-box-orient: vertical;
            -webkit-line-clamp: 2;
            overflow: hidden;
        }
    </style>
    <script>
        (function () {
            var box = document.getElementById('txtSearch');
            if (!box) return;
            box.addEventListener('input', function () {
                var term = box.value.trim().toLocaleLowerCase();
                var cards = document.querySelectorAll('.car-card');
                var matches = 0;
                cards.forEach(function (card) {
                    var match = card.querySelector('.car-name').textContent.toLocaleLowerCase().indexOf(term) >= 0;
                    card.parentElement.style.display = match ? '' : 'none';
                    if (match) matches++;
                });
                document.getElementById('liveNoResults').style.display = cards.length && !matches ? '' : 'none';
            });
        }());
    </script>
</asp:Content>
