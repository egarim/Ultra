. ".\common.ps1"

$spaAssemblyName = Get-Spa-Assembly-Name
$assembly = [Reflection.Assembly]::LoadWithPartialName($spaAssemblyName)

If(!$assembly)
{	
	$spaAssemblyLocalPath = Get-Spa-Assembly-Local-Path
	$assembly = [Reflection.Assembly]::LoadFrom($spaAssemblyLocalPath)
}

$clientDataPath = "build\"
$clientDataFile = "client-data.zip"
$pathInAssembly = "DevExpress.ExpressApp.Spa.Resources."
$fullNameInAssembly = $pathInAssembly + $clientDataFile

$zipFileInLocalFolder = $clientDataPath + $clientDataFile

If(!(test-path $clientDataPath))
{
	New-Item -ItemType Directory -Force -Path $clientDataPath
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
	param([string]$zipfile, [string]$outpath)
	[System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}

function UpdateAndBuild
{
	Get-ChildItem -Path $clientDataPath -Include * -File -Recurse | ? { $_.FullName -inotmatch 'node_modules' } | foreach { $_.Delete()}

	Write-output "Start updating node modules"
	[DevExpress.ExpressApp.Spa.ResourcesHelper]::SaveResourceFromCurrentAssemblyToFile($fullNameInAssembly, $zipFileInLocalFolder)	
	Unzip ($clientDataPath + $clientDataFile) $clientDataPath
	Set-Location "build"
	& "npm" install tarballs\app-player-react.tgz
	& "npm" install tarballs\devextreme-theme.tgz
	& "npm" install tarballs\xaf-app-module.tgz
	& "npm" install
    & "npm" run build
}

$generatedFile = "wwwroot\static\js\main.js"
$filesExist = (test-path $zipFileInLocalFolder) -and (test-path $generatedFile)

Write-output $filesExist

If($filesExist)
{
	$hashInCache = Calculate-Md5-Hash($zipFileInLocalFolder)
	$hashInAssembly = [DevExpress.ExpressApp.Spa.ResourcesHelper]::CalculateMd5HashForResource($fullNameInAssembly)
	Write-output $hashInAssembly
	Write-output $hashInCache
	if($hashInAssembly.equals($hashInCache)) {
		Write-output "Node modules are up to date"		
	} else {
		Write-output "Start updating node modules"				
		UpdateAndBuild
	}
} else {
	UpdateAndBuild
}