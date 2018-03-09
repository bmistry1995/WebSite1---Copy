<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login/Register</title>
    <link href="css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="container">
            <div class="row">
                <div class="col-6">
                    Login
                    <br />
                    <br />
                    <asp:Label ID="lbUsername" runat="server" Text="Username"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="lbPassword" runat="server" Text="Password"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Login" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Label ID="lbNotification" runat="server"></asp:Label>
                    <br />
                </div>
                <div class="col-6">
                    Registration
                    <br />
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Username"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtNewUserName" runat="server" CssClass="form-check-label"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtNewPassword" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label3" runat="server" Text="Name"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtNewName" runat="server"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnRegister" runat="server" OnClick="btnRegister_Click" Text="Register" />
                    <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" />
                    <br />
                    <br />
                    <asp:Label ID="lbRegisterNotification" runat="server"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
