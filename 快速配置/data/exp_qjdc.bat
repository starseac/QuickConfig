@ECHO OFF
echo 准备导出权籍调查数据库
set backupday={backupday}

exp {user_qjdc}/{password_qjdc}@{datasource}  file=%backupday%/exp-{user_qjdc}.dmp log=%backupday%/exp-{user_qjdc}.log
echo 导出结束
ping -n 3 127.0.0.1>nul