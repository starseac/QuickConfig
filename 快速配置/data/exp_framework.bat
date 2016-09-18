@ECHO OFF
echo 准备导出基础平台数据库
set backupday={backupday}

exp {user_framework}/{password_framework}@{datasource}  file=%backupday%/exp-{user_framework}.dmp log=%backupday%/exp-{user_framework}.log
echo 导出结束
ping -n 3 127.0.0.1>nul