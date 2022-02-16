Imports System.Data.SqlClient

Module DataConnections
    Public Function GetConnectionString() As String
        Return System.Configuration.ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString
    End Function
    Public Function GetNewConnection() As SqlConnection
        Return New SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("BookingConnectionString").ConnectionString)
    End Function
End Module
