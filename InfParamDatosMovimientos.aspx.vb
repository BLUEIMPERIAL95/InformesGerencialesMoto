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
Imports System.Xml
Partial Class InfParamDatosMovimientos
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes

    Private Sub InfParamDatosMovimientos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2048, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtgen As New DataTable

        csoper.LlenarDropDownList("nombrecompleto", "id", "generadores_mostrar_todos", dtgen, cbogeneradores)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Or cbogeneradores.SelectedValue = "0" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta Información por seleccionar...');", True)
        Else
            Dim dtMov As New DataTable
            Dim strSQL As String

            strSQL = "SELECT movimientos_transportes_consolidado.movimiento_numero AS Numero, "
            strSQL += "movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            strSQL += "movimientos_transportes_consolidado.remesa_numero AS Remesa, "
            strSQL += "movimientos_transportes_consolidado.movimiento_operacion AS Operacion, "
            strSQL += "DATE_FORMAT(movimientos_transportes_consolidado.movimiento_fecha, '%Y-%m-%d') as 'movimiento_fecha', "
            strSQL += "'NA' AS Tarifa, "
            strSQL += "movimientos_transportes_consolidado.movimiento_fl_empresa AS Facturado, "
            strSQL += "movimientos_transportes_consolidado.movimiento_origen AS Origen, "
            strSQL += "movimientos_transportes_consolidado.movimiento_destino AS Destino, "
            strSQL += "movimientos_transportes_consolidado.movimiento_unidades AS Cantidad, "
            strSQL += "movimientos_transportes_consolidado.movimiento_modalidad AS Unidad, "
            strSQL += "CONCAT(remitentes_destinatarios.nombre1, ' ', remitentes_destinatarios.nombre2, ' ', remitentes_destinatarios.apellido1, ' ', remitentes_destinatarios.apellido2) AS Nombre, "
            strSQL += "movimientos_transportes_rndc.xml, "
            strSQL += "movimientos_transportes_rndc.respuesta, "
            strSQL += "movimientos_transportes_remesa_rndc.rndc AS rem_radicado "
            strSQL += "FROM movimientos_transportes_consolidado "
            strSQL += "LEFT JOIN movimientos_transportes_detalles ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes_detalles.movimientos_transportes_idmovimientos "
            strSQL += "LEFT JOIN remitentes_destinatarios ON movimientos_transportes_detalles.remitentes_destinatarios_iddestinatarios = remitentes_destinatarios.idremitentes_destinatarios "
            strSQL += "INNER JOIN movimientos_transportes_rndc ON movimientos_transportes_consolidado.manifiesto_id = movimientos_transportes_rndc.reportado_id "
            strSQL += "INNER Join movimientos_transportes_remesa_rndc ON movimientos_transportes_consolidado.remesa_id = movimientos_transportes_remesa_rndc.remesa_id "
            strSQL += "WHERE movimientos_transportes_consolidado.generador_id = " & cbogeneradores.SelectedValue & " "
            strSQL += "AND movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += "AND movimientos_transportes_rndc.reporte = 'MANIFIESTO' AND LENGTH(movimientos_transportes_rndc.respuesta) < 15 "
            'strSQL += "ORDER BY movimientos_transportes_consolidado.movimiento_fecha "

            strSQL += "UNION ALL "

            strSQL += "Select movimientos_transportes_consolidado.movimiento_numero As Numero, "
            strSQL += "movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            strSQL += "movimientos_transportes_consolidado.remesa_numero As Remesa, "
            strSQL += "movimientos_transportes_consolidado.movimiento_operacion AS Operacion, "
            strSQL += "DATE_FORMAT(movimientos_transportes_consolidado.movimiento_fecha, '%Y-%m-%d') as 'movimiento_fecha', "
            strSQL += " 'NA' AS Tarifa, "
            strSQL += "movimientos_transportes_consolidado.movimiento_fl_empresa AS Facturado, "
            strSQL += "movimientos_transportes_consolidado.movimiento_origen As Origen, "
            strSQL += "movimientos_transportes_consolidado.movimiento_destino AS Destino, "
            strSQL += "movimientos_transportes_consolidado.movimiento_unidades As Cantidad, "
            strSQL += "movimientos_transportes_consolidado.movimiento_modalidad AS Unidad, "
            strSQL += "CONCAT(remitentes_destinatarios.nombre1, ' ', remitentes_destinatarios.nombre2, ' ', remitentes_destinatarios.apellido1, ' ', remitentes_destinatarios.apellido2) AS Nombre, "
            strSQL += "movimientos_transportes_rndc.xml, "
            strSQL += "movimientos_transportes_rndc.respuesta, "
            strSQL += "movimientos_transportes_remesa_rndc.rndc AS rem_radicado "
            strSQL += "From movimientos_transportes_consolidado "
            strSQL += "Left Join movimientos_transportes_detalles ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes_detalles.movimientos_transportes_idmovimientos "
            strSQL += "Left Join remitentes_destinatarios ON movimientos_transportes_detalles.remitentes_destinatarios_iddestinatarios = remitentes_destinatarios.idremitentes_destinatarios "
            strSQL += "Left Join movimientos_transportes_rndc ON movimientos_transportes_consolidado.urbano_id = movimientos_transportes_rndc.reportado_id "
            strSQL += "Left Join movimientos_transportes_remesa_rndc ON movimientos_transportes_consolidado.remesa_id = movimientos_transportes_remesa_rndc.remesa_id "
            strSQL += "WHERE movimientos_transportes_consolidado.generador_id = " & cbogeneradores.SelectedValue & " "
            strSQL += "AND movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += "And movimientos_transportes_rndc.reporte = 'MANIFIESTO' "
            strSQL += "And LENGTH(movimientos_transportes_rndc.respuesta) < 15 "
            'strSQL += "ORDER BY movimientos_transportes_consolidado.movimiento_fecha "

            dtMov = csinformes.ejecutar_query_bd(strSQL)

            If dtMov.Rows.Count > 0 Then
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

                strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left'></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO POR RANGO DE FECHAS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='9px'>Movimiento</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Placa</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Remesa</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Rem Radicado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Manifiesto</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Man Radicado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Operacion</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Fecha</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Tarifa</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Facturado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Origen</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Destino</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Cantidad</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Unidad</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Destinatario</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Movimiento</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Remesa</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Rem Radicado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Manifiesto</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Man Radicado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Operacion</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Tarifa</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Facturado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Origen</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Destino</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cantidad</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Unidad</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Destinatario</font></b></td>"
                strHtmlmostrar += "</tr>"

                For i As Integer = 0 To dtMov.Rows.Count - 1
                    Dim documentoxml As New XmlDocument
                    Dim InfoVariables, InfoManifiesto, InfoRemesaMan, InfoRemesa, InfoConsRemesa As XmlElement
                    Dim strManifiesto, strRemesa As String

                    Try
                        documentoxml.LoadXml(dtMov.Rows(i)("xml").ToString)
                        InfoVariables = documentoxml.DocumentElement.SelectSingleNode("variables")
                        InfoRemesaMan = InfoVariables.SelectSingleNode("REMESASMAN")
                        InfoRemesa = InfoRemesaMan.SelectSingleNode("REMESA")
                        InfoConsRemesa = InfoRemesa.SelectSingleNode("CONSECUTIVOREMESA")
                        strRemesa = InfoConsRemesa.InnerText.ToString
                    Catch ex As Exception
                        strRemesa = ""
                    End Try

                    Try
                        documentoxml.LoadXml(dtMov.Rows(i)("xml").ToString)
                        InfoVariables = documentoxml.DocumentElement.SelectSingleNode("variables")
                        InfoManifiesto = InfoVariables.SelectSingleNode("NUMMANIFIESTOCARGA")
                        strManifiesto = InfoManifiesto.InnerText.ToString
                    Catch ex As Exception
                        strManifiesto = ""
                    End Try

                    strHtml += "<tr>"
                    strHtml += "<td align='center'><font size='9px'>" & dtMov.Rows(i)("Numero").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & dtMov.Rows(i)("Placa").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & strRemesa & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & dtMov.Rows(i)("rem_radicado").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & strManifiesto & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & dtMov.Rows(i)("Respuesta").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtMov.Rows(i)("Operacion").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & dtMov.Rows(i)("movimiento_fecha").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & dtMov.Rows(i)("Tarifa").ToString & "</font></td>"
                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtMov.Rows(i)("Facturado").ToString, 0) & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtMov.Rows(i)("Origen").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtMov.Rows(i)("Destino").ToString & "</font></td>"
                    strHtml += "<td align='right'><font size='9px'>" & dtMov.Rows(i)("Cantidad").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtMov.Rows(i)("Unidad").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtMov.Rows(i)("Nombre").ToString & "</font></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtMov.Rows(i)("Numero").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtMov.Rows(i)("Placa").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & strRemesa & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtMov.Rows(i)("rem_radicado").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & strManifiesto & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtMov.Rows(i)("respuesta").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtMov.Rows(i)("Operacion").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtMov.Rows(i)("movimiento_fecha").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtMov.Rows(i)("Tarifa").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtMov.Rows(i)("Facturado").ToString, 0) & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtMov.Rows(i)("Origen").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtMov.Rows(i)("Destino").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='right'><font size='1px'>" & dtMov.Rows(i)("Cantidad").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtMov.Rows(i)("Unidad").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtMov.Rows(i)("Nombre").ToString & "</font></td>"
                    strHtmlmostrar += "</tr>"
                Next

                strHtml += "</table>"
                strHtmlmostrar += "</table>"

                divinforme.InnerHtml = strHtml
                divmostrar.InnerHtml = strHtmlmostrar
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe Información...');", True)
            End If
        End If
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=DatosMovimientos.xls")
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

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "DatosMovimientos"

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim stringWriter As StringWriter = New StringWriter()
            Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
            divinforme.RenderControl(htmlTextWriter)
            Dim stringReader As StringReader = New StringReader(stringWriter.ToString())
            Dim Doc As Document = New Document(PageSize.A3.Rotate, 5.0F, 5.0F, 5.0F, 0.0F)
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
