@ECHO OFF
echo ׼���������ƽ̨���ݿ�
imp {user_framework}/{password_framework}@{datasource} fromuser={user_framework} touser={user_framework} file={path_data_framework} log={path_data_main}/imp-{user_framework}.log
echo �������
ping -n 3 127.0.0.1>nul