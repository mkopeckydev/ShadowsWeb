<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Characters.aspx.cs" Inherits="Characters" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
                <div class="mb-1">
                    <h1>
                        <asp:Label ID="lblCaption" runat="server" Text="Postavy"></asp:Label></h1>
                </div>
                <div class="mb-1">
                    <asp:CheckBox ID="chbIsDeadFilter" runat="server" Text="Skrýt mrtvé postavy" AutoPostBack="true" OnCheckedChanged="chbIsDeadFilter_CheckedChanged" CssClass="chb" />
                </div>
                <div class="mb-1">
                    <asp:Button ID="btnNew" runat="server" Text="Nová postava" OnClick="New_Click" CssClass="btn btn-primary" />
                </div>
            </asp:Panel>

            <div class="app-panel">

                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Jméno</th>
                            <th>Login</th>
                            <th>Povolání</th>
                            <th>Povolání 2</th>
                            <th>Aktivní</th>
                            <th></th>
                        </tr>
                    </thead>

                    <asp:Repeater ID="rptItems" runat="server" OnItemCommand="Items_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td class="align-middle"><%# Eval("Name")%></td>
                                <td class="align-middle"><%# Eval("UserName")%></td>
                                <td class="align-middle"><%# Eval("CharacterClass")%></td>
                                <td class="align-middle"><%# Eval("CharacterClass2")%></td>
                                <td class="align-middle"><%# Eval("IsActiveLabel")%></td>
                                <td class="align-middle text-end">
                                    <asp:LinkButton ID="btnEditRow" runat="server" Text="Editovat" ToolTip="Editovat" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                                    <asp:LinkButton ID="btnDeleteRow" runat="server" Text="Smazat" ToolTip="Smazat" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' OnClientClick="return ConfirmDelete();" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>

                </table>
            </div>

            <act:ModalPopupExtender ID="detailPopup" runat="server" BackgroundCssClass="popupBackground" DropShadow="true"
                PopupControlID="pnlDetail" CancelControlID="btnCancel" TargetControlID="btnSave" />

            <asp:Panel CssClass="popupWindow" ID="pnlDetail" runat="server" Width="350px" DefaultButton="btnSave">
                <table class="w-100 form-table">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblDetailCaption" runat="server" CssClass="caption" Text="Editace postavy"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Jméno</td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" MaxLength="32" class="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Login</td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="32" class="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Heslo</td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="32" class="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Povolání</td>
                        <td>
                            <asp:DropDownList ID="ddlIdCharacterClass" runat="server" class="form-select" DataValueField="Id" DataTextField="Caption"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Povolání 2</td>
                        <td>
                            <asp:DropDownList ID="ddlIdCharacterClass2" runat="server" class="form-select" DataValueField="Id" DataTextField="Caption"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Zpráva (android)</td>
                        <td>
                            <asp:TextBox ID="txtClientMessage" runat="server" MaxLength="64" class="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Bonus INT / 2</td>
                        <td>
                            <asp:TextBox ID="txtIntelligenceBonus" runat="server" MaxLength="5" class="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Administrator</td>
                        <td>
                            <asp:CheckBox ID="chbAdministrator" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Aktivní postava</td>
                        <td>
                            <asp:CheckBox ID="chbIsActive" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Mrtvá postava</td>
                        <td>
                            <asp:CheckBox ID="chbIsDead" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="text-end" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Uložit" UseSubmitBehavior="false" OnClick="Save_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCancel" runat="server" Text="Zavřít" CssClass="btn btn-secondary" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
