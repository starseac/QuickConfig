using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.OracleClient;
using System.Data;

namespace QuickConfig
{
    public class setDB
    {

        protected OracleConnection con;//连接对象

        private string getConnString(string user,string password,string datasource){
          return "Password="+password+";User ID="+user+";Data Source="+datasource+";Persist Security Info=True";
        
        }

        //public setDB()
        //{
        //con=new OracleConnection(DafangFramework.AppConfig.DataBaseConnectionString);
        //}
        public setDB(string constr)
        {
            con = new OracleConnection(constr);
        }
        public setDB(string user, string password, string datasource)
        {
            con = new OracleConnection(getConnString(user,password,datasource));
        }

        #region 打开数据库连接
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private void Open()
        {
            //打开数据库连接
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    //打开数据库连接
                    con.Open();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion
        #region 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void Close()
        {
            //判断连接的状态是否已经打开
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        #endregion
        #region 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>OracleDataReader</returns>   
        public OracleDataReader ExecuteReader(string sql)
        {
            OracleDataReader myReader;
            Open();
            OracleCommand cmd = new OracleCommand(sql, con);
            myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myReader;
        }
        #endregion
        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>影响的记录数</returns>   
        //public int ExecuteSql(string sql, params OracleParameter[] cmdParms)
        //{
        //    OracleCommand cmd = new OracleCommand();
        //    {
        //        try
        //        {
        //            PrepareCommand(cmd, con, null, sql, cmdParms);
        //            int rows = cmd.ExecuteNonQuery();
        //            cmd.Parameters.Clear();
        //            return rows;
        //        }
        //        catch (System.Data.OracleClient.OracleException e)
        //        {
        //            throw e;
        //        }
        //    }
        //}
        #endregion
        #region 执行带参数的SQL语句
        /// <summary>
        /// 执行不带参数的SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>     
        public int ExecuteSql(string sql)
        {
            int ans = 0;
            OracleCommand cmd = new OracleCommand(sql, con);
            try
            {
                Open();
                ans=cmd.ExecuteNonQuery();
                Close();
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                Close();
                throw e;
            }
            return ans;
        }

     
        #endregion
        #region 执行SQL语句，返回数据到DataSet中
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                Open();//打开数据连接
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(ds);
            }
            catch//(Exception ex)
            {
            }
            finally
            {
                Close();//关闭数据库连接
            }
            return ds;
        }
        #endregion
        #region 执行SQL语句，返回数据到自定义DataSet中
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="DataSetName">自定义返回的DataSet表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql, string DataSetName)
        {
            DataSet ds = new DataSet();
            Open();//打开数据连接
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(ds, DataSetName);
            Close();//关闭数据库连接
            return ds;
        }
        #endregion
        #region 执行Sql语句,返回带分页功能的自定义dataset
        /// <summary>
        /// 执行Sql语句,返回带分页功能的自定义dataset
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="CurrPageIndex">当前页</param>
        /// <param name="DataSetName">返回dataset表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql, int PageSize, int CurrPageIndex, string DataSetName)
        {
            DataSet ds = new DataSet();
            Open();//打开数据连接
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(ds, PageSize * (CurrPageIndex - 1), PageSize, DataSetName);
            Close();//关闭数据库连接
            return ds;
        }
        #endregion
        #region 执行SQL语句，返回记录总数
        /// <summary>
        /// 执行SQL语句，返回记录总数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回记录总条数</returns>
        public int GetRecordCount(string sql)
        {
            int recordCount = 0;
            Open();//打开数据连接
            OracleCommand command = new OracleCommand(sql, con);
            OracleDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                recordCount++;
            }
            dataReader.Close();
            Close();//关闭数据库连接
            return recordCount;
        }
        #endregion
        #region 统计某表记录总数
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="KeyField">主键/索引键</param>
        /// <param name="TableName">数据库.用户名.表名</param>
        /// <param name="Condition">查询条件</param>
        /// <returns>返回记录总数</returns> 
        public int GetRecordCount(string keyField, string tableName, string condition)
        {
            int RecordCount = 0;
            string sql = "select count(" + keyField + ") as count from " + tableName + " " + condition;
            DataSet ds = GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            ds.Clear();
            ds.Dispose();
            return RecordCount;
        }
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">数据库.用户名.表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="flag">字段是否主键</param>
        /// <returns>返回记录总数</returns> 
        public int GetRecordCount(string Field, string tableName, string condition, bool flag)
        {
            int RecordCount = 0;
            if (flag)
            {
                RecordCount = GetRecordCount(Field, tableName, condition);
            }
            else
            {
                string sql = "select count(distinct(" + Field + ")) as count from " + tableName + " " + condition;
                DataSet ds = GetDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                ds.Clear();
                ds.Dispose();
            }
            return RecordCount;
        }
        #endregion
        #region 统计某表分页总数
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="RecordCount">记录总数</param>
        /// <returns>返回分页总数</returns> 
        public int GetPageCount(string keyField, string tableName, string condition, int pageSize, int RecordCount)
        {
            int PageCount = 0;
            PageCount = (RecordCount % pageSize) > 0 ? (RecordCount / pageSize) + 1 : RecordCount / pageSize;
            if (PageCount < 1) PageCount = 1;
            return PageCount;
        }
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <returns>返回页面总数</returns> 
        public int GetPageCount(string keyField, string tableName, string condition, int pageSize, ref int RecordCount)
        {
            RecordCount = GetRecordCount(keyField, tableName, condition);
            return GetPageCount(keyField, tableName, condition, pageSize, RecordCount);
        }
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="flag">是否主键</param>
        /// <returns>返回页页总数</returns> 
        public int GetPageCount(string Field, string tableName, string condition, ref int RecordCount, int pageSize, bool flag)
        {
            RecordCount = GetRecordCount(Field, tableName, condition, flag);
            return GetPageCount(Field, tableName, condition, pageSize, ref RecordCount);
        }
        #endregion
        #region Sql分页函数
        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="KeyField">主键</param>
        /// <param name="FieldStr">所有需要查询的字段(field1,field2...)</param>
        /// <param name="TableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="CurrentPage">当前页号</param>
        /// <param name="PageSize">页宽</param>
        /// <returns>SQL语句</returns> 
        public string JoinPageSQL(string KeyField, string FieldStr, string TableName, string Where, string Order, int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + " ";
            }
            else
            {
                sql = "select * from (";
                sql += "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + ") a ";
                sql += "where " + KeyField + " not in (";
                sql += "select  " + (CurrentPage - 1) * PageSize + " " + KeyField + " from " + TableName + " " + Where + " " + Order + ")";
            }
            return sql;
        }
        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="Field">字段名(非主键)</param>
        /// <param name="TableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="CurrentPage">当前页号</param>
        /// <param name="PageSize">页宽</param>
        /// <returns>SQL语句</returns> 
        public string JoinPageSQL(string Field, string TableName, string Where, string Order, int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field;
            }
            else
            {
                sql = "select * from (";
                sql += "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + " ) a ";
                sql += "where " + Field + " not in (";
                sql += "select rownum " + (CurrentPage - 1) * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + ")";
            }
            return sql;
        }

        #endregion

        
        #region 操作orale

        public string  getDataFolder() {

            string sql = @"select replace(file_name, '\SYSTEM01.DBF', '') filefolder
                              from dba_data_files
                             where tablespace_name = 'SYSTEM'
                               and file_id = 1
                             order by tablespace_name";

          DataTable dt=GetDataSet(sql).Tables[0];    

         return dt.Rows[0][0].ToString();
        }        

        public bool createTabelspace(string tablespacename,string filepath,string size) {
            string sql = @"create tablespace "+tablespacename+@"  
                            logging  
                            datafile '" + filepath + @"' 
                            size " + size + @" 
                            autoextend on  
                            next 50m maxsize 20480m  
                            extent management local";

           int ans= ExecuteSql(sql);
           if (ans ==0)
           {
               return true;
           }
           else {
               return false;
           }
        
        }

        public bool createUser(string username,string password,string tablespacename) {
            string sql =@"create user " + username + @"   identified by " + password + @"  default tablespace " + tablespacename + @"  temporary tablespace temp";
            string sql1=@" grant dba to " + username;
            string sql2=@"grant create session,create table to " + username;
            string sql3=@"grant resource,connect to  " + username;
            string sql4=@"GRANT SELECT ANY TABLE TO " + username;

            int ans = ExecuteSql(sql) + ExecuteSql(sql1) + ExecuteSql(sql2) + ExecuteSql(sql3) + ExecuteSql(sql4);
            if (ans == 0)
            {
                return true;
            }
            else
            {
                return false;
            }        
        
        }

        public bool grantUser(string username)
        {
            string sql1 = @" grant dba to " + username;
            string sql2 = @"grant create session,create table to " + username;
            string sql3 = @"grant resource,connect to  " + username;
            string sql4 = @"GRANT SELECT ANY TABLE TO " + username;

            int ans = ExecuteSql(sql1) + ExecuteSql(sql2) + ExecuteSql(sql3) + ExecuteSql(sql4);
            if (ans == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public bool deleteUser(string username) {

            string sql= @" drop user " + username.ToUpper()+" cascade ";

            int ans = ExecuteSql(sql);
            if (ans == 0)
            {
                return true;
            }
            else
            {
                return false;
            }      
        }

        public bool deleteTablespace(string tablespacename) {
            string sql = @"drop tablespace " + tablespacename.ToUpper() + " including contents and datafiles";

            int ans = ExecuteSql(sql);
            if (ans == 0)
            {
                return true;
            }
            else
            {
                return false;
            }      
        
        }

        public bool isTablespaceExist(string tablespacename)
        {
            string sql = @"select * from dba_TABLESPACES WHERE TABLESPACE_NAME='" + tablespacename.ToUpper() + "'";

            DataSet ds = GetDataSet(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else {
                return false;
            }

        
        }

        public bool isUserExist(string username) {
            string sql = @"select * from dba_users WHERE USERNAME='" + username.ToUpper() + "'";

            DataSet ds = GetDataSet(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool setAppInfo(string xzqname,string ipaddress) {
            string sql = @"update t_common_appinfo set aliasname='" + xzqname + "'||'基础平台' ,ssourl='~/oSystem/Frame/Default/sysFrame.html' where DBCFGNAME='WEBAPPDB'";
            string sql1 = @"update t_common_appinfo set aliasname='" + xzqname + "'||'不动产登记发证系统' ,ssourl='http://'||'" + ipaddress + "'||':8025/Account/SSOLogin.html' ,websiteurl='http://'||'" + ipaddress + ":8025' where DBCFGNAME='BDCDJSPDB'";
            string sql2 = @"update t_common_appinfo set aliasname='" + xzqname + "'||'工作流系统' ,ssourl='http://'||'" + ipaddress + "'||':8028/Account/SSOLogin.html' where DBCFGNAME='WORKFLOWDB'";
            string sql3 = @"update t_common_appinfo set aliasname='" + xzqname + "'||'人事系统' ,ssourl='http://'||'" + ipaddress + "'||':8030/Account/SSOLogin.html' where DBCFGNAME='HRDB'";

            int ans = ExecuteSql(sql) + ExecuteSql(sql1) + ExecuteSql(sql2) + ExecuteSql(sql3);
            if (ans == 0)
            {
                return true;
            }
            else
            {
                return false;
            }     
        
        }

        /// <summary>
        /// 设置11g exp导出表不全的问题
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool setEXP()
        {
            string sql = @"select 'alter table '|| table_name ||' allocate extent' sqlstr from user_tables where segment_created= 'NO'";
            int count = 0;
            DataSet ds = GetDataSet(sql);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                {
                    string sqlstr = ds.Tables[0].Rows[i]["sqlstr"].ToString();
                    int ans = ExecuteSql(sqlstr);
                    if (ans!= 0)
                    {
                        count += 1;
                    }                  
                        
                }
            }

            if (count == 0)
            {
                return true;
            }
            else {
                return false;
            }
           
        }

             

        #endregion
    }
}
