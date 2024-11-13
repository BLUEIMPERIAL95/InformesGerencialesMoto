Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Imports System.Globalization
Partial Class EnvioCartasClientesIngresos
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub EnvioCartasClientesIngresos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2047, Session("id_usua"))

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

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                If ConfigurationManager.AppSettings("bdsel").ToString = 3 Then
                    strSQL = "SELECT ventas_consolidado.generador_id,"
                    strSQL += " ventas_consolidado.generador_documento,"
                    strSQL += " ventas_consolidado.generador_nombre,"
                    strSQL += " COALESCE(SUM(COALESCE(ventas_consolidado.venta_total, 0)), 0) As Facturado,"
                    strSQL += " generadores.correo,"
                    strSQL += " zonas.zona"
                    strSQL += " FROM ventas_consolidado "
                    strSQL += " LEFT JOIN generadores ON ventas_consolidado.generador_id = generadores.idgeneradores "
                    strSQL += " LEFT JOIN zonas ON generadores.zonas_idzonas = zonas.idzonas "
                    strSQL += " WHERE ventas_consolidado.venta_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strSQL += " AND ventas_consolidado.venta_estado <> 'ANULADA' "

                    If cbogeneradores.SelectedValue > 0 Then
                        strSQL += " AND ventas_consolidado.generador_documento = " & cbogeneradores.SelectedValue & ""
                    End If

                    strSQL += " GROUP BY ventas_consolidado.generador_documento, ventas_consolidado.generador_nombre"
                    strSQL += " ORDER BY Facturado DESC LIMIT 100"

                    dtter = csinformes.ejecutar_query_bd(strSQL)
                Else
                    'strSQL = "SELECT movimientos_transportes_consolidado.generador_id,"
                    'strSQL += " movimientos_transportes_consolidado.generador_documento,"
                    'strSQL += " movimientos_transportes_consolidado.generador_nombre,"
                    'strSQL += " COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) + "
                    'strSQL += " COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)), 0) As Facturado,"
                    'strSQL += " generadores.correo,"
                    'strSQL += " zonas.zona"
                    'strSQL += " FROM movimientos_transportes_consolidado "
                    'strSQL += " LEFT JOIN generadores ON movimientos_transportes_consolidado.generador_id = generadores.idgeneradores "
                    'strSQL += " LEFT JOIN zonas ON generadores.zonas_idzonas = zonas.idzonas "
                    'strSQL += " WHERE movimientos_transportes_consolidado.factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    'strSQL += " AND movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                    'strSQL += " AND COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0  "

                    'If cbogeneradores.SelectedValue > 0 Then
                    '    strSQL += " AND movimientos_transportes_consolidado.generador_documento = " & cbogeneradores.SelectedValue & ""
                    'End If

                    'strSQL += " GROUP BY movimientos_transportes_consolidado.generador_documento, movimientos_transportes_consolidado.generador_nombre"
                    'strSQL += " ORDER BY Facturado DESC LIMIT 100"

                    'dtter = csinformes.ejecutar_query_bd(strSQL)

                    strSQL = "SELECT ventas_consolidado.generador_id,"
                    strSQL += " ventas_consolidado.generador_documento,"
                    strSQL += " ventas_consolidado.generador_nombre,"
                    strSQL += " COALESCE(SUM(COALESCE(ventas_consolidado.venta_total, 0)), 0) As Facturado,"
                    strSQL += " generadores.correo,"
                    strSQL += " zonas.zona"
                    strSQL += " FROM ventas_consolidado "
                    strSQL += " LEFT JOIN generadores ON ventas_consolidado.generador_id = generadores.idgeneradores "
                    strSQL += " LEFT JOIN zonas ON generadores.zonas_idzonas = zonas.idzonas "
                    strSQL += " WHERE ventas_consolidado.venta_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strSQL += " AND ventas_consolidado.venta_estado <> 'ANULADA' "

                    If cbogeneradores.SelectedValue > 0 Then
                        strSQL += " AND ventas_consolidado.generador_documento = " & cbogeneradores.SelectedValue & ""
                    End If

                    strSQL += " GROUP BY ventas_consolidado.generador_documento, ventas_consolidado.generador_nombre"
                    strSQL += " ORDER BY Facturado DESC LIMIT 100"

                    dtter = csinformes.ejecutar_query_bd(strSQL)
                End If

                If dtter.Rows.Count > 0 Then
                    gridDetalle.DataSource = dtter
                    gridDetalle.DataBind()
                Else
                    gridDetalle.DataSource = Nothing
                    gridDetalle.DataBind()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub gridDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDetalle.RowCommand
        Try
            If e.CommandName = "enviar" Then
                Dim idgen, idgenreg As Integer
                Dim strZona As String
                idgen = gridDetalle.DataKeys(e.CommandArgument).Values(0)
                strZona = gridDetalle.DataKeys(e.CommandArgument).Values(1)

                Dim orow As GridViewRow
                For Each orow In gridDetalle.Rows
                    idgenreg = gridDetalle.DataKeys(orow.RowIndex).Value

                    Dim textcorreo As TextBox = orow.FindControl("txtcorreo")

                    If idgen = idgenreg Then
                        Dim doc, nombre, valor, correo As String
                        Dim vecRespuesta As String

                        doc = gridDetalle.Rows(orow.RowIndex).Cells(0).Text
                        nombre = Replace(Replace(gridDetalle.Rows(orow.RowIndex).Cells(1).Text, "Ñ", "N"), "ñ", "n")
                        valor = gridDetalle.Rows(orow.RowIndex).Cells(2).Text
                        correo = textcorreo.Text
                        nombre = RemoveDiacritics(nombre)

                        vecRespuesta = doc & "|" & nombre & "|" & CDec(valor) & "|" & correo & "|" & txtFechaInicio.Value & "|" & txtFechaFin.Value & "|" & strZona

                        Response.Redirect("Formato_Envio_Carta_Generador.aspx?doc=" & vecRespuesta)
                    End If
                Next
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Shared Function RemoveDiacritics(stIn As String) As String

        Dim stFormD As String = stIn.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()

        For ich As Integer = 0 To stFormD.Length - 1
            Dim uc As UnicodeCategory = CharUnicodeInfo.GetUnicodeCategory(stFormD(ich))
            If uc <> UnicodeCategory.NonSpacingMark Then
                sb.Append(stFormD(ich))
            End If
        Next

        Return (sb.ToString().Normalize(NormalizationForm.FormC))

    End Function
End Class
