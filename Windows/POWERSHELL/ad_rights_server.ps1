####################
# AD RIGHTS SERVER #
# NOM1 @ IRIS      #
####################

# MODULES
try {
	Import-Module ActiveDirectory -ErrorAction Stop | Out-Null
} catch {
	echo "Trenger Microsoft ActiveDirectory Powershell Modulen (finnes i RSAT for Win7)";
	# http://itpro.no/artikkel/14923/administrasjon-av-active-directory-med-windows-powershell
	break;
}

# ALIAS
Set-Alias print Write-Host;

# ASSEMBLIES
$MWAPICP_Shell = [Reflection.Assembly]::LoadFrom(“C:\windows\Microsoft.NET\MicrosoftWindowsAPICodePack\Microsoft.WindowsAPICodePack.Shell.Dll”);


#############
# FUNCTIONS #
#############

# AD GROUP SEARCH
function Search-ADGroup {
	param($groupName);
	$group = Get-ADGroup -Filter * | Where-Object{$_.Name -like "*$groupName*"}
	return $group;
}

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

function taskbar-overlayIcon {
	param([string]$iconpath,[string]$iconname,[bool]$bitmap=$true); #BITMAP = png, gif, bmp, jpg; ELSE ico
	[System.Reflection.Assembly]::LoadWithPartialName("System.Drawing") | out-null
    # Verify the file exists
    if ([System.IO.File]::Exists($iconpath) -eq $true) {
        if($bitmap -eq $true) {
			$bitmapImg = new-object System.Drawing.Bitmap($iconpath);
			$iconHandle = $bitmapImg.GetHicon();
			$icon = [System.Drawing.Icon]::FromHandle($iconHandle);
		} else {
			$icon = new-object System.Drawing.Icon($iconpath)
        }
		if ($icon -ne $null) {
            $TaskBarObject = [Microsoft.WindowsAPICodePack.TaskBar.TaskBarManager]::Instance;
			$TaskBarObject.SetOverlayIcon($icon,$iconname);
        }
    } else {
        Write-Host "Icon file not found"
    }
}

# GLOBALS
$global:key = "NSIDK1OFL3AAIJ3O5M8M4U6NWD1SEE2NJSN9IDKKOGL4A5I6@JLBYUSBEA.KNOEDTFLN3I2I4K2O3OKEPICAF1992";
$global:id_cnt = 0;
$global:auth = @{};

function Set-Rights {
	param($path,$userGrp,$rights);
	if ([System.IO.Directory]::Exists($path) -ne $true) {
		color-write -Red "PATH DOESN'T EXIST";
		return $false;
	}
	$cmd = "cmd.exe /C icacls.exe $path /grant`:r $userGrp`:(OI)(CI)$rights /Q";
	$cmd = Start-Process 'C:\windows\system32\cmd.exe' $cmd -PassThru;
}

function Is-ADGroup {
	param($adGrp);
	try {
		$adg = Get-ADGroup -Filter * | Where-Object{$_.Name -eq $adGrp} 2> $null;
		if($adg.Name -eq $false) {
			return $false;
		} else {
			return $true;
		}
	} catch {
		return $false;
	}
}

function Set-Rights-PS {
	param($path,$userGrp,$rigths);
	color-write -Cyan "Setting rights";
	$InheritanceFlag = [System.Security.AccessControl.InheritanceFlags]::ContainerInherit -bor [System.Security.AccessControl.InheritanceFlags]::ObjectInherit
	$PropagationFlag = [System.Security.AccessControl.PropagationFlags]::InheritOnly
	$objType = [System.Security.AccessControl.AccessControlType]::Allow
	if ([System.IO.Directory]::Exists($path) -ne $true) {
		color-write -Red "PATH DOESN'T EXIST";
		return $false;
	}
	$acl = Get-Acl $path
	$permission = $userGrp,$rights, $InheritanceFlag, $PropagationFlag, $objType
	$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
	$acl.SetAccessRule($accessRule)
	Set-Acl $path $acl
	color-write -Green "OK!";
	return $true;
}

