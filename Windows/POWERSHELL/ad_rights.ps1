##################
# AD-RETTIGHETER #
# av nom1 @ IRIS #
##################

# SET-ALIAS
Set-Alias print Write-Host;

# TRY TO GET AND TRIM ARGUMENTS
try {
	$argv = @("");
	for($i=0;$i -lt $argv.count;$i++) {
		$argv[$i] = $args[$i].Trim('');
	}
} catch {}

# TEST ARGUMENT INDEX 0 PATH
if($argv[0]) {
	if(!(Test-Path -Path $argv[0] -PathType Container)) {
		# DIRECTORY DOESN'T EXITS -> EXIT
		print "The directory" $argv[0] "doesn't exist";
		break;
	}
} else {
	# NO PATH SPECIFIED IN ARGUMENT
	print "No path specified in the argument (pos 0)";
	break;
}
$path = $argv;
$argv = $null;

#-----------#
# FUNCTIONS #
#-----------#

# DISPLAY MENU
Function DisplayMenu {
	print "|------|"`n"| Meny |"`n"|------|"`n -ForegroundColor Cyan
	$menu = @(
				"IKKE RØR DENNE",
				"Vis direkte rettigheter",
				"Vis arvede rettigheter",
				"Sett rettigheter",
				"Legg til / fjern personer i gruppe"
			);

	for($i=1;$i -lt $menu.Count; $i++) {
		print "$i -"$menu[$i];
	}
	print "0 - Avslutt"`n
	$mic = $menu.Count - 1; # antall menu-items
	do {
		$menyvalg = Read-Host "Skriv inn et valg (1-$mic)"
	} until($menyvalg -ge 0 -and $menyvalg -le $mic)
	return [int]$menyvalg;
}

# START / RESTART SCRIPT
Function StartIT {
	# CLEAR SCREEN
	Clear-Host;
	# DISPLAY VALGT MAPPE
	print "Valgt mappe:" $path `n -ForegroundColor Green;
	# DISPLAY MENY
	$menyvalg = DisplayMenu;
	return $menyvalg;
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

# PRINT NICE ACL LIST
Function NiceACLList {
	param($hash_table,[bool]$only_header=$false); # F - FullControl, M - Modify...
	[string]$output = "";
	if($only_header -eq $true) {
		$keyCol = $hash_table.Keys;
		foreach($s in $keyCol) {
			$output += " $s  ";
		}
		return $output;
	}
	$hash_table.GetEnumerator() | ForEach-Object {
		$output += "[";
		if($rights.indexOf($_.Value) -ne -1) { $output += "X" } else { $output += " "; }
		$output += "] ";
	}
	return $output;
}

# SPACE GENERATOR
Function SpaceGen {
	Param([int]$num);
	[string]$out = "";
	for($i=0;$i -lt $num;$i++) {
		$out += " ";
	}
	return $out;
}

# GETACL
Function GetACLS {
	param($inherited=$false,$notinherited=$true);
	if($inherited -eq $true -and $notinherited -eq $true) {
		$acl_access = (Get-Acl -Path $path).access;
	} elseif($inherited -eq $true -and $notinherited -eq $false) {
		$acl_access = ((Get-Acl -Path $path).access | Where-Object {$_.IsInherited -eq $true});
	} elseif($inherited -eq $false -and $notinherited -eq $true) {
		$acl_access = ((Get-Acl -Path $path).access | Where-Object {$_.IsInherited -eq $false});
	} else {
		Color-Write -Red "Error!";
		return;
	}
	$mxl = 10;
	foreach($acc in $acl_access) {
		if($acc.IdentityReference.Value.Length -gt $mxl) {
			$mxl = $acc.IdentityReference.Value.Length + 1;
		}
	}
	$nacll = @{
				"F" = "FullControl";
				"M" = "Modify";
				"X" = "ReadAndExecute";
				"S" = "Synchronize";
			};
	#PRINT HEADER
	$out = NiceACLList $nacll $true;
	$space = SpaceGen $mxl;
	Color-Write $space -Yellow $out;
	#PRINT ROWS
	foreach($acc in $acl_access) {
		[string]$rights = $acc.FileSystemRights;
		$rights = $rights.trim(" ").split(",");
		#Full
		$out = NiceACLList $nacll $false;
		$space = SpaceGen ($mxl - $acc.IdentityReference.Value.Length -1);
		Color-Write $acc.IdentityReference.Value $space $out;
	}
	echo $acl;
}

#------#
# MAIN #
#------#

do {
	$menyvalg = StartIT;
	switch($menyvalg) {
		1 {
			$acls = GetACLS $false $true;
			echo $acls;
		}
		2 {
			$acls = GetACLS $true $false;
			echo $acls;
		}
		3 {
			$ACL = Get-Acl C:\folder
			Set-Privilege (new-object Pscx.Interop.TokenPrivilege "SeRestorePrivilege", $true)
			$ACL.SetOwner((new-object System.Security.Principal.NTAccount("FormerUser")))
			$ACL | Set-Acl C:\folder
		}
		4 {
		
		}
		default {}
	}
	if(!($menyvalg -eq 0)) {
		$menyvalg = Read-Host `n"Skriv 0 for å avslutte, press enter for å gå til menyen";
	}
} until($menyvalg -eq 0);
break; # THEN EXIT :)