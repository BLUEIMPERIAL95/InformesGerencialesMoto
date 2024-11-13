
function salvar_exitoso(url) {

    mensaje('success', 'Operación existosa', 'El Registro ha sido guardado exitosamente', 2, '', '');
    if (!url == "") {
        window.setTimeout(window.location.href = url, 3000);
    }

}

function operacion_exitosa(url) {

    mensaje('success', 'Operación existosa', 'El Registro ha sido sincronizado exitosamente', 3, '', '');
    if (!url == "") {
        window.setTimeout(window.parent.location.reload(), 3000);
    }

}

function operacion_exitosa_wms(msj) {

    mensaje('success', 'Operación existosa', msj.toString(), 4, '', '');
        window.setTimeout(window.parent.location.reload(), 3000);

}

function operacion_exitosa_prestador(url) {

    mensaje('success', 'Operación existosa', 'El Registro ha sido guardado exitosamente', 3, '', '');
    if (!url == "") {
        window.setTimeout(window.parent.location.reload(), 3000);
    }

}

function mvto_autorizado(url) {

    mensaje('warning', 'Validación Datos', 'El usuario no esta autorizado para ver este movimiento', 2, '', '');
    if (!url == "") {
        //alert(url);
        window.setTimeout(window.location.href = url, 3000);
    }

}

function mvto_manifiesto(url) {

    mensaje('warning', 'Validación Datos', 'El Manifiesto de Carga que intenta imprimir no se encuentra radicado en el RNDC. Si continúa, el proceso puede generar sanciones de ley a la Empresa', 6, '', '');
    if (!url == "") {
        //alert(url);
        window.setTimeout(window.location.href = url, 3000);
    }

}

function redir_mensaje(url) {

    mensaje('warning', 'Validación Datos', 'El Conductor tiene Cuentas por Cobrar (CxC) pendientes. <br>', 3, '', '');
    if (!url == "") {
        window.setTimeout(window.location.href = url, 3000);
    }

}
function redir_mensaje2(msj) {

    mensaje('warning', 'Validación Datos', msj.toString(), 0, '', '');


}
function mensaje_editar_placa(msj, url) {

   
    if (!url == "") {
        window.setTimeout(window.location.href = url, 3000);
        location.reload();
    }

}

function redir_mensaje1(url,msj) {

   alert(msj);
    if (!url == "") {
        window.setTimeout(window.location.href = url, 3000);
    }

}
function redir_mensaje3(url, msj) {
    var act = msj.split(",");
    var j=0;
    while (j < act.length)
    {
        alert("El " + act[j] + " Tiene CXC Pendientes");
        j = j + 1;   
    }
    
    if (!url == "") {
        window.setTimeout(window.location.href = url, 3000);
    }

}


function modalReports(data, cant, sRep) {

    var str = sRep.split(";");

    var webs = new Array();
    var vent = new Array();
    var reps = new Array();

    for (var s = 0; s < str.length; s++) {
        //alert("Element " + i + " = " + str[s]); 
        reps[s] = str[s];
    }

    for (var w = 0; w < cant; w++) {
        webs[w] = "http://reports.systram.com.co/Informes.aspx" + data;
        vent[w] = "Doc" + w;
    }

    for (var i = 0; i < cant; i++) {
        ventana = vent[i];
        var web = webs[i];
        ventana = window.open(web, '', "width=600, height=450");
    }
}




function loadingmsg(){
    //var btdeshabilitar = document.getElementById(idboton);
    //btdeshabilitar.disabled = true;
    mensaje('loading', 'Realizando Operación...', 'La operación esta siendo procesada. Espera un momento.', 2400, '', '');
}

function URLDes() {
    document.getElementById('hidurl').value = document.URL;
}

function calcula_tiempo(que_boton)
{
    var retorno;
    var actual;
    var h_actual;
    
    switch(que_boton){
    
        case 'hora_arriba':
            actual = document.getElementById('txthora').value;
            h_actual = parseInt(actual);
            if (h_actual == 23){ 
                retorno='00';
            }else{
                h_actual = h_actual + parseInt(1); 
                retorno = h_actual;
            }
            break;
            
        case 'hora_abajo':
            actual = document.getElementById('txthora').value;
            h_actual = parseInt(actual);
            if (h_actual == 0){
                    retorno = '23';
            }else{
                h_actual = h_actual - parseInt(1);
                retorno = h_actual
            }
            break;
            
        case 'minuto_arriba':
            actual = document.getElementById('txtminuto').value;
            h_actual = parseInt(actual);
            if (h_actual == 59){ 
                retorno='00';
            }else{
                h_actual = h_actual + parseInt(1); 
                retorno = h_actual;
            }
            break;
            
        case 'minuto_abajo':
            actual = document.getElementById('txtminuto').value;
            h_actual = parseInt(actual);
            if (h_actual == 0){
                    retorno = '59';
            }else{
                h_actual = h_actual - parseInt(1);
                retorno = h_actual
            }
            break;
    }
    
    if (retorno > 0 && retorno < 10){
        retorno = '0' + retorno.toString();
    }

    if (que_boton=='hora_arriba' || que_boton=='hora_abajo'){
        document.getElementById('txthora').value = retorno;
    }else{
        document.getElementById('txtminuto').value = retorno;
    }

}

function modal(ancho,alto,izq,arriba,formulario)
{
    izq = (screen.width - ancho)/2;

    var caracteristicas = "left="+izq+",top="+arriba+",width="+ancho+",height="+alto+",fullscreen=no,location=no,menubar=no,personalbar=no,resizable=no,"+
                          "scrollbars=no,status=no,toolbar=no";
    ventana = window.open(formulario,"newWin",caracteristicas);
    ventana.focus();
    return false;
    
}

function showmodal(ancho,alto,izq,arriba,formulario)
{
    var variables;
    izq = (screen.width - ancho)/2;
    variables = window.showModalDialog(formulario,'Ventana', 'status:no;dialogTop:'+arriba+'; dialogLeft:'+izq+'; dialogWidth:'+ancho+';dialogHeight:'+alto+';help:no;center:yes;resizable:no;scroll:yes;status:no;');     
    return false;
}


//SIMULAR LA POPUP COMO UNA VENTANA MODAL
function hijamodal()
{
    if(window.opener.focus){
        window.opener.focus = false;
        self.focus();
        alert('Para volver a la ventana anterior debes cerrar esta.');
    }
}

//CERRAR UNA VENTANA
function cerrar()
{
    window.close();
}


//FUNCION QUE ABRE EL MENU PRINCIPAL COMO UNA VENTANA DE APARIENCIA MAXIMIZADA
var windows = [];
function pantallaCompleta(pagina) 
{
    var_win = 'child';
    windows[var_win] = window.open(pagina, 'CustomPopUp'
    , 'center=no,help=no,status=no,menubar=no, toolbar=no, fullscreen=yes, resizable=no,scrollbars=yes, titlebar=no, directories=no, location=no, top=0,left=0'
    + ',Height=' + screen.availHeight + ',Width=' + screen.availWidth);
}


function triggerCerrar(){   
    setInterval(function(){conectado();}, 5000);
}

function conectado() {
    document.getElementById('hidconectado').value = "0";    
    for(w in windows) {
        if(!windows[w].closed) {
            
            document.getElementById('hidconectado').value = "1";
            
            //alert(windows[w]);
            //windows[w].close();       
        }
    }
}

function deshabilitar_tecla(e)
{   
    tecla_codigo = (document.all) ? e.kyeCode : e.which;
    if (tecla_codigo == 8){
        location.href = '../Default.aspx?inicio=1';
    } else {
        return true;
    }
}

function isNumberKey(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 44)
        return false;
    return true;
}



function redirecciona(pagina)
{
    location.href = pagina;
}

function inicial()
{
    location.reload(true);
}

//permiso: ingreso, interno, sesion 
//ventana: tb, modal, normal
function mensaje(idmensaje,cabeza,texto,tiempo,permiso,ventana)
{   
    if (tiempo != null){
        showDialog(cabeza,texto,idmensaje,parseInt(tiempo));
    }else{
        showDialog(cabeza,texto,idmensaje);
    }
}

function modal1(ancho,alto,izq,arriba,formulario)
{
    izq = (screen.width - ancho)/2;

    var caracteristicas = "left="+izq+",top="+arriba+",width="+ancho+",height="+alto+",fullscreen=no,location=no,menubar=no,personalbar=no,resizable=no,"+
                          "scrollbars=no,status=no,toolbar=no";
    window.parent.blur();
    ventana = window.open(formulario,"newWin",caracteristicas);
    ventana.focus();
    return false;
    
}


//A la función minifoto se pasan: el nombre de la imagen, el ancho y alto real de la imagen 
//y el ancho y alto en el que se quiere poner la imagen. La función "encaja" la imagen en un recuadro 
//manteniendo las proporciones. 

//<body>
//<script>
//    minifoto("foto.jpg",399,291,90,80);
//</script>
//</body>

    function minifoto(foto,H,V,ancho,alto) {
        propH = ancho / H;
        propV = alto / V;
        if (propH>propV) {
            anchoF = H * propV;
            altoF = alto;
        }
        else {
            altoF = V * propH;
            anchoF = ancho;
        }
        de = (ancho - anchoF) / 2;
        su = (alto - altoF) /2;
        imagen="<div style='width:"+ancho+";height:"+alto+";border:0px outset'>";
        imagen+="<img src='"+foto+"' widht="+anchoF+" height="+altoF;
        imagen+=" style='position:relative; left:"+de+";top:"+su+"'>";
        imagen+="</div>";
        document.write(imagen);
    }


    function mini(foto, ancho, alto) {
        var H = getImgSize(foto, 1);
        var V = getImgSize(foto, 2);

        if (H == 0) { H = 100; }
        if (V == 0) {V = 100;}
        
        propH = ancho / H;
        propV = alto / V;
        if (propH > propV) {
            anchoF = H * propV;
            altoF = alto;
        }
        else {
            altoF = V * propH;
            anchoF = ancho;
        }
        de = (ancho - anchoF) / 2;
        su = (alto - altoF) / 2;
        //imagen = "<div style='width:" + ancho + ";height:" + alto + ";border:0px outset'>";
        var imagen = "<img src='" + foto + "' widht=" + anchoF + " height=" + altoF;
        imagen += " style='position:relative; left:" + de + ";top:" + su + "'>";
        //imagen += "</div>";
        //document.write(imagen);
        return imagen;
    }

    function getImgSize(imgSrc,w_h) {
        var newImg = new Image();
        newImg.src = imgSrc;
        var height = newImg.height;
        var width = newImg.width;
        if (w_h == "1") { return width; }
        if (w_h == "2") {return height;}
    }



//------------------------------------------------------------------------------------------------------------------
//RETORNO DE DATOS DE TODAS LAS FUNCIONES DE BUSQUEDA


//CONFIGURACIONES

function mayoresvalorespagados(idconc, codigo, idctac, ctacon, reqcen, idcenc, cencos, intern, extern, inddet) {

        document.getElementById('hidconcep').value = idconc;
        document.getElementById('txtcodigo').value = codigo;
        document.getElementById('hidctacon').value = idctac;
        document.getElementById('txtcuenta').value = ctacon;
        document.getElementById('hidreqcen').value = reqcen;
        document.getElementById('hidcencos').value = idcenc;
        document.getElementById('txtcencostos').value = cencos;
        document.getElementById('txtinterna').value = intern;
        document.getElementById('txtexterna').value = extern;
        document.getElementById('cmbdetalle').value = inddet;

        document.getElementById('txtestado').value = "modificar"

        document.getElementById('btnuevo').disabled = true;
        document.getElementById('btguardar').disabled = true;
        document.getElementById('btmodificar').disabled = false;
        document.getElementById('bteliminar').disabled = false;
        document.getElementById('btdeshacer').disabled = false;
        document.getElementById('btsalir').disabled = false;
        document.getElementById('btcuenta').disabled = true;
        document.getElementById('btcencostos').disabled = true;
    }

function mvpagado(id, dex) {

        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidmvpagado').value = id;
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmvpagado').value = dex;

        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvalormvpagado').focus();
        self.parent.tb_remove();

    }

function ncreditoconcepto(id, cod, din, dex, cta, cos, ref, rqr, vrid, vrcod, vrdin, vrdex, vrcta, vrcos, vrrqr) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(din).value = vrdin;
    self.parent.document.getElementById(dex).value = vrdex;
    self.parent.document.getElementById(cta).value = vrcta;
    self.parent.document.getElementById(cos).value = vrcos;
    self.parent.document.getElementById(rqr).value = vrrqr;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function conceptoncredito(idconc, codigo, idctac, ctacon, reqcen, idcenc, cencos, intern, extern, inddet) {

    document.getElementById('hidconcep').value = idconc;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidcencos').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('cmbdetalle').value = inddet;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

function conceptorecibo(idconc, codigo, idctac, ctacon, reqcen, idcenc, cencos, intern, extern, inddet, cuotas) {
    //alert(cuotas);
    document.getElementById('hidconcep').value = idconc;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidcencos').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('cmbdetalle').value = inddet;
    document.getElementById('txtcuotas').value = cuotas;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

function conceptoegreso(idconc, codigo, idctac, ctacon, reqcen, idcenc, cencos, intern, extern, inddet) {

    document.getElementById('hidconcep').value = idconc;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidcencos').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('cmbdetalle').value = inddet;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

function impuestos(idimpu, codigo, natu, idctac, ctacon, idcenc, cencos, intern, extern, porc, tope) {

    document.getElementById('hidconcep').value = idimpu;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('cmbnaturaleza').value = natu;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidcencos').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('txtporcentaje').value = porc;
    document.getElementById('txttope').value = tope;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

function descuentoegreso(iddesc, codigo, idctac, ctacon, reqcen, idcenc, cencos, intern, extern, inddet) {

    document.getElementById('hiddescue').value = iddesc;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidcencos').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('cmbdetalle').value = inddet;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

function descuentorecibo(iddesc, codigo, idctac, ctacon, reqcen, idcenc, cencos, intern, extern, inddet) {

    document.getElementById('hiddescue').value = iddesc;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidcencos').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('cmbdetalle').value = inddet;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

//function egresompago(id, med, doc, ope, val, des, idt, ter) {
function egresompago(id, med, doc, ope, val, des, idt, ter,cant_con,avis,con) {
    var msj="Stock de Consecutivos: " + cant_con.toString()
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidmediopago').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmediopago').value = med;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtconsecutivo').value = doc;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidoperacion').value = ope;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidctavale').value = val;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidctadesvale').value = des;

    if (ope == "2") {
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidbeneficiario').value = idt;
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtbeneficiario').value = ter;
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_btbeneficiario').disabled = true;
    } else {
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidbeneficiario').value = "";
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtbeneficiario').value = "";
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_btbeneficiario').disabled = false;
    }
    
    
    if (con.toString() == "1") {
        if (parseInt(cant_con) <= parseInt(avis)) {
            //mensaje('warning', 'Alerta', msj, 0, '', '');
            alert(msj);
        }
        else {
            msj = msj;
        }
    }
    self.parent.tb_remove();
}

function egresobeneficiario(id, med, doc, vrid, vrmed, vrdoc) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(med).value = vrmed;
    self.parent.document.getElementById(doc).value = vrdoc;

    self.parent.tb_remove();

}

function egresotercero(id, vrid, nom, vrnom, doc, vrdoc, con) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(nom).value = vrnom;
    self.parent.document.getElementById(doc).value = vrdoc;

    self.parent.document.getElementById(con).focus();
    self.parent.tb_remove();

}

function egresoconcepto(id, cod, din, dex, cta, cos, ref, rqr, vrid, vrcod, vrdin, vrdex, vrcta, vrcos, vrrqr) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(din).value = vrdin;
    self.parent.document.getElementById(dex).value = vrdex;
    self.parent.document.getElementById(cta).value = vrcta;
    self.parent.document.getElementById(cos).value = vrcos;
    self.parent.document.getElementById(rqr).value = vrrqr;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function egresodescuento(id, cod, din, dex, cta, cos, ref, rqr, vrid, vrcod, vrdin, vrdex, vrcta, vrcos, vrrqr) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(din).value = vrdin;
    self.parent.document.getElementById(dex).value = vrdex;
    self.parent.document.getElementById(cta).value = vrcta;
    self.parent.document.getElementById(cos).value = vrcos;
    self.parent.document.getElementById(rqr).value = vrrqr;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function recibodescuento(id, cod, din, dex, cta, cos, ref, rqr, vrid, vrcod, vrdin, vrdex, vrcta, vrcos, vrrqr) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(din).value = vrdin;
    self.parent.document.getElementById(dex).value = vrdex;
    self.parent.document.getElementById(cta).value = vrcta;
    self.parent.document.getElementById(cos).value = vrcos;
    self.parent.document.getElementById(rqr).value = vrrqr;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function reciboconcepto(id, cod, din, dex, cta, cos, ref, rqr, vrid, vrcod, vrdin, vrdex, vrcta, vrcos, vrrqr) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(din).value = vrdin;
    self.parent.document.getElementById(dex).value = vrdex;
    self.parent.document.getElementById(cta).value = vrcta;
    self.parent.document.getElementById(cos).value = vrcos;
    self.parent.document.getElementById(rqr).value = vrrqr;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function reciboconcepto1(id, din) {

    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconcepto').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtliquidacioncxc').value = din;
    self.parent.tb_remove();

}

function recibobanco(id, ban) {

    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidbanco').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtorigen').value = ban;
    
    self.parent.tb_remove();

}


function compratercero(id, med, doc, vrid, vrmed, vrdoc) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(med).value = vrmed;
    self.parent.document.getElementById(doc).value = vrdoc;

    self.parent.tb_remove();

}

