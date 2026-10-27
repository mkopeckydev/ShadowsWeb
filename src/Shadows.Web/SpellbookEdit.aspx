<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SpellbookEdit.aspx.cs" Inherits="SpellbookEdit" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <h1>
                <asp:Label ID="lblCaption" runat="server"></asp:Label></h1>
        </div>
        <div class="mb-1 mx-auto text-start panel-control">
            <table class="w-100 form-table">
                <tr>
                    <td><b>Název kouzla</b></td>
                    <td>
                        <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control" MaxLength="128"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><b>Skupina kouzel</b></td>
                    <td>
                        <asp:TextBox ID="txtSpellGroup" runat="server" CssClass="form-control" MaxLength="128"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><b>Stupeň náročnosti</b></td>
                    <td>
                        <asp:TextBox ID="txtSpellLevel" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" class="text-center">
                        <asp:Button ID="btnSave" runat="server" Text="Uložit" OnClick="Save_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="btnClose" runat="server" Text="Zavřít" OnClick="Close_Click" CssClass="btn btn-secondary" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlDetail" runat="server" CssClass="app-panel">
        <div class="mb-1">
            <asp:Label ID="lblAttributes" runat="server" Text="Parametry kouzla" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1">
            <asp:TextBox ID="txtAttributes" runat="server" TextMode="MultiLine" Rows="12" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblDescription" runat="server" Text="Účinky kouzla" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblModification" runat="server" Text="Modifikace kouzla" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1">
            <asp:TextBox ID="txtModification" runat="server" TextMode="MultiLine" Rows="12" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblModificationRuneMage" runat="server" Text="Runový mág (modifikace za runu síly)" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1">
            <asp:TextBox ID="txtModificationRuneMage" runat="server" TextMode="MultiLine" Rows="6" CssClass="form-control"></asp:TextBox>
        </div>
        
    </asp:Panel>

</asp:Content>
