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
Partial Class InfParamPendientesTipoReunionResumido
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csreun As New reunion

    Private Sub InfParamPendientesTipoReunionResumido_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(38, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtreu As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_tipo_reuniones_id_combo", dtreu, cboreuniones)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            Dim dtreu As New DataTable

            dtreu = csreun.seleccionar_pendientes_reuniones_por_tipo_reunion(cboreuniones.SelectedValue)

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

                Dim strHtml, strHtmlmostrar, strReunActual As String
                strHtml = ""
                strHtmlmostrar = ""

                strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left'></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                strHtml += "<td align='center' colspan='5'><b><font size='4'>FORMATO DE PENDIENTES POR TIPO REUNION</font></b></td>"
                strHtml += "</tr>"
                'strHtml += "<tr>"
                'strHtml += "<td align='left' colspan='2'><b><font size='4'>Codigo: </font></b><font size='4'>" & dtreu.Rows(0)("codigo_reun").ToString & "</font></td>"
                'strHtml += "<td align='left' colspan='2'><b><font size='4'>Nombre: </font></b><font size='4'>" & dtreu.Rows(0)("nombre_reun").ToString & "</font></td>"
                'strHtml += "<td align='left' colspan='2'><b><font size='4'>Fecha: </font></b><font size='4'>" & dtreu.Rows(0)("fecha_reun").ToString & "</font></td>"
                'strHtml += "</tr>"
                'strHtml += "<tr>"
                'strHtml += "<td align='left' colspan='6'><b><font size='4'>Descripcion: </font></b><font size='4'>" & dtreu.Rows(0)("descripcion_reun").ToString & "</font></td>"
                'strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='left' colspan='3'><b><font size='4'>Reunión: </font></b><font size='4'>" & dtreu.Rows(0)("nombre_reun").ToString & "</font></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='4'>Código: </font></b><font size='4'>" & dtreu.Rows(0)("codigo_reun").ToString & "</font></td>"
                strHtml += "<td align='left' colspan='1'><b><font size='4'>Fecha: </font></b><font size='4'>" & dtreu.Rows(0)("fecha_reun").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left'></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center' colspan='2'><b><font size='1px'>Codigo: </font></b><font size='1px'>" & dtreu.Rows(0)("codigo_reun").ToString & "</font></td>"
                strHtmlmostrar += "<td align='center' colspan='1'><b><font size='1px'>Fecha: </font></b><font size='1px'>" & dtreu.Rows(0)("fecha_reun").ToString & "</font></td>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>Nombre: </font></b><font size='1px'>" & dtreu.Rows(0)("nombre_reun").ToString & "</font></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left'></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='9px'>Codigo</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='9px'>Nombre</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Estado</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Solicita</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='9px'>Responsables</font></b></td>"
                strHtml += "<td align='left' colspan='3'><b><font size='9px'>Respuesta</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Codigo</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Nombre</font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>Estado</font></b></td>"
                strHtmlmostrar += "</tr>"

                strReunActual = dtreu.Rows(0)("codigo_reun").ToString
                For i As Integer = 0 To dtreu.Rows.Count - 1
                    If strReunActual = dtreu.Rows(i)("codigo_reun").ToString Then
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='9px'>" & dtreu.Rows(i)("codigo_peru").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtreu.Rows(i)("nombre_peru").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='9px'>" & dtreu.Rows(i)("estado_peru").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='9px'>" & dtreu.Rows(i)("nombre_TERC").ToString & "</font></td>"
                        Dim dtres As New DataTable
                        dtres = csreun.capturar_datos_pendiente_reunion_terceros_por_id_pendiente(dtreu.Rows(i)("id_peru"))
                        If dtres.Rows.Count > 0 Then
                            If dtres.Rows.Count = 1 Then
                                strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtres.Rows(0)("nombre_TERC").ToString & "</font></td>"
                            Else
                                strHtml += "<td align='left' colspan='2'><font size='9px'>"
                                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                                For j As Integer = 0 To dtres.Rows.Count - 1
                                    strHtml += "<tr>"
                                    strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtres.Rows(j)("nombre_TERC").ToString & "</font></td>"
                                    strHtml += "</tr>"
                                Next
                                strHtml += "</table>"
                                strHtml += "</font></td>"
                            End If
                        Else
                            strHtml += "<td align='left' colspan='2'><font size='9px'>Sin responsable</font></td>"
                        End If
                        Dim dtrep As New DataTable
                        dtrep = csreun.seleccionar_respuestas_pendiente_reunion_terceros_por_id_pendiente(dtreu.Rows(i)("id_peru"))
                        If dtrep.Rows.Count Then
                            strHtml += "<td align='left' colspan='3'><font size='9px'>"
                            For j As Integer = 0 To dtrep.Rows.Count - 1
                                strHtml += " " & dtrep.Rows(j)("respuesta").ToString
                            Next
                            strHtml += "</font></td>"
                        Else
                            strHtml += "<td align='left' colspan='3'><font size='9px'>Sin Respuesta</font></td>"
                        End If
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtreu.Rows(i)("codigo_peru").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtreu.Rows(i)("nombre_peru").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtreu.Rows(i)("estado_peru").ToString & "</font></td>"
                        strHtmlmostrar += "</tr>"
                    Else
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "</tr>"

                        strHtml += "</table>"
                        strHtmlmostrar += "</table>"

                        strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                        strHtml += "<tr bgcolor='#BDBDBD'>"
                        strHtml += "<td align='left' colspan='3'><b><font size='4'>Reunión: </font></b><font size='4'>" & dtreu.Rows(i)("nombre_reun").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><b><font size='4'>Código: </font></b><font size='4'>" & dtreu.Rows(i)("codigo_reun").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='1'><b><font size='4'>Fecha: </font></b><font size='4'>" & dtreu.Rows(i)("fecha_reun").ToString & "</font></td>"
                        strHtml += "</tr>"
                        strHtml += "<tr>"
                        strHtml += "<td align='left'></td>"
                        strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtml += "</tr>"
                        strHtml += "</table>"

                        strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center' colspan='2'><b><font size='1px'>Codigo: </font></b><font size='1px'>" & dtreu.Rows(i)("codigo_reun").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center' colspan='1'><b><font size='1px'>Fecha: </font></b><font size='1px'>" & dtreu.Rows(i)("fecha_reun").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>Nombre: </font></b><font size='1px'>" & dtreu.Rows(i)("nombre_reun").ToString & "</font></td>"
                        strHtmlmostrar += "</tr>"
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='left'></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                        strHtmlmostrar += "</tr>"
                        strHtmlmostrar += "</table>"

                        strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><b><font size='9px'>Codigo</font></b></td>"
                        strHtml += "<td align='left' colspan='2'><b><font size='9px'>Nombre</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Estado</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Solicita</font></b></td>"
                        strHtml += "<td align='left' colspan='2'><b><font size='9px'>Responsables</font></b></td>"
                        strHtml += "<td align='left' colspan='3'><b><font size='9px'>Respuesta</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Codigo</font></b></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Nombre</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Estado</font></b></td>"
                        strHtmlmostrar += "</tr>"

                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='9px'>" & dtreu.Rows(i)("codigo_peru").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtreu.Rows(i)("nombre_peru").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='9px'>" & dtreu.Rows(i)("estado_peru").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='9px'>" & dtreu.Rows(i)("nombre_TERC").ToString & "</font></td>"
                        Dim dtres As New DataTable
                        dtres = csreun.capturar_datos_pendiente_reunion_terceros_por_id_pendiente(dtreu.Rows(i)("id_peru"))
                        If dtres.Rows.Count > 0 Then
                            If dtres.Rows.Count = 1 Then
                                strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtres.Rows(0)("nombre_TERC").ToString & "</font></td>"
                            Else
                                strHtml += "<td align='left' colspan='2'><font size='9px'>"
                                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                                For j As Integer = 0 To dtres.Rows.Count - 1
                                    strHtml += "<tr>"
                                    strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtres.Rows(j)("nombre_TERC").ToString & "</font></td>"
                                    strHtml += "</tr>"
                                Next
                                strHtml += "</table>"
                                strHtml += "</font></td>"
                            End If
                        Else
                            strHtml += "<td align='left' colspan='2'><font size='9px'>Sin responsable</font></td>"
                        End If
                        Dim dtrep As New DataTable
                        dtrep = csreun.seleccionar_respuestas_pendiente_reunion_terceros_por_id_pendiente(dtreu.Rows(i)("id_peru"))
                        If dtrep.Rows.Count Then
                            strHtml += "<td align='left' colspan='3'><font size='9px'>"
                            For j As Integer = 0 To dtrep.Rows.Count - 1
                                strHtml += " " & dtrep.Rows(j)("respuesta").ToString
                            Next
                            strHtml += "</font></td>"
                        Else
                            strHtml += "<td align='left' colspan='3'><font size='9px'>Sin Respuesta</font></td>"
                        End If
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtreu.Rows(i)("codigo_peru").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtreu.Rows(i)("nombre_peru").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtreu.Rows(i)("estado_peru").ToString & "</font></td>"
                        strHtmlmostrar += "</tr>"

                        strReunActual = dtreu.Rows(i)("codigo_reun").ToString
                    End If
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "</table>"
                strHtmlmostrar += "</table>"

                divmostrar.InnerHtml = strHtmlmostrar
                divinforme.InnerHtml = strHtml

            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Formato Pendientes Reunion"

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
