<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="Edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="act" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">



    <asp:Panel ID="pnlHeader" runat="server" CssClass="app-panel text-center">
        <div class="mb-1">
            <h1>
                <asp:Label ID="lblCaption" runat="server"></asp:Label></h1>
        </div>
        <div class="mb-2">
            <asp:Image ID="imgDetail" runat="server" CssClass="image-detail rounded" />
        </div>
        <div class="mb-1 mx-auto text-start panel-control">
            <table class="w-100 form-table">
                <tr>
                    <td>Název:</td>
                    <td>
                        <asp:TextBox ID="txtCaption" runat="server" CssClass="form-control" MaxLength="128"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Dovednost:</td>
                    <td>
                        <asp:DropDownList ID="ddlSkill" runat="server" CssClass="form-select" DataValueField="Id" DataTextField="Caption"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Obrázek:</td>
                    <td>
                        <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control text-start" Width="100%" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="m-1 text-center">
            <asp:Button ID="btnSave" runat="server" Text="Uložit" OnClick="Save_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnClose" runat="server" Text="Zavřít" OnClick="Close_Click" CssClass="btn btn-secondary" />
        </div>
    </asp:Panel>

    <div class="app-panel">

        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>

                <div class="m-1 app-float-panel">
                    <div class="float-start">
                        <asp:Label ID="lblRows" runat="server" CssClass="caption" Text="Informace"></asp:Label>
                    </div>
                    <div class="float-end">
                        <asp:Button ID="btnNewRow" runat="server" Text="Přidat informaci" OnClick="NewRow_Click" CssClass="btn btn-primary" />
                    </div>
                </div>

                <div class="m-1">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>Limit</th>
                                <th>Povolání</th>
                                <th>Min.znalost</th>
                                <th>Text</th>
                                <th></th>
                            </tr>
                        </thead>

                        <asp:Repeater ID="rptRows" runat="server" OnItemCommand="Rows_ItemCommand" OnItemDataBound="Rows_ItemDataBound">
                            <ItemTemplate>
                                <tr runat="server" id="trRow">
                                    <td class="align-middle"><%# Eval("InfoLevel")%></td>
                                    <td class="align-middle"><%# Eval("CharacterClass")%></td>
                                    <td class="align-middle"><%# Eval("SkillLevel")%></td>
                                    <td><%# Eval("InfoTextHtml")%></td>
                                    <td class="align-middle text-end">
                                        <asp:LinkButton ID="btnEditRow" runat="server" Text="Editovat" ToolTip="Editovat" CommandName="EditRow" CommandArgument='<%# Eval("Guid") %>' />
                                        <asp:LinkButton ID="btnDeleteRow" runat="server" Text="Smazat" ToolTip="Smazat" CommandName="DeleteRow" CommandArgument='<%# Eval("Guid") %>' OnClientClick="return ConfirmDelete();" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </table>
                </div>

                <act:ModalPopupExtender ID="detailPopup" runat="server" BackgroundCssClass="popupBackground" DropShadow="true"
                    PopupControlID="pnlDetailRow" CancelControlID="btnCancelRow" TargetControlID="btnSaveRow" />

                <asp:Panel CssClass="popupWindow" ID="pnlDetailRow" runat="server" DefaultButton="btnSaveRow" Width="900px">
                    <table class="w-100 form-table">
                        <tr>
                            <td class="w-15">Úroveň přístupu</td>
                            <td>
                                <asp:TextBox ID="txtInfoLevel" runat="server" MaxLength="5" Width="150px" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Povolání</td>
                            <td>
                                <asp:DropDownList ID="ddlIdCharacterClass" runat="server" Width="150px" DataValueField="Id" DataTextField="Caption" CssClass="form-select"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Minimální znalost</td>
                            <td>
                                <asp:DropDownList ID="ddlIdSkillLevel" runat="server" Width="150px" DataValueField="Id" DataTextField="Caption" CssClass="form-select"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Nadpis</td>
                            <td>
                                <asp:TextBox ID="txtItemCaption" runat="server" MaxLength="256" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Popis</td>
                            <td>
                                <asp:TextBox ID="txtInfoText" runat="server" TextMode="MultiLine" Rows="14" CssClass="form-control"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnSaveRow" runat="server" Text="Uložit" UseSubmitBehavior="false" OnClick="SaveRow_Click" CssClass="btn btn-primary" />
                                <asp:Button ID="btnCancelRow" runat="server" Text="Zavřít" CssClass="btn btn-secondary" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>

    </div>

    <asp:UpdatePanel ID="upValues" runat="server">
        <ContentTemplate>

            <div class="app-panel">
                <div class="m-1 app-float-panel">
                    <div class="float-start">
                        <asp:Label ID="lblValues" runat="server" CssClass="caption" Text="Hody kostkou a bonusy postav"></asp:Label>
                    </div>
                    <div class="float-end">
                        <asp:Button ID="btnGenerateValues" runat="server" Text="Doplnit hody" OnClick="GenerateValues_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="btnNewValue" runat="server" Text="Přidat hod" OnClick="NewValue_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
                <div class="m-1">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>Postava</th>
                                <th>Hod kostkou</th>
                                <th>Bonus</th>
                                <th></th>
                            </tr>
                        </thead>

                        <asp:Repeater ID="rptValues" runat="server" OnItemDataBound="Values_ItemDataBound" OnItemCommand="Values_ItemCommand">
                            <ItemTemplate>
                                <tr runat="server" id="trRow">
                                    <td><%# Eval("Character")%></td>
                                    <td><%# Eval("CharacterValue")%></td>
                                    <td><%# Eval("CharacterBonus")%></td>
                                    <td class="text-end">
                                        <asp:LinkButton ID="btnEditRow" runat="server" Text="Editovat" ToolTip="Editovat" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                                        <asp:LinkButton ID="btnDeleteRow" runat="server" Text="Smazat" ToolTip="Smazat" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' OnClientClick="return ConfirmDelete();" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </table>
                </div>
            </div>

            <act:ModalPopupExtender ID="detailPopupValues" runat="server" BackgroundCssClass="popupBackground" DropShadow="true"
                PopupControlID="pnlDetailValue" CancelControlID="btnCancelValue" TargetControlID="btnSaveValue" />

            <asp:Panel CssClass="popupWindow" ID="pnlDetailValue" runat="server" DefaultButton="btnSaveValue" Width="250px">
                <table class="w-100 form-table">
                    <tr>
                        <td>Postava</td>
                        <td>
                            <asp:DropDownList ID="ddlIdCharacter" runat="server" DataValueField="Id" DataTextField="Name" CssClass="form-select"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Hod kostkou</td>
                        <td>
                            <asp:TextBox ID="txtCharacterValue" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Bonus</td>
                        <td>
                            <asp:TextBox ID="txtCharacterBonus" runat="server" MaxLength="5" CssClass="form-control"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2" class="text-end">
                            <asp:Button ID="btnSaveValue" runat="server" Text="Uložit" UseSubmitBehavior="false" OnClick="SaveValue_Click" CssClass="btn btn-primary" />
                            <asp:Button ID="btnCancelValue" runat="server" Text="Zavřít" CssClass="btn btn-secondary" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
