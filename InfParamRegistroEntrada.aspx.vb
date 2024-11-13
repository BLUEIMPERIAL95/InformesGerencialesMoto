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
Partial Class InfParamRegistroEntrada
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csequi As New equipos

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2040, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            If txtFechaInicio.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim dtReg As New DataTable

                dtReg = csequi.capturar_datos_registro_entrada_por_fecha(txtFechaInicio.Value, txtFechaFin.Value, cboempresa.SelectedValue)

                If dtReg.Rows.Count > 0 Then
                    divmostrar.InnerHtml = ""

                    Dim strHtmlmostrar As String

                    strHtmlmostrar = ""

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Documento</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Tercero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Ingreso</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Salida</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Empresa</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Eps</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Arl</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Temperatura</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    For i As Integer = 0 To dtReg.Rows.Count - 1
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtReg.Rows(i)("documento_TERC").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtReg.Rows(i)("nombre_TERC").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtReg.Rows(i)("fechaent_REEN").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtReg.Rows(i)("fechasal_REEN").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtReg.Rows(i)("empresa_TERC").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtReg.Rows(i)("nombre_eps").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtReg.Rows(i)("nombre_arl").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtReg.Rows(i)("temperatura_REEN").ToString & "</font></td>"
                        strHtmlmostrar += "</tr>"
                    Next
                    strHtmlmostrar += "</table>"

                    divmostrar.InnerHtml = strHtmlmostrar
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información para el día seleccionado...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            'Response.ContentType = "application/x-msexcel"
            'Response.AddHeader("Content-Disposition", "attachment; filename=RegistroEntrada.xls")
            'Response.ContentEncoding = Encoding.UTF8
            'Dim tw As StringWriter = New StringWriter()
            'Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
            ''Me.Page.RenderControl(hw)
            'divmostrar.RenderControl(hw)
            'Response.Write(tw.ToString())
            'Dim sr As New StringReader(tw.ToString())
            'Dim myMemoryStream As New MemoryStream()

            'Dim archivoBytes As Byte() = myMemoryStream.ToArray()
            'Dim archivoBase64 As String = Convert.ToBase64String(archivoBytes)
            'Response.[End]()

            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=RegistroEntrada.xls")
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
