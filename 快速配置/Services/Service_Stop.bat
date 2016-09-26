@ECHO OFF
echo 自动备份服务停止中..... 
net stop QuickConfig_AutoBackup  
@echo.服务已停止！ 
ping -n 3 127.0.0.1>nul