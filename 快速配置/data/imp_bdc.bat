@ECHO OFF
echo ׼�����벻�����Ǽ����ݿ�
imp {user_bdc}/{password_bdc}@{datasource} fromuser={user_bdc} touser={user_bdc} file={path_data_bdc} log={path_data_main}/imp-{user_bdc}.log
echo �������
ping -n 3 127.0.0.1>nul