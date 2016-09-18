%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe AutoBackup.exe
Net Start QuickConfig_AutoBackup 
sc config QuickConfig_AutoBackup start= auto
ping -n 3 127.0.0.1>nul