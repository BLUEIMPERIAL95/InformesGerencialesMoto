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
Partial Class FormatoCuentaCobro
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim cscuen As New Cuentacob

    Private Sub FormatoCuentaCobro_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2044, Session("id_usua"))

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

            dtOrden = cscuen.seleccionar_proximo_numero_cuenta_cobro_por_id(hidOrden.Value)

            If dtOrden.Rows.Count > 0 Then
                Dim dtdet As New DataTable
                Dim strTipo As String

                strTipo = dtOrden.Rows(0)("tipo_cuco").ToString

                dtdet = cscuen.seleccionar_cuenta_cobro_detalle_por_id_cuenta(hidOrden.Value)

                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                If dtOrden.Rows(0)("id_emor").ToString = "1" Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                Else
                    If dtOrden.Rows(0)("id_emor").ToString = "2" Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo2.jpg")
                    Else
                        If dtOrden.Rows(0)("id_emor").ToString = "3" Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo3.jpg")
                        Else
                            If dtOrden.Rows(0)("id_emor").ToString = "4" Then
                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo4.jpg")
                            Else
                                If dtOrden.Rows(0)("id_emor").ToString = "5" Then
                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo5.jpg")
                                Else
                                    If dtOrden.Rows(0)("id_emor").ToString = "7" Then
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logocia.jpeg")
                                    Else
                                        If dtOrden.Rows(0)("id_emor").ToString = "8" Then
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
                strHtml += "<td align='center' width='50%' colspan='2'><b><font size='5'>Documento de Cobro Número: " & dtOrden.Rows(0)("numero_cuco").ToString & "</font></b></td>"
                'strHtml += "<td align='left'><table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='3'>Prefijo: </font></b><font size='3'>" & dtOrden.Rows(0)("prefijo_agcc").ToString & "</font></td></tr></table></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Fecha: </font></b><font size='4'>" & dtOrden.Rows(0)("fecha_cuco").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Empresa Organización: </font></b><font size='4'>" & dtOrden.Rows(0)("nombre_emor").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Solicita: </font></b><font size='4'>" & dtOrden.Rows(0)("nom_sol").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Proveedor: </font></b><font size='4'>" & dtOrden.Rows(0)("nom_ter").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Documento: </font></b><font size='4'>" & dtOrden.Rows(0)("documento_TERC").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Teléfono: </font></b><font size='4'>" & dtOrden.Rows(0)("telefono_TERC").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Celular: </font></b><font size='4'>" & dtOrden.Rows(0)("celular_TERC").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Correo: </font></b><font size='4'>" & dtOrden.Rows(0)("correo_TERC").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Dirección: </font></b><font size='4'>" & dtOrden.Rows(0)("direccion_TERC").ToString & "</font></td>"
                strHtml += "<td align='left' width='50%'><b><font size='4'>Ciudad: </font></b><font size='4'>" & dtOrden.Rows(0)("nombre_zona").ToString & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='3'><b><font size='3'>CONCEPTO</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>CANTIDAD</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>PRECIO</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>SUBTOTAL</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>RETENCION</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='3'>TOTAL</font></b></td>"
                strHtml += "</tr>"

                Dim decTotal, decRete, decNeto As Decimal
                decTotal = 0
                decRete = 0
                decNeto = 0
                For i As Integer = 0 To dtdet.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='left' width='20%' colspan='3'><font size='3'>" & dtdet.Rows(i)("concepto_ccde").ToString & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & dtdet.Rows(i)("cantidad_ccde").ToString & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("valor_ccde")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("total_ccde")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("valret_ccde")) & "</font></td>"
                    strHtml += "<td align='right' width='20%'><font size='3'>" & String.Format("{0:c}", dtdet.Rows(i)("totaltot_ccde")) & "</font></td>"
                    strHtml += "</tr>"

                    decTotal = decTotal + dtdet.Rows(i)("total_ccde")
                    decRete = decRete + dtdet.Rows(i)("valret_ccde")
                    decNeto = decNeto + dtdet.Rows(i)("totaltot_ccde")
                Next

                'If strTipo = "COMPRA" Then
                '    If decTotal >= 961000 Then
                '        decRete = decTotal * (3.5 / 100)
                '    End If
                'Else
                '    If strTipo = "SERVICIO" Then
                '        If decTotal >= 142000 Then
                '            decRete = decTotal * (6 / 100)
                '        End If
                '    Else
                '        decRete = decTotal * (10 / 100)
                '    End If
                'End If

                'decNeto = decTotal - decRete

                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='6'><b><font size='3'>SUBTOTAL</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='3'>" & String.Format("{0:c}", decTotal) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='6'><b><font size='3'>RETENCIÓN</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='3'>" & String.Format("{0:c}", decRete) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%' colspan='6'><b><font size='3'>TOTAL</font></b></td>"
                strHtml += "<td align='right' width='20%' colspan='2'><b><font size='3'>" & String.Format("{0:c}", decNeto) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                'strHtml += "<br />"

                'strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                'strHtml += "<tr>"
                'strHtml += "<td align='center' colspan='4' width='20%'><b><font size='3'>TARIFAS Y BASES RETENCIÓN AÑO 2020</font></b></td>"
                'strHtml += "</tr>"
                'strHtml += "<tr>"
                'strHtml += "<td align='center' width='20%'><b><font size='3'>CONCEPTO</font></b></td>"
                'strHtml += "<td align='center' width='20%'><b><font size='3'>BASE</font></b></td>"
                'strHtml += "<td align='center' width='20%'><b><font size='3'>RTE FUENTE</font></b></td>"
                'strHtml += "<td align='center' width='20%'><b><font size='3'>RTE ICA</font></b></td>"
                'strHtml += "</tr>"
                'strHtml += "<tr>"
                'strHtml += "<td align='center' width='20%'><font size='3'>COMPRA</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'>$961.000</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'>3.5%</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'></font></td>"
                'strHtml += "</tr>"
                'strHtml += "<tr>"
                'strHtml += "<td align='center' width='20%'><font size='3'>SERVICIO</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'>$142.000/font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'>6.0%</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'></font></td>"
                'strHtml += "</tr>"
                'strHtml += "<tr>"
                'strHtml += "<td align='center' width='20%'><font size='3'>HONORARIO</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'>$1</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'>10.0%</font></td>"
                'strHtml += "<td align='center' width='20%'><font size='3'></font></td>"
                'strHtml += "</tr>"
                'strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='90%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><b><font size='4'>OBSERVACIONES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='20%'><font size='3'>" & dtOrden.Rows(0)("observacion_cuo").ToString & "</font></td>"
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
                strHtml += "<td align='center' width='20%'><b><font size='4'>______________________________</font></b></td>"
                strHtml += "<td align='center' width='20%'><b><font size='4'>______________________________</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><font size='2'>SOLICITA: " & dtOrden.Rows(0)("nom_sol").ToString & "</font></td>"
                strHtml += "<td align='center' width='20%'><font size='3'>AUTORIZADO</font></td>"
                strHtml += "<td align='center' width='20%'><font size='3'>VENDEDOR</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='20%'><font size='2'>CC " & dtOrden.Rows(0)("doc_aut").ToString & "</font></td>"
                strHtml += "<td align='center' width='20%'><font size='3'></font></td>"
                strHtml += "<td align='center' width='20%'><font size='3'></font></td>"
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

            strNombreInforme = "Cuenta Cobro Nro: " & hidOrden.Value

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
