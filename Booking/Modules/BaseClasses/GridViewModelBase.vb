Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public MustInherit Class GridViewModelBase
    Implements INotifyPropertyChanged

    Public MustOverride Sub Load()
    Public MustOverride Sub OpenWindow()
    Public MustOverride Sub OpenNewWindow()


    Public Property OpenWindowCommand() As Command
    Public Property OpenNewCommand As Command

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub
End Class
