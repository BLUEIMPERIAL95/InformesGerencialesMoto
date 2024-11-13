Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports Amazon
Imports Amazon.SimpleEmail
Imports Amazon.SimpleEmail.Model
Partial Class FormatoCarteraGenerador
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Private Sub FormatoCarteraGenerador_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        If Me.IsPostBack = False Then
            Dim strSQL As String
            Dim dtTer As New DataTable
            Dim decTotalFac, decTotalAbo, decTotalSal As Decimal
            decTotalFac = 0
            decTotalAbo = 0
            decTotalSal = 0

            lbl_correo.InnerText = "Empresa: " & Session("empresa")

            strSQL = "Select vc.generador_id, vc.venta_id, vc.generador_nombre As Generador, "
            strSQL += "vc.generador_documento as Documento, vc.venta_plazo As Plazo, vc.generador_telefonos As Telefonos, "
            strSQL += "vc.generador_direccion As Direccion, zo.zona As zona, COALESCE(vc.asesor_id, 0) As idas, "
            strSQL += "COALESCE(vc.asesor_nombre, COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
            strSQL += "From generadores_asesores "
            strSQL += "Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
            strSQL += "Left Join terceros ON usuarios.idterceros = terceros.idterceros "
            strSQL += "WHERE terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 "
            strSQL += "And usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')) as Asesor, "
            strSQL += "vc.sucursal_origina_descripcion As Sucursal, vc.venta_numero As NroFac, "
            strSQL += "DATE_FORMAT(vc.venta_fecha,'%Y-%m-%d') as Fecha, DATE_FORMAT(vc.venta_vence,'%Y-%m-%d') as Vence, "
            strSQL += "vc.venta_total As Total, vc.venta_abonos As Abono, (vc.venta_total - vc.venta_abonos) As Saldo, "
            strSQL += "(SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) as DiasVence, "
            strSQL += "(SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo as DiasFac, "
            strSQL += "COALESCE(gena.usuarios_idusuarios, 1) As us_asesor, "
            strSQL += "COALESCE((SELECT CONCAT(te.nombre1, ' ', te.nombre2, ' ', te.apellido1, ' ', te.apellido2) "
            strSQL += "From generadores_asesores "
            strSQL += "Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
            strSQL += "Left Join terceros te ON usuarios.idterceros = te.idterceros "
            strSQL += "WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
            strSQL += "ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 'TERCERO ESPECIAL') AS ase_ofi "
            strSQL += "From ventas_consolidado vc "
            strSQL += "INNER Join terceros ter ON (vc.generador_terceroid=ter.idterceros) "
            strSQL += "INNER Join zonas zo on(ter.zonas_idzonas=zo.idzonas) "
            strSQL += "INNER Join generadores gen ON(vc.generador_id=gen.idgeneradores And gen.idel=0) "
            strSQL += "Left Join generadores_asesores gena ON(vc.asesor_id=gena.idgeneradores_asesores) "
            strSQL += "Left Join usuarios us ON(us.idusuarios=gena.usuarios_idusuarios) "
            strSQL += "WHERE vc.venta_id = (SELECT vd.ventas_idventas FROM ventas_detalles vd "
            strSQL += "WHERE vc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1) "
            strSQL += "And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd "
            strSQL += "WHERE vrd.ventas_idventas = vc.venta_id)) Or (SELECT SUM(vrd.valor) "
            strSQL += "From ventas_recaudos_detalle vrd Where vrd.ventas_idventas = vc.venta_id)Is NULL) "
            strSQL += "And vc.venta_abonos < vc.venta_total And (vc.venta_total - vc.venta_abonos) > 10 "
            strSQL += "And vc.venta_vence<=(SELECT( ADDDATE('" & DateTime.Now.ToString("yyyy-MM-dd") & "',INTERVAL vc.venta_plazo DAY))) "
            strSQL += "And vc.generador_documento = '" & Request.QueryString("nit") & "' "
            strSQL += "ORDER BY ase_ofi, Generador, (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo DESC "

            dtTer = csinformes.ejecutar_query_bd(strSQL)

            If dtTer.Rows.Count > 0 Then
                lbl_fecha.InnerText = DateTime.Now.ToString("yyyy-MM-dd") & " "
                lbl_asesor.InnerText = dtTer.Rows(0)("ase_ofi").ToString
                lbl_nombre_cliente.InnerText = dtTer.Rows(0)("Generador").ToString
                lbl_nit.InnerText = dtTer.Rows(0)("Documento").ToString
                lbl_telefono.InnerText = " Teléfono: " & dtTer.Rows(0)("Telefonos").ToString

                Dim strHtml As String
                strHtml = ""
                Dim tr As HtmlTableRow
                Dim td1 As HtmlTableCell
                Dim td2 As HtmlTableCell
                Dim td3 As HtmlTableCell
                Dim td4 As HtmlTableCell
                Dim td5 As HtmlTableCell
                Dim td6 As HtmlTableCell
                Dim td7 As HtmlTableCell
                Dim td8 As HtmlTableCell
                Dim td9 As HtmlTableCell
                Dim cel1 = ""
                Dim cel2 = ""
                Dim cel3 = ""
                Dim cel4 = ""
                Dim cel5 = ""
                Dim cel6 = ""
                Dim cel7 = ""
                Dim cel8 = ""
                Dim cel9 = ""

                For idet As Integer = 0 To dtTer.Rows.Count - 1
                    cel1 = "<font size='8px'>" & dtTer.Rows(idet)("Asesor").ToString.Substring(0, 7) & "</font>"
                    cel2 = "<font size='12px'>" & dtTer.Rows(idet)("NroFac").ToString & "</font>"
                    cel3 = "<font size='12px'>" & dtTer.Rows(idet)("Sucursal").ToString & "</font>"
                    cel4 = "<font size='12px'>" & dtTer.Rows(idet)("Fecha").ToString & "</font>"
                    cel5 = "<font size='12px'>" & dtTer.Rows(idet)("Vence").ToString & "</font>"
                    cel6 = "<font size='12px'>" & String.Format("{0:c}", dtTer.Rows(idet)("Total")) & "</font>"
                    cel7 = "<font size='12px'>" & String.Format("{0:c}", dtTer.Rows(idet)("Abono")) & "</font>"
                    cel8 = "<font size='12px'>" & String.Format("{0:c}", dtTer.Rows(idet)("Saldo")) & "</font>"
                    cel9 = "<font size='12px'>" & dtTer.Rows(idet)("DiasFac").ToString & "</font>"

                    tr = New HtmlTableRow
                    td1 = New HtmlTableCell
                    td2 = New HtmlTableCell
                    td3 = New HtmlTableCell
                    td4 = New HtmlTableCell
                    td5 = New HtmlTableCell
                    td6 = New HtmlTableCell
                    td7 = New HtmlTableCell
                    td8 = New HtmlTableCell
                    td9 = New HtmlTableCell

                    td1.Attributes("size") = "1"
                    td2.Attributes("size") = "1"
                    td3.Attributes("size") = "1"
                    td4.Attributes("size") = "1"
                    td5.Attributes("size") = "1"
                    td6.Attributes("size") = "1"
                    td7.Attributes("size") = "1"
                    td8.Attributes("size") = "1"
                    td9.Attributes("size") = "1"

                    td1.Attributes("align") = "left"
                    td2.Attributes("align") = "center"
                    td3.Attributes("align") = "center"
                    td4.Attributes("align") = "center"
                    td5.Attributes("align") = "center"
                    td6.Attributes("align") = "right"
                    td7.Attributes("align") = "right"
                    td8.Attributes("align") = "right"
                    td9.Attributes("align") = "right"

                    td1.InnerHtml = cel1
                    td2.InnerHtml = cel2
                    td3.InnerHtml = cel3
                    td4.InnerHtml = cel4
                    td5.InnerHtml = cel5
                    td6.InnerHtml = cel6
                    td7.InnerHtml = cel7
                    td8.InnerHtml = cel8
                    td9.InnerHtml = cel9

                    tr.Cells.Add(td1)
                    tr.Cells.Add(td2)
                    tr.Cells.Add(td3)
                    tr.Cells.Add(td4)
                    tr.Cells.Add(td5)
                    tr.Cells.Add(td6)
                    tr.Cells.Add(td7)
                    tr.Cells.Add(td8)
                    tr.Cells.Add(td9)

                    tbl_detalle.Rows.Add(tr)

                    decTotalFac = decTotalFac + dtTer.Rows(idet)("Total")
                    decTotalAbo = decTotalAbo + dtTer.Rows(idet)("Abono")
                    decTotalSal = decTotalSal + dtTer.Rows(idet)("Saldo")
                Next

                lbl_total_factura.InnerText = String.Format("{0:c}", decTotalFac)
                lbl_total_abono.InnerText = String.Format("{0:c}", decTotalAbo)
                lbl_total_saldo.InnerText = String.Format("{0:c}", decTotalSal)

                export_pdf()
            End If
        End If
    End Sub

    Sub export_pdf()
        Try
            Dim strEmpresa As String
            strEmpresa = Session("empresa")
            'Dim urlPdf = Server.MapPath("..") & "\QRManifiesto\" & Request.QueryString("pen") & ".pdf"
            Dim imagenc1 As Image
            Dim strCorreo As String
            Dim dtter As New DataTable
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=Cartera " & strEmpresa & ".pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            Me.Page.RenderControl(hw)
            Dim sr As New StringReader(sw.ToString())
            Dim myMemoryStream As New MemoryStream()
            Dim pdfDoc As New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
            Dim htmlparser As New HTMLWorker(pdfDoc)
            PdfWriter.GetInstance(pdfDoc, myMemoryStream)

            pdfDoc.Open()

            htmlparser.Parse(sr)

            'imagenc1 = iTextSharp.text.Image.GetInstance(urlFotoCabeza1)
            'imagenc1.BorderWidth = 1
            'imagenc1.Alignment = Element.ALIGN_LEFT

            'imagenc1.ScaleAbsoluteWidth(135)
            'imagenc1.ScaleAbsoluteHeight(80)

            'imagenc1.SetAbsolutePosition(35, 670)
            'pdfDoc.Add(imagenc1)

            pdfDoc.Close()

            Dim archivoBytes As Byte() = myMemoryStream.ToArray()
            Dim archivoBase64 As String = Convert.ToBase64String(archivoBytes)

            'strCorreo = "desarrollo@mototransportar.com.co"
            'strCorreo = "dirsistemas@mototransportar.com.co"
            strCorreo = Request.QueryString("cor")
            If strCorreo <> "" Then
                Dim asunto = "Cartera " & strEmpresa
                Dim cuerpoprueba = "<html><head></head><body><p>Esto es una prueba</p></body></html>"
                Dim cuerpo = "Cordial Saludo. Esta es la cartera pendiente por cancelar luego de aplicar su ultimo pago. Agradezco la revision del mismo y en caso de presentar diferencias por favor informarlas al correo cartera@mototransportar.com.co. Igualmente a este correo enviar los soportes de pago informando los numeros de facturas canceladas o al whatsapp 3103723816. Este es un correo automatico, por favor no responder."
                Dim correo = strCorreo
                Dim correocopia = "cartera@mototransportar.com.co"
                Dim vec As String() = ID.Split("|")

                'Dim cslog As Logs = New Logs

                Dim message = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
                              vbLf & "From: no-reply-systramweb@cargacontrol.com.co" &
                              vbLf & "Subject: " & asunto &
                              vbLf & "MIME-Version: 1.0" &
                              vbLf & "Content-Type: multipart/mixed;" &
                              vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Transfer-Encoding: 7bit" &
                              vbLf & "Content-Disposition: inline" &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Transfer-Encoding: base64" &
                              vbLf & "Content-Type: application/pdf;" &
                              vbLf & "Content-Disposition: attachment; filename=CarteraMototransportar" & Request.QueryString("codpen") & ".pdf" &
                              vbLf & "" &
                              vbLf & archivoBase64 &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

                Dim message2 = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correocopia &
                              vbLf & "From: no-reply-systramweb@cargacontrol.com.co" &
                              vbLf & "Subject: " & asunto &
                              vbLf & "MIME-Version: 1.0" &
                              vbLf & "Content-Type: multipart/mixed;" &
                              vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Transfer-Encoding: 7bit" &
                              vbLf & "Content-Disposition: inline" &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Transfer-Encoding: base64" &
                              vbLf & "Content-Type: application/pdf;" &
                              vbLf & "Content-Disposition: attachment; filename=preliquidacion.pdf" &
                              vbLf & "" &
                              vbLf & archivoBase64 &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

                Dim senderAddress = "no-reply-systramweb@cargacontrol.com.co"
                Dim receiverAddress = correo
                Dim receiverAddress1 = correocopia

                Dim lista = New List(Of String) From {
                    receiverAddress
                   }

                Dim lista1 = New List(Of String) From {
                    receiverAddress1
                   }

                Dim strusu As String = ConfigurationManager.AppSettings("AWSAccessKey").ToString
                Dim strcon As String = ConfigurationManager.AppSettings("AWSSecretKey").ToString

                Dim awsCredentials = New Amazon.Runtime.BasicAWSCredentials(strusu, strcon)
                Using client = New AmazonSimpleEmailServiceClient(awsCredentials, RegionEndpoint.USEast1)
                    Dim sendRequest = New SendRawEmailRequest With {
                     .Source = senderAddress,
                     .Destinations = lista,
                    .RawMessage = New RawMessage With {
                     .Data = message
                     }
                    }

                    Try
                        Dim response = client.SendRawEmail(sendRequest)
                        Dim respuesta_api = response.MessageId
                        'csreu.guardar_log_correos(correo, "EXITOSO", "ENVIO PENDIENTE REUNION", lbl_codpendiente.InnerText, Request.QueryString("idpen"))
                    Catch ex As Exception
                        Dim strMensaje = ex.Message
                        'csreu.guardar_log_correos(correo, strMensaje, "ENVIO PENDIENTE REUNION", lbl_codpendiente.InnerText, Request.QueryString("idpen"))
                    End Try
                End Using


                Using client = New AmazonSimpleEmailServiceClient(awsCredentials, RegionEndpoint.USEast1)
                    Dim sendRequest = New SendRawEmailRequest With {
                     .Source = senderAddress,
                     .Destinations = lista1,
                    .RawMessage = New RawMessage With {
                     .Data = message2
                     }
                    }

                    Try
                        Dim response = client.SendRawEmail(sendRequest)
                        Dim respuesta_api = response.MessageId
                        'cslog.log_correo(correoRemitente, correo1, Request.QueryString("prop"), Session("userid"), respuesta_api, 1, Request.QueryString("id"), "")
                    Catch ex As Exception
                        'cslog.log_correo(correoRemitente, correo1, Request.QueryString("prop"), Session("userid"), ex.Message, 2, Request.QueryString("id"), "")
                    End Try
                End Using
            End If

            Response.Write(pdfDoc)
            Response.[End]()

        Catch ex As Exception
            Dim cabeza = "Validación Imagenes", texto = "Error en envío de correo. Consultar administrador de sistema", idmensaje = "warning"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "alert", "javascript:mensaje('" & idmensaje & "','" & cabeza & "','" & texto & "','3','','');", True)
        End Try
    End Sub
End Class
