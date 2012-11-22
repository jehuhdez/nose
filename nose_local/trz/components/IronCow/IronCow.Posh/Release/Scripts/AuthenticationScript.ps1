#
#	IronCow.Posh Authentication Script
#
#	Authenticates IronCow.Posh with Remember The Milk so that the
#	authentication token is saved to the current user's settings.
#
$authInfo = (Get-RtmAuthenticationUrl -StartUrl)
Read-Host -Prompt "Press enter after you've authenticated IronCow.Posh in your browser"
$token = (Get-RtmAuthenticationToken -Frob $authInfo.Frob -Save)
if ($? -eq $true) {
	Write-Output "Successfully authenticated IronCow.Posh!"
	Write-Output "You can now start using the following commands:"
	$snapin = (Get-PSSnapIn IronCowPosh)
	Get-Command | Where-Object{ $_.PSSnapIn -eq $snapin } | Format-Table Name,Definition
} else {
	Write-Error "An error occured while trying to authenticate IronCow.Posh"
}
