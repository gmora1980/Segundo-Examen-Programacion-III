' App_Code/Data/DatabaseHelper.vb
Imports System.Data.SqlClient
Imports System.Configuration

Public Class DatabaseHelper

    Public Shared Function GetConnection() As SqlConnection
        Dim connString As String = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
        Return New SqlConnection(connString)
    End Function

    Public Shared Function ExecuteNonQuery(query As String, params As SqlParameter()) As Integer
        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params)
                End If
                conn.Open()
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Shared Function ExecuteReader(query As String, params As SqlParameter()) As SqlDataReader
        Dim conn As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(query, conn)
        If params IsNot Nothing Then
            cmd.Parameters.AddRange(params)
        End If
        conn.Open()
        Return cmd.ExecuteReader()
    End Function

    Public Shared Function ExecuteScalar(query As String, params As SqlParameter()) As Object
        Using conn As SqlConnection = GetConnection()
            Using cmd As New SqlCommand(query, conn)
                If params IsNot Nothing Then
                    cmd.Parameters.AddRange(params)
                End If
                conn.Open()
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function

End Class