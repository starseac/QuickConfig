@ECHO OFF
echo 准备导出工作流数据库
set backupday={backupday}

exp {user_workflow}/{password_workflow}@{datasource}  file=%backupday%/exp-{user_workflow}.dmp log=%backupday%/exp-{user_workflow}.log
echo 导出结束
ping -n 3 127.0.0.1>nul