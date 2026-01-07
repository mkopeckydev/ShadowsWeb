<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CharacterSkillSetup.aspx.cs" Inherits="CharacterSkillSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
                <div class="mb-1">
                    <h1>
                        <asp:Label ID="lblCaption" runat="server" Text="Dovednosti postav"></asp:Label></h1>
                </div>
                <div class="mb-1">
                    <asp:CheckBox ID="chbIsDeadFilter" runat="server" Text="Skrýt mrtvé postavy" AutoPostBack="true" OnCheckedChanged="chbIsDeadFilter_CheckedChanged" CssClass="chb" />
                </div>
                <div class="mb-1">
                    <asp:Button ID="btnNew" runat="server" Text="Nový záznam" OnClick="btnNew_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnGenerateAll" runat="server" Text="Vygenerovat chybějící hody" OnClick="btnGenerateAll_Click" CssClass="btn btn-primary" />
                </div>
            </asp:Panel>

            <div class="app-panel">

                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>Postava</th>
                            <th>Dovednost</th>
                            <th>Znalost</th>
                            <th></th>
                        </tr>
                    </thead>

                    <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td class="align-middle"><%# Eval("CharacterName")%></td>
                                <td class="align-middle"><%# Eval("Skill")%></td>
                                <td class="align-middle"><%# Eval("SkillLevel")%></td>
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
                            <asp:Label ID="lblDetailCaption" runat="server" CssClass="caption" Text="Editace znalosti dovednosti"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Postava</td>
                        <td>
                            <asp:DropDownList ID="ddlIdCharacter" runat="server" class="form-select" DataValueField="Id" DataTextField="Name"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Dovednost</td>
                        <td>
                            <asp:DropDownList ID="ddlIdSkill" runat="server" class="form-select" DataValueField="Id" DataTextField="Caption"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Znalost</td>
                        <td>
                            <asp:DropDownList ID="ddlIdSkillLevel" runat="server" class="form-select" DataValueField="Id" DataTextField="Caption"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="text-end" colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Uložit" UseSubmitBehavior="false" OnClick="btnSave_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCancel" runat="server" Text="Zavřít" CssClass="btn btn-secondary" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
