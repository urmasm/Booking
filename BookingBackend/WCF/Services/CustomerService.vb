Imports System.Data.SqlClient
Imports System.Threading.Tasks

Public Class CustomerService
    Implements ICustomerService

    Public Async Function GetCustomers() As Task(Of List(Of Customer)) Implements ICustomerService.GetCustomers
        Dim customersList As New List(Of Customer)
        Dim sql As String = "SELECT CustomerID, FirstName, LastName, IdNumber, Email FROM Customers"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            'Dim dtr = Await cmd.ExecuteReaderAsync
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim c As New Customer
                With c
                    .CustomerID = dtr(0)
                    .FirstName = dtr(1)
                    .LastName = dtr(2)
                    .IdNumber = dtr(3)
                    .Email = If(dtr(4).ToString = "", "", dtr(4))
                End With
                customersList.Add(c)
            End While
            cmd.Dispose()
        End Using
        Return customersList
    End Function

    Public Async Function GetCustomer(customerID As Integer) As Task(Of List(Of Customer)) Implements ICustomerService.GetCustomer
        Dim customersList As New List(Of Customer)
        Dim sql As String = "SELECT CustomerID, FirstName, LastName, IdNumber, Email FROM Customers WHERE CustomerID = @CustomerID"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@CustomerID", customerID)
            'For Each id In customerID
            'Dim dtr = Await cmd.ExecuteReaderAsync
            Dim dtr = cmd.ExecuteReader
            While dtr.Read
                Dim customer As New Customer
                With customer
                    .CustomerID = dtr(0)
                    .FirstName = dtr(1)
                    .LastName = dtr(2)
                    .IdNumber = dtr(3)
                    .Email = If(dtr(4).ToString = "", "", dtr(4))
                End With
                customersList.Add(customer)
            End While
            'Next

            cmd.Dispose()
        End Using
        Return customersList
    End Function

    Public Async Function SaveCustomer(customer As Customer) As Task(Of List(Of Integer)) Implements ICustomerService.SaveCustomer
        Dim updateSQL = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, IdNumber = @IdNumber, Email = @Email WHERE CustomerID = @CustomerID; SELECT @CustomerID"
        Dim insertSQL = "INSERT INTO Customers (FirstName, LastName, IdNumber, Email) VALUES (@FirstName, @LastName, @IdNumber, @Email); SELECT @@IDENTITY"
        Dim cID As Integer
        Dim lID As New List(Of Integer)
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand()
            cmd.Connection = conn
            With cmd.Parameters
                .AddWithValue("@CustomerID", customer.CustomerID)
                .AddWithValue("@FirstName", customer.FirstName)
                .AddWithValue("@LastName", customer.LastName)
                .AddWithValue("@IdNumber", customer.IdNumber)
                .AddWithValue("@Email", If(customer.Email Is Nothing, DBNull.Value, customer.Email))
            End With
            If customer.CustomerID > 0 Then
                cmd.CommandText = updateSQL
            Else
                cmd.CommandText = insertSQL
            End If
            'cID = Await cmd.ExecuteNonQueryAsync
            cID = cmd.ExecuteScalar()
            lID.Add(cID)
            cmd.Dispose()
        End Using
        Return lID
    End Function

    Public Function DeleteCustomer(customerID As Integer) As Task Implements ICustomerService.DeleteCustomer
        Dim sql = "DELETE FROM Customers WHERE CustomerID = @CustomerID"
        Dim conn = GetNewConnection()
        conn.Open()
        Using conn
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@CustomerID", customerID)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception

            End Try
        End Using
    End Function
End Class
