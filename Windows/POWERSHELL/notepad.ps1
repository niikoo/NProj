#Set aliases
Set-Alias wh Write-Host

#include
#.{Functions.ps1}

#First: Launch calc
calc

#Check responding
do {}
While (get-process calc | select -Property Responding)

cls

#Display end-time
wh "calc.exe failed to respond on: " -ForegroundColor red
Function Time {
	return Get-Date
}
wh Time -ForegroundColor red
wh `n

#Quit application
$strResponse = “Quit”
do {$strResponse = Read-Host “Are you sure you want to quit application? (Y/N)”}
until ($strResponse -ieq “Y”)