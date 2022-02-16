Imports System.ServiceModel
Imports System.Threading.Tasks

<ServiceContract()>
Public Interface IRoomService
    <OperationContract()>
    Function GetRooms() As Task(Of List(Of Room))
    <OperationContract()>
    Function GetRoom(roomID As Integer) As Task(Of List(Of Room))
    <OperationContract()>
    Function GetFreeRooms(startDate As DateTime, endDate As DateTime) As Task(Of List(Of Room))
    <OperationContract()>
    Function SaveRoom(room As Room) As Task(Of List(Of Integer))
    <OperationContract()>
    Function DeleteRoom(roomID As Integer) As Task
End Interface
