Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class vehiculos
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 24/01/2020 METODO PARA SELECCIONAR VEHICULOS COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_vehiculos_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtveh As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_vehiculos_completo"

            dbSQL.query_sql(dtveh)
        Catch ex As Exception

        End Try

        Return dtveh

    End Function

    ''' <summary>
    ''' LGUF 24/01/2020 METODO PARA GUARDAR VEHICULOS
    ''' </summary>
    ''' <param name="pla"></param>
    ''' <param name="mde"></param>
    ''' <param name="tpr"></param>
    ''' <param name="tpo"></param>
    ''' <param name="tfa"></param>
    ''' <param name="taf"></param>
    ''' <param name="tco"></param>
    ''' <param name="tor"></param>
    ''' <param name="tas"></param>
    ''' <param name="gps"></param>
    ''' <param name="afi"></param>
    ''' <param name="ali"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_vehiculos(ByVal pla As String, ByVal mde As Integer, ByVal tve As Integer, ByVal tpr As Integer, ByVal tpo As Integer, ByVal tfa As Integer, ByVal taf As Integer,
                               ByVal tco As Integer, ByVal tor As Integer, ByVal tas As Integer, ByVal gps As Integer, ByVal afi As Integer, ByVal ali As Integer, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_guardar_vehiculos"
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_veh", SqlDbType.VarChar, 10, pla)
            dbSQL.add_parameter(ParameterDirection.Input, "@mod_veh", SqlDbType.Int, 11, mde)
            dbSQL.add_parameter(ParameterDirection.Input, "@tvh_veh", SqlDbType.Int, 11, tve)
            dbSQL.add_parameter(ParameterDirection.Input, "@tpr_veh", SqlDbType.Int, 11, tpr)
            dbSQL.add_parameter(ParameterDirection.Input, "@tpo_veh", SqlDbType.Int, 11, tpo)
            dbSQL.add_parameter(ParameterDirection.Input, "@tfa_veh", SqlDbType.Int, 11, tfa)
            dbSQL.add_parameter(ParameterDirection.Input, "@taf_veh", SqlDbType.Int, 11, taf)
            dbSQL.add_parameter(ParameterDirection.Input, "@tco_veh", SqlDbType.Int, 11, tco)
            dbSQL.add_parameter(ParameterDirection.Input, "@tor_veh", SqlDbType.Int, 11, tor)
            dbSQL.add_parameter(ParameterDirection.Input, "@tas_veh", SqlDbType.Int, 11, tas)
            dbSQL.add_parameter(ParameterDirection.Input, "@gps_veh", SqlDbType.Bit, 1, gps)
            dbSQL.add_parameter(ParameterDirection.Input, "@afi_veh", SqlDbType.Bit, 1, afi)
            dbSQL.add_parameter(ParameterDirection.Input, "@ali_veh", SqlDbType.Bit, 1, ali)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_veh", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 24/01/2020 METODO PARA SELECCIONAR VEHICULOS POR PLACA
    ''' </summary>
    ''' <param name="pla"></param>
    ''' <returns></returns>
    Function seleccionar_datos_vehiculos_por_placa(ByVal pla As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtveh As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_vehiculos_por_placa"
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_veh", SqlDbType.VarChar, 10, pla)

            dbSQL.query_sql(dtveh)
        Catch ex As Exception

        End Try

        Return dtveh

    End Function

    ''' <summary>
    ''' LGUF 24/01/2020 METODO PARA ELIMINAR VEHICULOS
    ''' </summary>
    ''' <param name="pla"></param>
    ''' <returns></returns>
    Function eliminar_vehiculos(ByVal pla As String) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_vehiculos"
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_veh", SqlDbType.VarChar, 10, pla)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 27/01/2020 METODO PARA CAPTURAR VALORES DE COBERTURA
    ''' </summary>
    ''' <param name="pla"></param>
    ''' <param name="ipr"></param>
    ''' <param name="ico"></param>
    ''' <returns></returns>
    Function seleccionar_valores(ByVal pla As String, ByVal ipr As Integer, ByVal ico As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtveh As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_valor_cobertura"
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_veh", SqlDbType.VarChar, 10, pla)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pro", SqlDbType.Int, 11, ipr)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cob", SqlDbType.Int, 11, ico)

            dbSQL.query_sql(dtveh)
        Catch ex As Exception

        End Try

        Return dtveh

    End Function
End Class
