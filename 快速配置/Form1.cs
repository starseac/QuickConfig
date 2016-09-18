using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using System.Runtime.InteropServices;
using System.Threading;
using ESRI.ArcGIS.Geodatabase;

namespace QuickConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            setGCS();
            setValue();
            this.wcf_state.Text = getServiceState("OVIT_WebAppServer");
            this.lab_backupServicesState.Text = getServiceState("QuickConfig_AutoBackup");
        }

        private void setValue()
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            DataTable parTable = xml.readXML();

            this.path_main.Text = parTable.Select("name='path_main'")[0][1].ToString();
            this.path_service.Text = parTable.Select("name='path_service'")[0][1].ToString();
            this.path_framework.Text = parTable.Select("name='path_framework'")[0][1].ToString();
            this.path_workflow.Text = parTable.Select("name='path_workflow'")[0][1].ToString();
            this.path_hr.Text = parTable.Select("name='path_hr'")[0][1].ToString();
            this.path_bdc.Text = parTable.Select("name='path_bdc'")[0][1].ToString();
            this.path_qjdc.Text = parTable.Select("name='path_qjdc'")[0][1].ToString();

            this.path_ftp.Text = parTable.Select("name='path_ftp'")[0][1].ToString();
            this.path_gxml.Text = parTable.Select("name='path_gxml'")[0][1].ToString();

            this.path_data_main.Text = parTable.Select("name='path_data_main'")[0][1].ToString();
            this.path_data_framework.Text = parTable.Select("name='path_data_framework'")[0][1].ToString();
            this.path_data_workflow.Text = parTable.Select("name='path_data_workflow'")[0][1].ToString();
            this.path_data_hr.Text = parTable.Select("name='path_data_hr'")[0][1].ToString();
            this.path_data_bdc.Text = parTable.Select("name='path_data_bdc'")[0][1].ToString();
            this.path_data_qjdc.Text = parTable.Select("name='path_data_qjdc'")[0][1].ToString();
            this.path_data_bdcdj.Text = parTable.Select("name='path_data_bdcdj'")[0][1].ToString();
            this.path_data_bdcdj_his.Text = parTable.Select("name='path_data_bdcdj_his'")[0][1].ToString();
            this.path_data_bdcdj_pre.Text = parTable.Select("name='path_data_bdcdj_pre'")[0][1].ToString();

            this.txt_GCS.SelectedValue = parTable.Select("name='par_gcs'")[0][1].ToString();

            this.user_framework.Text = parTable.Select("name='user_framework'")[0][1].ToString();
            this.user_workflow.Text = parTable.Select("name='user_workflow'")[0][1].ToString();
            this.user_hr.Text = parTable.Select("name='user_hr'")[0][1].ToString();
            this.user_bdc.Text = parTable.Select("name='user_bdc'")[0][1].ToString();
            this.user_qjdc.Text = parTable.Select("name='user_qjdc'")[0][1].ToString();
            this.user_sde.Text = parTable.Select("name='user_sde'")[0][1].ToString();
            this.user_sde_his.Text = parTable.Select("name='user_sde_his'")[0][1].ToString();
            this.user_sde_pre.Text = parTable.Select("name='user_sde_pre'")[0][1].ToString();
            this.password_framework.Text = parTable.Select("name='password_framework'")[0][1].ToString();
            this.password_workflow.Text = parTable.Select("name='password_workflow'")[0][1].ToString();
            this.password_hr.Text = parTable.Select("name='password_hr'")[0][1].ToString();
            this.password_bdc.Text = parTable.Select("name='password_bdc'")[0][1].ToString();
            this.password_qjdc.Text = parTable.Select("name='password_qjdc'")[0][1].ToString();
            this.password_sde.Text = parTable.Select("name='password_sde'")[0][1].ToString();
            this.password_sde_his.Text = parTable.Select("name='password_sde_his'")[0][1].ToString();
            this.password_sde_pre.Text = parTable.Select("name='password_sde_pre'")[0][1].ToString();
            this.user_system.Text = parTable.Select("name='user_system'")[0][1].ToString();
            this.password_system.Text = parTable.Select("name='password_system'")[0][1].ToString();
            this.datasource.Text = parTable.Select("name='datasource'")[0][1].ToString();

            this.check_default.Checked = parTable.Select("name='path_datafolder_type'")[0][1].ToString() == "true" ? true : false;
            this.path_datafolder.Text = parTable.Select("name='path_datafolder'")[0][1].ToString();


            this.wcf_ip.Text = parTable.Select("name='wcf_ip'")[0][1].ToString();
            this.wcf_port.Text = parTable.Select("name='wcf_port'")[0][1].ToString();

            this.web_ip.Text = parTable.Select("name='web_ip'")[0][1].ToString();

            this.ftp_ip.Text = parTable.Select("name='ftp_ip'")[0][1].ToString();
            this.ftp_user.Text = parTable.Select("name='ftp_user'")[0][1].ToString();
            this.ftp_password.Text = parTable.Select("name='ftp_password'")[0][1].ToString();

            this.gxml_ip.Text = parTable.Select("name='gxml_ip'")[0][1].ToString();
            this.gxml_user.Text = parTable.Select("name='gxml_user'")[0][1].ToString();
            this.gxml_password.Text = parTable.Select("name='gxml_password'")[0][1].ToString();


            this.par_sjc.Text = parTable.Select("name='par_sjc'")[0][1].ToString();
            this.par_szdxzbm.Text = parTable.Select("name='par_szdxzbm'")[0][1].ToString();
            this.par_szdmc.Text = parTable.Select("name='par_szdmc'")[0][1].ToString();
            this.par_szdjc.Text = parTable.Select("name='par_szdjc'")[0][1].ToString();
            //获取备份设置参数
            string backupcontent = parTable.Select("name='backup_content'")[0][1].ToString();
            if (backupcontent != "")
            {
                string[] backupContent = backupcontent.Split(',');
                bool[] backupBool = new bool[backupContent.Length];
                for (int i = 0; i < backupBool.Length; i++)
                {
                    backupBool[i] = backupContent[i] == "True" ? true : false;
                }

                this.check_backup_db_framework.Checked = backupBool[0];
                this.check_backup_db_workflow.Checked = backupBool[1];
                this.check_backup_db_hr.Checked = backupBool[2];
                this.check_backup_db_bdc.Checked = backupBool[3];
                this.check_backup_db_qjdc.Checked = backupBool[4];

                this.check_backup_sde_sde.Checked = backupBool[5];
                this.check_backup_sde_sde_his.Checked = backupBool[6];
                this.check_backup_sde_sde_pre.Checked = backupBool[7];

                this.check_backup_app_wcf.Checked = backupBool[8];
                this.check_backup_app_framework.Checked = backupBool[9];
                this.check_backup_app_workflow.Checked = backupBool[10];
                this.check_backup_app_hr.Checked = backupBool[11];
                this.check_backup_app_bdc.Checked = backupBool[12];
                this.check_backup_app_qjdc.Checked = backupBool[13];

                this.check_backup_files_ftp.Checked = backupBool[14];
                this.check_backup_files_gxml.Checked = backupBool[15];

            }


            string backuptype = parTable.Select("name='backup_type'")[0][1].ToString();
            if (backuptype != "")
            {
                if (backuptype == "每日")
                {
                    this.tab_backup.SelectedIndex = 0;
                }
                else if (backuptype == "每周")
                {
                    this.tab_backup.SelectedIndex = 1;
                }
                else
                {
                    this.tab_backup.SelectedIndex = 2;
                }

            }
            else
            {
                this.tab_backup.SelectedIndex = 0;
            }

            string chooseWeekStr = parTable.Select("name='backup_type_week'")[0][1].ToString();
            foreach (Control cb in tab_backup.TabPages[1].Controls)
            {
                if (cb is CheckBox && chooseWeekStr.Contains(cb.Text))
                {
                    (cb as CheckBox).Checked = true;
                }
            }
            this.backup_week_starttime.Text = parTable.Select("name='backup_type_weektime'")[0][1].ToString();


            string chooseMonthStr = parTable.Select("name='backup_type_month'")[0][1].ToString();
            foreach (Control cb in tab_backup.TabPages[2].Controls)
            {
                if (cb is CheckBox)
                {
                    if ((chooseMonthStr.Contains(cb.Text) && chooseMonthStr == cb.Text) || (chooseMonthStr.Contains(cb.Text + ",")) || (chooseMonthStr.Contains("," + cb.Text + ",")) || (chooseMonthStr.Contains("," + cb.Text)))
                    {
                        (cb as CheckBox).Checked = true;
                    }
                }
            }
            this.backup_month_starttime.Text = parTable.Select("name='backup_type_monthtime'")[0][1].ToString();


        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (!checkAppFolder())
            {
                return;
            }

            setXml xml = new setXml();
            xml.setQuickConfig();
            xml.editxml("path_main", this.path_main.Text);
            xml.editxml("path_service", this.path_service.Text);
            xml.editxml("path_framework", this.path_framework.Text);
            xml.editxml("path_workflow", this.path_workflow.Text);
            xml.editxml("path_hr", this.path_hr.Text);
            xml.editxml("path_bdc", this.path_bdc.Text);
            xml.editxml("path_qjdc", this.path_qjdc.Text);

            xml.editxml("path_ftp", this.path_ftp.Text);
            xml.editxml("path_gxml", this.path_gxml.Text);

            xml.editxml("path_datafolder_type", this.check_default.Checked.ToString());
            xml.editxml("path_datafolder", this.path_datafolder.Text);


            xml.editxml("path_data_main", this.path_data_main.Text);
            xml.editxml("path_data_framework", this.path_data_framework.Text);
            xml.editxml("path_data_workflow", this.path_data_workflow.Text);
            xml.editxml("path_data_hr", this.path_data_hr.Text);
            xml.editxml("path_data_bdc", this.path_data_bdc.Text);
            xml.editxml("path_data_qjdc", this.path_data_qjdc.Text);
            xml.editxml("path_data_bdcdj", this.path_data_bdcdj.Text);
            xml.editxml("path_data_bdcdj_his", this.path_data_bdcdj_his.Text);
            xml.editxml("path_data_bdcdj_pre", this.path_data_bdcdj_pre.Text);

            xml.editxml("par_gcs", this.txt_GCS.SelectedValue.ToString());

            xml.editxml("user_framework", this.user_framework.Text);
            xml.editxml("user_workflow", this.user_workflow.Text);
            xml.editxml("user_hr", this.user_hr.Text);
            xml.editxml("user_bdc", this.user_bdc.Text);
            xml.editxml("user_qjdc", this.user_qjdc.Text);
            xml.editxml("user_sde", this.user_sde.Text);
            xml.editxml("user_sde_his", this.user_sde_his.Text);
            xml.editxml("user_sde_pre", this.user_sde_pre.Text);
            xml.editxml("password_framework", this.password_framework.Text);
            xml.editxml("password_workflow", this.password_workflow.Text);
            xml.editxml("password_hr", this.password_hr.Text);
            xml.editxml("password_bdc", this.password_bdc.Text);
            xml.editxml("password_qjdc", this.password_qjdc.Text);
            xml.editxml("password_sde", this.password_sde.Text);
            xml.editxml("password_sde_his", this.password_sde_his.Text);
            xml.editxml("password_sde_pre", this.password_sde_pre.Text);
            xml.editxml("user_system", this.user_system.Text);
            xml.editxml("password_system", this.password_system.Text);
            xml.editxml("datasource", this.datasource.Text);

            xml.editxml("wcf_ip", this.wcf_ip.Text);
            xml.editxml("wcf_port", this.wcf_port.Text);
            xml.editxml("web_ip", this.web_ip.Text);
            xml.editxml("ftp_ip", this.ftp_ip.Text);
            xml.editxml("ftp_user", this.ftp_user.Text);
            xml.editxml("ftp_password", this.ftp_password.Text);

            xml.editxml("gxml_ip", this.gxml_ip.Text);
            xml.editxml("gxml_user", this.gxml_user.Text);
            xml.editxml("gxml_password", this.gxml_password.Text);


            xml.editxml("par_sjc", this.par_sjc.Text);
            xml.editxml("par_szdxzbm", this.par_szdxzbm.Text);
            xml.editxml("par_szdmc", this.par_szdmc.Text);
            xml.editxml("par_szdjc", this.par_szdjc.Text);


            //设置备份服务
            string chooseContent =
                this.check_backup_db_framework.Checked.ToString() + "," +
                this.check_backup_db_workflow.Checked.ToString() + "," +
                this.check_backup_db_hr.Checked.ToString() + "," +
                this.check_backup_db_bdc.Checked.ToString() + "," +
                this.check_backup_db_qjdc.Checked.ToString() + "," +

                this.check_backup_sde_sde.Checked.ToString() + "," +
                this.check_backup_sde_sde_his.Checked.ToString() + "," +
                this.check_backup_sde_sde_pre.Checked.ToString() + "," +

                this.check_backup_app_wcf.Checked.ToString() + "," +
                this.check_backup_app_framework.Checked.ToString() + "," +
                this.check_backup_app_workflow.Checked.ToString() + "," +
                this.check_backup_app_hr.Checked.ToString() + "," +
                this.check_backup_app_bdc.Checked.ToString() + "," +
                this.check_backup_app_qjdc.Checked.ToString() + "," +

                this.check_backup_files_ftp.Checked.ToString() + "," +
                this.check_backup_files_gxml.Checked.ToString();

            xml.editxml("backup_content", chooseContent);

            xml.editxml("backup_type", this.tab_backup.SelectedTab.Text);
            xml.editxml("backup_type_daytime", this.backup_day_starttime.Value.ToString("HH:mm:ss"));

            string chooseWeekStr = "";
            foreach (Control cb in tab_backup.TabPages[1].Controls)
            {
                if (cb is CheckBox)
                {
                    if ((cb as CheckBox).Checked == true)
                    {
                        if (chooseWeekStr == "")
                        {
                            chooseWeekStr = cb.Text;
                        }
                        else
                        {
                            chooseWeekStr += "," + cb.Text;
                        }
                    }
                }
            }
            xml.editxml("backup_type_week", chooseWeekStr);
            xml.editxml("backup_type_weektime", this.backup_week_starttime.Value.ToString("HH:mm:ss"));

            string chooseMonthStr = "";
            foreach (Control cb in tab_backup.TabPages[2].Controls)
            {
                if (cb is CheckBox)
                {
                    if ((cb as CheckBox).Checked == true)
                    {
                        if (chooseMonthStr == "")
                        {
                            chooseMonthStr = cb.Text;
                        }
                        else
                        {
                            chooseMonthStr += "," + cb.Text;
                        }
                    }
                }
            }

            xml.editxml("backup_type_month", chooseMonthStr);
            xml.editxml("backup_type_monthtime", this.backup_month_starttime.Value.ToString("HH:mm:ss"));
            // MessageBox.Show("保存成功!");
            setMessage.MessageShow("", "保存成功!", this.btn_save);

        }


        private void check_default_CheckedChanged(object sender, EventArgs e)
        {
            if (this.check_default.Checked == true)
            {

                try
                {
                    setDB db = new setDB(this.user_system.Text, this.password_system.Text, this.datasource.Text);
                    this.path_datafolder.Text = db.getDataFolder();
                }
                catch (Exception eg)
                {
                    // MessageBox.Show("链接不上数据库，请检查参数和配置");
                    setMessage.MessageShow("", "链接不上数据库，请检查参数和配置!", this.check_default);
                    this.check_default.Checked = false;
                    return;
                }

            }
            else
            {
                this.path_datafolder.Text = "";
            }
        }

        private string getServiceState(string servicename)
        {
            //看打印服务状态
            try
            {
                ServiceController sc = new ServiceController(servicename);
                return sc.Status.ToString();
            }
            catch (Exception eg)
            {
                return "服务未安装";
            }
        }



        private bool checkAppFolder()
        {
            string errStr = "";
            if (!checkFolder(this.path_service.Text) && check_service.Checked == true)
            {
                errStr += "服务的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_framework.Text) && check_framework.Checked == true)
            {
                errStr += "基础平台的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_workflow.Text) && check_workflow.Checked == true)
            {
                errStr += "工作流的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_hr.Text) && check_hr.Checked == true)
            {
                errStr += "人事的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_bdc.Text) && check_bdc.Checked == true)
            {
                errStr += "不动产登记的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_qjdc.Text) && check_qjdc.Checked == true)
            {
                errStr += "权籍调查的文件夹不存在,请重新设置\r\n";
            }

            if (!checkFolder(this.path_ftp.Text))
            {
                errStr += "ftp的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_gxml.Text))
            {
                errStr += "共享目录的文件夹不存在,请重新设置\r\n";
            }

            //导入数据文件检查
            if (!checkFolder(this.path_gxml.Text))
            {
                errStr += "数据文件存放地址必须填写,请重新设置\r\n";
            }

            if (!checkFile(this.path_data_framework.Text) && this.check_data_framework.Checked == true)
            {
                errStr += "基础平台的dmp文件不存在,请重新设置\r\n";
            }
            if (!checkFile(this.path_data_workflow.Text) && this.check_data_workflow.Checked == true)
            {
                errStr += "工作流的dmp文件不存在,请重新设置\r\n";
            }
            if (!checkFile(this.path_data_hr.Text) && this.check_data_hr.Checked == true)
            {
                errStr += "人事的dmp文件不存在,请重新设置\r\n";
            }
            if (!checkFile(this.path_data_bdc.Text) && this.check_data_bdc.Checked == true)
            {
                errStr += "不动产的dmp文件不存在,请重新设置\r\n";
            }
            if (!checkFile(this.path_data_qjdc.Text) && this.check_data_qjdc.Checked == true)
            {
                errStr += "权籍调查的dmp文件不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_data_bdcdj.Text) && this.check_data_bdcdj.Checked == true)
            {
                errStr += "现势库的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_data_bdcdj_his.Text) && this.check_data_bdcdj_his.Checked == true)
            {
                errStr += "历史库的文件夹不存在,请重新设置\r\n";
            }
            if (!checkFolder(this.path_data_bdcdj_pre.Text) && this.check_data_bdcdj_pre.Checked == true)
            {
                errStr += "临时库的文件夹不存在,请重新设置\r\n";
            }

            if (errStr != "")
            {
                MessageBox.Show(errStr, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btn_beset_Click(object sender, EventArgs e)
        {
            if (!checkAppFolder())
            {
                return;
            }

            setXml xml = new setXml();
            xml.setQuickConfig();
            xml.addCopyFileXML(xml.scanfiles());

            setConfig config = new setConfig();
            List<string> checkApp = new List<string>();

            if (check_service.Checked == true)
            {
                checkApp.Add("service");
            }

            if (check_framework.Checked == true)
            {
                checkApp.Add("framework");
            }
            if (check_workflow.Checked == true)
            {
                checkApp.Add("workflow");
            }
            if (check_hr.Checked == true)
            {
                checkApp.Add("hr");
            }
            if (check_bdc.Checked == true)
            {
                checkApp.Add("bdc");
            }
            if (check_qjdc.Checked == true)
            {
                checkApp.Add("qjdc");
            }

            config.config(checkApp);

            // MessageBox.Show("设置完成!");
            setMessage.MessageShow("", "设置完成!", this.btn_beset);
        }

        private string folderPath()
        {
            string folderPath = "";
            FolderBrowserDialog folderfDialog = new FolderBrowserDialog();
            folderfDialog.Description = "请选着文件夹";
            if (folderfDialog.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderfDialog.SelectedPath;

            }
            return folderPath;
        }


        private string filePath(string format)
        {
            string filePath = "";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*." + format + ")|*." + format + "";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = fileDialog.FileName;
            }
            return filePath;
        }

        private bool checkFolder(string folderPath)
        {
            return System.IO.Directory.Exists(folderPath);

        }

        private bool checkFile(string filePath)
        {
            return System.IO.File.Exists(filePath);

        }

        #region 设置程序路径

        private void btn_main_Click(object sender, EventArgs e)
        {

            this.path_main.Text = this.folderPath();

            this.path_service.Text = this.path_main.Text + @"\service\WebAppServiceHost";
            this.path_framework.Text = this.path_main.Text + @"\web\BaseFramework";
            this.path_workflow.Text = this.path_main.Text + @"\web\Workflow";
            this.path_hr.Text = this.path_main.Text + @"\web\HRWebApp";
            this.path_bdc.Text = this.path_main.Text + @"\web\RealEstateRegister";

            this.path_qjdc.Text = this.path_main.Text + @"\QJDC";

            this.path_ftp.Text = this.path_main.Text + @"\BDCFTP";
            this.path_gxml.Text = this.path_main.Text + @"\共享目录";

        }

        private void btn_service_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_service.Text = Path == "" ? this.path_service.Text : Path;
        }

        private void btn_framework_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_framework.Text = Path == "" ? this.path_service.Text : Path;
        }

        private void btn_workflow_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_workflow.Text = Path == "" ? this.path_service.Text : Path;
        }

        private void btn_hr_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_hr.Text = Path == "" ? this.path_service.Text : Path;
        }

        private void btn_bdc_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_bdc.Text = Path == "" ? this.path_service.Text : Path;
        }

        private void btn_qjdc_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_qjdc.Text = Path == "" ? this.path_service.Text : Path;
        }

        private void btn_ftp_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_ftp.Text = Path == "" ? this.path_ftp.Text : Path;
        }

        private void btn_gxml_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_gxml.Text = Path == "" ? this.path_gxml.Text : Path;
        }

        #endregion



        private void wcf_ip_TextChanged(object sender, EventArgs e)
        {
            this.web_ip.Text = this.wcf_ip.Text;
            this.ftp_ip.Text = this.wcf_ip.Text;
            this.gxml_ip.Text = this.wcf_ip.Text;
        }

        private void btn_folder_ftp_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.path_ftp.Text) == false)
            {
                Directory.CreateDirectory(this.path_ftp.Text);
                // MessageBox.Show("ftp文件夹创建成功！");
                setMessage.MessageShow("", "ftp文件夹创建成功！", this.btn_folder_ftp);
                if (Directory.Exists(this.path_ftp.Text + @"\SJCL") == false)
                {
                    Directory.CreateDirectory(this.path_ftp.Text + @"\SJCL");
                    // MessageBox.Show("ftp中SJCL文件夹创建成功！");
                    setMessage.MessageShow("", "ftp中SJCL文件夹创建成功！", this.btn_folder_ftp);
                }
                else
                {
                    MessageBox.Show("ftp中SJCL文件夹已经存在！");

                }

            }
            else
            {
                MessageBox.Show("ftp文件夹已经存在！");

            }
        }

        private void btn_folder_gxml_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(this.path_gxml.Text) == false)
            {
                Directory.CreateDirectory(this.path_gxml.Text);
                // MessageBox.Show("共享目录文件夹创建成功！");
                setMessage.MessageShow("", "共享目录文件夹创建成功！", this.btn_folder_gxml);
            }
            else
            {
                MessageBox.Show("共享目录文件夹已经存在！");

            }
        }


        #region 配置、创建网站 wcf服务 ftp 共享目录
        private void btn_wcfinstall_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            setBAT.RunBat(xml.getSetXmlValue("path_service") + @"\Service_InstallUtil.bat");
            this.wcf_state.Text = getServiceState("OVIT_WebAppServer");
        }

        private void btn_wcfstart_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            setBAT.RunBat(xml.getSetXmlValue("path_service") + @"\Service_Start.bat");
            this.wcf_state.Text = getServiceState("OVIT_WebAppServer");
        }

        private void btn_wcfstop_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            setBAT.RunBat(xml.getSetXmlValue("path_service") + @"\Service_Stop.bat");
            this.wcf_state.Text = getServiceState("OVIT_WebAppServer");
        }

        private void btn_wcfremove_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            setBAT.RunBat(xml.getSetXmlValue("path_service") + @"\Service_UnInstallUtil.bat");
            this.wcf_state.Text = getServiceState("OVIT_WebAppServer");
        }

        private void btn_createweb_Click(object sender, EventArgs e)
        {

            setXml xml = new setXml();
            xml.setQuickConfig();
            setIIS iis = new setIIS();
            iis.DelSite("BaseFramework");
            iis.DelSite("HRWebApp");
            iis.DelSite("RealEstateRegister");
            iis.DelSite("Workflow");

            NewWebSiteInfo info_framework = new NewWebSiteInfo(xml.getSetXmlValue("web_ip"), "8024", "", "BaseFramework", xml.getSetXmlValue("path_framework"));
            NewWebSiteInfo info_workflow = new NewWebSiteInfo(xml.getSetXmlValue("web_ip"), "8028", "", "Workflow", xml.getSetXmlValue("path_workflow"));
            NewWebSiteInfo info_hr = new NewWebSiteInfo(xml.getSetXmlValue("web_ip"), "8030", "", "HRWebApp", xml.getSetXmlValue("path_hr"));
            NewWebSiteInfo info_bdc = new NewWebSiteInfo(xml.getSetXmlValue("web_ip"), "8025", "", "RealEstateRegister", xml.getSetXmlValue("path_bdc"));

            iis.CreateNewWebSite(info_framework, "BaseFramework");
            iis.CreateNewWebSite(info_workflow, "Workflow");
            iis.CreateNewWebSite(info_hr, "HRWebApp");
            iis.CreateNewWebSite(info_bdc, "RealEstateRegister");
            // MessageBox.Show("网站创建成功");
            setMessage.MessageShow("", "网站创建成功!", this.btn_createweb);

        }

        private void btn_updateappurl_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            try
            {
                setDB db = new setDB(xml.getSetXmlValue("user_framework"), xml.getSetXmlValue("password_framework"), this.datasource.Text);
                db.setAppInfo(xml.getSetXmlValue("par_szdmc"), xml.getSetXmlValue("web_ip"));
                // MessageBox.Show("appinfo设置完成。");
                setMessage.MessageShow("", "appinfo设置完成!", this.btn_updateappurl);
            }
            catch (Exception eg)
            {
                MessageBox.Show("请确保配置正确，并且framework用户能正常访问:\r\n" + eg.Message.ToString());
            }
        }

        private void btn_regasp4_Click(object sender, EventArgs e)
        {
            setBAT.RunBat(AppDomain.CurrentDomain.BaseDirectory + @"tool\regaspnet4iis.bat");
        }

        private void btn_createftp_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            //if (setOSUser.isUserExist(xml.getSetXmlValue("ftp_user")))
            //{
            //    setOSUser.RemoveUser(xml.getSetXmlValue("ftp_user"));
            //}
            if (!setOSUser.isUserExist(xml.getSetXmlValue("ftp_user")))
            {
                setOSUser.addUser(xml.getSetXmlValue("ftp_user"), xml.getSetXmlValue("ftp_password"));
            }

            setFTP ftp = new setFTP();
            ftp.delFtpSite("BDCFTP");
            ftp.createFTP("BDCFTP", xml.getSetXmlValue("path_ftp"), xml.getSetXmlValue("ftp_user"), xml.getSetXmlValue("ftp_ip"));
            //MessageBox.Show("ftp创建成功");
            setMessage.MessageShow("", "ftp创建成功!", this.btn_createftp);
        }

        private void btn_creategxml_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            setGXML gxml = new setGXML();
            gxml.SetFileRole(xml.getSetXmlValue("path_gxml"), xml.getSetXmlValue("gxml_user"));
            gxml.shareFolder(xml.getSetXmlValue("path_gxml"), "共享目录", "");
            //MessageBox.Show("共享目录创建成功");
            setMessage.MessageShow("", "共享目录创建成功!", this.btn_creategxml);
        }

        #endregion

        #region 数据库初始化

        private void setGCS()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("西安1980 3度分带 25度带", "2349");
            dic.Add("西安1980 3度分带 26度带", "2350");
            dic.Add("西安1980 3度分带 27度带", "2351");
            dic.Add("西安1980 3度分带 28度带", "2352");
            dic.Add("西安1980 3度分带 29度带", "2353");
            dic.Add("西安1980 3度分带 30度带", "2354");
            dic.Add("西安1980 3度分带 31度带", "2355");
            dic.Add("西安1980 3度分带 32度带", "2356");
            dic.Add("西安1980 3度分带 33度带", "2357");
            dic.Add("西安1980 3度分带 34度带", "2358");
            dic.Add("西安1980 3度分带 35度带", "2359");
            dic.Add("西安1980 3度分带 36度带", "2360");
            dic.Add("西安1980 3度分带 37度带", "2361");
            dic.Add("西安1980 3度分带 38度带", "2362");
            dic.Add("西安1980 3度分带 39度带", "2363");
            dic.Add("西安1980 3度分带 40度带", "2364");
            dic.Add("西安1980 3度分带 41度带", "2365");
            dic.Add("西安1980 3度分带 42度带", "2366");
            dic.Add("西安1980 3度分带 43度带", "2367");
            dic.Add("西安1980 3度分带 44度带", "2368");
            dic.Add("西安1980 3度分带 45度带", "2369");


            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Id", typeof(string));
            //然后把你想要加的数据加进DataTable 里
            foreach (var item in dic)
            {
                dt.Rows.Add(new object[] { item.Key, item.Value });
            }

            this.txt_GCS.Items.Clear();
            this.txt_GCS.DataSource = dt;
            this.txt_GCS.DisplayMember = "Name";
            this.txt_GCS.ValueMember = "Id";


        }

        private void btn_data_main_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_data_main.Text = Path == "" ? this.path_data_main.Text : Path;

            this.path_data_framework.Text = this.path_data_main.Text + "\\" + "framework.dmp";
            this.path_data_workflow.Text = this.path_data_main.Text + "\\" + "workflow.dmp";
            this.path_data_hr.Text = this.path_data_main.Text + "\\" + "hr.dmp";
            this.path_data_bdc.Text = this.path_data_main.Text + "\\" + "bdc.dmp";
            this.path_data_qjdc.Text = this.path_data_main.Text + "\\" + "qjdc.dmp";

            this.path_data_bdcdj.Text = this.path_data_main.Text + "\\" + "BDCDJ.gdb";
            this.path_data_bdcdj_his.Text = this.path_data_main.Text + "\\" + "BDCDJ_HIS.gdb";
            this.path_data_bdcdj_pre.Text = this.path_data_main.Text + "\\" + "BDCDJ_PRE.gdb";
        }

        private void btn_data_framework_Click(object sender, EventArgs e)
        {
            string Path = this.filePath("dmp");
            this.path_data_framework.Text = Path == "" ? this.path_data_framework.Text : Path;
        }

        private void btn_data_workflow_Click(object sender, EventArgs e)
        {
            string Path = this.filePath("dmp");
            this.path_data_workflow.Text = Path == "" ? this.path_data_workflow.Text : Path;
        }

        private void btn_data_hr_Click(object sender, EventArgs e)
        {
            string Path = this.filePath("dmp");
            this.path_data_hr.Text = Path == "" ? this.path_data_hr.Text : Path;
        }

        private void btn_data_bdc_Click(object sender, EventArgs e)
        {
            string Path = this.filePath("dmp");
            this.path_data_bdc.Text = Path == "" ? this.path_data_bdc.Text : Path;
        }

        private void btn_data_qjdc_Click(object sender, EventArgs e)
        {
            string Path = this.filePath("dmp");
            this.path_data_qjdc.Text = Path == "" ? this.path_data_qjdc.Text : Path;
        }

        private void btn_data_bdcdj_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_data_bdcdj.Text = Path == "" ? this.path_data_bdcdj.Text : Path;
        }

        private void btn_data_bdcdj_his_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_data_bdcdj_his.Text = Path == "" ? this.path_data_bdcdj_his.Text : Path;
        }

        private void btn_data_bdcdj_pre_Click(object sender, EventArgs e)
        {
            string Path = this.folderPath();
            this.path_data_bdcdj_pre.Text = Path == "" ? this.path_data_bdcdj_pre.Text : Path;
        }

        private void btn_createts_Click(object sender, EventArgs e)
        {
            setXml xml = new setXml();
            xml.setQuickConfig();
            try
            {
                setDB db = new setDB(xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), this.datasource.Text);

                string ansStr = "开始创建表空间\r\n";
                if (check_data_framework.Checked == true)
                {
                    bool ans = db.createTabelspace("TS_FRAMEWORK", xml.getSetXmlValue("path_datafolder") + "\\" + "TS_FRAMEWORK.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 TS_FRAMEWORK创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 TS_FRAMEWORK创建失败\r\n";
                    }
                }

                if (check_data_workflow.Checked == true)
                {
                    bool ans = db.createTabelspace("TS_WORKFLOW", xml.getSetXmlValue("path_datafolder") + "\\" + "TS_WORKFLOW.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 TS_WORKFLOW创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 TS_WORKFLOW创建失败\r\n";
                    }
                }

                if (check_data_hr.Checked == true)
                {
                    bool ans = db.createTabelspace("TS_HR", xml.getSetXmlValue("path_datafolder") + "\\" + "TS_HR.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 TS_HR创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 TS_HR创建失败\r\n";
                    }
                }

                if (check_data_bdc.Checked == true)
                {
                    bool ans = db.createTabelspace("TS_BDCDJSP", xml.getSetXmlValue("path_datafolder") + "\\" + "TS_BDCDJSP.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 TS_BDCDJSP创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 TS_BDCDJSP创建失败\r\n";
                    }
                }

                if (check_data_qjdc.Checked == true)
                {
                    bool ans = db.createTabelspace("TS_QJDC", xml.getSetXmlValue("path_datafolder") + "\\" + "TS_QJDC.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 TS_QJDC创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 TS_QJDC创建失败\r\n";
                    }
                }

                if (check_data_bdcdj.Checked == true)
                {
                    bool ans = db.createTabelspace("GEO_BDCDJ", xml.getSetXmlValue("path_datafolder") + "\\" + "GEO_BDCDJ.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 GEO_BDCDJ创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 GEO_BDCDJ创建失败\r\n";
                    }
                }

                if (check_data_bdcdj_his.Checked == true)
                {
                    bool ans = db.createTabelspace("GEO_BDCDJ_HIS", xml.getSetXmlValue("path_datafolder") + "\\" + "GEO_BDCDJ_HIS.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 GEO_BDCDJ_HIS创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 GEO_BDCDJ_HIS创建失败\r\n";
                    }
                }

                if (check_data_bdcdj_pre.Checked == true)
                {
                    bool ans = db.createTabelspace("GEO_BDCDJ_PRE", xml.getSetXmlValue("path_datafolder") + "\\" + "GEO_BDCDJ_PRE.DBF", "50m");
                    if (ans == true)
                    {
                        ansStr += "表空间 GEO_BDCDJ_PRE创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "表空间 GEO_BDCDJ_PRE创建失败\r\n";
                    }
                }
                ansStr += "结束创建\r\n";
                MessageBox.Show(ansStr);

            }
            catch (Exception eg)
            {
                MessageBox.Show(eg.Message.ToString());
            }
        }


        private void btn_createuser_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("创建用户会先drop掉原有用户和用户对象\r\n,请先备份数据库!\r\n继续请点击确定，放弃请点击取消。", "创建用户", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }

            setXml xml = new setXml();
            xml.setQuickConfig();
            try
            {
                setDB db = new setDB(xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), this.datasource.Text);

                string ansStr = "开始创建用户\r\n";
                if (check_data_framework.Checked == true)
                {
                    if (db.isUserExist(xml.getSetXmlValue("user_framework")))
                    {
                        db.deleteUser(xml.getSetXmlValue("user_framework"));
                        ansStr += "用户 " + xml.getSetXmlValue("user_framework") + "drop成功\r\n";
                    }

                    bool ans = db.createUser(xml.getSetXmlValue("user_framework"), xml.getSetXmlValue("password_framework"), "TS_FRAMEWORK");
                    if (ans == true)
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_framework") + "创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_framework") + "创建失败\r\n";
                    }
                }

                if (check_data_workflow.Checked == true)
                {
                    if (db.isUserExist(xml.getSetXmlValue("user_workflow")))
                    {
                        db.deleteUser(xml.getSetXmlValue("user_workflow"));
                        ansStr += "用户 " + xml.getSetXmlValue("user_workflow") + "drop成功\r\n";
                    }
                    bool ans = db.createUser(xml.getSetXmlValue("user_workflow"), xml.getSetXmlValue("password_workflow"), "TS_WORKFLOW");
                    if (ans == true)
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_workflow") + "创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_workflow") + "创建失败\r\n";
                    }
                }

                if (check_data_hr.Checked == true)
                {
                    if (db.isUserExist(xml.getSetXmlValue("user_hr")))
                    {
                        db.deleteUser(xml.getSetXmlValue("user_hr"));
                        ansStr += "用户 " + xml.getSetXmlValue("user_hr") + "drop成功\r\n";
                    }
                    bool ans = db.createUser(xml.getSetXmlValue("user_hr"), xml.getSetXmlValue("password_hr"), "TS_HR");
                    if (ans == true)
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_hr") + "创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_hr") + "创建失败\r\n";
                    }
                }

                if (check_data_bdc.Checked == true)
                {
                    if (db.isUserExist(xml.getSetXmlValue("user_bdc")))
                    {
                        db.deleteUser(xml.getSetXmlValue("user_bdc"));
                        ansStr += "用户 " + xml.getSetXmlValue("user_bdc") + "drop成功\r\n";
                    }
                    bool ans = db.createUser(xml.getSetXmlValue("user_bdc"), xml.getSetXmlValue("password_bdc"), "TS_BDCDJSP");
                    if (ans == true)
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_bdc") + "创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_bdc") + "创建失败\r\n";
                    }
                }

                if (check_data_qjdc.Checked == true)
                {
                    if (db.isUserExist(xml.getSetXmlValue("user_qjdc")))
                    {
                        db.deleteUser(xml.getSetXmlValue("user_qjdc"));
                        ansStr += "用户 " + xml.getSetXmlValue("user_qjdc") + "drop成功\r\n";
                    }
                    bool ans = db.createUser(xml.getSetXmlValue("user_qjdc"), xml.getSetXmlValue("password_qjdc"), "TS_QJDC");
                    if (ans == true)
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_qjdc") + "创建成功\r\n";
                    }
                    else
                    {
                        ansStr += "用户 " + xml.getSetXmlValue("user_qjdc") + "创建失败\r\n";
                    }
                }
                ansStr += "结束创建\r\n";
                MessageBox.Show(ansStr);

            }
            catch (Exception eg)
            {
                MessageBox.Show(eg.Message.ToString());
            }

        }

        private void btn_imp_framework_Click(object sender, EventArgs e)
        {
            if (!checkAppFolder())
            {
                return;
            }

            string imp_tem_path = AppDomain.CurrentDomain.BaseDirectory + @"data";
            string imp_path = AppDomain.CurrentDomain.BaseDirectory + @"dataTemp";
            setConfig config = new setConfig();

            setXml xml = new setXml();
            xml.setQuickConfig();

            string ansStr = "开始数据\r\n";
            if (check_data_framework.Checked == true)
            {
                config.repalcePar(xml,imp_tem_path, "imp_framework.bat", imp_path);
                setBAT.RunBat(imp_path + @"\imp_framework.bat");
                ansStr += "基础框架数据导入完成\r\n";
            }

            if (check_data_workflow.Checked == true)
            {
                config.repalcePar(xml,imp_tem_path, "imp_workflow.bat", imp_path);
                setBAT.RunBat(imp_path + @"\imp_workflow.bat");
                ansStr += "工作流据导入完成\r\n";
            }

            if (check_data_hr.Checked == true)
            {
                config.repalcePar(xml,imp_tem_path, "imp_hr.bat", imp_path);
                setBAT.RunBat(imp_path + @"\imp_hr.bat");
                ansStr += "人事据导入完成\r\n";
            }

            if (check_data_bdc.Checked == true)
            {
                config.repalcePar(xml,imp_tem_path, "imp_bdc.bat", imp_path);
                setBAT.RunBat(imp_path + @"\imp_bdc.bat");
                ansStr += "不动产导入完成\r\n";
            }

            if (check_data_qjdc.Checked == true)
            {
                config.repalcePar(xml,imp_tem_path, "imp_qjdc.bat", imp_path);
                setBAT.RunBat(imp_path + @"\imp_qjdc.bat");
                ansStr += "权籍调查数据导入完成\r\n";
            }
            ansStr += "导入结束\r\n";
            MessageBox.Show(ansStr);

        }

        #endregion

        private void btn_createsde_Click(object sender, EventArgs e)
        {
            setArcgis.init();
            EngineDatabase engine = new EngineDatabase();
           
            setXml xml = new setXml();
            xml.setQuickConfig();
            setDB db = new setDB(xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), this.datasource.Text);

            if (this.check_data_bdcdj.Checked == true)
            {
                if (db.isUserExist(xml.getSetXmlValue("user_sde")))
                {
                    // MessageBox.Show("现势库已存在");       

                    if (MessageBox.Show("现势库已存在,是否删除已有的现势库", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        bool res = db.deleteUser(xml.getSetXmlValue("user_sde"));
                        if (res == true)
                        {
                            MessageBox.Show("现势库删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("现势库删除失败!");
                        }

                    }
                }
                else
                {
                    string ans1 = engine.createSDE("Oracle", xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), xml.getSetXmlValue("user_sde"), xml.getSetXmlValue("password_sde"), "GEO_BDCDJ", AppDomain.CurrentDomain.BaseDirectory + @"\tool\Server101.ecp");
                    MessageBox.Show("现势库创建结果如下:\r\n" + ans1);
                }
            }

            if (this.check_data_bdcdj_his.Checked == true)
            {

                if (db.isUserExist(xml.getSetXmlValue("user_sde_his")))
                {
                    // MessageBox.Show("历史库已存在");

                    if (MessageBox.Show("历史库已存在,是否删除已有的历史库", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        bool res = db.deleteUser(xml.getSetXmlValue("user_sde_his"));
                        if (res == true)
                        {
                            MessageBox.Show("历史库删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("历史库删除失败!");
                        }

                    }
                }
                else
                {
                    string ans1 = engine.createSDE("Oracle", xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), xml.getSetXmlValue("user_sde_his"), xml.getSetXmlValue("password_sde_his"), "GEO_BDCDJ_HIS", AppDomain.CurrentDomain.BaseDirectory + @"\tool\Server101.ecp");

                    MessageBox.Show("历史库创建结果如下:\r\n" + ans1);
                }
            }

            if (this.check_data_bdcdj_pre.Checked == true)
            {

                if (db.isUserExist(xml.getSetXmlValue("user_sde_pre")))
                {
                    //  MessageBox.Show("临时库已存在");

                    if (MessageBox.Show("临时库已存在,是否删除已有的临时库", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        bool res = db.deleteUser(xml.getSetXmlValue("user_sde_pre"));
                        if (res == true)
                        {
                            MessageBox.Show("临时库删除成功!");
                        }
                        else
                        {
                            MessageBox.Show("临时库删除失败!");
                        }

                    }
                }
                else
                {
                    string ans1 = engine.createSDE("Oracle", xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), xml.getSetXmlValue("user_sde_pre"), xml.getSetXmlValue("password_sde_pre"), "GEO_BDCDJ_PRE", AppDomain.CurrentDomain.BaseDirectory + @"\tool\Server101.ecp");

                    MessageBox.Show("临时库创建结果如下:\r\n" + ans1);
                }
            }

            MessageBox.Show("企业空间库操作结束");


        }

        private void btn_initializtionSDE_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("初始化空间库，会删除空间库原有内容，\r\n,请先备空间库库!\r\n继续请点击确定，放弃请点击取消。", "初始化空间库", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }

            if (!checkAppFolder())
            {
                return;
            }
            setArcgis.init();
            //初始化 esri授权
            IAoInitialize pao = new AoInitializeClass();
            pao.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);

            EngineDatabase engine = new EngineDatabase();
          
            setXml xml = new setXml();
            xml.setQuickConfig();
            setDB db = new setDB(xml.getSetXmlValue("user_system"), xml.getSetXmlValue("password_system"), this.datasource.Text);

            if (this.check_data_bdcdj.Checked == true)
            {

                IWorkspace workspace = null;
                IFeatureWorkspace featureworkspace = null;
                IFeatureDataset featuredataset = null;
                workspace = engine.getSDEWorkspace(xml.getSetXmlValue("wcf_ip"), "sde:oracle10g:" + xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_sde"), xml.getSetXmlValue("password_sde"));
                string ans1 = engine.importGDB2SDE(workspace, featureworkspace, featuredataset, xml.getSetXmlValue("path_data_bdcdj"), Convert.ToInt32(xml.getSetXmlValue("par_gcs")), xml.getSetXmlValue("user_sde"));
                MessageBox.Show("现势库创建结果如下:\r\n" + ans1);
                Marshal.ReleaseComObject(workspace);
            }

            if (this.check_data_bdcdj_his.Checked == true)
            {
                string ans1 = "";
                if (db.isUserExist(xml.getSetXmlValue("user_sde_his")))
                {
                    db.grantUser(xml.getSetXmlValue("user_sde_his"));
                    ans1 += "用户 " + xml.getSetXmlValue("user_sde_his") + "授权成功\r\n";
                }

                IWorkspace workspace = null;
                IFeatureWorkspace featureworkspace = null;
                IFeatureDataset featuredataset = null;
                workspace = engine.getSDEWorkspace(xml.getSetXmlValue("wcf_ip"), "sde:oracle10g:" + xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_sde_his"), xml.getSetXmlValue("password_sde_his"));
                ans1 += engine.importGDB2SDE(workspace, featureworkspace, featuredataset, xml.getSetXmlValue("path_data_bdcdj_his"), Convert.ToInt32(xml.getSetXmlValue("par_gcs")), xml.getSetXmlValue("user_sde_his"));
                MessageBox.Show("历史库创建结果如下:\r\n" + ans1);
                Marshal.ReleaseComObject(workspace);
            }

            if (this.check_data_bdcdj_pre.Checked == true)
            {

                string ans1 = "";
                if (db.isUserExist(xml.getSetXmlValue("user_sde_pre")))
                {
                    db.grantUser(xml.getSetXmlValue("user_sde_pre"));
                    ans1 += "用户 " + xml.getSetXmlValue("user_sde_pre") + "授权成功\r\n";
                }

                IWorkspace workspace = null;
                IFeatureWorkspace featureworkspace = null;
                IFeatureDataset featuredataset = null;
                workspace = engine.getSDEWorkspace(xml.getSetXmlValue("wcf_ip"), "sde:oracle10g:" + xml.getSetXmlValue("datasource"), xml.getSetXmlValue("user_sde_pre"), xml.getSetXmlValue("password_sde_pre"));
                ans1 += engine.importGDB2SDE(workspace, featureworkspace, featuredataset, xml.getSetXmlValue("path_data_bdcdj_pre"), Convert.ToInt32(xml.getSetXmlValue("par_gcs")), xml.getSetXmlValue("user_sde_pre"));
                MessageBox.Show("临时库创建结果如下:\r\n" + ans1);
                Marshal.ReleaseComObject(workspace);
            }

            MessageBox.Show("企业空间库初始化完成");
        }

        private void btn_backup_Click(object sender, EventArgs e)
        {
            string exp_tem_path = AppDomain.CurrentDomain.BaseDirectory + @"data";
            string exp_path = AppDomain.CurrentDomain.BaseDirectory + @"dataTemp";

            setBackup backup = new setBackup();

            QuickConfig.setXml xml = new QuickConfig.setXml();
            xml.setQuickConfig();

            DataTable parTable = xml.readXML();

            //获取备份设置参数
            string backupcontent = parTable.Select("name='backup_content'")[0][1].ToString();
            bool[] backupBool = null;
            if (backupcontent != "")
            {
                string[] backupContent = backupcontent.Split(',');
                backupBool = new bool[backupContent.Length];
                for (int i = 0; i < backupBool.Length; i++)
                {
                    backupBool[i] = backupContent[i] == "True" ? true : false;
                }

            }

            backup.backup(backupBool,AppDomain.CurrentDomain.BaseDirectory,AppDomain.CurrentDomain.BaseDirectory+"\\config\\set.xml");
            //打开备份目录
            backup.openFolderAndLog();


        }

        private void check_backup_db_allnotchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_db_framework.Checked = false;
            this.check_backup_db_workflow.Checked = false;
            this.check_backup_db_hr.Checked = false;
            this.check_backup_db_bdc.Checked = false;
            this.check_backup_db_qjdc.Checked = false;
        }

        private void check_backup_db_allchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_db_framework.Checked = true;
            this.check_backup_db_workflow.Checked = true;
            this.check_backup_db_hr.Checked = true;
            this.check_backup_db_bdc.Checked = true;
            this.check_backup_db_qjdc.Checked = true;
        }

        private void check_backup_sde_allnotchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_sde_sde.Checked = false;
            this.check_backup_sde_sde_his.Checked = false;
            this.check_backup_sde_sde_pre.Checked = false;
        }

        private void check_backup_sde_allchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_sde_sde.Checked = true;
            this.check_backup_sde_sde_his.Checked = true;
            this.check_backup_sde_sde_pre.Checked = true;
        }

        private void check_backup_app_allnotchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_app_wcf.Checked = false;
            this.check_backup_app_framework.Checked = false;
            this.check_backup_app_workflow.Checked = false;
            this.check_backup_app_hr.Checked = false;
            this.check_backup_app_bdc.Checked = false;
            this.check_backup_app_qjdc.Checked = false;

        }

        private void check_backup_app_allchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_app_wcf.Checked = true;
            this.check_backup_app_framework.Checked = true;
            this.check_backup_app_workflow.Checked = true;
            this.check_backup_app_hr.Checked = true;
            this.check_backup_app_bdc.Checked = true;
            this.check_backup_app_qjdc.Checked = true;
        }

        private void check_backup_file_allnotchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_files_ftp.Checked = false;
            this.check_backup_files_gxml.Checked = false;

        }

        private void check_backup_file_allchoose_CheckedChanged(object sender, EventArgs e)
        {
            this.check_backup_files_ftp.Checked = true;
            this.check_backup_files_gxml.Checked = true;
        }

        private void btn_backup_serviceInstall_Click(object sender, EventArgs e)
        {
            setBAT.RunBat(AppDomain.CurrentDomain.BaseDirectory + @"Services\Service_Install.bat");
            this.lab_backupServicesState.Text = getServiceState("QuickConfig_AutoBackup");
        }

        private void btn_backup_serviceStart_Click(object sender, EventArgs e)
        {
            setBAT.RunBat(AppDomain.CurrentDomain.BaseDirectory + @"Services\Service_Start.bat");
            this.lab_backupServicesState.Text = getServiceState("QuickConfig_AutoBackup");
        }

        private void btn_backup_serviceStop_Click(object sender, EventArgs e)
        {

            setBAT.RunBat(AppDomain.CurrentDomain.BaseDirectory + @"Services\Service_Stop.bat");
            this.lab_backupServicesState.Text = getServiceState("QuickConfig_AutoBackup");
        }

        private void btn_backup_serviceUninstall_Click(object sender, EventArgs e)
        {

            setBAT.RunBat(AppDomain.CurrentDomain.BaseDirectory + @"Services\Service_Unistall.bat");
            this.lab_backupServicesState.Text = getServiceState("QuickConfig_AutoBackup");
        }



    }
}
