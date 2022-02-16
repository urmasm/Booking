Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class RoomGridViewModel
    Implements INotifyPropertyChanged

    Dim _roomService As IRoomService

    Public Sub New()
        _roomService = New RoomService
        LoadRooms()
        OpenRoomCommand = New Command(AddressOf OpenRoomWindow)
        NewRoomCommand = New Command(AddressOf OpenNewRoomWindow)
    End Sub

    Public Sub New(customerService As ICustomerService)
        _roomService = customerService
        LoadRooms()
        OpenRoomCommand = New Command(AddressOf OpenRoomWindow)
        NewRoomCommand = New Command(AddressOf OpenNewRoomWindow)
    End Sub

    Public Sub LoadRooms()
        Rooms = _roomService.GetRooms.Result
    End Sub

    Private Sub OpenRoomWindow()
        If Not SelectedRoom Is Nothing Then
            Dim viewmodel As New RoomViewModel(SelectedRoom.RoomID, Me, _roomService)
            Dim view As New RoomView
            view.DataContext = viewmodel
            view.ShowDialog()
        End If
    End Sub

    Private Sub OpenNewRoomWindow()
        Dim viewmodel As New RoomViewModel(0, Me, _roomService)
        Dim view As New RoomView
        view.DataContext = viewmodel
        view.ShowDialog()
    End Sub

    Public Property OpenRoomCommand() As Command
    Public Property NewRoomCommand As Command

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

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

End Class
