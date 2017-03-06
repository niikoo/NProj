####################
# AD RIGHTS CLIENT #
# NOM1 @ IRIS      #
####################

#############
# FUNCTIONS #
#############

# COLOR-WRITE
function Color-Write
{
    $allColors = ("-Black",   "-DarkBlue","-DarkGreen","-DarkCyan","-DarkRed","-DarkMagenta","-DarkYellow","-Gray",
                  "-Darkgray","-Blue",    "-Green",    "-Cyan",    "-Red",    "-Magenta",    "-Yellow",    "-White")
    $foreground = (Get-Host).UI.RawUI.ForegroundColor # current foreground
    $color = $foreground
    [bool]$nonewline = $false
    $sofar = ""
    $total = ""
    foreach($arg in $args)
    {
        if ($arg -eq "-nonewline") { $nonewline = $true }
        elseif ($arg -eq "-foreground")
        {
            if ($sofar) { Write-Host $sofar -foreground $color -nonewline }
            $color = $foregrnd
            $sofar = ""
        }
        elseif ($allColors -contains $arg)
        {
            if ($sofar) { Write-Host $sofar -foreground $color -nonewline }
            $color = $arg.substring(1)
            $sofar = ""
        }
        else
        {
            $sofar += "$arg "
            $total += "$arg "
        }
    }
    # last bit done special
    if (!$nonewline)
    {
        Write-Host $sofar -foreground $color
    }
    elseif($sofar)
    {
        Write-Host $sofar -foreground $color -nonewline
    }
}

[int]$port = 2008;
[string]$ip = "127.0.0.1";
$global:key = "NSIDK1OFL3AAIJ3O5M8M4U6NWD1SEE2NJSN9IDKKOGL4A5I6@JLBYUSBEA.KNOEDTFLN3I2I4K2O3OKEPICAF1992";
$global:id = 1;

# CREATE SOCKET
$endpoint = new-object System.Net.IPEndPoint([System.Net.IPAddress]::Parse($ip),$port)
$server = new-object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Stream, [System.Net.Sockets.ProtocolType]::Tcp);
if($server -eq $null) {
	Color-Write -red "Socket create error!";
	return;
}
try {
	$server.Connect($endpoint);
	Color-Write -Cyan "TCP CLIENT STARTED," -White " connected to $ip, port:" -Green "$port`n" 
} catch {
	Color-Write -red "Socket connect error!";
	return;
}
$encoding = new-object System.Text.UTF8Encoding
# Authorize
$tosend = "auth $key";
[int]$bytesSent = $server.Send($encoding.GetBytes($tosend));
$buffer = new-object System.Byte[] 64;
[int]$k = $server.Receive($buffer);
[string]$out = "";
for($i=0;$i -lt $k;$i++) {
	$out += [System.Convert]::ToChar($buffer[$i]);
}
[array]$outs = $out-split "-";
if($outs.Count -eq 2) {
	if($outs[0] -eq "ID") {
		$global:id = $outs[1];
	} else {
		Color-Write -Red "Auth failed! lvl:1";
		return;
	}
} else {
	Color-Write -Red "Auth failed! lvl:0";
	return;
}

# CLIENT LOOP
while($true)
{
	# Send / recive
	$tosenda = "cmd $key $global:id ";
	$tosendb = Read-Host "Send";
	$tosend = $tosenda + $tosendb;
	if($tosend.Length -ge 1) {
		[int] $bytesSent = $server.Send($encoding.GetBytes($tosend));
	}
	$buffer = new-object System.Byte[] 65536;
	[int]$k = $server.Receive($buffer);
	[string]$out = "";
	for($i=0;$i -lt $k;$i++) {
		$out += [System.Convert]::ToChar($buffer[$i]);
	}
	color-write -White "Response`:`n";
	color-write -Red $out;
}