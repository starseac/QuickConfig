@ECHO OFF
echo ׼���������������ݿ�
set backupday={backupday}

exp {user_workflow}/{password_workflow}@{datasource}  file=%backupday%/exp-{user_workflow}.dmp log=%backupday%/exp-{user_workflow}.log
echo ��������
ping -n 3 127.0.0.1>nul