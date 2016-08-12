using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.DirectoryServices;
using Microsoft.Web.Administration;
using System.Diagnostics;
using System.Security.AccessControl;
using System.IO;



namespace QuickConfig
{
   public class setIIS
   {

       #region 应用程序池
       public string getIISversion() {
           DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
           string Version = getEntity.Properties["MajorIISVersionNumber"].Value.ToString();
           return Version;
       }


       /// <summary>
       /// 判断程序池是否存在
       /// </summary>
       /// <param name="AppPoolName">程序池名称</param>
       /// <returns>true存在 false不存在</returns>
       private bool IsAppPoolName(string AppPoolName)
       {
           bool result = false;
           DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
           foreach (DirectoryEntry getdir in appPools.Children)
           {
               if (getdir.Name.Equals(AppPoolName))
               {
                   result = true;
               }
           }
           return result;
       }


       /// <summary>
       /// 删除指定程序池
       /// </summary>
       /// <param name="AppPoolName">程序池名称</param>
       /// <returns>true删除成功 false删除失败</returns>
       public bool DeleteAppPool(string AppPoolName)
       {
           bool result = false;
           DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
           foreach (DirectoryEntry getdir in appPools.Children)
           {
               if (getdir.Name.Equals(AppPoolName))
               {
                   try
                   {
                       getdir.DeleteTree();
                       result = true;
                   }
                   catch
                   {
                       result = false;
                   }
               }
           }
           return result;
       }

       public bool CreateAppPool(string appPoolName)
       {
           string AppPoolName = appPoolName;
           if (!IsAppPoolName(AppPoolName))
           {
               DirectoryEntry newpool;
               DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
               newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
               newpool.CommitChanges();
           }else{
               return false;
           }

           // #region 修改应用程序的配置(包含托管模式及其NET运行版本)
           ServerManager sm = new ServerManager();
           sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "v4.0";
           sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Integrated; //托管模式Integrated为集成 Classic为经典
           sm.CommitChanges();
           return true;

       }


       public void aspnet_regiis()
       {
           ////启动aspnet_regiis.exe程序 
           //string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
           //ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
           ////处理目录路径 
           //string path = vdEntry.Path.ToUpper();
           //int index = path.IndexOf("W3SVC");
           //path = path.Remove(0, index);
           ////启动ASPnet_iis.exe程序,刷新脚本映射 
           //startInfo.Arguments = "-s " + path;
           //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
           //startInfo.UseShellExecute = false;
           //startInfo.CreateNoWindow = true;
           //startInfo.RedirectStandardOutput = true;
           //startInfo.RedirectStandardError = true;
           //Process process = new Process();
           //process.StartInfo = startInfo;
           //process.Start();
           //process.WaitForExit();
           //string errors = process.StandardError.ReadToEnd();
       
       
       }

   

       #endregion 


       string entPath = String.Format("IIS://{0}/w3svc", "localhost");
       public DirectoryEntry GetDirectoryEntry(string entPath)
       {
           DirectoryEntry ent = new DirectoryEntry(entPath);
           return ent;
       }


   

       /// <summary>
    /// 创建网站
    /// </summary>
    /// <param name="siteInfo"></param>
      public  void CreateNewWebSite(NewWebSiteInfo siteInfo,string poolname)
        {
            if (isWebExist(siteInfo.CommentOfWebSite))
            {
                return;
            }
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
            string   newSiteNum = GetNewWebSiteID().ToString();
            DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum, "IIsWebServer");
            newSiteEntry.CommitChanges();
            newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
            newSiteEntry.Properties["ServerComment"].Value = siteInfo.CommentOfWebSite;
            newSiteEntry.CommitChanges();

            DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
            vdEntry.CommitChanges();
            string ChangWebPath = siteInfo.WebPath.Trim();
            vdEntry.Properties["Path"].Value = ChangWebPath;

            vdEntry.Invoke("AppCreate", true);//创建应用程序
            vdEntry.Properties["AccessRead"][0] = true; //设置读取权限
            vdEntry.Properties["AccessWrite"][0] = true;
            vdEntry.Properties["AccessScript"][0] = true;//执行权限
            vdEntry.Properties["AccessExecute"][0] = false;
            vdEntry.Properties["DefaultDoc"][0] = "Login.aspx";//设置默认文档
            vdEntry.Properties["AppFriendlyName"][0] = poolname; //应用程序名称           
            vdEntry.Properties["AuthFlags"][0] = 1;//0表示不允许匿名访问,1表示就可以3为基本身份验证，7为windows继承身份验证
            vdEntry.CommitChanges();
           
