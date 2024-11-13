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
Partial Class InfParamManifiestos
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

        strRespuestaPer = csusua.validar_permiso_usuario(22, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            If (txtFechaInicio.Value = "" Or txtFechaFin.Value = "") And txtManifiesto.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                divmostrar.InnerHtml = ""
                divinforme.InnerHtml = ""

                strSQL = " Select COALESCE(movimientos_transportes_consolidado.manifiesto_sucursal, '') AS Sucursal,"
                strSQL += " COALESCE(movimientos_transportes_consolidado.manifiesto_numero, '') AS NumManfiesto,"
                strSQL += " movimientos_transportes_consolidado.movimiento_numero As NumMovimiento,"
                strSQL += " DATE_FORMAT(movimientos_transportes_consolidado.movimiento_fecha,'%Y-%m-%d') AS FecMovimiento,"
                strSQL += " movimientos_transportes_consolidado.asesor_nombre As Asesor,"
                strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador,"
                strSQL += " COALESCE(tipo_estados.descripcion, '') AS EstManifiesto"
                strSQL += " From movimientos_transportes_consolidado"
                strSQL += " Left Join movimientos_transportes_manifiestos ON movimientos_transportes_consolidado.manifiesto_id = movimientos_transportes_manifiestos.idmovimientos_transportes_manifiestos"
                strSQL += " Left Join tipo_estados ON movimientos_transportes_manifiestos.tipo_estados_idtipo = tipo_estados.idtipo_estados"
                If txtManifiesto.Text = "" Then
                    strSQL += " where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '"
                    strSQL += txtFechaInicio.Value + "' AND '" + txtFechaFin.Value + "'"
                Else
                    strSQL += " where movimientos_transportes_consolidado.manifiesto_numero = " & txtManifiesto.Text & ""
                End If
                strSQL += " ORDER BY movimientos_transportes_consolidado.manifiesto_numero"

                dtter = csinformes.ejecutar_query_bd(strSQL)

                'gridAsesores.DataSource = dtter
                'gridAsesores.DataBind()

                If dtter.Rows.Count > 0 Then
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

                    strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtml += "<td align='center' colspan='6'><b><font size='4'>MANIFESTOS(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='7px'>Sucursal</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Numero Manifiesto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Numero Movimiento</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Fecha Movimiento</font></b></td>"
                    strHtml += "<td align='left'><b><font size='7px'>Asesor</font></b></td>"
                    strHtml += "<td align='left'><b><font size='7px'>Despachador</font></b></td>"
                    strHtml += "<td align='left'><b><font size='7px'>Estado</font></b></td>"
                    strHtml += "</tr>"

                    'strHtmlmostrar = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    'strHtmlmostrar += "<tr>"
                    'strHtmlmostrar += "<td align='left'></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "</tr>"
                    'strHtmlmostrar += "<tr>"
                    'strHtmlmostrar += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    'strHtmlmostrar += "<td align='center' colspan='6'><b><font size='4'>MANIFIESTOS(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                    'strHtmlmostrar += "</tr>"
                    'strHtmlmostrar += "</table>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Sucursal</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Numero Manifiesto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Numero Movimiento</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Movimiento</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Asesor</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Despachador</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Estado</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    For i As Integer = 0 To dtter.Rows.Count - 1
                        strHtml += "<tr>"

                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("NumManfiesto").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("NumMovimiento").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("FecMovimiento").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Asesor").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("EstManifiesto").ToString & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("NumManfiesto").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("NumMovimiento").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("FecMovimiento").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Asesor").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("EstManifiesto").ToString & "</font></td>"

                        strHtmlmostrar += "</tr>"
                    Next

                    strHtml += "</table>"
                    strHtmlmostrar += "</table>"

                    divmostrar.InnerHtml = strHtmlmostrar
                    divinforme.InnerHtml = strHtml
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

        'Dim name = "Asesores"

        'Dim sb As New StringBuilder()
        'Dim sw As New StringWriter(sb)
        'Dim htw As New HtmlTextWriter(sw)

        'Dim page As New Page()
        'Dim form As New HtmlForm()

        'gridAsesores.EnableViewState = False

        '' Deshabilitar la validación de eventos, sólo asp.net 2 
        'page.EnableEventValidation = False

        '' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        'page.DesignerInitialize()

        'page.Controls.Add(form)
        'form.Controls.Add(gridAsesores)

        'page.RenderControl(htw)

        'Response.Clear()
        'Response.Buffer = True

        'Response.ContentType = "application/pdf"
        'Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".pdf")
        'Response.Charset = "UTF-8"


        'Response.ContentEncoding = Encoding.[Default]
        'Response.Write(sb.ToString())
        'Response.[End]()
    End Sub

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Manifiestos desde " & txtFechaInicio.Value & " hasta " & txtFechaFin.Value

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
