Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim usuario As String = txtUsuario.Text.Trim()
        Dim password As String = txtPassword.Text.Trim()

        ' Simulación de autenticación (puedes conectar a DB después)
        If usuario = "admin" AndAlso password = "1234" Then
            FormsAuthentication.SetAuthCookie(usuario, False)
            Response.Redirect(Request.QueryString("ReturnUrl") Or "Clientes.aspx")
        Else
            lblError.Text = "Usuario o contraseña incorrectos."
        End If
    End Sub
End Class