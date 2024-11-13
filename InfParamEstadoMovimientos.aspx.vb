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
Partial Class InfParamEstadoMovimientos
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes

    Private Sub InfParamEstadoMovimientos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(21, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Private Sub combos()
        Dim dtEst As New DataTable
        csoper.LlenarDropDownListTipoEstadoMovimiento("descripcion", "idtipo_estados", "", dtEst, cboEstados, 46)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        divinforme.InnerHtml = ""
        divmostrar.InnerHtml = ""
        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Or cboEstados.SelectedValue = 0 Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar.');", True)
        Else
            Dim dtmov As New DataTable
            Dim strHtml, strHtmlmostrar, strSQL As String
            strHtml = ""
            strHtmlmostrar = ""
            strSQL = ""

            strSQL = "Select movimientos_transportes_consolidado.movimiento_numero, "
            strSQL += "movimientos_transportes_consolidado.vehiculo_placa, "
            strSQL += "CONCAT(movimientos_transportes_consolidado.movimiento_origen, ' - ', movimientos_transportes_consolidado.movimiento_destino) AS ruta, "
            strSQL += "movimientos_transportes_consolidado.generador_nombre, "
            strSQL += "movimientos_transportes_consolidado.conductor_nombre, "
            strSQL += "movimientos_transportes_consolidado.conductor_documento, "
            strSQL += "CASE WHEN COALESCE(movimientos_transportes_consolidado.factura_id, 'NA') <> 'NA' THEN 'FACTURADO' ELSE movimientos_transportes_consolidado.movimiento_estado END AS movimiento_estado, "
            strSQL += "movimientos_transportes_consolidado.movimiento_claseveh, "
            strSQL += "movimientos_transportes_consolidado_detalle.producto_descripcion, "
            strSQL += "vehiculos.capacidad, "
            strSQL += "telefonos.telefono "
            strSQL += "From movimientos_transportes_consolidado "
            strSQL += "Left Join movimientos_transportes_consolidado_detalle ON movimientos_transportes_consolidado_detalle.movimiento_id = movimientos_transportes_consolidado.movimiento_id "
            strSQL += "Left Join vehiculos ON vehiculos.idvehiculos = movimientos_transportes_consolidado.vehiculo_id "
            strSQL += "Left Join terceros_conductores ON terceros_conductores.idterceros_conductores = movimientos_transportes_consolidado.conductor_id "
            strSQL += "Left Join terceros ON terceros.idterceros = terceros_conductores.terceros_idterceros "
            strSQL += "Left Join telefonos ON telefonos.terceros_idterceros = terceros.idterceros "
            strSQL += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            If cboEstados.Items(cboEstados.SelectedIndex).Text = "FACTURADO" Then
                strSQL += "And COALESCE(movimientos_transportes_consolidado.factura_id, 'NA') <> 'NA' "
            Else
                strSQL += " And movimientos_transportes_consolidado.movimiento_estado = '" & cboEstados.Items(cboEstados.SelectedIndex).Text & "' "
            End If
            strSQL += "And (COALESCE(CONCAT(movimientos_transportes_consolidado.movimiento_origen, ' - ', movimientos_transportes_consolidado.movimiento_destino), 'NA')) <> 'NA' "
            strSQL += "And telefonos.tipo_telefonos_idtipo_telefonos = 2 And telefonos.idel = 0 "
            strSQL += "ORDER BY movimientos_transportes_consolidado.movimiento_numero "

            dtmov = csinformes.ejecutar_query_bd(strSQL)

            If dtmov.Rows.Count Then
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
                strHtml += "<td align='center' colspan='5'><b><font size='4'>FORMATO ESTADO VEHICULOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='9px'>Movimiento</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Placa</font></b></td>"
                strHtml += "<td align='left' colspan='3'><b><font size='9px'>Ruta</font></b></td>"
                strHtml += "<td align='left' colspan='3'><b><font size='9px'>Generador</font></b></td>"
                strHtml += "<td align='left' colspan='3'><b><font size='9px'>Conductor</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Telefono</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Tipo Vehiculo</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Capacidad</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Producto</font></b></td>"
                strHtml += "<td align='left'><b><font size='9px'>Estado</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Movimiento</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>Ruta</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>Generador</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>Conductor</font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>Telefono</font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>Tipo Vehiculo</font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>Capacidad</font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>Producto</font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>Estado</font></b></td>"
                strHtmlmostrar += "</tr>"

                For i As Integer = 0 To dtmov.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><font size='9px'>" & dtmov.Rows(i)("movimiento_numero").ToString & "</font></td>"
                    strHtml += "<td align='center'><font size='9px'>" & dtmov.Rows(i)("vehiculo_placa").ToString & "</font></td>"
                    strHtml += "<td align='left' colspan='3'><font size='9px'>" & dtmov.Rows(i)("ruta").ToString & "</font></td>"
                    strHtml += "<td align='left' colspan='3'><font size='9px'>" & dtmov.Rows(i)("generador_nombre").ToString & "</font></td>"
                    strHtml += "<td align='left' colspan='3'><font size='9px'>" & dtmov.Rows(i)("conductor_nombre").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtmov.Rows(i)("telefono").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtmov.Rows(i)("movimiento_claseveh").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtmov.Rows(i)("capacidad").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtmov.Rows(i)("producto_descripcion").ToString & "</font></td>"
                    strHtml += "<td align='left'><font size='9px'>" & dtmov.Rows(i)("movimiento_estado").ToString & "</font></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtmov.Rows(i)("movimiento_numero").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtmov.Rows(i)("vehiculo_placa").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left' colspan='3'><font size='1px'>" & dtmov.Rows(i)("ruta").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left' colspan='3'><font size='1px'>" & dtmov.Rows(i)("generador_nombre").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left' colspan='3'><font size='1px'>" & dtmov.Rows(i)("conductor_nombre").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(i)("telefono").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(i)("movimiento_claseveh").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(i)("capacidad").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(i)("producto_descripcion").ToString & "</font></td>"
                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(i)("movimiento_estado").ToString & "</font></td>"
                    strHtmlmostrar += "</tr>"
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1'></font></b></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "</table>"
                strHtmlmostrar += "</table>"

                divinforme.InnerHtml = strHtml
                divmostrar.InnerHtml = strHtmlmostrar
            End If
        End If

    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=FormatoEstadosVehiculos.xls")
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

            strNombreInforme = "Formato Estados Vehiculos"

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