function mcontabletercero(id,vrid,nom,vrnom,doc,vrdoc,cta) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(nom).value = vrnom;
    self.parent.document.getElementById(doc).value = vrdoc;

    self.parent.document.getElementById(cta).focus();
    self.parent.tb_remove();

}

function mcontablecuentas(id,cod,des,mov,cen,bas,pbas,vrid,vrcod,vrdes,vrmov,vrcen,vrbas,vrpbas,ref) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(des).value = vrdes;
    self.parent.document.getElementById(mov).value = vrmov;
    self.parent.document.getElementById(cen).value = vrcen;
    self.parent.document.getElementById(bas).value = vrbas;
    self.parent.document.getElementById(pbas).value = vrpbas;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function mcontablecuentas1(id, cod, des, mov, cen, bas, pbas, vrid, vrcod, vrdes, vrmov, vrcen, vrbas, vrpbas, ref) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(des).value = vrdes;
    self.parent.document.getElementById(mov).value = vrmov;
    self.parent.document.getElementById(cen).value = vrcen;
    self.parent.document.getElementById(bas).value = vrbas;
    self.parent.document.getElementById(pbas).value = vrpbas;
    self.parent.document.getElementById(ref).value = vrdes;

    self.parent.document.getElementById(ref).focus();
    self.parent.tb_remove();

}

function mcontablecostos(id, vrid, cod, vrcod, des, vrdes, deb) {

    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(cod).value = vrcod;
    self.parent.document.getElementById(des).value = vrdes;

    self.parent.document.getElementById(deb).focus();
    self.parent.tb_remove();

}

function mcontablecomprobante(id, comp, cons) {

    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidcomprobante').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtcomprobante').value = comp;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnumero').value = cons;
    
    self.parent.tb_remove();

}

function configur(nombre,retorno,webretorno)
{

    if (retorno=='master'){
    
        if ((nombre == "2S") || (nombre == "3S")){
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidcarroceria').value = "";
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtcarroceria').value = "";
        } 
    
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtconfiguracion').value = nombre;
        window.parent.tb_remove();
    }else if(retorno=='form'){
        window.location.href = webretorno + "|" + nombre;
    }
    
    //window.opener.document.getElementById('txtconfiguracion').value = nombre;
    //cerrar();
}

function configur2(nombre,retorno,webretorno)
{
    if (retorno=='master'){
        
        if ((nombre == "2S") || (nombre == "3S")){
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidcarroceria').value = "";
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtcarroceria').value = "";
        } 
        
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtconfiguracion').value = nombre;
        window.parent.tb_remove();
    }else if(retorno=='form'){
        window.location.href = webretorno + "|" + nombre;
    }
}

function movimientovehiculoesp(idveh, placa, modelo, marca, linea, imagen, lncond, lnprop) {
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidvehiculo').value = idveh;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtplaca').value = placa;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelo').value = modelo;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmarca').value = marca;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtlinea').value = linea;
    var foto = self.parent.document.getElementById('ctl00_ContentPlaceHolder1_imvehiculo').src = imagen;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconductor').value = lncond;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidpropietarios').value = lnprop;

    //alert(lncond + "+" + lnprop);
	
   
    window.parent.tb_remove();
}


function movimientovehiculo(idveh, placa, modelo, marca, linea, imagen, lncond, lnprop,cond) {
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidvehiculo').value = idveh;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtplaca').value = placa;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelo').value = modelo;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmarca').value = marca;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtlinea').value = linea;
	self.parent.document.getElementById('ctl00_ContentPlaceHolder1_lnConductores').value = cond;
    var foto = self.parent.document.getElementById('ctl00_ContentPlaceHolder1_imvehiculo').src = imagen;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconductor').value = lncond;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidpropietarios').value = lnprop;
   console.log("ole"+lncond);
    //alert(lncond + "+" + lnprop);
	

	 window.parent.tb_remove();
}

function movimientoasignarvehiculo(idveh, placa, modelo, marca, linea, imagen, lncond, lnprop, cond, idprop, prop) {
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidvehiculo').value = idveh;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtplaca').value = placa;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelo').value = modelo;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmarca').value = marca;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtlinea').value = linea;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_lnConductores').value = cond;
    var foto = self.parent.document.getElementById('ctl00_ContentPlaceHolder1_imvehiculo').src = imagen;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconductor').value = lncond;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidpropietarios').value = lnprop;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidpropietario').value = idprop;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidpropservicio').value = idprop;

    if (prop !== "") {
        self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtprestadorser').value = prop;
    } 
    
    console.log("prueba" + lncond);
    //alert(lncond + "+" + lnprop);


    window.parent.tb_remove();
}

function agrupacionventas(id, agrupacion, conexo) {
    
    document.getElementById('hidagrupacion').value = id;
    document.getElementById('txtagrupacion').value = agrupacion;
    if (conexo == 'SI') {
        document.getElementById('chkConexo').checked = true;
    }else{
        document.getElementById('chkConexo').checked = false;
    }
    document.getElementById('hidestado').value = "modificar";
    
}


