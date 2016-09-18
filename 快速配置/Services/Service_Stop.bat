@echo.服务停止中..... 
@echo off 
net stop QuickConfig_AutoBackup  
@echo.服务已停止！ 
ping -n 3 127.0.0.1>nul