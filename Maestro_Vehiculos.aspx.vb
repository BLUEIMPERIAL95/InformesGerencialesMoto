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

Partial Class Maestro_Vehiculos
    Inherits System.Web.UI.Page
    Dim csveh As New vehiculos
    Dim csoper As New Operaciones
    Dim csusua As New usuarios

    Private Sub Maestro_Vehiculos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(30, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
        End If
    End Sub

    Sub LlenarGrid()
        Dim dtveh As New DataTable

        dtveh = csveh.seleccionar_datos_vehiculos_completo()

        gridVehiculos.DataSource = dtveh
        gridVehiculos.DataBind()
    End Sub

    Sub combos()
        Dim dttiveh, dtprop, dtpose, dtfact, dtcont, dtafil, dtorga, dtaseg As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_tipoveh_combo", dttiveh, cboTipoVeh)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtprop, cboPropietario)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtpose, cboPoseedor)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtfact, cboFacturar)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtcont, cboContacto)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtafil, cboAfiliado)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtorga, cboOrganismo)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtaseg, cboAseguradora)
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtPlaca.Text = "" Or txtModelo.Text = "" Or cboTipoVeh.SelectedValue = 0 Or cboPropietario.SelectedValue = 0 Or cboPoseedor.SelectedValue = 0 Or cboFacturar.SelectedValue = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csveh.guardar_vehiculos(txtPlaca.Text, txtModelo.Text, cboTipoVeh.SelectedValue, cboPropietario.SelectedValue, cboPoseedor.SelectedValue, cboFacturar.SelectedValue,
                                                 cboAfiliado.SelectedValue, cboContacto.SelectedValue, cboOrganismo.SelectedValue, cboAseguradora.SelectedValue,
                                                 cboGps.SelectedValue, cboAfil.SelectedValue, cboAlianza.SelectedValue, cboActivo.SelectedValue)


                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Vehiculo actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Vehiculo no actualizado');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub Limpiar()
        txtPlaca.Text = ""
        txtModelo.Text = ""
        cboTipoVeh.SelectedValue = 0
        cboPropietario.SelectedValue = 0
        cboPoseedor.SelectedValue = 0
        cboFacturar.SelectedValue = 0
        cboContacto.SelectedValue = 0
        cboAfiliado.SelectedValue = 0
        cboOrganismo.SelectedValue = 0
        cboAseguradora.SelectedValue = 0
        cboGps.SelectedValue = 0
        cboAfil.SelectedValue = 0
        cboAlianza.SelectedValue = 0
        cboActivo.SelectedValue = 1
    End Sub

    Private Sub gridVehiculos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridVehiculos.RowCommand
        Try
            Dim strPlaca As String
            Dim dtvehi As New DataTable
            Dim strRes As String

            If e.CommandName = "modificar" Then
                strPlaca = gridVehiculos.DataKeys(e.CommandArgument).Values(0)
                hidplaca.Value = strPlaca

                dtvehi = csveh.seleccionar_datos_vehiculos_por_placa(hidplaca.Value)

                If dtvehi.Rows.Count > 0 Then
                    txtPlaca.Text = dtvehi.Rows(0)("placa_vetr").ToString
                    txtModelo.Text = dtvehi.Rows(0)("modelo_vetr").ToString
                    txtAños.Text = Year(Now) - txtModelo.Text
                    cboTipoVeh.SelectedValue = dtvehi.Rows(0)("id_tvtr")
                    cboPropietario.SelectedValue = dtvehi.Rows(0)("id_terc_prop")
                    cboPoseedor.SelectedValue = dtvehi.Rows(0)("id_terc_pose")
                    cboFacturar.SelectedValue = dtvehi.Rows(0)("id_terc_fact")
                    cboContacto.SelectedValue = dtvehi.Rows(0)("id_terc_cont")
                    cboOrganismo.SelectedValue = dtvehi.Rows(0)("id_terc_orga")
                    cboAfiliado.SelectedValue = dtvehi.Rows(0)("id_terc_afil")
                    cboAseguradora.SelectedValue = dtvehi.Rows(0)("id_terc_aseg")
                    cboGps.SelectedValue = dtvehi.Rows(0)("gps_vetr")
                    cboAfil.SelectedValue = dtvehi.Rows(0)("afiliado_vetr")
                    cboAlianza.SelectedValue = dtvehi.Rows(0)("alianza_vetr")
                    cboActivo.SelectedValue = dtvehi.Rows(0)("activo_vetr")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Vehiculo no valido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    strPlaca = gridVehiculos.DataKeys(e.CommandArgument).Values(0)
                    hidplaca.Value = strPlaca

                    strRes = csveh.eliminar_vehiculos(hidplaca.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Vehiculo eliminado');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Vehiculo No eliminado');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboAfiliado_TextChanged(sender As Object, e As EventArgs) Handles cboAfiliado.TextChanged

    End Sub

    Private Sub cboAlianza_TextChanged(sender As Object, e As EventArgs) Handles cboAlianza.TextChanged
        If cboAlianza.SelectedValue = 1 Then
            cboAfil.SelectedValue = 0
        End If
    End Sub

    Private Sub cboAfil_TextChanged(sender As Object, e As EventArgs) Handles cboAfil.TextChanged
        If cboAfil.SelectedValue = 1 Then
            cboAlianza.SelectedValue = 0
        End If
    End Sub

    Private Sub txtModelo_TextChanged(sender As Object, e As EventArgs) Handles txtModelo.TextChanged
        txtAños.Text = Year(Now) - txtModelo.Text
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub
End Class
