<%@ page title="המועדפים שלי" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="MyFavorites, App_Web_mis1waoc" %>

<asp:Content ID="FavoritesContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">הרכבים המועדפים שלי</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="alert alert-secondary">עדיין לא שמרתם רכבים במועדפים.</asp:Panel>
        <div class="row g-4">
            <asp:Repeater ID="rptFavorites" runat="server" OnItemCommand="rptFavorites_ItemCommand">
                <ItemTemplate>
                    <div class="col-md-6 col-xl-4">
                        <article class="card h-100 bg-dark text-white border-secondary">
                            <img src='<%# GetImageUrl(Eval("MyPicture")) %>' class="card-img-top p-3" style="height:220px;object-fit:contain" alt="תמונת הרכב" />
                            <div class="card-body">
                                <h2 class="h5"><%#: Eval("MyManufacturer") %> <%#: Eval("MyModelName") %></h2>
                                <p>₪<%#: Eval("MyPrice", "{0:0.##}") %> ליום</p>
                                <p class="small text-secondary">נוסף למועדפים: <%# Eval("MyFavoriteDateAdded", "{0:dd/MM/yyyy}") %></p>
                                <div class="d-flex gap-2">
                                    <a class="btn btn-outline-light" href='<%# "CarDetails.aspx?id=" + Eval("MyCarID") %>'>פרטים והזמנה</a>
                                    <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-outline-danger" CommandName="RemoveFavorite" CommandArgument='<%# Eval("MyCarID") %>'>הסרה</asp:LinkButton>
                                </div>
                            </div>
                        </article>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </main>
</asp:Content>
