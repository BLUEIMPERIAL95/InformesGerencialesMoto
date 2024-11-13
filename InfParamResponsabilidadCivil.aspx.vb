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

Partial Class InfParamResponsabilidadCivil
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csfact As New facturas

    Private Sub InfParamResponsabilidadCivil_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(32, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtprov As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_provedores_tramitar_combo", dtprov, cboProvedores)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            Dim dtfac As New DataTable
            Dim pathimgCabeza1 As String
            Dim urlFotoCabeza1 As String = ""
            Dim strHtml, strHtmlmostrar As String
            strHtml = ""
            strHtmlmostrar = ""
            Dim decTotal1, decTotal2, decTotal3, decTotal4 As Decimal

            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Or cboProvedores.SelectedValue = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                dtfac = csfact.seleccionar_datos_facturas_informe(txtFechaInicio.Value, txtFechaFin.Value, cboProvedores.SelectedValue)

                If dtfac.Rows.Count > 0 Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logorefri.jpg")

                    If File.Exists(pathimgCabeza1) Then
                        urlFotoCabeza1 = pathimgCabeza1
                    Else
                        urlFotoCabeza1 = Path.Combine(Server.MapPath("images"), "nophoto.jpg")
                    End If

                    strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    'strHtml += "<td align='left' colspan='5'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtml += "<td align='center' colspan='21'><b><font size='4'>SEGUROS RESPONSABILIDAD CIVIL EXTRACONTRACTUAL PARA VEHICULOS(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                    strHtml += "</tr>"
                    'strHtml += "</table>"

                    'strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>POL</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>CERTIFICADO</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>A NOMBRE DE</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>DOCUMENTO</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>COBERTURA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>MODELO</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>EDAD</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>TONS</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>PLACA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>VIGENCIA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>VENCIMIENTO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>DIAS</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>VALOR COMPLETO</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>COMPAÑIA</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>PRECIO VENTA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>COTIZACION</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>PROFORMA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>FACTURA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>RCAJA</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>GANANCIA TRAMITAR</font></b></td>"
                    strHtml += "</tr>"

                    decTotal1 = 0
                    decTotal2 = 0
                    decTotal3 = 0
                    decTotal4 = 0
                    For i As Integer = 0 To dtfac.Rows.Count - 1
                        strHtml += "<tr>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtfac.Rows(i)("id_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtfac.Rows(i)("certificado_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='left' colspan='2'><b><font size='1px'>" & dtfac.Rows(i)("nombre_TERC").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtfac.Rows(i)("documento_TERC").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtfac.Rows(i)("nombre_cotr").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("nombre_tvtr").ToString & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & dtfac.Rows(i)("modelo_vetr").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtfac.Rows(i)("edad_vetr").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("placa_vetr").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("fecha_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("fechaven_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("dias_venc").ToString & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtfac.Rows(i)("vlremp_fact")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtfac.Rows(i)("vlremp_fact")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtfac.Rows(i)("valor_fact")) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("cotizacion_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("proforma_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("numero_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & dtfac.Rows(i)("recibo_fact").ToString & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtfac.Rows(i)("vlr_gan")) & "</font></b></td>"
                        strHtml += "</tr>"

                        decTotal1 = decTotal1 + dtfac.Rows(i)("vlremp_fact")
                        decTotal2 = decTotal2 + dtfac.Rows(i)("vlremp_fact")
                        decTotal3 = decTotal3 + dtfac.Rows(i)("valor_fact")
                        decTotal4 = decTotal4 + dtfac.Rows(i)("vlr_gan")
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='center' colspan='13'><b><font size='1px'>TOTALES: </font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtml += "<td align='center' colspan='4'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=RC " & txtFechaInicio.Value & "  " & txtFechaFin.Value & ".xls")
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

    'Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click

    'End Sub
End Class
