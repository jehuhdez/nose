#
#		IronCow.Posh Installation Script
#
#	Runs InstallUtil.exe on IronCow.Posh if it hasn't been installed yet.
#	Installs it as a PSSnapIn if it hasn't been done yet.
#	Updates the display format information.
#
$icp = (Get-PSSnapIn -Registered "IronCowPosh" -ErrorAction SilentlyContinue)
if ($icp -eq $null) {
	if ((Test-Path IronCow.Posh.dll) -ne $true) {
		throw "Please run this script in the same directory as IronCow.Posh.dll"
	}
	
	Write-Output "Installing IronCow.Posh..."
	Set-Alias installutil "$env:windir\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe"
	installutil IronCow.Posh.dll
	if ($? -ne $true) {
		throw "An error occured while trying to install IronCow.Posh."
	}
}

$icp = (Get-PSSnapIn | Where-Object { $_.Name -eq "IronCowPosh" })
if ($icp -eq $null) {
	Write-Output "Adding IronCow.Posh add-in to the current console..."
	Add-PSSnapIn IronCowPosh
	if ($? -ne $true) {
		throw "An error occured while trying to load IronCow.Posh into the current console."
	}
}

$fd = (Join-Path ((Get-PSSnapin IronCowPosh).ApplicationBase) IronCow.Posh.format.ps1xml)
Write-Output ("Updating format data with " + (Split-Path $fd -Leaf) + "...")
Update-FormatData $fd

Write-Output "IronCow.Posh is now installed in your console."
Write-Output "If you like it, you can add the Add-PSSnapIn statement to your profile so IronCow.Posh is enable everytime you start Powershell."
Write-Output "Before getting started, you need to authenticate IronCow.Posh with Remember The Milk, if you haven't done so yet. See AuthentiationScript.ps1."
