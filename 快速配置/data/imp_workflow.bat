@ECHO OFF
echo ׼�����빤�������ݿ�
imp {user_workflow}/{password_workflow}@{datasource} fromuser={user_workflow} touser={user_workflow} file={path_data_workflow} log={path_data_main}/imp-{user_workflow}.log
echo �������
ping -n 3 127.0.0.1>nul