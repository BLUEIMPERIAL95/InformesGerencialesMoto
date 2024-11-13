Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class equiposcomputo
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 08/01/2020 METODO PARA CAPTURAR INFORMACIÓN DE EQUIPOS COMPLETA
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_equipos_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequ As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_equipos_completo"

            dbSQL.query_sql(dtequ)
        Catch ex As Exception

        End Try

        Return dtequ

    End Function

    ''' <summary>
    ''' LGUF 09/01/2020 METODO PARA GUARDAR EQUIPOS
    ''' </summary>
    ''' <param name="idequi"></param>
    ''' <param name="nom"></param>
    ''' <param name="ser"></param>
    ''' <param name="mar"></param>
    ''' <param name="emp"></param>
    ''' <param name="age"></param>
    ''' <param name="tip"></param>
    ''' <param name="moe"></param>
    ''' <param name="sis"></param>
    ''' <param name="sri"></param>
    ''' <param name="pan"></param>
    ''' <param name="dis"></param>
    ''' <param name="boa"></param>
    ''' <param name="pro"></param>
    ''' <param name="ram"></param>
    ''' <param name="mou"></param>
    ''' <param name="tec"></param>
    ''' <param name="dma"></param>
    ''' <param name="dwi"></param>
    ''' <param name="col"></param>
    ''' <param name="obs"></param>
    ''' <param name="uni"></param>
    ''' <param name="tsd"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_equipos(ByVal idequi As Integer, ByVal nom As String, ByVal cod As String, ByVal con As String, ByVal ser As String, ByVal mar As String,
                             ByVal emp As String, ByVal age As String, ByVal tip As String, ByVal moe As String, ByVal sis As String, ByVal sri As String, ByVal pan As String,
                             ByVal dis As String, ByVal boa As String, ByVal pro As String, ByVal ram As String, ByVal mou As String, ByVal tec As String,
                             ByVal dma As String, ByVal dwi As String, ByVal col As String, ByVal obs As String, ByVal uni As Integer, ByVal tsd As Integer,
                             ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_equipos"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_equi", SqlDbType.Int, 11, idequi)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_equi", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@cod_equi", SqlDbType.VarChar, 20, cod)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_equi", SqlDbType.Int, 11, con)
            dbSQL.add_parameter(ParameterDirection.Input, "@ser_equi", SqlDbType.VarChar, 50, ser)
            dbSQL.add_parameter(ParameterDirection.Input, "@mar_equi", SqlDbType.VarChar, 100, mar)
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_equi", SqlDbType.VarChar, 50, emp)
            dbSQL.add_parameter(ParameterDirection.Input, "@age_equi", SqlDbType.VarChar, 50, age)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_equi", SqlDbType.VarChar, 50, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@mod_equi", SqlDbType.VarChar, 100, moe)
            dbSQL.add_parameter(ParameterDirection.Input, "@sis_equi", SqlDbType.VarChar, 100, sis)
            dbSQL.add_parameter(ParameterDirection.Input, "@sri_equi", SqlDbType.VarChar, 100, sri)
            dbSQL.add_parameter(ParameterDirection.Input, "@pan_equi", SqlDbType.VarChar, 100, pan)
            dbSQL.add_parameter(ParameterDirection.Input, "@dis_equi", SqlDbType.VarChar, 100, dis)
            dbSQL.add_parameter(ParameterDirection.Input, "@boa_equi", SqlDbType.VarChar, 100, boa)
            dbSQL.add_parameter(ParameterDirection.Input, "@pro_equi", SqlDbType.VarChar, 100, pro)
            dbSQL.add_parameter(ParameterDirection.Input, "@ram_equi", SqlDbType.VarChar, 100, ram)
            dbSQL.add_parameter(ParameterDirection.Input, "@mou_equi", SqlDbType.VarChar, 100, mou)
            dbSQL.add_parameter(ParameterDirection.Input, "@tec_equi", SqlDbType.VarChar, 100, tec)
            dbSQL.add_parameter(ParameterDirection.Input, "@dma_equi", SqlDbType.VarChar, 100, dma)
            dbSQL.add_parameter(ParameterDirection.Input, "@dwi_equi", SqlDbType.VarChar, 100, dwi)
            dbSQL.add_parameter(ParameterDirection.Input, "@col_equi", SqlDbType.VarChar, 100, col)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_equi", SqlDbType.VarChar, 500, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@uni_equi", SqlDbType.Bit, 1, uni)
            dbSQL.add_parameter(ParameterDirection.Input, "@tsd_equi", SqlDbType.Bit, 1, tsd)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_equi", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 09/01/2020 METODO PARA CAPTURAR INFORMACION DE EQUIPOS
    ''' </summary>
    ''' <param name="idequi"></param>
    ''' <returns></returns>
    Function capturar_datos_equipos_por_id(ByVal idequi As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequi As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_equipos_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_equi", SqlDbType.Int, 11, idequi)

            dbSQL.query_sql(dtequi)
        Catch ex As Exception

        End Try

        Return dtequi

    End Function

    ''' <summary>
    ''' LGUF 09/01/2020 METODO PARA ELIMINAR EQUIPOS
    ''' </summary>
    ''' <param name="idequi"></param>
    ''' <returns></returns>
    Function eliminar_equipos(ByVal idequi As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_equipos"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_equi", SqlDbType.Int, 11, idequi)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 14/07/2020 METODO PARA SELECCIONAR EL ULTIMO CONSECUTIVO DE IMAGEN POR EQUIPO
    ''' </summary>
    ''' <param name="idequi"></param>
    ''' <returns></returns>
    Function seleccionar_ultimo_consecutivo_imagen_equipo(ByVal idequi As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequi As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_consecutivo_proximo_imagen_equipo"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_equ", SqlDbType.Int, 11, idequi)

            dbSQL.query_sql(dtequi)
        Catch ex As Exception

        End Try

        Return dtequi

    End Function

    ''' <summary>
    ''' LGUF 14/07/2020 METODO PARA GUARDAR IMAGENES EQUIPOS
    ''' </summary>
    ''' <param name="idequ"></param>
    ''' <param name="con"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_imagenes_equipos(ByVal idequ As Integer, ByVal con As Integer, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_imagenes_consecutivo"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_equ", SqlDbType.Int, 11, idequ)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_ieq", SqlDbType.Int, 11, con)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_ieq", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 27/07/2020 METODO PARA SELECCIONAR CONSECUTIVO CODIGO EQUIPO
    ''' </summary>
    ''' <param name="emp"></param>
    ''' <param name="ieq"></param>
    ''' <returns></returns>
    Function seleccionar_ultimo_consecutivo_empresa_tipo_equipo(ByVal emp As String, ByVal ieq As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequi As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_consecutivo_codigo_equipo"
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_equ", SqlDbType.VarChar, 50, emp)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_tie", SqlDbType.Int, 11, ieq)

            dbSQL.query_sql(dtequi)
        Catch ex As Exception

        End Try

        Return dtequi

    End Function
End Class
