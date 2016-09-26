@echo off 
echo 开始卸载自动备份服务..... 
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe  /u AutoBackup.exe
ping -n 3 127.0.0.1>nul