@ECHO OFF
echo ׼�������������ݿ�
imp {user_hr}/{password_hr}@{datasource} fromuser={user_hr} touser={user_hr} file={path_data_hr} log={path_data_main}/imp-{user_hr}.log
echo �������
ping -n 3 127.0.0.1>nul