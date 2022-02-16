Imports System.ServiceModel

<ServiceContract()>
Public Interface IBookingService
    Inherits ICustomerService, IRoomService, IBookingsService

End Interface
