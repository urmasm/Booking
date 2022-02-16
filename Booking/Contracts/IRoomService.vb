Public Interface IRoomService
    Function GetRooms() As Task(Of List(Of Room))
    Function GetFreeRooms(startDate As DateTime, endDate As DateTime) As Task(Of List(Of Room))
    Function GetRoom(id As Integer) As Task(Of Room)
    Function SaveRoom(room As Room) As Task(Of Integer)
    Function DeleteRoom(id As Integer) As Task
End Interface
