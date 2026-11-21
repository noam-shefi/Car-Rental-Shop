<%@ page title="ההזמנות שלי" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="MyOrders, App_Web_vkkzr0a4" %>

<asp:Content ID="OrdersContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">ההזמנות שלי</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="alert alert-secondary">עדיין לא שלחת בקשות להשכרת רכב.</asp:Panel>
        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand" OnItemDataBound="rptOrders_ItemDataBound">
            <ItemTemplate>
                <article class="card bg-dark text-white border-secondary mb-3 p-3">
                    <div class="row align-items-center g-3">
                        <div class="col-md-8">
                            <h2 class="h5"><%#: Eval("MyCarName") %></h2>
                            <div>תאריכים: <%# Eval("MyRentalStartDate", "{0:dd/MM/yyyy}") %> – <%# Eval("MyRentalEndDate", "{0:dd/MM/yyyy}") %></div>
                            <div>סה״כ: ₪<%# Eval("MyTotalPrice", "{0:0.00}") %></div>
                            <div>סטטוס: <%# GetStatus(Eval("MyRentalStatus")) %></div>
                        </div>
                        <div class="col-md-4 d-flex gap-2 flex-wrap">
                            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-outline-warning" CommandName="CancelRental" CommandArgument='<%# Eval("MyOrderID") %>'>ביטול / בקשת ביטול</asp:LinkButton>
                            <asp:HyperLink ID="lnkReview" runat="server" CssClass="btn btn-outline-info" Visible="false">כתיבת ביקורת</asp:HyperLink>
                        </div>
                    </div>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </main>
</asp:Content>
