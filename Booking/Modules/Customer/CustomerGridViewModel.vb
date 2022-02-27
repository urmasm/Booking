Public Class CustomerGridViewModel
    Inherits GridViewModelBase

    Dim _customerService As ICustomerService
    Public Sub New()
        _customerService = New CustomerService
        Load()
        OpenWindowCommand = New Command(AddressOf OpenWindow)
        OpenNewCommand = New Command(AddressOf OpenNewWindow)
    End Sub

    Public Sub New(customerService As ICustomerService)
        _customerService = customerService
        Load()
        OpenWindowCommand = New Command(AddressOf OpenWindow)
        OpenNewCommand = New Command(AddressOf OpenNewWindow)
    End Sub

    Public Overrides Sub Load()
        Customers = _customerService.GetCustomers.Result
    End Sub

    Public Overrides Sub OpenWindow()
        If Not SelectedCustomer Is Nothing Then
            Dim viewmodel As New CustomerViewModel(SelectedCustomer.CustomerID, Me, _customerService)
            Dim view As New CustomerView
            view.DataContext = viewmodel
            view.ShowDialog()
        End If
    End Sub

    Public Overrides Sub OpenNewWindow()
        Dim viewmodel As New CustomerViewModel(0, Me, _customerService)
        Dim view As New CustomerView
        view.DataContext = viewmodel
        view.ShowDialog()
    End Sub

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

End Class
