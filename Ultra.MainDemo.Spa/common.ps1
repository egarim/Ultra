function Get-Bin-Directory() {
	$projectFile = (get-childitem . -recurse -filter *.csproj).FullName
	$xmlProjFile = [xml](Get-Content $projectFile)
	$outputPath = ($xmlProjFile.Project.PropertyGroup | ? Condition -Like '*Debug*').OutputPath
	$targetFramework = $xmlProjFile.Project.PropertyGroup.TargetFramework[0]	
	return "$($outputPath)\$($targetFramework)"
}

function Get-Spa-Assembly-Name() {
	return "DevExpress.ExpressApp.Spa.v18.2"
}

function Get-Spa-Assembly-Local-Path() {
	$binDirectory = Get-Bin-Directory
	$spaAssemblyName = Get-Spa-Assembly-Name
	return "$($binDirectory)\$($spaAssemblyName).dll"
}

function Calculate-Md5-Hash($path) {
	$md5 = New-Object -TypeName System.Security.Cryptography.MD5CryptoServiceProvider
	$hash = [System.BitConverter]::ToString($md5.ComputeHash([System.IO.File]::ReadAllBytes($path)))
	return $hash.Replace("-", "")
}