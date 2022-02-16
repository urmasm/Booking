Public Class RoomService
    Implements IRoomService

    Public Async Function GetRooms() As Task(Of List(Of Room)) Implements IRoomService.GetRooms
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim srr = svc.GetRooms.Result
        Dim roomsList As New List(Of Room)
        For Each sr In srr
            Dim c As New Room With {
                .RoomID = sr.RoomID,
                .RoomNo = sr.RoomNo,
                .Beds = sr.Beds,
                .Price = sr.Price}
            roomsList.Add(c)
        Next
        svc.Close()
        Return roomsList
    End Function

    Public Async Function GetRoom(id As Integer) As Task(Of Room) Implements IRoomService.GetRoom
        Dim svc = New BookingServiceReference.BookingServiceSoapClient
        Dim sr = svc.GetRoom(id).Result.FirstOrDefault
        Dim r As Room
        If sr Is Nothing Then
            r = New Room With {
                    .RoomID = 0,
                    .RoomNo = "",
                    .Beds = 0,
                    .Price = 0}
        Else
            r = New Room With {
                    .RoomID = sr.RoomID,
                    .RoomNo = sr.RoomNo,
                    .Beds = sr.Beds,
                    .Price = sr.Price}
        End If

        svc.Close()
        Return r
    End Function

    Public Async Function SaveRoom(room As Room) As Task(Of Integer) Implements IRoomService.SaveRoom
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim r = New BookingServiceReference.Room With {
                .RoomID = room.RoomID,
                .RoomNo = room.RoomNo,
                .Beds = room.Beds,
                .Price = room.Price}
        Dim ID = svc.SaveRoom(r).Result.First
        svc.Close()
        Return ID
    End Function

    Public Async Function DeleteRoom(id As Integer) As Task Implements IRoomService.DeleteRoom
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        svc.DeleteRoom(id)
        svc.Close()
    End Function

    Public Async Function GetFreeRooms(startDate As Date, endDate As Date) As Task(Of List(Of Room)) Implements IRoomService.GetFreeRooms
        Dim svc = New BookingServiceReference.BookingServiceSoapClient()
        Dim srr = svc.GetFreeRooms(startDate, endDate).Result
        Dim roomsList As New List(Of Room)
        For Each sr In srr
            Dim c As New Room With {
                .RoomID = sr.RoomID,
                .RoomNo = sr.RoomNo,
                .Beds = sr.Beds,
                .Price = sr.Price}
            roomsList.Add(c)
        Next
        svc.Close()
        Return roomsList
    End Function
End Class