function ventasdescuentos(iddes, descr, refer, idcta, cuent, idcen, cecos) {
    
    document.getElementById('hiddescuento').value = iddes;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('txtreferencia').value = refer;
    document.getElementById('hidcuenta').value = idcta;
    document.getElementById('txtcuenta').value = cuent;
    document.getElementById('hidcencostos').value = idcen;
    document.getElementById('txtcencostos').value = cecos;
//    document.getElementById('txtPorcentaje').value = porc;

    document.getElementById('hidestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;

}

function tipospoliza(idtip, descr, tipo, idcta, cuent, valor) {

    document.getElementById('hiddescuento').value = idtip;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('cboEsporadico').value = tipo;
    document.getElementById('hidcuenta').value = idcta;
    document.getElementById('txtcuenta').value = cuent;
    document.getElementById('txtvalor').value = valor;
    //    document.getElementById('txtPorcentaje').value = porc;

    document.getElementById('hidestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
}

function conceptosdetalles(iddeta, idconc, idcuen, cuenta, reqmov, reqcen, reqbas, idcenc, cencos, descri, indval, valord, idterc, tercer){

    document.getElementById('hiddetall').value = iddeta;
    document.getElementById('hidconcep').value = idconc;
    document.getElementById('hidctacon').value = idcuen;
    document.getElementById('txtcuenta').value = cuenta;
    document.getElementById('hidreqmov').value = reqmov;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidreqbas').value = reqbas;
    document.getElementById('hidcencon').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;
    document.getElementById('txtinterna').value = descri;
    document.getElementById('cmbvalor').value = indval;
    document.getElementById('txtvalor').value = valord;
    document.getElementById('hidtercer').value = idterc;
    document.getElementById('txttercero').value = tercer;
    
    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;

}

function conceptosventas(idconc, codigo, idctac, ctacon, reqmov, reqcen, idcenc, cencos, refere, intern, extern, valorc, indiva, valiva, idctai, 
                            ctaiva, reqmiv, reqciv, idceni, cecosi, indriv, valriv, idctar, ctariv, reqmri, reqcri, idcenr, cecosr, inddet, retenc) {

    document.getElementById('hidconcep').value = idconc;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('hidctacon').value = idctac;
    document.getElementById('txtcuenta').value = ctacon;
    document.getElementById('hidrepmov').value = reqmov;
    document.getElementById('hidreqcen').value = reqcen;
    document.getElementById('hidcencon').value = idcenc;
    document.getElementById('txtcencostos').value = cencos;

    document.getElementById('txtreferencia').value = refere;
    document.getElementById('txtinterna').value = intern;
    document.getElementById('txtexterna').value = extern;
    document.getElementById('txtvalor').value = valorc;
    if (indiva == "1") {
        document.getElementById('chkindIVA').checked = true;
        document.getElementById('btctaIVA').disabled = true;
        document.getElementById('btcenIVA').disabled = true;
    } else {
        document.getElementById('chkindIVA').checked = false;
        document.getElementById('btctaIVA').disabled = false;
        document.getElementById('btcenIVA').disabled = false;
    }

    document.getElementById('txtIVA').value = valiva;
    document.getElementById('hidctaiva').value = idctai;
    document.getElementById('txtctaIVA').value = ctaiva;
    document.getElementById('hidreqivamov').value = reqmiv;
    document.getElementById('hidreqivacen').value = reqciv;
    
    document.getElementById('hidceniva').value = idceni;
    document.getElementById('txtcenIVA').value = cecosi;
    if (indriv == "1") {
        document.getElementById('chkrteiva').checked = true;
        document.getElementById('btctarteiva').disabled = true;
        document.getElementById('btcenrteiva').disabled = true;
    } else {
        document.getElementById('chkrteiva').checked = false;
        document.getElementById('btctarteiva').disabled = false;
        document.getElementById('btcenrteiva').disabled = false;
    }

    document.getElementById('txtrteiva').value = valriv;
    document.getElementById('hidctariv').value = idctar;
    document.getElementById('txtctarteiva').value = ctariv;
    document.getElementById('hidreqrivmov').value = reqmri;
    document.getElementById('hidreqrivcen').value = reqcri;
    document.getElementById('hidcenriv').value = idcenr;
    document.getElementById('txtcenrteiva').value = cecosr;
    document.getElementById('cmbdetalle').value = inddet;
    document.getElementById('txtRetencion').value = retenc;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('btcencostos').disabled = true;
}

function gestiontipos(id, cod, des, agen) {

    document.getElementById('hidtipo').value = id;
    document.getElementById('txtcodigo').value = cod;
    document.getElementById('txtdescripcion').value = des;
    document.getElementById('cmbagendar').value = agen;
    document.getElementById('txtestado').value = "modificar";

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}

function tipoprocesossuc(id, idpro, descr) {

    document.getElementById('hidcomppro').value = id;
    document.getElementById('cmbcomprobante').value = idpro;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('hidestado').value = "modificar";

}

function usuarioip(idrest, ipauto, descri) {

    document.getElementById('hidip').value = idrest;
    document.getElementById('txtdirip').value = ipauto;
    document.getElementById('txtdescripcion').value = descri;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function tipodescuentomov(id, inter, exter, idcta, cuenta, ind, vrent, vrtope, tdesp, tdesc) {

    document.getElementById('hidtipo').value = id;
    document.getElementById('txtinterna').value = inter;
    document.getElementById('txtexterna').value = exter;
    document.getElementById('hidcuenta').value = idcta;
    document.getElementById('txtcuenta').value = cuenta;
    document.getElementById('cmbindica').value = ind;
    document.getElementById('txtvalor').value = vrent;
    document.getElementById('txttope').value = vrtope;
    document.getElementById('cmbtipodepacho').value = tdesp;
    document.getElementById('cmbtipodesc').value = tdesc;

    document.getElementById('hidestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
}

//YCV
function tipodescuentomovzona(id, inter, exter, idcta, cuenta, ind, vrent, zona, vrtope, porc,idzona, centrob) {

    document.getElementById('hidtipo').value = id;
    document.getElementById('txtinterna').value = inter;
    document.getElementById('txtexterna').value = exter;
    document.getElementById('hidcuenta').value = idcta;
    document.getElementById('txtcuenta').value = cuenta;
    //document.getElementById('cmbindica').value = ind;
    document.getElementById('txtporcentaje').value = porc;
    document.getElementById('txttope').value = vrtope;
    document.getElementById('txtorigen').value = zona;
    document.getElementById('txtcentrobeneficio').value = centrob;

    document.getElementById('hidestado').value = "modificar"
    document.getElementById('hidzona').value = idzona;
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
}




function generadorgastoanexos(id, idtipo, tipo, idter, terce, ind, vrporc, vrent, tdesp) {

    document.getElementById('hidgasto_gen').value = id;
    document.getElementById('hidtipo').value = idtipo;
    document.getElementById('txttipo').value = tipo;
    document.getElementById('hidtercero').value = idter;
    document.getElementById('txttercero').value = terce;
    document.getElementById('cmbindica').value = ind;

    if (ind == "1") {
        document.getElementById('txtvalor').value = vrent;
    } else {
        document.getElementById('txtvalor').value = vrporc;
    }

    document.getElementById('cmbtipodepacho').value = tdesp;

    document.getElementById('hidestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;
    document.getElementById('bttipo').disabled = true;
    document.getElementById('bttercero').disabled = true;
}

function resoluciones_sucursal(idr, num, fec, tip, pre, ini, fin, avi) {

    document.getElementById('hidresolucion').value = idr;
    document.getElementById('txtnumero').value = num;
    document.getElementById('txtfecha').value = fec;
    document.getElementById('txttipo').value = tip;
    document.getElementById('txtprefijo').value = pre;
    document.getElementById('txtinicial').value = ini;
    document.getElementById('txtfinal').value = fin;
    document.getElementById('txtaviso').value = avi;
    
    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    
}

function cuentas_contables(id, cod, des, mov, cen, i_b, bas, est) {

    document.getElementById('hid').value = id;
    document.getElementById('txtcodigo').value = cod;
    document.getElementById('txtdescripcion').value = des;
    document.getElementById('cmbmovimiento').value = mov;
    document.getElementById('cmbccostos').value = cen;
    document.getElementById('cmbbase').value = i_b;
    document.getElementById('txtbase').value = bas;
    document.getElementById('cmbestado').value = est;

    document.getElementById('lblestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;

}

function centrodecostos(id, cod, des, est) {
    document.getElementById('hid').value = id;
    document.getElementById('txtcodigo').value = cod;
    document.getElementById('txtdescripcion').value = des;
    document.getElementById('cmbestado').value = est;

    document.getElementById('lblestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}

function tipoparametros(idpar, iddoc, iddes, idobl, limite) {

    document.getElementById('hidtipoparametro').value = idpar;
    document.getElementById('cmbdocumento').value = iddoc;
    document.getElementById('cmbtdespacho').value = iddes;
    document.getElementById('cmbparametro').value = idobl;
    document.getElementById('txtlimite').value = limite;
    document.getElementById('hidestado').value = "modificar"




    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;

}

function parametros_contables(idsuc, idpro, idpar, idcom, compr, idnat, iddes, idcta, cuent, idref, refer, idcen, cenco, cenau, inter, impre,
            ajind, ajidc, ajcom, ajdes, ajgen, secue, vtret, impcl) {

    document.getElementById('hidsucursal').value = idsuc;
    document.getElementById('hidproceso').value = idpro;
    document.getElementById('hidparametro').value = idpar;
    document.getElementById('hidcomprobante').value = idcom;
    document.getElementById('cmbnaturaleza').value = idnat;
    document.getElementById('cmbtdespacho').value = iddes;
    document.getElementById('hidcuenta').value = idcta;
    
    document.getElementById('txtcuenta').value = cuent;
    if (refer != "") {
        alert('1');
        document.getElementById('cmbreferencia').value = "2";
        document.getElementById('txtreferencia').disabled = false;
        document.getElementById('txtreferencia').value = refer;
    } else {
        document.getElementById('cmbreferencia').value = "1";
        document.getElementById('txtreferencia').disabled = true;
    }
    if (cenau == "1") {
        document.getElementById('hidcencostos').value = "";
        document.getElementById('txtcencostos').value = "";
        document.getElementById('btcencostos').disabled = true;
    } else {
        document.getElementById('hidcencostos').value = idcen;
        document.getElementById('txtcencostos').value = cenco;
        document.getElementById('btcencostos').disabled = false;
    }
    document.getElementById('cmbcencostos').value = cenau;
    document.getElementById('txtdescripcion').value = inter;
    document.getElementById('txtimpresion').value = impre;

    if ((idpro == "4") || (idpro == "7") || (idpro == "5")) { 
        if (ajind == "1") {
            document.getElementById('chajuste').checked = true;
            document.getElementById('hidcompajuste').value = ajidc;
            document.getElementById('txtcompajuste').value = ajcom;
            document.getElementById('txtdescajuste').value = ajdes;
            document.getElementById('cmbgenerar').value = ajgen;
            document.getElementById('btcompajuste').disabled = false;
        } else {
            document.getElementById('chajuste').checked = false;
            document.getElementById('hidcompajuste').value = '';
            document.getElementById('txtcompajuste').value = '';
            document.getElementById('txtdescajuste').value = '';
            document.getElementById('cmbgenerar').value = '0';
            document.getElementById('btcompajuste').disabled = true;        
        }
    }

    if (idpro == "5") {
        document.getElementById('cmbretencion').value = vtret;
        if (impcl == "1") {
            document.getElementById('chkquecampo').checked = true;
        } else {
            document.getElementById('chkquecampo').checked = false;
        }
    }

    document.getElementById('hidsecuencia').value = secue;
    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btcuenta').disabled = true;


}

function serviciodetalle(id,idorigen,origen,iddestino,destino,idproducto,producto,idempaque,empaque,naturaleza,
        idrem,remitente,iddes,destinatario,peso,unid,vrdec,vrman,dsori,dsdes,infor,infod) {
    
    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidorigen').value = idorigen;
    document.getElementById('txtorigen').value = origen;
    document.getElementById('hiddestino').value = iddestino;
    document.getElementById('txtdestino').value = destino;
    document.getElementById('hidproducto').value = idproducto;
    document.getElementById('txtproducto').value = producto;
    document.getElementById('hidempaque').value = idempaque;
    document.getElementById('txtempaque').value = empaque;
    document.getElementById('cmbnaturaleza').value = naturaleza;
    document.getElementById('hidremitente').value = idrem;
    document.getElementById('txtremitente').value = remitente;
    document.getElementById('hiddestinatario').value = iddes;
    document.getElementById('txtdestinatario').value = destinatario;
    document.getElementById('txtpesoinicial').value = peso;
    document.getElementById('txtunidadesinicial').value = unid;
    document.getElementById('txtvrdeclarado').value = vrdec;
    document.getElementById('txtvrmanejo').value = vrman;
    document.getElementById('txtporcmanejo').value = ((parseInt(vrman) * 100) / parseInt(vrdec));
    document.getElementById('txtdesorigen').value = dsori;
    document.getElementById('txtdesdestino').value = dsdes;
    document.getElementById('hidinforemitente').value = infor;
    document.getElementById('hidinfodestinatario').value = infod;

    document.getElementById('txtestado').value = "modificar"
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}
function serviciodetalle1(id, idorigen, origen, iddestino, destino, idproducto, producto, idempaque, empaque, naturaleza,
        idrem, remitente, iddes, destinatario, peso, unid, vrdec, vrman, dsori, dsdes, infor, infod,idzonarem,idzonades,
        claseproducto, fechacargue, horacargue, fechadescargue, horadescargue, idgenerador, generador, remcliente, factcliente,
        caj, met, anc, alt, lar, idguia, obser) {
    
    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidorigen').value = idorigen;
    document.getElementById('txtorigen').value = origen;
    document.getElementById('hiddestino').value = iddestino;
    document.getElementById('txtdestino').value = destino;
    document.getElementById('hidproducto').value = idproducto;
    document.getElementById('txtproducto').value = producto;
    document.getElementById('hidempaque').value = idempaque;
    document.getElementById('txtempaque').value = empaque;
    document.getElementById('cmbnaturaleza').value = naturaleza;
    document.getElementById('hidremitente').value = idrem;
    document.getElementById('txtremitente').value = remitente;
    document.getElementById('hiddestinatario').value = iddes;
    document.getElementById('txtdestinatario').value = destinatario;
    document.getElementById('txtpesoinicial').value = peso;
    document.getElementById('txtunidadesinicial').value = unid;
    document.getElementById('txtvrdeclarado').value = vrdec;
    document.getElementById('txtvrmanejo').value = vrman;
    document.getElementById('txtporcmanejo').value = ((parseInt(vrman) * 100) / parseInt(vrdec));
    document.getElementById('txtdesorigen').value = dsori;
    document.getElementById('txtdesdestino').value = dsdes;
    document.getElementById('hidinforemitente').value = infor;
    document.getElementById('hidinfodestinatario').value = infod;
    document.getElementById('hididzonarem').value = idzonarem;
    document.getElementById('hididzonades').value = idzonades;
    document.getElementById('txtGenerador').value = generador;
    document.getElementById('hidgeneradordetalle').value = idgenerador;
//    document.getElementById('hidgeneradordetalle').value = idasesor;
    document.getElementById('txtremcliente').value = remcliente;
    document.getElementById('txtfaccliente').value = factcliente;
    document.getElementById('cmbcajas').value = caj;
    document.getElementById('txtMetrosCubicos').value = met;
    document.getElementById('txtAncho').value = anc;
    document.getElementById('txtAlto').value = alt;
    document.getElementById('txtLargo').value = lar;
    document.getElementById('hidguia').value = idguia;
    document.getElementById('txtObservacion').value = obser;

    document.getElementById('txtestado').value = "modificar"
 
	//YCV
	document.getElementById('ddlClasesProducto').value = claseproducto;
	document.getElementById('fechaCarga').value =fechacargue+" "+horacargue;
	document.getElementById('fechaDescarga').value= fechadescargue+" "+horadescargue;
	
	document.getElementById('txtFechaCargaTemp').value =fechacargue+" "+horacargue;
	document.getElementById('txtFechaDescargaTemp').value= fechadescargue+" "+horadescargue;
	
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
	document.getElementById('btnmodvrdecl').disabled = false;

}

function serviciodetalle1_New_Model(id, idorigen, origen, iddestino, destino, idproducto, producto, idempaque, empaque, naturaleza,
    idrem, remitente, iddes, destinatario, peso, unid, vrdec, vrman, dsori, dsdes, infor, infod, idzonarem, idzonades,
    claseproducto, fechacargue, horacargue, fechadescargue, horadescargue, idgenerador, generador, idasesor, asesor, mfle, mpag) {
    //alert(mfle + " - " + mpag);
    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidorigen').value = idorigen;
    document.getElementById('txtorigen').value = origen;
    document.getElementById('hiddestino').value = iddestino;
    document.getElementById('txtdestino').value = destino;
    document.getElementById('hidproducto').value = idproducto;
    document.getElementById('txtproducto').value = producto;
    document.getElementById('hidempaque').value = idempaque;
    document.getElementById('txtempaque').value = empaque;
    document.getElementById('cmbnaturaleza').value = naturaleza;
    document.getElementById('hidremitente').value = idrem;
    document.getElementById('txtremitente').value = remitente;
    document.getElementById('hiddestinatario').value = iddes;
    document.getElementById('txtdestinatario').value = destinatario;
    document.getElementById('txtpesoinicial').value = peso;
    document.getElementById('txtunidadesinicial').value = unid;
    document.getElementById('txtvrdeclarado').value = vrdec;
    document.getElementById('txtvrmanejo').value = vrman;
    document.getElementById('txtporcmanejo').value = ((parseInt(vrman) * 100) / parseInt(vrdec));
    document.getElementById('txtdesorigen').value = dsori;
    document.getElementById('txtdesdestino').value = dsdes;
    document.getElementById('hidinforemitente').value = infor;
    document.getElementById('hidinfodestinatario').value = infod;
    document.getElementById('hididzonarem').value = idzonarem;
    document.getElementById('hididzonades').value = idzonades;
    //document.getElementById('txtGenerador').value = generador;
    //document.getElementById('hidgeneradordetalle').value = idgenerador;
    document.getElementById('txtasesor').value = asesor;
    document.getElementById('hidasesor').value = idasesor;
    document.getElementById('cmbmodflete').value = mfle;
    document.getElementById('cmbmodpago').value = mpag;
    //document.getElementById('txtremcliente').value = remcliente;
    //document.getElementById('txtfaccliente').value = factcliente;

    document.getElementById('txtestado').value = "modificar"

    //YCV
    document.getElementById('ddlClasesProducto').value = claseproducto;
    document.getElementById('fechaCarga').value = fechacargue + " " + horacargue;
    document.getElementById('fechaDescarga').value = fechadescargue + " " + horadescargue;

    document.getElementById('txtFechaCargaTemp').value = fechacargue + " " + horacargue;
    document.getElementById('txtFechaDescargaTemp').value = fechadescargue + " " + horadescargue;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;


}

function movimientodetdetalle(id, idorigen, origen, iddestino, destino, idproducto, producto, idempaque, empaque, naturaleza,
        idrem, remitente, iddes, destinatario, peso, unid, vrdec, vrman, dsori, dsdes, infor, infod, ctrem, ctfac) {

    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidorigen').value = idorigen;
    document.getElementById('txtorigen').value = origen;
    document.getElementById('hiddestino').value = iddestino;
    document.getElementById('txtdestino').value = destino;
    document.getElementById('hidproducto').value = idproducto;
    document.getElementById('txtproducto').value = producto;
    document.getElementById('hidempaque').value = idempaque;
    document.getElementById('txtempaque').value = empaque;
    document.getElementById('cmbnaturaleza').value = naturaleza;
    document.getElementById('hidremitente').value = idrem;
    document.getElementById('txtremitente').value = remitente;
    document.getElementById('hiddestinatario').value = iddes;
    document.getElementById('txtdestinatario').value = destinatario;
    document.getElementById('txtpesoinicial').value = peso;
    document.getElementById('txtunidadesinicial').value = unid;
    document.getElementById('txtvrdeclarado').value = vrdec;
    document.getElementById('txtvrmanejo').value = vrman;
    document.getElementById('txtporcmanejo').value = ((parseInt(vrman) * 100) / parseInt(vrdec));
    document.getElementById('txtdesorigen').value = dsori;
    document.getElementById('txtdesdestino').value = dsdes;
    document.getElementById('hidinforemitente').value = infor;
    document.getElementById('hidinfodestinatario').value = infod;
    document.getElementById('txtremcliente').value = ctrem;
    document.getElementById('txtfaccliente').value = ctfac;

    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}
function movimientodetdetalle1(id, idorigen, origen, iddestino, destino, idproducto, producto, idempaque, empaque, naturaleza,
        idrem, remitente, iddes, destinatario, peso, unid, vrdec, vrman, dsori, dsdes, infor, infod, ctrem, ctfac, idzonarem, idzonades,
		claseproducto, fechacargue, horacargue, fechadescargue, horadescargue, porcm) {

    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidorigen').value = idorigen;
    document.getElementById('txtorigen').value = origen;
    document.getElementById('hiddestino').value = iddestino;
    document.getElementById('txtdestino').value = destino;
    document.getElementById('hidproducto').value = idproducto;
    document.getElementById('txtproducto').value = producto;
    document.getElementById('hidempaque').value = idempaque;
    document.getElementById('txtempaque').value = empaque;
    document.getElementById('cmbnaturaleza').value = naturaleza;
    document.getElementById('hidremitente').value = idrem;
    document.getElementById('txtremitente').value = remitente;
    document.getElementById('hiddestinatario').value = iddes;
    document.getElementById('txtdestinatario').value = destinatario;
    document.getElementById('txtpesoinicial').value = peso;
    document.getElementById('txtunidadesinicial').value = unid;
    document.getElementById('txtvrdeclarado').value = vrdec;
    document.getElementById('txtvrmanejo').value = vrman;
    document.getElementById('txtporcmanejo').value = porcm;
    document.getElementById('txtdesorigen').value = dsori;
    document.getElementById('txtdesdestino').value = dsdes;
    document.getElementById('hidinforemitente').value = infor;
    document.getElementById('hidinfodestinatario').value = infod;
    document.getElementById('txtremcliente').value = ctrem;
    document.getElementById('txtfaccliente').value = ctfac;

    document.getElementById('txtestado').value = "modificar"
    document.getElementById('hididzonarem').value = idzonarem;
    document.getElementById('hididzonades').value = idzonades;

	
	//YCV
	document.getElementById('ddlClasesProducto').value = claseproducto;
	document.getElementById('fechaCarga').value =fechacargue+" "+horacargue;
	document.getElementById('fechaDescarga').value= fechadescargue+" "+horadescargue;
	
	document.getElementById('txtFechaCargaTemp').value =fechacargue+" "+horacargue;
	document.getElementById('txtFechaDescargaTemp').value= fechadescargue+" "+horadescargue;
		
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btnmodrem').disabled = false;
    document.getElementById('btnmodfac').disabled = false;

    //document.getElementById('txtremcliente').disabled = false;
    //document.getElementById('txtfaccliente').disabled = false;

}

//function enturnamientodetalle(id, idgen, gener, izoni, zonai, obser, pesoa) {
//    var a = document.getElementById("foo");
//    a.style.display = 'block';

//    document.getElementById('ctl00_ContentPlaceHolder1_hiddetalle').value = id;
//    document.getElementById('ctl00_ContentPlaceHolder1_hidgenerador').value = idgen;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtgenerador').value = gener;
//    document.getElementById('ctl00_ContentPlaceHolder1_hidzonas').value = izoni;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtpoblacioncompleta').value = zonai;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtpesoapr').value = pesoa;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtobservacion').value = obser;
//        
//    document.getElementById('ctl00_ContentPlaceHolder1_estadodet').value = "modificar"

//}

function enturnamientodetalle(id, idgen, gener, izoni, zonai, obser, pesoa) {

    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidgenerador').value = idgen;
    document.getElementById('txtgenerador').value = gener;
    document.getElementById('hidzonas').value = izoni;
    document.getElementById('txtpoblacioncompleta').value = zonai;
    document.getElementById('txtpesoapr').value = pesoa;
    document.getElementById('txtobservacion').value = obser;
        
    document.getElementById('txtestado').value = "modificar"

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}

//function cotizaciondetalle(id, idorigen, origen, iddestino, destino, ruta, producto, idempaque, empaque, idclase, clase, idcarroceria, carroceria, modflete, modpago, peso, unid, vrton, vrtot) {
//    var a = document.getElementById("foo");
//    a.style.display = 'block';

//    document.getElementById('ctl00_ContentPlaceHolder1_hiddetalle').value = id;
//    document.getElementById('ctl00_ContentPlaceHolder1_hidorigen').value = idorigen;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtorigen').value = origen;
//    document.getElementById('ctl00_ContentPlaceHolder1_hiddestino').value = iddestino;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtdestino').value = destino;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtruta').value = ruta;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtproducto').value = producto;
//    
//    document.getElementById('ctl00_ContentPlaceHolder1_hidempaque').value = idempaque;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtempaque').value = empaque;
//    document.getElementById('ctl00_ContentPlaceHolder1_hidclase').value = idclase;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtclase').value = clase;
//    document.getElementById('ctl00_ContentPlaceHolder1_hidcarroceria').value = idcarroceria;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtcarroceria').value = carroceria;
//    
//    document.getElementById('ctl00_ContentPlaceHolder1_cmbmodflete').value = modflete;
//    document.getElementById('ctl00_ContentPlaceHolder1_cmbmodpago').value = modpago;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtpeso').value = peso;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtunidades').value = unid;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtvrtonelada').value = vrton;
//    document.getElementById('ctl00_ContentPlaceHolder1_txtvrtotal').value = vrtot;

//    document.getElementById('ctl00_ContentPlaceHolder1_estadodetalle').value = "modificar"

//}

function cotizaciondetalle(id, idorigen, origen, iddestino, destino, ruta, producto, idempaque, empaque, idclase, clase, idcarroceria, carroceria, modflete, 
            modpago, peso, unid, vrton, vrtot,tdesp, dsori, dsdes) {

    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidorigen').value = idorigen;
    document.getElementById('txtorigen').value = origen;
    document.getElementById('hiddestino').value = iddestino;
    document.getElementById('txtdestino').value = destino;
    document.getElementById('txtruta').value = ruta;
    document.getElementById('txtproducto').value = producto;
    
    document.getElementById('hidempaque').value = idempaque;
    document.getElementById('txtempaque').value = empaque;
    document.getElementById('hidclase').value = idclase;
    document.getElementById('txtclase').value = clase;
    document.getElementById('hidcarroceria').value = idcarroceria;
    document.getElementById('txtcarroceria').value = carroceria;
    
    document.getElementById('cmbmodflete').value = modflete;
    document.getElementById('cmbmodpago').value = modpago;
    document.getElementById('txtpeso').value = peso;
    document.getElementById('txtunidades').value = unid;
    document.getElementById('txtvrtonelada').value = vrton;
    document.getElementById('txtvrtotal').value = vrtot;
    document.getElementById('cmbtipodepacho').value = tdesp;
    document.getElementById('txtdesorigen').value = dsori;
    document.getElementById('txtdesdestino').value = dsdes;

    document.getElementById('txtestado').value = "modificar"
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;


}


function retorna_valores_mtodetalle(vrdeclarado,vrmanejo) {

    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrdeclarado').value = vrdeclarado;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrmanejo').value = vrmanejo;
    format(self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrdeclarado'));
    format(self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrmanejo'));
    self.parent.tb_remove();

}

function retorna_valores_propietarioscxc(idprop,documento,nom1, nom2, ape1, ape2, dig) {

    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidpropietario').value = idprop;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdocumento').value = documento;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre1').value = nom1;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre2').value = nom2;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido').value = ape1;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido2').value = ape2;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdigito').value = dig;
    self.parent.tb_remove();

}

function retorna_valores_cotdetalle(vrdeclarado, vrmanejo) {

    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';

    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtotal').value = vrdeclarado;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtonelada').value = vrmanejo;
    format(self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtotal'));
    format(self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtonelada'));
    self.parent.tb_remove();

}

function retorna_valores_fletes_mto(femp, fter, temp, tter) {
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtfleteempresa').value = femp;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtfletetercero').value = fter;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtonempresa').value = temp;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtontercero').value = tter;
    self.parent.tb_remove();

}

function retorna_valores_tablafletes(id, iori, orig, ides, dest, peso, femp, fter, fmod) {
    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidflete').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidorigen').value = iori;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hiddestino').value = orig;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtorigen').value = ides;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdestino').value = dest;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtpesoinicial').value = peso;
//    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtfleteempresa').value = femp;
//    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtfletetercero').value = fter;
    self.parent.tb_remove();
}

function retorna_valores_tablafletes_detalle(vatonemp, vantonter) {
   // var a = self.parent.document.getElementById("foo");
  //  a.style.display = 'block';
    
    /*self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidflete').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidorigen').value = iori;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hiddestino').value = orig;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtorigen').value = ides;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdestino').value = dest;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtpesoinicial').value = peso;*/
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtonempresa').value = vatonemp;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtvrtontercero').value = vantonter;
    self.parent.tb_remove();
}






function enturnamientodetallelimpiar() {
    var a = document.getElementById("foo");
    a.style.display = 'block';

    document.getElementById('ctl00_ContentPlaceHolder1_txtpesoapr').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtgenerador').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtpoblacioncompleta').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hidgenerador').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hidzonas').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtobservacion').value = "";
    
    document.getElementById('ctl00_ContentPlaceHolder1_estadodet').value = "nuevo";
    
}


function retorna_enturnamiento_filtros(idcon,doc,nom1,nom2,ape1,ape2,idveh,pla,mode,mrep,conf,peso,capa)
{
    //alert(idveh);
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconveh').value = idcon;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdocumento').value = doc;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre1').value = nom1;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre2').value = nom2;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido').value = ape1;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido2').value = ape2;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidvehiculo').value = idveh;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtplaca').value = pla;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelo').value = mode;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelorep').value = mrep;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtconfiguracion').value = conf;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtpeso').value = peso;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtcapacidad').value = capa;

    self.parent.tb_remove();
    
}



