<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserTools.aspx.cs" Inherits="UserTools" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlUploadCopy" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <h1>
                <asp:Label ID="lblCaption" runat="server" Text="Kopie deníku"></asp:Label></h1>
        </div>
        <div class="mb-1">
            <asp:Label ID="lblUploadCopy" runat="server" Text="Vyber starou verzi deníku a potvrď" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1 panel-control mx-auto">
            <asp:FileUpload ID="fuUploadCopy" runat="server" CssClass="form-control text-start w-100" />
        </div>
        <div class="mb-1">
            <asp:Button ID="btnUploadCopy" runat="server" Text="Načíst deník" OnClick="btnUploadCopy_Click" CssClass="btn btn-primary" />
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlLogCopy" runat="server" CssClass="app-panel text-center">
        <asp:Label ID="lblLogCopy" runat="server" Text=""></asp:Label>
    </asp:Panel>

    <asp:Panel ID="pnlProcessCopy" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <asp:Label ID="lblProcessCopy" runat="server" Text="Vyber novou verzi verzi deníku a potvrď" CssClass="sub-caption"></asp:Label>
        </div>
        <div class="mb-1 panel-control mx-auto">
            <asp:FileUpload ID="fuProcessCopy" runat="server" CssClass="form-control text-start w-100" />
        </div>
        <div class="mb-1">
            <asp:Button ID="btnProcessCopy" runat="server" Text="Vyplnit deník" OnClick="btnProcessCopy_Click" CssClass="btn btn-primary" />
        </div>
    </asp:Panel>

</asp:Content>
