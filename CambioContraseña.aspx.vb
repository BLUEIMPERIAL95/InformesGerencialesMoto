Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient

Partial Class CambioContraseña
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim strRespuesta As String

        strRespuesta = validar_cambiar_nuevo(txtContraseña.Text, txtContraseña2.Text)

        If strRespuesta = "OK" Then
            Dim strRes As String

            strRes = csusua.guardar_contraseña_usuarios(Session("id_usua"), txtContraseña.Text)
            If strRes = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Contraseña Actualizada');", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Contraseña NO fue Actualizada');", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('" & strRespuesta & "');", True)
        End If
    End Sub

    Public Function validar_cambiar_nuevo(ByVal passnuevo As String, ByVal confirmarpass As String) As String
        Dim mensaje As String

        Dim caracteres = "ºª\!|@#·$%&/()=?¿¡,;.:-_<>[]{}*+"
        Dim contminus, contmayus, contnum, contcarac As Integer
        contminus = 0
        contmayus = 0
        contnum = 0
        contcarac = 0

        For i As Integer = 0 To passnuevo.Length - 1
            For j As Integer = 0 To caracteres.Length - 1
                If passnuevo(i).Equals(caracteres(j)) Then
                    contcarac = 1
                End If
            Next
            If (Asc(Mid(passnuevo, (i + 1), 1)) >= 65 And Asc(Mid(passnuevo, (i + 1), 1)) <= 90) Or (Mid(passnuevo, (i + 1), 1) = "Ñ") Then
                contmayus = contmayus + 1
            ElseIf Asc(Mid(passnuevo, (i + 1), 1)) >= 48 And Asc(Mid(passnuevo, (i + 1), 1)) <= 57 Then
                contnum = contnum + 1
            ElseIf Asc(Mid(passnuevo, (i + 1), 1)) >= 97 And Asc(Mid(passnuevo, (i + 1), 1)) <= 122 Then
                contminus = contminus + 1
            End If
        Next

        Dim passvalido = validar_pass(passnuevo, confirmarpass)

        If passnuevo = "" Then
            mensaje = "Ingresa la contrase;a que quiere asignar al usuario"
        ElseIf Session("cont") = passnuevo Then
            mensaje = "La contraseña no puede ser igual a la anterior"
        ElseIf Regex.IsMatch(passnuevo, "[a-z]") = False Then
            mensaje = "La contraseña debe contener al menos una letra minuscula"
        ElseIf Regex.IsMatch(passnuevo, "[A-Z]") = False Then
            mensaje = " La contraseña debe contener al menos una letra mayuscula"
        ElseIf Regex.IsMatch(passnuevo, "[0-9]") = False Then
            mensaje = " La contraseña debe contener al menos un numero"
        ElseIf contcarac = 0 Then
            mensaje = " La contraseña debe contener al menos un caracter especial"
        ElseIf passvalido <> "OK" Then
            mensaje = passvalido.ToString
        ElseIf passnuevo = "" Then
            mensaje = "Ingrese la nueva contraseña"
        ElseIf confirmarpass = "" Then
            mensaje = "Ingrese la confirmacion de la contraseña nueva"
        ElseIf passnuevo <> confirmarpass Then
            mensaje = "La nueva contraseña y la confirmacion no coinciden, favor verificar"
        ElseIf passnuevo.Length < 8 Then
            mensaje = "La contraseña debe contener 8 caracteres"
        ElseIf confirmarpass.Length < 8 Then
            mensaje = "La confirmación de contraseña debe contener 8 caracteres"
        Else
            mensaje = "OK"
        End If

        Return mensaje
    End Function

    Public Function validar_pass(ByVal pass As String, ByVal confirma As String) As String
        Dim mensaje As String

        If pass <> confirma Then
            mensaje = "La contraseña ingresada y la confirmacion no coinciden"
        Else
            mensaje = "OK"
        End If

        Return mensaje
    End Function
End Class