function serviciodetallelimpiar() {
    var a = document.getElementById("foo");
    a.style.display = 'block';

    document.getElementById('ctl00_ContentPlaceHolder1_hiddetalle').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hidorigen').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtorigen').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hiddestino').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtdestino').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hidproducto').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtproducto').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hidempaque').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtempaque').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_cmbnaturaleza').value = "0";
    document.getElementById('ctl00_ContentPlaceHolder1_hidremitente').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtremitente').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_hiddestinatario').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtdestinatario').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtpesoinicial').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtunidadesinicial').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtvrdeclarado').value = "";
    document.getElementById('ctl00_ContentPlaceHolder1_txtvrmanejo').value = "";

    document.getElementById('ctl00_ContentPlaceHolder1_estadodetalle').value = "nuevo";
    
}

function adicionalcotizacion(idadicional,idtipo,tipo,valor,observ)
{
    document.getElementById('hidadicional').value = idadicional;
    document.getElementById('hidtipoadicional').value = idtipo;
    document.getElementById('txttipo').value = tipo;
    document.getElementById('txtvalor').value = valor;
    document.getElementById('txtnovedades').value = observ;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function movimientoliquidaciongastos(idliqgas,idgastos,descripc,valorgas,aplicara,idtercer,terceros,idliqdet,fechagas)
{
    document.getElementById('hidliqgasto').value = idliqgas;
    document.getElementById('hidgasto').value = idgastos;
    document.getElementById('txttipo').value = descripc;
    document.getElementById('txtvalor').value = valorgas;
    document.getElementById('cmbaplicar').value = aplicara;
    document.getElementById('hidtercero').value = idtercer;
    document.getElementById('txttercero').value = terceros;
    document.getElementById('txtfecha').value = fechagas;
    document.getElementById('lblestado').value = 'modificar';

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function anexoempresa(idanexo, idtipo, tipo, valore, valort, observ, estcau, estfac, tipodoc, referencia) {
    document.getElementById('hidanexo').value = idanexo;
    document.getElementById('hidtipo').value = idtipo;
    document.getElementById('txttipo').value = tipo;
    document.getElementById('txtvalorempresa').value = valore;
    document.getElementById('txtvalortercero').value = valort;
    document.getElementById('txtobservacion').value = observ;
    document.getElementById('txtestado').value = 'modificar';
    document.getElementById('hidestcaucon').value = estcau;
    document.getElementById('hidestfaccon').value = estfac;
    document.getElementById('cbmtipodocumento').value = tipodoc;
    document.getElementById('txtdoccliente').value = referencia;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function gastosanexos(idanexo, idtipo, idter, terc, tipo, valor, observ, refer, idcentro, cencostos) {
    document.getElementById('hidanexo').value = idanexo;
    document.getElementById('hidtipo').value = idtipo;
    document.getElementById('txttipo').value = tipo;
    document.getElementById('txtvalor').value = valor;
    document.getElementById('hidtercero').value = idter;
    document.getElementById('txttercero').value = terc;
    document.getElementById('txtobservacion').value = observ;
    document.getElementById('txtreferencia').value = refer;
    document.getElementById('hidcencos').value = idcentro;
    document.getElementById('txtcencos').value = cencostos;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function descuentosmovimiento(iddes, idmov, idtip, tipod, valor, refer, obser, idcen, cecos) {
    document.getElementById('hiddescuento').value = iddes;
    document.getElementById('hidmovimiento').value = idmov;
    document.getElementById('hidtipo').value = idtip;
    document.getElementById('txttipo').value = tipod; 
    document.getElementById('txtvalor').value = valor;
    document.getElementById('txtreferencia').value = refer;
    document.getElementById('txtobservacion').value = obser;
    document.getElementById('hidcencostos').value = idcen;
    document.getElementById('txtcencostos').value = cecos;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

//function anexoempresa(idadicional,idtipo,tipo,valor,observ)
//{
//    document.getElementById('hidadicional').value = idadicional;
//    document.getElementById('hidtipoadicional').value = idtipo;
//    document.getElementById('txttipo').value = tipo;
//    document.getElementById('txtvalor').value = valor;
//    document.getElementById('txtobservaciones').value = observ;

//    document.getElementById('btnuevo').disabled = true;
//    document.getElementById('btguardar').disabled = true;
//    document.getElementById('btmodificar').disabled = false;
//    document.getElementById('bteliminar').disabled = false;
//    document.getElementById('btdeshacer').disabled = false;
//    document.getElementById('btsalir').disabled = false;
//}

function anexotercero(idadicional,idtipo,tipo,valor,observ)
{
    document.getElementById('hidadicional').value = idadicional;
    document.getElementById('hidtipoadicional').value = idtipo;
    document.getElementById('txttipo').value = tipo;
    document.getElementById('txtvalor').value = valor;
    document.getElementById('txtobservaciones').value = observ;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function tipoadicionales(id,descr,vemp,vter,idctaemp,ctaemp,idctater,ctater,idcen,cecos)
{
    document.getElementById('hidtipoadicional').value = id;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('txtvrempresa').value = vemp;
    document.getElementById('txtvrtercero').value = vter;
    document.getElementById('hidcuentaemp').value = idctaemp;
    document.getElementById('txtcuentaemp').value = ctaemp;
    document.getElementById('hidcuentater').value = idctater;
    document.getElementById('txtcuentater').value = ctater;
    document.getElementById('hidcencostos').value = idcen;
    document.getElementById('txtcencostos').value = cecos;
    //document.getElementById('cmbclasificacion').value = clasi;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function tipocumplidos(id, codigo, descr) {
    document.getElementById('hidtipocumplido').value = id;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('txtcodigo').value = codigo;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function servicioasesor(id,asesor)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidasesor").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtasesor").value = asesor;
    window.parent.tb_remove();
}

function movimiento_despachador(id, despachador) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hiddespachador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdespachador").value = despachador;
    window.parent.tb_remove();
}

function movimiento_conductoraux(idc, conductor) {	
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidconductoresaux").value = idc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtconductoraux").value = conductor;
    window.parent.tb_remove();
}


function servicio_cotizacion_retorno(idcotdetalle,modflete,modpago,tdespacho,estado,fleteemp,fleteter)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcotizacion").value = idcotdetalle;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbmodflete").value = modflete;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbmodpago").value = modpago;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbtipodepacho").value = tdespacho;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbestado").value = estado;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfleteempresa").value = fleteemp;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfletetercero").value = fleteter;

    /*self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcotizacion").value = idcot;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcotdetalle").value = iddetalle;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = tercero;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidclase").value = idclase;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtunidadesref").value = unid;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpesoref").value = peso;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfleterefempresa").value = valor;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidstring_retorno").value = stringdetalle;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbmodflete").selectedIndex = fletemod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbmodpago").value = pagomod;
        
    var generador = stringgenerador.split('$');
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = generador[0];
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = generador[1];
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre1").value = generador[2];
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre2").value = generador[3];
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido").value = generador[4];
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido2").value = generador[5];
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigito").value = generador[7];
    
    */
    
    window.parent.tb_remove();
}

function tipoadicional(id,descr,valor)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtipo").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidempresagps").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtempresagps").value = descr;
    window.parent.tb_remove();
}

function remitentes(id,nom,band)
{ 

    if(band=="rem"){
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidremitente").value = id;
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtremitente").value = nom;
        //window.opener.document.getElementById("hidremitente").value = id;
        //window.opener.document.getElementById("txtremitente").value = nom;
    }else if (band=="des"){
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hiddestinatario").value = id;
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdestinatario").value = nom;
        //window.opener.document.getElementById("hiddestinatario").value = id;
        //window.opener.document.getElementById("txtdestinatario").value = nom;
    }
    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    window.parent.tb_remove();
    //cerrar();
}

function clasesvehiculares(id,cod,descr,conf,capcar,capneta,master)
{
    if(master=="1"){
        window.opener.document.getElementById("hidclase").value = id;
        window.opener.document.getElementById("txtclase").value = descr;
        cerrar();
    } else if (master == "6") {
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidclase").value = id;
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtclase").value = descr;
        var a = self.parent.document.getElementById("foo");
        a.style.display = 'block';
        window.parent.tb_remove();
    }else{
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidclase").value = id;
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtclase").value = descr;
        window.parent.tb_remove();
    }
}

function cencostos(id, cen) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcencostos").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtcencostos").value = cen;
    window.parent.tb_remove();
}


function tipcomprobantes(id,cod,desc,cons,ipre,pref,act)
{
    document.getElementById('hidcomprobante').value = id;
    document.getElementById('txtcodigo').value = cod;
    document.getElementById('txtdescripcion').value = desc;
    
    if (cons == "1"){
        document.getElementById('chkconsecutivo').checked = true;
    }else{
        document.getElementById('chkconsecutivo').checked = false;
    }
    
    if (ipre == "1"){
        document.getElementById('chkprefijo').checked = true;
    }else{
        document.getElementById('chkprefijo').checked = false;
    } 
    
    document.getElementById('txtprefijo').value = pref;
    document.getElementById('cmbestado').value = act;
    document.getElementById('txtestado').value = 'modificar'; 
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
        
    document.getElementById('txtcodigo').focus();
}

function mediospago(id, desc, ref, icuen, cuen, ibanc, banco, cons, actu, ini, fin, aviso, mant, inter, exter, idcta, cuent, valorm, opera, idter, terce, porcentaje, trans_elec, cuenta, tipcuent) {
    document.getElementById('hidmedio').value = id;
    document.getElementById('txtdescripcion').value = desc;
    document.getElementById('txtreferencia').value = ref;
    document.getElementById('hidcuenta').value = icuen;
    document.getElementById('txtcuenta').value = cuen;
    document.getElementById('hidentidad').value = ibanc;
    document.getElementById('txtentidad').value = banco;
    document.getElementById('cmboperacion').value = opera;
    document.getElementById('hidtercero_tipo').value = idter;
    document.getElementById('txttercero').value = terce;
    var consec = document.getElementById('chconsecutivos');
    if (cons === '1') {
        consec.checked = true;
        consec.disabled = false;
        document.getElementById('txtactual').value = actu;
        document.getElementById('txtinicial').value = ini;
        document.getElementById('txtfinal').value = fin;
        document.getElementById('txtaviso').value = aviso;
    } else {
        consec.checked = false;
        consec.disabled = true;
        document.getElementById('txtactual').value = "";
        document.getElementById('txtinicial').value = "";
        document.getElementById('txtfinal').value = "";
        document.getElementById('txtaviso').value = "";
    }

    var manten = document.getElementById('chmantenimiento');
    //alert("prueba1");
    if (mant === '1') {
        manten.checked = true;
        manten.disabled = false;
        document.getElementById('txtinterna').value = inter;
        document.getElementById('txtexterna').value = exter;
        document.getElementById('hidcuentamant').value = idcta;
        document.getElementById('txtctamant').value = cuent;
        document.getElementById('txtvalor').value = valorm;
        document.getElementById('txtPorcentaje').value = porcentaje;
    } else {
        manten.checked = false;
        manten.disabled = true;
        document.getElementById('txtinterna').value = "";
        document.getElementById('txtexterna').value = "";
        document.getElementById('hidcuentamant').value = "";
        document.getElementById('txtctamant').value = "";
        document.getElementById('txtvalor').value = "";
        document.getElementById('txtPorcentaje').value = "";
    }

    document.getElementById('txtestado').value = 'modificar';

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

    var combo = document.getElementById('cmboperacion');
    var btter = document.getElementById('bttercero');
    //    if (combo.value == "1") {
    //        btter.disabled = true;
    //    } else {
    //        btter.disabled = false;
    //    }
    var transferencia_elect = document.getElementById('chktrans_electronica');
    var cuentatr = cuenta;
    var tipocuenta = tipcuent;
    if (trans_elec === '1') {
        transferencia_elect.checked = true;
        document.getElementById('txtnumcuenta').value = cuentatr;
        document.getElementById('cmbtipocuenta').value = tipocuenta;
    } else {
        transferencia_elect.checked = false;
    }
}





//FORM VEHICULOS TRAYLERS
function vehiculostraylers(id,idtray,plaq,idveh,est)
{
    document.getElementById('hid').value = id;
    document.getElementById('hidtrayler').value = idtray;
    document.getElementById('txtTrayler').value = plaq;
    document.getElementById('hidvehiculo').value = idveh;
    document.getElementById('cmbestado').value = est;

    document.getElementById('txtestado').value = 'modificar';

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

//ENTURNAMIENTO
function enturn(placa,mode,moderep,conf,peso,capac,cond,doc,apcon,apveh)
{
    document.getElementById('ctl00_ContentPlaceHolder1_l_placa').value = placa;
    document.getElementById('ctl00_ContentPlaceHolder1_l_modelo').value = mode;
    document.getElementById('ctl00_ContentPlaceHolder1_l_modelorep').value = moderep;
    document.getElementById('ctl00_ContentPlaceHolder1_l_configuracion').value = conf;
    document.getElementById('ctl00_ContentPlaceHolder1_l_peso').value = peso;
    document.getElementById('ctl00_ContentPlaceHolder1_l_capacidad').value = capac;
    document.getElementById('ctl00_ContentPlaceHolder1_l_conductor').value = cond;
    document.getElementById('ctl00_ContentPlaceHolder1_l_documento').value = doc;
    if(apcon=='0'){document.getElementById('ctl00_ContentPlaceHolder1_l_estcon').value='APROBADO';}else{document.getElementById('ctl00_ContentPlaceHolder1_l_estcon').value='SIN APROBAR';}
    if(apveh=='0'){document.getElementById('ctl00_ContentPlaceHolder1_l_estveh').value='APROBADO';}else{document.getElementById('ctl00_ContentPlaceHolder1_l_estveh').value='SIN APROBAR';}
}

function tipoadicionalesretorno(id,tipo,valor)
{ 
    window.opener.document.getElementById('hidtipoadicional').value = id;
    window.opener.document.getElementById('txttipo').value = tipo;
    window.opener.document.getElementById('txtvalor').value = valor;
    cerrar();
}

function gastosviaje(id,codigo,descripcion,idcta,cuenta,idter,tercer)
{ 
    document.getElementById('hidtipogastos').value = id;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('txtdescripcion').value = descripcion;
    document.getElementById('hidcuenta').value = idcta;
    document.getElementById('txtcuenta').value = cuenta;
    document.getElementById('hidtercero').value = idter;
    document.getElementById('txttercero').value = tercer;
    
    document.getElementById('lblestado').value = 'modificar';

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    
}

function tipogastosanexos(id,tipo,valor,idcta,cuenta,idcos,ccosto)
{ 
    document.getElementById('hidtipogastos').value = id;
    document.getElementById('txtdescripcion').value = tipo;
    document.getElementById('txtvr').value = valor;
    document.getElementById('hidcuenta').value = idcta;
    document.getElementById('txtcuenta').value = cuenta;
    document.getElementById('hidccosto').value = idcos;
    document.getElementById('txtcencostos').value = ccosto;
    
    document.getElementById('lblestado').value = 'modificar';

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    
}

function productosretorno(id,producto)
{ 
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidproducto').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtproducto').value = producto;
    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    window.parent.tb_remove();
}

function productosretornofletes(id,producto)
{    window.close();
	/*	window.opener.document.getElementById("hidclase").value = id;
        window.opener.document.getElementById("txtclase").value = producto;
	 console.log("paso");     
	 cerrar();*/

}



//TRAYLERS BUSCAR RETORNAR VALOR
function buscartrayler(id,placa)
{
    window.opener.document.getElementById('hidtrayler').value = id;
    window.opener.document.getElementById('txtTrayler').value = placa;
    cerrar();
}

//FORMULARIO CARROCERIAS
function carroceriastraylers(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcarroceria").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtcarroceria").value = descr;
    window.parent.tb_remove();
    /*window.opener.document.getElementById('hidcarroceria').value = cod;
    window.opener.document.getElementById('txtcarroceria').value = descr;
    cerrar();*/
}

//BUSQUEDA MARCAS TRAYLERS
function marcastraylers(cod,marca)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidmarca").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtmarca").value = marca;

    //window.opener.document.getElementById('hidmarca').value = cod;
    //window.opener.document.getElementById('txtmarca').value = marca;
    //cerrar();
    window.parent.tb_remove();
}


//BUSQUEDA CONDUCTORES
function conductorbus(id,nom,doc)
{
    
    window.opener.document.getElementById('hidconductor').value = id;
    window.opener.document.getElementById('txtconductor').value = nom;
    window.opener.document.getElementById('hiddocumento').value = doc;
    cerrar();
    //window.parent.tb_remove();
}

//BUSQUEDA PROPIETARIOS
function propietariobus(id,nom,doc)
{
    
    window.opener.document.getElementById('hidpropietario').value = id;
    window.opener.document.getElementById('txtpropietario').value = nom;
    window.opener.document.getElementById('hiddocumento').value = doc;
    cerrar();
    //window.parent.tb_remove();
}

//BUSQUEDA TRAYLERS
function traylers(id,placa,codmarca,marca,codcarroceria,carroceria,modelo,configuracion,peso,ruta)
{
    document.getElementById('hidtrayler').value = id;
    document.getElementById('txtplaqueta').value = placa;
    document.getElementById('hidmarca').value = codmarca;
    document.getElementById('txtmarca').value = marca;
    document.getElementById('hidcarroceria').value = codcarroceria;
    document.getElementById('txtcarroceria').value = carroceria;
    document.getElementById('txtmodelo').value = modelo;
    document.getElementById('txtconfiguracion').value = configuracion;
    document.getElementById('txtpeso').value = peso;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    
}

