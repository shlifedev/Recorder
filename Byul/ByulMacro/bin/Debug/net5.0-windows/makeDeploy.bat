mkdir deploy
mkdir deploy\temp
xcopy "ClickableTransparentOverlay.dll" "deploy\" /y
xcopy "ByulMacro.exe" "deploy\" /y 
xcopy "ByulMacro.dll" "deploy\" /y 
xcopy "ByulMacro.runtimeconfig.dev.json" "deploy\" /y 
xcopy "ByulMacro.runtimeconfig.json" "deploy\" /y   
xcopy "ByulMacro.deps.json" "deploy\" /y   