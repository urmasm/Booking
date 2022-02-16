Imports System.Data.SqlClient

Public Class Bookings
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then Response.Redirect("Default.aspx")
        If Not IsPostBack Then
            FillGrid()
        End If
    End Sub

    Public Sub FillGrid()
        Dim bookingsList As New List(Of Booking)
        Dim connstr As String = ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString
        Dim conn As New SqlConnection(connstr)
        Dim sql As String = "SELECT Reservations.ReservationID, Reservations.ReservationStart, Reservations.ReservationEnd, Rooms.RoomNo, Rooms.Beds, Rooms.Price, DATEDIFF(DAY,Reservations.ReservationStart,Reservations.ReservationEnd) * Rooms.Price AS Kokku
                            FROM Customers INNER JOIN Reservations ON Customers.CustomerID = Reservations.CustomerID INNER JOIN Rooms ON Reservations.RoomID = Rooms.RoomID
                            WHERE (Reservations.CustomerID = @CustomerID)"
        Dim cmd As New SqlCommand(sql, conn)
        Dim dtr As SqlDataReader
        With cmd
            .Parameters.AddWithValue("@CustomerID", Session("UserID"))
        End With
        conn.Open()
        Using conn
            dtr = cmd.ExecuteReader
            While dtr.Read
                Dim b = New Booking With {
                    .ReservationID = dtr(0),
                    .ReservationStart = dtr(1),
                    .ReservationEnd = dtr(2),
                    .RoomNo = dtr(3),
                    .Beds = dtr(4),
                    .Price = dtr(5)}
                bookingsList.Add(b)
            End While
        End Using
        cmd.Dispose()
        dtr.Close()
        ViewState("bookingsList") = bookingsList

        Me.grdBookings.DataSource = bookingsList
        Me.grdBookings.DataBind()
    End Sub

    Protected Sub grdBookings_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdBookings.SelectedIndexChanged
        Dim bookingsList As List(Of Booking)
        bookingsList = ViewState("bookingsList")
        Dim reservationID = Me.grdBookings.SelectedDataKey.Item(0)
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Response.Redirect("NewReservation.aspx")
    End Sub

    Private Sub grdBookings_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdBookings.RowDeleting
        Dim bookingsList As List(Of Booking)
        bookingsList = ViewState("bookingsList")
        Dim reservationID = e.Keys(0)

        Dim reservationStart = bookingsList.Where(Function(x) x.ReservationID = reservationID).First.ReservationStart
        Dim dd = DateDiff(DateInterval.Day, Now, reservationStart)
        Select Case dd
            Case < 0
                Me.lblError.Text = "Valitud broneeringu algus on minevikus. Tühistamine katkestatud"
                Me.lblError.Visible = True
                Return
            Case 0 To 3
                Me.lblError.Text = "Valitud broneeringut alguseni on vähem kui kolm päeva. Tühistamine katkestatud"
                Me.lblError.Visible = True
                Return
            Case Else
                Me.lblError.Visible = False
        End Select

        Dim connstr As String = ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString
        Dim conn As New SqlConnection(connstr)
        Dim sql As String = "DELETE FROM Reservations WHERE ReservationID = @ReservationID"
        Dim cmd As New SqlCommand(sql, conn)
        With cmd
            .Parameters.AddWithValue("@ReservationID", reservationID)
        End With
        conn.Open()
        Using conn
            cmd.ExecuteNonQuery()
        End Using
        cmd.Dispose()

        FillGrid()
    End Sub

    Private Sub grdBookings_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdBookings.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each button As LinkButton In e.Row.Cells(0).Controls.OfType(Of LinkButton)()
                If button.CommandName = "Delete" Then
                    button.Attributes("onclick") = "if(!confirm('Kas soovid valitud broneeringu kustutada?')){ return false; };"
                End If
            Next
        End If

    End Sub

    <Serializable>
    Public Class Booking
        Public Property ReservationID As Integer
        Public Property ReservationStart As DateTime
        Public Property ReservationEnd As DateTime
        Public Property RoomNo As String
        Public Property Beds As Integer
        Public Property Price As Decimal
        Public ReadOnly Property PriceTotal As Decimal
            Get
                Return DateDiff(DateInterval.Day, ReservationStart, ReservationEnd) * Price
            End Get
        End Property
    End Class
End Class