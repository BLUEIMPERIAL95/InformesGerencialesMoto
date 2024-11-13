Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Security.Cryptography
Imports System.IO
Imports System.ComponentModel
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Configuration
Imports System
Imports System.Web.UI.WebControls
Imports System.Text


Public Class Operaciones

    Dim elAdaptador As MySqlDataAdapter

    'Dim stringConnection As String = ConfigurationManager.ConnectionStrings("systramconnectionstringtamos").ConnectionString

    Dim db As database = New database
    Dim dbsql As databaseSQL = New databaseSQL

    Public Sub LlenarDropDownList(ByVal text As String, ByVal value As String, ByVal storedProcedure As String, ByVal dt As DataTable, ByVal combo As DropDownList)

        Try
            dt.Clear()
            dt = consultas(storedProcedure)

            combo.DataSource = dt
            combo.DataTextField = text
            combo.DataValueField = value
            combo.SelectedIndex = -1
            combo.DataBind()

            combo.Items.Insert(0, " - SELECCIONE - ")
            combo.Items(0).Value = 0
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' LGUF 09/01/2020 METODO PARA CARGAR COMBOS DE BD SQL
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="value"></param>
    ''' <param name="storedProcedure"></param>
    ''' <param name="dt"></param>
    ''' <param name="combo"></param>
    Public Sub LlenarDropDownList_Sql(ByVal text As String, ByVal value As String, ByVal storedProcedure As String, ByVal dt As DataTable, ByVal combo As DropDownList)

        Try
            dt.Clear()
            dt = consultas_sql(storedProcedure)

            combo.DataSource = dt
            combo.DataTextField = text
            combo.DataValueField = value
            combo.SelectedIndex = -1
            combo.DataBind()

            combo.Items.Insert(0, " - SELECCIONE - ")
            combo.Items(0).Value = 0
        Catch ex As Exception

        End Try

    End Sub

    ''' <summary>
    ''' LGUF 13/04/2020 METODO PARA LLENAR COMBO CON UN PARAMETRO BD SQL
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="value"></param>
    ''' <param name="storedProcedure"></param>
    ''' <param name="dt"></param>
    ''' <param name="combo"></param>
    ''' <param name="par"></param>
    Public Sub LlenarDropDownList_Sql_Parametro(ByVal text As String, ByVal value As String, ByVal storedProcedure As String, ByVal dt As DataTable, ByVal combo As DropDownList, ByVal par As String)
        Try
            Dim dbsql As databaseSQL = New databaseSQL
            Dim dtmun As New DataTable
            dbsql.str_storedprocedure = storedProcedure
            dbsql.add_parameter(ParameterDirection.Input, "@param", SqlDbType.VarChar, 50, par)

            dbsql.query_sql(dtmun)

            If dtmun.Rows.Count = 0 Then
                dt.Clear()
                dt = consultas(storedProcedure)
                combo.DataSource = dt
            Else
                combo.DataSource = dtmun
            End If

            combo.DataTextField = text
            combo.DataValueField = value
            combo.SelectedIndex = -1
            combo.DataBind()

            combo.Items.Insert(0, " - SELECCIONE - ")
            combo.Items(0).Value = 0
        Catch ex As Exception
            Dim strRespuesta = ex.Message
        End Try

    End Sub

    ''' <summary>
    ''' LGUF 27/03/2020 LLENAR COMBO ESTADOS DE MOVIMIENTOS
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="value"></param>
    ''' <param name="storedProcedure"></param>
    ''' <param name="dt"></param>
    ''' <param name="combo"></param>
    ''' <param name="Parametro"></param>
    Public Sub LlenarDropDownListTipoEstadoMovimiento(ByVal text As String, ByVal value As String, ByVal storedProcedure As String, ByVal dt As DataTable, ByVal combo As DropDownList, ByVal Parametro As Integer)

        Try
            Dim dttd As New DataTable
            db.setStoredProcedureName("tipo_estados_mostrar")
            db.add_parameter(ParameterDirection.Input, "modu", MySqlDbType.Int32, 11, Parametro)
            db.query_sql(dttd)

            If dttd.Rows.Count = 0 Then
                dt.Clear()
                dt = consultas(storedProcedure)
                combo.DataSource = dt
            Else
                combo.DataSource = dttd
            End If

            combo.DataTextField = text
            combo.DataValueField = value
            combo.SelectedIndex = -1
            combo.DataBind()

            combo.Items.Insert(0, " - SELECCIONE - ")
            combo.Items(0).Value = 0
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LlenarDropDownListTipoDespachoMovimiento(ByVal text As String, ByVal value As String, ByVal storedProcedure As String, ByVal dt As DataTable, ByVal combo As DropDownList, ByVal Parametro As Integer)

        Try
            Dim dttd As New DataTable
            db.setStoredProcedureName("tipos_despachos_mostrar_combo_parametro")
            db.add_parameter(ParameterDirection.Input, "idusu", MySqlDbType.Int32, 11, Parametro)
            db.query_sql(dttd)

            If dttd.Rows.Count = 0 Then
                dt.Clear()
                dt = consultas(storedProcedure)
                combo.DataSource = dt
            Else
                combo.DataSource = dttd
            End If

            combo.DataTextField = text
            combo.DataValueField = value
            combo.SelectedIndex = -1
            combo.DataBind()

            combo.Items.Insert(0, " - SELECCIONE - ")
            combo.Items(0).Value = 0
        Catch ex As Exception

        End Try

    End Sub

    Public Sub LlenarDropDownListTipoDespachoForma(ByVal text As String, ByVal value As String, ByVal storedProcedure As String, ByVal dt As DataTable, ByVal combo As DropDownList, ByVal Parametro As Integer)

        Try
            Dim dttd As New DataTable
            db.setStoredProcedureName("tipos_despachos_mostrar_combo_parametro_forma")
            db.add_parameter(ParameterDirection.Input, "idforma", MySqlDbType.Int32, 11, Parametro)
            db.query_sql(dttd)

            'If dttd.Rows.Count = 0 Then
            '    dt.Clear()
            '    dt = consultas(storedProcedure)
            '    combo.DataSource = dt
            'Else
            combo.DataSource = dttd
            'End If

            combo.DataTextField = text
            combo.DataValueField = value
            combo.SelectedIndex = -1
            combo.DataBind()

            combo.Items.Insert(0, " - SELECCIONE - ")
            combo.Items(0).Value = 0
        Catch ex As Exception

        End Try

    End Sub

    Function consultas(ByVal storedProcedure As String) As DataTable

        Dim dtoperaciones As New DataTable

        db.setStoredProcedureName(storedProcedure)
        db.query_sql(dtoperaciones)

        Return dtoperaciones

    End Function

    ''' <summary>
    ''' LGUF 09/01/2020 EJECUTAR PROCEDIMIENTO COMBO SQL
    ''' </summary>
    ''' <param name="storedProcedure"></param>
    ''' <returns></returns>
    Function consultas_sql(ByVal storedProcedure As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequ As New DataTable
        Try
            dbSQL.str_storedprocedure = storedProcedure

            dbSQL.query_sql(dtequ)
        Catch ex As Exception

        End Try

        Return dtequ

    End Function

    Function mostrar_forma_por_tipo_despacho(ByVal idtipodespacho As Integer) As DataTable
        Dim dttide As New DataTable

        db.setStoredProcedureName("sp_mostrar_forma_factura_por_tipo_despacho")
        db.add_parameter(ParameterDirection.Input, "idtipodes", MySqlDbType.Int32, 11, idtipodespacho)

        db.query_sql(dttide)

        Return dttide
    End Function

    Function consulta_documentos_vehiculos_afiliados(ByVal idveh As Int32, ByVal pagina As Int32, ByVal par As String, ByVal ind As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_documentos_afiliados")
        db.add_parameter(ParameterDirection.Input, "idveh", MySqlDbType.Int32, 11, idveh)
        db.add_parameter(ParameterDirection.Input, "pagina", MySqlDbType.Int32, 11, pagina)
        db.add_parameter(ParameterDirection.Input, "ind", MySqlDbType.Int32, 11, ind)
        db.add_parameter(ParameterDirection.Input, "par", MySqlDbType.VarChar, 255, par)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_documentos_vehiculos_especiales(ByVal idveh As Int32, ByVal pagina As Int32, ByVal par As String, ByVal ind As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_documentos_especiales")
        db.add_parameter(ParameterDirection.Input, "idveh", MySqlDbType.Int32, 11, idveh)
        db.add_parameter(ParameterDirection.Input, "pagina", MySqlDbType.Int32, 11, pagina)
        db.add_parameter(ParameterDirection.Input, "ind", MySqlDbType.Int32, 11, ind)
        db.add_parameter(ParameterDirection.Input, "par", MySqlDbType.VarChar, 255, par)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_propietarios(ByVal idveh As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_propietario_afiliados")
        db.add_parameter(ParameterDirection.Input, "idveh", MySqlDbType.Int32, 11, idveh)

        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_propietarios_especiales(ByVal idveh As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_propietario_especiales")
        db.add_parameter(ParameterDirection.Input, "idveh", MySqlDbType.Int32, 11, idveh)

        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_datos_vehiculos_afiliados(ByVal iddoc As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_datos_vehiculos_afiliados")
        db.add_parameter(ParameterDirection.Input, "iddoc", MySqlDbType.Int32, 11, iddoc)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_ubicacion_vehiculos_afiliados(ByVal idveh As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_ubicacion_vehiculos_afiliados")
        db.add_parameter(ParameterDirection.Input, "idveh", MySqlDbType.Int32, 11, idveh)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_usuario_afiliados(ByVal idus As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("usuarios_mostrar2")
        db.add_parameter(ParameterDirection.Input, "usu", MySqlDbType.Int32, 11, idus)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_info_manifiesto(ByVal idmov As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_info_manifiesto")
        db.add_parameter(ParameterDirection.Input, "idmov", MySqlDbType.Int32, 11, idmov)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function

    Function consulta_info_remesa(ByVal idmov As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_info_remesa")
        db.add_parameter(ParameterDirection.Input, "idmov", MySqlDbType.Int32, 11, idmov)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_info_orden(ByVal idmov As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_info_orden")
        db.add_parameter(ParameterDirection.Input, "idmov", MySqlDbType.Int32, 11, idmov)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function consulta_info_urbano(ByVal idmov As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_info_urbano")
        db.add_parameter(ParameterDirection.Input, "idmov", MySqlDbType.Int32, 11, idmov)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function mostrar_pagina_documentos_afiliados(ByVal tipo_doc As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_pagina_documentos_afiliados")
        db.add_parameter(ParameterDirection.Input, "tipo_doc", MySqlDbType.Int32, 11, tipo_doc)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function mostrar_pagina_documentos_especiales(ByVal tipo_doc As Int32) As DataTable
        Dim dtdoc As New DataTable
        db.setStoredProcedureName("mostrar_pagina_documentos_especiales")
        db.add_parameter(ParameterDirection.Input, "tipo_doc", MySqlDbType.Int32, 11, tipo_doc)
        db.query_sql(dtdoc)
        Return dtdoc
    End Function
    Function actualizar_estado_vehiculos_especiales(ByVal iddocesp As Int32, ByVal estado As String) As String
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("actualizar_estado_vehiculos_especiales")
        db.add_parameter(ParameterDirection.Input, "id_doc", MySqlDbType.Int32, 11, iddocesp)
        db.add_parameter(ParameterDirection.Input, "estado", MySqlDbType.VarChar, 255, estado)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)(0)
        Return res
    End Function
    Function actualizar_estado_vehiculos_afiliados(ByVal idveh As Int32, ByVal estado As String) As String
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("actualizar_estado_vehiculos_afiliados")
        db.add_parameter(ParameterDirection.Input, "id_veha", MySqlDbType.Int32, 11, idveh)
        db.add_parameter(ParameterDirection.Input, "estado", MySqlDbType.VarChar, 255, estado)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)(0)
        Return res
    End Function
    Function actualizar_datos_vehiculos_afiliados(ByVal iddoc As Int32, ByVal datos As String, ByVal fechaem As String) As String
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("actualizar_datos_vehiculos_afiliados")
        db.add_parameter(ParameterDirection.Input, "iddoc", MySqlDbType.Int32, 11, iddoc)
        db.add_parameter(ParameterDirection.Input, "dat", MySqlDbType.VarChar, 255, datos)
        db.add_parameter(ParameterDirection.Input, "f_em", MySqlDbType.VarChar, 255, fechaem)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)(0)
        Return res
    End Function
    Function insertar_documento_vehiculos_afiliados(ByVal idveha As Int32, ByVal tipod As Int32, ByVal fechav As String, ByVal usu As Int32, ByVal fechad As String) As String
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("insertar_documento_vehiculos_afiliados")
        db.add_parameter(ParameterDirection.Input, "idveha", MySqlDbType.Int32, 11, idveha)
        db.add_parameter(ParameterDirection.Input, "tipod", MySqlDbType.Int32, 11, tipod)
        db.add_parameter(ParameterDirection.Input, "fechav", MySqlDbType.Int32, 11, fechav)
        db.add_parameter(ParameterDirection.Input, "usu", MySqlDbType.VarChar, 255, usu)
        db.add_parameter(ParameterDirection.Input, "f_des", MySqlDbType.VarChar, 255, fechad)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)(0)
        Return res
    End Function
    Function insertar_documento_vehiculos_especiales(ByVal idvehe As Int32, ByVal tipod As Int32, ByVal fechav As String, ByVal usu As Int32, ByVal fechad As String) As String
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("insertar_documento_vehiculos_especiales")
        db.add_parameter(ParameterDirection.Input, "idveha", MySqlDbType.Int32, 11, idvehe)
        db.add_parameter(ParameterDirection.Input, "tipod", MySqlDbType.Int32, 11, tipod)
        db.add_parameter(ParameterDirection.Input, "fechav", MySqlDbType.Int32, 11, fechav)
        db.add_parameter(ParameterDirection.Input, "usu", MySqlDbType.VarChar, 255, usu)
        db.add_parameter(ParameterDirection.Input, "f_des", MySqlDbType.VarChar, 255, fechad)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)(0)
        Return res
    End Function
    Function mostrar_ciudad_sucursal(ByVal sucursal As String) As String
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("mostrar_ciudad_sucursal")
        db.add_parameter(ParameterDirection.Input, "sucur", MySqlDbType.VarChar, 255, sucursal)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)("zona")
        Return res
    End Function
    Function mostrar_admin_sucursal(ByVal sucursal As String) As DataTable
        Dim res As String = ""
        Dim dt_res As New DataTable
        db.setStoredProcedureName("mostrar_ciudad_sucursal")
        db.add_parameter(ParameterDirection.Input, "sucur", MySqlDbType.VarChar, 255, sucursal)
        db.query_sql(dt_res)

        Return dt_res
    End Function


    Function mostrar_datos_vehiculo_afiliado(ByVal iddocafil As Integer) As DataTable
        Dim dt_res As New DataTable
        db.setStoredProcedureName("mostrar_datos_vehiculo_afiliado")
        db.add_parameter(ParameterDirection.Input, "iddoc", MySqlDbType.Int64, 11, iddocafil)
        db.query_sql(dt_res)
        Return dt_res
    End Function
    Function mostrar_datos_vehiculo_propietario(ByVal idv As Integer) As DataTable
        Dim dt_res As New DataTable
        db.setStoredProcedureName("mostrar_datos_vehiculo_propietario")
        db.add_parameter(ParameterDirection.Input, "idv", MySqlDbType.Int64, 11, idv)
        db.query_sql(dt_res)
        Return dt_res
    End Function
    Function mostrar_datos_propietario_tarjeta(ByVal idprop As Integer) As DataTable
        Dim dt_res As New DataTable
        db.setStoredProcedureName("mostrar_datos_propietario_tarjeta")
        db.add_parameter(ParameterDirection.Input, "idprop", MySqlDbType.VarChar, 255, idprop)
        db.query_sql(dt_res)
        Return dt_res
    End Function
    Function mostrar_tipo_documento_afiliado(ByVal tipoafiliado As Integer) As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("mostrar_tipo_documento_afiliado")
        db.add_parameter(ParameterDirection.Input, "tipoafi", MySqlDbType.Int32, 11, tipoafiliado)

        db.query_sql(dt)
        Return dt
    End Function
    Function mostrar_tipo_documento_especiales() As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("mostrar_tipo_documento_especiales")
        db.query_sql(dt)
        Return dt
    End Function
    Function cargar_consecutivo_documentos_afiliados(ByVal id_doc As Int32) As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("cargar_consecutivo_documentos_afiliados")
        db.add_parameter(ParameterDirection.Input, "id_doc", MySqlDbType.VarChar, 255, id_doc)
        db.query_sql(dt)
        Return dt
    End Function
    Function cargar_consecutivo_documentos_especiales(ByVal id_doc As Int32) As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("cargar_consecutivo_documentos_especiales")
        db.add_parameter(ParameterDirection.Input, "id_doc", MySqlDbType.VarChar, 255, id_doc)
        db.query_sql(dt)
        Return dt
    End Function
    Function insertar_documentos_afiliados(ByVal id_veha As Int32, ByVal tipo_doc As Int32, ByVal vence As String, ByVal usuario As Int32) As String
        Dim dt_res As New DataTable
        Dim res = ""
        db.setStoredProcedureName("insertar_documentos_afiliados")
        db.add_parameter(ParameterDirection.Input, "id_veha", MySqlDbType.Int32, 11, id_veha)
        db.add_parameter(ParameterDirection.Input, "tipo_doc", MySqlDbType.Int32, 11, tipo_doc)
        db.add_parameter(ParameterDirection.Input, "fecha_vence", MySqlDbType.VarChar, 255, vence)
        db.add_parameter(ParameterDirection.Input, "usuario", MySqlDbType.Int32, 11, usuario)
        db.query_sql(dt_res)
        res = dt_res.Rows(0)(0)
        Return res
    End Function
    Function editar_cedula(ByVal cedula_a As String, ByVal cedula_n As String) As String
        db.setStoredProcedureName("editar_cedula_tercero")
        db.add_parameter(ParameterDirection.Input, "cedula_a", MySqlDbType.VarChar, 50, cedula_a)
        db.add_parameter(ParameterDirection.Input, "cedula_n", MySqlDbType.VarChar, 50, cedula_n)
        Return db.execute_sql()
    End Function
    Function editar_cedula_rd(ByVal cedula_a As String, ByVal cedula_n As String, ByVal n As Integer) As String
        db.setStoredProcedureName("editar_cedula_remitente_destinatario")
        db.add_parameter(ParameterDirection.Input, "cedula_a", MySqlDbType.VarChar, 50, cedula_a)
        db.add_parameter(ParameterDirection.Input, "cedula_n", MySqlDbType.VarChar, 50, cedula_n)
        db.add_parameter(ParameterDirection.Input, "n", MySqlDbType.Int32, 11, n)
        Return db.execute_sql()
    End Function



    Function editar_placa(ByVal placa_a As String, ByVal placa_n As String) As String
        db.setStoredProcedureName("editar_placa_vehiculo")
        db.add_parameter(ParameterDirection.Input, "placa_a", MySqlDbType.VarChar, 255, placa_a.Trim.ToUpper)
        db.add_parameter(ParameterDirection.Input, "placa_n", MySqlDbType.VarChar, 255, placa_n.Trim.ToUpper)
        Return db.execute_sql()
    End Function
    Function editar_plaqueta(ByVal placa_a As String, ByVal placa_n As String) As String
        db.setStoredProcedureName("editar_plaqueta_tariler")
        db.add_parameter(ParameterDirection.Input, "placa_a", MySqlDbType.VarChar, 255, placa_a.Trim.ToUpper)
        db.add_parameter(ParameterDirection.Input, "placa_n", MySqlDbType.VarChar, 255, placa_n.Trim.ToUpper)
        Return db.execute_sql()
    End Function
    Function existe_cedula(ByVal cedula As String) As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("terceros_existe_cedula")
        db.add_parameter(ParameterDirection.Input, "ced", MySqlDbType.VarChar, 50, cedula)
        db.query_sql(dt)
        Return dt
    End Function
    Function existe_cedula_remitente_destinatario(ByVal cedula As String, ByVal n As Integer) As DataSet
        Dim dt As New DataSet
        db.setStoredProcedureName("remitente_destinatario_existe_cedula")
        db.add_parameter(ParameterDirection.Input, "ced", MySqlDbType.VarChar, 50, cedula)
        db.add_parameter(ParameterDirection.Input, "n", MySqlDbType.Int32, 11, n)
        db.query_sql_set(dt)
        Return dt
    End Function

    Function existe_placa(ByVal placa_n As String) As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("vehiculos_existe_vehiculo")
        db.add_parameter(ParameterDirection.Input, "placaveh", MySqlDbType.VarChar, 255, placa_n.Trim.ToUpper)
        db.query_sql(dt)
        Return dt
    End Function
    Function existe_plaqueta(ByVal placa_n As String) As DataTable
        Dim dt As New DataTable
        db.setStoredProcedureName("traylers_existe_trayler")
        db.add_parameter(ParameterDirection.Input, "placa", MySqlDbType.VarChar, 255, placa_n.Trim.ToUpper)
        db.query_sql(dt)
        Return dt
    End Function

    Function ejecutar_sp(ByVal sp As String) As DataTable

        Dim dtoperaciones As New DataTable

        db.setStoredProcedureName(sp)
        db.query_sql(dtoperaciones)

        Return dtoperaciones
    End Function

    Public Function EncryptString(ByVal InputString As String, ByVal SecretKey As String, Optional ByVal CyphMode As CipherMode = CipherMode.ECB) As String
        Try
            Dim Des As New TripleDESCryptoServiceProvider
            'Put the string into a byte array

            If InputString = Nothing Then
                InputString = ""
            End If

            Dim InputbyteArray() As Byte = Encoding.UTF8.GetBytes(InputString)
            'Create the crypto objects, with the key, as passed in
            Dim hashMD5 As New MD5CryptoServiceProvider
            Des.Key = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(SecretKey))
            Des.Mode = CyphMode
            Dim ms As MemoryStream = New MemoryStream
            Dim cs As CryptoStream = New CryptoStream(ms, Des.CreateEncryptor(), _
            CryptoStreamMode.Write)
            'Write the byte array into the crypto stream
            '(It will end up in the memory stream)
            cs.Write(InputbyteArray, 0, InputbyteArray.Length)
            cs.FlushFinalBlock()
            'Get the data back from the memory stream, and into a string
            Dim ret As StringBuilder = New StringBuilder
            Dim b() As Byte = ms.ToArray
            ms.Close()
            Dim I As Integer
            For I = 0 To UBound(b)
                'Format as hex
                ret.AppendFormat("{0:X2}", b(I))
            Next

            Return ret.ToString()
        Catch ex As System.Security.Cryptography.CryptographicException
            'ExceptionManager.Publish(ex)
            Return ""
        End Try

    End Function

    Public Function DecryptString(ByVal InputString As String, ByVal SecretKey As String, Optional ByVal CyphMode As CipherMode = CipherMode.ECB) As String
        Try
            If InputString = String.Empty Then
                Return ""
            Else
                Dim Des As New TripleDESCryptoServiceProvider
                'Put the string into a byte array
                Dim InputbyteArray(CType(InputString.Length / 2 - 1, Integer)) As Byte '= Encoding.UTF8.GetBytes(InputString)
                'Create the crypto objects, with the key, as passed in
                Dim hashMD5 As New MD5CryptoServiceProvider

                Des.Key = hashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(SecretKey))
                Des.Mode = CyphMode
                'Put the input string into the byte array

                Dim X As Integer

                For X = 0 To InputbyteArray.Length - 1
                    Dim IJ As Int32 = (Convert.ToInt32(InputString.Substring(X * 2, 2), 16))
                    Dim BT As New ByteConverter
                    InputbyteArray(X) = New Byte
                    InputbyteArray(X) = CType(BT.ConvertTo(IJ, GetType(Byte)), Byte)
                Next

                Dim ms As MemoryStream = New MemoryStream
                Dim cs As CryptoStream = New CryptoStream(ms, Des.CreateDecryptor(), _
                CryptoStreamMode.Write)

                'Flush the data through the crypto stream into the memory stream
                cs.Write(InputbyteArray, 0, InputbyteArray.Length)
                cs.FlushFinalBlock()

                '//Get the decrypted data back from the memory stream
                Dim ret As StringBuilder = New StringBuilder
                Dim B() As Byte = ms.ToArray

                ms.Close()

                Dim I As Integer

                For I = 0 To UBound(B)
                    ret.Append(Chr(B(I)))
                Next

                Return ret.ToString()
            End If
        Catch ex As Exception

        End Try

    End Function

    Function fechaActual() As String

        Dim anio = Date.Now.Year.ToString
        Dim mes = Date.Now.Month.ToString
        Dim dia = Date.Now.Day.ToString

        If mes.Length = 1 Then
            mes = "0" & mes
        End If

        If dia.Length = 1 Then
            dia = "0" & dia
        End If

        Return anio & "-" & mes & "-" & dia

    End Function

    Function periodoActual() As String

        Dim anio = Date.Now.Year.ToString
        Dim mes = Date.Now.Month.ToString
        
        If mes.Length = 1 Then
            mes = "0" & mes
        End If

        Return anio & mes

    End Function

    Function horaActual() As String

        Dim hora = Date.Now.Hour.ToString
        Dim minuto = Date.Now.Minute.ToString
        Dim segundo = Date.Now.Second.ToString

        If hora.Length = 1 Then
            hora = "0" & hora
        End If

        If minuto.Length = 1 Then
            minuto = "0" & minuto
        End If

        If segundo.Length = 1 Then
            segundo = "0" & minuto
        End If

        Return hora & ":" & minuto & ":" & segundo

    End Function

    Sub llenar_grid(ByVal dt As DataTable, ByVal grid As GridView, ByVal storedProcedure As String)

        dt = ejecutar_sp(storedProcedure)
        grid.DataSource = dt
        grid.DataBind()

    End Sub

    Public Function calcular_digito(ByVal documento As String) As Integer
        Dim i, numero, matriz, resultado, acumulador, residuo, peso(11), largo, digito As Integer
        Dim cociente As Decimal
        largo = documento.Length
        acumulador = 0
        peso(0) = 47
        peso(1) = 43
        peso(2) = 41
        peso(3) = 37
        peso(4) = 29
        peso(5) = 23
        peso(6) = 19
        peso(7) = 17
        peso(8) = 13
        peso(9) = 7
        peso(10) = 3
        For i = 1 To largo
            numero = Val(documento.Chars(largo - i))
            matriz = 11 - i
            resultado = numero * peso(matriz)
            acumulador += resultado
        Next
        cociente = acumulador / 11
        cociente = Int(cociente)
        residuo = acumulador - (cociente * 11)
        digito = 11 - residuo
        If digito > 9 Then
            digito = residuo
        End If

        Return digito

    End Function

    Public Function uploadFileUsingFTP(ByVal rutaFTP As String, ByVal rutalocal As String, ByVal usuarioFTP As String, ByVal pwdFTP As String) As Boolean

        Try
            Dim reqObj As FtpWebRequest = WebRequest.Create(rutaFTP)
            Dim streamObj As FileStream = File.OpenRead(rutalocal)
            Dim buffer(streamObj.Length) As Byte


            reqObj.Method = WebRequestMethods.Ftp.UploadFile
            reqObj.Credentials = New NetworkCredential(usuarioFTP, pwdFTP)
            streamObj.Read(buffer, 0, buffer.Length)
            streamObj.Close()

            streamObj = Nothing
            reqObj.GetRequestStream().Write(buffer, 0, buffer.Length)
            reqObj = Nothing
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function IsValidEmail(ByVal email As String) As Boolean
        'If email = String.Empty Then Return False
        ' Compruebo si el formato de la dirección es correcto.
        If email <> "" Then
            Dim re As Regex = New Regex("^[\w._%-]+@[\w.-]+\.[a-zA-Z]{2,4}$")
            Dim m As Match = re.Match(email)
            Return (m.Captures.Count <> 0)
        Else
            Return True
        End If
    End Function

    Public Function validar_expresiones_regulares(ByVal texto As String, ByVal expresion As String) As String
        Dim mensaje As String


        Dim exp As Regex = New Regex(expresion)
        Dim si As Boolean = exp.IsMatch(texto)

        If si Then
            mensaje = ""
        Else
            mensaje = "Campo invalido"
        End If

        Return mensaje
    End Function

    Function procesos_por_modulo(ByVal idmodulo As Integer, ByVal dt As DataTable) As Boolean

        db.setStoredProcedureName("sistema_procesos_mostrar_pormodulo")
        db.add_parameter(ParameterDirection.Input, "", MySqlDbType.Int32, 11, idmodulo)

        db.query_sql(dt)

    End Function

    Function capturar_contrasena_segura() As DataTable
        Dim dtcose As New DataTable

        db.setStoredProcedureName("sp_capturar_contrasena_segura")

        db.query_sql(dtcose)

        Return dtcose
    End Function

    Function mostrar_ciudades_siigo(ByVal ind As Int32, ByVal pag As Int32, ByVal par As String) As DataTable
        Dim dtsiigo As New DataTable
        db.setStoredProcedureName("traer_ciudades_siigo")
        db.add_parameter(ParameterDirection.Input, "ind", MySqlDbType.Int32, 11, ind)
        db.add_parameter(ParameterDirection.Input, "pagina", MySqlDbType.Int32, 11, pag)
        db.add_parameter(ParameterDirection.Input, "par", MySqlDbType.VarChar, 255, par)
        db.query_sql(dtsiigo)
        Return dtsiigo
    End Function

    Sub insertar_ciudades_siigo(ByVal id As Int32, ByVal cod As String)
        Dim dtsiigo As New DataTable
        db.setStoredProcedureName("insertar_ciudades_siigo")
        db.add_parameter(ParameterDirection.Input, "id", MySqlDbType.Int32, 11, id)
        db.add_parameter(ParameterDirection.Input, "cod", MySqlDbType.VarChar, 255, cod)
        db.execute_sql()
    End Sub

    Sub mini(ByVal ruta As String, ByVal foto As Image)

        Dim imagen = System.Drawing.Image.FromFile(ruta)

        Dim ancho_real = imagen.Width, alto_real = imagen.Height, ancho = 70, alto = 60, propH = ancho / ancho_real, _
        propV = alto / alto_real, anchoF As Double, altoF As Double

        If propH > propV Then

            anchoF = ancho_real * propV
            altoF = alto

        Else

            altoF = alto_real * propH
            anchoF = ancho

        End If

        foto.Width = anchoF
        foto.Height = altoF

    End Sub

    Sub mini(ByVal ruta As String, ByVal foto As Image, ByVal ancho As Integer, ByVal alto As Integer)

        Dim imagen = System.Drawing.Image.FromFile(ruta)

        Dim ancho_real = imagen.Width, alto_real = imagen.Height, propH = ancho / ancho_real,
        propV = alto / alto_real, anchoF As Double, altoF As Double

        If propH > propV Then

            anchoF = ancho_real * propV
            altoF = alto

        Else

            altoF = alto_real * propH
            anchoF = ancho

        End If

        foto.Width = anchoF
        foto.Height = altoF

    End Sub

    'Sub minihtml(ByVal ruta As String, ByVal foto As HtmlImage, ByVal ancho As Integer, ByVal alto As Integer)

    '    Dim imagen = System.Drawing.Image.FromFile(ruta)

    '    Dim ancho_real = imagen.Width, alto_real = imagen.Height, propH = ancho / ancho_real,
    '    propV = alto / alto_real, anchoF As Double, altoF As Double

    '    If propH > propV Then

    '        anchoF = ancho_real * propV
    '        altoF = alto

    '    Else

    '        altoF = alto_real * propH
    '        anchoF = ancho

    '    End If

    '    foto.Width = anchoF
    '    foto.Height = altoF

    'End Sub

    Function formato_texto(ByVal cadena As String) As String

        cadena = cadena.Replace("Ã‘", "Ñ")
        cadena = cadena.Replace("Ã±", "ñ")
        cadena = cadena.Replace("""Ã“"", "Ó")
        cadena = cadena.Replace("Ã³", "ó")
        cadena = cadena.Replace("Ã¡", "á")

        Return cadena

    End Function

    Sub listar_estados_modulo(ByVal modulo As Integer, ByVal dt As DataTable, ByVal cmbestados As DropDownList)

        Try

            Dim db As New database

            db.setStoredProcedureName("tipo_estados_mostrar")
            db.add_parameter(ParameterDirection.Input, "modu", MySqlDbType.Int32, 11, modulo)

            db.query_sql(dt)

            cmbestados.DataSource = dt
            cmbestados.DataTextField = "descripcion"
            cmbestados.DataValueField = "idtipo_estados"
            cmbestados.SelectedIndex = -1
            cmbestados.DataBind()

            cmbestados.Items.Insert(0, " - SELECCIONE - ")
            cmbestados.Items(0).Value = 0

        Catch ex As Exception

        End Try

    End Sub

    Public Function guardar_estado(ByVal codigo As String, ByVal descripcion As String, ByVal listar As Integer, ByVal modulo As Integer) As String

        Try

            db.setStoredProcedureName("tipo_estados_guardar")
            db.add_parameter(ParameterDirection.Input, "cod", MySqlDbType.Int32, 11, codigo)
            db.add_parameter(ParameterDirection.Input, "des", MySqlDbType.VarChar, 255, descripcion)
            db.add_parameter(ParameterDirection.Input, "list", MySqlDbType.Int32, 11, listar)
            db.add_parameter(ParameterDirection.Input, "modu", MySqlDbType.Int32, 11, modulo)

            db.execute_sql()

            Return "OK"

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

    Function reemplaza_ptos(ByVal numero_con_puntuacion As String) As String
        'Por medio de esta funcion se formatea una cadena que esta con unidades de mil para guardarla como un entero en la BD

        Dim numero = ""

        For i As Integer = 0 To numero_con_puntuacion.Length - 1

            Dim caracter = numero_con_puntuacion.Substring(i, 1)

            If caracter <> "." Then
                numero = numero & caracter
            End If

        Next

        Return numero
    End Function

    Function separador_miles(ByVal numero As String) As Integer

        Dim longitud = numero.Length
        Dim caracter = 0, contador = 0

        While contador < longitud

            If caracter = 2 Then
                numero.Insert(contador, ".")
                caracter = 0
            End If
            contador += 1
            caracter += 1
        End While

        Return numero

    End Function

    Function formato_numero(ByVal numero As String) As String

        Dim retorno = ""

        For i As Integer = 0 To numero.Length - 1

            If numero.Substring(i, 1) = "." Or numero.Substring(i, 1) = "," Then
                If numero.Substring(i + 1, 1) <> "0" Then
                    retorno = retorno & "," & numero.Substring(i + 1, 1)
                Else
                    Return retorno
                End If
            Else
                retorno = retorno & numero.ToString().Substring(i, 1)
            End If

        Next

        Return retorno

    End Function

    Function sin_comas(ByVal valor As String) As String

        Dim cadena = valor.Replace(",", "")
        Return cadena

    End Function

    Function dec_ent(ByVal numero As String) As String
        'Funcion para convertir un numero decimal a un numero entero... Tecnicamente, quitar los decimales a un numero

        Dim vec As String() = numero.Split(".")
        Return vec(0)

    End Function

    Function dec_ent(ByVal numero As String, ByVal puntuacion As String) As String
        'Funcion para convertir un numero decimal a un numero entero... Tecnicamente, quitar los decimales a un numero

        Dim vec As String() = numero.Split("" & puntuacion & "")
        Return vec(0)

    End Function

    Function quitar_mascara(ByVal cadena As String) As String

        Dim retorno = ""

        For i As Integer = 0 To cadena.Length - 1

            If IsNumeric(cadena.Substring(i, 1)) Then
                retorno = retorno & cadena.Substring(i, 1)
            End If

        Next

        If cadena.Substring(0, 1) = "-" Then retorno = "-" & retorno

        Return retorno

    End Function

    Function quitar_mascara_dec(ByVal cadena As String) As String

        Dim retorno = ""

        For i As Integer = 0 To cadena.Length - 1

            If IsNumeric(cadena.Substring(i, 1)) Or cadena.Substring(i, 1) = "," Or cadena.Substring(i, 1) = "." Then
                retorno = retorno & cadena.Substring(i, 1)
            End If

        Next

        If cadena.Substring(0, 1) = "-" Then retorno = "-" & retorno

        Return retorno

    End Function

    Function valida_hora(ByVal hora As String) As String

        Dim vhor As String() = hora.Split(":")

        If Convert.ToInt32(vhor(0)) > 23 Or Convert.ToInt32(vhor(0)) < 0 Then
            Return "Formato de hora incorrecto"
        ElseIf Convert.ToInt32(vhor(1)) > 59 Or Convert.ToInt32(vhor(1)) < 0 Then
            Return "Formato de minutos incorrecto"
        Else
            Return ""
        End If

    End Function

    Function completar_ceros(ByVal numero As String, ByVal cant As Integer) As String

        Dim size_Numero = numero.Length
        Dim faltante = cant - size_Numero
        Dim retorno = ""


        If size_Numero = cant Then
            retorno = numero
        ElseIf size_Numero < cant Then
            For i As Integer = 0 To faltante - 1
                numero = "0" & numero
            Next
            retorno = numero
        End If

        Return retorno
    End Function

    Function format_campo_num(ByVal numero As String) As String

        '1. Saber que mascara tiene, con "." o con ","
        '2. Quitar decimales

        If numero.Length > 2 Then

            If numero.Substring(numero.Length - 3, 1) = "," Then

                numero = dec_ent(numero, ",")

            ElseIf numero.Substring(numero.Length - 3, 1) = "." Then

                numero = dec_ent(numero, ".")

            End If


            numero = quitar_mascara(numero)

        End If

        Return numero

    End Function

    Function format_campo_num_dec(ByVal numero As String) As String

        '1. Saber que mascara tiene, con "." o con ","
        '2. Quitar decimales

        If numero.Length > 2 Then

            'If numero.Substring(numero.Length - 3, 1) = "," Then

            '    numero = dec_ent(numero, ",")

            'ElseIf numero.Substring(numero.Length - 3, 1) = "." Then

            '    numero = dec_ent(numero, ".")

            'End If


            numero = quitar_mascara_dec(numero)

        End If

        Return numero

    End Function

    Sub periodo()



    End Sub

    Public Function RandomNumber(ByVal MaxNumber As Integer, _
    Optional ByVal MinNumber As Integer = 0) As Integer

        'initialize random number generator
        Dim r As New Random(System.DateTime.Now.Millisecond)

        'if passed incorrect arguments, swap them
        'can also throw exception or return 0

        If MinNumber > MaxNumber Then
            Dim t As Integer = MinNumber
            MinNumber = MaxNumber
            MaxNumber = t
        End If

        Return r.Next(MinNumber, MaxNumber)

    End Function

    'Sub log_Errores(ByVal mensaje As String)

    '    Dim db As New database
    '    db.setStoredProcedureName("log_errores_guardar")
    '    db.add_parameter(ParameterDirection.Input, "mje", MySqlDbType.String, 0, mensaje)

    '    db.execute_sql()

    'End Sub
    Function quitar_caracteres_especiales(ByVal cadena As String) As String
        Dim rpta = cadena.ToString
        rpta = rpta.Replace("#", " ")
        rpta = rpta.Replace(";", " ")
        rpta = rpta.Replace("Ñ", "N")
        rpta = rpta.Replace(",", " ")
        rpta = rpta.Replace("Á", "A")
        rpta = rpta.Replace("É", "E")
        rpta = rpta.Replace("Í", "I")
        rpta = rpta.Replace("Ó", "O")
        rpta = rpta.Replace("Ú", "U")
        'rpta = rpta.Replace("#", " ")
        'rpta = rpta.Replace("#", " ")
        'rpta = rpta.Replace("#", " ")
        'rpta = rpta.Replace("#", " ")
        Return rpta
    End Function

    Function quitar_espacio(ByVal cadena As String) As String
        Dim rpta = cadena.ToString
        rpta = rpta.Replace(" ", "")
        'rpta = rpta.Replace("", " ")
        'rpta = rpta.Replace("#", " ")
        'rpta = rpta.Replace("#", " ")
        'rpta = rpta.Replace("#", " ")
        'rpta = rpta.Replace("#", " ")
        Return rpta
    End Function
    '======================================INICIO==========================================================
    ' Editado Por: yeison Hiestroza M.
    ' Fecha: 17 May 2017

    Function vehiculos_especiales_idespecial(ByVal idveh As Integer) As DataTable
        Dim dti As New DataTable
        db.setStoredProcedureName("vehiculos_especiales_idespecial")
        db.add_parameter(ParameterDirection.Input, "idvehiculo", MySqlDbType.Int32, 11, idveh)
        db.query_sql(dti)
        Return dti
    End Function

    Function vehiculos_propietarios_mostrar_propietarios_porvehiculo(ByVal idveh As Integer) As DataTable
        Dim dtPropietario As New DataTable
        db.setStoredProcedureName("vehiculos_propietarios_mostrar_propietarios_porvehiculo")
        db.add_parameter(ParameterDirection.Input, "idvehic", MySqlDbType.Int32, 11, idveh)
        db.query_sql(dtPropietario)
        Return dtPropietario
    End Function


    Function sistema_empresa() As DataTable
        Dim dtEmpresa As New DataTable
        db.setStoredProcedureName("sistema_empresa")
        db.query_sql(dtEmpresa)
        Return dtEmpresa
    End Function

    ' Editado Por: yeison Hiestroza M.
    ' Fecha: 23 y 25 de  May 2017
    Function vehiculos_tenencias_poseedores(ByVal idvehiculo As Integer) As DataTable
        Dim dtPoseedores As New DataTable
        db.setStoredProcedureName("vehiculos_tenencias_poseedores")
        db.add_parameter(ParameterDirection.Input, "idvehiculo", MySqlDbType.Int32, 11, idvehiculo)
        db.query_sql(dtPoseedores)
        Return (dtPoseedores)
    End Function


    Function vehiculo_idvehiculo_especial(ByVal idvehiculo As Integer) As DataTable
        Dim dtespecial As New DataTable
        db.setStoredProcedureName("vehiculo_idvehiculo_especial")
        db.add_parameter(ParameterDirection.Input, "idvehiculo", MySqlDbType.Int32, 11, idvehiculo)
        db.query_sql(dtespecial)
        Return (dtespecial)
    End Function

    Function poseedor_validar_cartera(ByVal idvehiculo As String) As DataTable
        Dim dtcartera As New DataTable
        db.setStoredProcedureName("poseedor_cartera_pendiente")
        db.add_parameter(ParameterDirection.Input, "idtercero", MySqlDbType.Int32, 11, idvehiculo)
        db.query_sql(dtcartera)
        Return (dtcartera)
    End Function
    '===============================================FIN=======================================================

    Function terceros_buscar_individual(ByVal buscar As String) As DataTable

        Dim dttercero As New DataTable
        db.setStoredProcedureName("terceros_buscar_individual")
        db.add_parameter(ParameterDirection.Input, "doncuemento", MySqlDbType.Int32, 11, buscar)
        db.query_sql(dttercero)
        Return (dttercero)

    End Function

End Class

Public Class NumLetra

    Dim UNIDADES As String() = {"", "un ", "dos ", "tres ", "cuatro ", "cinco ", "seis ", "siete ", "ocho ", "nueve "}
    Dim DECENAS As String() = {"diez ", "once ", "doce ", "trece ", "catorce ", "quince ", "dieciseis ", "diecisiete ", "dieciocho ", "diecinueve", "veinte ", "treinta ", "cuarenta ", "cincuenta ", "sesenta ", "setenta ", "ochenta ", "noventa "}
    Dim CENTENAS As String() = {"", "ciento ", "doscientos ", "trecientos ", "cuatrocientos ", "quinientos ", "seiscientos ", "setecientos ", "ochocientos ", "novecientos "}

    Dim r As Regex


    Sub NumLetra()
    End Sub

    Public Function Convertir(ByVal numero As String, ByVal mayusculas As Boolean, ByVal moneda As String) As String
        Dim literal As String = ""
        Dim parte_decimal As String = ""
        'si el numero utiliza (.) en lugar de (,) -> se reemplaza
        numero = Replace(numero, ".", ",")
        'si el numero no tiene parte decimal, se le agrega ,00        
        If numero.IndexOf(",") = -1 Then
            numero = numero & ",00"
        End If
        'se valida formato de entrada -> 0,00 y 999 999 999,00
        'if (Pattern.matches("\\d{1,9},\\d{1,2}", numero)) {

        r = New Regex("\d{1,9},\d{1,2}")
        Dim mc As MatchCollection = r.Matches(numero)
        If mc.Count > 0 Then
            'se divide el numero 0000000,00 -> entero y decimal
            Dim Num As String() = numero.Split(",")
            'de da formato al numero decimal
            parte_decimal = Num(1) & "/100 " & moneda & "."
            'se convierte el numero a literal            
            If Num(0) = 0 Then
                literal = "cero "
            ElseIf Num(0) > 999999 Then
                literal = getMillones(Num(0))
            ElseIf Num(0) > 999 Then
                literal = getMiles(Num(0))
            ElseIf Num(0) > 99 Then
                literal = getCentenas(Num(0))
            ElseIf Num(0) > 9 Then
                literal = getDecenas(Num(0))
            Else
                literal = getUnidades(Num(0))
            End If
            'devuelve el resultado en mayusculas o minusculas
            If mayusculas Then
                Return (literal & " " & parte_decimal).ToUpper
            Else
                Return literal & " " & parte_decimal
            End If
        Else
            Return ""
        End If

    End Function

    ' funciones para convertir los numeros a literales

    Private Function getUnidades(ByVal numero As String) As String '1 - 9
        'si tuviera algun 0 antes se lo quita -> 09 = 9 o 009=9
        Dim num As String = numero.Substring(numero.Length - 1)
        Return UNIDADES(num)
    End Function

    Private Function getDecenas(ByVal numero As String) As String '99
        If numero < 10 Then 'para casos como -> 01 - 09
            Return getUnidades(numero)
        ElseIf numero > 19 Then 'para 20...99
            Dim u As String = getUnidades(numero)
            If u.Equals("") Then 'para 20,30,40,50,60,70,80,90
                Return DECENAS(numero.Substring(0, 1) + 8)
            Else
                Return DECENAS(numero.Substring(0, 1) + 8) & "y " & u
            End If
        Else
            Return DECENAS(numero - 10)
        End If
    End Function

    Private Function getCentenas(ByVal numero As String) As String
        If numero > 99 Then 'es centena
            If numero = 100 Then 'caso especial
                Return "cien "
            Else
                Return CENTENAS(numero.Substring(0, 1)) & getDecenas(numero.Substring(1))
            End If
        Else 'se quita el 0 antes de convertir a decenas
            Return getDecenas(numero)
        End If
    End Function


    Private Function getMiles(ByVal numero As String) As String
        'obtiene las centenas'
        Dim c As String = numero.Substring(numero.Length - 3)
        'obtiene los miles
        Dim m As String = numero.Substring(0, numero.Length - 3)
        Dim n As String = ""
        'se comprueba que miles tenga valor entero
        If m > 0 Then
            n = getCentenas(m)
            Return n & " mil " & getCentenas(c)
        Else
            Return "" & getCentenas(c)
        End If
    End Function

    Private Function getMillones(ByVal numero As String) As String
        'se obtiene los miles
        Dim miles As String = numero.Substring(numero.Length - 6)
        'millones
        Dim millon As String = numero.Substring(0, numero.Length - 6)
        Dim n As String = ""
        If millon > 9 Then
            n = getCentenas(millon) & " millones "
        Else
            n = getUnidades(millon) & " millon "
        End If
        Return n & getMiles(miles)
    End Function


    Public Function Num2Text(ByVal value As Double) As String
        Select Case value
            Case 0 : Num2Text = "CERO"
            Case 1 : Num2Text = "UN"
            Case 2 : Num2Text = "DOS"
            Case 3 : Num2Text = "TRES"
            Case 4 : Num2Text = "CUATRO"
            Case 5 : Num2Text = "CINCO"
            Case 6 : Num2Text = "SEIS"
            Case 7 : Num2Text = "SIETE"
            Case 8 : Num2Text = "OCHO"
            Case 9 : Num2Text = "NUEVE"
            Case 10 : Num2Text = "DIEZ"
            Case 11 : Num2Text = "ONCE"
            Case 12 : Num2Text = "DOCE"
            Case 13 : Num2Text = "TRECE"
            Case 14 : Num2Text = "CATORCE"
            Case 15 : Num2Text = "QUINCE"
            Case Is < 20 : Num2Text = "DIECI" & Num2Text(value - 10)
            Case 20 : Num2Text = "VEINTE"
            Case Is < 30 : Num2Text = "VEINTI" & Num2Text(value - 20)
            Case 30 : Num2Text = "TREINTA"
            Case 40 : Num2Text = "CUARENTA"
            Case 50 : Num2Text = "CINCUENTA"
            Case 60 : Num2Text = "SESENTA"
            Case 70 : Num2Text = "SETENTA"
            Case 80 : Num2Text = "OCHENTA"
            Case 90 : Num2Text = "NOVENTA"
            Case Is < 100 : Num2Text = Num2Text(Int(value \ 10) * 10) & " Y " & Num2Text(value Mod 10)
            Case 100 : Num2Text = "CIEN"
            Case Is < 200 : Num2Text = "CIENTO " & Num2Text(value - 100)
            Case 200, 300, 400, 600, 800 : Num2Text = Num2Text(Int(value \ 100)) & "CIENTOS"
            Case 500 : Num2Text = "QUINIENTOS"
            Case 700 : Num2Text = "SETECIENTOS"
            Case 900 : Num2Text = "NOVECIENTOS"
            Case Is < 1000 : Num2Text = Num2Text(Int(value \ 100) * 100) & " " & Num2Text(value Mod 100)
            Case 1000 : Num2Text = "MIL"
            Case Is < 2000 : Num2Text = "MIL " & Num2Text(value Mod 1000)
            Case Is < 1000000 : Num2Text = Num2Text(Int(value \ 1000)) & " MIL"
                If value Mod 1000 Then Num2Text = Num2Text & " " & Num2Text(value Mod 1000)
            Case 1000000 : Num2Text = "UN MILLON DE"
            Case Is < 2000000 : Num2Text = "UN MILLON " & Num2Text(value Mod 1000000)
            Case Is < 1000000000000.0#
                Num2Text = Num2Text(Int(value / 1000000)) & " MILLONES DE "
                If (value - Int(value / 1000000) * 1000000) Then
                    Num2Text = Num2Text(Int(value / 1000000)) & " MILLONES "
                    Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000) * 1000000)
                End If
            Case 1000000000000.0# : Num2Text = "UN BILLON"
            Case Is < 2000000000000.0# : Num2Text = "UN BILLON " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
            Case Else : Num2Text = Num2Text(Int(value / 1000000000000.0#)) & " BILLONES"
                If (value - Int(value / 1000000000000.0#) * 1000000000000.0#) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
        End Select

    End Function
End Class
