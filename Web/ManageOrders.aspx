<%@ Page Title="ניהול הזמנות" Language="C#" MasterPageFile="~/MasterPageVisitor.master" AutoEventWireup="true" CodeFile="ManageOrders.aspx.cs" Inherits="ManageOrders" %>

<asp:Content ID="ManageOrdersContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">ניהול בקשות השכרה</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <asp:Repeater ID="rptOrders" runat="server" OnItemCommand="rptOrders_ItemCommand" OnItemDataBound="rptOrders_ItemDataBound">
            <ItemTemplate>
                <article class="card bg-dark text-white border-secondary mb-3 p-3">
                    <h2 class="h5"><%#: Eval("MyCarName") %> — <%#: Eval("MyUsername") %></h2>
                    <div>תאריכים: <%# Eval("MyRentalStartDate", "{0:dd/MM/yyyy}") %> – <%# Eval("MyRentalEndDate", "{0:dd/MM/yyyy}") %></div>
                    <div>סה״כ: ₪<%# Eval("MyTotalPrice", "{0:0.00}") %> | סטטוס: <%# GetStatus(Eval("MyRentalStatus")) %></div>
                    <div class="d-flex gap-2 flex-wrap mt-3">
                        <asp:LinkButton ID="btnApprove" runat="server" CssClass="btn btn-success" CommandName="Approve" CommandArgument='<%# Eval("MyOrderID") %>'>אישור</asp:LinkButton>
                        <asp:LinkButton ID="btnReject" runat="server" CssClass="btn btn-outline-danger" CommandName="Reject" CommandArgument='<%# Eval("MyOrderID") %>'>דחייה</asp:LinkButton>
                        <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-outline-warning" CommandName="Cancel" CommandArgument='<%# Eval("MyOrderID") %>'>ביטול הזמנה</asp:LinkButton>
                        <asp:LinkButton ID="btnApproveCancellation" runat="server" CssClass="btn btn-warning" CommandName="ApproveCancellation" CommandArgument='<%# Eval("MyOrderID") %>'>אישור בקשת ביטול</asp:LinkButton>
                        <asp:LinkButton ID="btnRejectCancellation" runat="server" CssClass="btn btn-outline-light" CommandName="RejectCancellation" CommandArgument='<%# Eval("MyOrderID") %>'>דחיית בקשת ביטול</asp:LinkButton>
                    </div>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </main>
</asp:Content>
