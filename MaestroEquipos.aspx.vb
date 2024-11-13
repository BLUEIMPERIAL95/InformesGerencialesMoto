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
Partial Class MaestroEquipos
    Inherits System.Web.UI.Page
    Dim csequipos As New equiposcomputo
    Dim csoper As New Operaciones
    Dim csusua As New usuarios

    Private Sub Equipos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(28, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2042, Session("id_usua"))

        If strRespuestaPer = "" Then
            tbl_ficha_tecnica.Visible = True
        End If

        'If Session("documento") <> "98702336" And Session("documento") <> "71760116" Then
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
        '    Response.Redirect("Default.aspx")
        'End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
        End If
    End Sub

    Sub combos()
        Dim dtSuc, dttipequ As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_tipoequipos_combo", dttipequ, cboTipoEquipo)
        csoper.LlenarDropDownList("sucursal", "sucursal", "sucursales_mostrar", dtSuc, cboAgencia)
    End Sub

    Sub LlenarGrid()
        Dim dtequ As New DataTable

        dtequ = csequipos.seleccionar_datos_equipos_completo()

        gridEquipos.DataSource = dtequ
        gridEquipos.DataBind()
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtNombre.Text = "" Or cboEmpresa.SelectedValue = "-1" Or cboAgencia.SelectedValue = "-1" Or cboTipoEquipo.SelectedValue = "-1" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csequipos.guardar_equipos(hidequipo.Value, txtNombre.Text, txtCodigo.Text, hidcodigo.Value, txtSerie.Text, txtMarca.Text, cboEmpresa.SelectedValue,
                                                   cboAgencia.SelectedValue, cboTipoEquipo.SelectedValue, txtModelo.Text, txtSisOperativo.Text, txtSerial.Text, txtPantalla.Text,
                                                   txtDisco.Text, txtBoard.Text, txtProcesador.Text, txtRam.Text, txtMouse.Text, txtTeclado.Text, txtDirMac.Text, txtDirWifi.Text,
                                                   txtColor.Text, txtObservacion.Text, cboUnidad.SelectedValue, cboSD.SelectedValue, cboActivo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo no actualizado');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub Limpiar()
        txtNombre.Text = ""
        txtCodigo.Text = ""
        hidcodigo.Value = "0"
        txtSerie.Text = ""
        txtMarca.Text = ""
        cboEmpresa.SelectedValue = "-1"
        cboAgencia.SelectedValue = "0"
        cboTipoEquipo.SelectedValue = "0"
        cboTipoEquipo.Enabled = True
        cboEmpresa.Enabled = True
        txtModelo.Text = ""
        txtSisOperativo.Text = ""
        txtSerial.Text = ""
        txtPantalla.Text = ""
        txtDisco.Text = ""
        txtBoard.Text = ""
        txtProcesador.Text = ""
        txtRam.Text = ""
        txtMouse.Text = ""
        txtTeclado.Text = ""
        txtDirMac.Text = ""
        txtDirWifi.Text = ""
        txtColor.Text = ""
        txtObservacion.Text = ""
        cboUnidad.SelectedValue = "1"
        cboSD.SelectedValue = "1"
        cboActivo.SelectedValue = "1"
        imgEquipo.Visible = False
        hidequipo.Value = 0
    End Sub

    Private Sub gridEquipos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridEquipos.RowCommand
        Try
            Dim idequipo As Integer
            Dim dtequi As New DataTable
            Dim strRes As String
            If e.CommandName = "modificar" Then
                idequipo = gridEquipos.DataKeys(e.CommandArgument).Values(0)
                hidequipo.Value = idequipo

                dtequi = csequipos.capturar_datos_equipos_por_id(hidequipo.Value)

                If dtequi.Rows.Count > 0 Then
                    txtNombre.Text = dtequi.Rows(0)("nombre_EQUI").ToString
                    txtCodigo.Text = dtequi.Rows(0)("codigo_EQUI").ToString
                    hidcodigo.Value = dtequi.Rows(0)("consecutivo_tieq").ToString
                    txtSerie.Text = dtequi.Rows(0)("serie_EQUI").ToString
                    txtMarca.Text = dtequi.Rows(0)("marca_EQUI").ToString
                    cboEmpresa.SelectedValue = dtequi.Rows(0)("empresa_EQUI").ToString
                    cboAgencia.SelectedValue = dtequi.Rows(0)("agencia_EQUI").ToString
                    cboTipoEquipo.SelectedValue = dtequi.Rows(0)("id_tieq").ToString
                    txtModelo.Text = dtequi.Rows(0)("modelo_EQUI").ToString
                    txtSisOperativo.Text = dtequi.Rows(0)("sisoper_EQUI").ToString
                    txtSerial.Text = dtequi.Rows(0)("serial_EQUI").ToString
                    txtPantalla.Text = dtequi.Rows(0)("patalla_EQUI").ToString
                    txtDisco.Text = dtequi.Rows(0)("disco_EQUI").ToString
                    txtBoard.Text = dtequi.Rows(0)("board_EQUI").ToString
                    txtProcesador.Text = dtequi.Rows(0)("proce_EQUI").ToString
                    txtRam.Text = dtequi.Rows(0)("ram_EQUI").ToString
                    txtMouse.Text = dtequi.Rows(0)("mouse_EQUI").ToString
                    txtTeclado.Text = dtequi.Rows(0)("teclado_EQUI").ToString
                    txtDirMac.Text = dtequi.Rows(0)("dirmac_EQUI").ToString
                    txtDirWifi.Text = dtequi.Rows(0)("dirwifi_EQUI").ToString
                    txtColor.Text = dtequi.Rows(0)("color_EQUI").ToString
                    txtObservacion.Text = dtequi.Rows(0)("obser_EQUI").ToString
                    cboUnidad.SelectedValue = dtequi.Rows(0)("unidad").ToString
                    cboSD.SelectedValue = dtequi.Rows(0)("sd").ToString
                    cboActivo.SelectedValue = dtequi.Rows(0)("activo").ToString
                    hidimagen.Value = 1
                    cboTipoEquipo.Enabled = False
                    cboEmpresa.Enabled = False

                    Dim pathimg As String
                    pathimg = Path.Combine(Server.MapPath("Equipos"), "Equipo_" & idequipo & "_" & hidimagen.Value & ".jpeg")

                    If File.Exists(pathimg) Then
                        imgEquipo.Visible = True
                        imgEquipo.ImageUrl = "Equipos/Equipo_" & idequipo & "_" & hidimagen.Value & ".jpeg"
                    Else
                        imgEquipo.Visible = True
                        imgEquipo.ImageUrl = "Equipos/nophoto.jpg"
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo no valido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idequipo = gridEquipos.DataKeys(e.CommandArgument).Values(0)
                    hidequipo.Value = idequipo

                    strRes = csequipos.eliminar_equipos(hidequipo.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo eliminado');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo No eliminado');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub

    Private Sub btnSalvarImagen_Click(sender As Object, e As EventArgs) Handles btnSalvarImagen.Click
        Try
            If hidequipo.Value = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe seleccionar equipo.');", True)
            Else
                If fluImagen.HasFile Then
                    Dim extensión As String = Right(fluImagen.PostedFile.ContentType.Trim, 4)
                    If extensión = "jpeg" Then
                        Dim dtequi As New DataTable
                        Dim strRuta, strRes As String
                        Dim intConsecutivo As Integer

                        dtequi = csequipos.seleccionar_ultimo_consecutivo_imagen_equipo(hidequipo.Value)

                        If dtequi.Rows.Count > 0 Then
                            intConsecutivo = dtequi.Rows(0)("proximo")
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Consecutivo imagen inválido.');", True)
                            Exit Sub
                        End If

                        strRuta = "Equipos/Equipo_" & hidequipo.Value & "_" & intConsecutivo & ".jpeg"

                        Dim pathimg As String
                        pathimg = Path.Combine(Server.MapPath("Equipos"), "Equipo_" & hidequipo.Value & "_" & intConsecutivo & ".jpeg")

                        If File.Exists(pathimg) Then
                            File.Delete(pathimg)
                        End If

                        strRes = csequipos.guardar_imagenes_equipos(hidequipo.Value, intConsecutivo, 1)

                        If strRes = "" Then
                            fluImagen.SaveAs(Server.MapPath(strRuta))

                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo guardado con exito.');", True)
                            Limpiar()
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Equipo No guardado con exito.');", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Extensión inválida. Solo jpeg.');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe seleccionar imagen.');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error almacenando imagen');", True)
        End Try
    End Sub

    Private Sub imgAnterior_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnterior.Click
        Try
            If hidimagen.Value > 1 And hidequipo.Value > 0 Then
                hidimagen.Value = hidimagen.Value - 1
                Dim pathimg As String
                pathimg = Path.Combine(Server.MapPath("Equipos"), "Equipo_" & hidequipo.Value & "_" & hidimagen.Value & ".jpeg")

                If File.Exists(pathimg) Then
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "Equipos/Equipo_" & hidequipo.Value & "_" & hidimagen.Value & ".jpeg"
                Else
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "Equipos/nophoto.jpg"
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Invalido...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub imgSiguiente_Click(sender As Object, e As ImageClickEventArgs) Handles imgSiguiente.Click
        Try
            If hidimagen.Value > 0 And hidequipo.Value > 0 Then
                hidimagen.Value = hidimagen.Value + 1
                Dim pathimg As String
                pathimg = Path.Combine(Server.MapPath("Equipos"), "Equipo_" & hidequipo.Value & "_" & hidimagen.Value & ".jpeg")

                If File.Exists(pathimg) Then
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "Equipos/Equipo_" & hidequipo.Value & "_" & hidimagen.Value & ".jpeg"
                Else
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "Equipos/nophoto.jpg"
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Invalido...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Generar_Codigo()
        If cboTipoEquipo.SelectedValue <> "0" And cboEmpresa.SelectedValue <> "-1" Then
            Dim dtcod As New DataTable

            dtcod = csequipos.seleccionar_ultimo_consecutivo_empresa_tipo_equipo(cboEmpresa.SelectedValue, cboTipoEquipo.SelectedValue)

            If dtcod.Rows.Count > 0 Then
                hidcodigo.Value = dtcod.Rows(0)("con_equ").ToString
                txtCodigo.Text = dtcod.Rows(0)("cod_equ").ToString
            Else
                txtCodigo.Text = ""
                hidcodigo.Value = 0
            End If
        Else
            txtCodigo.Text = ""
            hidcodigo.Value = 0
        End If
    End Sub

    Private Sub cboTipoEquipo_TextChanged(sender As Object, e As EventArgs) Handles cboTipoEquipo.TextChanged
        Generar_Codigo()
    End Sub

    Private Sub cboEmpresa_TextChanged(sender As Object, e As EventArgs) Handles cboEmpresa.TextChanged
        Generar_Codigo()
    End Sub
End Class
