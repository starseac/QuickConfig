using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System.Runtime.InteropServices;
using System.Data;
using System.IO;

namespace QuickConfig
{
   public class setBackup
    {

        bool check_backup_db_framework =false;
        bool check_backup_db_workflow = false;
        bool check_backup_db_hr = false;
        bool check_backup_db_bdc = false;
        bool check_backup_db_qjdc = false;

        bool check_backup_sde_sde = false;
        bool check_backup_sde_sde_his = false;
        bool check_backup_sde_sde_pre = false;

        bool check_backup_app_wcf = false;
        bool check_backup_app_framework = false;
        bool check_backup_app_workflow = false;
        bool check_backup_app_hr = false;
        bool check_backup_app_bdc = false;
        bool check_backup_app_qjdc = false;

        bool check_backup_files_ftp = false;
        bool check_backup_files_gxml = false;



          string exp_tem_path = "";
           string exp_path ="";
           string dayString = "";
       
           string backup_path ="";


       public void backup(bool [] choose,string dataParentFolderPath,string xmlPath)
       {
           if (choose != null && choose.Length == 16)
           {

                check_backup_db_framework = choose[0];
                check_backup_db_workflow = choose[1];
                check_backup_db_hr = choose[2];
                check_backup_db_bdc = choose[3];
                check_backup_db_qjdc = choose[4];

                check_backup_sde_sde = choose[5];
                check_backup_sde_sde_his = choose[6];
                check_backup_sde_sde_pre = choose[7];

                check_backup_app_wcf = choose[8];
                check_backup_app_framework = choose[9];
                check_backup_app_workflow = choose[10];
                check_backup_app_hr = choose[11];
                check_backup_app_bdc = choose[12];
                check_backup_app_qjdc = choose[13];

                check_backup_files_ftp = choose[14];
                check_backup_files_gxml = choose[15];

           }else{
               return;
           
           }
           //   string exp_tem_path = AppDomain.CurrentDomain.BaseDirectory.Replace("\\Services","") + @"data";
           //   string exp_path = AppDomain.CurrentDomain.BaseDirectory.Replace("\\Services","") + @"dataTemp";


           // string dayString = DateTime.Now.Year + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month + "") + "-" + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day + "");  //DateTime.Now.ToShortDateString();
            exp_tem_path = dataParentFolderPath + @"data";
            exp_path = dataParentFolderPath + @"dataTemp";
            dayString = DateTime.Now.Year + "-" + (DateTime.Now.Month < 10 ? "0" + DateTime.Now.Month : DateTime.Now.Month + "") + "-" + (DateTime.Now.Day < 10 ? "0" + DateTime.Now.Day : DateTime.Now.Day + "");  //DateTime.Now.ToShortDateString();
           //MessageBox.Show(dayString);
           string timeString = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "--" + DateTime.Now.Second.ToString();
           setXml xml = new setXml();
           //xml.setQuickConfig();
           xml.setXMLPath(xmlPath);
            backup_path = xml.getSetXmlValue("path_data_main") + "\\backup-" + dayString + "\\" + timeString;
           if (!System.IO.Directory.Exists(backup_path))
           {
               System.IO.Directory.CreateDirectory(backup_path);
           }

           setConfig config = new setConfig();

           string ansStr = "开始备份数据\r\n";
           //  string ansStr = "";
           // setMessage.MessageShow("", "开始备份数据!", this);
           if (check_backup_db_framework == true)
           {
               //设置空表导出参数
               setDB db = new setDB(xml.getSetXmlValue("user_framework"), xml.getSetXmlValue("password_framework"), xml.getSetXmlValue("datasource"));
               db.setEXP();
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "backupday", backup_path });
               dtPar.Rows.Add(new object[] { "user_framework", xml.getSetXmlValue("user_framework") });
               dtPar.Rows.Add(new object[] { "password_framework", xml.getSetXmlValue("password_framework") });
               dtPar.Rows.Add(new object[] { "datasource", xml.getSetXmlValue("datasource") });
               config.repalcePar(exp_tem_path, "exp_framework.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\exp_framework.bat");
               ansStr += "基础框架数据导出完成\r\n";
           }

