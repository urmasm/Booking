Public Class BookingService
    Implements IBookingService

    Public Async Function GetBookings() As Task(Of List(Of Reservation)) Implements IBookingService.GetBookings
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim sbk = svc.GetBookings.Result
        Dim bookingsList As New List(Of Reservation)
        For Each sb In sbk
            Dim r As New Reservation With {
                .ReservationID = sb.ReservationID,
                .CustomerID = sb.CustomerID,
                .RoomID = sb.RoomID,
                .ReservationStart = sb.ReservationStart,
                .ReservationEnd = sb.ReservationEnd}
            bookingsList.Add(r)
        Next
        svc.Close()
        Return bookingsList
    End Function

    Public Async Function GetBookingsForGrid() As Task(Of List(Of BookingGridDto)) Implements IBookingService.GetBookingsForGrid
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim sbk = svc.GetBookings.Result
        Dim scs = svc.GetCustomers.Result
        Dim srm = svc.GetRooms.Result
        Dim bookingsList As New List(Of BookingGridDto)
        For Each sb In sbk
            Dim b As New BookingGridDto With {
                .ReservationID = sb.ReservationID,
                .Customer = scs.Where(Function(x) x.CustomerID = sb.CustomerID).FirstOrDefault.LastName & ", " & scs.Where(Function(x) x.CustomerID = sb.CustomerID).First.FirstName,
                .Room = srm.Where(Function(x) x.RoomID = sb.RoomID).First.RoomNo,
                .ReservationStart = sb.ReservationStart,
                .ReservationEnd = sb.ReservationEnd}
            bookingsList.Add(b)
        Next
        svc.Close()
        Return bookingsList
    End Function

    Public Function GetBookingsInPeriod(startDate As Date, endDate As Date) As Task(Of List(Of Reservation)) Implements IBookingService.GetBookingsInPeriod
        Throw New NotImplementedException()
    End Function

    Public Async Function GetBooking(id As Integer) As Task(Of Reservation) Implements IBookingService.GetBooking
        Dim svc = New BookingServiceReference.BookingServiceSoapClient
        Dim sb = svc.GetBooking(id).Result.FirstOrDefault
        Dim b As Reservation
        If sb Is Nothing Then
            b = New Reservation With {
                   .ReservationID = 0,
                   .CustomerID = 0,
                   .RoomID = 0,
                   .ReservationStart = DateAdd(DateInterval.Day, 1, Today.Date),
                   .ReservationEnd = DateAdd(DateInterval.Day, 2, Today.Date)}
        Else
            b = New Reservation With {
                   .ReservationID = sb.ReservationID,
                   .CustomerID = sb.CustomerID,
                   .RoomID = sb.RoomID,
                   .ReservationStart = sb.ReservationStart,
                   .ReservationEnd = sb.ReservationEnd}
        End If

        svc.Close()
        Return b
    End Function

    Public Async Function SaveBooking(Booking As Reservation) As Task(Of Integer) Implements IBookingService.SaveBooking
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim r = New BookingServiceReference.Reservation With {
                .ReservationID = Booking.ReservationID,
                .CustomerID = Booking.CustomerID,
                .RoomID = Booking.RoomID,
                .ReservationStart = Booking.ReservationStart,
                .ReservationEnd = Booking.ReservationEnd}
        Dim ID = svc.SaveBooking(r).Result.First
        svc.Close()
        Return ID
    End Function

    Public Function DeleteBooking(id As Integer) As Task Implements IBookingService.DeleteBooking
        Dim svc = New BookingServiceReference.BookingServiceSoapClient
        svc.DeleteBooking(id)
        Return Nothing
    End Function
End Class
