Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Partial Class Formato_Cad_Recibido
    Inherits System.Web.UI.Page
    Dim cscade As New cad

    Private Sub Formato_Cad_Recibido_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Me.IsPostBack = False Then
                Dim dtenc As New DataTable

                dtenc = cscade.seleccionar_cad_recibido_listado(2, Request.QueryString("id"))

                If dtenc.Rows.Count > 0 Then
                    Dim pathimgCabeza1 As String
                    Dim urlFotoCabeza1 As String = ""
                    If dtenc.Rows(0)("id_emor").ToString = "1" Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                    Else
                        If dtenc.Rows(0)("id_emor").ToString = "2" Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo2.jpg")
                        Else
                            If dtenc.Rows(0)("id_emor").ToString = "3" Then
                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo3.jpg")
                            Else
                                If dtenc.Rows(0)("id_emor").ToString = "4" Then
                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo4.jpg")
                                Else
                                    If dtenc.Rows(0)("id_emor").ToString = "5" Then
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo5.jpg")
                                    Else
                                        If dtenc.Rows(0)("id_emor").ToString = "7" Then
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logocia.jpeg")
                                        Else
                                            If dtenc.Rows(0)("id_emor").ToString = "8" Then
                                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                                            Else
                                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
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
                    strHtml += "<td align='center' width='30%'><img src='" & urlFotoCabeza1 & "' height='100' width='300'></td>"
                    strHtml += "<td align='center' width='40%' colspan='2'><b><font size='5'>Formato Cad Recibido # " & dtenc.Rows(0)("numero_care").ToString & "</font></b></td>"
                    strHtml += "<td align='left' width='30%' colspan='2'><table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='3'>Fecha: </font></b><font size='3'>" & dtenc.Rows(0)("fecha_care").ToString & "</font></td></tr><tr><td align='center'><b><font size='3'>Empresa: </font></b><font size='3'>" & dtenc.Rows(0)("nombre_emor").ToString & "</font></td></tr><tr><td align='center'><b><font size='3'>Agencia: </font></b><font size='3'>" & dtenc.Rows(0)("nombre_agcc").ToString & "</font></td></tr><tr><td align='center'><b><font size='3'>T.Comprobante: </font></b><font size='3'>" & dtenc.Rows(0)("tipodoc_care").ToString & "</font></td></tr><tr><td align='center'><b><font size='3'>Usuario: </font></b><font size='3'>" & dtenc.Rows(0)("strnom1_usua").ToString & "</font></td></tr></table></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<br />"
                    strHtml += "<br />"

                    Dim dtDet As New DataTable

                    dtDet = cscade.seleccionar_cad_recibido_detalle(Request.QueryString("id"))

                    If dtDet.Rows.Count > 0 Then
                        strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                        strHtml += "<tr>"
                        strHtml += "<td align='left' width='20%'><b><font size='3'># COMPROBANTE</font></b></td>"
                        strHtml += "<td align='left' width='20%' colspan='3'><b><font size='3'>TERCERO</font></b></td>"
                        strHtml += "<td align='center' width='20%'><b><font size='3'># FOLIOS</font></b></td>"
                        strHtml += "</tr>"

                        Dim cont = 0
                        For i As Integer = 0 To dtDet.Rows.Count - 1
                            strHtml += "<tr>"
                            strHtml += "<td align='left' width='20%'><font size='3'>" & dtDet.Rows(i)("numcom_card").ToString & "</font></td>"
                            strHtml += "<td align='left' width='20%' colspan='3'><font size='3'>" & dtDet.Rows(i)("tercero_card").ToString & "</font></td>"
                            strHtml += "<td align='center' width='20%'><font size='3'>" & dtDet.Rows(i)("folios_card").ToString & "</font></td>"
                            strHtml += "</tr>"

                            cont = cont + 1
                        Next

                        strHtml += "<tr>"
                        strHtml += "<td align='right' width='20%' colspan='4'><font size='3'>TOTAL DETALLES: </font></td>"
                        strHtml += "<td align='center' width='20%'><font size='3'>" & cont & "</font></td>"
                        strHtml += "</tr>"
                        strHtml += "</table>"
                    End If

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' width='20%'><b><font size='4'>OBSERVACIONES</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='justify' width='20%'><font size='3'>" & dtenc.Rows(0)("observacion_care").ToString & "</font></td>"
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
                    strHtml += "<td align='center' width='20%'><b><font size='4'>______________________________</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' width='20%'><font size='2'>Envía: " & dtenc.Rows(0)("strnom1_usua").ToString & "</font></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' width='20%'><font size='2'>CC " & dtenc.Rows(0)("strdoc_usua").ToString & "</font></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    principal.InnerHtml = strHtml

                    export_pdf()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub export_pdf()
        Try
            Dim imagenc1 As Image

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=FormatoRecibido.pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            Me.Page.RenderControl(hw)
            Dim sr As New StringReader(sw.ToString())
            Dim pdfDoc As New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 3.0F)
            Dim htmlparser As New HTMLWorker(pdfDoc)
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream)
            pdfDoc.Open()

            'imagenc1 = iTextSharp.text.Image.GetInstance(urlFotoCabeza5)
            'imagenc1.BorderWidth = 1
            'imagenc1.Alignment = Element.ALIGN_LEFT

            'imagenc1.ScaleAbsoluteWidth(150)
            'imagenc1.ScaleAbsoluteHeight(30)

            'imagenc1.SetAbsolutePosition(30, 1070)
            'pdfDoc.Add(imagenc1)

            htmlparser.Parse(sr)
            pdfDoc.Close()
            Response.Write(pdfDoc)
            Response.[End]()

        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
