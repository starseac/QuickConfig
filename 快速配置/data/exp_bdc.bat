@ECHO OFF
echo 准备导出不动产登记数据库
set backupday={backupday}

exp {user_bdc}/{password_bdc}@{datasource}  file=%backupday%/exp-{user_bdc}.dmp log=%backupday%/exp-{user_bdc}.log
echo 导出结束
ping -n 3 127.0.0.1>nul