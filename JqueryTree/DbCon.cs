using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Web;
using System.Configuration;
namespace JqueryTree
{
    public  class DbHelper
    {
        string path = HttpContext.Current.Server.MapPath("/dll/sqlite.db");
        String dirPath = HttpContext.Current.Server.MapPath("/upLoad");

        public void inital()
        {
            ("是否存在sqldb路径 " + path).AddLog(dirPath);
            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
            }
            SQLiteConnection conn = getConn();
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = path;
            builder.Password = "admin";
            conn.ConnectionString = builder.ToString();
            conn.Open();
            ("已打开sqldb数据库 " + path).AddLog(dirPath);
            string sql = "create table myfile(ID  integer(4) PRIMARY KEY,name text)";//,threat varchar(550),asset varchar(550),target varchar(550),interval varchar(550))  ";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            ("已打开sqldb数据库 " + path).AddLog(dirPath);
            conn.Close();
        }
        public void cleardb()
        {
            if (isExits())
            {
                OperateChanges("drop table  myfile");
            }
            //OperateChanges("drop from   sqlite_sequence");
            inital();
        }
        public bool isExits(){
            DataTable tab = GetTable("select name from sqlite_master where type='table'");
            return tab.Rows.Count > 0 ? true : false;
        }
        public SQLiteConnection getConn()
        {
            SQLiteConnection conn = new SQLiteConnection();
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = path;
            builder.Password = "admin";
            conn.ConnectionString = builder.ToString();
            return conn;
        }
        public DataTable GetTable(string str, params SQLiteParameter[] par)
        {
            SQLiteConnection con = getConn();
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand();
            string sql = str;
            cmd.CommandText = sql;
            cmd.Connection = con;
            PrepareCommand(cmd, par);
            SQLiteDataAdapter ad = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            ad.Fill(ds);
            con.Close();
            return ds.Tables[0];
        }
        public bool OperateChanges(string str,params SQLiteParameter[] par)
        {
            int ret = 0;
           
                SQLiteConnection con = getConn();
                con.Open();
                using (SQLiteTransaction tran = con.BeginTransaction())
                {
                   try
                   {
                        SQLiteCommand cmd = new SQLiteCommand();
                        string sql = str;
                        cmd.CommandText = sql;
                        cmd.Connection = con;
                        PrepareCommand(cmd,par);
                        ret = cmd.ExecuteNonQuery();
                        tran.Commit();
                        con.Close();
                    }
                    catch(Exception ex)
                    {
                        tran.Rollback();
                        con.Close();
                    }
            }
            return ret > 0 ? true : false;
        }
        public Object OperateReturnChanges(string str, params SQLiteParameter[] par)
        {
            SQLiteConnection con = getConn();
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(str, con);
            PrepareCommand(cmd, par);
            Object mm = cmd.ExecuteScalar();
            con.Close();
            return mm;
        }
        private static void PrepareCommand(SQLiteCommand cmd,SQLiteParameter[] cmdParms)
        {
            if (cmdParms != null)
            {
                foreach (SQLiteParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
    public class Node
    {
        public Text text { get; set; }
        public string HTMLclass { get; set; }
        public List<Node> chileren { get; set; }
    }
    public class Text
    {
        public string oldname { get; set; }
        public string name { get; set; }
        public string threat { get; set; }
        public string asset { get; set; }
        public string target { get; set; }
        public string interval { get; set; }
    }

    public class NewNode{
        public string name { get; set; }
        public string text { get; set; }
        public string guanxi { get; set; }
        public string threat { get; set; }
        public string asset { get; set; }
        public string target { get; set; }
        public string interval { get; set; }
        public List<NewNode> children { get; set; }
    }
}
