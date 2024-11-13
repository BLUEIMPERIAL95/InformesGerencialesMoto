
Partial Class Site
    Inherits System.Web.UI.MasterPage

    Private Sub Site_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            lblUsuario.Text = Session("nombre")
            lblEmpresa.Text = Session("empresa")
        End If
    End Sub
End Class

