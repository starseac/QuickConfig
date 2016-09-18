using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.Administration;
using System.DirectoryServices;

namespace QuickConfig
{
    public class setFTP
    {
        //给网站添加ftp
        public void createFtpSite(string ftpSiteName, string ftpLocalPath)
        {
            using (ServerManager serverManager = new ServerManager())
            {

                // Add FTP publishing to Default Web Site
                Site site = serverManager.Sites["Default Web Site"];

                // Add an FTP Binding to the Site
                site.Bindings.Add(@"*:21:", @"ftp");

                ConfigurationElement ftpServerElement = site.GetChildElement("ftpServer");

                ConfigurationElement securityElement = ftpServerElement.GetChildElement("security");


            

                // Enable SSL
                ConfigurationElement sslElement = securityElement.GetChildElement("ssl");
                sslElement["serverCertHash"] = @"53FC3C74A1978C734751AB7A14A3E48F70A58A84";
                sslElement["controlChannelPolicy"] = @"SslRequire";
                sslElement["dataChannelPolicy"] = @"SslRequire";

                // Enable Basic Authentication
                ConfigurationElement authenticationElement = securityElement.GetChildElement("authentication");
                ConfigurationElement basicAuthenticationElement = authenticationElement.GetChildElement("basicAuthentication");
                basicAuthenticationElement["enabled"] = true;


                // Add Authorization Rules
                Configuration appHost = serverManager.GetApplicationHostConfiguration();
                ConfigurationSection authorization = appHost.GetSection("system.ftpServer/security/authorization", site.Name);
                ConfigurationElementCollection authorizationRules = authorization.GetCollection();
                ConfigurationElement authElement = authorizationRules.CreateElement();
                authElement["accessType"] = "Allow";
                authElement["users"] = "bdc";
                authElement["permissions"] = "Read";
                authorizationRules.Add(authElement);


                serverManager.CommitChanges();
            }
        }


        private int GetNewSiteID()
        {
            ServerManager sm = new ServerManager();
            int siteID = 1;

            foreach (Site e in sm.Sites)
            {

                int ID = Convert.ToInt32(e.Id);
                if (ID >= siteID) { siteID = ID + 1; }

            }
            return siteID;
        }


        //添加ftp站点
        public void createFTP(string ftpname,string ftpPath,string username,string ipaddress)
        {

            using (ServerManager serverManager = new ServerManager())
            {
                Configuration config = serverManager.GetApplicationHostConfiguration();

                ConfigurationSection sitesSection = config.GetSection("system.applicationHost/sites");

                ConfigurationElementCollection sitesCollection = sitesSection.GetCollection();

                ConfigurationElement siteElement = sitesCollection.CreateElement("site");
                siteElement["name"] = ftpname;
                siteElement["id"] = GetNewSiteID();

                ConfigurationElementCollection bindingsCollection = siteElement.GetCollection("bindings");

                ConfigurationElement bindingElement = bindingsCollection.CreateElement("binding");
                bindingElement["protocol"] = @"ftp";
                bindingElement["bindingInformation"] = ipaddress+@":21:";
                bindingsCollection.Add(bindingElement);

               //ftp 授权规则
                ConfigurationSection authorizationSection = config.GetSection("system.ftpServer/security/authorization");
                ConfigurationElementCollection authorizationCollection = authorizationSection.GetCollection();
                // 清除子节点授权，防止有残余
                authorizationCollection.Clear();
                ConfigurationElement addElement1 = authorizationCollection.CreateElement("add");
                addElement1["accessType"] = @"Allow";
                addElement1["users"] = username;
                addElement1["permissions"] = @"Read,Write";
                authorizationCollection.Add(addElement1);

                ConfigurationElement ftpServerElement = siteElement.GetChildElement("ftpServer");
                //设置ftp ssl
                ConfigurationElement securityElement = ftpServerElement.GetChildElement("security");
                ConfigurationElement sslElement = securityElement.GetChildElement("ssl");
                //sslElement["serverCertHash"] = @"53FC3C74A1978C734751AB7A14A3E48F70A58A84";
                sslElement["controlChannelPolicy"] = @"SslAllow";
                sslElement["dataChannelPolicy"] = @"SslAllow";
                // 设置ftp身份验证
                ConfigurationElement authenticationElement = securityElement.GetChildElement("authentication");               
                //ConfigurationElementCollection authenticationElementCollection = authorizationSection.GetCollection();
                ConfigurationElement basicAuthenticationElement = authenticationElement.GetChildElement("basicAuthentication");
                basicAuthenticationElement["enabled"] = @"true";
                ConfigurationElement anonymousAuthenticationElement = authenticationElement.GetChildElement("anonymousAuthentication");
                anonymousAuthenticationElement["enabled"] = @"true";

               

                ConfigurationElementCollection siteCollection = siteElement.GetCollection();
                ConfigurationElement applicationElement = siteCollection.CreateElement("application");
                applicationElement["path"] = @"/";

                ConfigurationElementCollection applicationCollection = applicationElement.GetCollection();
                ConfigurationElement virtualDirectoryElement = applicationCollection.CreateElement("virtualDirectory");
                virtualDirectoryElement["path"] = @"/";
                virtualDirectoryElement["physicalPath"] =ftpPath;
                applicationCollection.Add(virtualDirectoryElement);
                siteCollection.Add(applicationElement);
                sitesCollection.Add(siteElement);
                serverManager.CommitChanges();
            }
        }


        public bool delFtpSite(string ftpSiteName)
        {
           
           
                try
                {
                    ServerManager sm=new ServerManager();
                    Site site = sm.Sites[ftpSiteName];
                    sm.Sites.Remove(site);
                    sm.CommitChanges();
                    return true;

                }
                catch (System.Exception)
                {
                    return false;
                }
           
        }

        public void getClassName()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
           // DirectoryEntry root = new DirectoryEntry("IIS://localhost/W3SVC");
            DirectoryEntry root = new DirectoryEntry("IIS://localhost/MSFtpsvc");
            foreach (DirectoryEntry e in root.Children)
            {
                result[e.SchemaClassName] = e.SchemaClassName;
            }
           
        }

  

    }
}