            #region 针对IIS7
            DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
            int Version =int.Parse(getEntity.Properties["MajorIISVersionNumber"].Value.ToString());
            if (Version > 6)
            {
                #region 创建应用程序池
                string AppPoolName = poolname;
                if (!IsAppPoolName(AppPoolName))
                {
                    DirectoryEntry newpool;
                    DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                    newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                    newpool.CommitChanges();
                }
                #endregion
                #region 修改应用程序的配置(包含托管模式及其NET运行版本)
                ServerManager sm = new ServerManager();
                sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "v4.0";
                sm.ApplicationPools[AppPoolName].Enable32BitAppOnWin64 = true;
                sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Integrated; //托管模式Integrated为集成 Classic为经典
                sm.CommitChanges();
                #endregion
                vdEntry.Properties["AppPoolId"].Value = AppPoolName;
                vdEntry.CommitChanges();
            }
            #endregion

            //启动aspnet_regiis.exe程序 
            string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            //处理目录路径 
            string path = vdEntry.Path.ToUpper();
            int index = path.IndexOf("W3SVC");
            path = path.Remove(0, index);
            //启动ASPnet_iis.exe程序,刷新脚本映射 
            startInfo.Arguments = "-s " + path;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            string errors = process.StandardError.ReadToEnd();
            if (errors != string.Empty)
            {
                throw new Exception(errors);
            }
        }

      private int GetNewWebSiteID()
      {
          //DirectoryEntry root = new DirectoryEntry("IIS://" + "localhost" + "/W3SVC");
          //int siteID = 1;
          //foreach (DirectoryEntry e in root.Children)
          //{
          //    if (e.SchemaClassName == "IIsWebServer")
          //    {
          //        int ID = Convert.ToInt32(e.Name);
          //        if (ID >= siteID) { siteID = ID + 1; }
          //    }
          //}
          //return siteID;

          ServerManager sm = new ServerManager();
          int siteID =  1;

          foreach (Site e in sm.Sites)
          {
             
                  int ID = Convert.ToInt32(e.Id);
                  if (ID >= siteID) { siteID = ID + 1; }
             
          }
          return siteID;

      }

      /// <summary>
      /// 获取网站列表
      /// </summary>
      /// <returns></returns>
      public List<string[]> GetWebSiteList()
      {
          try
          {
              List<string[]> infoList = new List<string[]>();
              DirectoryEntry deRoot = new DirectoryEntry("IIS://localhost/W3SVC");
             // DirectoryEntry deRoot = new DirectoryEntry("IIS://localhost");
              foreach (DirectoryEntry e in deRoot.Children)
              {
                  PropertyValueCollection pvc = e.Properties["ServerBindings"];
                  if (pvc.Count <= 0)
                      continue;
                  String[] srvBindings = pvc[0].ToString().Split(':');
                  string IpAddress = srvBindings[0];//绑定的IP地址
                  string Port = srvBindings[1];//对应端口号
                  string index = e.Name;//所在站点索引
                  string name = e.Properties["ServerComment"].Value.ToString();    //网站名称          
                  string[] parStr = new string[4];
                  parStr[0] = IpAddress;
                  parStr[1] = Port;
                  parStr[2] = index;
                  parStr[3] = name;
                  infoList.Add(parStr);
              }
              return infoList;
          }
          catch (Exception ex)
          {

              return null;
          }
      }

      private bool isWebExist(string webname)
      {
          List<string[]> weblist = GetWebSiteList();

          foreach( string [] aa in weblist){
              if (webname == aa[3]) {
                  return true;
              }          
          }
          return false;
      }
     

     


    /// <summary>
 /// 删除站点
 /// </summary>
 /// <param name="WebSiteName">站点名</param>
 /// <returns>成功或失败信息!</returns>
      public  string DelSite(string WebSiteName)
      {
          try
          {
              string SiteID = GetSiteID(WebSiteName);
                if (SiteID == "") return "error:该站点不存在.";

              DirectoryEntry deRoot = new DirectoryEntry("IIS://localhost/W3SVC");
              try
              {
                  DirectoryEntry deVDir = new DirectoryEntry();
                  deRoot.RefreshCache();
                  deVDir = deRoot.Children.Find(SiteID, "IIsWebServer");
                  deRoot.Children.Remove(deVDir);

                  deRoot.CommitChanges();
                  deRoot.Close();
                  return "successful:删除站点" + WebSiteName + "成功!";
              }
              catch (System.Exception)
              {
                  return "error:该站点不存在.";
              }
          }
          catch (Exception e)
          {
              return "error:删除站点失败." + e.Message;
          }
      }

