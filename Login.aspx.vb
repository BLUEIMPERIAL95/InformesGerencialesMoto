Imports Microsoft.VisualBasic
Imports System.Data
Imports MySql.Data.MySqlClient

Partial Class Login
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub OkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OkButton.Click
        Try
            If txtLogin.Text <> "" Or txtPass.Text <> "" Then
                Dim dtusuarios As New DataTable

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
                            If cboempresa.SelectedValue = 1 Then
                                Session("empresa") = "MOTOTRANSPORTAMOS"
                                Session("codEmpr") = "1"
                            Else
                                If cboempresa.SelectedValue = 2 Then
                                    Session("empresa") = "MOTOTRANSPORTAR"
                                    Session("codEmpr") = "2"
                                Else
                                    If cboempresa.SelectedValue = 3 Then
                                        Session("empresa") = "REFRILOGISTICA"
                                        Session("codEmpr") = "3"
                                    Else
                                        If cboempresa.SelectedValue = 4 Then
                                            Session("empresa") = "TRAMITAR"
                                            Session("codEmpr") = "4"
                                        Else
                                            If cboempresa.SelectedValue = 5 Then
                                                Session("empresa") = "CIACAPRI"
                                                Session("codEmpr") = "5"
                                            Else
                                                If cboempresa.SelectedValue = 6 Then
                                                    Session("empresa") = "MOTOSEGURIDAD"
                                                    Session("codEmpr") = "6"
                                                Else
                                                    If cboempresa.SelectedValue = 7 Then
                                                        Session("empresa") = "TRAMITAR LINEA"
                                                        Session("codEmpr") = "7"
                                                    Else
                                                        Session("empresa") = "CEFATRANS SAS"
                                                        Session("codEmpr") = "8"
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            ConfigurationManager.AppSettings("bdsel") = cboempresa.SelectedValue
                            Response.Redirect("Default.aspx")
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error Insertando Session...');", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Credenciales Inválidas...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('" & ex.Message & "');", True)
        End Try
    End Sub
End Class
