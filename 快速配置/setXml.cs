using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace QuickConfig
{
    public class setXml
    {

        public DataTable readXML()
        {
            string fileurl = AppDomain.CurrentDomain.BaseDirectory + @"\config\set.xml";
            DataSet ds = new DataSet();
            ds.ReadXml(fileurl);

            DataTable parSets = ds.Tables["par"];
            return parSets;

        }

        public string getSetXmlValue(string parname) {
            string fileurl = AppDomain.CurrentDomain.BaseDirectory + @"\config\set.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(fileurl);
            string value = "";
            XmlNodeList nodeList = doc.SelectSingleNode("parset").ChildNodes;//获取Employees节点的所有子节点 

            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                if (xe.GetAttribute("name") == parname)//如果genre属性值为“张三” 
                {
                    value = xe.GetAttribute("value");

                    break;

                }
            }
            return value;
        }

        public DataTable readXMLCopyPath()
        {
            string fileurl = AppDomain.CurrentDomain.BaseDirectory + @"\config\copyPath.xml";
            DataSet ds = new DataSet();
            ds.ReadXml(fileurl);

            DataTable parSets = ds.Tables["copy"];
            return parSets;

        }


        public void editxml(string parString, string parValue)
        {

            string fileurl = AppDomain.CurrentDomain.BaseDirectory + @"\config\set.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(fileurl);

            XmlNodeList nodeList = doc.SelectSingleNode("parset").ChildNodes;//获取Employees节点的所有子节点 

            foreach (XmlNode xn in nodeList)//遍历所有子节点 
            {
                XmlElement xe = (XmlElement)xn;//将子节点类型转换为XmlElement类型 
                if (xe.GetAttribute("name") == parString)//如果genre属性值为“张三” 
                {
                    xe.SetAttribute("value", parValue);//则修改该属性为“update张三” 

                    break;

                }
            }

            doc.Save(fileurl);


        }

        public void scan(DirectoryInfo AppFolder,List<string> list) {
            
            foreach (DirectoryInfo folder in AppFolder.GetDirectories())
            {

                DirectoryInfo Folder = new DirectoryInfo(folder.FullName + @"\" + folder.Name);
                scan(folder, list);
            }

            //遍历文件
            foreach (FileInfo NextFile in AppFolder.GetFiles())
            {
                list.Add(NextFile.FullName);
            }
        }

        public List<string> scanfiles()
        {
            List<string> filepath = new List<string>();

            DirectoryInfo TheFolder = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + @"config");
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                DirectoryInfo AppFolder = new DirectoryInfo(TheFolder.FullName +@"\"+ NextFolder.Name);
                scan(AppFolder, filepath);

                //foreach (FileInfo NextFile in AppFolder.GetFiles())
                //{
                //    filepath.Add(NextFile.FullName);
                //}

            }
            return filepath;

        }


        public void addCopyFileXML(List<string> filepath)
        {

            string filename = "copyPath.xml";
            string url = AppDomain.CurrentDomain.BaseDirectory + @"config/" + filename + "";

            XmlTextWriter xw = new XmlTextWriter(url, Encoding.UTF8);

            xw.Formatting = Formatting.Indented;

            xw.WriteStartDocument();
            xw.WriteStartElement("copyset");

            //---

            if (filepath != null && filepath.Count >= 1)
            {

                foreach (string pathString in filepath)
                {
                    string  newpathString = pathString.Replace(AppDomain.CurrentDomain.BaseDirectory + @"config\", "");
                    string appname = newpathString.Split('\\')[0];
                    string file = newpathString.Substring(appname.Length) ;

                    //申请书页面
                    xw.WriteStartElement("copy");
                    xw.WriteAttributeString("appname", appname);
                    xw.WriteAttributeString("filepath", file);
                    xw.WriteEndElement();
                }
            }

            //-----


            xw.WriteEndElement();
            xw.WriteEndDocument();

            xw.Flush();
            xw.Close();

        }

    }
}
