<%@ Page Title="ניהול מלאי" Language="C#" MasterPageFile="~/MasterPageVisitor.master" AutoEventWireup="true" CodeFile="ManageFleet.aspx.cs" Inherits="ManageFleet" %>

<asp:Content ID="StockContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main class="container py-4" dir="rtl">
        <h1 class="text-white">ניהול כמויות צי</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block text-warning mb-3" />
        <asp:Repeater ID="rptCars" runat="server" OnItemCommand="rptCars_ItemCommand">
            <ItemTemplate>
                <div class="card bg-dark text-white border-secondary mb-2 p-3 d-flex flex-row align-items-center gap-3">
                    <div class="flex-grow-1"><%#: Eval("MyManufacturer") %> <%#: Eval("MyModelName") %></div>
                    <asp:TextBox ID="txtStock" runat="server" Text='<%# Eval("MyFleetQuantity") %>' TextMode="Number" CssClass="form-control" Style="max-width:140px" />
                    <asp:LinkButton runat="server" CommandName="SaveStock" CommandArgument='<%# Eval("MyCarID") %>' CssClass="btn btn-primary">שמירה</asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </main>
</asp:Content>
