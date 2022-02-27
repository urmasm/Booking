Public Class RoomGridViewModel
    Inherits GridViewModelBase

    Dim _roomService As IRoomService
    Public Sub New()
        _roomService = New RoomService
        Load()
        OpenWindowCommand = New Command(AddressOf OpenWindow)
        OpenNewCommand = New Command(AddressOf OpenNewWindow)
    End Sub

    Public Sub New(customerService As ICustomerService)
        _roomService = customerService
        Load()
        OpenWindowCommand = New Command(AddressOf OpenWindow)
        OpenNewCommand = New Command(AddressOf OpenNewWindow)
    End Sub

    Public Overrides Sub Load()
        Rooms = _roomService.GetRooms.Result
    End Sub

    Public Overrides Sub OpenWindow()
        If Not SelectedRoom Is Nothing Then
            Dim viewmodel As New RoomViewModel(SelectedRoom.RoomID, Me, _roomService)
            Dim view As New RoomView
            view.DataContext = viewmodel
            view.ShowDialog()
        End If
    End Sub

    Public Overrides Sub OpenNewWindow()
        Dim viewmodel As New RoomViewModel(0, Me, _roomService)
        Dim view As New RoomView
        view.DataContext = viewmodel
        view.ShowDialog()
    End Sub

    Private _rooms As List(Of Room)
    Public Property Rooms As List(Of Room)
        Get
            Return _rooms
        End Get
        Set(value As List(Of Room))
            _rooms = value
            OnPropertyChanged(NameOf(Rooms))
        End Set
    End Property

    Private _selectedRoom As Room
    Property SelectedRoom As Room
        Get
            Return _selectedRoom
        End Get
        Set(value As Room)
            _selectedRoom = value
            OnPropertyChanged(NameOf(SelectedRoom))
        End Set
    End Property

End Class
