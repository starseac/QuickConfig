@ECHO OFF
echo 准备导出人事数据库
set backupday=backup-%date:~0,4%-%date:~5,2%-%date:~8,2%
cd {path_data_main}
%{path_data_main}:~0,2% 
mkdir %backupday%
exp {user_hr}/{password_hr}@{datasource}  file={path_data_main}/%backupday%/exp-{user_hr}.dmp log={path_data_main}/%backupday%/exp-{user_hr}.log
echo 导出结束
ping -n 3 127.0.0.1>nul