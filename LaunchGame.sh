#!/bin/bash
# Tumble Rumble - macOS/Linux Launcher
# This script launches the game executable

echo "========================================"
echo "   TUMBLE RUMBLE - Cosmic Circus"
echo "========================================"
echo ""
echo "Starting game..."
echo ""

# Detect platform
if [[ "$OSTYPE" == "darwin"* ]]; then
    # macOS
    if [ -f "Builds/macOS/Tumble Rumble.app/Contents/MacOS/Tumble Rumble" ]; then
        open "Builds/macOS/Tumble Rumble.app"
        echo "Game launched successfully!"
    else
        echo "ERROR: Game executable not found!"
        echo "Please build the game first using Unity Editor."
        echo "Go to: Tumble Rumble -> Build Game"
        read -p "Press Enter to exit..."
    fi
else
    # Linux
    if [ -f "Builds/Linux/Tumble Rumble.x86_64" ]; then
        cd "Builds/Linux"
        chmod +x "Tumble Rumble.x86_64"
        ./"Tumble Rumble.x86_64"
        echo "Game launched successfully!"
    else
        echo "ERROR: Game executable not found!"
        echo "Please build the game first using Unity Editor."
        echo "Go to: Tumble Rumble -> Build Game"
        read -p "Press Enter to exit..."
    fi
fi
