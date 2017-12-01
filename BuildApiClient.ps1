
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
    $configFiles=get-childitem . apiclient.config -Exclude Nuget.config, packages.config -rec
    foreach ($file in $configFiles)
    {
        Write-Output "Config file name is $file"
        # MakeFileWritable $file
        # get the full path and file name of the App.config file in the same directory as this script
        # initialize the xml object
        $apiClientConfig = New-Object XML
        # load the config file as an xml object
        $apiClientConfig.Load($file)

        foreach($n in $apiClientConfig.configuration.appSettings.add)
        {
            switch($n.key)
            {
                "ApiClient.ClientId" { $n.value = "CHANGE_ME" }
                "ApiClient.ClientSecret" { $n.value = "CHANGE_ME" }
                "ApiClient.RedirectUri" { $n.value = "CHANGE_ME" }
                "ApiClient.AccessToken" { $n.value = "" }
                "ApiClient.RefreshToken" { $n.value = "" }
                "ApiClient.ExpirationDateTime" { $n.value = "" }
            }
        }
        $apiClientConfig.Save($file)
    }
}

$from = $baseDir
$to = CombinePaths $dir "ApiClientSource"
$CreateZipFileDir = CombinePaths $from "CreateZipFile"
$exclude = @('CreateZipFile', "_Resharper.Caches", 'ApiClientSource', '.git', '.vs')

$zipFilename = 'ApiClientSource.7z'
$compressor = "c:\Program Files\7-Zip\7z.exe"

RemoveFile $zipFilename

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
cmd /c $compressor a -t7z  $zipFilename "ApiClientSource" -r 

RemoveFolder $to
