Imports System.ServiceModel
Imports System.Threading.Tasks

<ServiceContract()>
Public Interface ICustomerService
    <OperationContract()>
    Function GetCustomers() As Task(Of List(Of Customer))
    <OperationContract()>
    Function GetCustomer(customerID As Integer) As Task(Of List(Of Customer))
    <OperationContract()>
    Function SaveCustomer(customer As Customer) As Task(Of List(Of Integer))
    <OperationContract()>
    Function DeleteCustomer(customerID As Integer) As Task
End Interface
