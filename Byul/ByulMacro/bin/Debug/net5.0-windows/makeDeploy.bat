@echo OFF
if not exist deploy mkdir deploy
if not exist deploy\temp mkdir deploy\temp
if not exist deploy\ByulMacro.deps.json xcopy "ByulMacro.deps.json" "deploy\" /s /h /e /d /y


if not exist deploy\ByulMacro.dll xcopy "ByulMacro.dll" "deploy\" /y
if not exist deploy\ByulMacro.ex xcopy "ByulMacro.exe" "deploy\" /y
if not exist deploy\ByulMacro.runtimeconfig.dev.json xcopy "ByulMacro.runtimeconfig.dev.json" "deploy\" /y
if not exist deploy\ByulMacro.runtimeconfig.json xcopy "ByulMacro.runtimeconfig.json" "deploy\" /y
if not exist deploy\ClickableTransparentOverlay.dll xcopy "ClickableTransparentOverlay.dll" "deploy\" /y
if not exist deploy\ClickableTransparentOverlay.xml xcopy "ClickableTransparentOverlay.xml" "deploy\" /y
if not exist deploy\ByulMacro.dll xcopy "ByulMacro.dll" "deploy\" /y