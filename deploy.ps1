# Pack package to root directory of project and returns the file.
function Pack ([string] $project, [bool] $isBeta) {
  if ($isBeta) {
    dotnet pack $project --version-suffix beta -c Release -o ..\
  } else {
    dotnet pack $project -c Release -o ..\
  }
  if (!$?) {
    throw "$project could not be packed by command 'dotnet pack'."
  }
  return [System.IO.FileSystemInfo] (Get-ChildItem *.nupkg | select -First 1)
}

# Deploy package to NuGet.
function Deploy ([string] $package) {
  dotnet nuget push $package -k $env:NUGET_API_KEY -s 'https://www.nuget.org/api/v2/package'
}

# If returns true if the branch is develop and false if it's master.
function Is-beta([string] $branch) {
  switch ($branch) {
    "develop" {
      Write-Host "Proceeding script with beta version for branch: $branch." -ForegroundColor yellow
      return 1
    }
    "master" {
      Write-Host "Proceeding script with stable version for branch: $branch." -ForegroundColor yellow
      return 0
    }
    default {
      Write-Host "$branch is not a deployable branch exiting..." -ForegroundColor yellow
      exit
    }
  }
}
# Fetch the online version
function Fetch-OnlineVersion ([string] $listSource, [string] $projectName, [bool] $isBeta) {
  Write-Host "Fetching NuGet version from $listSource" -ForegroundColor yellow
  # Use beta version if the current branch is develop.
  if ($isBeta) {
    $packageName = NuGet list $projectName -PreRelease -Source $listSource
  } else {
    $packageName = NuGet list $projectName -Source $listSource
  }
  # $packageName comes in format: "packageName 1.0.0".
  $version = ($packageName.Split(" ") | Select-Object -Last 1)
  # In beta version the version also includes the string "version-beta" where version is the semver.
  if ($isBeta) {
    $version = ($version | select -Last 1).Split("-") | select -First 1
    # A hack to get set the revision property to zero.
    $version = "$version.0"
  }
  return [version] $version
}

function Get-LocalVersion ([string] $project) {
  [string] $versionNodeValue = ((Select-Xml -Path $project -XPath '//VersionPrefix') | select -ExpandProperty node).InnerText
  $version = "$versionNodeValue.0"
  return [version] $version
}


$project = '.\Genc\Genc.csproj'
$branch = $env:APPVEYOR_REPO_BRANCH
$isBeta = Is-beta $branch
[version] $onlineVersion = Fetch-OnlineVersion 'https://nuget.org/api/v2/' 'Genc' $isBeta
[version] $localVersion = Get-LocalVersion .\Genc\Genc.csproj

if ($localVersion -gt $onlineVersion) {
  Deploy (Pack $project $isBeta).Name
} else {
  Write-Host "Local version($localVersion) is not greater than online version($onlineVersion)"
}
