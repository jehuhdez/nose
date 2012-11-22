
=========================================================
					IronCow.Posh

			  IRON COW for POWERSHELL
=========================================================



IronCow.Posh is a snap-in for Powershell[1][2], Microsoft's .NET
shell. It adds a few Cmdlets that let you manage your Remember
The Milk[3] tasks through the command-line, like the l337 hacker
your are.



1. INSTALLING
=========================================================
Copy the binaries to a place you like. You should have at least
the following files:

	* IronCow.dll
	* IronCow.Posh.dll
	* IronCow.Posh.dll-Help.xml
	* IronCow.Posh.format.ps1xml
	* Scripts [folder]
		* AuthenticationScript.ps1
		* InstallScript.ps1
		* UninstallScript.ps1

Go into the directory containing the DLLs and run the install
script:

> & Scripts\InstallScript.ps1

If all goes well, run the authentication script so that IronCow.Posh
knows how to log-in to your RTM account:

> & Scripts\AuthenticationScript.ps1

If everything's still good, you're good to go. IronCow.Posh has
by now stored a token given by RTM so that you won't have to 
authenticate again in the future. You can always revoke IronCow.Posh's
access to your RTM account by going on the RTM website and removing
the IronCow application from your settings.



2. CONFIGURING
=========================================================
If you quit and rerun Powershell, you'll realize IronCow.Posh isn't
loaded. You'll need to add a few lines to your profile script to
load IronCow.Posh if you wish to. See the Powershell documentation for
more information.

Here's the relevant lines in my profile script:

Add-PSSnapIn IronCowPosh
if ($? -eq $true) {
	$ironCowPoshFormatData = (Join-Path ((Get-PSSnapin IronCowPosh).ApplicationBase) IronCow.Posh.format.ps1xml)
	Update-FormatData $ironCowPoshFormatData
	
It adds IronCow.Posh to your current session, and updates the format data
so that your RTM tasks show up nicely (the default output format is not
super appropriate).



3. UNINSTALLING
=========================================================
If you want to uninstall IronCow.Posh, run the UninstallScript.ps1 
from the Scripts directory.



Thanks!



A. REFERENCES
=========================================================
[1] http://en.wikipedia.org/wiki/Windows_PowerShell
[2] http://www.microsoft.com/windowsserver2003/technologies/management/powershell/default.mspx
[3] http://www.rememberthemilk.com
