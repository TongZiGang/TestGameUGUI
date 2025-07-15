set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t client ^
    -c cs-bin ^
    -d bin  ^
    --conf luban.conf ^
    -x outputCodeDir=output/Gen ^
    -x outputDataDir=output/bytes ^
 
 
rmdir /S /Q "..\..\UnityGame\Assets\Scripts\GameLogic\Export\Config"
mkdir "..\..\UnityGame\Assets\Scripts\GameLogic\Export\Config"
rmdir /S /Q "..\..\UnityGame\Assets\AssetBundle\Config"
mkdir "..\..\UnityGame\Assets\AssetBundle\Config"
xcopy output\Gen ..\..\UnityGame\Assets\Scripts\GameLogic\Export\Config /E /I /Y
xcopy output\bytes ..\..\UnityGame\Assets\AssetBundle\Config /E /I /Y
  
pause