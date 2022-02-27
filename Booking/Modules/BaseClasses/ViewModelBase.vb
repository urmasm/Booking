Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public MustInherit Class ViewModelBase
    Implements INotifyPropertyChanged

    Public MustOverride Async Sub Load(id As Integer)
    Public MustOverride Async Sub Save()
    Public MustOverride Async Sub Delete()

    Public Property SaveCommand As Command
    Public Property DeleteCommand As Command

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
