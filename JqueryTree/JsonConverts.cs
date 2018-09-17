using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text;
using System.IO;
namespace System
{
    public static class Extension 
    {
        public static void AddLog(this string message, string LogoPath)
        {
            if (!System.IO.Directory.Exists(LogoPath))
            {
                System.IO.Directory.CreateDirectory(LogoPath);
            }
            string urlpath = DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".txt";
            string pathtemp = LogoPath + "\\" + urlpath;
            if (!File.Exists(pathtemp))
            {
                File.Create(pathtemp).Close();
            }
            using (StreamWriter w = File.AppendText(pathtemp))
            {
                w.WriteLine("/************************************/");
                w.WriteLine("异常信息：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                w.WriteLine(message);
                w.WriteLine("\r\n");
                w.WriteLine("/************************************/\r\n\r\n");
                w.Flush();
                w.Close();
            }
        }
        public static string JsonToString<T>(this T model) where T : new()
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsontemp = new System.Web.Script.Serialization.JavaScriptSerializer();
            return jsontemp.Serialize(model); 
        }
        public static T JsonToObject<T>(this string model) where T : new()
        {
            System.Web.Script.Serialization.JavaScriptSerializer jsontemp = new System.Web.Script.Serialization.JavaScriptSerializer();
            return jsontemp.Deserialize<T>(model);
        }
        public static bool IsHave(this string model,string obj)  
        {
            string[] array = obj.Split('/');
            bool result = false;
            for(int i = 0; i < array.Length; i++)
            {
                if (model.Contains(array[i]))
                {
                    result = true;
                }else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        /// <summary>
        /// 去掉回车换行 跟\特殊字符
        /// </summary>
        public static string RemoveFlag(this string model)
        {
            string hangMeno = (((model.Replace("\\r", "")).Replace("\\n", "")).Replace("\r", "")).Replace("\n", "");
            hangMeno= (((hangMeno.Replace("\\","")).Replace("<br>","")).Replace("<br/>","")).Replace("</br>","");
            return hangMeno;
        }
    }
}
