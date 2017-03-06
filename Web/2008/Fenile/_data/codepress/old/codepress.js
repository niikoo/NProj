/*
 * CodePress - Real Time Syntax Highlighting Editor written in JavaScript - http://codepress.fermads.net/
 * 
 * Copyright (C) 2006 Fernando M.A.d.S. <fermads@gmail.com>
 *
 * This program is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Lesser General Public License as published by the Free Software Foundation.
 * 
 * Read the full licence: http://www.opensource.org/licenses/lgpl-license.php
 */
 
$ = function() { return document.getElementById(arguments[0]); }
var cpPath = $('cp-script').src.replace('codepress.js','');
document.write('<link type="text/css" href="'+cpPath+'themes/default/codepress-editor.css" rel="stylesheet" />');
 
CodePress = {
	initialize : function() {
		cpWindow = $('cp-window');
		cpEditor = $('cp-editor');
		cpBody = cpEditor.contentWindow;
		cpBody.CodePress.syntaxHighlight('init');
		if(onLoadEdit) { onLoadEdit=false; this.edit(this.fileName,pgCode); }
	},
	
	detect : function() {
		cpEngine = 'older';
		var ua = navigator.userAgent;
		if(ua.match('MSIE')) cpEngine = 'msie';
		else if(ua.match('KHTML')) cpEngine = 'khtml'; 
		else if(ua.match('Opera')) cpEngine = 'opera'; 
		else if(ua.match('Gecko')) cpEngine = 'gecko';
	},
	
	setFileName : function() {
		$('cp-filename').innerHTML = this.fileName = (arguments[0]) ? arguments[0] : Content.menu.untitledFile;
	},

	getLanguage : function() {
		var extension = this.fileName.replace(/.*\.([^\.]+)$/,'$1');
		for(lang in Content.languages) {
			extensions = ','+Content.languages[lang].extensions+',';
			if(extensions.match(','+extension+',')) return lang;
		}
		return 'generic';
	},

	loadScript : function(target, src, callback) {
		var node = target.createElement("script");
		if (node.addEventListener) node.addEventListener("load", callback, false);
		else node.onreadystatechange = function() { if (this.readyState == "loaded") { callback.call(this);} }
		node.src = src;
		target.getElementsByTagName("head").item(0).appendChild(node);
		node = null;
	},
	
	setLanguage : function(reload) {
		this.language = (typeof(Content.languages[arguments[0]])!='undefined') ? arguments[0] : this.getLanguage();
		$('cp-language-name').innerHTML = Content.languages[this.language].name;
		$('language-'+this.language).checked = true;
		this.hideAllMenu();
		if(reload) {
			if(cpBody.document.designMode=='on') cpBody.document.designMode = 'off';
			this.loadScript(cpBody.document, '../languages/'+this.language+'.js', function () { cpBody.CodePress.syntaxHighlight('init'); })
			cpBody.document.getElementById('cp-lang-style').href = '../languages/'+this.language+'.css';
		}
	},
	
	hideAllMenu : function() {
		$('cp-options-menu').className = $('cp-languages-menu').className = 'hide';
		$('cp-arrow-languages').src = $('cp-arrow-options').src = cpPath+'themes/default/menu-arrow-up.gif' ;		
	},
	
	toogleMenu : function(item) {
		$('cp-'+item+'-menu').className = ($('cp-'+item+'-menu').className=='show') ? 'hide' : 'show' ;
		$('cp-arrow-'+item).src = ($('cp-'+item+'-menu').className=='show') ? cpPath+'themes/default/menu-arrow-down.gif' : cpPath+'themes/default/menu-arrow-up.gif' ;
	},
	
	toggleComplete : function() {
		this.complete = $('cp-complete').checked ? true : false ;
		this.hideAllMenu();
	},

	toggleLineNumbers : function() {
	    cpBody.document.getElementsByTagName('body')[0].className = ($('cp-linenumbers').checked) ? 'show-line-numbers' : 'hide-line-numbers';
		this.hideAllMenu();
	},
	
	resizeFullScreen : function() {
		if($('cp-fullscreen').checked) {
			cH = self.innerHeight ? self.innerHeight : document.documentElement.clientHeight;
			cW = self.innerWidth ? self.innerWidth : document.documentElement.clientWidth;
			cpEditor.style.height = cH-22 + 'px';
			cpWindow.style.height = cH-2 + 'px';
			cpWindow.style.width = cW-2 + 'px';
			if(cpWindow.offsetParent.offsetTop!=0||cpWindow.offsetParent.offsetLeft!=0) {
				cpWindow.style.top = - cpWindow.offsetParent.offsetTop-3 +'px';
				cpWindow.style.left = - cpWindow.offsetParent.offsetLeft-3 +'px';
			}

		}
	},
	
	toggleFullScreen : function() {
	    if($('cp-fullscreen').checked) {
			cpWindow.className = 'fullscreen-on';
			document.getElementsByTagName('html')[0].style.overflow = 'hidden';
			self.scrollTo(0,0); 
			this.resizeFullScreen();
	    }
	    else {
			cpWindow.className = 'fullscreen-off';
			document.getElementsByTagName('html')[0].style.overflow = 'auto';
			cpWindow.style.width = '100%';
			cpWindow.style.height = cpWindowHeight+'px';
			cpEditor.style.height = cpEditorHeight+'px';
			if(cpWindow.offsetParent.offsetTop!=0||cpWindow.offsetParent.offsetLeft!=0) 
			cpWindow.style.top = cpWindow.style.left = 'auto'
	    }
		this.hideAllMenu();
	},

	edit : function() {
		this.setFileName(arguments[0]);
		if(!arguments[1]||arguments[1]=='') { // file name of the source code (to open from server)
			this.setLanguage();
			cpEditor.src = cpPath+'modules/codepress.php?action=edit&file='+this.fileName+'&language='+this.language+'&engine='+cpEngine
			return;
		}
		this.setLanguage('reload');
		if($(arguments[1])) CodePress.setCode($(arguments[1]).firstChild.nodeValue); // id name of the source code
		else if(arguments[1].match(/\w/)) CodePress.setCode(arguments[1]);  // text of the source code
		else if(typeof(arguments[1])=='Object') CodePress.setCode(arguments[1].firstChild.nodeValue); // object of the source code
	},

	setContent : function() {
		var allLanguages = '';
		for(lang in Content.languages) 
			allLanguages += '<input type=radio name=lang id="language-'+lang+'" onclick="CodePress.setLanguage(\''+lang+'\')"><label for="language-'+lang+'">'+Content.languages[lang].name+'</label><br />';
		
		pgCode = ($('codepress').firstChild) ? $('codepress').firstChild.nodeValue : '';
		this.fileName = $('codepress').title;
		this.language = this.getLanguage();
		this.complete = true;
		onLoadEdit = (pgCode.match(/\w/)) ? true : false ;

		cpWindowHeight = $('codepress').clientHeight;
		cpEditorHeight = ($('codepress').className.match('hideMenu')) ? cpWindowHeight : cpWindowHeight-20 ;
		if(cpWindowHeight==0) { setTimeout(function(){CodePress.setContent();},10); return; } // if css is not loaded yet, try again
		
		$('codepress').innerHTML = '<div id="cp-window">'+
			'<iframe id="cp-editor" src="'+cpPath+'modules/codepress.php?engine='+cpEngine+'&file='+ (onLoadEdit ? 'null' : this.fileName) +'&language='+this.language+'" style="height:'+ cpEditorHeight +'px"></iframe>'+
			'<div id="cp-menu">'+
				'<em id="cp-filename"></em><span id="cp-options" onclick="CodePress.toogleMenu(\'options\')"><img src="'+cpPath+'themes/default/menu-icon-options.gif" align="top" /> '+Content.menu.options+' <img src="'+cpPath+'themes/default/menu-arrow-up.gif" align="top" id="cp-arrow-options" /></span><span id="cp-language" onclick="CodePress.toogleMenu(\'languages\')"><img src="'+cpPath+'themes/default/menu-icon-languages.gif" align="top" /> <span id="cp-language-name">'+Content.languages.generic.name+'</span> <img src="'+cpPath+'themes/default/menu-arrow-up.gif" align=top id="cp-arrow-languages" /></span>'+
			'</div>'+
			'<div id="cp-options-menu" class="hide">'+
    			'<input type="checkbox" id="cp-fullscreen" onclick="CodePress.toggleFullScreen()"><label for="cp-fullscreen">'+Content.menu.fullScreen+'</label><br><input type=checkbox id="cp-linenumbers" onclick="CodePress.toggleLineNumbers()" checked="checked"><label for="cp-linenumbers">'+Content.menu.lineNumbers+'</label><br><input type=checkbox id="cp-complete" onclick="CodePress.toggleComplete()" checked="checked"><label for="cp-complete">'+Content.menu.autoComplete+'</label>'+
			'</div>'+
			'<div id="cp-languages-menu" class="hide">'+allLanguages+'</div>'+
		'</div>';
		
		this.setLanguage(); 
		this.setFileName(this.fileName); 
	},

	// transform syntax highlighted code to original code
	getCode : function() {
		var code = cpBody.editor.innerHTML;
		code = code.replace(/<br>/g,'\n');
		code = code.replace(/<\/p>/gi,'\r');
		code = code.replace(/<p>/i,''); // IE first line fix
		code = code.replace(/<p>/gi,'\n');
		code = code.replace(/&nbsp;/gi,'');
		code = code.replace(/\u2009/g,'');
		code = code.replace(/<.*?>/g,'');
		code = code.replace(/&lt;/g,'<');
		code = code.replace(/&gt;/g,'>');
		code = code.replace(/&amp;/gi,'&');
		return code;
	},

	// put some code inside editor
	setCode : function() {
		var code = arguments[0];
		code = code.replace(/\u2009/gi,'');
		code = code.replace(/&/gi,'&amp;');		
       	code = code.replace(/</g,'&lt;');
        code = code.replace(/>/g,'&gt;');
		cpBody.document.getElementById("code").innerHTML = "<pre>"+code+"</pre>";
	}
}

CodePress.detect();
Content={};
CodePress.loadScript(document, cpPath+'content/'+$('cp-script').lang+'.js', function() { CodePress.setContent(); }); 
onresize = function() { CodePress.resizeFullScreen(); };