Public Interface IBookingService
    Function GetBookings() As Task(Of List(Of Reservation))
    Function GetBookingsInPeriod(startDate As Date, endDate As Date) As Task(Of List(Of Reservation))
    Function GetBookingsForGrid() As Task(Of List(Of BookingGridDto))
    Function GetBooking(id As Integer) As Task(Of Reservation)
    Function SaveBooking(Booking As Reservation) As Task(Of Integer)
    Function DeleteBooking(id As Integer) As Task
End Interface
