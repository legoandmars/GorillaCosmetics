curl -L https://github.com/legoandmars/GorillaCosmetics/releases/download/v3.0.0/GorillaCosmetics-3.0.0.zip -o GC.zip
Expand-Archive GC.zip 
rm GC.zip
dotnet build -c Release -o Temp
cp Temp\GorillaCosmetics.dll GC\BepInEx\plugins\GorillaCosmetics\GorillaCosmetics.dll
rmdir Temp -Recurse
Compress-Archive GC\BepInEx\ GorillaCosmetics-v.zip 
rmdir GC -Recurse
