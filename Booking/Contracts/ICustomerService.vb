Public Interface ICustomerService
    Function GetCustomers() As Task(Of List(Of Customer))
    Function GetCustomer(id As Integer) As Task(Of Customer)
    Function SaveCustomer(customer As Customer) As Task(Of Integer)
    Function DeleteCustomer(id As Integer) As Task
End Interface
