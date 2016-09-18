@ECHO OFF
echo 准备导出人事数据库
set backupday={backupday}

exp {user_hr}/{password_hr}@{datasource}  file=%backupday%/exp-{user_hr}.dmp log=%backupday%/exp-{user_hr}.log
echo 导出结束
ping -n 3 127.0.0.1>nul