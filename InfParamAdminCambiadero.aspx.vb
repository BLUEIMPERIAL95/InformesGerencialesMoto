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

Partial Class InfParamAdminCambiadero
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim cscamb As New cambiadero

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("AdmistrarEgresos.aspx")
    End Sub

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Try
            Dim dtCam As New DataTable

            If txtFechaInicio.Value = "" And txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                Dim strHtml, strHtmlmostrar As String
                strHtml = ""
                strHtmlmostrar = ""

                dtCam = cscamb.seleccionar_admin_cambiadero_por_usuario_fecha(txtFechaInicio.Value, txtFechaFin.Value, Session("id_usua"))

                If dtCam.Rows.Count > 0 Then
                    Dim pathimgCabeza1 As String
                    Dim urlFotoCabeza1 As String = ""

                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.png")

                    If File.Exists(pathimgCabeza1) Then
                        urlFotoCabeza1 = pathimgCabeza1
                    Else
                        urlFotoCabeza1 = Path.Combine(Server.MapPath("images"), "nophoto.jpg")
                    End If

                    strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' colspan='7'><b><font size='1px'>Informe Control de pagos Cambiadero</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F9D2C6'>"
                    strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Empresa</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Agencia</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Egreso</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Tercero</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Usuario</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtmlmostrar += "<td align='center' colspan='6'><b><font size='12px'>Informe Control de pagos Cambiadero</font></b></td>"
                    strHtmlmostrar += "</tr>"
                    strHtmlmostrar += "<tr bgcolor='#F9D2C6'>"
                    strHtmlmostrar += "<td align='center'><b><font size='8px'>Fecha</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='8px'>Empresa</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='8px'>Agencia</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='8px'>Egreso</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='8px'>Tercero</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='8px'>Valor</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='8px'>Usuario</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    Dim strEmpActual As String
                    strEmpActual = dtCam.Rows(0)("nombre_emor").ToString
                    Dim valorempr, valortot, descemp, desctot, totalemp, totaltot As Integer
                    valorempr = 0
                    valortot = 0
                    descemp = 0
                    desctot = 0
                    totalemp = 0
                    totaltot = 0
                    For i As Integer = 0 To dtCam.Rows.Count - 1

                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='1px'>" & dtCam.Rows(i)("fecha_adca").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("nombre_emor").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("nombre_agcc").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("egreso_adca").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("tercero_adca").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("valor_adca")) & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("usuario").ToString & "</font></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='center'><font size='8px'>" & dtCam.Rows(i)("fecha_adca").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='8px'>" & dtCam.Rows(i)("nombre_emor").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='8px'>" & dtCam.Rows(i)("nombre_agcc").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='8px'>" & dtCam.Rows(i)("egreso_adca").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='8px'>" & dtCam.Rows(i)("tercero_adca").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='8px'>" & String.Format("{0:c}", dtCam.Rows(i)("valor_adca")) & "</font></td>"
                        strHtmlmostrar += "<td align='left'><font size='8px'>" & dtCam.Rows(i)("usuario").ToString & "</font></td>"
                        strHtmlmostrar += "</tr>"

                        valortot = valortot + dtCam.Rows(i)("valor_adca")
                    Next

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='center' colspan='6'><b><font size='1px'>TOTALES EMPRESA</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valortot) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                    strHtmlmostrar += "<td align='center' colspan='6'><b><font size='8px'>TOTALES EMPRESA</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='8px'>" & String.Format("{0:c}", valortot) & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtmlmostrar += "</table>"

                    divmostrar.InnerHtml = strHtml
                    divinforme.InnerHtml = strHtmlmostrar
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
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
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Cambiadero"

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
