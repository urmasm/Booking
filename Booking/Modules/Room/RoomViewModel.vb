Public Class RoomViewModel
    Inherits ViewModelBase

    Dim _roomService As IRoomService
    Private rvm As RoomGridViewModel
    Public Sub New(id As Integer, vm As RoomGridViewModel, roomService As IRoomService)
        _roomService = roomService
        Load(id)
        SaveCommand = New Command(AddressOf Save)
        DeleteCommand = New Command(AddressOf Delete)
        rvm = vm
    End Sub

    Public Overrides Async Sub Load(id As Integer)
        Room = Await _roomService.GetRoom(id)
    End Sub

    Public Overrides Async Sub Save()
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
        rvm.Load()
    End Sub

    Public Overrides Async Sub Delete()
        Await _roomService.DeleteRoom(RoomID)
        rvm.Load()
    End Sub

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

End Class
