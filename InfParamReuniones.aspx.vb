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

Partial Class InfParamReuniones
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csreun As New reunion

    Private Sub InfParamReuniones_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        'strRespuestaPer = csusua.validar_permiso_usuario(1, Session("id_usua"))

        'If strRespuestaPer <> "" Then
        '    Response.Redirect("Default.aspx")
        'End If

        If Me.IsPostBack = False Then
            hidreunion.Value = Request.QueryString("idreu")

            cargar_participantes(hidreunion.Value)
        End If
    End Sub

    Private Sub cargar_participantes(ByVal reu As Integer)
        Try
            Dim dtreu As New DataTable
            dtreu = csreun.seleccionar_participantes_reunion(reu)

            If dtreu.Rows.Count > 0 Then
                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                If ConfigurationManager.AppSettings("bdsel").ToString = 1 Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo.jpg")
                Else
                    If ConfigurationManager.AppSettings("bdsel").ToString = 2 Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                    Else
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logorefri.jpg")
                    End If
                End If

                If File.Exists(pathimgCabeza1) Then
                    urlFotoCabeza1 = pathimgCabeza1
                Else
                    urlFotoCabeza1 = Path.Combine(Server.MapPath("images"), "nophoto.jpg")
                End If

                Dim strHtml, strHtmlmostrar As String
                strHtml = ""
                strHtmlmostrar = ""

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' colspan='3'><b><font size='5'>" & dtreu.Rows(0)("nombre_reun").ToString & "</font></b></td>"
                strHtml += "<td align='left'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='5'>Codigo: </font></b><font size='5'>FO4H-024</font></td></tr><tr><td align='center'><b><font size='5'>Fecha: </font></b><font size='5'>2018-12-22</font></td></tr><tr><td align='center'><b><font size='5'>Version: </font></b><font size='5'>5</font></td></tr></table></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='5'>Fecha</font></b></td>"
                strHtml += "<td align='center'><font size='5'>" & dtreu.Rows(0)("fecha_reun").ToString & "</font></td>"
                strHtml += "<td align='center'><b><font size='5'>Hora</font></b></td>"
                strHtml += "<td align='center'><font size='5'>" & dtreu.Rows(0)("hora_reun").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='5'>Objetivo</font></b></td>"
                strHtml += "<td align='center' colspan='3'><font size='5'>" & dtreu.Rows(0)("objetivo_reun").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='5'>Expositor</font></b></td>"
                strHtml += "<td align='center' colspan='3'><font size='5'>" & dtreu.Rows(0)("nom_exp").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='5'>Descripción</font></b></td>"
                strHtml += "<td align='center' colspan='3'><font size='3'>" & dtreu.Rows(0)("descripcion_reun").ToString & "</font></td>"
                strHtml += "</tr>"

                strHtml += "</br>"
                strHtml += "</br>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='left'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='left'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='12px'>Documento</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='12px'>Nombre</font></b></td>"
                strHtml += "<td align='left'><b><font size='12px'>Empresa</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Fecha Hora Entrada</font></b></td>"
                'strHtml += "<td align='center'><b><font size='12px'>Fecha Hora Salida</font></b></td>"
                strHtml += "</tr>"

                For i As Integer = 0 To dtreu.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><font size='11px'>" & dtreu.Rows(i)("documento_TERC").ToString & "</font></td>"
                    strHtml += "<td align='left' colspan='2'><font size='11px'>" & dtreu.Rows(i)("nombre_TERC").ToString.ToUpper & "</font></td>"
                    strHtml += "<td align='left'><font size='11px'>" & dtreu.Rows(i)("empresa_TERC").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='11px'>" & dtreu.Rows(i)("fechahora_repa").ToString & "</font></td>"
                    'strHtml += "<td align='center'><font size='11px'>" & dtreu.Rows(i)("fehosal_repa").ToString & "</font></td>"
                    strHtml += "</tr>"
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center' colspan='4'><b><font size='11px'>TOTAL PARTICIPANTES</font></b></td>"
                strHtml += "<td align='right'><b><font size='11px'>" & dtreu.Rows.Count & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                divinforme.InnerHtml = strHtml

                export_pdf()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existen participantes para la reunión seleccionada.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub export_pdf()
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Participantes reunion " & hidreunion.Value

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
