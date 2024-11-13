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
Partial Class Formato_Certificado_Ica
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csterc As New equipos
    Dim csoper As New NumLetra
    Dim csoped As New Operaciones

    Private Sub Formato_Certificado_Ica_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        cargar_formato(Request.QueryString("tip"), Request.QueryString("per"), Request.QueryString("emp"), Request.QueryString("doc"), Request.QueryString("bim"))
    End Sub

    Private Sub cargar_formato(ByVal tip As Integer, ByVal per As Integer, ByVal emp As Integer, ByVal doc As Integer, ByVal bim As Integer)
        Try
            Dim dtCertificado As New DataTable

            dtCertificado = csterc.seleccionar_datos_certificados_por_documento_tipo_periodo_empresa(tip, per, emp, doc, bim)

            If dtCertificado.Rows.Count > 0 Then
                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                If dtCertificado.Rows(0)("id_emor").ToString = "1" Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                Else
                    If dtCertificado.Rows(0)("id_emor").ToString = "2" Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo2.jpg")
                    Else
                        If dtCertificado.Rows(0)("id_emor").ToString = "3" Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo3.jpg")
                        Else
                            If dtCertificado.Rows(0)("id_emor").ToString = "4" Then
                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo4.jpg")
                            Else
                                If dtCertificado.Rows(0)("id_emor").ToString = "5" Then
                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo5.jpg")
                                Else
                                    If dtCertificado.Rows(0)("id_emor").ToString = "7" Then
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logocia.jpeg")
                                    Else
                                        If dtCertificado.Rows(0)("id_emor").ToString = "8" Then
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "LogoTando.jpg")
                                        Else
                                            If dtCertificado.Rows(0)("id_emor").ToString = "9" Then
                                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "LogoTramSeg.jpg")
                                            Else
                                                If dtCertificado.Rows(0)("id_emor").ToString = "10" Then
                                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "LogoRefriCont.jpg")
                                                End If
                                            End If
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
                Dim intTotalBase, intTotalValor As Decimal
                strHtml = ""
                strHtmlmostrar = ""

                strHtml = "<br /><br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='30%'><img src='" & urlFotoCabeza1 & "' height='100' width='300'></td>"
                strHtml += "<td align='center' width='50%' colspan='2'><table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='5'>" & dtCertificado.Rows(0)("nombre_emor").ToString & "</font></b></td></tr><tr><td align='center' width='50%'><b><font size='4'>Nit: </font></b><font size='4'>" & dtCertificado.Rows(0)("nit_emor").ToString & "</font></td></tr><tr><td align='center' width='50%' colspan='2'><b><font size='3'>Telefono: </font></b><font size='3'>" & dtCertificado.Rows(0)("telefono_emor").ToString & " </font><b><font size='3'>Direccion: </font></b><font size='3'>" & dtCertificado.Rows(0)("direccion_emor").ToString & " </font><b><font size='3'>Ciudad: </font></b><font size='3'>" & dtCertificado.Rows(0)("ciudad_emor").ToString & " </font></td></tr><tr><td align='center' width='50%'><b><font size='4'>Certificado </font></b><font size='4'>" & dtCertificado.Rows(0)("nombre_tice").ToString & "</font></td></tr><tr><td align='center' width='50%'><b><font size='4'>Año Gravable </font></b><font size='4'>" & dtCertificado.Rows(0)("periodo_dace").ToString & "</font></td></tr>"
                If dtCertificado.Rows(0)("bimestre_dace") > 0 Then
                    strHtml += "<tr><td align='center' width='50%'><b><font size='4'>Bimestre </font></b><font size='4'>" & dtCertificado.Rows(0)("bimestre_dace").ToString & "</font></td></tr>"
                End If
                strHtml += "</table></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='50%' colspan='2'><b><font size='4'>Razon Social: </font></b></td>"
                strHtml += "<td align='center' width='50%' colspan='3'><font size='4'>" & dtCertificado.Rows(0)("nombre_dace").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='50%' colspan='2'><b><font size='4'>Nit: </font></b></td>"
                strHtml += "<td align='center' width='50%' colspan='3'><font size='4'>" & dtCertificado.Rows(0)("documento_dace").ToString & " - " & csoped.calcular_digito(dtCertificado.Rows(0)("documento_dace").ToString) & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='50%' colspan='2'><b><font size='4'>Concepto</font></b></td>"
                strHtml += "<td align='center' width='50%'><b><font size='4'>%</font></b></td>"
                strHtml += "<td align='center' width='50%'><b><font size='4'>Base</font></b></td>"
                strHtml += "<td align='center' width='50%'><b><font size='4'>Retencion</font></b></td>"
                strHtml += "</tr>"

                intTotalBase = 0
                intTotalValor = 0
                For i As Integer = 0 To dtCertificado.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='center' width='50%' colspan='2'><font size='4'>Retencion " & dtCertificado.Rows(i)("descripcion_ince").ToString & "</font></td>"
                    strHtml += "<td align='right' width='50%'><font size='4'>" & dtCertificado.Rows(i)("porcentaje_ince").ToString & " %</font></td>"
                    strHtml += "<td align='right' width='50%'><font size='4'>" & String.Format("{0:c}", dtCertificado.Rows(i)("base_dace")) & "</font></td>"
                    strHtml += "<td align='right' width='50%'><font size='4'>" & String.Format("{0:c}", dtCertificado.Rows(i)("valor_dace")) & "</font></td>"
                    strHtml += "</tr>"

                    intTotalBase = intTotalBase + dtCertificado.Rows(i)("base_dace")
                    intTotalValor = intTotalValor + dtCertificado.Rows(i)("valor_dace")
                Next

                strHtml += "<tr>"
                strHtml += "<td align='right' width='50%' colspan='3'><b><font size='4'>Totales: </font></b></td>"
                strHtml += "<td align='right' width='50%'><b><font size='4'>" & String.Format("{0:c}", intTotalBase) & "</font></b></td>"
                strHtml += "<td align='right' width='50%'><b><font size='4'>" & String.Format("{0:c}", intTotalValor) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='right' width='50%' colspan='2'><b><font size='4'>Valor en letras: </font></b></td>"
                strHtml += "<td align='right' width='50%' colspan='3'><font size='2'>" & csoper.Num2Text(Math.Round(intTotalValor, 0)) & " PESOS M/L</font></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"
                If dtCertificado.Rows(0)("id_tice").ToString = "1" Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' width='50%' colspan='5'><font size='4'>Los valores retenidos fueron declarados y consignados oportunamente en la Administración Municipal de cada Municipio. </font></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"
                Else
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' width='50%' colspan='5'><font size='4'>Con el fin de dar cumplimiento a la disposicion vigente sobre retencion en la fuente, expedimos certificado segun lo establecido en el articulo 381 E.T. </font></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"
                End If

                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='50%' colspan='5'><font size='4'>Certificado expedido el " & Day(Now) & " - " & Month(Now) & " - " & Year(Now) & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='50%' colspan='5'><font size='4'>DEPARTAMENTO DE CONTABILIDAD </font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divinforme.InnerHtml = strHtml

                export_pdf()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información para los datos ingresados.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub export_pdf()
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Certificado"

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
