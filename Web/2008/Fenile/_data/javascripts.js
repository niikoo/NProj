function MM_jumpMenu(targ,selObj,restore){ //v3.0
  eval(targ+".location='"+selObj.options[selObj.selectedIndex].value+"'");
  if (restore) selObj.selectedIndex=0;
}

function openClose(id)
	{
		var obj = "";	
		
		if(document.getElementById)
			obj = document.getElementById(id).style;
		else if(document.all)
			obj = document.all[id];
		else if(document.layers)
			obj = document.layers[id];
		else
			return 1;
		
		if(obj.display == "")
			obj.display = "none";
		else if(obj.display != "none")
			obj.display = "none";
		else
			obj.display = "block";
	}
var popUpWin=0;

function popUpWindow(URLStr, left, top, width, height) {

  if(popUpWin)

  {

    if(!popUpWin.closed) popUpWin.close();

  }

  popUpWin = open(URLStr, 'popUpWin', 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=no,resizable=no,copyhistory=yes,width='+width+',height='+height+',left='+left+', top='+top+',screenX='+left+',screenY='+top+'');

}
function changestyle(changeto,objectid) {
	document.all.getElementById(objectid).style = changeto;
}
function redirectURL(url) {
  var ns4 = document.layers;
  var ns6 = document.getElementById && !document.all;
  var ie4 = document.all;
  location = url;
}
function selectAll() {
	f = document.isel;
	c = f.selectall.checked;
	if (f.elements['filename[]']) {
		if (f.elements['filename[]'].length > 1) {
			for (i = 0; i < f.elements['filename[]'].length; i++) {
				f.elements['filename[]'][i].checked = c;
				value = f.elements['filename[]'][i].value;
				if ( c == true ) {
					document.getElementById(value).style.backgroundColor = "#A6FF99";
				} else if ( c == false ) {
					document.getElementById(value).style.backgroundColor = "#F6F1EE";
				}
			}
		} else {
			f.elements['filename[]'].checked = c;
		}
	}
}
function selxt(id,value) {
	if ( value == false ) {
		document.getElementById(id).style.backgroundColor = "#F6F1EE";
	} else if ( value == true ) {
		document.getElementById(id).style.backgroundColor = "#A6FF99";
	}
}
/* CONFIRM BUTTONS */
function fconfirm() {
	if ( document.isel.delete1.value != "            .: Confirm Delete :.            " ) {
		document.isel.delete1.value = "            .: Confirm Delete :.            ";
	} else {
		fcp('delete')
	}
}
function cconfirm() {
	if ( document.isel.chmods.value != "      .:   Confirm CHMOD   :.      " ) {
		document.isel.chmods.value = "      .:   Confirm CHMOD   :.      ";
	} else {
		fcp('chmod')
	}
}
function u_dconfirm() {
	if ( document.isel.chmods.value != "    .: Delete User :.    " ) {
		document.isel.chmods.value = "    .: Confirm Delete :.    ";
	} else {
		fcp('users')	
	}
}
function fcp(name) {
	document.isel.act.value=name
	document.isel.submit()
}