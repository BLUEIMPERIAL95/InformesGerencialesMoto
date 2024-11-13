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
Partial Class InfParamIngresosRangoFechas
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

        strRespuestaPer = csusua.validar_permiso_usuario(36, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtgen As New DataTable

        csoper.LlenarDropDownList("nombrecompleto", "nit", "generadores_mostrar_todos", dtgen, cbogeneradores)
    End Sub

    Private Sub btnAgencia_Click(sender As Object, e As EventArgs) Handles btnAgencia.Click
        Try
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
                Dim dblTotalAgen, dblTotal, dblTotalF, dblTotalP, dblTotalG As Double
                Dim strSuc As String

                dblTotal = 0
                dblTotalF = 0
                dblTotalP = 0
                dblTotalG = 0

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
                strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO POR RANGO DE FECHAS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Facturado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Pagado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Gasto</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Ingreso</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Facturado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Pagado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Gasto</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Ingreso</font></b></td>"
                strHtmlmostrar += "</tr>"

                For i As Integer = 0 To dtAge.Rows.Count - 1
                    Dim dtVal As New DataTable
                    idSuc = dtAge.Rows(i)("idsucursales")
                    strSuc = dtAge.Rows(i)("sucursal").ToString
                    strSQLMes = ""

                    strSQLMes = "Select "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)), 0) As Facturado, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0)), 0) As Pagado, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As Gasto, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                    strSQLMes += "From movimientos_transportes_consolidado "
                    strSQLMes += "Where movimientos_transportes_consolidado.sucorigina_id = " & idSuc & " And factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strSQLMes += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                    'strSQLMes += "COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                    If cbogeneradores.SelectedValue > 0 Then
                        strSQLMes += " AND movimientos_transportes_consolidado.generador_documento = " & cbogeneradores.SelectedValue & ""
                    End If

                    dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                    If dtVal.Rows.Count > 0 Then
                        If dtVal.Rows(0)("Facturado") > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Facturado"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Pagado"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Gasto"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("ingreso"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Facturado"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Pagado"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Gasto"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("ingreso"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If

                        dblTotal = dblTotal + dtVal.Rows(0)("ingreso")
                        dblTotalF = dblTotalF + dtVal.Rows(0)("Facturado")
                        dblTotalP = dblTotalP + dtVal.Rows(0)("Pagado")
                        dblTotalG = dblTotalG + dtVal.Rows(0)("Gasto")
                    Else
                        'strHtml += "<tr>"
                        'strHtml += "<td align='left'><font size='9px'>" & strSuc & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "</tr>"

                        'strHtmlmostrar += "<tr>"
                        'strHtmlmostrar += "<td align='left'><font size='1px'>" & strSuc & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "</tr>"
                    End If
                Next

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalF, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalP, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalG, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalF, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalP, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalG, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "</table>"
                strHtmlmostrar += "</table>"

                divinforme.InnerHtml = strHtml
                divmostrar.InnerHtml = strHtmlmostrar
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
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

    Private Sub btnComercial_Click(sender As Object, e As EventArgs) Handles btnComercial.Click
        Try
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
            strSQLCom += "Where movimiento_estado <> 'ANULADO' And movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQLCom += "ORDER BY asesor_nombre "

            dtAge = csinformes.ejecutar_query_bd(strSQLCom)

            If dtAge.Rows.Count > 0 Then
                Dim idSuc As Integer
                Dim dblTotalAgen, dblTotal, dblTotalF, dblTotalP, dblTotalG As Double
                Dim strCom As String

                dblTotal = 0
                dblTotalF = 0
                dblTotalP = 0
                dblTotalG = 0

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
                strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO POR RANGO DE FECHAS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Facturado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Pagado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Gasto</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Ingreso</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Facturado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Pagado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Gasto</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Ingreso</font></b></td>"
                strHtmlmostrar += "</tr>"

                For i As Integer = 0 To dtAge.Rows.Count - 1
                    Dim dtVal As New DataTable
                    strCom = dtAge.Rows(i)("asesor_nombre").ToString
                    strSQLMes = ""

                    strSQLMes = "Select "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)), 0) As Facturado, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0)), 0) As Pagado, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As Gasto, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                    strSQLMes += "From movimientos_transportes_consolidado "
                    strSQLMes += "Where movimientos_transportes_consolidado.asesor_nombre = '" & strCom & "' And factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strSQLMes += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                    'strSQLMes += "COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                    If cbogeneradores.SelectedValue > 0 Then
                        strSQLMes += " AND movimientos_transportes_consolidado.generador_documento = " & cbogeneradores.SelectedValue & ""
                    End If

                    dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                    If dtVal.Rows.Count > 0 Then
                        If dtVal.Rows(0)("Facturado") > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strCom & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Facturado"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Pagado"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Gasto"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("ingreso"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strCom & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Facturado"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Pagado"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Gasto"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("ingreso"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If

                        dblTotal = dblTotal + dtVal.Rows(0)("ingreso")
                        dblTotalF = dblTotalF + dtVal.Rows(0)("Facturado")
                        dblTotalP = dblTotalP + dtVal.Rows(0)("Pagado")
                        dblTotalG = dblTotalG + dtVal.Rows(0)("Gasto")
                    Else
                        'strHtml += "<tr>"
                        'strHtml += "<td align='left'><font size='9px'>" & strCom & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "</tr>"

                        'strHtmlmostrar += "<tr>"
                        'strHtmlmostrar += "<td align='left'><font size='1px'>" & strCom & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "</tr>"
                    End If
                Next

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalF, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalP, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalG, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalF, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalP, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalG, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "</table>"
                strHtmlmostrar += "</table>"

                divinforme.InnerHtml = strHtml
                divmostrar.InnerHtml = strHtmlmostrar
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnDespachador_Click(sender As Object, e As EventArgs) Handles btnDespachador.Click
        Try
            Dim dtAge As New DataTable
            Dim strSQLCom, strHtml, strHtmlmostrar, strSQLMes As String
            strSQLCom = ""
            strHtml = ""
            strHtmlmostrar = ""
            strSQLMes = ""
            divinforme.InnerHtml = ""
            divmostrar.InnerHtml = ""

            strSQLCom = "Select DISTINCT COALESCE(despachador_nombre, 'NA') AS despachador_nombre "
            strSQLCom += "From movimientos_transportes_consolidado "
            strSQLCom += "Where movimiento_estado <> 'ANULADO' And movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQLCom += "ORDER BY despachador_nombre "

            dtAge = csinformes.ejecutar_query_bd(strSQLCom)

            If dtAge.Rows.Count > 0 Then
                Dim dblTotalAgen, dblTotal, dblTotalF, dblTotalP, dblTotalG As Double
                Dim strCom As String

                dblTotal = 0
                dblTotalF = 0
                dblTotalP = 0
                dblTotalG = 0

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
                strHtml += "<td align='center' colspan='5'><b><font size='4'>INGRESO POR RANGO DE FECHAS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='9px'>Agencia</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Facturado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Pagado</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Gasto</font></b></td>"
                strHtml += "<td align='center'><b><font size='9px'>Ingreso</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Agencia</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Facturado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Pagado</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Gasto</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Ingreso</font></b></td>"
                strHtmlmostrar += "</tr>"

                For i As Integer = 0 To dtAge.Rows.Count - 1
                    Dim dtVal As New DataTable
                    strCom = dtAge.Rows(i)("despachador_nombre").ToString
                    strSQLMes = ""

                    strSQLMes = "Select "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)), 0) As Facturado, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0)), 0) As Pagado, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As Gasto, "
                    strSQLMes += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                    strSQLMes += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As ingreso "
                    strSQLMes += "From movimientos_transportes_consolidado "
                    strSQLMes += "Where COALESCE(movimientos_transportes_consolidado.despachador_nombre, 'NA') = '" & strCom & "' And factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strSQLMes += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                    'strSQLMes += "COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "

                    If cbogeneradores.SelectedValue > 0 Then
                        strSQLMes += " AND movimientos_transportes_consolidado.generador_documento = " & cbogeneradores.SelectedValue & ""
                    End If

                    dtVal = csinformes.ejecutar_query_bd(strSQLMes)

                    If dtVal.Rows.Count > 0 Then
                        If dtVal.Rows(0)("Facturado") > 0 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='9px'>" & strCom & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Facturado"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Pagado"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("Gasto"), 0) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtVal.Rows(0)("ingreso"), 0) & "</font></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & strCom & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Facturado"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Pagado"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("Gasto"), 0) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtVal.Rows(0)("ingreso"), 0) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                        End If

                        dblTotal = dblTotal + dtVal.Rows(0)("ingreso")
                        dblTotalF = dblTotalF + dtVal.Rows(0)("Facturado")
                        dblTotalP = dblTotalP + dtVal.Rows(0)("Pagado")
                        dblTotalG = dblTotalG + dtVal.Rows(0)("Gasto")
                    Else
                        'strHtml += "<tr>"
                        'strHtml += "<td align='left'><font size='9px'>" & strCom & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtml += "</tr>"

                        'strHtmlmostrar += "<tr>"
                        'strHtmlmostrar += "<td align='left'><font size='1px'>" & strCom & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        'strHtmlmostrar += "</tr>"
                    End If
                Next

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='9px'>TOTAL: </font></b></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalF, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalP, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotalG, 0) & "</font></td>"
                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalF, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalP, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotalG, 0) & "</font></td>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dblTotal, 0) & "</font></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "</table>"
                strHtmlmostrar += "</table>"

                divinforme.InnerHtml = strHtml
                divmostrar.InnerHtml = strHtmlmostrar
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
