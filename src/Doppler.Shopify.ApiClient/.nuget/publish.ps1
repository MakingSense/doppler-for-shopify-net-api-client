Param(
    [string]$assemblyFolder
)

if(-not($assemblyFolder)) {
    $assemblyFolder = '..\Doppler.Shopify.ApiClient\bin\Release\Merge'
}
$packagesDir = '..\Nuget'
$assemblyFile = [io.path]::Combine($PSScriptRoot, $assemblyFolder, 'Doppler.Shopify.ApiClient.dll')

cd $PSScriptRoot
& 'C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe' ..\Doppler.Shopify.ApiClient\Doppler.Shopify.ApiClient.csproj /p:Configuration=Release /p:Publish=True

if ($LastExitCode -ne 0)
{
  Write-Output "Error building Doppler.Shopify.ApiClient library."
}
else
{
  $assembly = [Reflection.Assembly]::LoadFrom($assemblyFile)
  $assemblyVersion =  $assembly.GetName().Version.ToString()
  $packageName = [io.path]::Combine($packagesDir, 'Doppler.Shopify.ApiClient.' + $assemblyVersion + '.nupkg')

  if((Test-Path $packagesDir) -eq 0)
  {
      md $packagesDir
  }

  .\NuGet.exe pack Doppler.Shopify.ApiClient.nuspec -BasePath . -OutputDirectory $packagesDir -Version $assemblyVersion
  .\NuGet.exe push $packageName  -ApiKey 9qesmq566dajasa1frmymimc -Source https://ci.appveyor.com/nuget/makingsense-aspnet/api/v2/package
}

read-Host "Pausing..."
