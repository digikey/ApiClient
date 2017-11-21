

# Script includes
 . ".\includes.ps1"


#  Initialize some variables
$ProjectDir = "../"
[IO.Directory]::SetCurrentDirectory((Convert-Path (Get-Location -PSProvider FileSystem)))
$dir            = [IO.Directory]::GetCurrentDirectory() 
$baseDir        = Split-path -parent $dir



#
# Cleanup directories and files that Visual Studio/msbuild creates, also cleanup NuGet packages
#
function CleanupFiles()
{
  get-childitem . -include *.vssscc,*.user,*.vspscc,
    *.v12.suo,debug,packages,bin,obj -recurse -force |   
      %{   
        write-output "Removing $_"
        remove-item $_.fullname -force -recurse   
      } 

}


#
# This function will thr TFS information from the solution files undeer the current directory
#
function RemoveTfsBindingFromSolutionFile()
{
  get-childitem . -include *.sln -recurse |   
    %{   
      $file = $_;   
      $inVCSection = $False;  
      get-content $file |   
      %{   
        $line = $_.Trim();   
        if ($inVCSection -eq $False -and $line.StartsWith('GlobalSection') -eq $True -and $line.Contains('VersionControl') -eq $True) {   
          $inVCSection = $True   
        }   
        if ($inVCSection -eq $False) {   
          add-content ($file.fullname + '.new') $_   
        }   
        if ($inVCSection -eq $True -and $line -eq 'EndGlobalSection') {   
          $inVCSection = $False  
        }  
      }  
      mv ($file.fullname + '.new') $file.fullname -force   
    } 
}


#
# This function will thr TFS information from the .csproj files undeer the current directory
#
function RemoveTfsBindingFromCSProjFile()
{
  get-childitem . -include *.csproj -recurse |   
    %{   
      $file = $_;   
      get-content $file |   
      %{   
        $line = $_.Trim();   
        if ($line.StartsWith('<Scc') -eq $False) {  
          add-content ($file.fullname + '.new') $_   
        }  
      }  
      mv ($file.fullname + '.new') $file.fullname -force   
    }
}


#
# Find the .config files that matter under the current directory and cleanup up the 
# values that have private information.
#
function CleanupConfigFiles()
{
    $configFiles=get-childitem . *.config -Exclude Nuget.config, packages.config -rec
    foreach ($file in $configFiles)
    {
        Write-Output "Config file name is $file"
        MakeFileWritable $file
		$xml = ( Select-Xml -Path $file -XPath /).Node

        foreach($n in $xml.configuration.appSettings.add)
        {
			Write-Output "name is $n"
            switch($n.key)
            {
                "OAuth.ClientId" { $n.value = "CHANGE_ME" }
                "OAuth.ClientSecret" { $n.value = "CHANGE_ME" }
                "OAuth.AccessToken" { $n.value = "" }
                "OAuth.RefreshToken" { $n.value = "" }
                "DigiKeyOAuthConsumerKey" { $n.value = "CHANGE_ME" }
                "DigiKeyOAuthConsumerSecret" { $n.value = "CHANGE_ME" }
            }
        }

        foreach($n in $xml.configuration.userSettings.'DKServicesWinFormsApp.Properties.Settings'.setting)
        {
			Write-Output "name is $n"
            switch($n.name)
            {
                "CustomerID" { $n.value = "" }
                "PartnerID" { $n.value = "CHANGE_ME" }
                "Password" { $n.value = "" }
            }
        }
        $xml.Save($file)
    }
}


$from = $baseDir
$to = CombinePaths $dir "DKServicesExamples"
$CreateZipFileDir = CombinePaths $from "CreateZipFile"
$exclude = @('CreateZipFile', 'Docs', 'WCFConsoleApp', 'DKServicesProd')

$zipFilename = 'DKServicesExamples.zip'
$compressor = "c:\Program Files\7-Zip\7z.exe"

RemoveFolder $to

Copy-ToCreateFolder $from $to $exclude -Recurse

# Move to newly created copy of DKServiceExample
set-location -Path $to

CleanupFiles
# RemoveTfsBindingFromSolutionFile
# RemoveTfsBindingFromCSProjFile
# CleanupConfigFiles

# Move back to starting directory
# set-location -Path $dir  

# RemoveFile $zipFilename
# cmd /c $compressor a -tzip  $zipFilename "DKServicesExamples" -r 

# cmd /c copy $zipname $deployPath /Y


