Imports System.ServiceModel
Imports System.Threading.Tasks

<ServiceContract()>
Public Interface IBookingsService
    <OperationContract()>
    Function GetBookings() As Task(Of List(Of Reservation))
    <OperationContract()>
    Function GetBookingsInPeriod(startDate As DateTime, endDate As DateTime) As Task(Of List(Of Reservation))
    <OperationContract()>
    Function GetBooking(bookingID As Integer) As Task(Of List(Of Reservation))
    <OperationContract()>
    Function GetCustomerBookings(customerID As Integer) As Task(Of List(Of Reservation))
    <OperationContract()>
    Function SaveBooking(booking As Reservation) As Task(Of List(Of Integer))
    <OperationContract()>
    Function DeleteBooking(bookingID As Integer) As Task
End Interface
