<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Spellbook.aspx.cs" Inherits="Spellbook" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel">

        <div class="mb-1 text-center">
            <h1>Kniha kouzel</h1>
        </div>
        <div class="mb-1 app-float-panel">

            <div class="float-start m-1 panel-control">
                <table class="w-100 form-table">
                    <tr>
                        <td>Hledat:</td>
                        <td>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control w-100"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Stupeň:</td>
                        <td>
                            <asp:TextBox ID="txtSearchLevel" runat="server" CssClass="form-control w-100"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="float-end m-1">
                <asp:Button ID="btnSearch" runat="server" Text="Hledat" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnPrint" runat="server" Text="Tisk" OnClick="btnPrint_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnNew" runat="server" Text="Nový záznam" ToolTip="Nový" OnClick="btnNew_Click" CssClass="btn btn-primary" />
            </div>
        </div>

    </asp:Panel>

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <div class="app-panel">
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Název kouzla</th>
                            <th>Skupina kouzel</th>
                            <th>Stupeň kouzla</th>
                            <th></th>
                        </tr>
                    </thead>

                    <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand" OnItemDataBound="rptItems_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td class="align-middle">
                                    <asp:LinkButton ID="btnShowRowCaption" runat="server" Text='<%# Eval("Caption")%>' ToolTip="Zobrazit" CommandName="ShowRow" CommandArgument='<%# Eval("Id") %>' />
                                </td>
                                <td class="align-middle"><%# Eval("SpellGroup") %></td>
                                <td class="align-middle"><%# Eval("SpellLevel") %></td>
                                <td class="text-end">
                                    <asp:LinkButton ID="btnVisibility" runat="server" Text='<%# Eval("VisibilityLabel") %>' ToolTip="Viditelnost kouzla" CommandName="VisibilityRow" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:LinkButton ID="btnShowRow" runat="server" Text="Zobrazit" ToolTip="Zobrazit" CommandName="ShowRow" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:LinkButton ID="btnEditRow" runat="server" Text="Editovat" ToolTip="Editovat" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:LinkButton ID="btnDeleteRow" runat="server" Text="Smazat" ToolTip="Smazat" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' OnClientClick="return ConfirmDelete();" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
            </div>


            <act:ModalPopupExtender ID="detailPopup" runat="server" BackgroundCssClass="popupBackground" DropShadow="true"
                PopupControlID="pnlVisibility" CancelControlID="btnCancel" TargetControlID="btnSave" />

            <asp:Panel CssClass="popupWindow" ID="pnlVisibility" runat="server" Width="250px" DefaultButton="btnSave">
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
                        <td align="right">
                            <asp:Button ID="btnSave" runat="server" Text="Uložit" UseSubmitBehavior="false" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCancel" runat="server" Text="Zavřít" CssClass="btn btn-secondary" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
