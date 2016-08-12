@ECHO OFF
echo 准备导入人事数据库
imp {user_hr}/{password_hr}@{datasource} fromuser={user_hr} touser={user_hr} file={path_data_hr} log={path_data_main}/imp-{user_hr}.log
echo 导入结束
ping -n 3 127.0.0.1>nul