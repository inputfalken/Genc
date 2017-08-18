param ([string] $type)
[System.IO.FileSystemInfo] $project = gi '.\src\Genc\Genc.csproj'
[xml] $doc = Get-Content $project
[version] $version ="$($doc.Project.PropertyGroup.VersionPrefix).0"

Function Major-Increment {
  return [string] "$($version.Major + 1).0.0"
}

Function Minor-Increment {
  return [string] "$($version.Major).$($version.Minor + 1).0"
}

Function Patch-Increment {
  return [string] "$($version.Major).$($version.Minor).$($version.Build + 1)"
}


switch ($type) {
  'major' {
    $doc.Project.PropertyGroup.VersionPrefix = Major-Increment
  }
  'minor' {
    $doc.Project.PropertyGroup.VersionPrefix = Minor-Increment
  }
  'patch' {
    $doc.Project.PropertyGroup.VersionPrefix = Patch-Increment
  }
  default {
    [string] $validArgs = 'valid arguments are: major, minor and patch'
    if ([string]::IsNullOrEmpty($type)) {
      Write-Host "Argument required, $validArgs" -Foregroundcolor Yellow
    } else {
      Write-Host "'$type' is an invalid argument, $validArgs" -Foregroundcolor Yellow
    }
    exit
  }
}
$doc.Save($project.FullName)
