Imports System.Data.SqlClient

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") = "0" Then
            lblWrong.Visible = True
        Else
            lblWrong.Visible = False
        End If
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Session("UserID") = 0
        Dim user As String
        Dim intKasutajaID As Integer
        user = Request.Form("txtUser")
        Dim sql As String = "SELECT CustomerID FROM Customers WHERE Email = @Email"
        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString)
        Dim cmd As New SqlCommand(sql, conn)
        With cmd
            .Parameters.AddWithValue("@Email", user)
        End With

        conn.Open()
        Using conn
            intKasutajaID = cmd.ExecuteScalar
        End Using

        Session("UserID") = intKasutajaID

        If intKasutajaID > 0 Then Response.Redirect("Bookings.aspx")
    End Sub
End Class