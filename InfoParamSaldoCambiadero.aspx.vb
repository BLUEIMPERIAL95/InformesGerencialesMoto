

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
Partial Class InfoParamSaldoCambiadero
    Inherits System.Web.UI.Page

    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csreun As New reunion
    Dim cscamb As New cambiadero

    Private Sub InfParamSaldoCambiadero_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3074, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim dtcam As New DataTable
                Dim strHtml, strHtmlPDF As String
                dtcam = cscamb.seleccionar_saldos_cambiadero_listado_por_fecha(txtFechaInicio.Value, txtFechaFin.Value, Session("id_usua"))

                If dtcam.Rows.Count > 0 Then

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr bgcolor='#251E4C'>"
                    strHtml += "<td align='center'><b><font size='1px'><font color='white'>FECHA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'><font color='white'>TIPO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'><font color='white'>SALDO INICIAL</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'><font color='white'>SALDO FINAL</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'><font color='white'>USUARIO</font></b></td>"
                    strHtml += "</tr>"

                    For i As Integer = 0 To dtcam.Rows.Count - 1
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcam.Rows(i)("fecha_saca").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcam.Rows(i)("tipo_saca").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcam.Rows(i)("saldoini_saca").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcam.Rows(i)("saldofin_saca").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcam.Rows(i)("strnom1_usua").ToString & "</font></td>"
                        strHtml += "</tr>"
                    Next

                    strHtml += "</table>"
                    divmostrar.InnerHtml = strHtml

                    strHtmlPDF += "<table cellpadding='2' cellspacing='0' border='1' width='100%'>"
                    strHtmlPDF += "<tr>"
                    strHtmlPDF += "<td align='center'><b>FECHA</b></td>"
                    strHtmlPDF += "<td align='center'><b>TIPO</b></td>"
                    strHtmlPDF += "<td align='center'><b>SALDO INICIAL</b></td>"
                    strHtmlPDF += "<td align='center'><b>SALDO FINAL</b></td>"
                    strHtmlPDF += "<td align='center'><b>USUARIO</b></td>"
                    strHtmlPDF += "</tr>"

                    For i As Integer = 0 To dtcam.Rows.Count - 1
                        strHtmlPDF += "<tr>"
                        strHtmlPDF += "<td align='center'>" & dtcam.Rows(i)("fecha_saca").ToString & "</td>"
                        strHtmlPDF += "<td align='left'>" & dtcam.Rows(i)("tipo_saca").ToString & "</td>"
                        strHtmlPDF += "<td align='left'>" & dtcam.Rows(i)("saldoini_saca").ToString & "</td>"
                        strHtmlPDF += "<td align='left'>" & dtcam.Rows(i)("saldofin_saca").ToString & "</td>"
                        strHtmlPDF += "<td align='left'>" & dtcam.Rows(i)("strnom1_usua").ToString & "</td>"
                        strHtmlPDF += "</tr>"
                    Next

                    strHtmlPDF += "</table>"
                    divinforme.InnerHtml = strHtmlPDF
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub


    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Formato informe saldos cambiadero"

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim stringWriter As StringWriter = New StringWriter()
            Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
            divinforme.RenderControl(htmlTextWriter)
            Dim stringReader As StringReader = New StringReader(stringWriter.ToString())
            Dim Doc As Document = New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
            Dim htmlparser As HTMLWorker = New HTMLWorker(Doc)
            PdfWriter.GetInstance(Doc, Response.OutputStream)
            Doc.Open()
            htmlparser.Parse(stringReader)
            Doc.Close()
            Response.Write(Doc)
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class

