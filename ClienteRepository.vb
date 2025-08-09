' App_Code/Data/ClienteRepository.vb
Imports System.Data.SqlClient
Imports System.Collections.Generic

Public Class ClienteRepository

    Public Function GetAll() As List(Of Cliente)
        Dim clientes As New List(Of Cliente)
        Dim query As String = "SELECT ClienteId, Nombre, Apellidos, Telefono, Email FROM Clientes"

        Using reader As SqlDataReader = DatabaseHelper.ExecuteReader(query, Nothing)
            While reader.Read()
                clientes.Add(New Cliente() With {
                    .ClienteId = Convert.ToInt32(reader("ClienteId")),
                    .Nombre = reader("Nombre").ToString(),
                    .Apellidos = reader("Apellidos").ToString(),
                    .Telefono = reader("Telefono").ToString(),
                    .Email = reader("Email").ToString()
                })
            End While
        End Using

        Return clientes
    End Function

    Public Sub Insert(cliente As Cliente)
        Dim query As String = "INSERT INTO Clientes (Nombre, Apellidos, Telefono, Email) VALUES (@Nombre, @Apellidos, @Telefono, @Email)"
        Dim parameters() As SqlParameter = {
            New SqlParameter("@Nombre", cliente.Nombre),
            New SqlParameter("@Apellidos", cliente.Apellidos),
            New SqlParameter("@Telefono", cliente.Telefono),
            New SqlParameter("@Email", cliente.Email)
        }
        DatabaseHelper.ExecuteNonQuery(query, parameters)
    End Sub

    Public Sub Update(cliente As Cliente)
        Dim query As String = "UPDATE Clientes SET Nombre = @Nombre, Apellidos = @Apellidos, Telefono = @Telefono, Email = @Email WHERE ClienteId = @ClienteId"
        Dim parameters() As SqlParameter = {
            New SqlParameter("@Nombre", cliente.Nombre),
            New SqlParameter("@Apellidos", cliente.Apellidos),
            New SqlParameter("@Telefono", cliente.Telefono),
            New SqlParameter("@Email", cliente.Email),
            New SqlParameter("@ClienteId", cliente.ClienteId)
        }
        DatabaseHelper.ExecuteNonQuery(query, parameters)
    End Sub

    Public Sub Delete(clienteId As Integer)
        Dim query As String = "DELETE FROM Clientes WHERE ClienteId = @ClienteId"
        Dim parameters() As SqlParameter = {
            New SqlParameter("@ClienteId", clienteId)
        }
        DatabaseHelper.ExecuteNonQuery(query, parameters)
    End Sub

End Class