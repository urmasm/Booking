Public Class BookingGridViewModel
    Inherits GridViewModelBase

    Dim _bookingService As IBookingService
    Public Sub New()
        _bookingService = New BookingService
        Load()
        OpenWindowCommand = New Command(AddressOf OpenWindow)
        OpenNewCommand = New Command(AddressOf OpenNewWindow)
    End Sub

    Public Sub New(bookingService As IBookingService)
        _bookingService = bookingService
        Load()
        OpenWindowCommand = New Command(AddressOf OpenWindow)
        OpenNewCommand = New Command(AddressOf OpenNewWindow)
    End Sub

    Public Overrides Sub Load()
        Bookings = _bookingService.GetBookingsForGrid.Result
    End Sub

    Public Overrides Sub OpenWindow()
        If Not SelectedBooking Is Nothing Then
            Dim viewmodel As New BookingViewModel(SelectedBooking.ReservationID, Me, _bookingService)
            Dim view As New BookingView
            view.DataContext = viewmodel
            view.ShowDialog()
        End If
    End Sub

    Public Overrides Sub OpenNewWindow()
        Dim viewmodel As New BookingViewModel(0, Me, _bookingService)
        Dim view As New NewBookingView
        view.DataContext = viewmodel
        view.ShowDialog()
    End Sub

    Private _bookings As List(Of BookingGridDto)
    Public Property Bookings As List(Of BookingGridDto)
        Get
            Return _bookings
        End Get
        Set(value As List(Of BookingGridDto))
            _bookings = value
            OnPropertyChanged(NameOf(Bookings))
        End Set
    End Property

    Private _selectedBooking As BookingGridDto
    Property SelectedBooking As BookingGridDto
        Get
            Return _selectedBooking
        End Get
        Set(value As BookingGridDto)
            _selectedBooking = value
            OnPropertyChanged(NameOf(SelectedBooking))
        End Set
    End Property

End Class
