Imports System.Data

Partial Class AdmistrarEgresos
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscamb As New cambiadero
    Dim csinformes As New Informes
    Dim cscuco As New Cuentacob

    Private Sub AdmistrarEgresos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(3063, Session("id_usua"))

            If Me.IsPostBack = False Then
                combos()
                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtage As New DataTable

        If Session("id_usua") <> "52617671" And Session("id_usua") <> "22187687" <> "98702336" And Session("documento") <> "71760116" Then
            csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo_usuario", dtage, cboAgencia, Session("id_usua"))
        Else
            csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
        End If
    End Sub

    Sub LlenarGrid()
        Dim dtcam As New DataTable

        dtcam = cscamb.seleccionar_administrador_egresos_diario_por_usuario(Session("id_usua"))

        If dtcam.Rows.Count > 0 Then
            gridCambiadero.DataSource = dtcam
            gridCambiadero.DataBind()
        Else
            gridCambiadero.DataSource = Nothing
            gridCambiadero.DataBind()
        End If
    End Sub

    Private Sub cboEmpresa_TextChanged(sender As Object, e As EventArgs) Handles cboEmpresa.TextChanged
        Dim dtage As New DataTable

        If Session("documento") <> "52617671" And Session("documento") <> "22187687" And Session("documento") <> "98702336" And Session("documento") <> "71760116" Then
            csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo_usuario", dtage, cboAgencia, Session("id_usua"))
        Else
            csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
        End If
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If cboEmpresa.SelectedValue = "0" Or cboAgencia.SelectedValue = "0" Or txtNumero.Text = "" Or txtValor.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim dtcam As New DataTable


                Dim strFecha As String
                If txtReferencia.Text.Length < 10 Then
                    strFecha = txtReferencia.Text.Substring(0, 9)
                Else
                    strFecha = txtReferencia.Text.Substring(0, 10)
                End If
                dtcam = cscamb.guardar_administrador_egresos(hidcambiadero.Value, cboEmpresa.SelectedValue, cboAgencia.SelectedValue,
                                                                 txtNumero.Text, txtValor.Text, strFecha,
                                                                 txtTercero.Text, txtMovimiento.Text, Session("id_usua"), 1)

                If dtcam.Rows.Count > 0 Then
                    If dtcam.Rows(0)("Respuesta").ToString = "OK" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción almacenada exitosamente...');", True)
                        Limpiar()
                        LlenarGrid()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción no almacenada exitosamente...');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción no almacenada exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub txtNumero_TextChanged(sender As Object, e As EventArgs) Handles txtNumero.TextChanged

    End Sub

    Private Sub cboAgencia_TextChanged(sender As Object, e As EventArgs) Handles cboAgencia.TextChanged
        Dim dtAge As New DataTable

        dtAge = cscuco.seleccionar_agencias_cuenta_cobro_por_id(cboAgencia.SelectedValue)

        If dtAge.Rows.Count > 0 Then
            hidsucursal.Value = dtAge.Rows(0)("id_sucursal").ToString
        Else
            hidsucursal.Value = "0"
        End If
        'hidsucursal.Value = cboAgencia.SelectedValue
    End Sub
    Sub calcular_valores()
        'txtDescuento.Text = txtValor.Text * (Replace(cboPorcentaje.SelectedValue, ".", ",") / 100)
        'txtTotal.Text = txtValor.Text - txtDescuento.Text
    End Sub

    Sub Limpiar()
        cboEmpresa.Enabled = True
        cboAgencia.Enabled = True
        txtNumero.Enabled = True
        hidcambiadero.Value = "0"
        cboEmpresa.SelectedValue = "3"
        cboAgencia.SelectedValue = "0"
        txtNumero.Text = ""
        txtValor.Text = ""
        txtTercero.Text = ""
        txtMovimiento.Text = ""
        txtValor.Text = ""
        txtReferencia.Text = ""
    End Sub

    Private Sub gridCambiadero_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCambiadero.RowCommand
        Try
            Dim idcam As Integer
            Dim dtcam As New DataTable
            Dim strRespuesta As String
            strRespuesta = ""
            If e.CommandName = "modificar" Then
                idcam = gridCambiadero.DataKeys(e.CommandArgument).Values(0)
                hidcambiadero.Value = idcam

                dtcam = cscamb.seleccionar_administrador_cambiadero_por_id(hidcambiadero.Value)

                If dtcam.Rows.Count > 0 Then
                    cboEmpresa.SelectedValue = dtcam.Rows(0)("id_emor").ToString
                    cboEmpresa.Enabled = False
                    Dim dtage As New DataTable
                    csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
                    cboAgencia.SelectedValue = dtcam.Rows(0)("id_agcc").ToString
                    cboAgencia.Enabled = False
                    txtNumero.Text = dtcam.Rows(0)("egreso_adca").ToString
                    txtNumero.Enabled = False
                    txtValor.Text = CInt(dtcam.Rows(0)("valor_adca"))
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro inválido...');", True)
                End If
            End If

            If e.CommandName = "eliminar" Then
                idcam = gridCambiadero.DataKeys(e.CommandArgument).Values(0)
                hidcambiadero.Value = idcam

                strRespuesta = cscamb.eliminar_administrador_cambiadero_por_id(hidcambiadero.Value, Session("id_usua"))

                If strRespuesta = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción almacenada exitosamente...');", True)
                    LlenarGrid()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción no almacenada exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            Dim dtdoc As New DataTable
            Dim strSQL As String
            strSQL = ""

            If cboEmpresa.SelectedValue = 3 Or cboEmpresa.SelectedValue = 4 Or cboEmpresa.SelectedValue = 8 Then
                strSQL += "Select MAX(movimientos_contables_detalles.valor) AS valor, "
                strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre, "
                strSQL += "movimientos_contables.fecha AS Fecha, "
                strSQL += "COALESCE(movimientos_transportes.numero, 0) AS Movimiento "
                strSQL += "From movimientos_contables "
                strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                strSQL += "LEFT JOIN movimientos_transportes ON movimientos_contables_detalles.idref_movimiento = movimientos_transportes.idmovimientos_transportes "
                strSQL += "WHERE tipo_comprobantes.descripcion Like '%EGRESO%' "
                strSQL += "And movimientos_contables_detalles.tipo = 2 "
                strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & hidsucursal.Value & " "
                strSQL += "And movimientos_contables.numero = '" & txtNumero.Text & "' "

                If cboEmpresa.SelectedValue = 3 Then
                    ConfigurationManager.AppSettings("bdsel") = "1"
                Else
                    If cboEmpresa.SelectedValue = 8 Then
                        ConfigurationManager.AppSettings("bdsel") = "2"
                    Else
                        If cboEmpresa.SelectedValue = 4 Then
                            ConfigurationManager.AppSettings("bdsel") = "3"
                        End If
                    End If
                End If

                dtdoc = csinformes.ejecutar_query_bd(strSQL)
            Else
                strSQL += "Select tbl_terceros.nombre_terc AS Nombre, "
                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                strSQL += "From tbl_movimientos_contables "
                strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%EGRESO%' "
                strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                strSQL += "And tbl_movimientos_contables.numero_moco = '" & txtNumero.Text & "' "
                strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "

                dtdoc = csinformes.ejecutar_query_bd_sql(strSQL, cboEmpresa.SelectedValue)
            End If

            If dtdoc.Rows.Count > 0 Then
                txtValor.Text = CInt(dtdoc.Rows(0)("valor"))
                txtReferencia.Text = Format(dtdoc.Rows(0)("fecha"), "yyyy-MM-dd")
                txtTercero.Text = dtdoc.Rows(0)("Nombre").ToString
                txtMovimiento.Text = dtdoc.Rows(0)("Movimiento").ToString

                calcular_valores()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Egreso inválido...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnInforme_Click(sender As Object, e As EventArgs) Handles btnInforme.Click
        Response.Redirect("InfParamAdminCambiadero.aspx")
    End Sub
End Class
