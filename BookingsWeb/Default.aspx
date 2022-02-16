<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Bookings.Login" %>

<!DOCTYPE html>
<link rel="stylesheet" href="App_Themes/default.css" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="login" runat="server">
		<div style="text-align:center">
    <table width="100%">
        <tr><td align="center">
              <table>
                <tr>
                    <td align="right"><asp:Label id="lblUsername" runat="server" meta:resourcekey="lblUsernameResource1" Text="kasutaja:"></asp:Label>
                    </td>
                    <td><asp:TextBox id="txtUser" runat="server" Width="150px" meta:resourcekey="txtUserResource1"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </td></tr>
    </table>
			<p><asp:LinkButton id="btnLogin" runat="server" CssClass="btn" meta:resourcekey="btnLoginResource1">sisene</asp:LinkButton></p>
			<p><asp:Label id="lblWrong" runat="server" ForeColor="Red" meta:resourcekey="lblWrongResource1">kasutajat ei leitud</asp:Label></p>
		</div>
    </form>
</body>
</html>
