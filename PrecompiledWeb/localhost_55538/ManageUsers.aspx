<%@ page title="ניהול משתמשים" language="C#" masterpagefile="~/MasterPageVisitor.master" autoeventwireup="true" inherits="ManageUsers, App_Web_pskrdzto" %>

<asp:Content ID="UsersContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">ניהול משתמשים</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand" OnItemDataBound="rptUsers_ItemDataBound">
            <ItemTemplate>
                <article class="card bg-dark text-white border-secondary mb-2 p-3">
                    <div class="d-flex justify-content-between align-items-center flex-wrap gap-2">
                        <div><strong><%#: Eval("MyUsername") %></strong> — <%#: Eval("MyFirstName") %> <%#: Eval("MyLastName") %><div class="small text-secondary"><%#: Eval("MyEmail") %> · <%#: Eval("MyPhoneNumber") %></div></div>
                        <div class="d-flex gap-2">
                            <asp:LinkButton ID="btnRole" runat="server" CssClass="btn btn-outline-info" CommandName="ToggleAdmin" CommandArgument='<%# Eval("MyUsername") %>' />
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-outline-danger" CommandName="DeleteUser" CommandArgument='<%# Eval("MyUsername") %>' OnClientClick="return confirm('למחוק את המשתמש?');">מחיקת משתמש</asp:LinkButton>
                        </div>
                    </div>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </main>
</asp:Content>
