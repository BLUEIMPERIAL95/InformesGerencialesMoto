Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient
Partial Class Login_Cartas
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csterc As New equipos

    Private Sub OkButton_Click(sender As Object, e As EventArgs) Handles OkButton.Click
        Try
            If txtLogin.Text <> "" Or txtPass.Text <> "" Then
                Dim dtusuarios, dtcer As New DataTable

                'dtcer = csterc.seleccionar_datos_certificados_por_documento(txtLogin.Text)

                'If dtcer.Rows.Count > 0 Then
                dtusuarios = csusua.capturar_datos_usuarios_por_cedula(txtLogin.Text)

                If dtusuarios.Rows.Count > 0 Then
                    Dim strCedula, strContraseña, strNombre As String
                    Dim intUsua As Integer

                    intUsua = dtusuarios.Rows(0)("id_usua").ToString
                    strCedula = dtusuarios.Rows(0)("strdoc_usua").ToString
                    strContraseña = dtusuarios.Rows(0)("strcon_usua").ToString
                    strNombre = dtusuarios.Rows(0)("nombre").ToString

                    If strCedula = txtLogin.Text And strContraseña = txtPass.Text Then
                        Dim strRespuesta As String

                        strRespuesta = csusua.insertar_session_usuarios(intUsua, Session.SessionID)

                        If strRespuesta = "" Then
                            Session("id_usua") = intUsua
                            Session("usua") = txtLogin.Text
                            Session("nombre") = strNombre
                            Session("documento") = strCedula

                            Response.Redirect("Generacion_Cartas.aspx")
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error Insertando Session...');", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)
                    End If
                Else
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)

                    Dim strRes, strDoc, strNom As String

                    dtcer = csusua.seleccionar_retiro_empresa_por_documento(txtLogin.Text)

                    If dtcer.Rows.Count > 0 Then
                        strDoc = txtLogin.Text
                        strNom = dtcer.Rows(0)("nombre_reem").ToString

                        strRes = csusua.guardar_usuarios(0, 1, strDoc, strNom, "0", "0", "C", "C", "2021-01-01", strDoc, "cert" & strDoc, 1)

                        If strRes = "" Then
                            dtusuarios = csusua.capturar_datos_usuarios_por_cedula(txtLogin.Text)

                            If dtusuarios.Rows.Count > 0 Then
                                Dim strCedula, strContraseña, strNombre As String
                                Dim intUsua As Integer

                                intUsua = dtusuarios.Rows(0)("id_usua").ToString
                                strCedula = dtusuarios.Rows(0)("strdoc_usua").ToString
                                strContraseña = dtusuarios.Rows(0)("strcon_usua").ToString
                                strNombre = dtusuarios.Rows(0)("nombre").ToString

                                If strCedula = txtLogin.Text And strContraseña = txtPass.Text Then
                                    Dim strRespuesta As String

                                    strRespuesta = csusua.insertar_session_usuarios(intUsua, Session.SessionID)

                                    If strRespuesta = "" Then
                                        Session("id_usua") = intUsua
                                        Session("usua") = txtLogin.Text
                                        Session("nombre") = strNombre
                                        Session("documento") = strCedula

                                        Response.Redirect("Generacion_Cartas.aspx")
                                    Else
                                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error Insertando Session...');", True)
                                    End If
                                Else
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)
                                End If
                            End If
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Usuario no pudo ser registrado. Consultar administrador de sistema...');", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Documento no existe en la base de datos...');", True)
                    End If
                End If
                'Else
                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('El documento digitado no tiene certificados a generar para ningún periódo...');", True)
                'End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('" & ex.Message & "');", True)
        End Try
    End Sub
End Class
