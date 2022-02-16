Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class RoomViewModel
    Implements INotifyPropertyChanged

    Dim _roomService As IRoomService
    Private rvm As RoomGridViewModel
    Public Sub New(id As Integer, vm As RoomGridViewModel, roomService As IRoomService)
        _roomService = roomService
        LoadRoom(id)
        SaveCommand = New Command(AddressOf SaveRoom)
        DeleteCommand = New Command(AddressOf DeleteRoom)
        rvm = vm
    End Sub

    Public Async Sub LoadRoom(id As Integer)
        Room = Await _roomService.GetRoom(id)
    End Sub

    Public Async Sub SaveRoom()

        HasNoErrors = False
        If String.IsNullOrEmpty(RoomNo) Then
            NotificationText = "Toa number on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If Beds = 0 Then
            NotificationText = "Voodite arv on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If Price = 0 Then
            NotificationText = "Hind on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        HasNoErrors = True
        NotificationVisibility = Visibility.Collapsed
        Dim ID = Await _roomService.SaveRoom(Room)
        Room.RoomID = ID
        rvm.LoadRooms()
    End Sub

    Public Sub DeleteRoom()
        _roomService.DeleteRoom(RoomID)
        rvm.LoadRooms()
    End Sub

    Public Property SaveCommand As Command
    Public Property DeleteCommand As Command

    Private _room As Room
    Public Property Room As Room
        Get
            Return _room
        End Get
        Set(value As Room)
            _room = value
            OnPropertyChanged(NameOf(Room))
            OnPropertyChanged(NameOf(RoomID))
            OnPropertyChanged(NameOf(RoomNo))
            OnPropertyChanged(NameOf(Beds))
            OnPropertyChanged(NameOf(Price))
        End Set
    End Property

    Public Property RoomID As Integer
        Get
            Return Room.RoomID
        End Get
        Set(value As Integer)
            Room.RoomID = value
            OnPropertyChanged(NameOf(RoomID))
        End Set
    End Property

    Public Property RoomNo As String
        Get
            Return Room.RoomNo
        End Get
        Set(value As String)
            Room.RoomNo = value
            OnPropertyChanged(NameOf(RoomNo))
        End Set
    End Property

    Public Property Beds As String
        Get
            Return Room.Beds
        End Get
        Set(value As String)
            Room.Beds = String.Join("", value.Where(Function(x) Char.IsDigit(x)).ToArray())
            OnPropertyChanged(NameOf(Beds))
        End Set
    End Property

    Public Property Price As Decimal
        Get
            Return Room.Price
        End Get
        Set(value As Decimal)
            Room.Price = value
            OnPropertyChanged(NameOf(Price))
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
