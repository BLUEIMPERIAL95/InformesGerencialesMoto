Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class cad
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 2022-06-08 METODO PARA SELECCIONAR TODOS LOS CAD DE ENVIO
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function seleccionar_cad_envio_listado(ByVal ind As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtegr As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_envio_listado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 15, par)

            dbSQL.query_sql(dtegr)
        Catch ex As Exception

        End Try

        Return dtegr

    End Function

    ''' <summary>
    ''' LGUF 15/06/2022 METODO PARA GENERAR NUMERO PROXIMO CAD ENVIO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_numero_proximo_cad_envio() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_numero_proximo_cad_envio"

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 15/06/2022 METODO PARA GUARDAR ENVIO CAD
    ''' </summary>
    ''' <param name="num"></param>
    ''' <param name="fec"></param>
    ''' <param name="ide"></param>
    ''' <param name="ida"></param>
    ''' <param name="tid"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_cad_envio_encabezado(ByVal idc As Integer, ByVal num As Integer, ByVal fec As String, ByVal ide As Integer,
                                          ByVal ida As Integer, ByVal tid As String, ByVal obs As String, ByVal usu As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_cad_envio_encabezado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, idc)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_cae", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_cae", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.Int, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_age", SqlDbType.Int, 11, ida)
            dbSQL.add_parameter(ParameterDirection.Input, "@tid_cae", SqlDbType.VarChar, 50, tid)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_cae", SqlDbType.VarChar, 1000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 15/06/2022 METODO PARA GUARDAR ENVIO DETALLE CAD
    ''' </summary>
    ''' <param name="idc"></param>
    ''' <param name="num"></param>
    ''' <param name="ter"></param>
    ''' <param name="fol"></param>
    ''' <param name="obs"></param>
    ''' <returns></returns>
    Function guardar_cad_envio_detalle(ByVal ide As Integer, ByVal idc As Integer, ByVal num As Integer, ByVal ter As String, ByVal fol As Integer, ByVal obs As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_guardar_cad_envio_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cad", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, idc)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_cad", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ter_cad", SqlDbType.VarChar, 200, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@fol_cad", SqlDbType.Int, 11, fol)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_cad", SqlDbType.VarChar, 1000, obs)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 15/06/2022 METODO PARA SELECCIONAR ENVIO CAD DETALLE
    ''' </summary>
    ''' <param name="idd"></param>
    ''' <returns></returns>
    Function seleccionar_cad_envio_detalle(ByVal idd As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_envio_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.Int, 11, idd)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 16/06/2022 METODO PARA BUSCAR DOCUMENTO EN DETALLE CAD ENVIO
    ''' </summary>
    ''' <param name="num"></param>
    ''' <param name="com"></param>
    ''' <returns></returns>
    Function seleccionar_cad_envio_detalle_por_documento(ByVal num As Integer, ByVal ids As Integer, ByVal com As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_envio_detalle_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@num_doc", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_suc", SqlDbType.Int, 11, ids)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_com", SqlDbType.VarChar, 50, com)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    Function seleccionar_cad_envio_detalle_por_documento_informe(ByVal num As Integer, ByVal ids As Integer, ByVal com As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_envio_detalle_por_documento_informe"
            dbSQL.add_parameter(ParameterDirection.Input, "@num_doc", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_suc", SqlDbType.Int, 11, ids)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_com", SqlDbType.VarChar, 50, com)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 16/06/2022 METODO PARA ELIMINAR DETALLE CAD ENVIO
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_cad_envio_detalle(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cad_envio_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cad", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 16/06/2022 METODO PARA EMITIR CAD ENVIO
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function emitir_cad_envio(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_emitir_cad_envio"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 22/06/2022 METODO PARA SELECCIONAR LISTADO CAD RECIBIDO
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function seleccionar_cad_recibido_listado(ByVal ind As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_recibido_listado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 15, par)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 22/06/2022 METODO PARA SELECCIONAR DETALLE RECIBIDO SELECCION
    ''' </summary>
    ''' <param name="idd"></param>
    ''' <returns></returns>
    Function seleccionar_cad_recibido_seleccion(ByVal idd As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_recibido_seleccion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.Int, 11, idd)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 22/06/2022 METODO PARA SELECCIONAR NUMERO PROXIMO CAD RECIBIDO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_numero_proximo_cad_recibido() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_numero_proximo_cad_recibido"

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 22/06/2022 METODO PARA GUARDAR CAD RECIBIDO ENCABEZADO
    ''' </summary>
    ''' <param name="idc"></param>
    ''' <param name="ide"></param>
    ''' <param name="num"></param>
    ''' <param name="fec"></param>
    ''' <param name="ide"></param>
    ''' <param name="ida"></param>
    ''' <param name="tid"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_cad_recibido_encabezado(ByVal idc As Integer, ByVal ide As Integer, ByVal num As Integer, ByVal fec As String, ByVal ido As Integer,
                                          ByVal ida As Integer, ByVal tid As String, ByVal obs As String, ByVal usu As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_cad_recibido_encabezado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_car", SqlDbType.BigInt, 11, idc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_car", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_car", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.Int, 11, ido)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_age", SqlDbType.Int, 11, ida)
            dbSQL.add_parameter(ParameterDirection.Input, "@tid_car", SqlDbType.VarChar, 50, tid)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_car", SqlDbType.VarChar, 1000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 22/06/2022 METODO PARA GUARDAR DETALLE RECIBIDO CAD
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="idc"></param>
    ''' <param name="num"></param>
    ''' <param name="ter"></param>
    ''' <param name="fol"></param>
    ''' <param name="obs"></param>
    ''' <returns></returns>
    Function guardar_cad_recibido_detalle(ByVal ide As Integer, ByVal idc As Integer, ByVal num As Integer, ByVal ter As String, ByVal fol As Integer, ByVal obs As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_guardar_cad_recibido_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_car", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, idc)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_cad", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ter_cad", SqlDbType.VarChar, 200, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@fol_cad", SqlDbType.Int, 11, fol)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_cad", SqlDbType.VarChar, 1000, obs)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 07/07/2022 METODO PARA SELECCIONAR CAD RECIBIDO DETALLE POR DOCUMENTO
    ''' </summary>
    ''' <param name="num"></param>
    ''' <param name="com"></param>
    ''' <returns></returns>
    Function seleccionar_cad_recibido_detalle_por_documento(ByVal num As Integer, ByVal ids As Integer, ByVal com As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_recibido_detalle_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@num_doc", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_suc", SqlDbType.Int, 11, ids)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_com", SqlDbType.VarChar, 50, com)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    Function seleccionar_cad_recibido_detalle_por_documento_informe(ByVal num As Integer, ByVal ids As Integer, ByVal com As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_recibido_detalle_por_documento_informe"
            dbSQL.add_parameter(ParameterDirection.Input, "@num_doc", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_suc", SqlDbType.Int, 11, ids)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_com", SqlDbType.VarChar, 50, com)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 07/07/2022 METODO PARA SELECCIONAR CAD RECIBIDO DETALLE
    ''' </summary>
    ''' <param name="idd"></param>
    ''' <returns></returns>
    Function seleccionar_cad_recibido_detalle(ByVal idd As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cad_recibido_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.Int, 11, idd)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 07/07/2022 METODO PARA ELIMINAR CAD RECIBIDO DETALLE
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_cad_recibido_detalle(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cad_recibido_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cad", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 07/07/2022 METODO PARA EMITIR CAD ENVIO
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function emitir_cad_recibido(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_emitir_cad_recibido"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 14/07/2022 METODO PARA ELIMINAR CAD ENVIO DETALLE POR COMPROBANTE
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="num"></param>
    ''' <returns></returns>
    Function eliminar_cad_envio_detalle_por_comprobante(ByVal ide As Integer, ByVal num As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cad_envio_detalle_por_comprobante"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_cae", SqlDbType.BigInt, 11, num)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 14/07/2022 METODO PARA ELIMINAR CAD ENVIO POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_cad_envio_por_id(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cad_envio_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cae", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 14/07/2022 METODO PARA ELIMINAR CAD RECIBIDO POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_cad_recibido_por_id(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cad_recibido_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_car", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function
End Class
