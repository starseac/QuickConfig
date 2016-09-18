%启动Oracle的服务% 
@echo.服务启动...... 
@echo off 
net start QuickConfig_AutoBackup 
@echo.启动完毕！ 
ping -n 3 127.0.0.1>nul