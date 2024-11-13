Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class usuarios
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 06/12/2019 METODO PARA CAPTURAR DATOS DE USUARIO POR CEDULA
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function capturar_datos_usuarios_por_cedula(ByVal doc As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtusua As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_usua_por_cedula"
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_usua", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtusua)
        Catch ex As Exception

        End Try

        Return dtusua

    End Function

    ''' <summary>
    ''' LGUF 06/12/2019 METODO PARA INSERTAR SESSION EN EL MOMENTO DEL LOGUEO
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <param name="strsession"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function insertar_session_usuarios(ByVal idusu As Integer, ByVal strsession As String) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_insertar_usua_session"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_usua", SqlDbType.BigInt, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@session_usua", SqlDbType.VarChar, 100, strsession)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 06/12/2019 METODO PARA VALIDAR SI UN USUARIO ESTA LOGUEADO O NO
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <param name="strsession"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function validar_session_usuarios(ByVal idusu As Integer, ByVal strsession As String) As String
        Dim dbSQL As New databaseSQL
        Dim dtusua As New DataTable
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_validar_usua_session"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_usua", SqlDbType.BigInt, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@session_usua", SqlDbType.VarChar, 100, strsession)

            dbSQL.query_sql(dtusua)

            If dtusua.Rows.Count > 0 Then
                strRespuesta = ""
            Else
                strRespuesta = "Sessión inválida debe estar logueado."
            End If
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 10/01/2020 METODO PARA SELECCIONAR USUARIOS COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_usuarios_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtusu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_usuarios_completo"

            dbSQL.query_sql(dtusu)
        Catch ex As Exception

        End Try

        Return dtusu

    End Function

    ''' <summary>
    ''' LGUF 10/01/2020 METODO PARA GUARDAR USUARIOS
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <param name="idper"></param>
    ''' <param name="doc"></param>
    ''' <param name="nom"></param>
    ''' <param name="tel"></param>
    ''' <param name="cel"></param>
    ''' <param name="dir"></param>
    ''' <param name="cor"></param>
    ''' <param name="fecn"></param>
    ''' <param name="usu"></param>
    ''' <param name="con"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_usuarios(ByVal idusu As Integer, ByVal idper As Integer, ByVal doc As String, ByVal nom As String, ByVal tel As String, ByVal cel As String,
                              ByVal dir As String, ByVal cor As String, ByVal fecn As String, ByVal usu As String, ByVal con As String, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_usuarios"
            dbSQL.add_parameter(ParameterDirection.Input, "@idu_usu", SqlDbType.Int, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@idu_per", SqlDbType.Int, 11, idper)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_usu", SqlDbType.VarChar, 50, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_usu", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@tel_usu", SqlDbType.VarChar, 50, tel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cel_usu", SqlDbType.VarChar, 50, cel)
            dbSQL.add_parameter(ParameterDirection.Input, "@dir_usu", SqlDbType.VarChar, 200, dir)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_usu", SqlDbType.VarChar, 200, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_nac", SqlDbType.VarChar, 15, fecn)
            dbSQL.add_parameter(ParameterDirection.Input, "@usu_usu", SqlDbType.VarChar, 20, usu)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_usu", SqlDbType.VarChar, 20, con)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_usu", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 13/01/2020 CAPTURAR USUARIO POR ID
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <returns></returns>
    Function capturar_datos_usuarios_por_id(ByVal idusu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtusu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_usuarios_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, idusu)

            dbSQL.query_sql(dtusu)
        Catch ex As Exception

        End Try

        Return dtusu

    End Function

    ''' <summary>
    ''' LGUF 14/01/2020 METODO PARA ELIMINAR USUARIOS
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <returns></returns>
    Function eliminar_usuarios(ByVal idusu As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_usuarios"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, idusu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 14/01/2020 METODO PARA SELECCIONAR PERFILES COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_perfiles_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtusu As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_perfiles_completo"

            dbSQL.query_sql(dtusu)
        Catch ex As Exception

        End Try

        Return dtusu

    End Function

    ''' <summary>
    ''' LGUF METODO PARA CREAR PERFILES
    ''' </summary>
    ''' <param name="idper"></param>
    ''' <param name="nom"></param>
    ''' <param name="des"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_perfiles(ByVal idper As Integer, ByVal nom As String, ByVal des As String, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_perfiles"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, idper)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_per", SqlDbType.VarChar, 100, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@des_per", SqlDbType.VarChar, 200, des)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_per", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF METODO PARA SELECCIONAR PERFILES POR ID
    ''' </summary>
    ''' <param name="idper"></param>
    ''' <returns></returns>
    Function capturar_datos_perfiles_por_id(ByVal idper As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtper As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_perfiles_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, idper)

            dbSQL.query_sql(dtper)
        Catch ex As Exception

        End Try

        Return dtper

    End Function

    ''' <summary>
    ''' LGUF 14/01/2020 METODO PARA ELIMINAR PERFILES
    ''' </summary>
    ''' <param name="idper"></param>
    ''' <returns></returns>
    Function eliminar_perfiles(ByVal idper As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_perfiles"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, idper)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 02/04/2020 METODO PARA SELECCIONAR PERMISOS POR USUARIO
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <returns></returns>
    Function seleccionar_permisos_por_usuario(ByVal idusu As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtper As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_permisos_por_id_usua"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, idusu)

            dbSQL.query_sql(dtper)
        Catch ex As Exception

        End Try

        Return dtper

    End Function

    ''' <summary>
    ''' LGUF 02/04/2020 METODO PARA GUARDAR PERMISOS POR USUARIO
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <param name="idmod"></param>
    ''' <param name="usuin"></param>
    ''' <returns></returns>
    Function guardar_permisos(ByVal idusu As Integer, ByVal idmod As Integer, ByVal usuin As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_permisos"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_mod", SqlDbType.Int, 11, idmod)
            dbSQL.add_parameter(ParameterDirection.Input, "@usu_ing", SqlDbType.Int, 11, usuin)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 02/04/2020 METODO PARA ELIMINAR PERMISOS
    ''' </summary>
    ''' <param name="idper"></param>
    ''' <returns></returns>
    Function eliminar_permisos(ByVal idper As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_permisos"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_per", SqlDbType.Int, 11, idper)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 02/04/2020 METODO PARA VALIDAR SI EL USUARIO TIENE PERMISO
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <param name="idmodu"></param>
    ''' <returns></returns>
    Function validar_permiso_usuario(ByVal idmodu As Integer, ByVal idusu As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim dtusua As New DataTable
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_validar_permiso_usuario"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_usua", SqlDbType.BigInt, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@id_modu", SqlDbType.BigInt, 11, idmodu)

            dbSQL.query_sql(dtusua)

            If dtusua.Rows.Count > 0 Then
                strRespuesta = ""
            Else
                strRespuesta = "Permiso inválido. Debe tener permiso para poder acceder."
            End If
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 12/10/2021 METODO PARA SELECCIONAR SALARIO EMPRESA POR DOCUMENTO
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Function seleccionar_salario_empresa_por_documento(ByVal doc As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_salario_empresa_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_sae", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 18/11/2021 METODO PARA SELECCIONAR RETIROS EMPRESA POR DOCUMENTO
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Function seleccionar_retiro_empresa_por_documento(ByVal doc As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_retiro_empresa_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_ree", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 2023-02-14 METODO PARA ACTUALIZAR CONTRASEÑA Y FECHA VENCE EN USUARIOS
    ''' </summary>
    ''' <param name="idusu"></param>
    ''' <param name="conusu"></param>
    ''' <returns></returns>
    Function guardar_contraseña_usuarios(ByVal idusu As Integer, ByVal conusu As String) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_contraseña_usuarios"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_usua", SqlDbType.BigInt, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_usua", SqlDbType.VarChar, 20, conusu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 07/01/2020 METODO PARA GUARDAR TERCERSO
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <param name="tipodoc"></param>
    ''' <param name="doc"></param>
    ''' <param name="nom"></param>
    ''' <param name="dir"></param>
    ''' <param name="tel"></param>
    ''' <param name="cel"></param>
    ''' <param name="cor"></param>
    ''' <param name="act"></param>
    ''' <param name="tip"></param>
    ''' <returns></returns>
    Function guardar_terceros(ByVal idter As Integer, ByVal tipodoc As String, ByVal doc As String, ByVal dig As Integer, ByVal nom As String, ByVal no2 As String, ByVal ap1 As String,
                              ByVal ap2 As String, ByVal dir As String, ByVal tel As String, ByVal cel As String, ByVal cor As String, ByVal zon As Integer, ByVal emp As String,
                              ByVal age As String, ByVal are As Integer, ByVal car As Integer, ByVal act As Integer, ByVal tip As String) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_terceros"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, idter)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_doc", SqlDbType.VarChar, 100, tipodoc)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_ter", SqlDbType.VarChar, 50, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@dig_ter", SqlDbType.Int, 11, dig)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_ter", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@no2_ter", SqlDbType.VarChar, 200, no2)
            dbSQL.add_parameter(ParameterDirection.Input, "@ap1_ter", SqlDbType.VarChar, 200, ap1)
            dbSQL.add_parameter(ParameterDirection.Input, "@ap2_ter", SqlDbType.VarChar, 200, ap2)
            dbSQL.add_parameter(ParameterDirection.Input, "@dir_ter", SqlDbType.VarChar, 200, dir)
            dbSQL.add_parameter(ParameterDirection.Input, "@tel_ter", SqlDbType.VarChar, 50, tel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cel_ter", SqlDbType.VarChar, 50, cel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_ter", SqlDbType.VarChar, 100, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_zon", SqlDbType.Int, 11, zon)
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_ter", SqlDbType.VarChar, 100, emp)
            dbSQL.add_parameter(ParameterDirection.Input, "@age_ter", SqlDbType.VarChar, 100, age)
            dbSQL.add_parameter(ParameterDirection.Input, "@are_ter", SqlDbType.Int, 11, are)
            dbSQL.add_parameter(ParameterDirection.Input, "@car_ter", SqlDbType.Int, 11, car)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_ter", SqlDbType.Bit, 1, act)
            dbSQL.add_parameter(ParameterDirection.Input, "@tipo", SqlDbType.VarChar, 50, tip)
            'dbSQL.add_parameter(ParameterDirection.Input, "@img_ter", SqlDbType.VarBinary, 1, img)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function
End Class
