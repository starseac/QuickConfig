@ECHO OFF
echo ׼�������������Ǽ����ݿ�
set backupday={backupday}

exp {user_bdc}/{password_bdc}@{datasource}  file=%backupday%/exp-{user_bdc}.dmp log=%backupday%/exp-{user_bdc}.log
echo ��������
ping -n 3 127.0.0.1>nul