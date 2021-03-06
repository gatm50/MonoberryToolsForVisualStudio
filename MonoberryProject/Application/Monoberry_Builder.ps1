## Monoberry_Builder.ps1: Create and install a .bar package for Blackberry 10 OS Simulator
#### Authors:
##   Gustavo Torrico (gatm50@gmail.com)
#### Licensed under the terms of the Microsoft Public License (MS-PL)
#### Copyright 2012 Cup-Coffee, ( http://cup-coffe.blogspot.com )
##

################
##### Environment Variables
$descriptor_path='monoberry-descriptor.xml';
$deploy='Deploy';
$temp='tmp';
$locale='tmp\locale';
$exe='$sdkPath$\win32\x86\usr\bin\blackberry-nativepackager.bat';
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
 
function CreatePackage()  
{  
	param (
		[string]$exe = $(Throw "An executable must be specified"), 	
		[string] $scriptPath = $(Throw "A Script Path must be specified"),
		[string] $barPath = $(Throw "A Script Path must be specified")
		)
		
	$completePath = [System.IO.Path]::Combine([System.IO.Path]::GetDirectoryName($scriptPath), "");
	$rootPath=[System.IO.Directory]::GetParent($completePath);
	$descriptor_path=[System.IO.Path]::Combine($rootPath, $descriptor_path);
	$deploy=[System.IO.Path]::Combine($rootPath, $deploy);
	$temp=[System.IO.Path]::Combine($rootPath, $temp);
	$locale=[System.IO.Path]::Combine($rootPath, $locale);
	$barPath=[System.IO.Path]::Combine($rootPath, $barPath);
	
	[System.IO.Directory]::CreateDirectory($temp);
	[System.IO.Directory]::CreateDirectory($locale);
	[System.IO.Directory]::CreateDirectory($deploy);
	
	$arguments= [System.String]::Format('-package "{0}" -devMode "{1}" "{2}" -C "{3}" "{4}"', $barPath, $descriptor_path, $deploy, $temp, $locale);
	RunProcess $exe $arguments;
	
	[System.IO.Directory]::Delete($deploy);
	[System.IO.Directory]::Delete($locale);
	[System.IO.Directory]::Delete($temp);
}

######
## Begin Main Method
######
CreatePackage $exe $scriptPath $barName;
######
## End Main Method
######