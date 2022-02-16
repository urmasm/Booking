Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CustomerGridViewModel
    Implements INotifyPropertyChanged

    Dim _customerService As ICustomerService

    Public Sub New()
        _customerService = New CustomerService
        LoadCustomers()
        OpenCustomerCommand = New Command(AddressOf OpenCustomerWindow)
        NewCustomerCommand = New Command(AddressOf OpenNewCustomerWindow)
    End Sub

    Public Sub New(customerService As ICustomerService)
        _customerService = customerService
        LoadCustomers()
        OpenCustomerCommand = New Command(AddressOf OpenCustomerWindow)
        NewCustomerCommand = New Command(AddressOf OpenNewCustomerWindow)
    End Sub

    Public Sub LoadCustomers()
        Customers = _customerService.GetCustomers.Result
    End Sub

    Private Sub OpenCustomerWindow()
        If Not SelectedCustomer Is Nothing Then
            Dim viewmodel As New CustomerViewModel(SelectedCustomer.CustomerID, Me, _customerService)
            Dim view As New CustomerView
            view.DataContext = viewmodel
            view.ShowDialog()
        End If
    End Sub

    Private Sub OpenNewCustomerWindow()
        Dim viewmodel As New CustomerViewModel(0, Me, _customerService)
        Dim view As New CustomerView
        view.DataContext = viewmodel
        view.ShowDialog()
    End Sub

    Public Property OpenCustomerCommand() As Command
    Public Property NewCustomerCommand As Command

    Private _customers As List(Of Customer)
    Public Property Customers As List(Of Customer)
        Get
            Return _customers
        End Get
        Set(value As List(Of Customer))
            _customers = value
            OnPropertyChanged(NameOf(Customers))
        End Set
    End Property

    Private _selectedCustomer As Customer
    Property SelectedCustomer As Customer
        Get
            Return _selectedCustomer
        End Get
        Set(value As Customer)
            _selectedCustomer = value
            OnPropertyChanged(NameOf(SelectedCustomer))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

End Class
