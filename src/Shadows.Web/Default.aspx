<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="cphMain" runat="server" ContentPlaceHolderID="contentBody">

    <asp:Panel ID="pnlHeader" runat="server" DefaultButton="btnSearch" CssClass="app-panel">
        <div class="mb-1 text-center">
            <h1>Knihovna</h1>
        </div>
        <div class="mb-1 app-float-panel">
            <div class="float-start m-1 panel-control">
                <table class="w-100 form-table">
                    <tr>
                        <td>Dovednost:</td>
                        <td>
                            <asp:DropDownList ID="ddlSearchSkill" runat="server" DataValueField="IdSkill" DataTextField="DisplayCaption" CssClass="form-select w-100"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlSearchSkill_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Hledat:</td>
                        <td>
                            <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control w-100"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="float-end m-1">
                <asp:CheckBox ID="chbShowImages" runat="server" OnCheckedChanged="Search_Click" AutoPostBack="true" CssClass="me-1 align-middle chb" Text="Obrázky" />
                <asp:Button ID="btnSearch" runat="server" Text="Hledat" OnClick="Search_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnPrint" runat="server" Text="Tisk" OnClick="Print_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnNew" runat="server" Text="Nový záznam" ToolTip="Nový" OnClick="New_Click" CssClass="btn btn-primary" />
            </div>

        </div>

    </asp:Panel>

    <div class="app-panel">

        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>

                <asp:Repeater ID="rptItems" runat="server" OnItemCreated="Items_ItemCreated" OnItemCommand="Items_ItemCommand" OnItemDataBound="Items_ItemDataBound">
                    <HeaderTemplate>
                        <table class="table table-hover">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="align-middle">
                                <asp:LinkButton ID="btnShowRowCaption" runat="server" Text='<%# Eval("Caption")%>' ToolTip="Zobrazit" CommandName="ShowRow" CommandArgument='<%# Eval("Id") %>' />
                            </td>
                            <td class="align-middle">
                                <%# Eval("SkillName") %>
                            </td>
                            <td class="align-middle text-center">
                                <asp:Image ID="imgListImage" runat="server" CssClass="image-list rounded" />
                            </td>
                            <td class="align-middle text-end">
                                <asp:LinkButton ID="btnShowRow" runat="server" Text='<%# Eval("ShowDetailButtonLabel") %>' ToolTip="Zobrazit" CommandName="ShowRow" CommandArgument='<%# Eval("Id") %>' />
                                <asp:LinkButton ID="btnEditRow" runat="server" Text="Editovat" ToolTip="Editovat" CommandName="EditRow" CommandArgument='<%# Eval("Id") %>' />
                                <asp:LinkButton ID="btnDeleteRow" runat="server" Text="Smazat" ToolTip="Smazat" CommandName="DeleteRow" CommandArgument='<%# Eval("Id") %>' OnClientClick="return ConfirmDelete();" />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <div class="text-center">
                    <asp:Label runat="server" ID="lblRowsCountInfo"></asp:Label>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </div>


</asp:Content>