function conductorvehiculo(id,idcond,idveh,tipo,cond)
{
    document.getElementById('hid').value = id;
    document.getElementById('hidconductor').value = idcond;
    document.getElementById('hidvehiculo').value = idveh;
    document.getElementById('cmbtipocond').value = tipo;
    document.getElementById('txtconductor').value = cond;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function propietariovehiculo(id,idprop,prop,tipo,porc,idveh)
{
    document.getElementById('hid').value = id;
    document.getElementById('hidpropietario').value = idprop;
    document.getElementById('txtpropietario').value = prop;
    document.getElementById('cmbtipoten').value = tipo;
    document.getElementById('txtporctenencia').value = porc;
    document.getElementById('hidvehiculo').value = idveh;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function propietariotrayler(id,idprop,prop,tipo,porc,idtra)
{
    document.getElementById('hid').value = id;
    document.getElementById('hidpropietario').value = idprop;
    document.getElementById('txtpropietario').value = prop;
    document.getElementById('cmbtipoten').value = tipo;
    document.getElementById('txtporctenencia').value = porc;
    document.getElementById('hidtrayler').value = idtra;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

//FORMULARIO USUARIOSBUSQUEDA
function usuariosbusqueda(usuario)
{
    self.parent.document.getElementById('txtusuario').value = usuario;
    window.parent.tb_remove();
}


//FORMULARIO TRANSITOS
function transitos(id,cod,descr)
{
    window.opener.document.getElementById('hidtransito').value = id;
    window.opener.document.getElementById('txttransito').value = descr;
    cerrar();
}

//FORMULARIO TRANSITOS PARA VEHICULOS
function transitos1(id,cod,descr)
{
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidtransito').value = id;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txttransito').value = descr;
    window.parent.tb_remove();
}

function tiporeferencias(id,referencia)
{
    document.getElementById('hidref').value = id;
    document.getElementById('txtdescripcion').value = referencia;
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function tipodespachos(id,despacho,movimiento,cencosto,idcencosto,forma,flt_empresa,flt_tercero)
{
    alert(forma);
    document.getElementById('hiddes').value = id;
    document.getElementById('txtdescripcion').value = despacho;
    document.getElementById('cboMovimiento').value = movimiento;
    document.getElementById('txtcencostos').value = cencosto;
    document.getElementById('hidcencon').value = idcencosto;
    document.getElementById('cmbForma').value = forma;
    document.getElementById('txtfltempresa').value = flt_empresa;
    document.getElementById('txtflttercero').value = flt_tercero;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btbusc').disabled = false;
}

function tipocopias(id, codigo, descripcion) {
    document.getElementById('hidcopias').value = id;
    document.getElementById('txtcodigo').value = codigo;
    document.getElementById('txtdescripcion').value = descripcion;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function sistemaperiodos(id, anio, mes, descripcion, estado) {
    document.getElementById('hidperiodo').value = id;
    document.getElementById('cmbanio').value = anio;
    document.getElementById('cmbmes').value = mes;
    document.getElementById('txtdescripcion').value = descripcion;
    document.getElementById('cmbestado').value = estado;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function tipoescoltajes(id,descripcion,procedimiento)
{
    document.getElementById('hidtipo').value = id;
    document.getElementById('txtdescripcion').value = descripcion;
    document.getElementById('txtprocedimiento').value = procedimiento;
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

//FORMULARIO PUESTOS DE ENTURNAMIENTO
function puestosent(id,nombre,descripcion,funcionario,zona,idzona)
{
    document.getElementById('hidpuesto').value = id;
    document.getElementById('txtnombre').value = nombre;
    document.getElementById('txtdescripcion').value = descripcion;
    document.getElementById('txtfuncionario').value = funcionario;
    document.getElementById('hidzonas').value = idzona;
    document.getElementById('txtpoblacioncompleta').value = zona;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function gener_cumplidos(id, tcum, tdes, carac) {
    document.getElementById('hidcumplido').value = id;
    document.getElementById('cmbtipo').value = tcum;
    document.getElementById('cmbdespacho').value = tdes;
    document.getElementById('cmbcaracter').value = carac;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function tipoempaques(id,descr)
{
    /*window.opener.document.getElementById("hidempaque").value = id;
    window.opener.document.getElementById("txtempaque").value = descr;
    cerrar();*/

    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidempaque").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtempaque").value = descr;
    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    window.parent.tb_remove();
    
}

function territoriales(id,cod,descr)
{
    /*window.opener.document.getElementById('hidactividadeconomica').value = id;
    window.opener.document.getElementById('txtactividadeconomica').value = descr;
    cerrar();*/
    
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidterritorial").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtterritorial").value = "(" + cod + ")" + " " + descr;
    window.parent.tb_remove();
}


//FORMULARIO ACTIVIDADES ECONOMICAS
function actividades(id,cod,descr)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidactividadeconomica").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtactividadeconomica").value = descr;
    window.parent.tb_remove();
}

//FORMULARIO COLORES
function colores(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcolor").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtcolor").value = descr;
    window.parent.tb_remove();
}

//FORMULARIO CARROCERIAS
function carrocerias(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcarroceria").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtcarroceria").value = descr;
    window.parent.tb_remove();
}

function carroceriascot(cod, descr) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidcarroceria").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtcarroceria").value = descr;
    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    window.parent.tb_remove();
}


function carroceriaswin(cod,descr)
{ 
    window.opener.document.getElementById("hidcarroceria").value = cod;
    window.opener.document.getElementById("txtcarroceria").value = descr;
    cerrar();
}

//FORMULARIO MARCAS
function marcas(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidmarca").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtmarca").value = descr;
    window.parent.tb_remove();
}

//FORMULARIO LINEAS
function lineas(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidlinea").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtlinea").value = descr;
    window.parent.tb_remove();
}

//FORMULARIO LINEAS
function gps(cod,descr,web)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidempresagps").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtempresagps").value = descr;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidempresagpsweb").value = web;
    window.parent.tb_remove();
}

//FORMULARIO PUESTOS DE ENTURNAMIENTO
function puestos(id,puesto)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpuesto").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpuesto").value = puesto;
    window.parent.tb_remove();
}

function generadoresentur(id,nombre) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtgenerador").value = nombre;
    var a = self.parent.document.getElementById("foo");
    a.style.display = 'block';
    window.parent.tb_remove();
//    window.opener.document.getElementById("hidgenerador").value = id;
//    window.opener.document.getElementById("txtgenerador").value = nombre;
//    cerrar();
}

function generadoresentur1(id,nombre)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtgenerador").value = nombre;
    window.parent.tb_remove();
}

function icasventas(id, porc) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidica").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_PorcIca").value = porc;
    //window.parent.tb_remove();
    //window.parent.location.href = "Venta.aspx?doc=" & self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidventa").value;
}



function CambioConductorAUX(info) {
    var vecInfo = info.split("|");
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtconductoraux").value = vecInfo[0];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtplaca').value = vecInfo[1];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelo').value = vecInfo[2];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmarca').value = vecInfo[3];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtlinea').value = vecInfo[4];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_lntrailer').value = vecInfo[5];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconductoresaux').value = vecInfo[6];
    window.parent.tb_remove();
}

function cambioConductor(info){

	var vecInfo=info.split("|");
	//console.log(vecInfo[0]);
	 self.parent.document.getElementById("ctl00_ContentPlaceHolder1_lnConductores").value = vecInfo[0];  
	// self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidvehiculo').value = idveh;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtplaca').value = vecInfo[1];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmodelo').value = vecInfo[2];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtmarca').value = vecInfo[3];
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtlinea').value = vecInfo[4];
	self.parent.document.getElementById('ctl00_ContentPlaceHolder1_lntrailer').value = vecInfo[5];
   // var foto = self.parent.document.getElementById('ctl00_ContentPlaceHolder1_imvehiculo').src = imagen;
    self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidconductor').value = vecInfo[6];
    window.parent.tb_remove();
  //window.top.location.href = "http://codigomaldito.blogspot.com/2011/06/hacer-un-redirect-con-javascript.html";
}


function generadoresserv1(id,doc,gen,suc,dir,pob,dv,ter,vence,plazo)
{

    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtgenerador").value = gen;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = dir;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacion").value = pob;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigito").value = dv;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtercero").value = ter;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidplazo").value = plazo;
    
    var fecha_vence = self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence");
    if (fecha_vence != null){
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence").value = vence;
    }
    
    window.parent.tb_remove();
}


function generadoresserv(id, doc, gen, suc, dir, pob, dv, ter, vence, toperet, ventas, idgenerador, asesor, iddespachador, despachador, tipo) {
    //alert("1" + " " + toperet + " - " + ventas);
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtgenerador").value = gen;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = dir;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacion").value = pob;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigito").value = dv;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtercero").value = ter;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidasesor").value = idgenerador;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtasesor").value = asesor;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hiddespachador").value = iddespachador;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdespachador").value = despachador;
    
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtipoter").value = tipo;
    
    if (ventas == 1){
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtTopeRetencion").value = toperet;
    }
    
    var fecha_vence = self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence");
    if (fecha_vence != null) {
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence").value = vence;
    }

    window.parent.tb_remove();
}


function generadoresservmovdet(id, gen) {
    //alert(toperet + " - " + ventas);
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtgenerador").value = gen;

    window.parent.tb_remove();
}

function generadoresserv2(id, doc, gen, suc, dir, pob, dv, ter, vence, toperet, ventas, tipo) {
    //alert("2" + " " + toperet + " - " + ventas);
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidgenerador").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtgenerador").value = gen;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = dir;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacion").value = pob;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigito").value = dv;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtercero").value = ter;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtipoter").value = tipo;

    if (ventas == 1) {
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtTopeRetencion").value = toperet;
    }

    var fecha_vence = self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence");
    if (fecha_vence != null) {
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence").value = vence;
    }

    window.parent.tb_remove();
}


function generadoresserv_facturar(idgen, idmov, id) {

    window.location.href = 'MovimientoFacturar.aspx?idgen=' + idgen + '&idmov=' + idmov + '&busq=1&id=' + id;
}


function conductoresbusqueda(id, doc, nom1, nom2, ape1, ape2, suc, dir, pob, dv) {

    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidconductor").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre1").value = nom1;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre2").value = nom2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido").value = ape1;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido2").value = ape2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = dir;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtzona").value = pob;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsuc").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigito").value = dv;
    
    window.parent.tb_remove();
}


//FORMULARIOS EMPRESAS
function emp(cod,descr)
{ 
    window.opener.document.getElementById("hidempresa").value = cod;
    window.opener.document.getElementById("txtempresa").value = descr;
    cerrar();
}

//FORMULARIO TRANSITO MATRICULA (VEHICULO DETALLES)
function transitomatricula(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtransito").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txttransito").value = descr;
    window.parent.tb_remove();
}

//FORMULARIO EMPRESAS GPS (VEHICULO DETALLES)
function transitomatricula(cod,descr)
{ 
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidempresagps").value = cod;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtempresagps").value = descr;
    window.parent.tb_remove();
}

//FORMULARIO PRELIQUIDACIONES
function Asignar_propietario(id, doc, dv, nom, dir, pob, suc, plazo, vence) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpropietario").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigito").value = dv;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpropietario").value = nom;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = dir;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacion").value = pob;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidplazo").value = plazo;

    var fecha_vence = self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence");
    if (fecha_vence != null) {
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechavence").value = vence;
    }

    window.parent.tb_remove();
}

//FORMULARIO MOVIMIENTOS TERCEROS
function Asignar_propietario_mov_tercero(id, doc, dv, nom, dir, pob, suc) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpropietario").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumentoter").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdigitoter").value = dv;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpropietario").value = nom;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccionter").value = dir;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacionter").value = pob;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursalter").value = suc;

    window.parent.tb_remove();
}

function Asignar_propietarioMov(id, nom) {
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpropservicio").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtprestadorser").value = nom;
    window.parent.tb_remove();
}

//FORMULARIO ENTIDADES BANCARIAS
function entidades(id,cod,descr)
{
    window.opener.document.getElementById('txtbanco').value = descr;
    window.opener.document.getElementById('hidbanco').value = id;
    cerrar();
}

//FORMULARIO PERFILES
function perfiles(id,perfil,descripcion,superu)
{
    document.getElementById('hidperfil').value = id;
    document.getElementById('txtperfil').value = perfil;
    document.getElementById('txtdescripcion').value = descripcion;
    document.getElementById('hidsuperusuario').value = superu;
            
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btguardar').disabled = true;  
    document.getElementById('btasignar').disabled = false;  
    document.getElementById('txtperfil').disabled = true;
    document.getElementById('txtdescripcion').disabled = true;
    //cerrar();
}


//FORMULARIO PRODUCTOS
function productosmin(prod,idprod,cod,gen) {

    var params = idprod +"|" + prod + "|" + cod + "|" + gen;
    window.location.href = "GeneradoresProductos.aspx?prod=" + params + "&_ihneg=" + gen + " ";
    
    /*
    window.opener.document.getElementById('txtproducto').value = prod;
    window.opener.document.getElementById('hidproducto').value = idprod;
    window.opener.document.getElementById('txtcodigo').value = cod;
    
    window.opener.document.getElementById('btnuevo').disabled = true;
    window.opener.document.getElementById('btguardar').disabled = false;
    window.opener.document.getElementById('btmodificar').disabled = true;
    window.opener.document.getElementById('bteliminar').disabled = true;
    window.opener.document.getElementById('btdeshacer').disabled = false;
    
    window.opener.document.getElementById('txtproducto').disabled = true;
    window.opener.document.getElementById('txtinicial').disabled = false;
    window.opener.document.getElementById('txtfinal').disabled = false;
    window.opener.document.getElementById('cmbriesgos').disabled = false;
    window.opener.document.getElementById('cmbescoltajes').disabled = false;
    window.opener.document.getElementById('txtpersonal').disabled = false;
    
    cerrar();*/
}

//FORMULARIO SUCURSALESBUSCAR
function sucursales(id,sucur,desc)
{

    document.getElementById('hidsucursal').value = id;
    document.getElementById('txtsucursal').value = sucur;
    document.getElementById('txtdescripcion').value = desc;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btguardar').disabled = true; 
     
    document.getElementById('txtsucursal').disabled = true;
    document.getElementById('txtdescripcion').disabled = true;
}


//----------------****************************************************************




function tercerosub(idter,doc,nom1,nom2,ape1,ape2,master,tipoventana,dig)
{
    //master = 1: Cuando retorna los datos a un formulario que no pertene a un master page
    //master = 2: Cuando retorna los datos a un formulario que pertene a un master page

    //tipoventana = 1: modal jquery
    //tipoventana = 2: modal window
    if (tipoventana=="1"){
    
        if(master=="1"){
            self.parent.document.getElementById('hidtercero').value = idter;
            self.parent.document.getElementById('txtdocumento').value = doc;
            self.parent.document.getElementById('txtnombre1').value = nom1;
            self.parent.document.getElementById('txtnombre2').value = nom2;
            self.parent.document.getElementById('txtapellido').value = ape1;
            self.parent.document.getElementById('txtapellido2').value = ape2;
            self.parent.document.getElementById('txtdigito').value = dig;
        }else{
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_hidtercero').value = idter;
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdocumento').value = doc;
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre1').value = nom1;
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre2').value = nom2;
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido').value = ape1;
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido2').value = ape2;
            self.parent.document.getElementById('ctl00_ContentPlaceHolder1_txtdigito').value = dig;
        }
        
        window.parent.tb_remove();
    
    }else{
    
        if(master=="1"){
            window.opener.document.getElementById('hidtercero').value = idter;
            window.opener.document.getElementById('txtdocumento').value = doc;
            window.opener.document.getElementById('txtnombre1').value = nom1;
            window.opener.document.getElementById('txtnombre2').value = nom2;
            window.opener.document.getElementById('txtapellido').value = ape1;
            window.opener.document.getElementById('txtapellido2').value = ape2;
            window.opener.document.getElementById('txtdigito').value = dig;
        }else{
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hidtercero').value = idter;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtdocumento').value = doc;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre1').value = nom1;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtnombre2').value = nom2;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido').value = ape1;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtapellido2').value = ape2;
            window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtdigito').value = dig;
        }
        cerrar();
    }
}



//FORMULARIO TIPOPPARENTESCOS
function parentescos(id,des)
{
    window.opener.document.getElementById('hidparentesco').value = id;
    window.opener.document.getElementById('txtparentesco').value = des;
    cerrar();
}

//BUSQUEDAPAISES
function paises(id,pais)
{
    //window.opener.document.getElementById('txtnacionalidad').value = pais;
    //window.opener.document.getElementById('hidpais').value = id;
    
    //cerrar();
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpais").value = id;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnacionalidad").value = pais;
    
    window.parent.tb_remove();
    
}

//FORMULARIO DOCUMENTOSTERCEROS
function llenardocumentos(id, abr, des)
{
    window.opener.document.getElementById('hiddocumento').value = id;
    window.opener.document.getElementById('txtabreviatura').value = abr;
    window.opener.document.getElementById('txtdescripcion').value = des;
               
    window.opener.document.getElementById('txtestado').value = 'modificar'; 
        
    window.opener.document.getElementById('btnuevo').disabled = true;
    window.opener.document.getElementById('btmodificar').disabled = false;
    window.opener.document.getElementById('bteliminar').disabled = false;
    window.opener.document.getElementById('btguardar').disabled = true;
    window.opener.document.getElementById('btdeshacer').disabled = false;
        
    window.opener.document.getElementById('txtabreviatura').focus();
    
    cerrar();
    
}

