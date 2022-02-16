Imports System.Data.SqlClient
Imports System.Threading.Tasks

Public Class RoomService
    Implements IRoomService

    Public Async Function GetRooms() As Task(Of List(Of Room)) Implements IRoomService.GetRooms
        Dim roomsList As New List(Of Room)
        Dim sql As String = "SELECT RoomID, RoomNo, Beds, Price FROM Rooms"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim room As New Room
                With room
                    .RoomID = dtr(0)
                    .RoomNo = dtr(1)
                    .Beds = dtr(2)
                    .Price = dtr(3)
                End With
                roomsList.Add(room)
            End While
            cmd.Dispose()
        End Using
        Return roomsList
    End Function

    Public Async Function GetRoom(roomID As Integer) As Task(Of List(Of Room)) Implements IRoomService.GetRoom
        Dim roomsList As New List(Of Room)
        Dim sql As String = "SELECT RoomID, RoomNo, Beds, Price FROM Rooms WHERE RoomID = @RoomID"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@RoomID", roomID)
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim room As New Room
                With room
                    .RoomID = dtr(0)
                    .RoomNo = dtr(1)
                    .Beds = dtr(2)
                    .Price = dtr(3)
                End With
                roomsList.Add(room)
            End While
            cmd.Dispose()
        End Using
        Return roomsList
    End Function

    Public Async Function GetFreeRooms(startDate As Date, endDate As Date) As Task(Of List(Of Room)) Implements IRoomService.GetFreeRooms
        Dim roomsList As New List(Of Room)
        Dim sql = "SELECT RoomID, RoomNo, Beds, Price FROM Rooms
                   WHERE (RoomID NOT IN
                   (SELECT RoomID FROM Reservations WHERE(ReservationStart BETWEEN @ReservationStart AND @ReservationEnd) 
                   OR (ReservationEnd BETWEEN @ReservationStart AND @ReservationEnd) 
                   OR (ReservationStart >= @ReservationStart) AND (ReservationEnd <= @ReservationEnd) 
                   OR (ReservationStart <= @ReservationStart) AND (ReservationEnd >= @ReservationEnd)))"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@ReservationStart", startDate)
            cmd.Parameters.AddWithValue("@ReservationEnd", endDate)
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim room As New Room
                With room
                    .RoomID = dtr(0)
                    .RoomNo = dtr(1)
                    .Beds = dtr(2)
                    .Price = dtr(3)
                End With
                roomsList.Add(room)
            End While
            cmd.Dispose()
        End Using
        Return roomsList
    End Function

    Public Async Function SaveRoom(room As Room) As Task(Of List(Of Integer)) Implements IRoomService.SaveRoom
        Dim updateSQL = "UPDATE Rooms SET RoomNo = @RoomNo, Beds = @Beds, Price = @Price WHERE RoomID = @RoomID; SELECT @RoomID"
        Dim insertSQL = "INSERT INTO Rooms (RoomNo, Beds, Price) VALUES (@RoomNo, @Beds, @Price); SELECT @@IDENTITY"
        Dim rID As Integer
        Dim lID As New List(Of Integer)
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            With cmd.Parameters
                .AddWithValue("@RoomID", room.RoomID)
                .AddWithValue("@RoomNo", room.RoomNo)
                .AddWithValue("@Beds", room.Beds)
                .AddWithValue("@Price", room.Price)
            End With
            If room.RoomID > 0 Then
                cmd.CommandText = updateSQL
            Else
                cmd.CommandText = insertSQL
            End If
            rID = cmd.ExecuteScalar()
            lID.Add(rID)
            cmd.Dispose()
        End Using
        Return lID
    End Function

    Public Function DeleteRoom(roomID As Integer) As Task Implements IRoomService.DeleteRoom
        Dim sql = "DELETE FROM Rooms WHERE RoomID = @RoomID"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@RoomID", roomID)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Function
End Class
