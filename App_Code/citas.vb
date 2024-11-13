Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class citas
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 26/07/2022 METODO PARA SELECCIONAR CITAS DATOS CALENDARIO
    ''' </summary>
    ''' <param name="fec"></param>
    ''' <param name="mue"></param>
    ''' <returns></returns>
    Function seleccionar_citas_datos_calendario(ByVal fec As String, ByVal mue As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcit As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_citas_datos_calendario"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_act", SqlDbType.DateTime, 11, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_mue", SqlDbType.Int, 11, mue)

            dbSQL.query_sql(dtcit)
        Catch ex As Exception

        End Try

        Return dtcit

    End Function

    ''' <summary>
    ''' LGUF 27/07/2022 METODO PARA GUARDAR CITAS CARGUE DESCARGUE
    ''' </summary>
    ''' <param name="sed"></param>
    ''' <param name="mue"></param>
    ''' <param name="doc"></param>
    ''' <param name="nom"></param>
    ''' <param name="pla"></param>
    ''' <param name="hor"></param>
    ''' <param name="fec"></param>
    ''' <param name="dur"></param>
    ''' <param name="est"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_citas_cargue_descargue(ByVal sed As Integer, ByVal mue As Integer, ByVal tip As String, ByVal doc As String,
                                            ByVal nom As String, ByVal pla As String, ByVal emp As String, ByVal veh As String,
                                            ByVal hor As String, ByVal cla As String, ByVal fec As String, ByVal dur As Integer,
                                            ByVal est As String, ByVal obs As String, ByVal usu As Integer, ByVal dot As String,
                                            ByVal pes As Integer, ByVal can As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_guardar_citas_cargue_descargue"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_scd", SqlDbType.Int, 11, sed)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_mcd", SqlDbType.Int, 11, mue)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_ccd", SqlDbType.Int, 11, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_ccd", SqlDbType.VarChar, 20, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_ccd", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_ccd", SqlDbType.VarChar, 10, pla)
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_ccd", SqlDbType.VarChar, 200, emp)
            dbSQL.add_parameter(ParameterDirection.Input, "@tiv_ccd", SqlDbType.VarChar, 200, veh)
            dbSQL.add_parameter(ParameterDirection.Input, "@cla_ccd", SqlDbType.VarChar, 100, cla)
            dbSQL.add_parameter(ParameterDirection.Input, "@hor_ccd", SqlDbType.VarChar, 4, hor)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ccd", SqlDbType.VarChar, 30, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@dur_ccd", SqlDbType.Int, 11, dur)
            dbSQL.add_parameter(ParameterDirection.Input, "@dot_ccd", SqlDbType.VarChar, 100, dot)
            dbSQL.add_parameter(ParameterDirection.Input, "@pes_ccd", SqlDbType.Int, 11, pes)
            dbSQL.add_parameter(ParameterDirection.Input, "@can_ccd", SqlDbType.VarChar, 100, can)
            dbSQL.add_parameter(ParameterDirection.Input, "@est_ccd", SqlDbType.VarChar, 2, est)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_ccd", SqlDbType.VarChar, 3000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 28/04/2022 METODO PARA CUMPLIR O CANCELAR CITA CARGUE DESCARGUE
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="dur"></param>
    ''' <returns></returns>
    Function cumplir_cancelar_cita_cargue_descargue(ByVal ide As Integer, ByVal dur As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_cumplir_cancelar_cita_cargue_descargue"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ccd", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@dur_ccd", SqlDbType.Int, 11, dur)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 28/07/2022 METODO PARA SELECCIONAR CITAS CARGUE DESCARGUE POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_citas_cargue_descargue_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcit As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_citas_cargue_descargue_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ccd", SqlDbType.BigInt, 11, ide)

            dbSQL.query_sql(dtcit)
        Catch ex As Exception

        End Try

        Return dtcit

    End Function

    ''' <summary>
    ''' LGUF 29/07/2022 METODO PARA SELECCIONAR CITAS CARGUE DESCARGUE POR FECHA
    ''' </summary>
    ''' <param name="ini"></param>
    ''' <param name="fin"></param>
    ''' <returns></returns>
    Function seleccionar_citas_cargue_descargue_por_id_informe(ByVal ini As String, ByVal fin As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcit As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_citas_cargue_descargue_por_id_informe"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 30, ini)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 30, fin)

            dbSQL.query_sql(dtcit)
        Catch ex As Exception

        End Try

        Return dtcit

    End Function
End Class
