<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stíny nad pevninou</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <link href="Styles/bootstrap.min.css" rel="stylesheet" type="text/css" media="all" />
    <link href="Styles/app.css" rel="stylesheet" type="text/css" media="all" />

    <script src="Scripts/jquery-3.7.1.min.js"></script>
    <script src="Scripts/bootstrap.bundle.min.js"></script>
    <script src="Scripts/app.js" type="text/javascript"></script>
</head>
<body>
    <form id="frmMain" runat="server">
        <div class="container">
            <div class="m-2 text-center">
                <asp:Image ID="imgLogo" runat="server" ImageUrl="~/Images/logo.png" class="img-fluid" Style="max-height: 300px;" />
            </div>

            <div class="m-2 login-form mx-auto">
                <div class="mb-1 text-center">
                    <h1>Přihlášení</h1>
                </div>
                <div class="mb-1">
                    <div class="input-group">
                        <span class="input-group-text" id="basic-addon1">Login</span>
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" MaxLength="32" placeholder="Login" aria-label="Login" aria-describedby="basic-addon1"></asp:TextBox>
                    </div>
                </div>
                <div class="mb-2">
                    <div class="input-group">
                        <span class="input-group-text" id="basic-addon2">Heslo</span>
                        <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="32" CssClass="form-control" placeholder="Password" aria-label="Password" aria-describedby="basic-addon2"></asp:TextBox>
                    </div>
                </div>
                <div class="mb-1 text-center">
                    <asp:Button ID="btnLogin" runat="server" Text="Přihlásit" OnClick="btnLogin_Click" CssClass="btn btn-primary" />
                </div>
                <div class="mb-1 err-message text-center">
                    <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                </div>
                <div class="mb-1 text-center mini-text">
                    Demo účet: demo/demo
                </div>
            </div>
            <div class="m-2 text-center">
                <asp:HyperLink NavigateUrl="https://mkopecky.dev/policy/shadows" ID="hlAppPolicy" runat="server">Zásady ochrany osobních údajů aplikace<br />Stíny nad pevninou</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>
