


function CleanupFiles()
{
  get-childitem . -include *.vssscc,*.user,*.vspscc,
    *.v12.suo,debug,packages,bin,obj -recurse -force |   
      %{   
        write-output "Removing $_"
        remove-item $_.fullname -force -recurse   
      } 

}

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

CleanupFiles
RemoveTfsBindingFromSolutionFile
RemoveTfsBindingFromCSProjFile
