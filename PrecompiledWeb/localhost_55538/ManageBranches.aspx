<%@ page title="ניהול סניפים" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="ManageBranches, App_Web_vkkzr0a4" %>

<asp:Content ID="BranchesContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">ניהול סניפים</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <div class="card bg-dark text-white border-secondary p-3 mb-4">
            <h2 class="h5">הוספת סניף</h2>
            <div class="row g-2">
                <div class="col-md-3"><label for="txtName">שם</label><asp:TextBox ID="txtName" runat="server" CssClass="form-control" /></div>
                <div class="col-md-3"><label for="txtCity">עיר</label><asp:TextBox ID="txtCity" runat="server" CssClass="form-control" /></div>
                <div class="col-md-3"><label for="txtAddress">כתובת</label><asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" /></div>
                <div class="col-md-3"><label for="txtPhone">טלפון</label><asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" /></div>
            </div>
            <asp:Button ID="btnAdd" runat="server" Text="הוספת סניף" CssClass="btn btn-primary mt-3" OnClick="btnAdd_Click" />
        </div>
        <asp:Repeater ID="rptBranches" runat="server" OnItemCommand="rptBranches_ItemCommand">
            <ItemTemplate>
                <div class="card bg-dark text-white border-secondary p-3 mb-2 d-flex flex-row justify-content-between align-items-center">
                    <div><strong><%#: Eval("MyBranchName") %></strong> — <%#: Eval("MyCity") %><div class="small text-secondary"><%#: Eval("MyAddress") %> · <%#: Eval("MyPhone") %></div></div>
                    <asp:LinkButton runat="server" CssClass="btn btn-outline-danger" CommandName="DeleteBranch" CommandArgument='<%# Eval("MyBranchID") %>' OnClientClick="return confirm('למחוק את הסניף? הרכבים יישארו ללא שיוך.');">מחיקה</asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </main>
</asp:Content>
