using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace QuickConfig
{
    public class setConfig
    {

        public void scanAndCopy(DirectoryInfo AppFolder, DirectoryInfo NewAppFolder)
        {

            foreach (DirectoryInfo folder in AppFolder.GetDirectories())
            {

                DirectoryInfo Folder = new DirectoryInfo(AppFolder.FullName + @"\" + folder.Name);

                DirectoryInfo NewFolder = new DirectoryInfo(NewAppFolder.FullName + @"\" + folder.Name);
                Directory.CreateDirectory(NewFolder.FullName);
                scanAndCopy(Folder, NewFolder);
            }

            //遍历文件
            foreach (FileInfo NextFile in AppFolder.GetFiles())
            {
                System.IO.File.Copy(NextFile.FullName, NewAppFolder.FullName + @"\" + NextFile.Name);
            }
        }

        public void copyConfig()
        {

            DirectoryInfo TheFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"config");

            DirectoryInfo TempFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"configTemp");
            if (Directory.Exists(TempFolder.FullName) == true)
            {
                TempFolder.Delete(true);
            }

            Directory.CreateDirectory(TempFolder.FullName);
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                DirectoryInfo AppFolder = new DirectoryInfo(TheFolder.FullName + @"\" + NextFolder.Name);
                DirectoryInfo NewAppFolder = new DirectoryInfo(TempFolder.FullName + @"\" + NextFolder.Name);
                Directory.CreateDirectory(NewAppFolder.FullName);
                scanAndCopy(AppFolder, NewAppFolder);

            }

        }


        public System.Text.Encoding GetFileEncodeType(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
            Byte[] buffer = br.ReadBytes(2);

            fs.Close();
            br.Close();
            if (buffer[0] >= 0xEF)
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    return System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    return System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    return System.Text.Encoding.Unicode;
                }
                else
                {
                    return System.Text.Encoding.Default;
                }
            }
            else
            {
                return System.Text.Encoding.Default;
            }
        }


        public void repalcePar()
        {
            setXml xml = new setXml();
            DataTable dtPar = xml.readXML();
            DataTable dtFile = xml.readXMLCopyPath();
            DirectoryInfo TempFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"configTemp");

            for (int i = 0; i < dtFile.Rows.Count; i++)
            {
                string path = TempFolder.FullName + "\\" + dtFile.Rows[i]["appname"].ToString() + "\\" + dtFile.Rows[i]["filepath"].ToString();
                FileInfo file = new FileInfo(path);

                Encoding encoding;
                encoding = GetFileEncodeType(file.FullName);

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                StreamReader sr;
              
                //if (file.Extension.ToLower() == ".bat")
                //{
                //    encoding = System.Text.Encoding.ASCII;
                //}
                //else
                //{
                //    encoding = System.Text.Encoding.UTF8;
                //}
                sr = new StreamReader(fs, encoding);
                string con = sr.ReadToEnd();
                for (int j = 0; j < dtPar.Rows.Count; j++)
                {
                    con = con.Replace("{" + dtPar.Rows[j]["name"].ToString() + "}", dtPar.Rows[j]["value"].ToString());
                }
                sr.Close();
                fs.Close();
                File.WriteAllText(path, con, encoding);


            }



        }


        public void repalcePar(string oriFileFolderPath, string filename, string targetFileFolderPath)
        {
            if (File.Exists(targetFileFolderPath + "\\" + filename) == true)
            {
               File.Delete(targetFileFolderPath + "\\" + filename);
            }
             System.IO.File.Copy(oriFileFolderPath+"\\"+filename, targetFileFolderPath+"\\"+filename);
            setXml xml = new setXml();
            DataTable dtPar = xml.readXML();
          
            
                string path = targetFileFolderPath+"\\"+filename;
                FileInfo file = new FileInfo(path);

                Encoding encoding;
                encoding = GetFileEncodeType(file.FullName);

                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                StreamReader sr;

              
                sr = new StreamReader(fs, encoding);
                string con = sr.ReadToEnd();
                for (int j = 0; j < dtPar.Rows.Count; j++)
                {
                    con = con.Replace("{" + dtPar.Rows[j]["name"].ToString() + "}", dtPar.Rows[j]["value"].ToString());
                }
                sr.Close();
                fs.Close();
                File.WriteAllText(path, con, encoding);

            }

        public void repalcePar(string oriFileFolderPath, string filename, string targetFileFolderPath, DataTable dtPar)
        {
            if (File.Exists(targetFileFolderPath + "\\" + filename) == true)
            {
                File.Delete(targetFileFolderPath + "\\" + filename);
            }
            System.IO.File.Copy(oriFileFolderPath + "\\" + filename, targetFileFolderPath + "\\" + filename);
            // setXml xml = new setXml();
            //DataTable dtPar = xml.readXML();


            string path = targetFileFolderPath + "\\" + filename;
            FileInfo file = new FileInfo(path);

            Encoding encoding;
            encoding = GetFileEncodeType(file.FullName);

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            StreamReader sr;


            sr = new StreamReader(fs, encoding);
            string con = sr.ReadToEnd();
            for (int j = 0; j < dtPar.Rows.Count; j++)
            {
                con = con.Replace("{" + dtPar.Rows[j]["name"].ToString() + "}", dtPar.Rows[j]["value"].ToString());
            }
            sr.Close();
            fs.Close();
            File.WriteAllText(path, con, encoding);

        }


        public void config(List<string> checkapp)
        {
            // 复制文件到  临时文件夹
            copyConfig();
            // 替换参数 为真实值      
            repalcePar();
            //应用配置到 程序
            applyConfig(checkapp);
        }

        private void applyConfig(List<string> checkapp)
        {
            setXml xml = new setXml();

            DataTable dtFile = xml.readXMLCopyPath();

            foreach (string appname in checkapp)
            {
                DataTable selectDT = new DataTable();
                selectDT = dtFile.Clone();
                System.Data.DataRow[] selectDT_row = dtFile.Select("appname='" + appname + "'");
                for (int j = 0; j < selectDT_row.Length; j++)
                {
                    selectDT.ImportRow((System.Data.DataRow)selectDT_row[j]);
                }

                for (int i = 0; i < selectDT.Rows.Count; i++)
                {
                    string oriFilePath = AppDomain.CurrentDomain.BaseDirectory + "configTemp\\" + appname + selectDT.Rows[i]["filepath"].ToString();
                    string targetFilePath = xml.getSetXmlValue("path_" + appname) + selectDT.Rows[i]["filepath"].ToString();
                    if (File.Exists(targetFilePath))
                    {
                        File.Delete(targetFilePath);
                    }
                    File.Copy(oriFilePath, targetFilePath);

                }

            }
        }

     

    }
}
