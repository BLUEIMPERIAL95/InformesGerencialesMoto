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
Partial Class InfParamIngresosAcumulados
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes

    Private Sub InfParamIngresosAcumulados_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(19, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnAgencia_Click(sender As Object, e As EventArgs) Handles btnAgencia.Click
        Try
            If cboAno.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar.');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLAge, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLAge = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLAge = "Select sistema_sucursales.idsucursales, "
                strSQLAge += "sistema_sucursales.sucursal "
                strSQLAge += "From sistema_sucursales "
                strSQLAge += "Where sistema_sucursales.idel = 0 "
                strSQLAge += "Order By sistema_sucursales.descripcion "

                dtAge = csinformes.ejecutar_query_bd(strSQLAge)

                If dtAge.Rows.Count > 0 Then
                    Dim idSuc As Integer
                    Dim dblTotalAgen, dblTotalTotal, dblTotal1, dblTotal2, dblTotal3, dblTotal4, dblTotal5, dblTotal6, dblTotal7, dblTotal8, dblTotal9, dblTotal10, dblTotal11, dblTotal12 As Double
                    Dim strSuc As String

                    dblTotalTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO ACUMULADO POR AGENCIA</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Enero</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Febrero</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Marzo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Abril</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Mayo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Junio</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Julio</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Agosto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Septiembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Octubre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Noviembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Diciembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Total Agencia</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Enero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Febrero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Marzo</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Abril</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Mayo</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Junio</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Julio</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agosto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Septiembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Octubre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Noviembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Diciembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Total Agencia</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    dblTotal1 = 0
                    dblTotal2 = 0
                    dblTotal3 = 0
                    dblTotal4 = 0
                    dblTotal5 = 0
                    dblTotal6 = 0
                    dblTotal7 = 0
                    dblTotal8 = 0
                    dblTotal9 = 0
                    dblTotal10 = 0
                    dblTotal11 = 0
                    dblTotal12 = 0

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        idSuc = dtAge.Rows(i)("idsucursales")
                        strSuc = dtAge.Rows(i)("sucursal").ToString

                        strHtml += "<tr>"
                        strHtmlmostrar += "<tr>"

                        strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"

                        dblTotalAgen = 0
                        For j As Integer = 1 To 12
                            Dim dtValMes As New DataTable
                            strSQLMes = ""
                            If cboAno.SelectedValue < Year(Now) Then
                                strSQLMes = "Select COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                                strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                                strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                                strSQLMes += "From movimientos_transportes_consolidado "
                                strSQLMes += "Where movimientos_transportes_consolidado.sucorigina_id = " & idSuc & " And Year(factura_fecha) = " & cboAno.SelectedValue & " "
                                strSQLMes += " And Month(factura_fecha) = " & j & " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                                'strSQLMes += "COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                                dtValMes = csinformes.ejecutar_query_bd(strSQLMes)

                                If dtValMes.Rows.Count > 0 Then
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                    dblTotalAgen = dblTotalAgen + dtValMes.Rows(0)("ingreso")
                                    dblTotalTotal = dblTotalTotal + dtValMes.Rows(0)("ingreso")

                                    If j = 1 Then
                                        dblTotal1 = dblTotal1 + dtValMes.Rows(0)("ingreso")
                                    Else
                                        If j = 2 Then
                                            dblTotal2 = dblTotal2 + dtValMes.Rows(0)("ingreso")
                                        Else
                                            If j = 3 Then
                                                dblTotal3 = dblTotal3 + dtValMes.Rows(0)("ingreso")
                                            Else
                                                If j = 4 Then
                                                    dblTotal4 = dblTotal4 + dtValMes.Rows(0)("ingreso")
                                                Else
                                                    If j = 5 Then
                                                        dblTotal5 = dblTotal5 + dtValMes.Rows(0)("ingreso")
                                                    Else
                                                        If j = 6 Then
                                                            dblTotal6 = dblTotal6 + dtValMes.Rows(0)("ingreso")
                                                        Else
                                                            If j = 7 Then
                                                                dblTotal7 = dblTotal7 + dtValMes.Rows(0)("ingreso")
                                                            Else
                                                                If j = 8 Then
                                                                    dblTotal8 = dblTotal8 + dtValMes.Rows(0)("ingreso")
                                                                Else
                                                                    If j = 9 Then
                                                                        dblTotal9 = dblTotal9 + dtValMes.Rows(0)("ingreso")
                                                                    Else
                                                                        If j = 10 Then
                                                                            dblTotal10 = dblTotal10 + dtValMes.Rows(0)("ingreso")
                                                                        Else
                                                                            If j = 12 Then
                                                                                dblTotal11 = dblTotal11 + dtValMes.Rows(0)("ingreso")
                                                                            Else
                                                                                dblTotal12 = dblTotal12 + dtValMes.Rows(0)("ingreso")
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                End If
                            Else
                                If j <= Month(Now) And cboAno.SelectedValue = Year(Now) Then
                                    strSQLMes = "Select COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                                    strSQLMes += "From movimientos_transportes_consolidado "
                                    strSQLMes += "Where movimientos_transportes_consolidado.sucorigina_id = " & idSuc & " And Year(factura_fecha) = " & cboAno.SelectedValue & " "
                                    strSQLMes += " And Month(factura_fecha) = " & j & " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                                    'strSQLMes += " AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                                    dtValMes = csinformes.ejecutar_query_bd(strSQLMes)

                                    If dtValMes.Rows.Count > 0 Then
                                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                        dblTotalAgen = dblTotalAgen + dtValMes.Rows(0)("ingreso")
                                        dblTotalTotal = dblTotalTotal + dtValMes.Rows(0)("ingreso")

                                        If j = 1 Then
                                            dblTotal1 = dblTotal1 + dtValMes.Rows(0)("ingreso")
                                        Else
                                            If j = 2 Then
                                                dblTotal2 = dblTotal2 + dtValMes.Rows(0)("ingreso")
                                            Else
                                                If j = 3 Then
                                                    dblTotal3 = dblTotal3 + dtValMes.Rows(0)("ingreso")
                                                Else
                                                    If j = 4 Then
                                                        dblTotal4 = dblTotal4 + dtValMes.Rows(0)("ingreso")
                                                    Else
                                                        If j = 5 Then
                                                            dblTotal5 = dblTotal5 + dtValMes.Rows(0)("ingreso")
                                                        Else
                                                            If j = 6 Then
                                                                dblTotal6 = dblTotal6 + dtValMes.Rows(0)("ingreso")
                                                            Else
                                                                If j = 7 Then
                                                                    dblTotal7 = dblTotal7 + dtValMes.Rows(0)("ingreso")
                                                                Else
                                                                    If j = 8 Then
                                                                        dblTotal8 = dblTotal8 + dtValMes.Rows(0)("ingreso")
                                                                    Else
                                                                        If j = 9 Then
                                                                            dblTotal9 = dblTotal9 + dtValMes.Rows(0)("ingreso")
                                                                        Else
                                                                            If j = 10 Then
                                                                                dblTotal10 = dblTotal10 + dtValMes.Rows(0)("ingreso")
                                                                            Else
                                                                                If j = 12 Then
                                                                                    dblTotal11 = dblTotal11 + dtValMes.Rows(0)("ingreso")
                                                                                Else
                                                                                    dblTotal12 = dblTotal12 + dtValMes.Rows(0)("ingreso")
                                                                                End If
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    End If
                                Else
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                End If
                            End If
                        Next
                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalAgen, 0) & "</font></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalAgen, 0) & "</font></td>"
                        strHtmlmostrar += "</tr>"
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='right'><b><font size='9px'>TOTALES: </font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal1, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal2, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal3, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal4, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal5, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal6, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal7, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal8, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal9, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal10, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal11, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal12, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotalTotal, 0) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>TOTALES: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal1, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal2, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal3, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal4, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal5, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal6, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal7, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal8, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal9, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal10, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal11, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal12, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotalTotal, 0) & "</font></b></td>"
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

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=Acumulado.xls")
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

            strNombreInforme = "Acumulado"

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

    Private Sub btnComercial_Click(sender As Object, e As EventArgs) Handles btnComercial.Click
        Try
            If cboAno.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar.');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLCom, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLCom = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLCom = "Select DISTINCT asesor_nombre "
                strSQLCom += "From movimientos_transportes_consolidado "
                strSQLCom += "Where Year(factura_fecha) = " & cboAno.SelectedValue & " And movimiento_estado <> 'ANULADO' "
                strSQLCom += "ORDER BY asesor_nombre "

                dtAge = csinformes.ejecutar_query_bd(strSQLCom)

                If dtAge.Rows.Count > 0 Then
                    Dim dblTotalCom, dblTotalTotal, dblTotal1, dblTotal2, dblTotal3, dblTotal4, dblTotal5, dblTotal6, dblTotal7, dblTotal8, dblTotal9, dblTotal10, dblTotal11, dblTotal12 As Double
                    Dim strCom As String

                    dblTotalTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO ACUMULADO POR COMERCIAL</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Comercial</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Enero</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Febrero</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Marzo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Abril</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Mayo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Junio</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Julio</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Agosto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Septiembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Octubre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Noviembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Diciembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Total Agencia</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Comercial</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Enero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Febrero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Marzo</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Abril</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Mayo</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Junio</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Julio</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agosto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Septiembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Octubre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Noviembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Diciembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Total Agencia</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    dblTotal1 = 0
                    dblTotal2 = 0
                    dblTotal3 = 0
                    dblTotal4 = 0
                    dblTotal5 = 0
                    dblTotal6 = 0
                    dblTotal7 = 0
                    dblTotal8 = 0
                    dblTotal9 = 0
                    dblTotal10 = 0
                    dblTotal11 = 0
                    dblTotal12 = 0

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        strCom = dtAge.Rows(i)("asesor_nombre").ToString

                        strHtml += "<tr>"
                        strHtmlmostrar += "<tr>"

                        strHtml += "<td align='left'><font size='9px'>" & strCom & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & strCom & "</font></td>"

                        dblTotalCom = 0
                        For j As Integer = 1 To 12
                            Dim dtValMes As New DataTable
                            strSQLMes = ""
                            If cboAno.SelectedValue < Year(Now) Then
                                strSQLMes = "Select COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                                strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                                strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                                strSQLMes += "From movimientos_transportes_consolidado "
                                strSQLMes += "Where movimientos_transportes_consolidado.asesor_nombre = '" & strCom & "' And Year(factura_fecha) = " & cboAno.SelectedValue & " "
                                strSQLMes += " And Month(factura_fecha) = " & j & " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                                'strSQLMes += "COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                                dtValMes = csinformes.ejecutar_query_bd(strSQLMes)

                                If dtValMes.Rows.Count > 0 Then
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                    dblTotalCom = dblTotalCom + dtValMes.Rows(0)("ingreso")
                                    dblTotalTotal = dblTotalTotal + dtValMes.Rows(0)("ingreso")

                                    If j = 1 Then
                                        dblTotal1 = dblTotal1 + dtValMes.Rows(0)("ingreso")
                                    Else
                                        If j = 2 Then
                                            dblTotal2 = dblTotal2 + dtValMes.Rows(0)("ingreso")
                                        Else
                                            If j = 3 Then
                                                dblTotal3 = dblTotal3 + dtValMes.Rows(0)("ingreso")
                                            Else
                                                If j = 4 Then
                                                    dblTotal4 = dblTotal4 + dtValMes.Rows(0)("ingreso")
                                                Else
                                                    If j = 5 Then
                                                        dblTotal5 = dblTotal5 + dtValMes.Rows(0)("ingreso")
                                                    Else
                                                        If j = 6 Then
                                                            dblTotal6 = dblTotal6 + dtValMes.Rows(0)("ingreso")
                                                        Else
                                                            If j = 7 Then
                                                                dblTotal7 = dblTotal7 + dtValMes.Rows(0)("ingreso")
                                                            Else
                                                                If j = 8 Then
                                                                    dblTotal8 = dblTotal8 + dtValMes.Rows(0)("ingreso")
                                                                Else
                                                                    If j = 9 Then
                                                                        dblTotal9 = dblTotal9 + dtValMes.Rows(0)("ingreso")
                                                                    Else
                                                                        If j = 10 Then
                                                                            dblTotal10 = dblTotal10 + dtValMes.Rows(0)("ingreso")
                                                                        Else
                                                                            If j = 12 Then
                                                                                dblTotal11 = dblTotal11 + dtValMes.Rows(0)("ingreso")
                                                                            Else
                                                                                dblTotal12 = dblTotal12 + dtValMes.Rows(0)("ingreso")
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                End If
                            Else
                                If j <= Month(Now) And cboAno.SelectedValue = Year(Now) Then
                                    strSQLMes = "Select COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                                    strSQLMes += "From movimientos_transportes_consolidado "
                                    strSQLMes += "Where movimientos_transportes_consolidado.asesor_nombre = '" & strCom & "' And Year(factura_fecha) = " & cboAno.SelectedValue & " "
                                    strSQLMes += " And Month(factura_fecha) = " & j & " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                                    'strSQLMes += " AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                                    dtValMes = csinformes.ejecutar_query_bd(strSQLMes)

                                    If dtValMes.Rows.Count > 0 Then
                                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                        dblTotalCom = dblTotalCom + dtValMes.Rows(0)("ingreso")
                                        dblTotalTotal = dblTotalTotal + dtValMes.Rows(0)("ingreso")

                                        If j = 1 Then
                                            dblTotal1 = dblTotal1 + dtValMes.Rows(0)("ingreso")
                                        Else
                                            If j = 2 Then
                                                dblTotal2 = dblTotal2 + dtValMes.Rows(0)("ingreso")
                                            Else
                                                If j = 3 Then
                                                    dblTotal3 = dblTotal3 + dtValMes.Rows(0)("ingreso")
                                                Else
                                                    If j = 4 Then
                                                        dblTotal4 = dblTotal4 + dtValMes.Rows(0)("ingreso")
                                                    Else
                                                        If j = 5 Then
                                                            dblTotal5 = dblTotal5 + dtValMes.Rows(0)("ingreso")
                                                        Else
                                                            If j = 6 Then
                                                                dblTotal6 = dblTotal6 + dtValMes.Rows(0)("ingreso")
                                                            Else
                                                                If j = 7 Then
                                                                    dblTotal7 = dblTotal7 + dtValMes.Rows(0)("ingreso")
                                                                Else
                                                                    If j = 8 Then
                                                                        dblTotal8 = dblTotal8 + dtValMes.Rows(0)("ingreso")
                                                                    Else
                                                                        If j = 9 Then
                                                                            dblTotal9 = dblTotal9 + dtValMes.Rows(0)("ingreso")
                                                                        Else
                                                                            If j = 10 Then
                                                                                dblTotal10 = dblTotal10 + dtValMes.Rows(0)("ingreso")
                                                                            Else
                                                                                If j = 12 Then
                                                                                    dblTotal11 = dblTotal11 + dtValMes.Rows(0)("ingreso")
                                                                                Else
                                                                                    dblTotal12 = dblTotal12 + dtValMes.Rows(0)("ingreso")
                                                                                End If
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    End If
                                Else
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                End If
                            End If
                        Next
                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalCom, 0) & "</font></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalCom, 0) & "</font></td>"
                        strHtmlmostrar += "</tr>"
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='right'><b><font size='9px'>TOTALES: </font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal1, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal2, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal3, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal4, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal5, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal6, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal7, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal8, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal9, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal10, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal11, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal12, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotalTotal, 0) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>TOTALES: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal1, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal2, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal3, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal4, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal5, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal6, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal7, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal8, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal9, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal10, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal11, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal12, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotalTotal, 0) & "</font></b></td>"
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

    Private Sub btnDespachador_Click(sender As Object, e As EventArgs) Handles btnDespachador.Click
        Try
            If cboAno.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar.');", True)
            Else
                Dim dtAge As New DataTable
                Dim strSQLCom, strHtml, strHtmlmostrar, strSQLMes As String
                strSQLCom = ""
                strHtml = ""
                strHtmlmostrar = ""
                strSQLMes = ""
                divinforme.InnerHtml = ""
                divmostrar.InnerHtml = ""

                strSQLCom = "Select DISTINCT despachador_nombre "
                strSQLCom += "From movimientos_transportes_consolidado "
                strSQLCom += "Where Year(factura_fecha) = " & cboAno.SelectedValue & " And movimiento_estado <> 'ANULADO' "
                strSQLCom += "ORDER BY despachador_nombre "

                dtAge = csinformes.ejecutar_query_bd(strSQLCom)

                If dtAge.Rows.Count > 0 Then
                    Dim dblTotalCom, dblTotalTotal, dblTotal1, dblTotal2, dblTotal3, dblTotal4, dblTotal5, dblTotal6, dblTotal7, dblTotal8, dblTotal9, dblTotal10, dblTotal11, dblTotal12 As Double
                    Dim strCom As String

                    dblTotalTotal = 0

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
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO ACUMULADO POR DESPACHADOR</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Despachador</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Enero</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Febrero</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Marzo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Abril</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Mayo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Junio</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Julio</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Agosto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Septiembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Octubre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Noviembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Diciembre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Total Agencia</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Despachador</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Enero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Febrero</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Marzo</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Abril</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Mayo</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Junio</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Julio</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Agosto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Septiembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Octubre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Noviembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Diciembre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Total Agencia</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    dblTotal1 = 0
                    dblTotal2 = 0
                    dblTotal3 = 0
                    dblTotal4 = 0
                    dblTotal5 = 0
                    dblTotal6 = 0
                    dblTotal7 = 0
                    dblTotal8 = 0
                    dblTotal9 = 0
                    dblTotal10 = 0
                    dblTotal11 = 0
                    dblTotal12 = 0

                    For i As Integer = 0 To dtAge.Rows.Count - 1
                        strCom = dtAge.Rows(i)("despachador_nombre").ToString

                        strHtml += "<tr>"
                        strHtmlmostrar += "<tr>"

                        strHtml += "<td align='left'><font size='9px'>" & strCom & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='1px'>" & strCom & "</font></td>"

                        dblTotalCom = 0
                        For j As Integer = 1 To 12
                            Dim dtValMes As New DataTable
                            strSQLMes = ""
                            If cboAno.SelectedValue < Year(Now) Then
                                strSQLMes = "Select COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                                strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                                strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                                strSQLMes += "From movimientos_transportes_consolidado "
                                strSQLMes += "Where movimientos_transportes_consolidado.despachador_nombre = '" & strCom & "' And Year(factura_fecha) = " & cboAno.SelectedValue & " "
                                strSQLMes += " And Month(factura_fecha) = " & j & " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                                'strSQLMes += "COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                                dtValMes = csinformes.ejecutar_query_bd(strSQLMes)

                                If dtValMes.Rows.Count > 0 Then
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                    dblTotalCom = dblTotalCom + dtValMes.Rows(0)("ingreso")
                                    dblTotalTotal = dblTotalTotal + dtValMes.Rows(0)("ingreso")

                                    If j = 1 Then
                                        dblTotal1 = dblTotal1 + dtValMes.Rows(0)("ingreso")
                                    Else
                                        If j = 2 Then
                                            dblTotal2 = dblTotal2 + dtValMes.Rows(0)("ingreso")
                                        Else
                                            If j = 3 Then
                                                dblTotal3 = dblTotal3 + dtValMes.Rows(0)("ingreso")
                                            Else
                                                If j = 4 Then
                                                    dblTotal4 = dblTotal4 + dtValMes.Rows(0)("ingreso")
                                                Else
                                                    If j = 5 Then
                                                        dblTotal5 = dblTotal5 + dtValMes.Rows(0)("ingreso")
                                                    Else
                                                        If j = 6 Then
                                                            dblTotal6 = dblTotal6 + dtValMes.Rows(0)("ingreso")
                                                        Else
                                                            If j = 7 Then
                                                                dblTotal7 = dblTotal7 + dtValMes.Rows(0)("ingreso")
                                                            Else
                                                                If j = 8 Then
                                                                    dblTotal8 = dblTotal8 + dtValMes.Rows(0)("ingreso")
                                                                Else
                                                                    If j = 9 Then
                                                                        dblTotal9 = dblTotal9 + dtValMes.Rows(0)("ingreso")
                                                                    Else
                                                                        If j = 10 Then
                                                                            dblTotal10 = dblTotal10 + dtValMes.Rows(0)("ingreso")
                                                                        Else
                                                                            If j = 12 Then
                                                                                dblTotal11 = dblTotal11 + dtValMes.Rows(0)("ingreso")
                                                                            Else
                                                                                dblTotal12 = dblTotal12 + dtValMes.Rows(0)("ingreso")
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Else
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                End If
                            Else
                                If j <= Month(Now) And cboAno.SelectedValue = Year(Now) Then
                                    strSQLMes = "Select COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                                    strSQLMes += "From movimientos_transportes_consolidado "
                                    strSQLMes += "Where movimientos_transportes_consolidado.despachador_nombre = '" & strCom & "' And Year(factura_fecha) = " & cboAno.SelectedValue & " "
                                    strSQLMes += " And Month(factura_fecha) = " & j & " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                                    'strSQLMes += " AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                                    dtValMes = csinformes.ejecutar_query_bd(strSQLMes)

                                    If dtValMes.Rows.Count > 0 Then
                                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtValMes.Rows(0)("ingreso"), 0) & "</font></td>"
                                        dblTotalCom = dblTotalCom + dtValMes.Rows(0)("ingreso")
                                        dblTotalTotal = dblTotalTotal + dtValMes.Rows(0)("ingreso")

                                        If j = 1 Then
                                            dblTotal1 = dblTotal1 + dtValMes.Rows(0)("ingreso")
                                        Else
                                            If j = 2 Then
                                                dblTotal2 = dblTotal2 + dtValMes.Rows(0)("ingreso")
                                            Else
                                                If j = 3 Then
                                                    dblTotal3 = dblTotal3 + dtValMes.Rows(0)("ingreso")
                                                Else
                                                    If j = 4 Then
                                                        dblTotal4 = dblTotal4 + dtValMes.Rows(0)("ingreso")
                                                    Else
                                                        If j = 5 Then
                                                            dblTotal5 = dblTotal5 + dtValMes.Rows(0)("ingreso")
                                                        Else
                                                            If j = 6 Then
                                                                dblTotal6 = dblTotal6 + dtValMes.Rows(0)("ingreso")
                                                            Else
                                                                If j = 7 Then
                                                                    dblTotal7 = dblTotal7 + dtValMes.Rows(0)("ingreso")
                                                                Else
                                                                    If j = 8 Then
                                                                        dblTotal8 = dblTotal8 + dtValMes.Rows(0)("ingreso")
                                                                    Else
                                                                        If j = 9 Then
                                                                            dblTotal9 = dblTotal9 + dtValMes.Rows(0)("ingreso")
                                                                        Else
                                                                            If j = 10 Then
                                                                                dblTotal10 = dblTotal10 + dtValMes.Rows(0)("ingreso")
                                                                            Else
                                                                                If j = 12 Then
                                                                                    dblTotal11 = dblTotal11 + dtValMes.Rows(0)("ingreso")
                                                                                Else
                                                                                    dblTotal12 = dblTotal12 + dtValMes.Rows(0)("ingreso")
                                                                                End If
                                                                            End If
                                                                        End If
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    End If
                                Else
                                    strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                End If
                            End If
                        Next
                        strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalCom, 0) & "</font></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalCom, 0) & "</font></td>"
                        strHtmlmostrar += "</tr>"
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='right'><b><font size='9px'>TOTALES: </font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal1, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal2, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal3, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal4, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal5, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal6, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal7, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal8, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal9, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal10, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal11, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotal12, 0) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", dblTotalTotal, 0) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>TOTALES: </font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal1, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal2, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal3, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal4, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal5, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal6, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal7, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal8, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal9, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal10, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal11, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotal12, 0) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dblTotalTotal, 0) & "</font></b></td>"
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
