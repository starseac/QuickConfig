@ECHO OFF
echo ׼�������������ݿ�
set backupday={backupday}

exp {user_hr}/{password_hr}@{datasource}  file=%backupday%/exp-{user_hr}.dmp log=%backupday%/exp-{user_hr}.log
echo ��������
ping -n 3 127.0.0.1>nul