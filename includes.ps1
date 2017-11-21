
function Get-SvnRevision($dir) 
{ 
  ([xml](svn info $dir --xml)).info.entry.revision 
}


function RunMsBuild([string]$solutionPath,
                    [string]$platform = "x86")
{
  try {
    "msbuild $solutionPath $platform"
    msbuild /p:Configuration=Release  /p:Platform="$platform" $solutionPath /t:rebuild
  } catch [Exception] {
    throw $_.Message
    Send-Error
  }
  if ($LastExitCode -ne 0) {
    throw "An error occured during invocation of MsBuild"
    Send-Error
  }
}

function RunDevEnv([string]$solutionPath, 
                   [string]$projectPath, 
                   [string]$devEnvPath, 
                   [string]$tmpDir,
                   [string]$platform = "x86")
{
  $parameters = "/rebuild `"Release|$platform`" ""$solutionPath"" /Project ""$projectPath"" /ProjectConfig Release /Out $tmpDir/devenv.log"
  "Process to start [$devEnvPath $parameters]"
  try {
    $process = Start-Process -FilePath $devEnvPath -ArgumentList $parameters -Wait -PassThru
    Write-Host "Exit Code is " $process.ExitCode

    if($process.ExitCode -ne 0) {
        throw "devenv process returned error code: $($p.ExitCode)"
    }
  } catch [Exception] {
    throw $_.Message
    Send-Error
  }
  if ($LastExitCode -ne 0) {
    throw "An error occured during invocation of devenv"
    Send-Error
  }
}




[char[]]$trimChars = '/'
function FixTerminatingSlash ($root) {
    return $root.TrimEnd($trimChars)   
}

function FixStartingSlash($suffix) {
    return $suffix.TrimStart($trimChars)
}

function CombinePaths ([string]$root, [string]$subdir) {
    $root   = $root.Replace("\", "/")
    $subdir = $subdir.Replace("\", "/")
    $left = FixTerminatingSlash($root)
    $right = FixStartingSlash($subdir)
    $fullPath = [System.IO.Path]::Combine($left, $right)
    return $fullPath.Replace("\", "/")
}

function RemoveFile($file)
{
  "Removing file $file"
  if (test-path $file) {
    remove-item $file -force
  }
}
function RemoveFolder($path)
{
  if (test-path $path) {
      remove-item $path -force -recurse 
  }
}

function CreateFolder($path)
{
  "Creating folder $path"
  if (!(Test-Path -path $path)) {
    new-item -path $path -type directory | out-null
  }
}

function GetFileName($full_path){
  return [System.IO.Path]::GetFileName($full_path)
}

function MakeFileWritable($file)
{
  if (test-path $file) {
    Get-Item -Path $file | Set-ItemProperty -Name IsReadOnly -Value $false  
  }
}

#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs
#-------------------------------------------------------------------------------
function Update-CommonAssemblyInfoFiles
{
  param([string] $version5x, [string] $version4x) 

  $assemblyVersionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
  $fileVersionPattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
  $infoVersionPattern = 'AssemblyInformationalVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
  $assemblyVersion = 'AssemblyVersion("' + $version5x + '")';
  $fileVersion = 'AssemblyFileVersion("' + $version5x + '")';
  $infoVersion = 'AssemblyInformationalVersion("' + $version4x + '")';
    
  Get-ChildItem -r -filter CommonAssemblyInfo.cs | ForEach-Object {
     $filename = $_.Directory.ToString() + '\' + $_.Name
     $filename + ' -> ' + $version5x  + ' -> ' + $version4x
        
     # If you are using a source control that requires to check-out files before 
     # modifying them, make sure to check-out the file here.
     # For example, TFS will require the following command:
     # tf checkout $filename
    
     (Get-Content $filename) | ForEach-Object {
         % {$_ -replace $assemblyVersionPattern, $assemblyVersion } |
         % {$_ -replace $infoVersionPattern, $infoVersion } |
         % {$_ -replace $fileVersionPattern, $fileVersion }
     } | Set-Content $filename
  }
}

function Copy-ToCreateFolder
{
    param(
        [string]$src,
        [string]$dest,
        $exclude,
        [switch]$Recurse
    )
    
    # The promlem with Copy-Item -Rec -Exclude is that -exclude effects only top-level files
    # Copy-Item $src $dest    -Exclude $exclude       -EA silentlycontinue -Recurse:$recurse
    # http://stackoverflow.com/questions/731752/exclude-list-in-powershell-copy-item-does-not-appear-to-be-working
    
    if (Test-Path($src))
    {
        # nonstandard: I create destination directories on the fly
        [void](New-Item $dest -itemtype directory -EA silentlycontinue )
        Get-ChildItem -Path $src -Force -exclude $exclude | % {
            
            if ($_.psIsContainer)
            {
                if ($Recurse) # non standard: I don't want to copy empty directories
                {
                    $sub = $_
                    $p = Split-path $sub
                    $currentfolder = Split-Path $sub -leaf
                    #Get-ChildItem $_ -rec -name  -exclude $exclude -Force | % {  "{0}    {1}" -f $p, "$currentfolder\$_" }
                    [void](New-item $dest\$currentfolder -type directory -ea silentlycontinue)
                    Get-ChildItem $_ -Recurse:$Recurse -name  -exclude $exclude -Force | % {  Copy-item $sub\$_ $dest\$currentfolder\$_ }
                }
            }
            else
            {
                
                #"{0}    {1}" -f (split-path $_.fullname), (split-path $_.fullname -leaf)
                Copy-Item $_ $dest
            }
        }
    }
}
