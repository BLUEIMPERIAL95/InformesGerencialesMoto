Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class reunion
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 07/02/2020 METODO PARA SELECCIONAR REUNIONES COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_reuniones_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_reuniones_completo"

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 07/02/2020 METODO PARA SELECCIONAR PARTICIPANTES DE REUNION
    ''' </summary>
    ''' <param name="idreu"></param>
    ''' <returns></returns>
    Function seleccionar_participantes_reunion(ByVal idreu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_participantes_reunion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, idreu)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 12/02/2020 METODO PARA SELECCIONAR REUNIONES POR ID
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function capturar_datos_reuniones_por_id(ByVal id As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_reuniones_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, id)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 28/02/2019 METODO PARA GUARDAR REUNIONES
    ''' </summary>
    ''' <param name="cod"></param>
    ''' <param name="fec"></param>
    ''' <param name="nom"></param>
    ''' <param name="des"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_reuniones(ByVal cod As String, ByVal fec As String, ByVal nom As String, ByVal des As String, ByVal tir As String, ByVal ter As Integer,
                               ByVal obj As String, ByVal hor As String, ByVal lug As String, ByVal act As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtres As New DataTable

        Try
            dbSQL.str_storedprocedure = "sp_guardar_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@cod_reu", SqlDbType.VarChar, 20, cod)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_reu", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_reu", SqlDbType.VarChar, 100, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@des_reu", SqlDbType.Text, 100000, des)
            dbSQL.add_parameter(ParameterDirection.Input, "@tir_reu", SqlDbType.VarChar, 2, tir)
            dbSQL.add_parameter(ParameterDirection.Input, "@exp_reu", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@obj_reu", SqlDbType.VarChar, 200, obj)
            dbSQL.add_parameter(ParameterDirection.Input, "@hor_reu", SqlDbType.VarChar, 15, hor)
            dbSQL.add_parameter(ParameterDirection.Input, "@lug_reu", SqlDbType.VarChar, 100, lug)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_reu", SqlDbType.Bit, 1, act)

            dbSQL.query_sql(dtres)
        Catch ex As Exception
            dtres = Nothing
        End Try

        Return dtres
    End Function

    ''' <summary>
    ''' LGUF 04/03/2020 METODO PARA SELECCIONAR PROXIMO CODIGO PENDIENTE POR REUNION
    ''' </summary>
    ''' <param name="idreu"></param>
    ''' <returns></returns>
    Function seleccionar_proximo_codigo_pendientes_reuniones(ByVal idreu As Integer) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_ultimo_id_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_reu", SqlDbType.Int, 11, idreu)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 04/03/2020 METODO PARA GUARDAR PENDIENTES DE REUNIONES
    ''' </summary>
    ''' <param name="idreu"></param>
    ''' <param name="cod"></param>
    ''' <param name="des"></param>
    ''' <param name="est"></param>
    ''' <param name="pri"></param>
    ''' <param name="pla"></param>
    ''' <param name="ter"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_pendientes_reuniones(ByVal idreu As Integer, ByVal cod As String, ByVal nom As String, ByVal des As String, ByVal est As String, ByVal pri As String,
                                          ByVal pla As String, ByVal ter As Integer, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, idreu)
            dbSQL.add_parameter(ParameterDirection.Input, "@cod_per", SqlDbType.VarChar, 20, cod)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_per", SqlDbType.VarChar, 100, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@des_per", SqlDbType.Text, 1000000, des)
            dbSQL.add_parameter(ParameterDirection.Input, "@est_per", SqlDbType.VarChar, 100, est)
            dbSQL.add_parameter(ParameterDirection.Input, "@pri_per", SqlDbType.VarChar, 50, pri)
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_per", SqlDbType.VarChar, 15, pla)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_per", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' METODO PARA SELECCIONAR PENDIENTES REUNION POR ID REUNION 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function capturar_datos_pendientes_reuniones_por_id_reunion(ByVal id As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_pendientes_reuniones_por_id_reunion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, id)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 04/03/2020 CAPTURAR DATOS DE PENDIENTE POR ID PENDIENTE
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function capturar_datos_pendiente_reunion_por_id_pendiente(ByVal id As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_pendiente_reunion_por_id_pendiente"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, id)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 05/03/2020 METODO PARA GUARDAR TERCEROS DE PENDIENTES REUNIONES
    ''' </summary>
    ''' <param name="idper"></param>
    ''' <param name="idter"></param>
    ''' <returns></returns>
    Function guardar_pendientes_reuniones_terceros(ByVal idper As Integer, ByVal idter As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_pendientes_reuniones_terceros"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, idper)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, idter)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 05/03/2020 METODO PARA CAPTURAR PENDIENTES REUNIONES TERCEROS POR ID PENDIENTE
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function capturar_datos_pendiente_reunion_terceros_por_id_pendiente(ByVal id As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_pendiente_reunion_terceros_por_id_pendiente"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, id)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 05/03/2020 METODO PARA ELIMINAR PENDIENTES REUNIONES TERCEROS
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function eliminar_pendiente_reunion_terceros_por_id_pendiente_tercero(ByVal id As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_pendiente_reunion_terceros_por_id_pendiente_tercero"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pet", SqlDbType.Int, 11, id)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta

    End Function

    ''' <summary>
    ''' LGUF 28/08/2020 METODO PARA MODIFICAR ESTADO PENDIENTE TERCERO
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function modificar_pendiente_reunion_terceros_por_id_pendiente_tercero(ByVal id As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_modificar_pendiente_reunion_terceros_por_id_pendiente_tercero"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pet", SqlDbType.Int, 11, id)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta

    End Function

    ''' <summary>
    ''' LGUF 05/03/2020  METODO PARA ELIMINAR PENDIENTES REUNIONES
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function eliminar_pendiente_reunion_por_id_pendiente(ByVal id As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_pendiente_reunion_por_id_pendiente"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pen", SqlDbType.Int, 11, id)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta

    End Function

    ''' <summary>
    ''' LGUF 05/03/2020 METODO PARA ELIMINAR REUNIONES
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function eliminar_reunion_por_id_reunion(ByVal id As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_reunion_por_id_reunion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, id)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta

    End Function

    ''' <summary>
    ''' LGUF 17/03/2020 GUADAR INFORMACIÓN DE CORREOS ENVIADOS DE PENDIENTES
    ''' </summary>
    ''' <param name="correo"></param>
    ''' <param name="respuesta"></param>
    ''' <param name="nombre"></param>
    ''' <param name="codigo"></param>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function guardar_log_correos(ByVal correo As String, ByVal respuesta As String, ByVal nombre As String, ByVal codigo As String, ByVal id As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_log_correos"
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_loc", SqlDbType.VarChar, 100, correo)
            dbSQL.add_parameter(ParameterDirection.Input, "@res_loc", SqlDbType.VarChar, 1000, respuesta)
            dbSQL.add_parameter(ParameterDirection.Input, "@not_loc", SqlDbType.VarChar, 100, nombre)
            dbSQL.add_parameter(ParameterDirection.Input, "@cot_loc", SqlDbType.VarChar, 100, codigo)
            dbSQL.add_parameter(ParameterDirection.Input, "@idt_loc", SqlDbType.BigInt, 11, id)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 17/03/2020 METODO PARA GUARDAR RESPUESTAS DE PENDIENTES DE REUNIONES
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <param name="idper"></param>
    ''' <param name="estrep"></param>
    ''' <param name="obsrep"></param>
    ''' <returns></returns>
    Function guardar_respuesta_pendientes_correos(ByVal idper As Integer, ByVal idter As Integer, ByVal estrep As String, ByVal obsrep As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_respuestas_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.BigInt, 11, idper)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.BigInt, 11, idter)
            dbSQL.add_parameter(ParameterDirection.Input, "@est_per", SqlDbType.VarChar, 50, estrep)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_rep", SqlDbType.VarChar, 3000, obsrep)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 26/03/2020 METODO PARA GUARDAR INFORMACION DE TEST SALUD
    ''' </summary>
    ''' <param name="mov"></param>
    ''' <param name="pri"></param>
    ''' <param name="cua"></param>
    ''' <param name="seg"></param>
    ''' <param name="ter"></param>
    ''' <param name="cur"></param>
    ''' <returns></returns>
    Function guardar_test_salud(ByVal mov As String, ByVal pri As String, ByVal cua As String, ByVal seg As String, ByVal ter As String, ByVal cur As String,
                                ByVal qui As String, ByVal sex As String, ByVal sep As String, ByVal oct As String, ByVal nov As String, ByVal dec As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_test_salud"
            dbSQL.add_parameter(ParameterDirection.Input, "@mov_tes", SqlDbType.VarChar, 50, mov)
            dbSQL.add_parameter(ParameterDirection.Input, "@pri_tes", SqlDbType.VarChar, 10, pri)
            dbSQL.add_parameter(ParameterDirection.Input, "@cua_tes", SqlDbType.VarChar, 100, cua)
            dbSQL.add_parameter(ParameterDirection.Input, "@seg_tes", SqlDbType.VarChar, 10, seg)
            dbSQL.add_parameter(ParameterDirection.Input, "@ter_tes", SqlDbType.VarChar, 10, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@cur_tes", SqlDbType.VarChar, 10, cur)
            dbSQL.add_parameter(ParameterDirection.Input, "@qui_tes", SqlDbType.VarChar, 10, qui)
            dbSQL.add_parameter(ParameterDirection.Input, "@sex_tes", SqlDbType.VarChar, 10, sex)
            dbSQL.add_parameter(ParameterDirection.Input, "@sep_tes", SqlDbType.VarChar, 10, sep)
            dbSQL.add_parameter(ParameterDirection.Input, "@oct_tes", SqlDbType.VarChar, 10, oct)
            dbSQL.add_parameter(ParameterDirection.Input, "@nov_tes", SqlDbType.VarChar, 10, nov)
            dbSQL.add_parameter(ParameterDirection.Input, "@dec_tes", SqlDbType.VarChar, 10, dec)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' METODO PARA SELECCIONAR INFORMACION DE TEST SALUD
    ''' </summary>
    ''' <param name="ini"></param>
    ''' <param name="fin"></param>
    ''' <returns></returns>
    Function seleccionar_test_salud_conductores(ByVal ini As String, ByVal fin As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_test_salud_por_fecha"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, ini)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fin)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 13/04/2020 METODO PARA SELECCIONAR LAS RESPUESTAS DE PENDIENTES REUNION POR ID REUNION
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function seleccionar_respuestas_pendiente_reunion_terceros_por_id_pendiente(ByVal id As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_respuestas_pendiente_reunion_terceros_por_id_pendiente"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, id)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 16/04/2020 METODO PARA SELECCIONAR PENDIENTES POR TIPO DE REUNION
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Function seleccionar_pendientes_reuniones_por_tipo_reunion(ByVal id As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_pendientes_reuniones_por_tipo_reunion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_tir", SqlDbType.Int, 11, id)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 05/05/2020 METODO PARA ASIGNAR PARTICIPANTES A REUNIONES
    ''' </summary>
    ''' <param name="reu"></param>
    ''' <param name="ter"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_reuniones_participantes(ByVal reu As Integer, ByVal ter As Integer, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_reuniones_participantes"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, reu)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_rep", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 01/02/2021 METODO PARA SELECCIONAR PARTICIPANTES DE REUNION ANTERIOR
    ''' </summary>
    ''' <param name="reu"></param>
    ''' <returns></returns>
    Function capturar_participantes_reunion_anterior(ByVal reu As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_participantes_reunion_anterior"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_reu", SqlDbType.Int, 11, reu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 01/02/2021 METODO PARA ELIMINAR PARTICIPANTES REUNION
    ''' </summary>
    ''' <param name="rep"></param>
    ''' <returns></returns>
    Function eliminar_participante_reunion(ByVal rep As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_participante_reunion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_rep", SqlDbType.Int, 11, rep)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 02/03/2022 METODO PARA SELECCIONAR CORREOS PENDIENTES
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_enviar_correos_automaticos_reuniones_correo_amazon() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_enviar_correos_automaticos_reuniones_correo_amazon"

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 02/03/2022 METODO PARA SELECCIONAR CORREO PENDIENTE ACTIVO SI O NO
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <param name="correo"></param>
    ''' <returns></returns>
    Function seleccionar_correos_activos_pendientes_reuniones(ByVal idter As Integer, ByVal correo As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_correos_activos_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, idter)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_per", SqlDbType.VarChar, 200, correo)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception

        End Try

        Return dtreu

    End Function

    ''' <summary>
    ''' LGUF 02/03/2022 METODO PARA GUARDAR CORREOS REUNIONES
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <param name="correo"></param>
    ''' <returns></returns>
    Function guardar_correos_pendientes_reuniones(ByVal idter As Integer, ByVal correo As String) As DataTable
        Dim dbSQL As New databaseSQL
        Dim dtreu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_guardar_correos_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, idter)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_per", SqlDbType.VarChar, 200, correo)

            dbSQL.query_sql(dtreu)
        Catch ex As Exception
            dtreu = Nothing
        End Try

        Return dtreu
    End Function

    ''' <summary>
    ''' LGUF 03/03/2022 METODO PARA MARCAR CORREO CON ENVIO DE ACTIVACION
    ''' </summary>
    ''' <param name="idpec"></param>
    ''' <returns></returns>
    Function envio_correo_pendientes_reuniones(ByVal idpec As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_envio_correo_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pec", SqlDbType.Int, 11, idpec)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 03/03/2022 METODO PARA ACTIVAR CORREOS PENDIENTES REUNIONES
    ''' </summary>
    ''' <param name="idpec"></param>
    ''' <returns></returns>
    Function activar_correo_pendientes_reuniones(ByVal idpec As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_activar_correo_pendientes_reuniones"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pec", SqlDbType.Int, 11, idpec)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 2023-04-27 METODO PARA SELECCIONAR CLAVES EMPRESARIALES
    ''' </summary>
    ''' <param name="tip"></param>
    ''' <returns></returns>
    Function seleccionar_claves_empresariales(ByVal tip As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcla As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_claves_empresariales"
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_cla", SqlDbType.VarChar, 20, tip)

            dbSQL.query_sql(dtcla)
        Catch ex As Exception

        End Try

        Return dtcla

    End Function
End Class