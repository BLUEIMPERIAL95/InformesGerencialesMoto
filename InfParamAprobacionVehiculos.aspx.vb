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
Partial Class InfParamAprobacionVehiculos
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(20, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        'combos()
    End Sub

    'Sub combos()
    '    Dim dtusu As New DataTable

    '    csoper.LlenarDropDownList("nombrecompleto", "id", "usuarios_mostrar", dtusu, cbousuarios)
    'End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim strSQL As String
                Dim dtveh As New DataTable

                divmostrar.InnerHtml = ""

                strSQL = " Select CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Usuario,"
                strSQL += " log_transacciones.codigo_registro As placa,"
                strSQL += " log_transacciones.fecha,"
                strSQL += " log_transacciones.hora,"
                strSQL += " log_transacciones.descripcion"
                strSQL += " From log_transacciones"
                strSQL += " Left Join usuarios ON usuarios.idusuarios = log_transacciones.usuarios_idusuarios"
                strSQL += " Left Join terceros ON terceros.idterceros = usuarios.idterceros"
                strSQL += " Where log_transacciones.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += " And log_transacciones.sistema_modulos_idmodulos = 31 "
                'If cbousuarios.SelectedValue <> "0" Then
                '    strSQL += " And log_transacciones.usuarios_idusuarios = " & cbousuarios.SelectedValue & " "
                'End If
                strSQL += " And log_transacciones.descripcion Like '%APROBADO%'"
                strSQL += " ORDER BY log_transacciones.fecha, log_transacciones.hora"

                dtveh = csinformes.ejecutar_query_bd(strSQL)

                If dtveh.Rows.Count > 0 Then
                    Dim strHtml As String

                    strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>Usuario</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Hora</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>Descripcion</font></b></td>"
                    strHtml += "</tr>"

                    For i As Integer = 0 To dtveh.Rows.Count - 1
                        strHtml += "<tr>"

                        strHtml += "<td align='left' colspan='2'><font size='1px'>" & dtveh.Rows(i)("Usuario").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtveh.Rows(i)("placa").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtveh.Rows(i)("fecha").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtveh.Rows(i)("hora").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='1px'>" & dtveh.Rows(i)("descripcion").ToString & "</font></td>"

                        strHtml += "</tr>"
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=ExcelFile.xls")
            Response.ContentEncoding = Encoding.UTF8
            Dim tw As StringWriter = New StringWriter()
            Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
            divmostrar.RenderControl(hw)
            Response.Write(tw.ToString())
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
