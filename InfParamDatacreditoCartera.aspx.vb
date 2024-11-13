Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports System.util
Imports System.Globalization
Imports System.Math

Partial Class InfParamDatacreditoCartera
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Dim intContMov As Integer

    Private Sub InfParamDatacreditoCartera_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(24, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        If txtFechaInicio.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fecha inválida...');", True)
        Else
            Dim strSQL As String
            Dim dtsab As New DataTable

            If Session("codEmpr") = "1" Or Session("codEmpr") = "3" Then
                strSQL = "Select ter.tipo_documentos_idtipo_documentos As 'TIPO DE IDENTIFICACION', "
                strSQL += "vc.generador_documento as 'NUMERO DE IDENTIFICACION', "
                strSQL += "vc.generador_nombre As 'NOMBRE COMPLETO', "
                strSQL += "vc.venta_numero As 'NUMERO DE LA CUENTA U OBLIGACION', "
                strSQL += "DATE_FORMAT(vc.venta_fecha,'%Y%m%d') as 'FECHA APERTURA', "
                strSQL += "DATE_FORMAT(vc.venta_vence,'%Y%m%d') as 'FECHA VENCIMIENTO', "
                strSQL += "'0' AS 'RESPONSABLE', "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 30 THEN '01' ELSE "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 30 And (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 60 THEN '06' ELSE "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 60 And (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 90 THEN '07' ELSE "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 90 And (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 120 THEN '08' ELSE "
                strSQL += "'09' END END END END AS 'NOVEDAD', "
                strSQL += "'0' AS 'SITUACION CARTERA', "
                strSQL += "vc.venta_total As 'VALOR INICIAL', "
                strSQL += "vc.venta_total As 'VALOR SALDO DEUDA', "
                strSQL += "'0' AS 'VALOR DISPONIBLE', "
                strSQL += "vc.venta_total As 'VALOR CUOTA MENSUAL', "
                strSQL += "(vc.venta_total - vc.venta_abonos) As 'VALOR SALDO MORA', "
                strSQL += "'1' AS 'TOTAL CUOTAS', "
                strSQL += "'0' AS 'CUOTAS CANCELADAS', "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 0 THEN 1 ELSE 0 END AS 'CUOTAS EN MORA', "
                strSQL += "DATE_FORMAT(vc.venta_vence,'%Y%m%d') as 'FECHA LIMITE DE PAGO', "
                strSQL += "DATE_FORMAT(vc.venta_vence,'%Y%m%d') as 'FECHA DE PAGO', "
                strSQL += "zo.zona as 'CIUDAD CORRESPONDENCIA', "
                strSQL += "REPLACE(vc.generador_direccion, ';', '') As 'DIRECCION DE CORRESPONDENCIA', "
                strSQL += "(SELECT CASE WHEN COALESCE(terceros_contactos.correo, '') = '' THEN 'juridica@mototransportar.com.co' ELSE terceros_contactos.correo END FROM terceros_contactos WHERE terceros_contactos.terceros_idterceros = ter.idterceros AND terceros_contactos.idel = 0 LIMIT 1) AS 'CORREO ELECTRONICO', "
                strSQL += "COALESCE(telefonos.telefono, '6445499') as 'CELULAR', "
                strSQL += "'0' AS 'SITUACION DEL TITULAR', "
                strSQL += "'000' AS 'EDAD DE MORA', "
                strSQL += "'0' AS 'FORMA DE PAGO', "
                strSQL += "SUBSTRING(SYSDATE() FROM 1 For 10) As 'FECHA SITUACION CARTERA', "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 30 THEN '01' ELSE "
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 30 And (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 60 THEN '06' ELSE " 
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 60 And (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 90 THEN '07' ELSE " 
                strSQL += "Case WHEN (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) > 90 And (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) <= 120 THEN '08' ELSE " 
                strSQL += "'09' END END END END AS 'ESTADO DE LA CUENTA', "
                strSQL += "SUBSTRING(SYSDATE() FROM 1 For 10) As 'FECHA ESTADO DE LA CUENTA', "
                strSQL += "'0' AS 'ADJETIVO', "
                strSQL += "'0' AS 'FECHA ADJETIVO', "
                strSQL += "'0' AS 'CLAUSULA DE PERMANENCIA', "
                strSQL += "'0' AS 'FECHA CLAUSULA DE PERMANENCIA' "
                strSQL += "From ventas_consolidado vc " 
                strSQL += "INNER Join terceros ter ON (vc.generador_terceroid=ter.idterceros) " 
                strSQL += "INNER Join zonas zo on(ter.zonas_idzonas=zo.idzonas) " 
                strSQL += "INNER Join generadores gen ON(vc.generador_id=gen.idgeneradores And gen.idel=0) " 
                strSQL += "Left Join generadores ON vc.generador_id = generadores.idgeneradores Left Join generadores_asesores gena ON(vc.asesor_id=gena.idgeneradores_asesores) " 
                strSQL += "Left Join usuarios us ON(us.idusuarios=gena.usuarios_idusuarios) "
                strSQL += "Left Join telefonos ON ter.idterceros = telefonos.terceros_idterceros And telefonos.idprincipal = 1 "
                strSQL += "WHERE vc.venta_id = (SELECT vd.ventas_idventas FROM ventas_detalles vd WHERE vc.venta_id=vd.ventas_idventas And vd.idel=0 LIMIT 1) And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd " 
                strSQL += "WHERE vrd.ventas_idventas = vc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)Is NULL) "
                strSQL += "And vc.venta_abonos < vc.venta_total And (vc.venta_total - vc.venta_abonos) > 10 And vc.venta_vence<=(SELECT( ADDDATE('2024-02-22',INTERVAL vc.venta_plazo DAY))) "
                strSQL += "ORDER BY 'NOMBRE COMPLETO', (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo DESC "

                dtsab = csinformes.ejecutar_query_bd(strSQL)
            End If

            If Session("codEmpr") = "6" Then
                strSQL = "Select tbl_terceros.nombre_terc As Generador, "
                strSQL += "tbl_terceros.documento_terc AS Documento, "
                strSQL += "tbl_terceros.dirppal_terc As DIRCTE, "
                strSQL += "tbl_terceros.telppal_terc AS TELCTE1, "
                strSQL += "tbl_terceros.telppal_terc As TELCTE2, "
                strSQL += "tbl_facturas.numero_fact AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_facturas.fecha_fact, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_facturas.fechavence_fact, 126) As FECMVT, "
		        strSQL += "DATEDIFF(DAY, tbl_facturas.fecha_fact, SYSDATETIME()) As DIAS, "
		        strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde As INT) As Valor, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Abono, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde - tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Saldo "
                strSQL += "From tbl_terceros_cxc "
                strSQL += "LEFT JOIN tbl_terceros_cxc_detalles ON tbl_terceros_cxc.id_tecc = tbl_terceros_cxc_detalles.id_tecc "
                strSQL += "LEFT JOIN tbl_facturas ON tbl_terceros_cxc_detalles.id_fact = tbl_facturas.id_fact "
                strSQL += "LEFT JOIN tbl_terceros ON tbl_terceros_cxc.id_terc = tbl_terceros.id_terc "
                strSQL += "WHERE tbl_terceros_cxc.estado_tecc = 'PENDIENTE' "
                strSQL += "And tbl_facturas.fecha_fact <= '" + txtFechaInicio.Value + "' "
                strSQL += "And (CAST(tbl_terceros_cxc_detalles.valor_ccde AS INT) "
                strSQL += "- (CASE WHEN (SELECT COUNT(tbl_pagos_cxc.id_pacc) FROM tbl_pagos_cxc WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_fact) > 0 THEN "
                strSQL += "(SELECT CAST(ISNULL(SUM(ISNULL(tbl_pagos_cxc.valor_pacc, 0)), 0) AS INT) FROM tbl_pagos_cxc "
                strSQL += "WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_fact And tbl_pagos_cxc.fechaemision_pacc <= '" + txtFechaInicio.Value + "' "
                strSQL += "GROUP BY tbl_pagos_cxc.id_fact) ELSE 0 END)) > 0 "
                strSQL += "UNION ALL "
                strSQL += "Select tbl_cartera_sistema_viejo.nombre_casv As Generador, "
                strSQL += "tbl_cartera_sistema_viejo.nit_casv AS Documento, "
                strSQL += "tbl_cartera_sistema_viejo.direccion_casv As DIRCTE, "
                strSQL += "tbl_cartera_sistema_viejo.telefono_casv AS TELCTE1, "
                strSQL += "tbl_cartera_sistema_viejo.telefono_casv As TELCTE2, "
                strSQL += "tbl_cartera_sistema_viejo.factura_casv AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_cartera_sistema_viejo.fecha_casv, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_cartera_sistema_viejo.fechav_casv, 126) As FECMVT, "
		        strSQL += "DATEDIFF(DAY, tbl_cartera_sistema_viejo.fecha_casv, SYSDATETIME()) As DIAS, "
		        strSQL += "CAST(tbl_cartera_sistema_viejo.valor_casv As INT) As Valor, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.abono_casv As INT) As Abono, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.valor_casv - tbl_cartera_sistema_viejo.abono_casv As INT) As Saldo "
                strSQL += "From tbl_cartera_sistema_viejo "
                strSQL += "Where (tbl_cartera_sistema_viejo.valor_casv - tbl_cartera_sistema_viejo.abono_casv) > 0 "
                strSQL += "And tbl_cartera_sistema_viejo.anulado_casv = 0 "
                strSQL += "And tbl_cartera_sistema_viejo.fecha_casv <= '" + txtFechaInicio.Value + "' "
                strSQL += "ORDER BY Generador, DIAS DESC "

                dtsab = csinformes.ejecutar_query_cartera_bd_sql(strSQL, Session("codEmpr"))
            End If

            If Session("codEmpr") = "4" Then
                strSQL = "Select tbl_terceros.nombre_terc As Generador, "
                strSQL += "tbl_terceros.documento_terc AS Documento, "
                strSQL += "tbl_terceros.dirppal_terc As DIRCTE, "
                strSQL += "tbl_terceros.telppal_terc AS TELCTE1, "
                strSQL += "tbl_terceros.telppal_terc As TELCTE2, "
                strSQL += "tbl_proformas.numero_prof AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_proformas.fecha_prof, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_proformas.fechavence_prof, 126) As FECMVT, "
                strSQL += "DATEDIFF(DAY, tbl_proformas.fecha_prof, SYSDATETIME()) As DIAS, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde As INT) As Valor, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Abono, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde - tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Saldo "
                strSQL += "From tbl_terceros_cxc "
                strSQL += "LEFT JOIN tbl_terceros_cxc_detalles ON tbl_terceros_cxc.id_tecc = tbl_terceros_cxc_detalles.id_tecc "
                strSQL += "LEFT JOIN tbl_proformas ON tbl_terceros_cxc_detalles.id_prof = tbl_proformas.id_prof "
                strSQL += "LEFT JOIN tbl_terceros ON tbl_terceros_cxc.id_terc = tbl_terceros.id_terc "
                strSQL += "WHERE tbl_terceros_cxc.estado_tecc = 'PENDIENTE' "
                strSQL += "And tbl_proformas.fecha_prof <= '" + txtFechaInicio.Value + "' "
                strSQL += "And (CAST(tbl_terceros_cxc_detalles.valor_ccde AS INT) "
                strSQL += "- (CASE WHEN (SELECT COUNT(tbl_pagos_cxc.id_pacc) FROM tbl_pagos_cxc WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_prof) > 0 THEN "
                strSQL += "(SELECT CAST(ISNULL(SUM(ISNULL(tbl_pagos_cxc.valor_pacc, 0)), 0) AS INT) FROM tbl_pagos_cxc "
                strSQL += "WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_prof And tbl_pagos_cxc.fechaemision_pacc <= '" + txtFechaInicio.Value + "' "
                strSQL += "GROUP BY tbl_pagos_cxc.id_fact) ELSE 0 END)) > 0 "
                strSQL += "UNION ALL "
                strSQL += "Select tbl_terceros.nombre_terc As Generador, "
                strSQL += "tbl_terceros.documento_terc AS Documento, "
                strSQL += "tbl_terceros.dirppal_terc As DIRCTE, "
                strSQL += "tbl_terceros.telppal_terc AS TELCTE1, "
                strSQL += "tbl_terceros.telppal_terc As TELCTE2, "
                strSQL += "tbl_facturas.numero_fact AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_facturas.fecha_fact, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_facturas.fechavence_fact, 126) As FECMVT, "
                strSQL += "DATEDIFF(DAY, tbl_facturas.fecha_fact, SYSDATETIME()) As DIAS, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde As INT) As Valor, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Abono, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde - tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Saldo "
                strSQL += "From tbl_terceros_cxc "
                strSQL += "LEFT JOIN tbl_terceros_cxc_detalles ON tbl_terceros_cxc.id_tecc = tbl_terceros_cxc_detalles.id_tecc "
                strSQL += "LEFT JOIN tbl_facturas ON tbl_terceros_cxc_detalles.id_fact = tbl_facturas.id_fact "
                strSQL += "LEFT JOIN tbl_terceros ON tbl_terceros_cxc.id_terc = tbl_terceros.id_terc "
                strSQL += "WHERE tbl_terceros_cxc.estado_tecc = 'PENDIENTE' "
                strSQL += "And tbl_facturas.fecha_fact <= '" + txtFechaInicio.Value + "' "
                strSQL += "And (CAST(tbl_terceros_cxc_detalles.valor_ccde AS INT) "
                strSQL += "- (CASE WHEN (SELECT COUNT(tbl_pagos_cxc.id_pacc) FROM tbl_pagos_cxc WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_fact) > 0 THEN "
                strSQL += "(SELECT CAST(ISNULL(SUM(ISNULL(tbl_pagos_cxc.valor_pacc, 0)), 0) AS INT) FROM tbl_pagos_cxc "
                strSQL += "WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_fact And tbl_pagos_cxc.fechaemision_pacc <= '" + txtFechaInicio.Value + "' "
                strSQL += "GROUP BY tbl_pagos_cxc.id_fact) ELSE 0 END)) > 0 "
                strSQL += "UNION ALL "
                strSQL += "Select tbl_cartera_sistema_viejo.nombre_casv As Generador, "
                strSQL += "tbl_cartera_sistema_viejo.nit_casv AS Documento, "
                strSQL += "tbl_cartera_sistema_viejo.direccion_casv As DIRCTE, "
                strSQL += "tbl_cartera_sistema_viejo.telefono_casv AS TELCTE1, "
                strSQL += "tbl_cartera_sistema_viejo.telefono_casv As TELCTE2, "
                strSQL += "tbl_cartera_sistema_viejo.factura_casv AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_cartera_sistema_viejo.fecha_casv, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_cartera_sistema_viejo.fechav_casv, 126) As FECMVT, "
                strSQL += "DATEDIFF(DAY, tbl_cartera_sistema_viejo.fecha_casv, SYSDATETIME()) As DIAS, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.valor_casv As INT) As Valor, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.abono_casv As INT) As Abono, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.valor_casv - tbl_cartera_sistema_viejo.abono_casv As INT) As Saldo "
                strSQL += "From tbl_cartera_sistema_viejo "
                strSQL += "Where (tbl_cartera_sistema_viejo.valor_casv - tbl_cartera_sistema_viejo.abono_casv) > 0 "
                strSQL += "And tbl_cartera_sistema_viejo.anulado_casv = 1 "
                strSQL += "And tbl_cartera_sistema_viejo.fecha_casv <= '" + txtFechaInicio.Value + "' "
                strSQL += "ORDER BY Generador, DIAS DESC "

                dtsab = csinformes.ejecutar_query_cartera_bd_sql(strSQL, Session("codEmpr"))
            End If

            If Session("codEmpr") = "7" Then
                strSQL = "Select tbl_terceros.nombre_terc As Generador, "
                strSQL += "tbl_terceros.documento_terc AS Documento, "
                strSQL += "tbl_terceros.dirppal_terc As DIRCTE, "
                strSQL += "tbl_terceros.telppal_terc AS TELCTE1, "
                strSQL += "tbl_terceros.telppal_terc As TELCTE2, "
                strSQL += "tbl_proformas.numero_prof AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_proformas.fecha_prof, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_proformas.fechavence_prof, 126) As FECMVT, "
                strSQL += "DATEDIFF(DAY, tbl_proformas.fecha_prof, SYSDATETIME()) As DIAS, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde As INT) As Valor, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Abono, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde - tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Saldo "
                strSQL += "From tbl_terceros_cxc "
                strSQL += "LEFT JOIN tbl_terceros_cxc_detalles ON tbl_terceros_cxc.id_tecc = tbl_terceros_cxc_detalles.id_tecc "
                strSQL += "LEFT JOIN tbl_proformas ON tbl_terceros_cxc_detalles.id_prof = tbl_proformas.id_prof "
                strSQL += "LEFT JOIN tbl_terceros ON tbl_terceros_cxc.id_terc = tbl_terceros.id_terc "
                strSQL += "WHERE tbl_terceros_cxc.estado_tecc = 'PENDIENTE' "
                strSQL += "And tbl_proformas.fecha_prof <= '" + txtFechaInicio.Value + "' "
                strSQL += "And (CAST(tbl_terceros_cxc_detalles.valor_ccde AS INT) "
                strSQL += "- (CASE WHEN (SELECT COUNT(tbl_pagos_cxc.id_pacc) FROM tbl_pagos_cxc WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_prof) > 0 THEN "
                strSQL += "(SELECT CAST(ISNULL(SUM(ISNULL(tbl_pagos_cxc.valor_pacc, 0)), 0) AS INT) FROM tbl_pagos_cxc "
                strSQL += "WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_prof And tbl_pagos_cxc.fechaemision_pacc <= '" + txtFechaInicio.Value + "' "
                strSQL += "GROUP BY tbl_pagos_cxc.id_fact) ELSE 0 END)) > 0 "
                strSQL += "UNION ALL "
                strSQL += "Select tbl_terceros.nombre_terc As Generador, "
                strSQL += "tbl_terceros.documento_terc AS Documento, "
                strSQL += "tbl_terceros.dirppal_terc As DIRCTE, "
                strSQL += "tbl_terceros.telppal_terc AS TELCTE1, "
                strSQL += "tbl_terceros.telppal_terc As TELCTE2, "
                strSQL += "tbl_facturas.numero_fact AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_facturas.fecha_fact, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_facturas.fechavence_fact, 126) As FECMVT, "
                strSQL += "DATEDIFF(DAY, tbl_facturas.fecha_fact, SYSDATETIME()) As DIAS, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde As INT) As Valor, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Abono, "
                strSQL += "CAST(tbl_terceros_cxc_detalles.valor_ccde - tbl_terceros_cxc_detalles.valorabono_ccde As INT) As Saldo "
                strSQL += "From tbl_terceros_cxc "
                strSQL += "LEFT JOIN tbl_terceros_cxc_detalles ON tbl_terceros_cxc.id_tecc = tbl_terceros_cxc_detalles.id_tecc "
                strSQL += "LEFT JOIN tbl_facturas ON tbl_terceros_cxc_detalles.id_fact = tbl_facturas.id_fact "
                strSQL += "LEFT JOIN tbl_terceros ON tbl_terceros_cxc.id_terc = tbl_terceros.id_terc "
                strSQL += "WHERE tbl_terceros_cxc.estado_tecc = 'PENDIENTE' "
                strSQL += "And tbl_facturas.fecha_fact <= '" + txtFechaInicio.Value + "' "
                strSQL += "And (CAST(tbl_terceros_cxc_detalles.valor_ccde AS INT) "
                strSQL += "- (CASE WHEN (SELECT COUNT(tbl_pagos_cxc.id_pacc) FROM tbl_pagos_cxc WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_fact) > 0 THEN "
                strSQL += "(SELECT CAST(ISNULL(SUM(ISNULL(tbl_pagos_cxc.valor_pacc, 0)), 0) AS INT) FROM tbl_pagos_cxc "
                strSQL += "WHERE tbl_pagos_cxc.id_fact = tbl_terceros_cxc_detalles.id_fact And tbl_pagos_cxc.fechaemision_pacc <= '" + txtFechaInicio.Value + "' "
                strSQL += "GROUP BY tbl_pagos_cxc.id_fact) ELSE 0 END)) > 0 "
                strSQL += "UNION ALL "
                strSQL += "Select tbl_cartera_sistema_viejo.nombre_casv As Generador, "
                strSQL += "tbl_cartera_sistema_viejo.nit_casv AS Documento, "
                strSQL += "tbl_cartera_sistema_viejo.direccion_casv As DIRCTE, "
                strSQL += "tbl_cartera_sistema_viejo.telefono_casv AS TELCTE1, "
                strSQL += "tbl_cartera_sistema_viejo.telefono_casv As TELCTE2, "
                strSQL += "tbl_cartera_sistema_viejo.factura_casv AS NUMDOC, "
                strSQL += "Convert(VARCHAR(15), tbl_cartera_sistema_viejo.fecha_casv, 126) As FEC, "
                strSQL += "Convert(VARCHAR(15), tbl_cartera_sistema_viejo.fechav_casv, 126) As FECMVT, "
                strSQL += "DATEDIFF(DAY, tbl_cartera_sistema_viejo.fecha_casv, SYSDATETIME()) As DIAS, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.valor_casv As INT) As Valor, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.abono_casv As INT) As Abono, "
                strSQL += "CAST(tbl_cartera_sistema_viejo.valor_casv - tbl_cartera_sistema_viejo.abono_casv As INT) As Saldo "
                strSQL += "From tbl_cartera_sistema_viejo "
                strSQL += "Where (tbl_cartera_sistema_viejo.valor_casv - tbl_cartera_sistema_viejo.abono_casv) > 0 "
                strSQL += "And tbl_cartera_sistema_viejo.anulado_casv = 1 "
                strSQL += "And tbl_cartera_sistema_viejo.fecha_casv <= '" + txtFechaInicio.Value + "' "
                strSQL += "ORDER BY Generador, DIAS DESC "

                dtsab = csinformes.ejecutar_query_cartera_bd_sql(strSQL, Session("codEmpr"))
            End If

            gridSabana.DataSource = dtsab
            gridSabana.DataBind()
        End If
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Dim name = "Sabana"

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        Dim page As New Page()
        Dim form As New HtmlForm()

        gridSabana.EnableViewState = False

        ' Deshabilitar la validación de eventos, sólo asp.net 2 
        page.EnableEventValidation = False

        ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        page.DesignerInitialize()

        page.Controls.Add(form)
        form.Controls.Add(gridSabana)

        page.RenderControl(htw)

        Response.Clear()
        Response.Buffer = True

        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".xls")
        Response.Charset = "UTF-8"


        Response.ContentEncoding = Encoding.[Default]
        Response.Write(sb.ToString())
        Response.[End]()
    End Sub
End Class
