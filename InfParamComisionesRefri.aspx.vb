Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text

Partial Class InfParamComisionesRefri
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamComisionesRefri_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(4, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtase As New DataTable

        csoper.LlenarDropDownList("Nombre", "documento", "cargarasesores", dtase, cboasesores)
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Dim strSQL As String
        Dim dtter As New DataTable

        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Or cboasesores.SelectedValue = "0" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
        Else
            Dim strHtml As String

            strHtml = "SELECT /*CASE WHEN COALESCE(equipos.placa, '') <> '' THEN equipos.placa ELSE equipos.numero_equipo END AS Equipo,*/ "
            strHtml += "ventas_consolidado.asesor_nombre AS Asesor, "
            strHtml += "ventas.numero AS Factura, "
            strHtml += "ventas.fecha As Fecha, "
            'strHtml += "movimientos_contables.numero AS Recibo, "
            'strHtml += "movimientos_contables.fecha As Fecha_Recibo, "
            'strHtml += "ventas_recaudos_detalle.valor AS Valor_Recibo, "
            strHtml += "ventas_consolidado.generador_nombre As Cliente, "
            strHtml += "ventas_consolidado.generador_documento AS Doc_Cliente, "
            strHtml += "ventas_detalles.cantidad * ventas_detalles.valor_unidad As Valor_Detalle, "
            strHtml += "ventas_detalles.valor_total as Valor_Total, "
            strHtml += "ventas_detalles.descripcion as Desc_Detalle "
            strHtml += "From ventas "
            strHtml += "Left Join ventas_consolidado ON ventas.idventas = ventas_consolidado.venta_id "
            strHtml += "Left Join ventas_detalles ON ventas.idventas = ventas_detalles.ventas_idventas "
            strHtml += "Left Join ventas_control ON ventas_detalles.idventas_detalles = ventas_control.ventas_detalles_id "
            'strHtml += "Left Join ventas_recaudos_detalle ON ventas_recaudos_detalle.ventas_idventas = ventas.idventas "
            'strHtml += "Left Join ventas_recaudos ON ventas_recaudos.idventas_recaudos = ventas_recaudos_detalle.ventas_recaudos_id "
            'strHtml += "Left Join movimientos_contables ON ventas_recaudos.movimientos_contables_id = movimientos_contables.idcontabilidad "
            'strHtml += "Left Join equipos_facturacion ON ventas.idventas = equipos_facturacion.id_venta "
            'strHtml += "Left Join equipos_contratos ON equipos_facturacion.id_contrato = equipos_contratos.id "
            'strHtml += "Left Join equipos ON equipos_contratos.id_equipo = equipos.id "
            strHtml += "WHERE ventas.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strHtml += "AND ventas_consolidado.asesor_documento = '" & cboasesores.SelectedValue & "' "
            strHtml += "And ventas_control.idel = 0 "
            strHtml += "ORDER BY ventas.numero"

            dtter = csinformes.ejecutar_query_bd(strHtml)

            gridAsesores.DataSource = dtter
            gridAsesores.DataBind()
        End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim name = "ComisionesXAsesor"

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        Dim page As New Page()
        Dim form As New HtmlForm()

        gridAsesores.EnableViewState = False

        ' Deshabilitar la validación de eventos, sólo asp.net 2 
        page.EnableEventValidation = False

        ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        page.DesignerInitialize()

        page.Controls.Add(form)
        form.Controls.Add(gridAsesores)

        page.RenderControl(htw)

        Response.Clear()
        Response.Buffer = True

        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".xls")
        Response.Charset = "UTF-8"


        Response.ContentEncoding = Encoding.[Default]
        Response.Write(sb.ToString())
        Response.[End]()
    End Sub
End Class
