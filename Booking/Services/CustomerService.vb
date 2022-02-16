Public Class CustomerService
    Implements ICustomerService

    Public Async Function GetCustomer(id As Integer) As Task(Of Customer) Implements ICustomerService.GetCustomer
        Dim svc = New BookingServiceReference.BookingServiceSoapClient
        Dim sc = svc.GetCustomer(id).Result.FirstOrDefault
        Dim c As Customer
        If sc Is Nothing Then
            c = New Customer With {
                .CustomerID = 0,
                .FirstName = "",
                .LastName = "",
                .IdNumber = "",
                .Email = ""}
        Else
            c = New Customer With {
                .CustomerID = sc.CustomerID,
                .FirstName = sc.FirstName,
                .LastName = sc.LastName,
                .IdNumber = sc.IdNumber,
                .Email = sc.Email}
        End If
        svc.Close()
        Return c
    End Function

    Public Async Function SaveCustomer(customer As Customer) As Task(Of Integer) Implements ICustomerService.SaveCustomer
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim c = New BookingServiceReference.Customer With {
            .CustomerID = customer.CustomerID,
            .FirstName = customer.FirstName,
            .LastName = customer.LastName,
            .IdNumber = customer.IdNumber,
            .Email = customer.Email}
        Dim ID = svc.SaveCustomer(c).Result.FirstOrDefault
        svc.Close()
        Return ID
    End Function

    Public Async Function GetCustomers() As Task(Of List(Of Customer)) Implements ICustomerService.GetCustomers
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim scl = svc.GetCustomers.Result
        Dim customersList As New List(Of Customer)
        For Each sc In scl
            Dim c As New Customer With {
                .CustomerID = sc.CustomerID,
                .FirstName = sc.FirstName,
                .LastName = sc.LastName,
                .IdNumber = sc.IdNumber,
                .Email = sc.Email}
            customersList.Add(c)
        Next
        svc.Close()
        Return customersList
    End Function

    Public Async Function DeleteCustomer(id As Integer) As Task Implements ICustomerService.DeleteCustomer
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        svc.DeleteCustomer(id)
        svc.Close()
    End Function
End Class
