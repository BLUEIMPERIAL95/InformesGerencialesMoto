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
Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb

Partial Class ConciliacionCartera
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamCarteraXAsesor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(26, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then

        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Dim strFac As String
        strFac = ""
        Try
            Dim strSQL, strTipo, color As String
            Dim dtter As New System.Data.DataTable
            Dim dtsal As New System.Data.DataTable
            color = "FFFFFF"

            If FileExcel.HasFile Then
                If Path.GetExtension(FileExcel.FileName) = ".csv" Then
                    If txtFechaInicio.Value = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fecha Inválida...');", True)
                    Else
                        divmostrar.InnerHtml = ""
                        'divinforme.InnerHtml = ""

                        strSQL = "Select vc.generador_nombre As Generador, "
                        strSQL += " vc.generador_documento as Documento,"
                        strSQL += " ve.numero As NroFac, "
                        strSQL += " DATE_FORMAT(vc.venta_fecha,'%Y-%m-%d') as Fecha, "
                        strSQL += " DATE_FORMAT(vc.venta_vence,'%Y-%m-%d') as Vence, "
                        strSQL += " COALESCE((SELECT  SUM(vrk.valor) From ventas_recaudos vrf "
                        strSQL += " inner Join ventas_recaudos_detalle vrk On vrf.idventas_recaudos=vrk.ventas_recaudos_id "
                        strSQL += " where vrf.fecha_recaudo <='" & txtFechaInicio.Value & "' And vrk.ventas_idventas = vc.venta_id "
                        strSQL += " Group BY vrk.ventas_idventas),0) As 'abono', "
                        strSQL += "(vc.venta_total - COALESCE((SELECT  SUM(vrk.valor)  "
                        strSQL += " From ventas_recaudos vrf "
                        strSQL += " inner Join ventas_recaudos_detalle vrk  On vrf.idventas_recaudos=vrk.ventas_recaudos_id "
                        strSQL += " where vrf.fecha_recaudo <='" & txtFechaInicio.Value & "' And vrk.ventas_idventas = vc.venta_id "
                        strSQL += " Group BY vrk.ventas_idventas),0)) As 'Saldo'"
                        strSQL += " From ventas_consolidado vc  "
                        strSQL += " Left Join   ventas ve On vc.venta_id=ve.idventas  "
                        strSQL += " Left Join   ventas_recaudos_detalle vrd On vc.venta_id=vrd.ventas_idventas  "
                        strSQL += " Left Join   ventas_recaudos vr On vrd.ventas_recaudos_id=vr.idventas_recaudos "
                        strSQL += " where  vc.venta_fecha <='" & txtFechaInicio.Value & "'  AND vc.venta_estado='EMITIDA'"
                        strSQL += " AND vc.venta_id = (SELECT vd.ventas_idventas FROM ventas_detalles vd WHERE vc.venta_id=vd.ventas_idventas And vd.idel=0 LIMIT 1) "
                        'strSQL += " And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)Is NULL)"
                        'strSQL += " And ((vc.venta_total > (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vc.venta_id AND vr.fecha_recaudo <= '" & txtFechaInicio.Value & "')) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vc.venta_id AND vr.fecha_recaudo <= '" & txtFechaInicio.Value & "') Is NULL)"
                        strSQL += " And (COALESCE((SELECT  SUM(vrk.valor) From ventas_recaudos vrf "
                        strSQL += " inner Join ventas_recaudos_detalle vrk On vrf.idventas_recaudos=vrk.ventas_recaudos_id "
                        strSQL += " where vrf.fecha_recaudo <='" & txtFechaInicio.Value & "' And vrk.ventas_idventas = vc.venta_id "
                        strSQL += " Group BY vrk.ventas_idventas),0)) < vc.venta_total "
                        strSQL += " Group By vc.venta_id Order By vc.venta_numero"

                        dtter = csinformes.ejecutar_query_bd(strSQL)

                        If dtter.Rows.Count > 0 Then
                            Dim intCont As Integer
                            Dim strDoc, strGen, vlrSal, strHtmlmostrar As String
                            strHtmlmostrar = ""

                            strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'>DOCUMENTO</font></b></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>NOMBRE</font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>FACTURA</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>VALOR SYSTRAM</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>VALOR CONTAI</font></b></td>"
                            strHtmlmostrar += "</tr>"

                            'Save the uploaded Excel file.
                            Dim filePath As String = Server.MapPath("Carga_Excel/") + Path.GetFileName(FileExcel.PostedFile.FileName)
                            FileExcel.PostedFile.SaveAs(filePath)

                            ' Create an instance of StreamReader to read from a file.
                            Dim sr As StreamReader = New StreamReader(filePath)
                            Dim line As String

                            Dim vec As String()
                            line = sr.ReadLine()
                            intCont = 0
                            Do
                                intCont = intCont + 1
                                If intCont = 83 Then
                                    Dim strRes = ""
                                End If
                                vec = line.Split(";")

                                strDoc = vec(0)
                                strGen = vec(1)
                                strDoc = strDoc.Substring(0, strDoc.Length - 2)
                                'Try
                                '    strDoc = strDoc.Replace(" ", "")
                                '    strDoc = strDoc.Replace(".", "")
                                '    strDoc = strDoc.Substring(0, strDoc.Length - 2)
                                'Catch ex As Exception
                                '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('" & ex.Message & "');", True)
                                'End Try
                                If strFac = "47966" Then
                                    Dim intfac As Int32
                                    intfac = strFac
                                End If
                                If vec(2) <> "" Then
                                    strFac = CInt(vec(2))
                                Else
                                    strFac = vec(2)
                                End If
                                vlrSal = CInt(vec(3))

                                If strFac <> "" And vlrSal > 0 Then
                                    'strSQL = "Select vc.generador_nombre As Generador, "
                                    'strSQL += " vc.generador_documento as Documento,"
                                    'strSQL += " vc.venta_numero As NroFac, "
                                    'strSQL += " DATE_FORMAT(vc.venta_fecha,'%Y-%m-%d') as Fecha, "
                                    'strSQL += " DATE_FORMAT(vc.venta_vence,'%Y-%m-%d') as Vence, "
                                    'strSQL += " IFNULL((SELECT  SUM(vrk.valor) From ventas_recaudos vrf "
                                    'strSQL += " inner Join ventas_recaudos_detalle vrk On vrf.idventas_recaudos=vrk.ventas_recaudos_id "
                                    'strSQL += " where vrf.fecha_recaudo <='" & txtFechaInicio.Value & "' And vrk.ventas_idventas = vc.venta_id "
                                    'strSQL += " Group BY vrk.ventas_idventas),0) As 'abono', "
                                    'strSQL += "(vc.venta_total - IFNULL((SELECT  SUM(vrk.valor)  "
                                    'strSQL += " From ventas_recaudos vrf "
                                    'strSQL += " inner Join ventas_recaudos_detalle vrk  On vrf.idventas_recaudos=vrk.ventas_recaudos_id "
                                    'strSQL += " where vrf.fecha_recaudo <='" & txtFechaInicio.Value & "' And vrk.ventas_idventas = vc.venta_id "
                                    'strSQL += " Group BY vrk.ventas_idventas),0)) As 'Saldo'"
                                    'strSQL += " From ventas_consolidado vc  "
                                    'strSQL += " Left Join   ventas_recaudos_detalle vrd On vc.venta_id=vrd.ventas_idventas  "
                                    'strSQL += " Left Join   ventas_recaudos vr On vrd.ventas_recaudos_id=vr.idventas_recaudos "
                                    'strSQL += " where  vc.venta_fecha <='" & txtFechaInicio.Value & "'  AND vc.venta_estado='EMITIDA' AND vc.generador_documento = '" & strDoc & "'"
                                    'strSQL += " AND vc.venta_numero LIKE '%" & strFac & "%' And (vc.venta_total - IFNULL((Select  SUM(vrk.valor) "
                                    'strSQL += " From ventas_recaudos vrf inner Join ventas_recaudos_detalle vrk  On vrf.idventas_recaudos=vrk.ventas_recaudos_id "
                                    'strSQL += " Where vrf.fecha_recaudo <='" & txtFechaInicio.Value & "' "
                                    'strSQL += " And vrk.ventas_idventas = vc.venta_id Group By vrk.ventas_idventas),0))>0  "
                                    'strSQL += " Group By vc.venta_id Order By vc.generador_nombre, vc.venta_numero"

                                    'dtsal = csinformes.ejecutar_query_bd(strSQL)

                                    Dim result() As DataRow = dtter.Select("Documento = " & strDoc.ToString & " AND NroFac = " & strFac.ToString & "")
                                    'Dim result() As DataRow = dtter.Select("Documento = '" & strDoc.ToString & "' AND NroFac = '" & strFac.ToString & "'")
                                    'Dim result() As DataRow = dtter.Select("Documento = 900218342 AND NroFac = 8947")

                                    If result.Count > 0 Then
                                        If result(0)("Saldo") = vlrSal Then
                                            strHtmlmostrar += "<tr>"
                                        Else
                                            If (result(0)("Saldo") - vlrSal) > 100 Or (result(0)("Saldo") - vlrSal) < -100 Then
                                                strHtmlmostrar += "<tr bgcolor='#F5A9A9'>"
                                            Else
                                                strHtmlmostrar += "<tr>"
                                            End If
                                        End If

                                        strHtmlmostrar += "<td align='left'><font size='1px'>" & result(0)("Documento").ToString & "</font></td>"
                                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & result(0)("Generador").ToString & "</font></td>"
                                        strHtmlmostrar += "<td align='center'><font size='1px'>" & result(0)("NroFac").ToString & "</font></td>"
                                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", result(0)("Saldo")) & "</font></td>"
                                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", CInt(vlrSal)) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & result(0)("Saldo") & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & vlrSal & "</font></td>"

                                        strHtmlmostrar += "</tr>"
                                    Else
                                        strHtmlmostrar += "<tr bgcolor='#A9D0F5'>"

                                        strHtmlmostrar += "<td align='left'><font size='1px'>" & strDoc.ToString & "</font></td>"
                                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & strGen.ToString & "</font></td>"
                                        strHtmlmostrar += "<td align='center'><font size='1px'>" & strFac.ToString & "</font></td>"
                                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", CInt(vlrSal)) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>0</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & vlrSal & "</font></td>"

                                        strHtmlmostrar += "</tr>"
                                    End If
                                End If

                                line = sr.ReadLine()
                            Loop Until line Is Nothing
                            sr.Close()
                            'System.IO.File.Delete(filePath)

                            For i As Integer = 0 To dtter.Rows.Count - 1
                                If strHtmlmostrar.Contains(dtter.Rows(0)("NroFac").ToString) Then
                                    Dim strNoFac As String
                                    strNoFac = ""
                                Else
                                    strHtmlmostrar += "<tr bgcolor='#F2F5A9'>"

                                    strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Documento").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                                    strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("NroFac").ToString & "</font></td>"
                                    'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                    'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Saldo") & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>0</font></td>"

                                    strHtmlmostrar += "</tr>"
                                End If
                            Next

                            strHtmlmostrar += "</table>"

                            divmostrar.InnerHtml = strHtmlmostrar

                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transaccion Finalizada...');", True)
                        End If
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('El archivo a leer debe tener extensión .csv.');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
            End If
        Catch ex As Exception
            Dim intFact As Integer
            intFact = strFac
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
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

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        'Try
        '    Dim strNombreInforme As String

        '    strNombreInforme = "Informe Cartera Hasta: " & txtFechaInicio.Value

        '    Response.ContentType = "application/pdf"
        '    Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
        '    Response.Cache.SetCacheability(HttpCacheability.NoCache)
        '    Dim stringWriter As StringWriter = New StringWriter()
        '    Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
        '    divinforme.RenderControl(htmlTextWriter)
        '    Dim stringReader As StringReader = New StringReader(stringWriter.ToString())
        '    Dim Doc As Document = New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
        '    Dim htmlparser As HTMLWorker = New HTMLWorker(Doc)
        '    PdfWriter.GetInstance(Doc, Response.OutputStream)
        '    Doc.Open()
        '    htmlparser.Parse(stringReader)
        '    Doc.Close()
        '    Response.Write(Doc)
        '    Response.[End]()
        'Catch ex As Exception
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        'End Try
    End Sub
End Class
