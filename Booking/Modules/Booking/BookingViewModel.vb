Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class BookingViewModel
    Implements INotifyPropertyChanged

    Dim _bookingService As IBookingService
    Private bvm As BookingGridViewModel
    Public Sub New(id As Integer, vm As BookingGridViewModel, bookingService As IBookingService)
        _bookingService = bookingService
        LoadBooking(id)
        SaveCommand = New Command(AddressOf SaveBooking)
        DeleteCommand = New Command(AddressOf DeleteBooking)
        FindFreeRoomsCommand = New Command(AddressOf LoadFreeRooms)
        bvm = vm
        CanClose = True
        FreeRoomsVisible = Visibility.Collapsed
        ReservationSummary = ReservationText(ReservationTextType.SelectBeds)
    End Sub

    Public Async Sub LoadBooking(id As Integer)
        Booking = Await _bookingService.GetBooking(id)
        Dim _roomService As New RoomService
        RoomsList = Await _roomService.GetRooms
        Dim _customerService As New CustomerService
        Dim ccl As New List(Of CustomersListItem)
        Dim cl = Await _customerService.GetCustomers
        cl.Sort(Function(x, y) x.LastName.CompareTo(y.LastName))
        For Each c In cl
            Dim nc As New CustomersListItem With {
                .CustomerID = c.CustomerID,
                .CustomerName = c.LastName & ", " & c.FirstName}
            ccl.Add(nc)
        Next
        CustomersList = ccl
    End Sub

    Public Async Sub SaveBooking()
        HasNoErrors = False
        CanClose = False
        If CustomerID = 0 Then
            NotificationText = "Klient on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If DateDiff(DateInterval.Day, ReservationStart, ReservationEnd) < 0 Then
            CanClose = False
            NotificationText = "Broneeringu lõpp ei saa olla enne algust."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        If RoomID = 0 Then
            NotificationText = "Tuba on nõutav."
            NotificationVisibility = Visibility.Visible
            Return
        End If
        HasNoErrors = True
        NotificationVisibility = Visibility.Collapsed

        CanClose = True
        Dim ID = Await _bookingService.SaveBooking(Booking)
            Booking.ReservationID = ID
            bvm.LoadBookings()

    End Sub

    Public Sub DeleteBooking()
        Dim errorString As String
        Dim dd = DateDiff(DateInterval.Day, Today.Date, ReservationStart)
        Select Case dd
            Case < 0
                errorString = "Valitud broneeringu algus on minevikus. Kustutamine katkestatud."
            Case 0 To 3
                errorString = "Valitud broneeringut alguseni on vähem kui kolm päeva. Kustutamine katkestatud."
            Case Else
                errorString = ""
        End Select
        If errorString.Length Then
            CanClose = False
            MessageBox.Show(errorString)
            Return
        Else
            CanClose = True
            If MessageBox.Show("Valitud reserveering kustutatakse jäädavalt.", "Kustutamine", MessageBoxButton.OKCancel, MessageBoxImage.Warning) = MessageBoxResult.OK Then
                _bookingService.DeleteBooking(BookingID)
                bvm.LoadBookings()
            End If
        End If

    End Sub

    Public Async Sub LoadFreeRooms()
        If String.IsNullOrEmpty(Beds) Then
            ReservationSummary = ReservationText(ReservationTextType.SelectBeds)
            FreeRoomsVisible = Visibility.Collapsed
        Else
            Dim _roomService As New RoomService
            Dim rl As New List(Of Room)
            rl = Await _roomService.GetFreeRooms(ReservationStart, ReservationEnd)
            Dim frl = rl.Where(Function(x) x.Beds = Beds).ToList
            frl.Sort(Function(x, y) x.Price.CompareTo(y.Price))
            RoomsList = frl

            If RoomsList.Count = 0 Then
                ReservationSummary = ReservationText(ReservationTextType.NoRooms)
                FreeRoomsVisible = Visibility.Collapsed
            End If
        End If

    End Sub

    Public Function ReservationText(type As ReservationTextType) As String
        Dim r As String = ""
        Select Case type
            Case ReservationTextType.SelectBeds
                r = "Sisesta soovitud voodite arv."
            Case ReservationTextType.SelectRoom
                r = "Vali sobiv tuba"
            Case ReservationTextType.NoRooms
                r = "Valitud ajavahemikus sobivat tuba ei ole."
            Case ReservationTextType.WrongDates
                r = "Vali sobiv ajavahemik."
            Case ReservationTextType.ReservatinonSummay
                r = "Broneeringu algus: " & ReservationStart & ", " & "Broneeringu lõpp: " &
