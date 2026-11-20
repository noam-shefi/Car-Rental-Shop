<%@ page title="ניהול רכב" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="CarEdit, App_Web_mis1waoc" %>

<asp:Content ID="CarEditContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white"><asp:Literal ID="litHeading" runat="server" Text="הוספת רכב" /></h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <div class="card bg-dark text-white border-secondary p-4">
            <div class="row g-3">
                <div class="col-md-6"><label for="txtManufacturer">יצרן</label><asp:TextBox ID="txtManufacturer" runat="server" CssClass="form-control" /></div>
                <div class="col-md-6"><label for="txtModel">דגם</label><asp:TextBox ID="txtModel" runat="server" CssClass="form-control" /></div>
                <div class="col-md-3"><label for="txtYear">שנה</label><asp:TextBox ID="txtYear" runat="server" CssClass="form-control" TextMode="Number" /></div>
                <div class="col-md-3"><label for="ddlCarCategory">קטגוריה</label><asp:DropDownList ID="ddlCarCategory" runat="server" CssClass="form-select"><asp:ListItem Text="סדאן" Value="sedan" /><asp:ListItem Text="ג׳יפ / SUV" Value="suv" /><asp:ListItem Text="ספורט" Value="sports" /><asp:ListItem Text="משפחתי" Value="family" /></asp:DropDownList></div>
                <div class="col-md-3"><label for="txtPrice">מחיר יומי</label><asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" /></div>
                <div class="col-md-3"><label for="txtStock">כמות צי</label><asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" /></div>
                <div class="col-md-6"><label for="ddlBranch">סניף</label><asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-select" /></div>
                <div class="col-md-6"><label for="txtImage">תמונה (כתובת או נתוני תמונה)</label><asp:TextBox ID="txtImage" runat="server" CssClass="form-control" /></div>
                <div class="col-12"><label for="txtDescription">תיאור</label><asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" /></div>
                <div class="col-md-3"><label for="txtSeats">מושבים</label><asp:TextBox ID="txtSeats" runat="server" CssClass="form-control" TextMode="Number" /></div>
                <div class="col-md-3"><label for="txtFuel">סוג דלק</label><asp:TextBox ID="txtFuel" runat="server" CssClass="form-control" /></div>
                <div class="col-md-2"><label for="txtRange">טווח בק״מ</label><asp:TextBox ID="txtRange" runat="server" CssClass="form-control" TextMode="Number" /></div>
                <div class="col-md-2"><label for="txtTransmission">תיבת הילוכים</label><asp:TextBox ID="txtTransmission" runat="server" CssClass="form-control" /></div>
                <div class="col-md-2"><label for="txtLuggage">מקום למטען</label><asp:TextBox ID="txtLuggage" runat="server" CssClass="form-control" TextMode="Number" /></div>
            </div>
            <asp:Button ID="btnSave" runat="server" Text="שמירה" CssClass="btn btn-primary mt-4" OnClick="btnSave_Click" />
        </div>
    </main>
</asp:Content>
