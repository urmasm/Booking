<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewReservation.aspx.vb" Inherits="Bookings.NewReservation" %>

        <script language="javascript" type="text/javascript" src="App_Scripts/rowchange.js"></script>

<!DOCTYPE html>
<link rel="stylesheet" href="App_Themes/default.css" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Uus broneering</title>
</head>
<body style="padding-left:50px; padding-top:50px;">
    <form id="form1" runat="server">
     <div>
                <p/>
                <table>
                    <tr>
                        <td><asp:Label ID="lbla" runat="server" Text="Broneeringu algus"></asp:Label></td>
                        <td><asp:Calendar ID="calStart" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lble" runat="server" Text="Broneeringu lõpp"></asp:Label></td>
                        <td><asp:Calendar ID="calEnd" runat="server"/></td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="lblb" runat="server" Text="Voodeid" /></td>
                        <td><asp:TextBox TextMode="Number" runat="server" ID="tbxBeds" /></td>
                    </tr>
                 </table>
         <p />
         <table>
             <tr>
                 <td><asp:Button runat="server" ID="btnFindRooms" Text="Otsi tube" /></td>
                 <td><asp:Label ID="lblError" runat="server" Visible="false" /></td>
             </tr>
         </table>
              <asp:Panel runat="server" ID="pnlRooms" Visible="false">
                  <asp:Label runat="server" ID="lblNoRooms" Text="Sobivaid tube ei leitud." Visible="false" />
                  <asp:GridView ID="grdRooms" runat="server" AutoGenerateColumns="False" ShowFooter="False" AllowSorting="True" GridLines="Vertical" DataKeyNames="RoomID">
                  <Columns>
                    <asp:CommandField ShowSelectButton="true" SelectText="Vali" Visible="true"/>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <Label>Toa nr</Label>
                        </HeaderTemplate>
                        <HeaderStyle/>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("RoomNo") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <Label>Hind</Label>
                        </HeaderTemplate>
                        <HeaderStyle/>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Price") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <Label>Broneeringu maksumus</Label>
                        </HeaderTemplate>
                        <HeaderStyle/>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("PriceTotal") %>' ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RoomID" HeaderText="RoomID" InsertVisible="False" ReadOnly="True" SortExpression="RoomID" Visible="False"/>
                  </Columns>
                  </asp:GridView>

            </asp:Panel>
          </div>
        <div>
            <p />
            <asp:Label ID="lblConfirm" runat="server" Font-Size="12" Text="Broneeringu andmed:" Visible="false" />
            <br />
            <asp:Label ID="lblReservationText" runat="server" />
            <p />
            <table>
                <tr>
                    <td><asp:Button runat="server" ID="btnSave" Text="Salvesta" Enabled="false"/></td>
                    <td> <asp:Button runat="server" ID="btnCancel" Text="Alusta uuesti" /></td>
                    <td><asp:Button runat="server" ID="btnBack" Text="Avalehele" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
