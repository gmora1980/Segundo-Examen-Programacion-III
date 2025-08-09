<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 300px; margin: 50px auto; font-family: Arial;">
            <h2>Login</h2>
            <asp:Label ID="Label1" runat="server" Text="Usuario:" AssociatedControlID="txtUsuario" /><br />
            <asp:TextBox ID="txtUsuario" runat="server" Width="200px"></asp:TextBox><br /><br />

            <asp:Label ID="Label2" runat="server" Text="Contraseña:" AssociatedControlID="txtPassword" /><br />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox><br /><br />

            <asp:Button ID="btnLogin" runat="server" Text="Iniciar Sesión" OnClick="btnLogin_Click" /><br /><br />

            <asp:Label ID="lblError" runat="server" ForeColor="Red" EnableViewState="false" />
        </div>
    </form>
</body>
</html>