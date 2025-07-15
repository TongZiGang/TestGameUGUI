@echo off
setlocal enabledelayedexpansion

:: 输入目标文件夹路径
set "baseDir=Protobuf"
set "path=ProtobufCompiler/bin"

:: 转换为绝对路径并确保以反斜杠结尾
for %%I in ("%baseDir%") do set "baseDirAbs=%%~fI\"

:: 递归遍历所有文件并输出包含父目录名的相对路径
for /r "%baseDirAbs%" %%F in (*) do (
    set "absPath=%%F"
    set "relPath=!absPath:%baseDirAbs%=!"
    echo %baseDir%\!relPath!
	call protoc.exe --csharp_out=generated %baseDir%\!relPath!
)

endlocal
pause