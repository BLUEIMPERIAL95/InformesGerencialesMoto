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
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json

Partial Class ConsumoServicioFacturacionAlbateq
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csfact As New facturas

    Private Sub ConsumoServicioFacturacionAlbateq_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(1038, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            divmostrar.InnerHtml = ""
            divinforme.InnerHtml = ""

            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim strRespuesta, strJson As String

                strJson = "{""FechaInicial"": """ & txtFechaInicio.Value & """," & """FechaFinal"": """ & txtFechaFin.Value & """," & """Usuario"": ""Moto"", ""Clave"": ""4DFF4EA340F0A823F15D3F4F01AB62EAE0E5DA579CCB851F8DB9DFE84C58B2B37B89903A740E1EE172DA793A6E79D560E5F7F9BD058A12A280433ED6FA46510A""}"
                'strJson = "{\"FechaInicial"/}"

                'strJson = JsonConvert.DeserializeObject(strJson)

                strRespuesta = csfact.Post("https://albateq.com/ApiExternos/api/Moto/TiquetesMoto", strJson)

                If strRespuesta <> "null" Then
                    Dim dtjson, dtmov As New DataTable
                    dtjson = JsonConvert.DeserializeObject(Of DataTable)(strRespuesta)

                    If dtjson.Rows.Count > 0 Then
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
                        strHtml += "</tr>"
                        strHtml += "<tr>"
                        strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                        strHtml += "<td align='center' colspan='5'><b><font size='4'>FACTURACIÓN ALBATEQ DESDE: " & txtFechaInicio.Value & " HASTA: " & txtFechaFin.Value & "</font></b></td>"
                        strHtml += "</tr>"
                        strHtml += "</table>"

                        strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><b><font size='9px'>Placa</font></b></td>"
                        strHtml += "<td align='center'><b><font size='9px'>Fecha Tiquete</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Descripcion</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Tiquete</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Total Kardex</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Proveedor</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Kilos Faltante</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Valor Unt Faltante</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Valor Tot Faltante</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Valor Flete Kilo</font></b></td>"
                        strHtml += "<td align='center'><b><font size='9px'>Mvto</font></b></td>"
                        strHtml += "<td align='center'><b><font size='9px'>Fecha Mvto</font></b></td>"
                        strHtml += "<td align='left' colspan='2'><b><font size='9px'>Generador</font></b></td>"
                        strHtml += "<td align='left'><b><font size='9px'>Estado</font></b></td>"
                        strHtml += "<td align='right'><b><font size='9px'>Fte Empresa</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Tiquete</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Descripcion</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Tiquete</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Total Kardex</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Proveedor</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Kilos Faltante</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Valor Unt Faltante</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Valor Tot Faltante</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Valor Flete Kilo</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Mvto</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Mvto</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Generador</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Estado</font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Empresa</font></b></td>"
                        strHtmlmostrar += "</tr>"

                        Dim decValorTotal As Decimal
                        Dim numViajes As Integer
                        decValorTotal = 0
                        numViajes = 0
                        For i As Integer = 0 To dtjson.Rows.Count - 1
                            strSQL = "Select movimientos_transportes_consolidado.movimiento_numero, "
                            strSQL += "movimientos_transportes_consolidado.movimiento_fecha, "
                            strSQL += "movimientos_transportes_consolidado.generador_nombre, "
                            strSQL += "movimientos_transportes_consolidado.movimiento_fl_empresa, "
                            strSQL += "CASE WHEN COALESCE(ventas_control.movimientos_transportes_id, 0) <> 0 THEN 'FACTURADO' ELSE 'SIN FACTURAR' END AS Estado "
                            strSQL += "From movimientos_transportes_consolidado "
                            strSQL += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                            strSQL += "WHERE movimientos_transportes_consolidado.vehiculo_placa = '" & dtjson.Rows(i)("Placa").ToString & "'  "
                            strSQL += "AND movimientos_transportes_consolidado.movimiento_fecha < '" & CDate(dtjson.Rows(i)("Fecha").ToString).ToString(("yyyy-MM-dd")) & "'  "
                            strSQL += "And COALESCE(ventas_control.movimientos_transportes_id, 0) = 0  "
                            strSQL += " And movimientos_transportes_consolidado.generador_id = 11 "
                            strSQL += " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                            strSQL += "ORDER BY movimientos_transportes_consolidado.movimiento_numero "

                            dtmov = csinformes.ejecutar_query_bd(strSQL)

                            If dtmov.Rows.Count > 0 Then
                                For j As Integer = 0 To dtmov.Rows.Count - 1
                                    strHtml += "<tr>"
                                    strHtml += "<td align='center'><font size='9px'>" & dtjson.Rows(i)("Placa").ToString & "</font></td>"
                                    strHtml += "<td align='center'><font size='9px'>" & dtjson.Rows(i)("Fecha").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("DescripciónItem").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("Tiquete").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("Neto Kardex Total").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("Proveedor").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("Kilos FalTanTe").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("Valor UniTario FalTanTe").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("Valor ToTal FalTanTe").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtjson.Rows(i)("vr fleTe kilo").ToString & "</font></td>"
                                    strHtml += "<td align='center'><font size='9px'>" & dtmov.Rows(j)("movimiento_numero").ToString & "</font></td>"
                                    strHtml += "<td align='center'><font size='9px'>" & dtmov.Rows(j)("movimiento_fecha").ToString & "</font></td>"
                                    strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtmov.Rows(j)("generador_nombre").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='9px'>" & dtmov.Rows(j)("Estado").ToString & "</font></td>"
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtmov.Rows(j)("movimiento_fl_empresa")) & "</font></td>"
                                    strHtml += "</tr>"

                                    strHtmlmostrar += "<tr>"
                                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtjson.Rows(i)("Placa").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtjson.Rows(i)("Fecha").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("DescripciónItem").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("Tiquete").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("Neto Kardex Total").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("Proveedor").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("Kilos FalTanTe").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("Valor UniTario FalTanTe").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("Valor ToTal FalTanTe").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtjson.Rows(i)("vr fleTe kilo").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtmov.Rows(j)("movimiento_numero").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtmov.Rows(j)("movimiento_fecha").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(j)("generador_nombre").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtmov.Rows(j)("Estado").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtmov.Rows(j)("movimiento_fl_empresa")) & "</font></td>"
                                    strHtmlmostrar += "</tr>"

                                    decValorTotal = decValorTotal + dtmov.Rows(j)("movimiento_fl_empresa")
                                    numViajes = numViajes + 1
                                Next
                            End If
                        Next
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtml += "<td align='left'><font size='9px'>Viajes: " & numViajes & "</font></td>"
                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", decValorTotal) & "</font></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>Viajes: " & numViajes & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", decValorTotal) & "</font></td>"
                        strHtmlmostrar += "</tr>"

                        strHtml += "</table>"
                        strHtmlmostrar += "</table>"

                        divmostrar.InnerHtml = strHtmlmostrar
                        divinforme.InnerHtml = strHtml
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información disponible...');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información disponible...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=FacturacionAlbateq.xls")
            Response.ContentEncoding = Encoding.UTF8
            Dim tw As StringWriter = New StringWriter()
            Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
            'Me.Page.RenderControl(hw)
            divmostrar.RenderControl(hw)
            Response.Write(tw.ToString())
            Dim sr As New StringReader(tw.ToString())
            Dim myMemoryStream As New MemoryStream()

            Dim archivoBytes As Byte() = myMemoryStream.ToArray()
            Dim archivoBase64 As String = Convert.ToBase64String(archivoBytes)
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "FacturacionAlbateq"

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
