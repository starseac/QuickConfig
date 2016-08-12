@ECHO OFF
echo 准备导入工作流数据库
imp {user_workflow}/{password_workflow}@{datasource} fromuser={user_workflow} touser={user_workflow} file={path_data_workflow} log={path_data_main}/imp-{user_workflow}.log
echo 导入结束
ping -n 3 127.0.0.1>nul