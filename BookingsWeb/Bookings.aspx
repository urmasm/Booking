<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Bookings.aspx.vb" Inherits="Bookings.Bookings" EnableEventValidation="false" %>

<!DOCTYPE html>
        <link rel="stylesheet" href="App_Themes/default.css" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Broneeringud</title>
</head>
<body style="padding-left:50px; padding-top:50px;">
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnNew" runat="server" Text="Alusta uut broneeringut" />
            <p />
            <hr />
            <p />
            <asp:Label ID="lbl0" runat="server" Text="Broneeringud" Font-Size="12" visible="true" />
            <br />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" />
    <asp:GridView ID="grdBookings" runat="server" AutoGenerateColumns="False" ShowFooter="False" AllowSorting="True" GridLines="Vertical" DataKeyNames="ReservationID">
        
        <Columns>
        <asp:CommandField ButtonType="Link" ShowDeleteButton="true" HeaderText="" DeleteText="kustuta"
            ControlStyle-Width="50" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50">
            <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
        </asp:CommandField>

             <asp:TemplateField>
                <HeaderTemplate>
                    <Label>Algus</Label>
                </HeaderTemplate>
                <HeaderStyle VerticalAlign="Bottom" />
                <ItemTemplate>
                    <asp:Label ID="lbl1" runat="server" Text='<%# Bind("ReservationStart", "{0:dd.MM.yyyy}") %>' ></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <Label>Lõpp</Label>
                </HeaderTemplate>
                <HeaderStyle VerticalAlign="Bottom" />
                <ItemTemplate>
                    <asp:Label ID="lbl2" runat="server" Text='<%# Bind("ReservationEnd", "{0:dd.MM.yyyy}") %>' ></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <Label>Tuba<Label>
                </HeaderTemplate>
                <HeaderStyle VerticalAlign="Bottom" />
                <ItemTemplate>
                    <asp:Label ID="lbl3" runat="server" Text='<%# Bind("RoomNo") %>' ></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <Labe>Voodeid</Label>
                </HeaderTemplate>
                <HeaderStyle VerticalAlign="Bottom" />
                <ItemTemplate>
                    <asp:Label ID="lbl4" runat="server" Text='<%# Bind("Beds") %>' ></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <Label>Hind</Label>
                </HeaderTemplate>
                <HeaderStyle VerticalAlign="Bottom" />
                <ItemTemplate>
                    <asp:Label ID="lbl5" runat="server" Text='<%# Bind("Price") %>' ></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <Label>Hind kokku</Label>
                </HeaderTemplate>
                <HeaderStyle VerticalAlign="Bottom" />
                <ItemTemplate>
                    <asp:Label ID="lbl6" runat="server" Text='<%# Bind("PriceTotal") %>' ></asp:Label>
                </ItemTemplate>
             </asp:TemplateField>

             <asp:BoundField DataField="ReservationID" HeaderText="ReservationID" InsertVisible="False" ReadOnly="True" SortExpression="ReservationID" Visible="False"/>

            </Columns>
        <RowStyle CssClass="row" />
        <AlternatingRowStyle CssClass="altrow" />
        <SelectedRowStyle CssClass="selrow" />   
    </asp:GridView>
        </div>

   
    </form>
</body>
</html>