function runcommand {
	param($cmd,$clientaddr,$socket);
	$commandList = @{
		"Get-Help"	  =	"Display commandlist";
		"Get-Members" = "Display ActiveDirectory group members";
		"Add-Member"  = "Adds a member to an ActiveDirectory group";
		"Remove-Member" = "Removes a member from an ActiveDirectory group";
		"Set-Rights"  =	"Set rights";
	}
	$cmd = $cmd-split " ";
	if($cmd.Count -ge 1) {
		#color-write -Cyan "Decoding command, please wait..`n";
		# CHECK FOR AUTH COMMAND
		if($cmd.Count -ge 2) {
			if($cmd[0] -ieq "auth") {
				color-write -Yellow "Authorizing..`n";
				if($cmd[1] -eq $global:key) {
					if(!$global:auth[$clientaddr]) {
						Color-Write -Green "Authorized with $clientaddr `n";
						$global:id_cnt++;
						$global:auth.Add($clientaddr,$global:id_cnt);
						socket-send $socket "ID-$global:id_cnt";
					} else {
						Color-Write -Yellow "You're already authorized `n";
						socket-send $socket "ID-$global:id_cnt";
					}
				} else {
					Color-Write -Red "Not Authorized`n";
					socket-send $socket "deauth";
					taskbar-error
					return $false;
				}
			} elseif($cmd[0] -eq "cmd") {
				$failed = $true;
				if($cmd[1] -eq $global:key) {
					if($cmd[2] -eq $global:auth[$clientaddr]) {
						# COMMAND OUTPUT
						Color-Write -Cyan "Command" -nonewline;
						print $cmd[3] -nonewline;
						Color-Write -Cyan ", number of arguments`:" -nonewline;
						print ($cmd.Count-4) "`n";
						
						# CMD:
						#	setrights
						# Info:
						#	Sets rights
						# Params:
						#	[string]$user_grp
						#	[string]$rights
						if($cmd[3] -ieq "Set-Rights") {
							if($cmd.Count -eq 7 -and $cmd[4] -ine "--help") {
								$the_path = $cmd[4];
								$user_grp = $cmd[5];
								$rights = $cmd[6];
								Color-Write -Red "Set-Rights, data:";
								Color-Write -Magenta "Path: " -Green $the_path;
								Color-Write -Magenta "User/group: " -Green $user_grp;
								Color-Write -Magenta "Rights: " -Green $rights;
								Set-Rights $the_path $user_grp $rights;
								socket-send $socket "Set-Rights: OK!";
							} else {
								Color-Write -Magenta "Usage: Set-Rights [path] [AD user/group] [rights]"
								Color-Write "Rights:";
								Color-Write -Red  "N: " -White "No access";
								Color-Write -Cyan "F: " -White "Full access";
								Color-Write -Cyan "M: " -White "Modify";
								Color-Write -Cyan "RX:" -White "Read & exec";
								Color-Write -Cyan "R: " -White "Read";
								Color-Write -Cyan "W: " -White "Write";
								Color-Write -Cyan "D: " -White "Delete";
								socket-send $socket "Usage: Set-Rights [path] [AD user/group] [rights]";
							}
						# CMD:
						#	Get-Members
						# Info:
						#	Get AD group members
						# Params:
						#	[string]$group
						} elseif($cmd[3] -ieq "Get-Members") {
							if($cmd.Count -eq 5) {
								if(Is-ADGroup $cmd[4] -eq $true) {
									$members = Get-ADGroupMember $cmd[4] -Recursive;
									$out = "";
									foreach($member in $members) {
										$out += $member.SamAccountName;
										$out += "`n";
									}
									color-write -Magenta "Members:`n" -nonewline;
									print $out;
									socket-send $socket $out;
								} else {
									color-Write -Red "Group not found!";
									socket-send $socket "Group not found!";
								}
							} else {
								color-Write -Magenta "Usage: Get-Members [AD group]";
								socket-send $socket "Usage: Get-Members [AD group]";
							}
						# CMD:
						#	Add-Member
						# Info:
						#	Adds a member to an ActiveDirectory group
						# Params:
						#	[string]$group
						#	[string]$username
						} elseif($cmd[3] -ieq "Add-Member") {
							if($cmd.Count -eq 6) {
								if(Is-ADGroup $cmd[4] -eq $true) {
									try {
										Add-ADGroupMember -Identity $cmd[4] -Members $cmd[5] 2>$null;
										color-write -Green "The user" $cmd[5] "is successfully added!";
										$out = "The user ";
										$out += $cmd[5];
										$out += " is successfully added!";
										socket-send $socket $out;
									} catch {
										color-write -Red "An error occured! Couldn't add the user" $cmd[5] "to the group" $cmd[4];
										$out = "An error occured! Couldn't add the user ";
										$out += $cmd[5];
										$out += " to the group ";
										$out += $cmd[4];
										socket-send $socket $out;
									}
								} else {
									color-Write -Red "Group not found!";
									socket-send $socket "Group not found!";
								}
							} else {
								color-Write -Magenta "Usage: Add-Member [AD group] [AD user]";
								socket-send $socket "Usage: Add-Member [AD group] [AD user]";
							}
						# CMD:
						#	Remove-Member
						# Info:
						#	Removes a member from an ActiveDirectory group
						# Params:
						#	[string]$group
						#	[string]$username
						} elseif($cmd[3] -ieq "Remove-Member") {
							if($cmd.Count -eq 6) {
								if(Is-ADGroup $cmd[4] -eq $true) {
									try {
										Remove-ADGroupMember -Identity $cmd[4] -Members $cmd[5] -Confirm:$false 2>$null;
										color-write -Green "The user" $cmd[5] "is successfully removed!";
										$out = "The user ";
										$out += $cmd[5];
										$out += " is successfully removed!";
										socket-send $socket $out;
									} catch {
										color-write -Red "An error occured! Couldn't remove the user" $cmd[5] "from the group" $cmd[4];
										$out = "An error occured! Couldn't remove the user ";
										$out += $cmd[5];
										$out += " from the group ";
										$out += $cmd[4];
										socket-send $socket $out;
									}
								} else {
									color-Write -Red "Group not found!";
									socket-send $socket "Group not found!";
								}
							} else {
								color-Write -Magenta "Usage: Remove-Members [AD group] [AD user]";
								socket-send $socket "Usage: Remove-Members [AD group] [AD user]";
							}
						# CMD:
						#	Get-Help
						#		alias: help
						# Info:
						# 	Get commands
						} elseif($cmd[3] -ieq "Help" -or $cmd[3] -ieq "Get-Help") {
							color-write "Command list: ";
							$commandList;
							$out = "";
							foreach($key in $commandList.Keys) {
								$out += "$key`: ";
								$out += $commandList[$key];
								$out += "`n";
							}
							Socket-Send $socket $out;
						} else {
							Color-Write -Red "Command" -nonewline;
							print $cmd[3] -nonewline;
							Color-Write -Red " not found!";
							socket-send $socket "Command not found";
						}
						$failed = $false;
					}
				}
				if($failed -eq $true) {
					taskbar-error
					socket-send $socket "error!";
					try {
						$global:auth.Remove($clientaddr)
					} catch {}
					return $false;
				}
			} else {
				Color-Write -Red "Command" -nonewline;
				print $cmd[0] -nonewline;
				Color-Write -Red " not found!";
				taskbar-error
				socket-send $socket "Command not found";
			}
		}# else {
		#	Color-write -Red "Not a valid command :-(`n`n";
		#	taskbar-error
		#	socket-send $socket "error";
		#}
	}
	return $true;
}

