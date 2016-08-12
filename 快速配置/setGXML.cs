using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Management;
using System.Windows.Forms;
using System.Security.AccessControl;

namespace QuickConfig
{
   public class setGXML
    {
        /// <summary>
        /// 设置文件夹权限 处理给 用户 赋予所有权限
        /// </summary>
        /// <param name="FileAdd">文件夹路径</param>
        public void SetFileRole(string path, string osusername)
        {
            DirectorySecurity fSec = new DirectorySecurity();
            fSec.AddAccessRule(new FileSystemAccessRule(osusername, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(path, fSec);
        }

        public bool shareFolder(string FolderPath, string ShareName, string Description)
        {
            // 创建共享目录            
            try
            {
                ManagementClass managementClass = new ManagementClass("Win32_Share");
                ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
                ManagementBaseObject outParams;
                inParams["Description"] = Description;
                inParams["Name"] = ShareName;
                inParams["Path"] = FolderPath;
                inParams["Type"] = 0x0;
                outParams = managementClass.InvokeMethod("Create", inParams, null);

                if ((uint)(outParams.Properties["ReturnValue"].Value) != 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
