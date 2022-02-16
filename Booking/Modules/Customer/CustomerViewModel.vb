Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CustomerViewModel
    Implements INotifyPropertyChanged

    Dim _customerService As ICustomerService
    Private cvm As CustomerGridViewModel
    Public Sub New(id As Integer, vm As CustomerGridViewModel, customerService As ICustomerService)
        _customerService = customerService
        LoadCustomer(id)
        SaveCommand = New Command(AddressOf SaveCustomer)
        DeleteCommand = New Command(AddressOf DeleteCustomer)
        cvm = vm
        NotificationVisibility = Visibility.Collapsed
    End Sub

    Public Async Sub LoadCustomer(id As Integer)
        Customer = Await _customerService.GetCustomer(id)
    End Sub

    Public Async Sub SaveCustomer()
        HasNoErrors = False

        If String.IsNullOrEmpty(FirstName) Then
            NotificationText = "Eesnimi on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If String.IsNullOrEmpty(LastName) Then
                NotificationText = "Perekonnanimi on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If String.IsNullOrEmpty(IdNumber) Then
            NotificationText = "Isikukood on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If String.IsNullOrEmpty(Email) Then
            NotificationText = "E-post on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If Not IsValidEmailFormat(Email) Then
            NotificationText = "E-posti aadress ei ole korrektne."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        HasNoErrors = True
        NotificationVisibility = Visibility.Collapsed

        Dim ID = Await _customerService.SaveCustomer(Customer)
        Customer.CustomerID = ID
        cvm.LoadCustomers()
    End Sub

    Public Sub DeleteCustomer()
        _customerService.DeleteCustomer(CustomerID)
        cvm.LoadCustomers()
    End Sub

    Public Property SaveCommand As Command
    Public Property DeleteCommand As Command

    Private _customer As Customer
    Public Property Customer As Customer
        Get
            Return _customer
        End Get
        Set(value As Customer)
            _customer = value
            OnPropertyChanged(NameOf(Customer))
            OnPropertyChanged(NameOf(CustomerID))
            OnPropertyChanged(NameOf(FirstName))
            OnPropertyChanged(NameOf(LastName))
            OnPropertyChanged(NameOf(IdNumber))
            OnPropertyChanged(NameOf(Email))
        End Set
    End Property

    Public Property CustomerID As Integer
        Get
            Return Customer.CustomerID
        End Get
        Set(value As Integer)
            Customer.CustomerID = value
            OnPropertyChanged(NameOf(CustomerID))
        End Set
    End Property

    Public Property FirstName As String
        Get
            Return Customer.FirstName
        End Get
        Set(value As String)
            Customer.FirstName = value
            OnPropertyChanged(NameOf(FirstName))
        End Set
    End Property

    Public Property LastName As String
        Get
            Return Customer.LastName
        End Get
        Set(value As String)
            Customer.LastName = value
            OnPropertyChanged(NameOf(LastName))
        End Set
    End Property

    Public Property IdNumber As String
        Get
            Return Customer.IdNumber
        End Get
        Set(value As String)
            Customer.IdNumber = String.Join("", value.Where(Function(x) Char.IsDigit(x)).ToArray())
            OnPropertyChanged(NameOf(IdNumber))
        End Set
    End Property

    Public Property Email As String
        Get
            Return Customer.Email
        End Get
        Set(value As String)
            Customer.Email = value
            OnPropertyChanged(NameOf(Email))
        End Set
    End Property

    Private _notificationText As String
    Public Property NotificationText As String
        Get
            Return _notificationText
        End Get
        Set(value As String)
            _notificationText = value
            OnPropertyChanged(NameOf(NotificationText))
        End Set
    End Property

    Private _hasNoErrors As Boolean
    Public Property HasNoErrors As Boolean
        Get
            Return _hasNoErrors
        End Get
        Set(value As Boolean)
            _hasNoErrors = value
            OnPropertyChanged(NameOf(HasNoErrors))
        End Set
    End Property

    Private _notificationVisibility
    Public Property NotificationVisibility As Visibility
        Get
            Return _notificationVisibility
        End Get
        Set(value As Visibility)
            _notificationVisibility = value
            OnPropertyChanged(NameOf(NotificationVisibility))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class
