<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Detail" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <h1>
                <asp:Label ID="lblCaption" runat="server"></asp:Label></h1>
        </div>
        <asp:Panel ID="pnlImage" runat="server" CssClass="mb-1">
            <asp:Image ID="imgDetail" runat="server" CssClass="image-detail rounded" />
        </asp:Panel>
        <div class="mb-1">
            <asp:Label ID="lblValueSum" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblBonusLevel" runat="server" Text=""></asp:Label>
        </div>
        <div class="mb-1">
            <asp:Button ID="btnClose" runat="server" Text="Zavřít" OnClick="Close_Click" CssClass="btn btn-primary" />
        </div>
    </asp:Panel>

    <asp:Repeater ID="rptRows" runat="server">
        <HeaderTemplate>
            <div class="app-panel">
                <table class="table">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td width="30px" align="center"><%# Eval("InfoLevel")%></td>
                <td><%# Eval("InfoTextHtml")%></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
            </div>
        </FooterTemplate>
    </asp:Repeater>

    <asp:UpdatePanel ID="upDetail" runat="server">
        <ContentTemplate>

            <div class="app-panel">
                <div class="fw-bold">
                    Poznámky
                </div>
                <div class="mb-2">
                    <asp:TextBox ID="txtItemNote" runat="server" TextMode="MultiLine" Rows="4" CssClass="w-100 form-control"></asp:TextBox>
                </div>
                <div class="text-center mb-1">
                    <asp:Label ID="lblSaveItemNote" runat="server" Text="" CssClass="err-message me-1"></asp:Label>
                    <asp:Button ID="btnSaveItemNote" runat="server" Text="Uložit" OnClick="SaveItemNote_Click" CssClass="btn btn-primary" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
