Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports System.util
Imports DPFP
Imports DPFP.Capture
Imports System.Drawing
Partial Class RegistroEntrada
    Inherits System.Web.UI.Page
    Dim csequipos As New equipos
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim dtSuc As New DataTable
    Private Captura As DPFP.Capture.Capture

    Private Sub RegistroEntrada_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2038, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            txtDocumento.Focus()
            combos()
            Llenar_Grid()
        End If
    End Sub

    Sub combos()
        Dim dtare, dteps, dtarl As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_areas_combo", dtare, cboArea)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_eps_combo", dteps, cboEps)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_arl_combo", dtarl, cboArl)
    End Sub

    Sub Llenar_Grid()
        Dim dtreg As New DataTable
        Dim strRespuestaPerVer As String

        strRespuestaPerVer = csusua.validar_permiso_usuario(2043, Session("id_usua"))

        If strRespuestaPerVer <> "" Then
            dtreg = csequipos.capturar_datos_registro_entrada_diario(Session("empresa"))
        Else
            dtreg = csequipos.capturar_datos_registro_entrada_diario("0")
        End If

        gridRegistro.DataSource = dtreg
        gridRegistro.DataBind()
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If cboTipo.SelectedValue = "-1" Or txtDocumento.Text = "" Or txtNombre.Text = "" Or txtTelefono.Text = "" _
            Or txtCorreo.Text = "" Or csoper.IsValidEmail(txtCorreo.Text) = False Or cboEps.SelectedValue = "0" _
            Or cboArl.SelectedValue = "0" Or cboArea.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Todos los campos con * son obligarios o validar que sea un correo válido.');", True)
            Else
                Dim strRespuestaReg As String
                If hidtercero.Value = 0 Then
                    Dim strRespuesta, strEmpresa As String

                    If Session("codEmpr") = "1" Then
                        strEmpresa = "MOTOTRANSPORTAMOS"
                    Else
                        If Session("codEmpr") = "2" Then
                            strEmpresa = "MOTOTRANSPORTAR"
                        Else
                            If Session("codEmpr") = "3" Then
                                strEmpresa = "REFRILOGISTICA"
                            Else
                                If Session("codEmpr") = "4" Then
                                    strEmpresa = "TRAMITAR"
                                Else
                                    If Session("codEmpr") = "5" Then
                                        strEmpresa = "CIACAPRI"
                                    Else
                                        strEmpresa = "MOTOSEGURIDAD"
                                    End If
                                End If
                            End If
                        End If
                    End If

                    strRespuesta = csequipos.guardar_terceros(0, cboTipo.SelectedValue, txtDocumento.Text, 0, txtNombre.Text, "", "", "", "", "", "", "", 1, strEmpresa, "PRINCIPAL", 0, 0, 1, "Tercero")

                    If strRespuesta = "" Then
                        Dim btValDatos As Integer
                        Capturar_id_tercero(txtDocumento.Text)

                        If chkpoliticas.Checked Then
                            btValDatos = 1
                        Else
                            btValDatos = 0
                        End If
                        strRespuestaReg = csequipos.guardar_registro_entrada(hidtercero.Value, Replace(txtTemperatura.Text, ".", ","), cboArea.SelectedValue, txtTelefono.Text, txtCorreo.Text, cboEps.SelectedValue, cboArl.SelectedValue, btValDatos)

                        If strRespuestaReg = "" Then
                            Limpiar()
                            Llenar_Grid()
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción exitosa...');", True)
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro no pudo ser salvado...');", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero no pudo ser salvado...');", True)
                    End If
                Else
                    Dim btValDatos As Integer

                    If chkpoliticas.Checked Then
                        btValDatos = 1
                    Else
                        btValDatos = 0
                    End If
                    strRespuestaReg = csequipos.guardar_registro_entrada(hidtercero.Value, Replace(txtTemperatura.Text, ".", ","), cboArea.SelectedValue, txtTelefono.Text, txtCorreo.Text, cboEps.SelectedValue, cboArl.SelectedValue, btValDatos)

                    If strRespuestaReg = "" Then
                        Limpiar()
                        Llenar_Grid()
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción exitosa...');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro no pudo ser salvado...');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub txtDocumento_TextChanged(sender As Object, e As EventArgs) Handles txtDocumento.TextChanged
        Try
            Dim dtter, dtreg As New DataTable

            dtter = csequipos.capturar_datos_terceros_por_documento(txtDocumento.Text)

            If dtter.Rows.Count > 0 Then
                hidtercero.Value = dtter.Rows(0)("id")
                cboTipo.SelectedValue = dtter.Rows(0)("tipodoc").ToString
                txtNombre.Text = dtter.Rows(0)("nombre").ToString
                txtTelefono.Text = dtter.Rows(0)("telefono").ToString
                txtCorreo.Text = dtter.Rows(0)("correo").ToString
                cboEps.SelectedValue = dtter.Rows(0)("id_eps").ToString
                cboArl.SelectedValue = dtter.Rows(0)("id_arl").ToString
                If dtter.Rows(0)("val_datos").ToString = "0" Then
                    chkpoliticas.Checked = False
                Else
                    chkpoliticas.Checked = True
                End If

                dtreg = csequipos.seleccionar_registro_entrada(hidtercero.Value)

                If dtreg.Rows.Count Then
                    'txtTelefono.Text = dtreg.Rows(0)("telefono_REEN").ToString
                    'txtCorreo.Text = dtreg.Rows(0)("correo_REEN").ToString
                    'cboEps.SelectedValue = dtreg.Rows(0)("id_eps").ToString
                    'cboArl.SelectedValue = dtreg.Rows(0)("id_arl").ToString
                    cboArea.SelectedValue = dtreg.Rows(0)("id_area").ToString
                End If
            Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero no existe en bd. Debe crearlo.');", True)
                txtNombre.Enabled = True
                txtNombre.Focus()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Limpiar()
        hidtercero.Value = 0
        cboTipo.SelectedValue = "Cedula de Ciudadania"
        txtDocumento.Text = ""
        txtNombre.Text = ""
        txtTemperatura.Text = "0"
        txtDocumento.Focus()
        txtTelefono.Text = ""
        txtCorreo.Text = ""
        cboEps.SelectedValue = "0"
        cboArl.SelectedValue = "0"
        cboArea.SelectedValue = "0"
        chkpoliticas.Checked = False
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub

    Private Sub Capturar_id_tercero(ByVal doc As String)
        Try
            Dim dtter As New DataTable

            dtter = csequipos.capturar_datos_terceros_por_documento(doc)

            If dtter.Rows.Count > 0 Then
                hidtercero.Value = dtter.Rows(0)("id")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub imgPdf_Click(sender As Object, e As ImageClickEventArgs) Handles imgPdf.Click
        Dim ruta = "C:\Inetpub\wwwroot\InformesGerencialesMoto\politicas.pdf"
        'Dim ruta = "D:\Luis Guillermo Uribe\Documents\Visual Studio 2010\WebSites\InformesGerencialesMoto\politicas.pdf"
        Try
            Response.Clear()
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-disposition", "attachment; filename=" & "Politicas.pdf")
            Response.WriteFile(ruta)
            Response.Flush()
            Response.Close()
        Catch Exp As Exception

        End Try
    End Sub
End Class
