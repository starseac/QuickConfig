@ECHO OFF
echo ׼��ж�ط���
pause
REM The following directory is for .NET 4.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319
set PATH=%PATH%;%DOTNETFX2%
echo ж�ط���...
echo ---------------------------------------------------
InstallUtil /u {path_service}\OVIT.WebApp.ServiceHost.exe
echo ---------------------------------------------------
echo ж�ط���ɹ�!
pause