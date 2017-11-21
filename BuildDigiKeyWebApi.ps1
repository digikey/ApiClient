
# Script includes
 . ".\includes.ps1"


#  Initialize some variables
[IO.Directory]::SetCurrentDirectory((Convert-Path (Get-Location -PSProvider FileSystem)))
$dir            = [IO.Directory]::GetCurrentDirectory() 
$baseDir        = $dir



#
# Cleanup directories and files that Visual Studio/msbuild creates, also cleanup NuGet packages
#
function CleanupFiles()
{
  get-childitem . -include *.vssscc,*.user,*.vspscc,
    *.v12.suo,debug,packages,bin,obj,.git*,*.ps1 -recurse -force |   
      %{   
        write-output "Removing $_"
        remove-item $_.fullname -force -recurse   
      } 

}

#
# Find the .config files that matter under the current directory and cleanup up the 
# values that have private information.
#
function CleanupConfigFiles()
{
    $configFiles=get-childitem . webapi.config -Exclude Nuget.config, packages.config -rec
    foreach ($file in $configFiles)
    {
        Write-Output "Config file name is $file"
        # MakeFileWritable $file
        # get the full path and file name of the App.config file in the same directory as this script
        # initialize the xml object
        $webApiConfig = New-Object XML
        # load the config file as an xml object
        $webApiConfig.Load($file)

        foreach($n in $webApiConfig.configuration.appSettings.add)
        {
			Write-Output "name is $n.key"
            switch($n.key)
            {
                "WebApi.ClientId" { $n.value = "CHANGE_ME" }
                "WebApi.ClientSecret" { $n.value = "CHANGE_ME" }
                "WebApi.RedirectUri" { $n.value = "CHANGE_ME" }
                "WebApi.AccessToken" { $n.value = "" }
                "WebApi.RefreshToken" { $n.value = "" }
                "WebApi.ExpirationDateTime" { $n.value = "" }
            }
        }
        $webApiConfig.Save($file)
    }
}

$from = $baseDir
$to = CombinePaths $dir "DigiKeyApi"
$CreateZipFileDir = CombinePaths $from "CreateZipFile"
$exclude = @('CreateZipFile', "_Resharper.Caches", 'DigiKeyApi', '.git', '.vs')

$zipFilename = 'DigiKey.Api.zip'
$compressor = "c:\Program Files\7-Zip\7z.exe"

write-Host "From directory is $from"
write-Host "to directory is $to"
RemoveFolder $to

Copy-ToCreateFolder $from $to $exclude -Recurse

# Move to newly created copy of DKServiceExample
set-location -Path $to

CleanupFiles
CleanupConfigFiles

# Move back to starting directory
set-location -Path $dir  

# RemoveFile $zipFilename
cmd /c $compressor a -tzip  $zipFilename "DigiKeyApi" -r 

# cmd /c copy $zipname $deployPath /Y


