@ECHO OFF
echo 准备导出工作流数据库
set backupday=backup-%date:~0,4%-%date:~5,2%-%date:~8,2%
cd {path_data_main}
%{path_data_main}:~0,2% 
mkdir %backupday%
exp {user_workflow}/{password_workflow}@{datasource}  file={path_data_main}/%backupday%/exp-{user_workflow}.dmp log={path_data_main}/%backupday%/exp-{user_workflow}.log
echo 导出结束
ping -n 3 127.0.0.1>nul