function socket-send {
	param($socket,[string]$msg);
	try {
		$encoding = new-object System.Text.UTF8Encoding;
		[int]$bytesSent = $socket.Send($encoding.GetBytes($msg));
	} catch {
		$global:serverRunning = false;
	}
	return;
}

function taskbar-progress {
	param($tb_state,$tb_current=0,$tb_max=100);
	#HELP: $tb_state = @("No Progress","Indeterminate","Normal","error","Paused");
	$TaskBarObject = [Microsoft.WindowsAPICodePack.TaskBar.TaskBarManager]::Instance;
	$TaskBarObject.SetProgressState($tb_state);
	if($tb_state -ne "Indeterminate") { 
		$TaskBarObject.SetProgressValue($tb_current,$tb_max);
	}
}

function taskbar-error {
	taskbar-progress "Error" 100 100
	taskbar-overlayIcon "./error.png" "Error";
}
function taskbar-normal {
	taskbar-progress "Normal" 0 100
	taskbar-overlayIcon "./running.png" "Running";
}

########
# MAIN #
########

[int]$port = 2008;

# CREATE SOCKET
$endpoint = new-object System.Net.IPEndPoint ([ipaddress]::any,$port)
$server = new-object System.Net.Sockets.Socket([System.Net.Sockets.AddressFamily]::InterNetwork, [System.Net.Sockets.SocketType]::Stream, [System.Net.Sockets.ProtocolType]::Tcp);
if($server -eq $null) {
	Color-Write -red "Socket create error!";
	taskbar-progress "Error" 100 100
	return;
}
try {
	$server.Bind($endpoint);
	Color-Write -Cyan "TCP SERVER STARTED," -White " listening on port " -Green "$port`n"
	taskbar-progress "Normal" 100 100
} catch {
	Color-Write -red "Socket bind error!";
	taskbar-error
	return;
}
$server.Listen(1);
$encoding = new-object System.Text.UTF8Encoding
while($true) {
	taskbar-progress "Indeterminate"
	taskbar-overlayIcon "./running.png" "Listening";
	$socket = $server.Accept();
	$global:serverRunning = $true;
	while($global:serverRunning) {
		taskbar-overlayIcon "./connected.png" "Connected";
		taskbar-progress "Indeterminate"
		$buffer = new-object System.Byte[] 65536;
		try {
			[int]$k = $socket.Receive($buffer);
		} catch {
			taskbar-error
			$global:serverRunning = $false;
		}
		taskbar-overlayIcon "./exec.png" "Executing";
		[string]$out = "";
		for($i=0;$i -lt $k;$i++) {
			$out += [System.Convert]::ToChar($buffer[$i]);
		}
		color-write -White "`nReceived command: " -Red $out;
		$clientaddr = $socket.RemoteEndPoint.get_Address();
		$result = runcommand $out $clientaddr $socket;
		if($result -eq $false) { break; }
	}
}
$socket.Close();