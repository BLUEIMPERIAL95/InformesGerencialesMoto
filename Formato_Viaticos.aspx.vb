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
Partial Class Formato_Viaticos
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csviat As New cviaticos

    Private Sub FormatoOrdenCompra_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(4074, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            cargar_formato(Request.QueryString("id"))
        End If
    End Sub

    Private Sub cargar_formato(ByVal orc As Integer)
        Try
            Dim dtOrden As New DataTable

            dtOrden = csviat.seleccionar_viaticos("3", Request.QueryString("id"))

            If dtOrden.Rows.Count > 0 Then
                Dim dtdet As New DataTable

                dtdet = csviat.seleccionar_viaticos_detalle(Request.QueryString("id"))

                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                If Session("codEmpr") = "1" Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo.jpg")
                Else
                    If Session("codEmpr") = "2" Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                    Else
                        If Session("codEmpr") = "3" Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logorefri.jpg")
                        Else
                            If Session("codEmpr") = "4" Then
                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotra.jpeg")
                            Else
                                If Session("codEmpr") = "5" Then
                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logocia.jpeg")
                                Else
                                    If Session("codEmpr") = "6" Then
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logomotoseguridad.jpg")
                                    Else
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                                    End If
                                End If
                            End If
                        End If
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

                strHtml = "<br /><br /><br /><br /><br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='30%'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                strHtml += "<td align='center' width='50%' colspan='2'><b><font size='6'>Viatico Número: " & dtOrden.Rows(0)("numero_viat").ToString & "</font></b></td>"
                strHtml += "<td align='left'><table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='5'>Codigo: </font></b><font size='5'>FO5C-006</font></td></tr><tr><td align='center'><b><font size='5'>Fecha: </font></b><font size='5'>2020-02-10</font></td></tr><tr><td align='center'><b><font size='5'>Version: </font></b><font size='5'>5</font></td></tr></table></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Fecha: </font></b><font size='4'>" & dtOrden.Rows(0)("fecha_viat").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Empleado: </font></b><font size='4'>" & dtOrden.Rows(0)("nombre_terc").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='20%' colspan='2'><b><font size='3'>TERCERO</font></b></td>"
                strHtml += "<td align='left' width='20%' colspan='3'><b><font size='3'>DESCRIPCION</font></b></td>"
                strHtml += "<td align='left' width='20%'><b><font size='3'>TIPO</font></b></td>"
                strHtml += "<td align='left' width='20%'><b><font size='3'>REF</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>CANTIDAD</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>VALOR</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>TOTAL</font></b></td>"
                strHtml += "</tr>"

                Dim decTotalEnt, decTotalSal As Decimal
                decTotalEnt = 0
                decTotalSal = 0
                For i As Integer = 0 To dtdet.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='left' width='20%' colspan='2'><font size='2'>" & dtdet.Rows(i)("nombre_terc").ToString & "</font></td>"
                    strHtml += "<td align='left' width='20%' colspan='3'><font size='1'>" & dtdet.Rows(i)("observacion_vide").ToString & "</font></td>"
                    strHtml += "<td align='left' width='20%'><font size='3'>" & dtdet.Rows(i)("tipo_vide").ToString & "</font></td>"
                    strHtml += "<td align='left' width='20%'><font size='3'>" & dtdet.Rows(i)("referencia_vide").ToString & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & dtdet.Rows(i)("cant_vide").ToString & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='2'>" & String.Format("{0:c}", dtdet.Rows(i)("valor_vide")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='2'>" & String.Format("{0:c}", dtdet.Rows(i)("total_vide")) & "</font></td>"
                    strHtml += "</tr>"

                    If dtdet.Rows(i)("tipo_vide").ToString = "ENTRADA" Then
                        decTotalEnt = decTotalEnt + dtdet.Rows(i)("total_vide")
                    Else
                        decTotalSal = decTotalSal + dtdet.Rows(i)("total_vide")
                    End If
                Next

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='1'><b><font size='2'>ENTRADAS</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='2'>" & String.Format("{0:c}", decTotalEnt) & "</font></b></td>"
                strHtml += "<td align='center' width='20%' colspan='1'><b><font size='2'>SALIDAS</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='2'>" & String.Format("{0:c}", decTotalSal) & "</font></b></td>"
                strHtml += "<td align='center' width='20%' colspan='2'><b><font size='3'>SALDO</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='3'>" & String.Format("{0:c}", decTotalEnt - decTotalSal) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><b><font size='4'>OBSERVACIONES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='20%'><font size='3'>" & dtOrden.Rows(0)("observacion_viat").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><b><font size='4'>_____________________________________________</font></b></td>&nbsp;&nbsp;&nbsp;&nbsp;"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><font size='3'>Solicita: " & dtOrden.Rows(0)("nombre_terc").ToString & "</font></td>&nbsp;&nbsp;&nbsp;&nbsp;"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><font size='3'>CC " & dtOrden.Rows(0)("documento_TERC").ToString & "</font></td>&nbsp;&nbsp;&nbsp;&nbsp;"
                strHtml += "</tr>"
                strHtml += "</table>"

                divinforme.InnerHtml = strHtml

                export_pdf()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Orden Inválida.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub export_pdf()
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Viatico " & Request.QueryString("id")

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