//POBLACION
function obtenerpoblacion(poblacion,idpoblacion,ind)
{


    // Esta funcion permite retornar dinamicamente el id y la poblacion en cualquier formulario, segun parametros enviados, como tipo de ventana y tipo de retorno
    // ind 1 es para caso donde se abre de una ventana tb_show() y retorna a una masterpage
    // ind 2 es para caso donde se abre de una ventana showmodal() y retorna a un formulario normal
    // ind 3 es para caso donde se abre de una ventana showmodal() y retorna a una masterpage
    // ind 4 por error lo repeti
    // ind 5 es para caso donde se llama la busqueda de ciudades desde una ventana tb_show() y debe redireccionar en vez de abrir

    if (ind == '1') {
        necesita = self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hiddenvalor").value;
    } else if (ind == '2') {
        necesita = window.opener.document.getElementById('hiddenvalor').value;
    } else if (ind == '3') {
        necesita = window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hiddenvalor').value;
    } else if (ind == '4') {
        necesita = window.opener.document.getElementById('hiddenvalor').value;
    } else if (ind == '5') {
        necesita = 50;
    }
    
    
    switch(necesita)
    {
        case "1": // Unico busqueda zonas form usuarios
            window.opener.document.getElementById('hidzonas').value = idpoblacion;
            window.opener.document.getElementById('txtpoblaciontercero').value = poblacion;        
            break;
        case "2": // Poblacion terceros - form Propietarios
            window.opener.document.getElementById('hidzonatercero').value = idpoblacion;
            window.opener.document.getElementById('txtpoblaciontercero').value = poblacion;        
            break;
        case "3": // Poblacion nacimiento - form Propietarios
            window.opener.document.getElementById('hidzonanacimiento').value = idpoblacion;
            window.opener.document.getElementById('txtpoblacionnacimiento').value = poblacion;        
            break;
        case "4": // Poblacion Documento - form Propietarios
            window.opener.document.getElementById('hidzonadocumento').value = idpoblacion;
            window.opener.document.getElementById('txtpoblaciondocumento').value = poblacion;        
            break;
        case "5": //Poblacion tercero - form Generadores
            window.opener.document.getElementById('hidzonatercero').value = idpoblacion;
            window.opener.document.getElementById('txtpoblaciontercero').value = poblacion;        
            break;
        case "6": //Poblacion cobro - form Generadores
            window.opener.document.getElementById('hidzonas0').value = idpoblacion;
            window.opener.document.getElementById('txtpoblacioncompleta1').value = poblacion;        
            break;
        case "7": //Poblacion tercero - form terceros telefonos
            window.opener.document.getElementById('hidzonas').value = idpoblacion;
            window.opener.document.getElementById('txtpoblacioncompleta').value = poblacion;  
            break;
        case "8":
            window.opener.document.getElementById('hidorigen').value = idpoblacion;
            window.opener.document.getElementById('txtorigen').value = poblacion;        
            break;            
        case "9":
            window.opener.document.getElementById('hiddestino').value = idpoblacion;
            window.opener.document.getElementById('txtdestino').value = poblacion;        
            break;
        case "12":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonatercero").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciontercero").value = poblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonas0").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacioncompleta1").value = poblacion;
            break;
        case "13":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonanacimiento").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacionnacimiento").value = poblacion;
            break;
        case "14":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonadocumento").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciondocumento").value = poblacion;
            break;
        case "17":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonas").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacioncompleta").value = poblacion;
            break;
        case "18":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonas0").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacioncompleta1").value = poblacion;
            break;
        case "19":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonatercero").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciontercero").value = poblacion;
            break;
        case "20":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpoblacion").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacion").value = poblacion;
            break;
        case "23":
            window.opener.document.getElementById('hidzonacargue').value = idpoblacion;
            window.opener.document.getElementById('txtpoblacioncargue').value = poblacion;        
            break;
        case "24":
            window.opener.document.getElementById('hidzonadescargue').value = idpoblacion;
            window.opener.document.getElementById('txtpoblaciondescargue').value = poblacion;        
            break;
        case "26":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpobrepresentante").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciontercero").value = poblacion;        
            break;
        case "27":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpobaseguradora").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpobaseguradora").value = poblacion;        
            break;
        case "28":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpobempresa").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpobempresa").value = poblacion;
            break;
        case "29":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidorigen").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtorigen").value = poblacion;
            var a = self.parent.document.getElementById("foo");
            a.style.display = 'block';
            break;
        case "30":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hiddestino").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdestino").value = poblacion;
            var a = self.parent.document.getElementById("foo");
            a.style.display = 'block';
            break;
        case "31":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzona").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacionrem").value = poblacion;
            break;
        case "32":
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonas").value = idpoblacion;
            self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacioncompleta").value = poblacion;
            var a = self.parent.document.getElementById("foo");
            a.style.display = 'block';
            break;
    }
        
        if(ind=='1'){
            window.parent.tb_remove();
        }else{
            cerrar();
        }
        
    
}

function poblaciones(valor)
{
    document.getElementById('hiddenvalor').value = valor;
    modal(450,580,200,150,'busquedadepartamentos.aspx');
}

//TERCEROS BUSCAR
function terceros(tdoc,doc,n1,n2,ap,ap2,di,zon,idzon,idt,suc)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbtipodocumento").value = tdoc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre1").value = n1;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre2").value = n2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido").value = ap;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido2").value = ap2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = di;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtercero").value = idt;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidnuevasucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciontercero").value = zon;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonatercero").value = idzon;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_existedetalletercero").value = 'NO';

    window.parent.tb_remove();
}

//FORMULARIO EMPRESA.ASPX, TERCERO PARA EL REPRESENTANTE LEGAL
function tercerosreplegal(doc,n1,n2,ap,ap2,di,zon,idzon,idt)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocrepresentante").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre1").value = n1;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre2").value = n2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido").value = ap;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido2").value = ap2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdirrepresentante").value = di;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidrepresentante").value = idt;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciontercero").value = zon;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpobrepresentante").value = idzon;

    window.parent.tb_remove();
}
//FORMULARIO EMPRESA.ASPX, TERCERO PARA LA ASEGURADORA
function tercerosaseguradoraempresa(doc,n1,di,zon,idzon,idt)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocaseguradora").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnomaseguradora").value = n1;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdiraseguradora").value = di;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidaseguradora").value = idt;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpobaseguradora").value = zon;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpobaseguradora").value = idzon;

    window.parent.tb_remove();
}



//TERCEROS BUSCAR CON DETALLES
function terceros_detalles(tdoc,doc,n1,n2,ap,ap2,di,zon,idzon,idt,suc,idtdet,tr,tn,est,gen,cor,snom,fnac,eciv,idpobn,pobn,idpobd,pobd,pnac,pcom)
{
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbtipodocumento").value = tdoc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdocumento").value = doc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre1").value = n1;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnombre2").value = n2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido").value = ap;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapellido2").value = ap2;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtdireccion").value = di;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtercero").value = idt;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidnuevasucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtsucursal").value = suc;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciontercero").value = zon;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonatercero").value = idzon;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_existedetalletercero").value = 'NO';
                   
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_existedetalletercero").value = 'SI';
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidtercerodetalle").value = idtdet;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbtiporegimen").value = tr;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbnaturaleza").value = tn;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbestado").value = est;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtcorreo").value = cor;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtapodo").value = snom;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_cmbestadocivil").value = eciv;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonanacimiento").value = idpobn;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblacionnacimiento").value = pobn;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidzonadocumento").value = idpobd;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtpoblaciondocumento").value = pobd;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_hidpais").value = pnac;
    self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtnacionalidad").value = pcom; 
    //self.parent.document.getElementById("RadioButtonList1").value = gen;  
    //self.parent.document.form1.RadioButtonList1[gen-1].checked = true;

    if (fnac=='1900-01-01'){
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechanacimiento").value='';
    }else{
        self.parent.document.getElementById("ctl00_ContentPlaceHolder1_txtfechanacimiento").value=fnac;
    }
    //alert(fnac);
    window.parent.tb_remove();

}


//FORM CONDUCTORES
function conductores(idcond,obs,apr,fapr,hapr,uapr,idter,tdoc,doc,suc,n1,n2,a1,a2,ncom,dir,idpob,pob,tr,tn,est,gen,cor,snom,fnac,eciv,idpobn,pobn,idpobd,pobd,pnac,pcom,idterdet)
{
    if (fnac=='1900-01-01'){
        window.opener.document.getElementById('txtfechanacimiento').value='';
    }else{
        window.opener.document.getElementById('txtfechanacimiento').value=fnac;
    }
        window.opener.document.getElementById('cmbtipodocumento').value = tdoc;
        window.opener.document.getElementById('txtdocumento').value = doc;
        window.opener.document.getElementById('txtnombre1').value = n1;
        window.opener.document.getElementById('txtnombre2').value = n2;
        window.opener.document.getElementById('txtapellido').value = a1;
        window.opener.document.getElementById('txtapellido2').value = a2;
        window.opener.document.getElementById('txtdireccion').value = dir;
        window.opener.document.getElementById('hidtercero').value = idter;
        window.opener.document.getElementById('hidnuevasucursal').value = suc;
        window.opener.document.getElementById('txtsucursal').value = suc;
        window.opener.document.getElementById('txtpoblaciontercero').value = pob;
        window.opener.document.getElementById('hidzonatercero').value = idpob;
        window.opener.document.getElementById('hidconductor').value = idcond;
        window.opener.document.getElementById('txtobservaciones').value = obs;
        window.opener.document.getElementById('hidaprobado').value = apr;
        window.opener.document.getElementById('hidusuarioaprueba').value = uapr;
        window.opener.document.getElementById('hidtercerodetalle').value = idterdet;
        window.opener.document.getElementById('cmbtiporegimen').value = tr;
        window.opener.document.getElementById('cmbnaturaleza').value = tn;
        window.opener.document.getElementById('cmbestado').value = est;
        window.opener.document.form1.RadioButtonList1[gen-1].checked = true;
        window.opener.document.getElementById('txtcorreo').value = cor;
        window.opener.document.getElementById('txtapodo').value = snom;
        window.opener.document.getElementById('cmbestadocivil').value = eciv;
        window.opener.document.getElementById('hidzonanacimiento').value = idpobn;
        window.opener.document.getElementById('txtpoblacionnacimiento').value = pobn;
        window.opener.document.getElementById('hidzonadocumento').value = idpobd;
        window.opener.document.getElementById('txtpoblaciondocumento').value = pobd;
        window.opener.document.getElementById('hidpais').value = pnac;
        window.opener.document.getElementById('txtnacionalidad').value = pcom;
        
        if(est=="1"){
            window.opener.document.getElementById('txestado').value = "ESTADO: ACTIVO";
        }else{document.getElementById('txestado').value = "ESTADO: INACTIVO"};
                
        if(apr=="1"){
            window.opener.document.getElementById('txaprobado').value = "APROBADO";
        }else{
            window.opener.document.getElementById('txaprobado').value = "NO APROBADO"
        };
        
        cerrar();
        
}


//FORM PROPIETARIOS
function propietarios(idprop,obs,apr,fapr,hapr,uapr,idter,tdoc,doc,suc,n1,n2,a1,a2,ncom,dir,idpob,pob,tr,tn,est,gen,cor,snom,fnac,eciv,idpobn,pobn,idpobd,pobd,pnac,pcom,idterdet)
{
    window.opener.document.getElementById('cmbtipodocumento').value = tdoc;
    window.opener.document.getElementById('txtdocumento').value = doc;
    window.opener.document.getElementById('txtnombre1').value = n1;
    window.opener.document.getElementById('txtnombre2').value = n2;
    window.opener.document.getElementById('txtapellido').value = a1;
    window.opener.document.getElementById('txtapellido2').value = a2;
    window.opener.document.getElementById('txtdireccion').value = dir;
    window.opener.document.getElementById('hidtercero').value = idter;
    window.opener.document.getElementById('hidnuevasucursal').value = suc;
    window.opener.document.getElementById('txtsucursal').value = suc;
    window.opener.document.getElementById('txtpoblaciontercero').value = pob;
    window.opener.document.getElementById('hidzonatercero').value = idpob;
                
    window.opener.document.getElementById('hidpropietario').value = idprop;
    window.opener.document.getElementById('txtobservaciones').value = obs;
    window.opener.document.getElementById('hidaprobado').value = apr;
    window.opener.document.getElementById('hidusuarioaprueba').value = uapr;
                
    window.opener.document.getElementById('hidtercerodetalle').value = idterdet;
    window.opener.document.getElementById('cmbtiporegimen').value = tr;
    window.opener.document.getElementById('cmbnaturaleza').value = tn;
    window.opener.document.getElementById('cmbestado').value = est;
    window.opener.document.form1.RadioButtonList1[gen-1].checked = true;
    window.opener.document.getElementById('txtcorreo').value = cor;
    window.opener.document.getElementById('txtapodo').value = snom;
    window.opener.document.getElementById('txtfechanacimiento').value = fnac;
    window.opener.document.getElementById('cmbestadocivil').value = eciv;
    window.opener.document.getElementById('hidzonanacimiento').value = idpobn;
    window.opener.document.getElementById('txtpoblacionnacimiento').value = pobn;
    window.opener.document.getElementById('hidzonadocumento').value = idpobd;
    window.opener.document.getElementById('txtpoblaciondocumento').value = pobd;
    window.opener.document.getElementById('hidpais').value = pnac;
    window.opener.document.getElementById('txtnacionalidad').value = pcom;
                
    if(est=="1")
    {window.opener.document.getElementById('txestado').value = "ESTADO: ACTIVO";
    }else{window.opener.document.getElementById('txestado').value = "ESTADO: INACTIVO"};
                
    if(apr=="1")
    {window.opener.document.getElementById('txaprobado').value = "APROBADO";
    }else{window.opener.document.getElementById('txaprobado').value = "NO APROBADO"};
    
    cerrar();
    
}


//FORM USUARIOS
function usuarios(tdoc,doc,n1,n2,ap,ap2,di,zon,idzon,idt,suc,idu,usu,pass,cad,fcad,idper,est,cor)
{
    window.opener.document.getElementById('cmbtipodocumento').value = tdoc;
    window.opener.document.getElementById('txtdocumento').value = doc;
    window.opener.document.getElementById('txtnombre1').value = n1;
    window.opener.document.getElementById('txtnombre2').value = n2;
    window.opener.document.getElementById('txtapellido').value = ap;
    window.opener.document.getElementById('txtapellido2').value = ap2;
    window.opener.document.getElementById('txtdireccion').value = di;
    window.opener.document.getElementById('hidtercero').value = idt;
    window.opener.document.getElementById('hidnuevasucursal').value = suc;
    window.opener.document.getElementById('txtsucursal').value = suc;
    window.opener.document.getElementById('txtpoblaciontercero').value = zon;
    window.opener.document.getElementById('hidzonatercero').value = idzon;

    window.opener.document.getElementById('hidusuario').value = idu;
    window.opener.document.getElementById('txtusuario').value = usu;
    window.opener.document.getElementById('txtpass').value = pass;
    window.opener.document.getElementById('chkcaduca').value = cad;
    window.opener.document.getElementById('txtfechacaduca').value = fcad;
    window.opener.document.getElementById('cmbperfil').value = idper;
    window.opener.document.getElementById('cmbestado').value = est;
    window.opener.document.getElementById('txtcorreo').value = cor;
                              
    if (cad == '1')
    {window.opener.document.form1.chkcaduca.checked = true;}
    else if(cad == '0')
    {window.opener.document.form1.chkcaduca.checked = false;}

    cerrar();
}

//FORM GENERADORESCARGA

function generadores(tdoc,doc,n1,n2,ap,ap2,di,zon,idzon,idt,suc,nat,ret,cor,dcobro,plazo,inter,cupo,cierre,acu,est,idgen,zon1,idzon1,tope,maneja,treg,web,aa,obs,fia,act,idact)
{
    window.opener.document.getElementById('cmbtipodocumento').value = tdoc;
    window.opener.document.getElementById('txtdocumento').value = doc;
    window.opener.document.getElementById('txtnombre1').value = n1;
    window.opener.document.getElementById('txtnombre2').value = n2;
    window.opener.document.getElementById('txtapellido').value = ap;
    window.opener.document.getElementById('txtapellido2').value = ap2;
    window.opener.document.getElementById('txtdireccion').value = di;
    window.opener.document.getElementById('hidtercero').value = idt;
    window.opener.document.getElementById('hidnuevasucursal').value = suc;
    window.opener.document.getElementById('txtsucursal').value = suc;
    window.opener.document.getElementById('txtpoblaciontercero').value = zon;
    window.opener.document.getElementById('hidzonatercero').value = idzon;
                
    window.opener.document.getElementById('cmbnaturaleza').value = nat;
    window.opener.document.getElementById('txtretencion').value = ret;
    window.opener.document.getElementById('txtcorreo').value = cor;
    window.opener.document.getElementById('txtdireccioncobro').value = dcobro;
    window.opener.document.getElementById('txtplazo').value = plazo;
    window.opener.document.getElementById('txtintermediacion').value = inter;
    window.opener.document.getElementById('txtcupo').value = cupo;
    window.opener.document.getElementById('cmbdiascierre').value = cierre;
    window.opener.document.getElementById('cmbacuerdo').value = acu;
    window.opener.document.getElementById('cmbestado').selectedIndex = est;
    window.opener.document.getElementById('hidgenerador').value = idgen;
    window.opener.document.getElementById('txtpoblacioncompleta1').value = zon1;
    window.opener.document.getElementById('hidzonas0').value = idzon1;
    window.opener.document.getElementById('txttope').value = tope;
                
    window.opener.document.getElementById('cmbtiporegimen').value = treg;
    window.opener.document.getElementById('txtweb').value = web;
    window.opener.document.getElementById('txtapartadoaereo').aa;
    window.opener.document.getElementById('txtobservacion').value = obs;
    window.opener.document.getElementById('txtfechainicio').value = fia;
    window.opener.document.getElementById('txtactividadeconomica').value = act;
    window.opener.document.getElementById('hidactividadeconomica').value = idact;
                
    window.opener.document.getElementById('txtdocumento').disabled = true;
    window.opener.document.getElementById('btigual').disabled = true;
    
    if (tdoc == '2')
    {
        window.opener.document.getElementById('txtnombre2').disabled = true;
        window.opener.document.getElementById('txtapellido').disabled = true;
        window.opener.document.getElementById('txtapellido2').disabled = true;
    }
                
    if (maneja == '1')
    {
        window.opener.document.form1.chkmanejatope.checked = true;
    }
    else if(maneja == '0')
    {
        window.opener.document.form1.chkmanejatope.checked = false;
    }
    
    cerrar();
    
}