ReservationStart & ", " & "Tuba number " & SelectedRoom.RoomNo & ", " & "Broneeringu maksumus: " & SelectedRoom.Price * DateDiff(DateInterval.Day, ReservationStart, ReservationEnd)
        End Select
        Return r
    End Function

    Public Property SaveCommand As Command
    Public Property DeleteCommand As Command
    Public Property FindFreeRoomsCommand As Command

    Private _reservationSummary As String
    Public Property ReservationSummary As String
        Get
            Return _reservationSummary
        End Get
        Set(value As String)
            _reservationSummary = value
            OnPropertyChanged(NameOf(ReservationSummary))
        End Set
    End Property

    Private _freeRoomsVisible As Visibility
    Public Property FreeRoomsVisible As Visibility
        Get
            Return _freeRoomsVisible
        End Get
        Set(value As Visibility)
            _freeRoomsVisible = value
            OnPropertyChanged(NameOf(FreeRoomsVisible))
        End Set
    End Property

    Private _selectedRoom As Room
    Public Property SelectedRoom As Room
        Get
            Return _selectedRoom
        End Get
        Set(value As Room)
            _selectedRoom = value
            OnPropertyChanged(NameOf(SelectedRoom))
            'ReservationSummaryVisible = Visibility.Visible
            Booking.RoomID = If(value Is Nothing, 0, value.RoomID)
            ReservationSummary = ReservationText(ReservationTextType.ReservatinonSummay)
        End Set
    End Property

    Dim _canClose As Boolean
    Public Property CanClose As Boolean
        Get
            Return _canClose
        End Get
        Set(value As Boolean)
            _canClose = value
            OnPropertyChanged(NameOf(CanClose))
        End Set
    End Property

    Private _roomsList As List(Of Room)
    Public Property RoomsList As List(Of Room)
        Get
            Return _roomsList
        End Get
        Set(value As List(Of Room))
            _roomsList = value
            OnPropertyChanged(NameOf(RoomsList))
        End Set
    End Property

    Private _customersList As List(Of CustomersListItem)
    Public Property CustomersList As List(Of CustomersListItem)
        Get
            Return _customersList
        End Get
        Set(value As List(Of CustomersListItem))
            _customersList = value
            OnPropertyChanged(NameOf(CustomersList))
        End Set
    End Property

    Private _booking As Reservation
    Public Property Booking As Reservation
        Get
            Return _booking
        End Get
        Set(value As Reservation)
            _booking = value
            OnPropertyChanged(NameOf(Booking))
            OnPropertyChanged(NameOf(BookingID))
            OnPropertyChanged(NameOf(RoomID))
            OnPropertyChanged(NameOf(CustomerID))
            OnPropertyChanged(NameOf(ReservationStart))
            OnPropertyChanged(NameOf(ReservationEnd))
            OnPropertyChanged(NameOf(PriceTotal))
        End Set
    End Property

    Public Property BookingID As Integer
        Get
            Return Booking.ReservationID
        End Get
        Set(value As Integer)
            Booking.ReservationID = value
            OnPropertyChanged(NameOf(BookingID))
        End Set
    End Property

    Public Property CustomerID As Integer
        Get
            Return Booking.CustomerID
        End Get
        Set(value As Integer)
            Booking.CustomerID = value
            OnPropertyChanged(NameOf(CustomerID))
        End Set
    End Property

    Public Property RoomID As Integer
        Get
            Return Booking.RoomID
        End Get
        Set(value As Integer)
            Booking.RoomID = value
            OnPropertyChanged(NameOf(RoomID))
            OnPropertyChanged(NameOf(PriceTotal))
            OnPropertyChanged(NameOf(ReservationSummary))
        End Set
    End Property

    Public Property ReservationStart As DateTime
        Get
            Return Booking.ReservationStart
        End Get
        Set(value As DateTime)
            Booking.ReservationStart = value
            OnPropertyChanged(NameOf(ReservationStart))
            OnPropertyChanged(NameOf(PriceTotal))
            OnPropertyChanged(NameOf(ReservationSummary))
            OnPropertyChanged(NameOf(SelectedRoom))
            If DateDiff(DateInterval.Day, ReservationStart, ReservationEnd) >= 0 Then
                LoadFreeRooms()
                ReservationSummary = ReservationText(ReservationTextType.SelectRoom)
            Else
                ReservationSummary = ReservationText(ReservationTextType.WrongDates)
            End If
        End Set
    End Property

    Public Property ReservationEnd As DateTime
        Get
            Return Booking.ReservationEnd
        End Get
        Set(value As DateTime)
            Booking.ReservationEnd = value
            OnPropertyChanged(NameOf(ReservationEnd))
            OnPropertyChanged(NameOf(PriceTotal))
            OnPropertyChanged(NameOf(ReservationSummary))
            OnPropertyChanged(NameOf(SelectedRoom))
            If DateDiff(DateInterval.Day, ReservationStart, ReservationEnd) >= 0 Then
                LoadFreeRooms()
                ReservationSummary = ReservationText(ReservationTextType.SelectRoom)
            Else
                ReservationSummary = ReservationText(ReservationTextType.WrongDates)
            End If
        End Set
    End Property

    Private _selectedStartDate As DateTime
    Public Property SelectedStartDate As DateTime
        Get
            Return _selectedStartDate
        End Get
        Set(value As DateTime)
            _selectedStartDate = value
            OnPropertyChanged(NameOf(SelectedStartDate))
        End Set
    End Property

    Private _selectedEndDate As DateTime
    Public Property SelectedEndDate As DateTime
        Get
            Return _selectedEndDate
        End Get
        Set(value As DateTime)
            _selectedEndDate = value
            OnPropertyChanged(NameOf(SelectedEndDate))
        End Set
    End Property

    Private _priceTotal As Decimal
    Public Property PriceTotal As Decimal
        Get
            Return RoomsList.Where(Function(x) x.RoomID = Booking.RoomID).FirstOrDefault.Price * DateDiff(DateInterval.Day, ReservationStart, ReservationEnd)
        End Get
        Set(value As Decimal)
            _priceTotal = value
            OnPropertyChanged(NameOf(PriceTotal))
        End Set
    End Property

    Private _beds As String
    Public Property Beds As String
        Get
            Return _beds
        End Get
        Set(value As String)
            _beds = String.Join("", value.Where(Function(x) Char.IsDigit(x)).ToArray())
            If Not String.IsNullOrEmpty(_beds) Then
                LoadFreeRooms()

                FreeRoomsVisible = Visibility.Visible
                ReservationSummary = ReservationText(ReservationTextType.SelectRoom)
                If RoomsList.Count = 0 Then
                    ReservationSummary = ReservationText(ReservationTextType.NoRooms)
                    FreeRoomsVisible = Visibility.Collapsed
                End If

            Else
                FreeRoomsVisible = Visibility.Collapsed
                ReservationSummary = ReservationText(ReservationTextType.SelectBeds)

            End If
            OnPropertyChanged(NameOf(Beds))
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

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

    Public Enum ReservationTextType
        SelectBeds
        SelectRoom
        NoRooms
        WrongDates
        ReservatinonSummay
    End Enum

    Public Class CustomersListItem
        Public Property CustomerID As Integer
        Public Property CustomerName As String
    End Class
End Class
