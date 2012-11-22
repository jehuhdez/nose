#
#		IronCow.Posh Uninstallation Script
#
#	Runs InstallUtil.exe on IronCow.Posh to uninstall it.
#
$icp = (Get-PSSnapIn -Registered "IronCowPosh" -ErrorAction SilentlyContinue)
if ($icp -ne $null) {
	if ((Test-Path IronCow.Posh.dll) -ne $true) {
		throw "Please run this script in the same directory as IronCow.Posh.dll"
	}
	
	Write-Output "Uninstalling IronCow.Posh..."
	Set-Alias installutil "$env:windir\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe"
	installutil /u IronCow.Posh.dll
	if ($? -ne $true) {
		throw "An error occured while trying to uninstall IronCow.Posh."
	}
} else {
	Write-Output "IronCow.Posh wasn't registered anyway."
}
