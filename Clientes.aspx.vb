Imports System.Data.SqlClient

Partial Class Clientes
    Inherits System.Web.UI.Page

    Private repo As New ClienteRepository()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.IsAuthenticated Then
                Response.Redirect("Login.aspx")
            End If
            CargarClientes()
        End If
    End Sub

    Private Sub CargarClientes()
        gvClientes.DataSource = repo.GetAll()
        gvClientes.DataBind()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        If Not Page.IsValid Then Return

        Dim cliente As New Cliente()
        cliente.Nombre = txtNombre.Text.Trim()
        cliente.Apellidos = txtApellidos.Text.Trim()
        cliente.Telefono = txtTelefono.Text.Trim()
        cliente.Email = txtEmail.Text.Trim()

        Dim clienteId As Integer = If(ViewState("ClienteId") IsNot Nothing, CInt(ViewState("ClienteId")), 0)

        Try
            If clienteId = 0 Then
                ' INSERT
                repo.Insert(cliente)
                lblMensaje.Text = "Cliente creado exitosamente."
            Else
                ' UPDATE
                cliente.ClienteId = clienteId
                repo.Update(cliente)
                lblMensaje.Text = "Cliente actualizado exitosamente."
                ViewState("ClienteId") = Nothing
            End If

            LimpiarFormulario()
            CargarClientes()

        Catch ex As SqlException
            If ex.Number = 2627 Or ex.Number = 2601 Then ' Violación de unicidad
                lblMensaje.Text = "Error: El email ya está registrado."
                lblMensaje.ForeColor = Drawing.Color.Red
            Else
                lblMensaje.Text = "Error en la base de datos: " & ex.Message
                lblMensaje.ForeColor = Drawing.Color.Red
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error: " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try
    End Sub

    Protected Sub gvClientes_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim clienteId As Integer = CInt(gvClientes.SelectedDataKey.Value)
        Dim cliente As Cliente = repo.GetAll().Find(Function(c) c.ClienteId = clienteId)

        If cliente IsNot Nothing Then
            txtNombre.Text = cliente.Nombre
            txtApellidos.Text = cliente.Apellidos
            txtTelefono.Text = cliente.Telefono
            txtEmail.Text = cliente.Email
            ViewState("ClienteId") = cliente.ClienteId
        End If
    End Sub

    Protected Sub gvClientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim clienteId As Integer = CInt(gvClientes.DataKeys(e.RowIndex).Value)

        Try
            repo.Delete(clienteId)
            lblMensaje.Text = "Cliente eliminado."
            If ViewState("ClienteId") IsNot Nothing AndAlso CInt(ViewState("ClienteId")) = clienteId Then
                LimpiarFormulario()
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error al eliminar: " & ex.Message
            lblMensaje.ForeColor = Drawing.Color.Red
        End Try

        CargarClientes()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)
        LimpiarFormulario()
    End Sub

    Private Sub LimpiarFormulario()
        txtNombre.Text = ""
        txtApellidos.Text = ""
        txtTelefono.Text = ""
        txtEmail.Text = ""
        ViewState("ClienteId") = Nothing
        gvClientes.SelectedIndex = -1
        lblMensaje.Text = ""
    End Sub

End Class