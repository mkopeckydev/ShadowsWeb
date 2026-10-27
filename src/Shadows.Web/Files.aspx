<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Files.aspx.cs" Inherits="Files" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <h1>
                <asp:Label ID="lblCaption" runat="server" Text="Soubory ke stažení"></asp:Label></h1>
        </div>
        <asp:Panel ID="pnlUpload" runat="server" CssClass="mb-1 mx-auto text-start panel-control">
            <table class="w-100 form-table">
                <tr>
                    <td>
                        <asp:FileUpload ID="fuUpload" runat="server" CssClass="form-control text-start w-100" />
                    </td>
                </tr>
                <tr>
                    <td class="text-center">
                        <asp:Button ID="btnUpload" runat="server" Text="Nahrát na server" OnClick="Upload_Click" CssClass="btn btn-primary" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div class="app-panel">

                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Název souboru</th>
                            <th></th>
                        </tr>
                    </thead>

                    <asp:Repeater ID="rptItems" runat="server" OnItemCommand="Items_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HyperLink ID="hlFileName" runat="server" Target="_blank" NavigateUrl='<%# Eval("FileUrl") %>'><%# Eval("FileName")%></asp:HyperLink>
                                </td>
                                <td class="text-end">
                                    <asp:LinkButton ID="btnVisibility" runat="server" Text='<%# Eval("VisibilityLabel") %>' ToolTip="Viditelnost souboru" CommandName="VisibilityRow" Visible='<%# Eval("IsAdmin") %>' CommandArgument='<%# Eval("IdFileData") %>' />
                                    <asp:LinkButton ID="btnDeleteRow" runat="server" Text="Smazat" ToolTip="Smazat" CommandName="DeleteRow" Visible='<%# Eval("IsAdmin") %>' CommandArgument='<%# Eval("FileName") %>' OnClientClick="return ConfirmDelete();" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>

            <act:ModalPopupExtender ID="detailPopup" runat="server" BackgroundCssClass="popupBackground" DropShadow="true"
                PopupControlID="pnlVisibility" CancelControlID="btnCancel" TargetControlID="btnSave" />

            <asp:Panel CssClass="popupWindow" ID="pnlVisibility" runat="server" DefaultButton="btnSave" Width="250px">
                <table class="w-100 form-table">
                    <tr>
                        <td>
                            <asp:Label ID="lblDetailCaption" runat="server" CssClass="caption" Text="Viditelnost souboru"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlVisibilityCharacters" runat="server" Height="500px" ScrollBars="Vertical" CssClass="w-100 border text-start p-1 rounded">
                                <asp:CheckBoxList ID="chbVisibilityCharacters" runat="server" DataTextField="Name" DataValueField="Id" CssClass="w-100 chb"></asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="text-end">
                            <asp:Button ID="btnSave" runat="server" Text="Uložit" UseSubmitBehavior="false" OnClick="Save_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCancel" runat="server" Text="Zavřít" CssClass="btn btn-secondary" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
