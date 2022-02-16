Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class BookingGridViewModel
    Implements INotifyPropertyChanged

    Dim _bookingService As IBookingService

    Public Sub New()
        _bookingService = New BookingService
        LoadBookings()
        OpenBookingCommand = New Command(AddressOf OpenBookingWindow)
        NewBookingCommand = New Command(AddressOf OpenNewBookingWindow)
    End Sub

    Public Sub New(bookingService As IBookingService)
        _bookingService = bookingService
        LoadBookings()
        OpenBookingCommand = New Command(AddressOf OpenBookingWindow)
        NewBookingCommand = New Command(AddressOf OpenNewBookingWindow)
    End Sub

    Public Sub LoadBookings()
        Bookings = _bookingService.GetBookingsForGrid.Result
    End Sub

    Private Sub OpenBookingWindow()
        If Not SelectedBooking Is Nothing Then
            Dim viewmodel As New BookingViewModel(SelectedBooking.ReservationID, Me, _bookingService)
            Dim view As New BookingView
            view.DataContext = viewmodel
            view.ShowDialog()
        End If
    End Sub

    Private Sub OpenNewBookingWindow()
        Dim viewmodel As New BookingViewModel(0, Me, _bookingService)
        Dim view As New NewBookingView
        view.DataContext = viewmodel
        view.ShowDialog()
    End Sub

    Public Property OpenBookingCommand() As Command
    Public Property NewBookingCommand As Command

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

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class
