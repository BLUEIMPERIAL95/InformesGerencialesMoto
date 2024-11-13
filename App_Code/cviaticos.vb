Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class cviaticos
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA SELECCIONAR VIATICOS
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function seleccionar_viaticos(ByVal ind As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtvia As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_viaticos"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 15, par)

            dbSQL.query_sql(dtvia)
        Catch ex As Exception

        End Try

        Return dtvia

    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA SELECCIONAR NUMERO PROXIMO VIATICO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_numero_proximo_viatico() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_numero_proximo_viatico"

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad

    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA GUARDAR EL ENCABEZADO DEL VIATICO
    ''' </summary>
    ''' <param name="idv"></param>
    ''' <param name="num"></param>
    ''' <param name="fec"></param>
    ''' <param name="ter"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_viatico_encabezado(ByVal idv As Integer, ByVal num As Integer, ByVal fec As String, ByVal ter As Integer, ByVal obs As String, ByVal usu As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_viatico_encabezado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_via", SqlDbType.BigInt, 11, idv)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_via", SqlDbType.BigInt, 11, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_via", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_via", SqlDbType.VarChar, 2000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA SELECCIONAR VIATICOS DETALLES
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <returns></returns>
    Function seleccionar_viaticos_detalle(ByVal ind As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtvia As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_viaticos_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_via", SqlDbType.Int, 11, ind)

            dbSQL.query_sql(dtvia)
        Catch ex As Exception

        End Try

        Return dtvia

    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA GUARDAR VIATICOS DETALLES
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
    Function guardar_viatico_detalle(ByVal vid As Integer, ByVal via As Integer, ByVal ter As Integer, ByVal tip As String, ByVal ref As String, ByVal can As Decimal, ByVal val As Decimal, ByVal obs As String) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_viatico_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_vid", SqlDbType.BigInt, 11, vid)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_via", SqlDbType.BigInt, 11, via)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_vid", SqlDbType.VarChar, 50, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@ref_vid", SqlDbType.VarChar, 50, ref)
            dbSQL.add_parameter(ParameterDirection.Input, "@can_vid", SqlDbType.Decimal, 18, can)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_vid", SqlDbType.Decimal, 18, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_vid", SqlDbType.VarChar, 1000, obs)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function

    ''' <summary>
    ''' LGUF 2024-05-06 METODO PARA ELIMINAR VIATICO DETALLE
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_viatico_detalle(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_viatico_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_vid", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA EMITIR VIATICO
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function emitir_viatico(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_emitir_viatico"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_via", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 2024-05-05 METODO PARA SELECCIONAR VIATICOS POR FECHA
    ''' </summary>
    ''' <param name="fei"></param>
    ''' <param name="fef"></param>
    ''' <returns></returns>
    Function seleccionar_viaticos_por_fecha(ByVal fei As String, ByVal fef As String) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcad As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_viaticos_por_fecha"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fei)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fef)

            dbSQL.query_sql(dtcad)
        Catch ex As Exception

        End Try

        Return dtcad
    End Function
End Class
