Imports System.Data.SqlClient
Imports System.Threading.Tasks

Public Class BookingsService
    Implements IBookingsService

    Public Async Function GetBookings() As Task(Of List(Of Reservation)) Implements IBookingsService.GetBookings
        Dim bookingsList As New List(Of Reservation)

        Dim sql As String = "SELECT ReservationID, CustomerID, RoomID, ReservationStart, ReservationEnd FROM Reservations"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim c As New Reservation
                With c
                    .ReservationID = dtr(0)
                    .CustomerID = dtr(1)
                    .RoomID = dtr(2)
                    .ReservationStart = dtr(3)
                    .ReservationEnd = dtr(4)
                End With
                bookingsList.Add(c)
            End While
            cmd.Dispose()
        End Using
        Return bookingsList
    End Function

    Public Async Function GetBookingsInPeriod(startDate As Date, endDate As Date) As Task(Of List(Of Reservation)) Implements IBookingsService.GetBookingsInPeriod
        Throw New NotImplementedException()
    End Function

    Public Async Function GetBooking(bookingID As Integer) As Task(Of List(Of Reservation)) Implements IBookingsService.GetBooking
        Dim bookingsList As New List(Of Reservation)
        Dim sql As String = "SELECT ReservationID, CustomerID, RoomID, ReservationStart, ReservationEnd FROM Reservations WHERE ReservationID = @ReservationID"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ReservationID", bookingID)
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim booking As New Reservation
                With booking
                    .ReservationID = dtr(0)
                    .CustomerID = dtr(1)
                    .RoomID = dtr(2)
                    .ReservationStart = dtr(3)
                    .ReservationEnd = dtr(4)
                End With
                bookingsList.Add(booking)
            End While
            cmd.Dispose()
        End Using
        Return bookingsList
    End Function

    Public Async Function GetCustomerBookings(customerID As Integer) As Task(Of List(Of Reservation)) Implements IBookingsService.GetCustomerBookings
        Throw New NotImplementedException()
    End Function

    Public Async Function SaveBooking(booking As Reservation) As Task(Of List(Of Integer)) Implements IBookingsService.SaveBooking
        Dim updateSQL = "UPDATE Reservations SET CustomerID = @CustomerID, RoomID = @RoomID, ReservationStart = @ReservationStart, ReservationEnd = @ReservationEnd WHERE ReservationID = @ReservationID; SELECT @ReservationID"
        Dim insertSQL = "INSERT INTO Reservations (CustomerID, RoomID, ReservationStart, ReservationEnd) VALUES (@CustomerID, @RoomID, @ReservationStart, @ReservationEnd); SELECT @@IDENTITY"
        Dim bID As Integer
        Dim lID As New List(Of Integer)
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            With cmd.Parameters
                .AddWithValue("@CustomerID", booking.CustomerID)
                .AddWithValue("@RoomID", booking.RoomID)
                .AddWithValue("@ReservationStart", booking.ReservationStart)
                .AddWithValue("@ReservationEnd", booking.ReservationEnd)
                .AddWithValue("@ReservationID", booking.ReservationID)
            End With
            If booking.ReservationID > 0 Then
                cmd.CommandText = updateSQL
            Else
                cmd.CommandText = insertSQL
            End If
            bID = cmd.ExecuteScalar()
            lID.Add(bID)
            cmd.Dispose()
        End Using
        Return lID
    End Function

    Public Function DeleteBooking(bookingID As Integer) As Task Implements IBookingsService.DeleteBooking
        Dim sql = "DELETE FROM Reservations WHERE ReservationID = @ReservationID"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ReservationID", bookingID)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Function
End Class
