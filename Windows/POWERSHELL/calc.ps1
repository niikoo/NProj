#Set aliases
Set-Alias wh Write-Host

#First: Launch calc
calc

#Check responding
do {}
While (get-process calc | select -Property Responding)

cls

#Display end-time
wh "calc.exe failed to respond on: " -ForegroundColor red
$date = Get-Date
wh $date -ForegroundColor red
wh `n

#Quit application
$strResponse = “Quit”
do {$strResponse = Read-Host “Are you sure you want to quit application? (Y/N)”}
until ($strResponse -ieq “Y”)
