@echo off 
echo ��ʼж���Զ����ݷ���..... 
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe  /u AutoBackup.exe
ping -n 3 127.0.0.1>nul