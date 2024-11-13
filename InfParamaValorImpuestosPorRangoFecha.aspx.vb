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
Partial Class InfParamaValorImpuestosPorRangoFecha
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes

    Private Sub InfParamaValorImpuestosPorRangoFecha_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(36, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=Ingreso.xls")
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

            strNombreInforme = "Ingreso"

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

    Private Sub btnIca_Click(sender As Object, e As EventArgs) Handles btnIca.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLAge, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLAge = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLAge = "Select DISTINCT movimientos_transportes_consolidado.sucorigina_id, "
                strSQLAge += "movimientos_transportes_consolidado.sucorigina_nombre "
                strSQLAge += "From movimientos_transportes_consolidado "
                strSQLAge += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQLAge += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                strSQLAge += "ORDER BY movimientos_transportes_consolidado.sucorigina_nombre "

                dtAge = csinformes.ejecutar_query_bd(strSQLAge)

                If dtAge.Rows.Count > 0 Then
                    Dim idSuc As Integer
                    Dim dblTotalAgen, dblTotal As Double
                    Dim strSuc As String

                    dblTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>VALOR ICA POR RANGO DE FECHAS DESDE: " & txtFechaInicio.Value & " HASTA: " & txtFechaFin.Value & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Valor Ica</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Valor Ica</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        Dim dtVal As New DataTable
                        idSuc = dtAge.Rows(i)("sucorigina_id")
                        strSuc = dtAge.Rows(i)("sucorigina_nombre").ToString
                        strSQLMes = ""

                        strSQLMes = "Select SUM(movimiento_ica) As ICA "
                        strSQLMes += "From movimientos_transportes_consolidado "
                        strSQLMes += "Where movimientos_transportes_consolidado.sucorigina_id = " & idSuc & " "
                        strSQLMes += "And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                        strSQLMes += "And movimientos_transportes_consolidado.movimiento_estado = 'CAUSADO' "

                        dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                        If dtVal.Rows.Count > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("ICA"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("ICA"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"

                            dblTotal = dblTotal + dtVal.Rows(0)("ICA")
                        Else
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "</table>"
                    strHtmlmostrar += "</table>"

                    divinforme.InnerHtml = strHtml
                    divmostrar.InnerHtml = strHtmlmostrar
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnRetencion_Click(sender As Object, e As EventArgs) Handles btnRetencion.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLAge, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLAge = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLAge = "Select DISTINCT movimientos_transportes_consolidado.sucorigina_id, "
                strSQLAge += "movimientos_transportes_consolidado.sucorigina_nombre "
                strSQLAge += "From movimientos_transportes_consolidado "
                strSQLAge += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQLAge += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                strSQLAge += "ORDER BY movimientos_transportes_consolidado.sucorigina_nombre "

                dtAge = csinformes.ejecutar_query_bd(strSQLAge)

                If dtAge.Rows.Count > 0 Then
                    Dim idSuc As Integer
                    Dim dblTotalAgen, dblTotal As Double
                    Dim strSuc As String

                    dblTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>VALOR RETENCION POR RANGO DE FECHAS DESDE: " & txtFechaInicio.Value & " HASTA: " & txtFechaFin.Value & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Valor Retención</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Valor Retención</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        Dim dtVal As New DataTable
                        idSuc = dtAge.Rows(i)("sucorigina_id")
                        strSuc = dtAge.Rows(i)("sucorigina_nombre").ToString
                        strSQLMes = ""

                        strSQLMes = "Select SUM(movimiento_rtefte) As Rte "
                        strSQLMes += "From movimientos_transportes_consolidado "
                        strSQLMes += "Where movimientos_transportes_consolidado.sucorigina_id = " & idSuc & " "
                        strSQLMes += "And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                        strSQLMes += "And movimientos_transportes_consolidado.movimiento_estado = 'CAUSADO' "

                        dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                        If dtVal.Rows.Count > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Rte"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Rte"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"

                            dblTotal = dblTotal + dtVal.Rows(0)("Rte")
                        Else
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "</table>"
                    strHtmlmostrar += "</table>"

                    divinforme.InnerHtml = strHtml
                    divmostrar.InnerHtml = strHtmlmostrar
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnIva_Click(sender As Object, e As EventArgs) Handles btnIva.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLAge, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLAge = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLAge = "Select DISTINCT ventas_consolidado.sucursal_elabora_id, "
                strSQLAge += "ventas_consolidado.sucursal_elabora_descripcion "
                strSQLAge += "From ventas_consolidado "
                strSQLAge += "Where ventas_consolidado.venta_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQLAge += "And ventas_consolidado.idtipo_estados = 18 "
                strSQLAge += "ORDER BY ventas_consolidado.sucursal_elabora_descripcion "

                dtAge = csinformes.ejecutar_query_bd(strSQLAge)

                If dtAge.Rows.Count > 0 Then
                    Dim idSuc As Integer
                    Dim dblTotalAgen, dblTotal As Double
                    Dim strSuc As String

                    dblTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>VALOR IVA POR RANGO DE FECHAS DESDE: " & txtFechaInicio.Value & " HASTA: " & txtFechaFin.Value & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Valor Iva</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Valor Iva</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        Dim dtVal As New DataTable
                        idSuc = dtAge.Rows(i)("sucursal_elabora_id")
                        strSuc = dtAge.Rows(i)("sucursal_elabora_descripcion").ToString
                        strSQLMes = ""

                        strSQLMes = "Select SUM(CASE WHEN COALESCE(ventas_detalles.iva, 0) > 0 Then (ventas_detalles.valor_total * (ventas_detalles.iva / 100)) Else 0 End) As valor_iva "
                        strSQLMes += "From ventas "
                        strSQLMes += "Left Join ventas_detalles ON ventas_detalles.ventas_idventas = ventas.idventas "
                        strSQLMes += "WHERE ventas.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                        strSQLMes += "And ventas.sistema_sucursales_elaboracion = " & idSuc & " "
                        strSQLMes += "And ventas.tipo_estados_idtipo = 18 "

                        dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                        If dtVal.Rows.Count > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("valor_iva"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("valor_iva"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"

                            dblTotal = dblTotal + dtVal.Rows(0)("valor_iva")
                        Else
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "</table>"
                    strHtmlmostrar += "</table>"

                    divinforme.InnerHtml = strHtml
                    divmostrar.InnerHtml = strHtmlmostrar
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnReteIva_Click(sender As Object, e As EventArgs) Handles btnReteIva.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLAge, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLAge = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLAge = "Select DISTINCT ventas_consolidado.sucursal_elabora_id, "
                strSQLAge += "ventas_consolidado.sucursal_elabora_descripcion "
                strSQLAge += "From ventas_consolidado "
                strSQLAge += "Where ventas_consolidado.venta_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQLAge += "And ventas_consolidado.idtipo_estados = 18 "
                strSQLAge += "ORDER BY ventas_consolidado.sucursal_elabora_descripcion "

                dtAge = csinformes.ejecutar_query_bd(strSQLAge)

                If dtAge.Rows.Count > 0 Then
                    Dim idSuc As Integer
                    Dim dblTotalAgen, dblTotal As Double
                    Dim strSuc As String

                    dblTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>VALOR RETE IVA POR RANGO DE FECHAS DESDE: " & txtFechaInicio.Value & " HASTA: " & txtFechaFin.Value & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Valor Rete Iva</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Valor Rete Iva</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        Dim dtVal As New DataTable
                        idSuc = dtAge.Rows(i)("sucursal_elabora_id")
                        strSuc = dtAge.Rows(i)("sucursal_elabora_descripcion").ToString
                        strSQLMes = ""

                        strSQLMes = "Select SUM(CASE WHEN COALESCE(ventas_detalles.rte_iva, 0) = 0 THEN 0 ELSE ((ventas_detalles.valor_total * (ventas_detalles.iva / 100)) * (ventas_detalles.rte_iva / 100)) END) As valor_riv "
                        strSQLMes += "From ventas "
                        strSQLMes += "Left Join ventas_detalles ON ventas_detalles.ventas_idventas = ventas.idventas "
                        strSQLMes += "WHERE ventas.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                        strSQLMes += "And ventas.sistema_sucursales_elaboracion = " & idSuc & " "
                        strSQLMes += "And ventas.tipo_estados_idtipo = 18 "

                        dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                        If dtVal.Rows.Count > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("valor_riv"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("valor_riv"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"

                            dblTotal = dblTotal + dtVal.Rows(0)("valor_riv")
                        Else
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "</table>"
                    strHtmlmostrar += "</table>"

                    divinforme.InnerHtml = strHtml
                    divmostrar.InnerHtml = strHtmlmostrar
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
