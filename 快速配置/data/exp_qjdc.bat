@ECHO OFF
echo ׼������Ȩ���������ݿ�
set backupday=backup-%date:~0,4%-%date:~5,2%-%date:~8,2%
cd {path_data_main}
%{path_data_main}:~0,2% 
mkdir %backupday%
exp {user_qjdc}/{password_qjdc}@{datasource}  file={path_data_main}/%backupday%/exp-{user_qjdc}.dmp log={path_data_main}/%backupday%/exp-{user_qjdc}.log
echo ��������
ping -n 3 127.0.0.1>nul