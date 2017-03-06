var editor = '"C:\\Program Files (x86)\\JetBrains\\PhpStorm 2016.1.2\\bin\\PhpStorm64.exe" nosplash --line %line% "%file%"';
var url = WScript.Arguments(0);
var match = /^phpstorm:\/\/open\/\?file=(.+)&line=(\d+)$/.exec(url);
if (match) {
	var file = decodeURIComponent(match[1]).replace(/\+/g, ' ');
	var command = editor.replace(/%line%/g, match[2]).replace(/%file%/g, file);
	var shell = new ActiveXObject("WScript.Shell");
	shell.Exec(command.replace(/\\/g, '\\\\'));
}