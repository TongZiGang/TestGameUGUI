@echo off
setlocal enabledelayedexpansion

:: 输入目标文件夹路径
set "baseDir=Protobuf"
set "path=ProtobufCompiler/bin"

:: 清理目标目录
if exist generated (
    echo 正在清理目标目录: %TARGET_DIR%
    rmdir /s /q generated
    mkdir generated
) else (
    echo 创建目标目录: generated
    mkdir generated
)

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

:: 设置源目录和目标目录
set "source_dir=generated"
set "target_dir=..\GameServer\ProtoBuf"

:: 拷贝所有文件
xcopy "%source_dir%\*" "%target_dir%" /E /C /H /Y
pause
