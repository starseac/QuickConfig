@ECHO OFF
echo 准备导入基础平台数据库
imp {user_framework}/{password_framework}@{datasource} fromuser={user_framework} touser={user_framework} file={path_data_framework} log={path_data_main}/imp-{user_framework}.log
echo 导入结束
ping -n 3 127.0.0.1>nul