@echo off
setlocal

set url="https://github.com/3F/DllExport/releases/download/v1.7.4/DllExport.bat"
set output="DllExport.bat"

echo Downloading file...

curl -o "%output%" "%url%"

if exist "%output%" (
    echo Download complete. Executeing file...
    call "%output%"
) else (
    echo Download failed.
)

endlocal