Imports System.Data.SqlClient

Public Class NewReservation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") Is Nothing Then Response.Redirect("Default.aspx")
        If Not IsPostBack Then
            Dim startDate As DateTime = DateAdd(DateInterval.Day, 1, DateTime.Today)
            calStart.TodaysDate = startDate
            calStart.SelectedDate = calStart.TodaysDate

            Dim endDate As DateTime = DateAdd(DateInterval.Day, 2, DateTime.Today)
            calEnd.TodaysDate = endDate
            calEnd.SelectedDate = calEnd.TodaysDate
        End If

    End Sub

    Private Sub grRooms_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdRooms.RowDataBound
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Attributes.Add("onmouseover", "SetNewColor(this);")
            e.Row.Attributes.Add("onmouseout", "SetOldColor(this);")
        End If
    End Sub

    Protected Sub btnFindRooms_Click(sender As Object, e As EventArgs) Handles btnFindRooms.Click
        Dim reservationStart As DateTime = calStart.SelectedDate
        Dim reservationEnd As DateTime = calEnd.SelectedDate
        Dim roomsList As New List(Of Room)
        If reservationStart > reservationEnd Then
            SetError("Lõpuaeg ei saa olla enne algusaega.")
            Return
        End If
        If reservationStart < Now Then
            SetError("Algusaeg ei saa olla minevikus.")
            Return
        End If
        Dim beds As Integer
        Try
            beds = tbxBeds.Text
            If beds > 10 Then
                SetError("Kontrolli voodite arvu.")
                Return
            End If
        Catch ex As Exception
            SetError("Kontrolli voodite arvu.")
            Return
        End Try
        Dim dd = DateDiff(DateInterval.Day, reservationStart, reservationEnd)
        Dim connstr As String = ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString
        Dim conn As New SqlConnection(connstr)
        Dim sql As String = "SELECT RoomID, RoomNo, Price FROM Rooms
                            WHERE (Beds = @Beds) AND (RoomID NOT IN
                            (SELECT RoomID FROM Reservations WHERE(ReservationStart BETWEEN @ReservationStart AND @ReservationEnd) 
                            OR (ReservationEnd BETWEEN @ReservationStart AND @ReservationEnd) 
                            OR (ReservationStart >= @ReservationStart) AND (ReservationEnd <= @ReservationEnd) 
                            OR (ReservationStart <= @ReservationStart) AND (ReservationEnd >= @ReservationEnd)))
                            ORDER BY Price"

        Dim cmd As New SqlCommand(sql, conn)
        Dim dtr As SqlDataReader
        With cmd
            .Parameters.AddWithValue("@Beds", beds)
            .Parameters.AddWithValue("@ReservationStart", reservationEnd)
            .Parameters.AddWithValue("@ReservationEnd", reservationEnd)
        End With
        conn.Open()
        Using conn
            dtr = cmd.ExecuteReader
            While dtr.Read
                Dim r = New Room With {
                    .RoomID = dtr(0),
                    .RoomNo = dtr(1),
                    .Price = dtr(2),
                    .PriceTotal = dtr(2) * dd}
                roomsList.Add(r)
            End While
        End Using
        cmd.Dispose()
        dtr.Close()

        ViewState("roomsList") = roomsList
        Me.grdRooms.DataSource = roomsList
        Me.grdRooms.DataBind()

        Me.grdRooms.SelectedIndex = roomsList.First.RoomID

        Me.btnFindRooms.Visible = False
        Me.pnlRooms.Visible = True
        Me.btnSave.Enabled = True
        SetError("")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim reservationStart As DateTime = calStart.SelectedDate
        Dim reservationEnd As DateTime = calEnd.SelectedDate
        Dim connstr As String = ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString
        Dim conn As New SqlConnection(connstr)
        Dim sql As String = "INSERT INTO Reservations(CustomerID, RoomID, ReservationStart, ReservationEnd) VALUES(@CustomerID, @RoomID, @ReservationStart, @ReservationEnd)"
        Dim cmd As New SqlCommand(sql, conn)
        With cmd
            .Parameters.AddWithValue("@CustomerID", Session("UserID"))
            .Parameters.AddWithValue("@RoomID", Session("RoomID"))
            .Parameters.AddWithValue("@ReservationStart", reservationStart)
            .Parameters.AddWithValue("@ReservationEnd", reservationEnd)
        End With
        conn.Open()
        Using conn
            cmd.ExecuteNonQuery()
        End Using

        Response.Redirect("Bookings.aspx")
    End Sub

    Public Sub SetError(errorText As String)
        lblError.Text = errorText
        lblError.Visible = True
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        btnSave.Enabled = False
        btnFindRooms.Visible = True
        pnlRooms.Visible = False
        lblConfirm.Visible = False
        lblReservationText.Text = ""
    End Sub

    Private Sub grdRooms_SelectedIndexChanged(sender As Object, e As EventArgs) Handles grdRooms.SelectedIndexChanged
        Dim roomsList As List(Of Room) = ViewState("roomsList")
        Dim roomNr = roomsList.Where(Function(x) x.RoomID = sender.selectedvalue).First.RoomNo
        Dim priceTotal = roomsList.Where(Function(x) x.RoomID = sender.selectedvalue).First.PriceTotal

        Session("RoomID") = sender.selectedvalue
        Dim confirmationText As String = "Broneeringu algus: " & calStart.SelectedDate & " <br/> " & "Broneeringu lõpp: " &
        calEnd.SelectedDate & " <br/> " & "Tuba number " & roomNr & " <br/> " & "Broneeringu maksumus: " & priceTotal

        lblReservationText.Text = confirmationText
        lblConfirm.Visible = True
    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("Bookings.aspx")
    End Sub

    <Serializable>
    Public Class Room
        Public Property RoomID As Integer
        Public Property RoomNo As String
        Public Property Price As Decimal
        Public Property PriceTotal As Decimal
    End Class

End Class