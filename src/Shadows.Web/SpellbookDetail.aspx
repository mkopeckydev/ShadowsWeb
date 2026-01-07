<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SpellbookDetail.aspx.cs" Inherits="SpellbookDetail" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <h1>
                <asp:Label ID="lblCaption" runat="server"></asp:Label></h1>
        </div>
        <div class="mb-1">
            <asp:Button ID="btnClose" runat="server" Text="Zavřít" OnClick="btnClose_Click" CssClass="btn btn-primary" />
        </div>
    </asp:Panel>

    <div class="app-panel">
        <div class="mb-1">
            <asp:Label ID="lblAttributes" runat="server" Text="Parametry kouzla" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1 pb-1 border-bottom">
            <asp:Label ID="lblAttributesValue" runat="server" Text=""></asp:Label>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblDescription" runat="server" Text="Účinky kouzla" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1 pb-1 border-bottom">
            <asp:Label ID="lblDescriptionValue" runat="server" Text=""></asp:Label>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblModification" runat="server" Text="Modifikace kouzla" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1 pb-1 border-bottom">
            <asp:Label ID="lblModificationValue" runat="server" Text=""></asp:Label>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblModificationRuneMage" runat="server" Text="Runový mág (modifikace za runu síly)" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1 pb-1">
            <asp:Label ID="lblModificationRuneMageValue" runat="server" Text=""></asp:Label>
        </div>
    </div>

    <asp:UpdatePanel ID="upDetail" runat="server">
        <ContentTemplate>

            <div class="app-panel">
                <div class="fw-bold">
                    Poznámky
                </div>
                <div class="mb-2">
                    <asp:TextBox ID="txtSpellbookItemNote" runat="server" TextMode="MultiLine" Rows="4" CssClass="w-100 form-control"></asp:TextBox>
                </div>
                <div class="text-center mb-1">
                    <asp:Label ID="lblSaveSpellbookItemNote" runat="server" Text="" CssClass="err-message me-1"></asp:Label>
                    <asp:Button ID="btnSaveSpellbookItemNote" runat="server" Text="Uložit" OnClick="btnSaveSpellbookItemNote_Click" CssClass="btn btn-primary" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
