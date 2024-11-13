Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Novedades
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 2024-05-16 METODO PARA SELECCIONAR NOVEDADES DE NOMINA COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_conceptos_novedades_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dcon As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_conceptos_novedades_completo"

            dbSQL.query_sql(dcon)
        Catch ex As Exception

        End Try

        Return dcon

    End Function

    ''' <summary>
    ''' LGUR 2024-05-16 METODO PARA GUARDAR LOS CONCEPTOS DE NOVEDADES DE NOMINA
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="nom"></param>
    ''' <param name="ref"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_conceptos_novedades(ByVal ide As Integer, ByVal nom As String, ByVal ref As String, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String

        Try
            dbSQL.str_storedprocedure = "sp_guardar_conceptos_novedades"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_con", SqlDbType.Int, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_con", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@ref_con", SqlDbType.VarChar, 200, ref)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_con", SqlDbType.Int, 11, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 2024-05-16 METODO PARA SELECCIONAR CONCEPTOS NOVEDADES POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_conceptos_novedades_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dcon As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_conceptos_novedades_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_con", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dcon)
        Catch ex As Exception

        End Try

        Return dcon

    End Function

    ''' <summary>
    ''' LGUF 2024-05-16 METODO PARA ELIMINAR CONCEPTOS NOVEDADES POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_conceptos_novedades_por_id(ByVal ide As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_conceptos_novedades_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_con", SqlDbType.Int, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta

    End Function

    ''' <summary>
    ''' LGUF 2024-05-16 METODO PARA SELECCIONAR NOVEDADES DE NOMINA LISTADO
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function seleccionar_novedades(ByVal ind As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtvia As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_novedades"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 15, par)

            dbSQL.query_sql(dtvia)
        Catch ex As Exception

        End Try

        Return dtvia

    End Function

    ''' <summary>
    ''' LGUF 2024-05-16 METODO PARA SELECCIONAR NUMERO PROXIMO NOVEDADES DE NOMINA
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_numero_proximo_novedades_nomina() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_numero_proximo_novedades_nomina"

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA GUARDAR EL ENCABEZADO DE LA NOVEDAD DE NOMINA
    ''' </summary>
    ''' <param name="idv"></param>
    ''' <param name="num"></param>
    ''' <param name="fec"></param>
    ''' <param name="ter"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_novedad_nomina_encabezado(ByVal idv As Integer, ByVal num As Integer, ByVal fec As String, ByVal ter As Integer, ByVal obs As String, ByVal usu As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_novedad_nomina_encabezado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_non", SqlDbType.BigInt, 11, idv)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_non", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_non", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_non", SqlDbType.VarChar, 2000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA SELECCIONAR NOVEDADES DE NOMINA DETALLES
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <returns></returns>
    Function seleccionar_novedades_nomina_detalle(ByVal ind As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtvia As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_novedades_nomina_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_non", SqlDbType.Int, 11, ind)

            dbSQL.query_sql(dtvia)
        Catch ex As Exception

        End Try

        Return dtvia

    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA GUARDAR NOVEDAD NOMINA DETALLES
    ''' </summary>
    ''' <param name="vid"></param>
    ''' <param name="via"></param>
    ''' <param name="ter"></param>
    ''' <param name="tip"></param>
    ''' <param name="ref"></param>
    ''' <param name="can"></param>
    ''' <param name="val"></param>
    ''' <param name="obs"></param>
    ''' <returns></returns>
    Function guardar_novedad_nomina_detalle(ByVal vid As Integer, ByVal via As Integer, ByVal ter As Integer, ByVal tip As Integer, ByVal ref As String, ByVal fei As String, ByVal fef As String, ByVal can As Decimal, ByVal val As Decimal, ByVal cuo As Integer, ByVal obs As String) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_novedad_nomina_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_nod", SqlDbType.BigInt, 11, vid)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_non", SqlDbType.BigInt, 11, via)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_con", SqlDbType.VarChar, 50, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@ref_nod", SqlDbType.VarChar, 50, ref)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fei)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fef)
            dbSQL.add_parameter(ParameterDirection.Input, "@can_nod", SqlDbType.Decimal, 18, can)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_nod", SqlDbType.Decimal, 18, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@cuo_con", SqlDbType.Int, 11, cuo)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_nod", SqlDbType.VarChar, 1000, obs)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 2024-05-06 METODO PARA ELIMINAR NOVEDAD NOMINA DETALLE
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_novedad_nomina_detalle(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_novedad_nomina_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_nod", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA EMITIR NOVEDAD DE NOMINA
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function emitir_novedad_nomina(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_emitir_novedad_nomina"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_non", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA SELECCIONAR NOVEDADES NOMINA POR FECHA
    ''' </summary>
    ''' <param name="fei"></param>
    ''' <param name="fef"></param>
    ''' <returns></returns>
    Function seleccionar_novedades_nomina_por_fecha(ByVal fei As String, ByVal fef As String) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_novedades_nomina_por_fecha"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fei)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fef)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 04/08/2022 METODO PARA SELECCIONAR PROXIMO CONSECUTIVO NOVEDADES IMAGEN
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_consecutivo_proximo_imagen_novedades(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtdoc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_consecutivo_proximo_imagen_novedades"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_dte", SqlDbType.BigInt, 11, ide)

            dbSQL.query_sql(dtdoc)
        Catch ex As Exception

        End Try

        Return dtdoc

    End Function

    ''' <summary>
    ''' LGUF 04/08/2022 METODO PARA GUARDAR IMAGENES NOVEDADES CONSECUTIVO
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="con"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_imagenes_novedades_consecutivo(ByVal ide As Integer, ByVal con As Integer, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_imagenes_novedades_consecutivo"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_dte", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_dim", SqlDbType.Int, 11, con)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_dim", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function
End Class
