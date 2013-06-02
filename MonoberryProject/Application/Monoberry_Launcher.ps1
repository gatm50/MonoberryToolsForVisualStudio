## Monoberry_Launcher.ps1: Create and install a .bar package for Blackberry 10 OS Simulator
#### Authors:
##   Gustavo Torrico (gatm50@gmail.com)
#### Licensed under the terms of the Microsoft Public License (MS-PL)
#### Copyright 2012 Cup-Coffee, ( http://cup-coffe.blogspot.com )
##

################
##### Environment Variables
$descriptor_path='monoberry-descriptor.xml';
$deploy='Deploy';
$temp='tmp\';
$locale='tmp\locale';
$exe='$sdkPath$\win32\x86\usr\bin\blackberry-nativepackager.bat';
#$address="192.168.75.135";
$address="$deviceIpAddress$";
$scriptPath = $MyInvocation.mycommand.path;
$barName="$safeprojectname$.bar";

####
## Function that calls the native application
###
function RunProcess()
{
	param (
		[string]$exe = $(Throw "An executable must be specified"), 	
		[string] $arguments = $(Throw "A Script Path must be specified")
		)
	$startInfo= new-Object System.Diagnostics.ProcessStartInfo;  
	$startInfo.FileName = $exe;  
	$startInfo.Arguments=$arguments;  
	$startInfo.RedirectStandardOutput = $true; 
	$startInfo.UseShellExecute = $false;  
	$startinfo.RedirectStandardError = $true;
	$startinfo.WindowStyle = "Normal";
	$process = New-Object System.Diagnostics.Process;  
	$process.StartInfo = $startInfo;  
	$process.Start();  
	$process.WaitForExit();
	$result = $process.StandardOutput.ReadToEnd();
	$result;
}

function InstallPackage()
{
	param (
		[string]$exe = $(Throw "An executable must be specified"), 	
		[string] $scriptPath = $(Throw "A Script Path must be specified"),
		[string] $barPath = $(Throw "A Bar Path must be specified"),
		[string] $address = $(Throw "An Address must be specified")
		)
		
	#The Script will fail if $scriptPath contains spaces.
	$completePath = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($scriptPath), "");
	$rootPath=[System.IO.Directory]::GetParent($completePath);
	$barPath=[System.IO.Path]::Combine($rootPath, $barPath);
	
	$arguments= [System.String]::Format('-installApp -device {0} "{1}"', $address, $barPath);
	RunProcess $exe $arguments;
}

function LaunchApplication()
{
	param (
		[string]$exe = $(Throw "An executable must be specified"), 	
		[string] $scriptPath = $(Throw "A Script Path must be specified"),
		[string] $barPath = $(Throw "A Bar Path must be specified"),
		[string] $address = $(Throw "An Address must be specified")
		)
		
	#The Script will fail if $scriptPath contains spaces.
	$completePath = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($scriptPath), "");
	$rootPath=[System.IO.Directory]::GetParent($completePath);
	$barPath=[System.IO.Path]::Combine($rootPath, $barPath);
	
	$arguments= [System.String]::Format('-launchApp -device {0} "{1}"', $address, $barPath);
	RunProcess $exe $arguments;
}

######
## Begin Main Method
######
if( Test-Connection -ComputerName $address -Count 1 -Quiet)
{
	InstallPackage $exe $scriptPath $barName $address;
	LaunchApplication $exe $scriptPath $barName $address;
}
else
{
	echo "The host" + $address + "is unrechable";
}
######
## End Main Method
######