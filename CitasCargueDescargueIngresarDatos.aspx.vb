Imports System.Data
Partial Class CitasCargueDescargueIngresarDatos
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscita As New citas

    Private Sub CitasCargueDescargueIngresarDatos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Me.IsPostBack = False Then
                If Request.QueryString("Reserva") = "0" Then
                    CargaHoras()
                    CargaDuracion()

                    Dim strHoraAmPm As String
                    Dim strHora As String
                    Dim strMinutos As String
                    Dim intPrimerCara As Integer
                    Dim intHoraInt As Integer
                    Dim intMinutos As Integer

                    strHoraAmPm = ""
                    strHora = ""
                    strMinutos = ""
                    intPrimerCara = 0
                    intHoraInt = 0
                    intMinutos = 0

                    strHoraAmPm = Request.QueryString("Hora")

                    intPrimerCara = Convert.ToInt32(strHoraAmPm.Substring(0, 1))
                    If intPrimerCara = 0 Then
                        strHora = strHoraAmPm.Substring(1, 1)
                    Else
                        strHora = strHoraAmPm.Substring(0, 2)
                    End If
                    strMinutos = strHoraAmPm.Substring(3, 2)
                    intHoraInt = Integer.Parse(strHora)
                    intMinutos = Integer.Parse(strMinutos)

                    cboHora.SelectedValue = Convert.ToString(intHoraInt * 60 + intMinutos)
                Else
                    CargaHoras()
                    CargaDuracion()

                    Dim strHoraAmPm As String
                    Dim strHora As String
                    Dim strMinutos As String
                    Dim intPrimerCara As Integer
                    Dim intHoraInt As Integer
                    Dim intMinutos As Integer

                    strHoraAmPm = ""
                    strHora = ""
                    strMinutos = ""
                    intPrimerCara = 0
                    intHoraInt = 0
                    intMinutos = 0

                    strHoraAmPm = Request.QueryString("Hora")

                    intPrimerCara = Convert.ToInt32(strHoraAmPm.Substring(0, 1))
                    If intPrimerCara = 0 Then
                        strHora = strHoraAmPm.Substring(1, 1)
                    Else
                        strHora = strHoraAmPm.Substring(0, 2)
                    End If
                    strMinutos = strHoraAmPm.Substring(3, 2)
                    intHoraInt = Integer.Parse(strHora)
                    intMinutos = Integer.Parse(strMinutos)

                    cboHora.SelectedValue = Convert.ToString(intHoraInt * 60 + intMinutos)

                    'tbl_datos.Visible = False
                    txtDocumento.Enabled = False
                    txtNombre.Enabled = False
                    txtPlaca.Enabled = False
                    txtEmpresa.Enabled = False
                    txtTipoVehiculo.Enabled = False
                    cboHora.Enabled = False
                    cboDuracion.Enabled = False
                    txtDocTrans.Enabled = False
                    txtpeso.Enabled = False
                    txtCantidad.Enabled = False
                    txtObservacion.Enabled = False
                    btnSalvar.Enabled = False
                    tbl_cumplir_cancelar.Visible = True
                    CargaDuracionReal()

                    Dim dtcit As New DataTable

                    dtcit = cscita.seleccionar_citas_cargue_descargue_por_id(Request.QueryString("Reserva"))

                    If dtcit.Rows.Count > 0 Then
                        txtDocumento.Text = dtcit.Rows(0)("documento_cicd").ToString
                        txtNombre.Text = dtcit.Rows(0)("nombre_cicd").ToString
                        txtPlaca.Text = dtcit.Rows(0)("placa_cicd").ToString
                        txtEmpresa.Text = dtcit.Rows(0)("empresa_cicd").ToString
                        txtTipoVehiculo.Text = dtcit.Rows(0)("tipoveh_cicd").ToString
                        cboHora.Text = dtcit.Rows(0)("hora_cicd").ToString
                        cboDuracion.Text = dtcit.Rows(0)("duracion_cicd").ToString
                        txtDocTrans.Text = dtcit.Rows(0)("doctransporte_cicd").ToString
                        txtpeso.Text = dtcit.Rows(0)("peso_cicd").ToString
                        txtCantidad.Text = dtcit.Rows(0)("cantidad_cicd").ToString
                        txtObservacion.Text = dtcit.Rows(0)("observacion_cicd").ToString
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Public Sub CargaHoras()
        Dim lngI As Integer
        Dim lngJ As Integer
        Dim lngValor As Integer
        Dim strDato As String
        Dim strAmPm As String

        cboHora.Items.Clear()
        cboHora.Items.Add(New ListItem("- SELECCIONE -", "-1"))
        For lngI = 0 To 23
            For lngJ = 0 To 30 Step 30
                lngValor = lngI * 60 + lngJ
                strDato = lngI.ToString()
                strAmPm = " a.m."
                If lngI > 12 Then
                    strDato = (lngI - 12).ToString()
                    strAmPm = " p.m."
                End If
                If lngI = 12 Then
                    strAmPm = " p.m."
                    If lngJ = 0 Then strAmPm = " m."
                End If
                strDato = strDato & ":" & Right("00" & lngJ.ToString(), 2) & strAmPm
                If lngValor >= 0 Then
                    cboHora.Items.Add(New ListItem(Convert.ToString(strDato), Convert.ToString(lngValor)))
                End If
            Next
        Next
    End Sub

    Public Sub CargaDuracion()
        Dim lngI As Integer
        Dim lngJ As Integer
        Dim lngValor As Integer
        Dim strDato As String

        cboDuracion.Items.Clear()
        cboDuracion.Items.Add(New ListItem("- SELECCIONE -", "-1"))
        For lngI = 0 To 23
            For lngJ = 0 To 30 Step 30
                lngValor = lngI * 60 + lngJ
                strDato = lngI.ToString() & ":" & Right("00" & lngJ.ToString(), 2)
                If lngValor >= 0 Then
                    cboDuracion.Items.Add(New ListItem(Convert.ToString(strDato), Convert.ToString(lngValor)))
                End If
            Next
        Next
    End Sub

    Public Sub CargaDuracionReal()
        Dim lngI As Integer
        Dim lngJ As Integer
        Dim lngValor As Integer
        Dim strDato As String

        cboDuracionReal.Items.Clear()
        cboDuracionReal.Items.Add(New ListItem("- SELECCIONE -", "-1"))
        For lngI = 0 To 23
            For lngJ = 0 To 30 Step 30
                lngValor = lngI * 60 + lngJ
                strDato = lngI.ToString() & ":" & Right("00" & lngJ.ToString(), 2)
                If lngValor >= 0 Then
                    cboDuracionReal.Items.Add(New ListItem(Convert.ToString(strDato), Convert.ToString(lngValor)))
                End If
            Next
        Next
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtDocumento.Text = "" Or txtNombre.Text = "" Or txtPlaca.Text = "" Or txtEmpresa.Text = "" Or txtTipoVehiculo.Text = "" Or cboHora.SelectedValue = "-1" Or cboDuracion.SelectedValue = "-1" Or txtDocTrans.Text = "" Or txtpeso.Text = "" Or txtCantidad.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por ingresar...');", True)
            Else
                Dim strClase As String
                strClase = ""
                If Request.QueryString("Clase1") = "0" Then
                    strClase = Request.QueryString("Clase2")
                Else
                    strClase = Request.QueryString("Clase1")
                End If

                Dim strRes As String

                strRes = cscita.guardar_citas_cargue_descargue(Request.QueryString("Sede"), Request.QueryString("Muelle"), Request.QueryString("Tipo"), txtDocumento.Text,
                                                               txtNombre.Text, txtPlaca.Text, txtEmpresa.Text, txtTipoVehiculo.Text, Request.QueryString("Hora"),
                                                               strClase, Request.QueryString("Fecha") & " " + Request.QueryString("Hora") & ":00",
                                                               cboDuracion.SelectedValue, "PR", txtObservacion.Text, Session("id_usua"), txtDocTrans.Text, txtpeso.Text, txtCantidad.Text)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cita guardada exitosamente.');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cita no guardada exitosamente.');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnCumplir_Click(sender As Object, e As EventArgs) Handles btnCumplir.Click
        Try
            If cboTipoTarea.SelectedValue = "1" Then
                If cboDuracionReal.SelectedValue = "-1" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por ingresar...');", True)
                Else
                    Dim strResCum As String

                    strResCum = cscita.cumplir_cancelar_cita_cargue_descargue(Request.QueryString("Reserva"), cboDuracionReal.SelectedValue)

                    If strResCum = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cita cumplida exitosamente.');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cita no cumplida exitosamente.');", True)
                    End If
                End If
            Else
                If cboTipoTarea.SelectedValue = "0" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por ingresar...');", True)
                Else
                    Dim strResCan As String

                    strResCan = cscita.cumplir_cancelar_cita_cargue_descargue(Request.QueryString("Reserva"), cboDuracionReal.SelectedValue)

                    If strResCan = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cita cumplida exitosamente.');", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cita no cumplida exitosamente.');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboTipoTarea_TextChanged(sender As Object, e As EventArgs) Handles cboTipoTarea.TextChanged
        If cboTipoTarea.SelectedValue = "2" Then
            cboDuracionReal.SelectedValue = "-1"
            cboDuracionReal.Enabled = False
        Else
            cboDuracionReal.SelectedValue = "-1"
            cboDuracionReal.Enabled = True
        End If
    End Sub
End Class
