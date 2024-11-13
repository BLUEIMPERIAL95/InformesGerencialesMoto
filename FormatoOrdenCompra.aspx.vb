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
Partial Class FormatoOrdenCompra
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csorco As New OrdenesCompra

    Private Sub FormatoOrdenCompra_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2041, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            hidOrden.Value = Request.QueryString("idoc")
            cargar_formato(hidOrden.Value)
        End If
    End Sub

    Private Sub cargar_formato(ByVal orc As Integer)
        Try
            Dim dtOrden As New DataTable

            dtOrden = csorco.seleccionar_orden_compra_por_id(hidOrden.Value)

            If dtOrden.Rows.Count > 0 Then
                Dim dtdet As New DataTable

                dtdet = csorco.seleccionar_detalle_ordenes_compra_por_id_orden(hidOrden.Value)

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
                                        If Session("codEmpr") = "7" Then
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                                        Else
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "LOGOCEFATRANS.png")
                                        End If
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
                strHtml += "<td align='center' width='50%' colspan='2'><b><font size='6'>Orden de Compra Número: " & dtOrden.Rows(0)("numero_orco").ToString & "</font></b></td>"
                strHtml += "<td align='left'><table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='5'>Codigo: </font></b><font size='5'>FO5C-006</font></td></tr><tr><td align='center'><b><font size='5'>Fecha: </font></b><font size='5'>2020-02-10</font></td></tr><tr><td align='left'><b><font size='5'>Version: </font></b><font size='5'>5</font></td></tr></table></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Fecha: </font></b><font size='4'>" & dtOrden.Rows(0)("fecha_orco").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Empresa: </font></b><font size='4'>" & Session("empresa") & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Solicitante: </font></b><font size='4'>" & dtOrden.Rows(0)("nom_sol").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Proveedor: </font></b><font size='4'>" & dtOrden.Rows(0)("nom_pro").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Documento: </font></b><font size='4'>" & dtOrden.Rows(0)("documento_TERC").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Teléfono: </font></b><font size='4'>" & dtOrden.Rows(0)("telefono_TERC").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Dirección: </font></b><font size='4'>" & dtOrden.Rows(0)("direccion_TERC").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Correo: </font></b><font size='4'>" & dtOrden.Rows(0)("correo_TERC").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='3'><b><font size='3'>DESCRIPCION</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>CANTIDAD</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>PRECIO</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>RETENCION</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>IVA</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>TOTAL</font></b></td>"
                strHtml += "</tr>"

                Dim decTotal As Decimal
                decTotal = 0
                For i As Integer = 0 To dtdet.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='left' width='20%' colspan='3'><font size='3'>" & dtdet.Rows(i)("nombre_EQUI").ToString & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & dtdet.Rows(i)("cantidad_deoc").ToString & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("costounitario_deoc")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("ret_deoc")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("iva_deoc")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("valor_deoc")) & "</font></td>"
                    strHtml += "</tr>"

                    decTotal = decTotal + dtdet.Rows(i)("valor_deoc")
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='6'><b><font size='3'>TOTAL</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='3'>" & String.Format("{0:c}", decTotal) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><b><font size='4'>OBSERVACIONES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='20%'><font size='3'>" & dtOrden.Rows(0)("observacion_orco").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><b><font size='4'>_____________________________________________</font></b></td>&nbsp;&nbsp;&nbsp;&nbsp;"
                strHtml += "<td align='center' width='20%'><b><font size='4'>_____________________________________________</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><font size='3'>Solicita: " & dtOrden.Rows(0)("nom_sol").ToString & "</font></td>&nbsp;&nbsp;&nbsp;&nbsp;"
                strHtml += "<td align='center' width='20%'><font size='3'>Autoriza: " & dtOrden.Rows(0)("nom_aut").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><font size='3'>CC " & dtOrden.Rows(0)("documento_soli").ToString & "</font></td>&nbsp;&nbsp;&nbsp;&nbsp;"
                strHtml += "<td align='center' width='20%'><font size='3'>CC " & dtOrden.Rows(0)("documento_auto").ToString & "</font></td>"
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

            strNombreInforme = "Orden Compra " & hidOrden.Value

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