           if (check_backup_db_workflow == true)
           {
               //设置空表导出参数
               setDB db = new setDB(xml.getSetXmlValue("user_workflow"), xml.getSetXmlValue("password_workflow"), xml.getSetXmlValue("datasource"));
               db.setEXP();
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "backupday", backup_path });
               dtPar.Rows.Add(new object[] { "user_workflow", xml.getSetXmlValue("user_workflow") });
               dtPar.Rows.Add(new object[] { "password_workflow", xml.getSetXmlValue("password_workflow") });
               dtPar.Rows.Add(new object[] { "datasource", xml.getSetXmlValue("datasource") });
               config.repalcePar(exp_tem_path, "exp_workflow.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\exp_workflow.bat");
               ansStr += "工作流据导出完成\r\n";
           }

           if (check_backup_db_hr == true)
           {
               //设置空表导出参数
               setDB db = new setDB(xml.getSetXmlValue("user_hr"), xml.getSetXmlValue("password_hr"), xml.getSetXmlValue("datasource"));
               db.setEXP();
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "backupday", backup_path });
               dtPar.Rows.Add(new object[] { "user_hr", xml.getSetXmlValue("user_hr") });
               dtPar.Rows.Add(new object[] { "password_hr", xml.getSetXmlValue("password_hr") });
               dtPar.Rows.Add(new object[] { "datasource", xml.getSetXmlValue("datasource") });
               config.repalcePar(exp_tem_path, "exp_hr.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\exp_hr.bat");
               ansStr += "人事据导出完成\r\n";
           }

           if (check_backup_db_bdc == true)
           {
               //设置空表导出参数
               setDB db = new setDB(xml.getSetXmlValue("user_bdc"), xml.getSetXmlValue("password_bdc"), xml.getSetXmlValue("datasource"));
               db.setEXP();
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "backupday", backup_path });
               dtPar.Rows.Add(new object[] { "user_bdc", xml.getSetXmlValue("user_bdc") });
               dtPar.Rows.Add(new object[] { "password_bdc", xml.getSetXmlValue("password_bdc") });
               dtPar.Rows.Add(new object[] { "datasource", xml.getSetXmlValue("datasource") });

               config.repalcePar(exp_tem_path, "exp_bdc.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\exp_bdc.bat");
               ansStr += "不动产导出完成\r\n";
           }

           if (check_backup_db_qjdc == true)
           {
               //设置空表导出参数
               setDB db = new setDB(xml.getSetXmlValue("user_qjdc"), xml.getSetXmlValue("password_qjdc"), xml.getSetXmlValue("datasource"));
               db.setEXP();
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "backupday", backup_path });
               dtPar.Rows.Add(new object[] { "user_qjdc", xml.getSetXmlValue("user_qjdc") });
               dtPar.Rows.Add(new object[] { "password_qjdc", xml.getSetXmlValue("password_qjdc") });
               dtPar.Rows.Add(new object[] { "datasource", xml.getSetXmlValue("datasource") });
               config.repalcePar(exp_tem_path, "exp_qjdc.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\exp_qjdc.bat");
               ansStr += "权籍调查数据导出完成\r\n";
           }

          


           //备份 sde库
           //初始化 esri授权
           setArcgis.init();         

           IAoInitialize pao = new AoInitializeClass();
           pao.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);

           EngineDatabase engine = new EngineDatabase();

           if (check_backup_sde_sde == true)
           {
               IWorkspace workspace = null;
               IFeatureWorkspace featureworkspace = null;
               IFeatureDataset featuredataset = null;
               workspace = engine.getSDEWorkspace(xml.getSetXmlValue("wcf_ip"), "sde:oracle10g:" + xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_sde"), xml.getSetXmlValue("password_sde"));
               engine.createGDBFile(backup_path, "BDCDJ.gdb");
               // frmLoading.ShowLoading();
               string ans1 = engine.exportSDE2GDB(workspace, featureworkspace, featuredataset, backup_path + "\\BDCDJ.gdb", Convert.ToInt32(xml.getSetXmlValue("par_gcs")), xml.getSetXmlValue("user_sde"));
               // frmLoading.HideLoading();
               //MessageBox.Show("现势库导出结果如下:\r\n" + ans1);
               ansStr += "现势库导出结果如下:\r\n" + ans1 + "\r\n";
               //setMessage.MessageShow("", "现势库导出结果如下:\r\n" + ans1, this);
               Marshal.ReleaseComObject(workspace);
               ansStr += "现势库导出导出完成\r\n";
           }

           if (check_backup_sde_sde_his == true)
           {
               IWorkspace workspace = null;
               IFeatureWorkspace featureworkspace = null;
               IFeatureDataset featuredataset = null;
               workspace = engine.getSDEWorkspace(xml.getSetXmlValue("wcf_ip"), "sde:oracle10g:" + xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_sde_his"), xml.getSetXmlValue("password_sde_his"));
               engine.createGDBFile(backup_path, "BDCDJ_HIS.gdb");
               //frmLoading.ShowLoading();
               string ans1 = engine.exportSDE2GDB(workspace, featureworkspace, featuredataset, backup_path + "\\BDCDJ_HIS.gdb", Convert.ToInt32(xml.getSetXmlValue("par_gcs")), xml.getSetXmlValue("user_sde_his"));
               //MessageBox.Show("历史库导出结果如下:\r\n" + ans1);
               //frmLoading.HideLoading();
               // setMessage.MessageShow("", "历史库导出结果如下:\r\n" + ans1, this);
               ansStr += "历史库导出结果如下:\r\n" + ans1;
               Marshal.ReleaseComObject(workspace);
               ansStr += "历史库导出完成\r\n";
           }
           if (check_backup_sde_sde_pre == true)
           {
               IWorkspace workspace = null;
               IFeatureWorkspace featureworkspace = null;
               IFeatureDataset featuredataset = null;
               workspace = engine.getSDEWorkspace(xml.getSetXmlValue("wcf_ip"), "sde:oracle10g:" + xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_sde_pre"), xml.getSetXmlValue("password_sde_pre"));
               engine.createGDBFile(backup_path, "BDCDJ_PRE.gdb");
               // frmLoading.ShowLoading();
               string ans1 = engine.exportSDE2GDB(workspace, featureworkspace, featuredataset, backup_path + "\\BDCDJ_PRE.gdb", Convert.ToInt32(xml.getSetXmlValue("par_gcs")), xml.getSetXmlValue("user_sde_pre"));
               //MessageBox.Show("临时库导出结果如下:\r\n" + ans1);
               // frmLoading.HideLoading();
               //setMessage.MessageShow("", "临时库导出结果如下:\r\n" + ans1, this);
               ansStr += "临时库导出结果如下:\r\n" + ans1;
               Marshal.ReleaseComObject(workspace);
               ansStr += "临时库数据导出完成\r\n";
           }
           //备份 程序

           if (check_backup_app_wcf == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "wcf服务程序" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\WebAppServiceHost.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_service") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "wcf服务程序备份完成\r\n";
           }

           if (check_backup_app_framework == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "基础框架程序" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\BaseFramework.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_framework") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "基础框架程序备份完成\r\n";
           }

           if (check_backup_app_workflow == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里\
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "工作流程序" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\Workflow.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_workflow") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "工作流程序备份完成\r\n";
           }
           if (check_backup_app_hr == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "人事程序" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\HRWebApp.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_hr") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "人事程序备份完成\r\n";
           }
           if (check_backup_app_bdc == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "不动产程序" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\RealEstateRegister.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_bdc") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "不动产程序备份完成\r\n";
           }
           if (check_backup_app_qjdc == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "权籍调查程序" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\qjdc.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_qjdc") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "权籍调查程序备份完成\r\n";
           }

           if (check_backup_files_ftp == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "ftp文件" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\ftp文件.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_ftp") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "权籍调查ftp文件备份完成\r\n";
           }

           if (check_backup_files_gxml == true)
           {
               DataTable dtPar = new DataTable();
               dtPar.Columns.Add("name", typeof(string));
               dtPar.Columns.Add("value", typeof(string));
               //然后把你想要加的数据加进DataTable 里
               dtPar.Rows.Add(new object[] { "appfolder", dataParentFolderPath });
               dtPar.Rows.Add(new object[] { "filename", "共享目录" });
               dtPar.Rows.Add(new object[] { "target", backup_path + "\\共享目录.7z" });
               dtPar.Rows.Add(new object[] { "orifolder", xml.getSetXmlValue("path_gxml") });

               config.repalcePar(exp_tem_path, "7z.bat", exp_path, dtPar);
               setBAT.RunBat(exp_path + @"\7z.bat");
               ansStr += "权籍调查共享目录文件备份完成\r\n";
           }

           ansStr += "导出结束\r\n";
           // MessageBox.Show(ansStr);
           //Thread.Sleep(3000);
           // setMessage.MessageShow("", ansStr, this);
           StreamWriter sw = null;
           if (!File.Exists(backup_path + "\\" + dayString + ".log"))
           {
               //不存在就新建一个文本文件,并写入一些内容 
               sw = File.CreateText(backup_path + "\\" + dayString + ".log");
           }
           else
           {
               sw = new StreamWriter(backup_path + "\\" + dayString + ".log");
           }

           sw.Write(ansStr);
           sw.Close();




       }

       public void openFolderAndLog() {
           setConfig config = new setConfig();
           //打开备份目录
           DataTable folderPar = new DataTable();
           folderPar.Columns.Add("name", typeof(string));
           folderPar.Columns.Add("value", typeof(string));
           //然后把你想要加的数据加进DataTable 里
           folderPar.Rows.Add(new object[] { "targetfolder", backup_path + "\\" });
           folderPar.Rows.Add(new object[] { "dayString", dayString });
           config.repalcePar(AppDomain.CurrentDomain.BaseDirectory + "tool\\", "explorer.bat", exp_path, folderPar);
           setBAT.RunBat(exp_path + @"\explorer.bat");
       }
   
    }
}
