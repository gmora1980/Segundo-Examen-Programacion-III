Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data

Public Class ClienteRepository

    Private connectionString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString

    Public Function GetAll() As List(Of Cliente)
        Dim clientes As New List(Of Cliente)

        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand("SELECT ClienteId, Nombre, Apellidos, Telefono, Email FROM Clientes", conn)
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
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
            End Using
        End Using

        Return clientes
    End Function

    Public Sub Insert(cliente As Cliente)
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand("INSERT INTO Clientes (Nombre, Apellidos, Telefono, Email) VALUES (@Nombre, @Apellidos, @Telefono, @Email)", conn)
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre)
                cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos)
                cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono)
                cmd.Parameters.AddWithValue("@Email", cliente.Email)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Update(cliente As Cliente)
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand("UPDATE Clientes SET Nombre = @Nombre, Apellidos = @Apellidos, Telefono = @Telefono, Email = @Email WHERE ClienteId = @ClienteId", conn)
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre)
                cmd.Parameters.AddWithValue("@Apellidos", cliente.Apellidos)
                cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono)
                cmd.Parameters.AddWithValue("@Email", cliente.Email)
                cmd.Parameters.AddWithValue("@ClienteId", cliente.ClienteId)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Delete(clienteId As Integer)
        Using conn As New SqlConnection(connectionString)
            Using cmd As New SqlCommand("DELETE FROM Clientes WHERE ClienteId = @ClienteId", conn)
                cmd.Parameters.AddWithValue("@ClienteId", clienteId)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

End Class