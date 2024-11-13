Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Activacion_Correo
    Inherits System.Web.UI.Page
    Dim csreu As New reunion

    Private Sub Activacion_Correo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim strRes As String
            strRes = ""

            strRes = csreu.activar_correo_pendientes_reuniones(Request.QueryString("idcope"))

            If strRes = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Correo activado exitosamente...');", True)
                Dim script As String
                script = "window.close();"
                ScriptManager.RegisterStartupScript(Page, Page.[GetType](), "closewindows", script, True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Correo no fue activado...');", True)
            End If
        End If
    End Sub
End Class