//FORM COMISIONES

function comisiones(idcom,tdes,inter,ini,fin,obs)
{
    document.getElementById('hidcomision').value = idcom;
    document.getElementById('cmbtipodespacho').value = tdes;
    document.getElementById('txtintermediacion').value = inter;

    document.getElementById('txtinicial').value = ini;
    document.getElementById('txtfinal').value = fin;
    document.getElementById('txtobservacion').value = obs;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('cmbtipodespacho').disabled = true;   
}


//FORM CLASE VEHICULOS
function clasesdevehiculos(id, descr, minis, capca, capne, confi, color, valor) {
    
    document.getElementById('hid').value = id;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('txtcodministerio').value = minis;
    document.getElementById('txtcapcarga').value = capca;
    document.getElementById('txtcapneta').value = capne;
    document.getElementById('txtconfiguracion').value = confi;
    document.getElementById('txtcolor').value = color;
    document.getElementById('txtValor').value = valor;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}

//FORM EMPRESAS GPS
function empresasgps(ide,emp,sig,web)
{
    document.getElementById('hidempresa').value = ide;
    document.getElementById('txtempresa').value = emp;
    document.getElementById('txtsigla').value = sig;
    document.getElementById('txtweb').value = web;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}


function tipomantenimiento(idman,nombre,desc,codigo) 
{
    document.getElementById('hidmantenimiento').value = idman;
    document.getElementById('txtmantenimiento').value = nombre;
    document.getElementById('txtdescripcion').value = desc;
    document.getElementById('txtcodigo').value = codigo;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}



//FORM EMPRESAS (ASEGURADORAS,TRANSPORTES,SERVITECAS)
function empresas(ide,tdoc,iden,emp,sig,dir,tel,idz,zona)
{
    document.getElementById('hidempresa').value = ide;
    document.getElementById('cmbtdocumento').value = tdoc;
    document.getElementById('txtidentificacion').value = iden;
    document.getElementById('txtempresa').value = emp;
    document.getElementById('txtsigla').value = sig;
    document.getElementById('txtdireccion').value = dir;
    document.getElementById('txttelefono').value = tel;
    document.getElementById('hidzonas').value = idz;
    document.getElementById('txtpoblacioncompleta').value = zona;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    
}


//FORM RANGOS
function rangos(id,inf,sup,com)
{
    document.getElementById('hidrangocomision').value = id;
    document.getElementById('txtinferior').value = inf;
    document.getElementById('txtsuperior').value = sup;
    document.getElementById('txtcomision').value = com;
            
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
            
    document.getElementById('txtinferior').disabled = true;
    document.getElementById('txtsuperior').disabled = true;
    document.getElementById('txtcomision').disabled = true;
}


//FORM TRANSACCIONES
function transacciones(fecha,hora,modulo,proceso,descripcion,usuario)
{
    document.getElementById('txtmostrarfecha').value = fecha;
    document.getElementById('txtmostrarhora').value = hora;
    document.getElementById('txtmostrarmodulo').value = modulo;
    document.getElementById('txtmostrarproceso').value = proceso;
    document.getElementById('txtmostrardescripcion').value = descripcion;
    document.getElementById('txtmostrarusuario').value = usuario;
}


//FORM TRANSFERENCIAS
function transferencias(id,doc,cuen,tcuen,nom1,nom2,ap1,ap2,of,idban,ban,ncom,indicappal)
{
	
    document.getElementById('hidtransferencias').value = id;
    document.getElementById('txtdoctitular').value = doc;
    document.getElementById('txtcuenta').value = cuen;
    document.getElementById('cmbtipocuenta').value = tcuen;
    document.getElementById('txtnom1').value = nom1;
    document.getElementById('txtnom2').value = nom2;
    document.getElementById('txtape1').value = ap1;
    document.getElementById('txtape2').value = ap2;  
    document.getElementById('txtoficina').value = of; 
    document.getElementById('hidbanco').value = idban;
    document.getElementById('txtbanco').value = ban; 
    document.getElementById('txtestado').value = 'modificar'; 
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
	//document.getElementById('cmbtiporef').focus();		
	//document.getElementById('checkppal').checked = true;	
	if (indicappal == 1) { document.getElementById('checkppal').checked = true; } else { document.getElementById('checkppal').checked = false; }
}


//FORM TERCEROS TELEFONOS

function telefonoster(d, t,e,p,idp,idt,tip,ind,pal)
{
    document.getElementById('txtdescripcion').value = d;
    document.getElementById('txttelefono').value = t;
    document.getElementById('txtextension').value = e;
    document.getElementById('txtpoblacioncompleta').value = p;
    document.getElementById('hidzonas').value = idp;
    document.getElementById('hidtelefonos').value = idt;

    document.getElementById('cmbtipotel').value = tip;
    document.getElementById('hidindica').value = ind;
    if (pal == "PRINCIPAL") { document.getElementById('chprincipal').checked = true;}else{document.getElementById('chprincipal').checked = false;}
    //alert(tip + "-" + ind + "-" + pal);
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
        
    document.getElementById('txtdescripcion').disabled = true;
    document.getElementById('txttelefono').disabled = true;
    document.getElementById('txtextension').disabled = true;
    document.getElementById('txtpoblacioncompleta').disabled = true;
    document.getElementById('poblacionesss').disabled = true;
}


//NOVEDADES VEHICULOS
function seguimientoVehiculo(usuario, fecha, hora, seguimiento, estadonov)
{
	
    document.getElementById('txtUsuario').value = usuario;
    document.getElementById('txtFecha').value = fecha;
    document.getElementById('txtHora').value =hora;
    document.getElementById('txtSeguimiento').value =seguimiento;
		
		console.log("Ole");
		
	if (estadonov=='RESUELTA'){
			document.getElementById('btnuevo').disabled = true;
	}else{
		 document.getElementById('btguardar').disabled = false;
	}
   document.getElementById('btguardar').disabled = true;

	}






function asesorespre(an, e, f, m, a, ma, ju, jul, ag, sep, oct, nov, dic) {
    document.getElementById('fecha').value = an;
    document.getElementById('txtenero').value = e;
    document.getElementById('txtfebrero').value = f;
    document.getElementById('txtmarzo').value = m;
    document.getElementById('txtabril').value = a;
    document.getElementById('txtmayo').value = ma;
    document.getElementById('txtjunio').value = ju;

    document.getElementById('txtjulio').value = jul;
    document.getElementById('txtagosto').value = ag;
    document.getElementById('txtseptiembre').value = sep;
    document.getElementById('txtoctubre').value = oct;
    document.getElementById('txtnoviembre').value = nov;
    document.getElementById('txtdiciembre').value = dic;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('btsalir').disabled = false;

//    document.getElementById('txtdescripcion').disabled = true;
//    document.getElementById('txttelefono').disabled = true;
//    document.getElementById('txtextension').disabled = true;
//    document.getElementById('txtpoblacioncompleta').disabled = true;
//    document.getElementById('poblacionesss').disabled = true;
}

function tipo_documentos_transporte(idtipo, documento, descripcion)
{
    document.getElementById('hidtipo').value = idtipo;
    document.getElementById('txtdocumento').value = documento;
    document.getElementById('txtdescripcion').value = descripcion;
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
        
    document.getElementById('txtestado').value = "modificar";
}


//FORM REFERENCIAS TERCEROS
function referenciaster(idref,tref,nom,cont,car,tel,ext,cor,fec,cal,idpob,obs,pob,preg,resp,cel)
{
    var radio = null;
    var indice_radio = parseInt(cal) - parseInt(1);
    
    var calimg1 = document.getElementById('Im1');
    var calimg2 = document.getElementById('Im2');
    var calimg3 = document.getElementById('Im3');
    var calimg4 = document.getElementById('Im4');
    var calimg5 = document.getElementById('Im5');
    
    document.getElementById('hidreferencias').value = idref;
    document.getElementById('cmbtiporef').value = tref;
    document.getElementById('txtnombre').value = nom;
    document.getElementById('txtcontacto').value = cont;
    document.getElementById('txtcargo').value = car;
    document.getElementById('txttelefono').value = tel;
    document.getElementById('txtextension').value = ext;
    document.getElementById('txtcorreo').value = cor;  
    document.getElementById('txtfecha').value = fec; 
    
    radio = document.getElementsByName('RadioButtonList1');
    radio[indice_radio].checked = true;
    
    if (cal == '1'){
        calimg1.src = '../presentacion/imagenes/bar1.jpg';
        calimg2.src = '../presentacion/imagenes/bar2.jpg';
        calimg3.src = '../presentacion/imagenes/bar2.jpg';
        calimg4.src = '../presentacion/imagenes/bar2.jpg';
        calimg5.src = '../presentacion/imagenes/bar2.jpg';
    }else if (cal == '2'){
        calimg1.src = '../presentacion/imagenes/bar1.jpg';
        calimg2.src = '../presentacion/imagenes/bar1.jpg';
        calimg3.src = '../presentacion/imagenes/bar2.jpg';
        calimg4.src = '../presentacion/imagenes/bar2.jpg';
        calimg5.src = '../presentacion/imagenes/bar2.jpg';
    }else if (cal == '3'){
        calimg1.src = '../presentacion/imagenes/bar1.jpg';
        calimg2.src = '../presentacion/imagenes/bar1.jpg';
        calimg3.src = '../presentacion/imagenes/bar1.jpg';
        calimg4.src = '../presentacion/imagenes/bar2.jpg';
        calimg5.src = '../presentacion/imagenes/bar2.jpg';
    }else if (cal == '4'){
        calimg1.src = '../presentacion/imagenes/bar1.jpg';
        calimg2.src = '../presentacion/imagenes/bar1.jpg';
        calimg3.src = '../presentacion/imagenes/bar1.jpg';
        calimg4.src = '../presentacion/imagenes/bar1.jpg';
        calimg5.src = '../presentacion/imagenes/bar2.jpg';
    }else if ( cal == '5'){
        calimg1.src = '../presentacion/imagenes/bar1.jpg';
        calimg2.src = '../presentacion/imagenes/bar1.jpg';
        calimg3.src = '../presentacion/imagenes/bar1.jpg';
        calimg4.src = '../presentacion/imagenes/bar1.jpg';
        calimg5.src = '../presentacion/imagenes/bar1.jpg';
    }
    
    
    document.getElementById('txtpoblacioncompleta').value = pob; 
    document.getElementById('txtobservacion').value = obs;   
    document.getElementById('hidzonas').value = idpob;  
    document.getElementById('txtpregunta').value = preg;
    document.getElementById('txtrespuesta').value = resp;
    document.getElementById('txtcelular').value = cel; 
    document.getElementById('txtestado').value = 'modificar'; 
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
        
    document.getElementById('cmbtiporef').focus();
}


//FOMR CONDUCTORES LICENCIAS
function licenciaster(idlic,num,cat,gsan,rest,fexp,fven,ruta,ter,idtran,tran)
{
    document.getElementById('hidlicencia').value = idlic;
    document.getElementById('txtnlicencia').value = num;
    document.getElementById('cmbcategoria').value = cat;
    document.getElementById('cmbgsanguineo').value = gsan;
    document.getElementById('cmbrestricciones').value = rest;
    document.getElementById('txtfechaexpedicion').value = fexp;
    document.getElementById('txtfechavencimiento').value = fven;
    document.getElementById('hidtercero').value = ter;
    document.getElementById('hidtransito').value = idtran;
    document.getElementById('txttransito').value = tran;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('btfecexp').disabled = true;
    document.getElementById('btfecven').disabled = true;
    
    document.getElementById('hidlicencia').disabled = true;
    document.getElementById('txtnlicencia').disabled = true;
    document.getElementById('cmbcategoria').disabled = true;
    document.getElementById('cmbgsanguineo').disabled = true;
    document.getElementById('cmbrestricciones').disabled = true;
    document.getElementById('txtfechaexpedicion').disabled = true;
    document.getElementById('txtfechavencimiento').disabled = true;
    document.getElementById('hidtercero').disabled = true;
    document.getElementById('hidtransito').disabled = true;
    document.getElementById('txttransito').disabled = true;
            
}

//FORM EMERGENCIAS TERCEROS

function emergenciaster(id,idterdet,nom,tel,avi,idpar,par,idpob,pob,celu)
{
    document.getElementById('hid').value = id;
    document.getElementById('hidterdetalle').value = idterdet;
    document.getElementById('txtnombre').value = nom;
    document.getElementById('txttelefono').value = tel;
    document.getElementById('cmbaviso').value = avi;
    document.getElementById('hidparentesco').value = idpar;
    document.getElementById('txtparentesco').value = par;
    document.getElementById('hidzonas').value = idpob;
    document.getElementById('txtpoblacioncompleta').value = pob;
    document.getElementById('txtcelular').value = celu; 
    document.getElementById('txtestado').value = 'modificar'; 
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
        
    document.getElementById('txtcontacto').focus();
}


//FORM TERCEROS DOCUMENTOS INFO
function infodocumentoster(id, des)
{
    //alert(id+' - '+des);
    window.opener.document.getElementById('hiddoctercero').value = id;
    window.opener.document.getElementById('txtdocumento').value = des;
    
    cerrar(); 
}

//FORM VEHICULOS DOCUMENTOS INFO
function infodocumentosveh(id, des,temp)
{
    //alert(id+' - '+des);
    window.opener.document.getElementById('hiddocvehiculo').value = id;
    window.opener.document.getElementById('txtdocumento').value = des;
    window.opener.document.getElementById('hidtipoempresa').value = temp;
    
    cerrar(); 
}


//FORM TERCEROS CONTACTOS
function contactoster(idcontacto, contacto, cargo, telefono, correo, poblacion, idpoblacion, ext, celular)
{
    document.getElementById('hidcontacto').value = idcontacto;
    document.getElementById('txtcontacto').value = contacto;
    document.getElementById('txtcargo').value = cargo;
    document.getElementById('txttelefono').value = telefono;
    document.getElementById('txtcorreo').value = correo;
    document.getElementById('txtpoblacioncompleta').value = poblacion;
    document.getElementById('hidzonas').value = idpoblacion;
    document.getElementById('txtext').value = ext;
    document.getElementById('txtcelular').value = celular;
    document.getElementById('txtestado').value = 'modificar'; 
        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btdeshacer').disabled = false;
        
    document.getElementById('txtcontacto').focus();
}


//FORM TERCEROS DOCUMENTOS
function documentoster(iddt,idter,idtdoc,ident,fexp,fven,obs,ruta,inic,descr,digital,vence)
{
    document.getElementById('hid').value = iddt;
    document.getElementById('hidtercero').value = idter;
    document.getElementById('hiddoctercero').value = idtdoc;
    document.getElementById('txtidentificador').value = ident;
    document.getElementById('txtfechaexpedicion').value = fexp;
    document.getElementById('txtfechavencimiento').value = fven;
    document.getElementById('txtdocumento').value = descr;
    document.getElementById('txtobservacion').value = obs;
    document.getElementById('cmbVence').value = vence;
    document.getElementById('hidfechaven').value = fven;
    
    document.getElementById('txtidentificador').value = ident;
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    
}

function pruebajs()
{
    alert('1234');
}

//FORM VEHICULOS DOCUMENTOS
function documentosvehiculares(id,idveh,idtdoc,ident,fexp,fven,descr,obs, idemp, empresa, temp, vence)
{
    document.getElementById('hid').value = id;
    document.getElementById('hidvehiculo').value = idveh;
    document.getElementById('hiddocvehiculo').value = idtdoc;
    document.getElementById('txtidentificador').value = ident;
    document.getElementById('txtfechaexpedicion').value = fexp;
    document.getElementById('txtfechavencimiento').value = fven;
    document.getElementById('txtdocumento').value = descr;
    document.getElementById('txtobservacion').value = obs;
    document.getElementById('hidempresa').value = idemp;
    document.getElementById('txtempresa').value = empresa;
    document.getElementById('hidtipoempresa').value = temp;
    document.getElementById('cmbVence').value = vence;
    
    document.getElementById('txtidentificador').value = ident;
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
}

//FORM GENERADORES PRODUCTOS
function productosgen(idprodgen, idgen, idprod, prod,ini,fin,rie,esc,pmin,cmin)
{
    document.getElementById('hidprodgen').value = idprodgen;
    document.getElementById('hidproducto').value = idprod;
    document.getElementById('hidgenerador').value = idgen;
    document.getElementById('txtproducto').value = prod;
    document.getElementById('txtinicial').value = ini;
    document.getElementById('txtfinal').value = fin;
    document.getElementById('cmbriesgos').value = rie; 
    document.getElementById('cmbescoltajes').value = esc;
    document.getElementById('txtpersonal').value = pmin;
    document.getElementById('txtcodigo').value = cmin;
            
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
}

