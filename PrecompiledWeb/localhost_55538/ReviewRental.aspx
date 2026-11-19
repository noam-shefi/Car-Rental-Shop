<%@ page title="כתיבת ביקורת" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="ReviewRental, App_Web_pskrdzto" %>

<asp:Content ID="ReviewContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">ביקורת על השכרה שהסתיימה</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <asp:Panel ID="pnlReview" runat="server" Visible="false" CssClass="card bg-dark text-white border-secondary p-4" style="max-width:640px">
            <asp:Label ID="lblCar" runat="server" CssClass="h5 d-block mb-3" />
            <label for="ddlRating">דירוג</label>
            <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-select mb-3"><asp:ListItem Text="5 — מצוין" Value="5" /><asp:ListItem Text="4 — טוב מאוד" Value="4" /><asp:ListItem Text="3 — טוב" Value="3" /><asp:ListItem Text="2 — סביר" Value="2" /><asp:ListItem Text="1 — לא טוב" Value="1" /></asp:DropDownList>
            <label for="txtComment">תגובה (לא חובה)</label>
            <asp:TextBox ID="txtComment" runat="server" CssClass="form-control mb-3" TextMode="MultiLine" Rows="4" MaxLength="2000" />
            <asp:Button ID="btnSubmit" runat="server" Text="שליחת ביקורת" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </asp:Panel>
    </main>
</asp:Content>
