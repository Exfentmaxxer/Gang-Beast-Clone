@echo off
REM Tumble Rumble - Windows Launcher
REM This script launches the game executable

echo ========================================
echo    TUMBLE RUMBLE - Cosmic Circus
echo ========================================
echo.
echo Starting game...
echo.

REM Check if build exists
if exist "Builds\Windows\Tumble Rumble.exe" (
    cd "Builds\Windows"
    start "" "Tumble Rumble.exe"
    echo Game launched successfully!
) else (
    echo ERROR: Game executable not found!
    echo Please build the game first using Unity Editor.
    echo Go to: Tumble Rumble -^> Build Game
    echo.
    pause
)
