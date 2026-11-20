<%@ page title="עדכון פרופיל" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="UpdateProfile, App_Web_mis1waoc" %>

<asp:Content ID="ProfileContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">עדכון פרופיל</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3" />
        <div class="card bg-dark text-white border-secondary p-4" style="max-width:640px">
            <label for="txtFirstName">שם פרטי</label><asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control mb-3" />
            <label for="txtLastName">שם משפחה</label><asp:TextBox ID="txtLastName" runat="server" CssClass="form-control mb-3" />
            <label for="txtEmail">דוא״ל</label><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-3" TextMode="Email" />
            <label for="txtPhone">טלפון</label><asp:TextBox ID="txtPhone" runat="server" CssClass="form-control mb-3" />
            <asp:Button ID="btnSave" runat="server" Text="שמירת פרטים" CssClass="btn btn-primary" OnClick="btnSave_Click" />
        </div>
    </main>
</asp:Content>