      private  string GetSiteID(string WebSiteName)
      {
          List<string[]> weblist = GetWebSiteList();
          foreach (string[] aa in weblist) {
              if (aa[3] == WebSiteName) {
                  return aa[2];
                 
              }
          
          }
          return "";
      }

      //#region<<虚拟目录>>
      /// <summary>
      /// 创建虚拟目录
      /// </summary>
      /// <param >虚拟目录别名</param>
      /// <param >内容所在路径</param>
      public static bool CreateVirtualDirectory(string webSite, string virtualName, string path)
      {
          if (virtualName.Equals(string.Empty) || (!Directory.Exists(path)))
          {
             // MessageBox.Show("1.虚拟目录别名不能为空/r/n 2.虚拟目录内容所在路径可能不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return false;
          }
          try
          {
              DirectoryEntry tbEntry = FindVirtualDir(webSite, virtualName);//查找虚拟目录
              if (tbEntry == null) //找不到则创建虚拟目录
              {
                  string IISpath = "IIS://localhost/W3SVC/" + webSite + "/ROOT";//IIS参数格式
                  System.DirectoryServices.DirectoryEntry root = new DirectoryEntry(IISpath);
                  tbEntry = root.Children.Add(virtualName, root.SchemaClassName);//创建虚拟目录
              }
              return UpdateVirDir(tbEntry, path, virtualName);
          }
          catch (Exception ex)
          {
            //  MessageBox.Show("1.请确认IIS是否已经安装/r/n2.虚拟目录有可能已经存在/r/n (Error：" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //  WriterLog("CreateVir:/r/n" + ex.ToString());
              return false;
          }
      }
    
      /// <summary>
      /// 删除虚拟目录
      /// </summary>
      /// <param ></param>
      /// <param ></param>
      /// <returns></returns>
      public static bool DelVirtualDirectory(string webSite, string nameDirectory)
      {
          try
          {
              DirectoryEntry deRoot = new DirectoryEntry("IIS://localhost/W3SVC/" + webSite + "/ROOT");
              try
              {
                  deRoot.Children.Remove(FindVirtualDir(webSite, nameDirectory));
                  deRoot = null;
                  return true;
              }
              catch
              {
                  return true;
              }
          }
          catch (Exception e)
          {
             // WriterLog(e.ToString());
              return false;
          }
      }
      /// <summary>
      /// 修改虚拟目录
      /// </summary>
      /// <param ></param>
      /// <param ></param>
      /// <param ></param>
      /// <returns></returns>
      public static bool UpdateVirDir(DirectoryEntry tbEntry, string path, string virtualName)
      {
          if (tbEntry == null)
              return false;
          tbEntry.Properties["Path"][0] = path;//物理目录
          tbEntry.Invoke("AppCreate", true);//创建应用程序
          tbEntry.Properties["AccessRead"][0] = true;//允许访问
          tbEntry.Properties["ContentIndexed"][0] = true; ;//
          tbEntry.Properties["DefaultDoc"][0] = "index.asp,Default.aspx";
          tbEntry.Properties["AppFriendlyName"][0] = virtualName;//应用程序名
          tbEntry.Properties["AccessScript"][0] = true;//访问权限
          tbEntry.Properties["DontLog"][0] = true;//日志
          tbEntry.Properties["AuthFlags"][0] = 1;//验证,0不，1允许，3基本身份
          tbEntry.CommitChanges();//开始应用属性
          return true;
      }
      /// <summary>
      /// 查找虚拟目录
      /// </summary>
      /// <param ></param>
      /// <returns></returns>
      public static DirectoryEntry FindVirtualDir(string webSite, string virtualName)
      {
          if (virtualName.Equals(string.Empty))
          {
             // MessageBox.Show("虚拟目录别名不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              return null;
          }
          try
          {
              string IISpath = "IIS://localhost/W3SVC/" + webSite + "/ROOT";//IIS参数格式
              System.DirectoryServices.DirectoryEntry root = new DirectoryEntry(IISpath);//实例IIS类
              return root.Children.Find(virtualName, root.SchemaClassName);//查找虚拟目录)
          }
          catch (Exception ex)
          {
            //  WriterLog("虚拟目录不存在： " + ex.Message);
              return null;
          }
      }
    
   

  
   }



}