@echo off
echo 开始压缩文件  {filename}
{appfolder}tool/7-Zip/7z a  {target} {orifolder}

ping -n 3 127.0.0.1>nul
