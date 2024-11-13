Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class cambiadero
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 08/09/2022 METODO PARA SELECCIONAR ADMINISTRADOR CAMBIADERO DIARIO POR USUARIO
    ''' </summary>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function seleccionar_administrador_cambiadero_diario_por_usuario(ByVal usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_administrador_cambiadero_diario_por_usuario"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam

    End Function

    ''' <summary>
    ''' LGUF 08/09/2022 METODO PARA GUARDAR ADMINISTRAR CAMBIADERO
    ''' </summary>
    ''' <param name="ida"></param>
    ''' <param name="iem"></param>
    ''' <param name="iag"></param>
    ''' <param name="ieg"></param>
    ''' <param name="med"></param>
    ''' <param name="por"></param>
    ''' <param name="val"></param>
    ''' <param name="des"></param>
    ''' <param name="tot"></param>
    ''' <param name="ref"></param>
    ''' <param name="ter"></param>
    ''' <param name="usu"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_administrador_cambiadero(ByVal ida As Integer, ByVal iem As Integer, ByVal iag As Integer, ByVal ieg As Integer,
                                          ByVal med As String, ByVal por As Decimal, ByVal val As Decimal, ByVal des As Decimal,
                                          ByVal tot As Decimal, ByVal ref As String, ByVal ter As String, ByVal usu As Integer,
                                          ByVal act As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_administrador_cambiadero"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_adc", SqlDbType.BigInt, 11, ida)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.Int, 11, iem)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_agc", SqlDbType.Int, 11, iag)
            dbSQL.add_parameter(ParameterDirection.Input, "@egr_adc", SqlDbType.BigInt, 11, ieg)
            dbSQL.add_parameter(ParameterDirection.Input, "@mep_adc", SqlDbType.VarChar, 50, med)
            dbSQL.add_parameter(ParameterDirection.Input, "@por_adc", SqlDbType.Decimal, 18, por)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_adc", SqlDbType.Decimal, 18, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@des_adc", SqlDbType.Decimal, 18, des)
            dbSQL.add_parameter(ParameterDirection.Input, "@tot_adc", SqlDbType.Decimal, 18, tot)
            dbSQL.add_parameter(ParameterDirection.Input, "@ref_adc", SqlDbType.VarChar, 100, ref)
            dbSQL.add_parameter(ParameterDirection.Input, "@ter_adc", SqlDbType.VarChar, 200, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_adc", SqlDbType.Int, 11, act)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam
    End Function

    ''' <summary>
    ''' LGUF 08/09/2022 METODO PARA SELECCIONAR ADMINISTRADOR CAMBIADERO POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_administrador_cambiadero_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_administrador_cambiadero_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_adc", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam

    End Function

    ''' <summary>
    ''' LGUF 08/09/2022 METODO PARA ELIMINAR ADMINISTRADOR CAMBIADERO POR ID
    ''' </summary>
    ''' <param name="ida"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function eliminar_administrador_cambiadero_por_id(ByVal ida As Integer, ByVal usu As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_administrador_cambiadero_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_adc", SqlDbType.BigInt, 11, ida)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 09/09/2022 METODO PARA SELECCIONAR ADMINISTRADOR CAMBIADERO USUARIO Y FECHA
    ''' </summary>
    ''' <param name="fei"></param>
    ''' <param name="fef"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function seleccionar_administrador_cambiadero_por_usuario_fecha(ByVal fei As String, ByVal fef As String, ByVal usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_administrador_cambiadero_por_usuario_fecha"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fei)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fef)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam

    End Function

    ''' <summary>
    ''' LGUF 2023-09-04 METODO PARA GUARDAR ADMINISTRADOR EGRESOS
    ''' </summary>
    ''' <param name="ida"></param>
    ''' <param name="iem"></param>
    ''' <param name="iag"></param>
    ''' <param name="ieg"></param>
    ''' <param name="val"></param>
    ''' <param name="ref"></param>
    ''' <param name="ter"></param>
    ''' <param name="usu"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_administrador_egresos(ByVal ida As Integer, ByVal iem As Integer, ByVal iag As Integer, ByVal ieg As Integer,
                                          ByVal val As Decimal, ByVal ref As String, ByVal ter As String, ByVal mov As Integer,
                                          ByVal usu As Integer, ByVal act As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_administrador_egresos"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_adc", SqlDbType.BigInt, 11, ida)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.Int, 11, iem)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_agc", SqlDbType.Int, 11, iag)
            dbSQL.add_parameter(ParameterDirection.Input, "@egr_adc", SqlDbType.BigInt, 11, ieg)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_adc", SqlDbType.Decimal, 18, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@ref_adc", SqlDbType.VarChar, 100, ref)
            dbSQL.add_parameter(ParameterDirection.Input, "@ter_adc", SqlDbType.VarChar, 200, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@mov_adc", SqlDbType.BigInt, 11, mov)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_adc", SqlDbType.Int, 11, act)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam
    End Function

    ''' <summary>
    ''' LGUF 04/09/2023 METODO PARA SELECCIONAR ADMINISTRADOR EGRESOS DIARIO POR USUARIO
    ''' </summary>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function seleccionar_administrador_egresos_diario_por_usuario(ByVal usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_administrador_egresos_diario_por_usuario"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam

    End Function

    ''' <summary>
    ''' LGUF 2023-09-05 METODO PARA SELECCIONAR ADMIN EGRESOS
    ''' </summary>
    ''' <param name="fei"></param>
    ''' <param name="fef"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function seleccionar_admin_cambiadero_por_usuario_fecha(ByVal fei As String, ByVal fef As String, ByVal usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_admin_cambiadero_por_usuario_fecha"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fei)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fef)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam

    End Function


    ''' <summary>
    ''' LGUF 2024-02-20 METODO PARA SELECCIONAR SALDOS CAMBIADERO LISTADO
    ''' </summary>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function seleccionar_saldos_cambiadero_listado_por_usuario(ByVal usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtegr As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_saldos_cambiadero_listado_por_usuario"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtegr)
        Catch ex As Exception

        End Try

        Return dtegr

    End Function

    ''' <summary>
    ''' LGUF 2024-02-20 METODO PARA GUARDAR SALDOS CAMBIADERO
    ''' </summary>
    ''' <param name="ida"></param>
    ''' <param name="ano"></param>
    ''' <param name="mes"></param>
    ''' <param name="sai"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_saldos_cambiadero(ByVal ida As Integer, ByVal fec As String, ByVal tip As Integer, ByVal ano As Integer, ByVal mes As Integer, ByVal sai As Decimal,
                                       ByVal obs As String, ByVal usu As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtcam As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_saldos_cambiadero"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_sac", SqlDbType.BigInt, 11, ida)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_sac", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_sac", SqlDbType.Int, 11, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@ano_sac", SqlDbType.Int, 11, ano)
            dbSQL.add_parameter(ParameterDirection.Input, "@mes_sac", SqlDbType.Int, 11, mes)
            dbSQL.add_parameter(ParameterDirection.Input, "@sal_ini", SqlDbType.Decimal, 18, sai)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_sac", SqlDbType.VarChar, 1000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtcam)
        Catch ex As Exception

        End Try

        Return dtcam
    End Function

    ''' <summary>
    ''' LGUF 2024-02-20 METODO PARA SELECCIONAR SALDOS CAMBIADERO POR ID
    ''' </summary>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function seleccionar_saldos_cambiadero_por_id(ByVal usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtegr As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_saldos_cambiadero_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_sac", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtegr)
        Catch ex As Exception

        End Try

        Return dtegr

    End Function

    ''' <summary>
    ''' LGUF 2024-02-20 METODO PARA ELIMINAR SALDOS CAMBIADERO POR ID
    ''' </summary>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function eliminar_saldos_cambiadero_por_id(ByVal usu As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_saldos_cambiadero_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_sac", SqlDbType.Int, 11, usu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta

    End Function

    ''' <summary>
    ''' LGUF 2024-02-21 METODO PARA SELECCIONAR SALDOS CAMBIADERO POR USUARIO Y FECHA
    ''' </summary>
    ''' <param name="usu"></param>
    ''' <param name="fec"></param>
    ''' <returns></returns>
    Function seleccionar_saldos_cambiadero_por_fecha_usuario(ByVal usu As Integer, ByVal fec As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtegr As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_saldos_cambiadero_por_fecha_usuario"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_sac", SqlDbType.VarChar, 15, fec)

            dbSQL.query_sql(dtegr)
        Catch ex As Exception

        End Try

        Return dtegr

    End Function

    Function seleccionar_saldos_cambiadero_listado_por_fecha(fecha_Inicial As Date, fecha_Final As Date, usu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtegr As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_saldos_cambiadero_listado_por_fecha"
            dbSQL.add_parameter(ParameterDirection.Input, "@fecha_Inicial", SqlDbType.Date, 11, fecha_Inicial)
            dbSQL.add_parameter(ParameterDirection.Input, "@fecha_Final", SqlDbType.Date, 11, fecha_Final)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            dbSQL.query_sql(dtegr)
        Catch ex As Exception

        End Try

        Return dtegr

    End Function
End Class
