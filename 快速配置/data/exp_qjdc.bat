@ECHO OFF
echo ׼������Ȩ���������ݿ�
set backupday={backupday}

exp {user_qjdc}/{password_qjdc}@{datasource}  file=%backupday%/exp-{user_qjdc}.dmp log=%backupday%/exp-{user_qjdc}.log
echo ��������
ping -n 3 127.0.0.1>nul