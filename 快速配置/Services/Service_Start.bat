@ECHO OFF
echo 启动自动备份服务......  
net start QuickConfig_AutoBackup 
@echo.启动完毕！ 
ping -n 3 127.0.0.1>nul