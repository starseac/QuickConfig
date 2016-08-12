@ECHO OFF
echo 准备导入不动产登记数据库
imp {user_bdc}/{password_bdc}@{datasource} fromuser={user_bdc} touser={user_bdc} file={path_data_bdc} log={path_data_main}/imp-{user_bdc}.log
echo 导入结束
ping -n 3 127.0.0.1>nul