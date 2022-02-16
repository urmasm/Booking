Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Class MainWindowVM
    Implements INotifyPropertyChanged

    Public Sub New()
        CustomersCommand = New Command(AddressOf ShowCustomersGrid)
        RoomsCommand = New Command(AddressOf ShowRoomsGrid)
        BookingsCommand = New Command(AddressOf ShowBookingsGrid)
    End Sub


    Public Sub ShowCustomersGrid()
        SelectedViewModel = New CustomerGridViewModel
    End Sub

    Public Sub ShowRoomsGrid()
        SelectedViewModel = New RoomGridViewModel
    End Sub

    Public Sub ShowBookingsGrid()
        SelectedViewModel = New BookingGridViewModel
    End Sub

    Public Property CustomersCommand As Command
    Public Property RoomsCommand As Command
    Public Property BookingsCommand As Command

    Private _selectedViewModel As Object
    Public Property SelectedViewModel As Object
        Get
            Return _selectedViewModel
        End Get
        Set(value As Object)
            _selectedViewModel = value
            OnPropertyChanged(NameOf(SelectedViewModel))
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class
