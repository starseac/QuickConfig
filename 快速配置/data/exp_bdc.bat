@ECHO OFF
echo ׼�������������Ǽ����ݿ�
set backupday=backup-%date:~0,4%-%date:~5,2%-%date:~8,2%
cd {path_data_main}
%{path_data_main}:~0,2% 
mkdir %backupday%
exp {user_bdc}/{password_bdc}@{datasource}  file={path_data_main}/%backupday%/exp-{user_bdc}.dmp log={path_data_main}/%backupday%/exp-{user_bdc}.log
echo ��������
ping -n 3 127.0.0.1>nul