@ECHO OFF
echo 准备导入权籍调查数据库
imp {user_qjdc}/{password_qjdc}@{datasource} fromuser={user_qjdc} touser={user_qjdc} file={path_data_qjdc} log={path_data_main}/imp-{user_qjdc}.log
echo 导入结束
ping -n 3 127.0.0.1>nul