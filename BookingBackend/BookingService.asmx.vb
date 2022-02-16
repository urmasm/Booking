Imports System.ComponentModel
Imports System.Threading.Tasks
Imports System.Web.Services

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class BookingService
    Inherits WebService
    Implements IBookingService

    Public Property CustomerService As ICustomerService
    Public Property RoomService As IRoomService
    Public Property BookingsService As IBookingsService

    Public Sub New()
        CustomerService = New CustomerService
        RoomService = New RoomService
        BookingsService = New BookingsService
    End Sub

    <WebMethod()>
    Public Function GetCustomers() As Task(Of List(Of Customer)) Implements ICustomerService.GetCustomers
        Return CustomerService.GetCustomers
    End Function

    <WebMethod()>
    Public Function GetCustomer(customerID As Integer) As Task(Of List(Of Customer)) Implements ICustomerService.GetCustomer
        Return CustomerService.GetCustomer(customerID)
    End Function

    <WebMethod()>
    Public Function SaveCustomer(customer As Customer) As Task(Of List(Of Integer)) Implements ICustomerService.SaveCustomer
        Return CustomerService.SaveCustomer(customer)
    End Function

    <WebMethod()>
    Public Function DeleteCustomer(customerID As Integer) As Task Implements ICustomerService.DeleteCustomer
        Return CustomerService.DeleteCustomer(customerID)
    End Function

    <WebMethod()>
    Public Function GetRooms() As Task(Of List(Of Room)) Implements IRoomService.GetRooms
        Dim result = RoomService.GetRooms.Result
        Return RoomService.GetRooms
    End Function

    <WebMethod()>
    Public Function GetRoom(roomID As Integer) As Task(Of List(Of Room)) Implements IRoomService.GetRoom
        Return RoomService.GetRoom(roomID)
    End Function

    <WebMethod()>
    Public Function GetFreeRooms(startDate As Date, endDate As Date) As Task(Of List(Of Room)) Implements IRoomService.GetFreeRooms
        Return RoomService.GetFreeRooms(startDate, endDate)
    End Function

    <WebMethod()>
    Public Function SaveRoom(room As Room) As Task(Of List(Of Integer)) Implements IRoomService.SaveRoom
        Return RoomService.SaveRoom(room)
    End Function

    <WebMethod()>
    Public Function DeleteRoom(roomID As Integer) As Task Implements IRoomService.DeleteRoom
        Return RoomService.DeleteRoom(roomID)
    End Function

    <WebMethod()>
    Public Function GetBookings() As Task(Of List(Of Reservation)) Implements IBookingsService.GetBookings
        Return BookingsService.GetBookings
    End Function

    <WebMethod()>
    Public Function GetBookingsInPeriod(startDate As Date, endDate As Date) As Task(Of List(Of Reservation)) Implements IBookingsService.GetBookingsInPeriod
        Return BookingsService.GetBookingsInPeriod(startDate, endDate)
    End Function

    <WebMethod()>
    Public Function GetBooking(bookingID As Integer) As Task(Of List(Of Reservation)) Implements IBookingsService.GetBooking
        Return BookingsService.GetBooking(bookingID)
    End Function

    <WebMethod()>
    Public Function GetCustomerBookings(customerID As Integer) As Task(Of List(Of Reservation)) Implements IBookingsService.GetCustomerBookings
        Return GetCustomerBookings(customerID)
    End Function

    <WebMethod()>
    Public Function SaveBooking(booking As Reservation) As Task(Of List(Of Integer)) Implements IBookingsService.SaveBooking
        Return BookingsService.SaveBooking(booking)
    End Function

    <WebMethod()>
    Public Function DeleteBooking(bookingID As Integer) As Task Implements IBookingService.DeleteBooking
        Return BookingsService.DeleteBooking(bookingID)
    End Function
End Class