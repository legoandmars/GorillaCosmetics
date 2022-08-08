# Needs to be at least that version, or mmm can't read the archive
#Requires -Modules @{ ModuleName="Microsoft.PowerShell.Archive"; ModuleVersion="1.2.3" }
$MyInvocation.MyCommand.Path | Split-Path | Push-Location # Run from this script's directory
curl -L https://github.com/legoandmars/GorillaCosmetics/releases/download/v3.0.0/GorillaCosmetics-3.0.0.zip -o GC.zip
Expand-Archive GC.zip 
rm GC.zip
dotnet build -c Release -o Temp
cp Temp\GorillaCosmetics.dll GC\BepInEx\plugins\GorillaCosmetics\GorillaCosmetics.dll
rmdir Temp -Recurse
Compress-Archive GC\BepInEx\ GorillaCosmetics-v.zip 
rmdir GC -Recurse
