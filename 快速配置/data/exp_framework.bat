@ECHO OFF
echo ׼����������ƽ̨���ݿ�
set backupday={backupday}

exp {user_framework}/{password_framework}@{datasource}  file=%backupday%/exp-{user_framework}.dmp log=%backupday%/exp-{user_framework}.log
echo ��������
ping -n 3 127.0.0.1>nul