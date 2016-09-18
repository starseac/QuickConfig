using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.IO;

namespace AutoBackup
{
    public partial class AutoBackupServices : ServiceBase
    {
        public AutoBackupServices()
        {
            InitializeComponent();
        }

        System.IO.StreamWriter sw;

        string path = AppDomain.CurrentDomain.BaseDirectory + "log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

        string parentFolder = AppDomain.CurrentDomain.BaseDirectory.Replace("\\Services", "");


        int DayCount = 0;

        /// <summary>
        /// 间隔秒数
        /// </summary>
        int intervalSecond = 30;

        protected override void OnStart(string[] args)
        {
            write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "服务启动.");

            StartDoSomething();

        }

        protected override void OnStop()
        {
            write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "服务停止.");

        }


        private void write(string content)
        {
            try
            {
                sw = new StreamWriter(path, true);
                sw.Write(DateTime.Now.ToString(content + "\r\n"));
            }
            catch { }
            finally
            {
                sw.Close();
                sw.Dispose();
            }

        }

        private void StartDoSomething()
        {
            System.Timers.Timer timer = new System.Timers.Timer(intervalSecond * 1000); //间隔
            timer.AutoReset = true;
            timer.Enabled = false;  //执行一次
            timer.Elapsed += new ElapsedEventHandler(ExecutionCode);
            timer.Start();
        }
        /// <summary>
        /// 判断 执行时间是否在当前时间后，是的话，返回true
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="min"></param>
        /// <param name="second"></param>
        /// <param name="now_hour"></param>
        /// <param name="now_min"></param>
        /// <param name="now_second"></param>
        /// <returns></returns>
        private bool JuidgeTime(int hour,int min,int second,int now_hour,int now_min,int now_second) {
           
                if ((now_hour * 3600 + now_min * 60 + now_second) >= (hour * 3600 + min * 60 + second) && (now_hour * 3600 + now_min * 60 + now_second) <= (hour * 3600 + min * 60 + second) + intervalSecond)
                {
                    return true;
                }
                else
                {
                    return false;
                }          

        }

        private void ExecutionCode(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                string now_week = Day[Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"))].ToString();


                int now_year = DateTime.Now.Year;
                int now_month = DateTime.Now.Month;
                int now_day = DateTime.Now.Day;

                int now_day_hour = DateTime.Now.Hour;
                int now_day_min = DateTime.Now.Minute;
                int now_day_second = DateTime.Now.Second;


                QuickConfig.setBackup backup = new QuickConfig.setBackup();

                QuickConfig.setXml xml = new QuickConfig.setXml();
                xml.setXMLPath(parentFolder + "\\config\\set.xml");

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

                string backuptype = parTable.Select("name='backup_type'")[0][1].ToString();

                string backup_day_starttime = parTable.Select("name='backup_type_daytime'")[0][1].ToString();

                string chooseWeekStr = parTable.Select("name='backup_type_week'")[0][1].ToString();
                string backup_week_starttime = parTable.Select("name='backup_type_weektime'")[0][1].ToString();

                string chooseMonthStr = parTable.Select("name='backup_type_month'")[0][1].ToString();
                string backup_month_starttime = parTable.Select("name='backup_type_monthtime'")[0][1].ToString();

                // 备份类型 为 每天   什么时间  
                if (backuptype == "" || backuptype == "每天")
                {
                    string[] str = backup_day_starttime.Split(':');
                    int hour = Convert.ToInt32(str[0]);
                    int miniute = Convert.ToInt32(str[1]);
                    int second = Convert.ToInt32(str[2]);

                    if (JuidgeTime(hour,miniute,second,now_day_hour,now_day_min,now_day_second) && DayCount == 0)
                    {
                        DayCount = 1;
                        write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份类型为【每天】的自动备份开始执行！.");
                        backup.backup(backupBool, parentFolder, parentFolder + "\\config\\set.xml");
                        write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份结束！.");

                    }

                    if (!JuidgeTime(hour, miniute, second, now_day_hour, now_day_min, now_day_second))
                    {

                        DayCount = 0;
                    }
                }
                // 备份类型为 每周   周几  什么时间
                else if (backuptype == "每周")
                {


                    string[] str = backup_week_starttime.Split(':');
                    int hour = Convert.ToInt32(str[0]);
                    int miniute = Convert.ToInt32(str[1]);
                    int second = Convert.ToInt32(str[2]);


                    if (JuidgeTime(hour, miniute, second, now_day_hour, now_day_min, now_day_second) && chooseWeekStr.Contains(now_week) && DayCount == 0)
                    {
                        DayCount = 1;
                        write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份类型为【每周】【" + now_week + "】的自动备份开始执行！.");
                        backup.backup(backupBool, parentFolder, parentFolder + "\\config\\set.xml");
                        write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份结束！.");

                    }

                    if (!JuidgeTime(hour, miniute, second, now_day_hour, now_day_min, now_day_second))
                    {

                        DayCount = 0;
                    }
                }

               //备份类型 为 每月   几号   什么时间
                else if (backuptype == "每月")
                {
                    string[] str = backup_month_starttime.Split(':');
                    int hour = Convert.ToInt32(str[0]);
                    int miniute = Convert.ToInt32(str[1]);
                    int second = Convert.ToInt32(str[2]);

                    int[] days = null;

                    if (chooseMonthStr != "")
                    {
                        string[] daysStr = chooseMonthStr.Split(',');
                        days = new int[daysStr.Length];
                        for (int i = 0; i < daysStr.Length; i++)
                        {
                            days[i] = Convert.ToInt32(daysStr[i].ToString());

                        }

                    }

                    if (JuidgeTime(hour, miniute, second, now_day_hour, now_day_min, now_day_second) && days.Contains(now_day) && DayCount == 0)
                    {
                        DayCount = 1;
                        write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份类型为【每月】【" + now_day + "号】的自动备份开始执行！.");
                        backup.backup(backupBool, parentFolder, parentFolder + "\\config\\set.xml");
                        write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份结束！.");

                    }

                    if (!JuidgeTime(hour, miniute, second, now_day_hour, now_day_min, now_day_second))
                    {

                        DayCount = 0;
                    }
                }
            }
            catch (Exception eg)
            {
                write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + ":备份出现异常：" + eg.Message.ToString());
            }
        }

    }
}
