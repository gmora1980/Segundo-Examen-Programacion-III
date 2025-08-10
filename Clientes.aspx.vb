' Clientes.aspx.vb
Partial Class Clientes
    Inherits System.Web.UI.Page

    Private repo As New ClienteRepository()
    Private Shared SelectedIndex As Integer
    Public Property lblMensaje As Object

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Usuario") Is Nothing Then
                Response.Redirect("Login.aspx")
            End If
            CargarClientes()
        End If
    End Sub

    Private Sub CargarClientes()
        Dim gvClientes As Object = Nothing
        gvClientes.DataSource = repo.GetAll()
        Clientes.DataBind()
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs, lblMensaje As Object, lblMensajeValidacion As Object, lblErrorNombre As Object)
        btnGuardar_Click(sender, e, lblMensaje, lblMensajeValidacion, lblErrorNombre, lblValidacionNombre)
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs, lblMensaje As Object, lblMensajeValidacion As Object, lblErrorNombre As Object, lblValidacionNombre As Object)
        Dim txtNombre As Object = Nothing
        ' Validaciones manuales (además de los validadores)
        If Not String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Dim txtApellidos As Object = Nothing

            If String.IsNullOrWhiteSpace(txtApellidos.Text) Then
                lblMensaje.Text = "Apellidos son obligatorios."
                lblMensaje.ForeColor = Drawing.Color.Red
                Return
            End If

            Dim txtTelefono As Object = Nothing

            If String.IsNullOrWhiteSpace(txtTelefono.Text) Then
                lblMensaje.Text = "Teléfono es obligatorio."
                lblMensaje.ForeColor = Drawing.Color.Red
                Return
            End If

            Dim txtEmail As Object = Nothing

            If String.IsNullOrWhiteSpace(txtEmail.Text) OrElse Not IsValidEmail(txtEmail.Text) Then
                lblMensaje.Text = "Email no válido."
                lblMensaje.ForeColor = Drawing.Color.Red
                Return
            End If

            Dim cliente As New Cliente()
            cliente.Nombre = Nothing.Text.Trim()
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
        Else
            lblNombreValidacion.Text = "Nombre es obligatorio."
            Dim lblNombreValidacion As Object = Nothing
            lblNombreValidacion.ForeColor = Drawing.Color.Red
            Return
        End If
    End Sub

    Protected Sub gvClientes_SelectedIndexChanged(sender As Object, e As EventArgs, gvClientes As Object)
        Dim clienteId As Integer = CInt(gvClientes.SelectedDataKey.Value)
        Dim cliente As Cliente = repo.GetAll().Find(Function(c) c.ClienteId = clienteId)

        If cliente IsNot Nothing Then
            Dim txtNombre As Object = Nothing
            txtNombre.Text = cliente.Nombre
            Dim txtApellidos As Object = Nothing
            txtApellidos.Text = cliente.Apellidos
            Dim txtTelefono As Object = Nothing
            txtTelefono.Text = cliente.Telefono
            Dim txtEmail As Object = Nothing
            txtEmail.Text = cliente.Email
            ViewState("ClienteId") = cliente.ClienteId
        End If
    End Sub

    Protected Sub gvClientes_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)
        Dim gvClientes As Object = Nothing
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
        Dim txtNombre As Object = Nothing
        txtNombre.Text = ""
        Dim txtApellidos As Object = Nothing
        txtApellidos.Text = ""
        Dim txtTelefono As Object = Nothing
        txtTelefono.Text = ""
        Dim txtEmail As Object = Nothing
        txtEmail.Text = ""
        ViewState("ClienteId") = Nothing
        Clientes.SelectedIndex = -1
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