//FORM CONDUCTORES CAPACITACIONES
function capacitacionester(idcap, idcond, fec, capac, enti, inte, obs)
{
    document.getElementById('hidcapacitacion').value = idcap;
    document.getElementById('hidconductor').value = idcond;
    document.getElementById('txtfecha').value = fec;
    document.getElementById('txtcapacitacion').value = capac;
    document.getElementById('txtentidad').value = enti;
    document.getElementById('txtintensidad').value = inte;
    document.getElementById('txtobservacion').value = obs;
                        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('dtsalir').disabled = false;
    document.getElementById('btcalendario').disabled = true;
            
    document.getElementById('hidcapacitacion').disabled = true;
    document.getElementById('hidconductor').disabled = true;
    document.getElementById('txtfecha').disabled = true;
    document.getElementById('txtcapacitacion').disabled = true;
    document.getElementById('txtentidad').disabled = true;
    document.getElementById('txtobservacion').disabled = true;
    document.getElementById('txtintensidad').disabled = true;
}


//FORM TERCEROS DOCUMENTOS
function documentosterc(id,ini,descr, perm, venc)
{
    document.getElementById('hiddocumento').value = id;
    document.getElementById('txtabreviatura').value = ini;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('cbpermiso').value = perm;
    document.getElementById('cbvencimiento').value = venc;
    

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('dtdeshacer').disabled = false;
}

//FORM VEHIUCLOS DOCUMENTOS
function documentosveh(id,ini,descr,clas, permi, venci)
{
    document.getElementById('hiddocumento').value = id;
    document.getElementById('txtabreviatura').value = ini;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('cmbclasificacion').value = clas;
    document.getElementById('cbpermiso').value = permi;
    document.getElementById('cbvencimiento').value = venci;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('dtdeshacer').disabled = false;
}
//FORM CONFIGURACION DOCUMENTOS VEHICULOS
function configuraciondoc(id, config)
{
    document.getElementById('hidid').value = id;
    document.getElementById('txtconfiguracion').value = config;
    

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('dtdeshacer').disabled = false;

}


//FOMR TERCEROS BITACORA
function bitacorater(idbitacora,fecha,asunto,tema,compromiso,seguimiento,contacto,asesor)
{
    document.getElementById('hidbitacora').value = idbitacora;
    document.getElementById('txtfecha').value = fecha;
    document.getElementById('txtasunto').value = asunto;
    document.getElementById('txttema').value = tema;
    document.getElementById('txtcompromiso').value = compromiso;
    document.getElementById('txtseguimiento').value = seguimiento;
    document.getElementById('cmbcontacto').value= contacto;
    document.getElementById('cmbasesor').value = asesor;
                        
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('dtsalir').disabled = false;
    document.getElementById('btcalendario').disabled = true;
            
    document.getElementById('hidbitacora').disabled = true;
    document.getElementById('txtfecha').disabled = true;
    document.getElementById('txtasunto').disabled = true;
    document.getElementById('txttema').disabled = true;
    document.getElementById('txtcompromiso').disabled = true;
    document.getElementById('txtseguimiento').disabled = true;
    document.getElementById('cmbcontacto').disabled = true;
    document.getElementById('cmbasesor').disabled = true;
}




//FORM FLETES GENERADOR
function fletesgenerador(idf,idori,iddes,ori,des,descri,idgen,idclas,clas, idpro, pro, idase ,ase , tipodes)
{
    //alert("Entro");
    document.getElementById('hidgeneradorflete').value = idf;
    
    document.getElementById('hidgenerador').selectedIndex = idgen;
    document.getElementById('hidorigen').value = idori;
    document.getElementById('hiddestino').value = iddes;
	document.getElementById('hidclase').value = idclas;
	document.getElementById('hidproducto').value = idpro;
	document.getElementById('hidasesor').value = idase;
	document.getElementById('hidtipodes').value = tipodes;
    
    document.getElementById('txtorigen').value = ori;
    document.getElementById('txtdestino').value = des;
    document.getElementById('txtdescripcion').value = descri;
    document.getElementById('txtestado').value = 'modificar';
	document.getElementById('txtclase').value = clas;
	document.getElementById('txtProducto').value = pro;
	document.getElementById('txtAsesor').value = ase;
	document.getElementById('cmbtipodepacho').value = tipodes;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;

}

//FORM COMPARENDOS 
function comparendos(codigo, trancito, valor, fec, servicio, tipo, placa, descripcion, idcomparendo, idconductor)
    {
        document.getElementById('txtdcodigo').value = codigo;
        document.getElementById('txttransito').value = trancito;
        document.getElementById('txtvalor').value = valor;
        document.getElementById('txtfechaexpedicion').value = fec;
        document.getElementById('cmbServicio').value = servicio;
        document.getElementById('txttipovehiculo').value = tipo;
        document.getElementById('txtPlaca').value = placa;
        document.getElementById('txtdescripcion').value = descripcion;
        document.getElementById('hidtercero').value = idconductor;
        document.getElementById('hidcomparendo').value = idcomparendo;
        
        document.getElementById('btmodificar').disabled = false;
        document.getElementById('btguardar').disabled = true;
       
    }


    //FORM ACCIDENTES INCIDENTES

    function accidentes_incidentes(fecha, area, clasif, lugar, placvehiculo, descripcion, idvehiculo, idacci) {
//        alert("Entro");
        //        console.log("Entro");
        document.getElementById('txtfechaexpedicion').value = fecha;
        document.getElementById('cmbArea').value = area;
        document.getElementById('cmbClasificacion').value = clasif;
        document.getElementById('txtlugar').value = lugar;
        document.getElementById('txtPlaca').value = placvehiculo;
        document.getElementById('txtdescripcion').value = descripcion;        
        document.getElementById('hidvehiculo').value = idvehiculo;
        document.getElementById('hidacciden').value = idacci;

        document.getElementById('btmodificar').disabled = false;
        document.getElementById('btguardar').disabled = true;
    }

    function accidentes_incidentes_vehi(fecha, area, clasif, lugar, placvehiculo, descripcion, idvehiculo, idacci) {
        //        alert("Entro");
        //        console.log("Entro");
        document.getElementById('txtfechaexpedicion').value = fecha;
        document.getElementById('cmbArea').value = area;
        document.getElementById('cmbClasificacion').value = clasif;
        document.getElementById('txtlugar').value = lugar;
        document.getElementById('txtcoductor').value = placvehiculo;
        document.getElementById('txtdescripcion').value = descripcion;
        document.getElementById('hidtercero').value = idvehiculo;
        document.getElementById('hidacciden').value = idacci;

        document.getElementById('btmodificar').disabled = false;
        document.getElementById('btguardar').disabled = true;
    }



//FORM FLETES GENERADOR DETALLES
function rangosflete(id,inf,sup,flemp,flter,ind,tiem)
{   
    document.getElementById('hidrangoflete').value = id;
    document.getElementById('txtinferior').value = inf;
    document.getElementById('txtsuperior').value = sup;
    document.getElementById('txtfleteempresa').value = flemp;
    document.getElementById('txtfletetercero').value = flter;
    document.getElementById('txttiempo').value = tiem;
    document.getElementById('cmbindica').value = ind;
         
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
            
    document.getElementById('txtinferior').disabled = true;
    document.getElementById('txtsuperior').disabled = true;
    document.getElementById('txtcomision').disabled = true;
}




//TOOLTIP BOTONES
/************************************************************************************************************
Ajax dynamic list
Copyright (C) September 2005  DTHMLGoodies.com, Alf Magne Kalleland

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Dhtmlgoodies.com., hereby disclaims all copyright interest in this script
written by Alf Magne Kalleland.

Alf Magne Kalleland, 2006
Owner of DHTMLgoodies.com
	
************************************************************************************************************/	
function showToolTip(e,text){
	if(document.all)e = event;
	
	var obj = document.getElementById('bubble_tooltip');
	var obj2 = document.getElementById('bubble_tooltip_content');
	obj2.innerHTML = text;
	obj.style.display = 'block';
	var st = Math.max(document.body.scrollTop,document.documentElement.scrollTop);
	if(navigator.userAgent.toLowerCase().indexOf('safari')>=0)st=0; 
	var leftPos = e.clientX + 10;
	if(leftPos<0)leftPos = 0;
	obj.style.left = leftPos + 'px';
	obj.style.top = e.clientY - obj.offsetHeight + 50 + st + 'px';
}	

function hideToolTip()
{
	document.getElementById('bubble_tooltip').style.display = 'none';
	
}


function tablaenturnamiento(sitio,pesoapr,indice,placa,modelo,moderep,conf,peso,capac,cond,doc,apcon,apveh)
{ 
    // Debe haber una tabla que tenga como propiedad idtabla
    // La tabla tiene una etiqueta llamada TBODY
   
    var tbody = document.getElementById('detallestabla').getElementsByTagName("TBODY")[0];
     
    if (parseInt(indice)==0){
        enturn(placa,modelo,moderep,conf,peso,capac,cond,doc,apcon,apveh);
        //eliminar_filas_anteriores();
    }
    
    var c = new Array();
    var p = new Array();

    c = sitio.split('&&');
    p = pesoapr.split('&&');
  
    // Creamos los elementos TR para las filas de la tabla
    var titulo = document.createElement("TR");
    var columna01 = document.createElement("TD");
    var columna02 = document.createElement("TD");
    columna01.appendChild(document.createTextNode('Destino Interés')); 
    columna02.appendChild(document.createTextNode('Peso Apróx. (KG)')); 
    titulo.appendChild(columna01);
    titulo.appendChild(columna02);
    tbody.appendChild(titulo);
  
    var k = 1;
    for (k=1;k<=c.length-1;k++){
        
        var fila = document.createElement("TR"); 
        var columna11 = document.createElement("TD");
        var columna12 = document.createElement("TD");
        columna11.appendChild(document.createTextNode(c[k]));
        if (p[k]=='0'){
            columna12.appendChild(document.createTextNode('NO ESPECIFICA'));
        }else{
            columna12.appendChild(document.createTextNode(p[k]));
        }
        
        fila.appendChild(columna11);
        fila.appendChild(columna12);
        tbody.appendChild(fila);
    } 
}

function eliminar_filas_anteriores()
{
    var tbody = document.getElementById('detallestabla').getElementsByTagName("TBODY")[0];
    var j=0;
    
    limpiar_enturn();
    
    while(j<=tbody.childNodes.length){
        tbody.removeChild(tbody.childNodes[j]);
    }
            
//    for(var i = 0; i <= tbody.childNodes.length+(tbody.childNodes.length-i); i++){
//       var tr = tbody.getElementsByTagName("TR");
//       tbody.removeChild(tbody.childNodes[i]);
//    }
    
    
}

function limpiar_enturn()
{
    document.getElementById('ctl00_ContentPlaceHolder1_l_placa').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_modelo').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_modelorep').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_configuracion').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_peso').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_capacidad').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_conductor').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_documento').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_estveh').value = '';
    document.getElementById('ctl00_ContentPlaceHolder1_l_estcon').value = '';
}

function enturnamiento_detallado(id,idgenerador,idpoblacion,peso,observ,poblacion,generador)
{
    document.getElementById('hiddetalle').value = id;
    document.getElementById('hidgenerador').value = idgenerador;
    document.getElementById('hidzonas').value = idpoblacion;
    document.getElementById('txtpeso').value = peso;
    document.getElementById('txtobservacion').value = observ;
    document.getElementById('txtpoblacioncompleta').value = poblacion;
    document.getElementById('txtgenerador').value = generador;
    
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    
    document.getElementById('txtgenerador').disabled = true;
    document.getElementById('txtpoblacioncompleta').disabled = true;
    document.getElementById('txtestado').value = 'modificar';

}

function aduanas(id, nombre, descripcion, idpais, pais) {
    document.getElementById('hidaduana').value = id;
    document.getElementById('txtnombre').value = nombre;
    document.getElementById('txtdescripcion').value = descripcion;
    document.getElementById('hidpais').value = idpais;
    document.getElementById('txtpais').value = pais;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
    document.getElementById('txtpais').disabled = true;
}


function mantenimietos_especiales(idmant,nombre,kmini,kmfin,fechaini,fechafin,observaciones) {




    document.getElementById('hidmantenimiento').value = idmant;
    document.getElementById('txttipoman').value = nombre;
    document.getElementById('txtkiloini').value = kmini;
    document.getElementById('txtkilofin').value = kmfin;
    document.getElementById('txtfechaini').value = fechaini;
    document.getElementById('txtfechafin').value = fechafin;
    document.getElementById('txtobservacion').value = observaciones;


    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('bteliminar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
    document.getElementById('btsalir').disabled = false;
}

function eventospuertos(id, cod, event, descrip) {
	document.getElementById('hidevento').value = id;
	document.getElementById('txtcodigo').value = cod;
	document.getElementById('txtevento').value = event;
	document.getElementById('txtdescripcion').value = descrip;

	document.getElementById('btnuevo').disabled = true;
	document.getElementById('btguardar').disabled = true;
	document.getElementById('btdeshacer').disabled = false;
	document.getElementById('btmodificar').disabled = false;
	document.getElementById('btsalir').disabled = false;
}



contenido_textarea = ""; 
num_caracteres_permitidos = 11; 

function valida_longitud(idcampo){ 
    alert(document.getElementById(idcampo).length);
   num_caracteres = document.getElementById(idcampo).length; 
    alert(num_caracteres);
    alert(num_caracteres_permitidos);
   if (num_caracteres > num_caracteres_permitidos){ 
      document.getElementById(idcampo).value = contenido_textarea; 
   }else{ 
      contenido_textarea = document.getElementById(idcampo).value;	
   }

}

function solo_numeros(e) {

    var evt = (e) ? e : window.event;
    var key = (evt.keyCode) ? evt.keyCode : evt.which;
    if (key != null) {
        key = parseInt(key, 10);
        if ((key < 48 || key > 57) && (key < 96 || key > 105)) {
            //Aca tenemos que reemplazar "Decimals" por "NoDecimals" si queremos que no se permitan decimales
            if (!jsIsUserFriendlyChar(key, "Decimals")) {
                return false;
            }
        }
        else {
            if (evt.shiftKey) {
                return false;
            }
        }
    }
    return true;
}

// Función para las teclas especiales
//------------------------------------------
function jsIsUserFriendlyChar(val, step) {
    // Backspace, Tab, Enter, Insert, y Delete
    if (val == 8 || val == 9 || val == 13 || val == 45 || val == 46) {
        return true;
    }
    // Ctrl, Alt, CapsLock, Home, End, y flechas
    if ((val > 16 && val < 21) || (val > 34 && val < 41)) {
        return true;
    }
    if (step == "Decimals") {
        if (val == 190 || val == 110) {  //Check dot key code should be allowed
            return true;
        }
    }
    // The rest
    return false;
}

//FORM VEHIUCLOS DOCUMENTOS
function documentostrayler(id, ini, descr, permi, venci) {
    document.getElementById('hiddocumento').value = id;
    document.getElementById('txtabreviatura').value = ini;
    document.getElementById('txtdescripcion').value = descr;
    document.getElementById('cbpermiso').value = permi;
    document.getElementById('cbvencimiento').value = venci;

    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('dtdeshacer').disabled = false;
}

function documentosTraylers(id, idveh, idtdoc, ident, fexp, fven, descr, obs, vence) {
    document.getElementById('hid').value = id;
    document.getElementById('hidtrayler').value = idveh;
    document.getElementById('hiddoctrayler').value = idtdoc;
    document.getElementById('txtidentificador').value = ident;
    document.getElementById('txtfechaexpedicion').value = fexp;
    document.getElementById('txtfechavencimiento').value = fven;
    document.getElementById('txtdocumento').value = descr;
    document.getElementById('txtobservacion').value = obs;
    document.getElementById('cmbVence').value = vence;
    document.getElementById('txtidentificador').value = ident;
    document.getElementById('btnuevo').disabled = true;
    document.getElementById('btguardar').disabled = true;
    document.getElementById('btmodificar').disabled = false;
    document.getElementById('btdeshacer').disabled = false;
}
function equiposcostos(id, vrid, des, vrdes) {
    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(des).value = vrdes;        
    self.parent.tb_remove();
}
function equipostrans(id, vrid, des, vrdes) {
    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(des).value = vrdes;
    self.parent.tb_remove();
}

function equiposbate(id, vrid, des, vrdes) {
    self.parent.document.getElementById(id).value = vrid;
    self.parent.document.getElementById(des).value = vrdes;
    self.parent.tb_remove();
}


function equipostercero(id, vrid, des, vrdes) {
    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_hidtercero').value = vrid;
    window.opener.document.getElementById('ctl00_ContentPlaceHolder1_txtdocumento').value = vrdes;
    cerrar();
}

function contratos_detalle_editar(id, valor, fechaini, fechafin, factura) {


    document.getElementById('hididcontratodetalle').value = id;
    document.getElementById('txtvalor').value = valor;
    document.getElementById('txtfechaini').value = fechaini;
    document.getElementById('txtfechafin').value = fechafin;
    document.getElementById('hidfactura').value = factura;
    document.getElementById('btguardar').disabled = false;

}

function contratos_editar(num, ter, fini, ffin, factura, periodo, cocepto, valor, obs, id, id_ter, id_periodo, id_cocepto,id_equipo,tipo_fac, obs2) {
    
    document.getElementById('txtcontrato').value = num;
    document.getElementById('txttecero').value = ter;
    document.getElementById('txtfechaexpedicion').value = fini;
    document.getElementById('txtfechavencimiento').value = ffin;
    document.getElementById('txtfecha_factura').value = factura;
    document.getElementById('cmbperiocidad').value = id_periodo;
    document.getElementById('cmbconcepto').value = id_cocepto;
    document.getElementById('txtvalor').value = valor;
    document.getElementById('txtobservacion').value = obs;

    document.getElementById('hidtercero').value = id_ter;
    document.getElementById('hidequipo').value = id_equipo;
    document.getElementById('hidcontrato').value = id;
    document.getElementById('cmbtipofacturacion').value = tipo_fac;  
    document.getElementById('txtobservacionven').value = obs2;


    document.getElementById('btmodificar').disabled = false;

}
