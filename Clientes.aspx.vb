' Clientes.aspx.vb
Partial Class Clientes
    Inherits System.Web.UI.Page

    Private repo As New ClienteRepository()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Usuario") Is Nothing Then
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
        ' Validaciones manuales (además de los validadores)
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            lblMensaje.Text = "Nombre es obligatorio."
            lblMensaje.ForeColor = Drawing.Color.Red
            Return
        End If

        If String.IsNullOrWhiteSpace(txtApellidos.Text) Then
            lblMensaje.Text = "Apellidos son obligatorios."
            lblMensaje.ForeColor = Drawing.Color.Red
            Return
        End If

        If String.IsNullOrWhiteSpace(txtTelefono.Text) Then
            lblMensaje.Text = "Teléfono es obligatorio."
            lblMensaje.ForeColor = Drawing.Color.Red
            Return
        End If

        If String.IsNullOrWhiteSpace(txtEmail.Text) OrElse Not IsValidEmail(txtEmail.Text) Then
            lblMensaje.Text = "Email no válido."
            lblMensaje.ForeColor = Drawing.Color.Red
            Return
        End If

        Dim cliente As New Cliente()
        cliente.Nombre = txtNombre.Text.Trim()
        cliente.Apellidos = txtApellidos.Text.Trim()
        cliente.Telefono = txtTelefono.Text.Trim()
        cliente.Email = txtEmail.Text.Trim()

        Dim clienteId As Integer = 0
        If Integer.TryParse(ViewState("ClienteId"), clienteId) AndAlso clienteId > 0 Then
            cliente.ClienteId = clienteId
            repo.Update(cliente)
            lblMensaje.Text = "Cliente actualizado correctamente."
        Else
            repo.Insert(cliente)
            lblMensaje.Text = "Cliente guardado correctamente."
        End If

        lblMensaje.ForeColor = Drawing.Color.Green
        LimpiarFormulario()
        CargarClientes()
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
            lblMensaje.Text = "Cliente eliminado correctamente."
            lblMensaje.ForeColor = Drawing.Color.Green

            ' Si se está eliminando el cliente en edición
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

    Private Function IsValidEmail(email As String) As Boolean
        Try
            Dim addr As New System.Net.Mail.MailAddress(email)
            Return addr.Address = email
        Catch
            Return False
        End Try
    End Function

End Class