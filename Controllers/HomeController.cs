using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using System.Web;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using System.Collections;
using System.Data.SQLite;
using System.Text.RegularExpressions;
namespace JqueryTree.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int flag=0)
        {
            List<NewNode> ListNode =GetNodeList(); //  
            string result = "", tempstr = "";
            List<string> listtemp = new List<string>();
            if (ListNode.Count > 0)
            {
                int i = 0;
                //整个foreach循环主要实现取出所有结点装填到ul标签中 最后显示到页面上 从而完成无限级结点展示
                foreach (var mm in ListNode)
                {
                    List<string> tempstrlist = new List<string>();
                    //mm.guanxi  关系分 And  跟Or  两种  _1代表是否有下级结点
                    string str = "litag=\"collapsed_" + (!string.IsNullOrEmpty(mm.guanxi) ? mm.guanxi : "other") + "&_&" + mm.name.Trim(' ') + "&_&1&_&" + mm.threat.Trim(' ') + "&_&" + mm.asset.Trim(' ') + "&_&" + mm.target.Trim(' ') + "&_&" + mm.interval.Trim(' ')+"\"";
                    string classt = flag == 0 ? "class=\"collapsed\"" + str : str;   //class 为collapsed 的表示隐藏  为空则显示
                    result += "<li " + classt + " >" + mm.name.Trim(' ') + (mm.children != null && mm.children.Count > 0  ? "<ul>" : "");
                    //下面是递归算法取出当前结点下面的所有子结点
                    List<string> temp = mm.children != null && mm.children.Count > 0 ? digui(mm.children, flag, tempstrlist) : new List<string>();
                    //遍历取出的所有结点转换为字符串
                    tempstr = getresult(temp);
                    result += tempstr;
                }
                result += "</ul></li>";
            }
            ViewBag.Str = result;
            return View();
        }
        //如果公式文件有误 请 执行/home/qingli   就可以把公式文件数据清理掉了 
        public ActionResult QingLi()
        {
            new DbHelper().cleardb();
            return Redirect("/home/index");
        }
        //递归算法
        public List<string> digui(List<NewNode> list,int flag, List<string> results)
        {
            if (list.Count > 0)
            {
                foreach (var mm in list)
                {
                    //Text nn = mm.text;
                    string classt = flag == 0 ? "class=\"collapsed\"" : "";
                    //mm.guanxi  关系分 And  跟Or  两种  _1代表是否有下级结点
                    string tag = "litag=\"collapsed_" + (!string.IsNullOrEmpty(mm.guanxi) ? mm.guanxi : "Other") + "&_&" + mm.name.Trim(' ') + "&_&" + (mm.children == null ? 0 : 1) + "&_&" + mm.threat.Trim(' ') + "&_&" + mm.asset.Trim(' ') + "&_&" + mm.target.Trim(' ') + "&_&" + mm.interval.Trim(' ') + "\"";
                    //string tag=!string.IsNullOrEmpty(mm.guanxi)? " litag=\"collapsed_"+mm.guanxi+"_"+mm.name+"_"+(mm.children==null? 0:1)+"\"":"";
                    results.Add("<li " + classt + tag + ">" + mm.name.Trim(' ') + (mm.children != null && mm.children.Count > 0 ? "<ul>" : ""));
                    if (mm.children != null && mm.children.Count > 0)
                    {
                        //遇到下级节点 还有下级节点的  再次执行递归算法 知道没有下级节点为止
                        digui(mm.children, flag, results);
                        results.Add("</ul></li>");
                    }
                    else
                    {
                        results.Add("</li>");
                    }
                }
            }
            return results; 
        }
        public string getresult(List<string> results)
        {
            string temp = "";
            //遍历list泛型  最终转为字符串
            foreach(var mm in results)
            {
                temp += mm;// +"\r\n";
            }
            return temp;
        }
        //获取结点列表
        public List<NewNode> GetNodeList()
        {
            //从数据库中读取配置文件信息
            DataTable tab = new DbHelper().GetTable("select * from myfile");
            string json = tab.Rows.Count > 0 ? tab.Rows[0][1].ToString() : "";
            //通过回车换行符拆分字符串
            string[] arry = json.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            List<NewNode> ListNode = new List<NewNode>();
            List<string> GroupA = new List<string>();
            List<string> GroupB = new List<string>();
            for (int i = 0; i < arry.Length; i++)
            {
                //取出<--  左侧跟右侧的数据 分别存放在两个数组汇总
                string[] temparray = arry[i].Split(new string[] { "<--" }, StringSplitOptions.RemoveEmptyEntries);
                GroupA.Add(temparray[0].Trim(' '));
                GroupB.Add(temparray[1].Trim(' '));
            }

            for (int t = 0; t < arry.Length; t++)
            {
                NewNode model = new NewNode();
                string[] temparray = arry[t].Split(new string[] { "<--" }, StringSplitOptions.RemoveEmptyEntries);
                string a1 = temparray[0].Trim(' ');
                bool exist = false;
                for (int k = 0; k < GroupB.Count; k++)
                {
                    if (GroupB[k].Contains(a1))
                    {
                        //判断左侧结点数据是否存在于右侧数组中 存在说明有下级结点  就不再遍历左侧的该结点
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                    {
                        model.name = a1.Split('(')[0];
                        model.text = (a1.Split('(')[1]).Trim(')');
                        string four = a1.Substring(a1.IndexOf("(") + 1, a1.IndexOf(")") - a1.IndexOf("(")-1);
                        string [] fourarray=four.Split(',');
                        model.threat = fourarray[0];
                        model.asset = fourarray[1];
                        model.target = fourarray[2];
                        model.interval = fourarray[3]; 
                        //父节点
                        string b1 = temparray[1].Trim(' ');
                        //判断右侧与跟或关系
                        model.guanxi = b1.IndexOf("&&") > 0 || b1.IndexOf("||") > 0 ? (b1.IndexOf("&&") > 0 ? "And" : "Or") : (b1!=""? "And":"");
                        string[] temparray2 = b1.IndexOf("&&") > 0 || b1.IndexOf("||") > 0 ? b1.Split(new string[] { (b1.IndexOf("&&") > 0 ? "&&" : "||") }, StringSplitOptions.RemoveEmptyEntries) : new string[] { b1 };
                        List<NewNode> ListchildNode = new List<NewNode>();
                        for (int s = 0; s < temparray2.Length; s++)
                        {
                            NewNode modelchild = new NewNode();
                            //读取右侧结点信息 如名称
                            modelchild.name = temparray2[s].Split('(')[0];
                            string node2 = temparray2[s].Split('(')[1].Replace("\r", "").Replace("\n", "");
                            modelchild.text = node2.Substring(0, node2.IndexOf(")"));

                            string fourchild = temparray2[s].Substring(temparray2[s].IndexOf("(")+1, temparray2[s].IndexOf(")")-temparray2[s].IndexOf("(")-1);
                            string[] fourarraychild = fourchild.Split(',');
                            modelchild.threat = fourarraychild[0];
                            modelchild.asset = fourarraychild[1];
                            modelchild.target = fourarraychild[2];
                            modelchild.interval = fourarraychild[3]; 
                            //判断左侧数组中是否存在该结点 存在说明有下级结点  执行递归算法diguichild
                            if (GroupA.Contains(temparray2[s].Trim(' ')))
                            {
                                for (int i = 0; i < GroupA.Count; i++)
                                {
                                    if (GroupA[i].Trim(' ') == temparray2[s].Trim(' '))
                                    {
                                        //获取该节点下面对应的子结点信息（右侧数组中匹配后取出关系 &&  跟  ||）
                                        modelchild.guanxi = GroupB[i].IndexOf("&&") > 0 || GroupB[i].IndexOf("||") > 0 ? (GroupB[i].IndexOf("&&") > 0 ? "And" : "Or") : "";
                                        break;
                                    }
                                }
                                //将递归后 获取的所有子节点挂在该结点下  层层相扣  整理出层级
                                modelchild = diguichild(GroupA, GroupB, temparray2[s].Trim(' '), modelchild);
                                //modelchild.children = resultcurrent;
                            }
                            ListchildNode.Add(modelchild);
                        }
                        model.children = ListchildNode;
                        ListNode.Add(model);
                    }
            }
            //返回整理完后的所有结点信息
            return ListNode;
        }
        public NewNode diguichild(List<string> GroupA, List<string> GroupB, string node, NewNode model)
        {
            int i = 0;
            //判断是否存在于GroupA中 
            List<NewNode> Result = new List<NewNode>();
            if (IsExistInGroupA(GroupA,node,out i)) {
                    //取出该结点下面的所有子结点
                    string[] temparray2 = GroupB[i].IndexOf("&&") > 0 || GroupB[i].IndexOf("||") > 0 ? GroupB[i].Split(new string[] { (GroupB[i].IndexOf("&&") > 0 ? "&&" : "||") }, StringSplitOptions.RemoveEmptyEntries) : new string[] { GroupB[i] };
                    List<NewNode> ListchildNode = new List<NewNode>();
                    //遍历取出的子结点
                    for (int s = 0; s < temparray2.Length; s++)
                    {
                        NewNode modelchild = new NewNode();
                        /**G1(A,B,C) <-- G2(A,B,C) && G3(A,B,C)   依次取出右侧的 A  B C D */
                        modelchild.name = temparray2[s].Split('(')[0];
                        //去掉回车换行符
                        string node2 = temparray2[s].Split('(')[1].Replace("\r", "").Replace("\n", "");
                        modelchild.text = node2.Substring(0, node2.IndexOf(")"));

                        string fourchild = temparray2[s].Substring(temparray2[s].IndexOf("(") + 1, temparray2[s].IndexOf(")") - temparray2[s].IndexOf("(") - 1);
                        string[] fourarraychild = fourchild.Split(',');
                        modelchild.threat = fourarraychild[0];
                        modelchild.asset = fourarraychild[1];
                        modelchild.target = fourarraychild[2];
                        modelchild.interval = fourarraychild[3]; 
                         int t = 0;
                         //判断取出的子结点是否存在于GroupA中  递归遍历
                         if (IsExistInGroupA(GroupA, temparray2[s].Trim(' '), out t))
                         {
                             modelchild.guanxi = GroupB[t].IndexOf("&&") > 0 || GroupB[t].IndexOf("||") > 0 ? (GroupB[t].IndexOf("&&") > 0 ? "And" : "Or") : "";
                             //当前结点如果含有下级结点  那么继续递归
                             diguichild(GroupA, GroupB, temparray2[s].Trim(' '), modelchild);
                             //取出递归结束后的下级所有结点信息 加载到当前父结点中
                             List<NewNode> templist =model.children!=null && model.children.Count>0? model.children: new List<NewNode>();
                             //防止父结点中记录存在重复的子结点  需要筛重过滤
                             if (!templist.Contains(modelchild))
                             {
                                 templist.Add(modelchild);
                             }
                             //最终把子结点信息加载到父结点的children属性中
                             model.children = templist;
                         }
                         else{
                             //子结点查找下级结点 如果没有  就把自家店加载到ListchildNode 泛型中  为它的父结点准备好 子结点信息
                             ListchildNode.Add(modelchild);
                        }
                    }
                    //父结点加载准备好的子结点信息 
                    Result = model.children != null && model.children.Count > 0 ? model.children : new List<NewNode>();
                    if (Result != ListchildNode && ListchildNode.Count>0)
                    {
                        Result.AddRange(ListchildNode);
                    }
                    model.children = Result;
                    Result = new List<NewNode>();
            }
            return model;
        }
        //判断G1(A,B,C) <-- G2(A,B,C) && G3(A,B,C)   判断 <-- 右侧的结点 是否在左侧的结点列表中 
        //存在 则说明含有子结点  反之则说明是底层节点 不用再递归循环查找
        public bool IsExistInGroupA(List<string> GroupA, string node,out int index)
        {
            bool result = false;
            index = -1;
            for (int i = 0; i < GroupA.Count; i++)
            {
                if (GroupA[i].Trim(' ') == node.Trim(' '))
                {
                    index = i;
                    result = true;
                    break;
                }
            }
            return result;
        }
        //上传公示文件
        public ActionResult Upload()
        {
            string json = "";
            try
            {
                String savePath = "/upLoad";
                //文件保存目录URL
                //定义允许上传的文件扩展名
                Hashtable extTable = new Hashtable();
                //保存文件
                extTable.Add("image", "txt, js");
                //读取上传文件二进制流
                var tmpstr = HttpContext.Request.Files[0];
                //获取当前程序所在的物理路径  例如D:\JqueryTree\Upload
                String dirPath = Server.MapPath(savePath);
                string logopath = dirPath;
                //写入当前所在文件路径日志  
                dirPath.AddLog(logopath);
                //以当前日期命名文件  
                /****************上传文件的代码 可以不用理会  只是内部程序所用********************/
                String ymd = DateTime.Now.ToString("yyyy_MM_dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                string fujianname = string.IsNullOrEmpty(Request.Form["fujianname"]) ? "" : Request.Form["fujianname"];
                string dirName = ymd;
                string fileUrl = "";
                String fileName = tmpstr.FileName;
                //文件格式
                String fileExt = Path.GetExtension(fileName).ToLower();
                //文件名 最终以 2018_09_15_23_41_1234.txt  类似文本格式保存到指定的文件夹下  
                String newFileName = new Random().Next(10000, 90000) + DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_fff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + fileExt;
                ("newFileName   " + dirPath).AddLog(logopath);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
               
                dirPath += fujianname == "" ? "\\" + ymd + "\\" : "\\" + fujianname + "\\";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                int maxSize = !string.IsNullOrEmpty(Request["filesize"]) ? int.Parse(Request["filesize"].ToString()) : 10485760;//10M
                ("传入文件大小 ： " + tmpstr.InputStream.Length.ToString()).AddLog(logopath);
                ("最终文件上传目录   " + dirPath + "文件大小  " + Request["filesize"]).AddLog(logopath);
                if (tmpstr == null)
                {
                    //json = "{success:false,msg:'请上传文件！'}";
                    json = "请上传文件！";
                }
                else if (tmpstr.InputStream == null || tmpstr.InputStream.Length > maxSize)
                {
                    //json = "{success:false,msg:'文件请不要超出10M！'}";
                    json = "文件请不要超出10M！";
                }
                else
                {
                    string[] array = !string.IsNullOrEmpty(Request["filetype"]) ? Request["filetype"].ToString().Split(',') : new string[] {"txt", "js" };
                    if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(array, fileExt.Substring(1).ToLower()) == -1)
                    {
                        string tmptype = string.Join("|", array);
                        //json = "{success:false,msg:'请上传" + tmptype + "类型的文件！'}";
                        json = "请上传" + tmptype + "类型的文件！";
                    }
                    else
                    {
                        string types = fileExt.Substring(1).ToLower();
                        fileUrl = savePath + "/" + (fujianname == "" ? ymd + "/" : fujianname + "/") + newFileName;
                        ("上传文件的路径   " + dirPath + newFileName).AddLog(logopath);
                        tmpstr.SaveAs(System.IO.Path.Combine(dirPath, newFileName));  //gif,jpg,jpeg,png,bmp
                        fileName = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        fileName = fileName.Substring(0, fileName.LastIndexOf(".")) + fileExt;
                        newFileName = dirPath + newFileName;
                        ("保存成功   " + dirPath + newFileName).AddLog(logopath);
                        StreamReader sr = new StreamReader(newFileName, System.Text.Encoding.Default);
                        String input = sr.ReadToEnd();
                        sr.Close();
                        string strout = input;
                        strout=strout.Replace("（","(");
                        strout = strout.Replace("）", ")");
                        strout = strout.Replace("，", ")");
                        ("读取文件内容   " + strout).AddLog(logopath);
                        //strout = strout.IndexOf("nodeStructure:") >0? "["+strout.Substring(strout.IndexOf("nodeStructure:")+14).Trim(';')+"]": strout;
                        //List<Node> strlist = new List<Node>();
                        //strlist = strout.JsonToObject<List<Node>>();
                        ("开始清理数据库   ").AddLog(logopath);
                        new DbHelper().cleardb();
                        ("已清理清理数据库   ").AddLog(logopath);
                        //bool ret = new DbHelper().OperateChanges("delete from myfile");
                        SQLiteParameter[] par = new SQLiteParameter[] { (new SQLiteParameter("name", strout)) };
                        bool ret = new DbHelper().OperateChanges("insert into  myfile(name) values(@name)", par);
                        ("插入数据库   ").AddLog(logopath);
                        json = "true";// "{'success': true,'oldfile':'" + fileName + "','newfileurl':'" + fileUrl + "','newfile':'" + HttpUtility.UrlEncode(newFileName) + "'}";
                        //json = "{ 'success': true,'msg':'" + fileName + "','imgurl':'" + fileUrl + "'}";
                    }
                }
            }
            catch (Exception ex)
            {
                json = "错误原因：" + ex.Message.Substring(0, ex.Message.IndexOf("。"));
            }
            return Content(json);
            //return Content("");
        }
        public ActionResult getnodeinfo(string name)
        {
            //双击某个结点  获取该节点对应的信息threat    asset   target  interval   可以理解为结点括号里面的 A  B  C  D
            name = HttpUtility.UrlDecode(name);
            name = name.Replace("\r", "").Replace("\n", "");
            //读取所有结点信息
            List<NewNode> ListNode = GetNodeList();
            DataTable tab = new DbHelper().GetTable("select * from myfile");
            string json = tab.Rows.Count > 0 ? tab.Rows[0][1].ToString() : "";
            List<Text> listtext = new List<Text>();
            listtext = GetInfo(ListNode, name, listtext);
            //获取指定结点下面的数据threat    asset   target  interval   可以理解为结点括号里面的 A  B  C  D
            Text text =listtext.Count>0?  listtext[0]: new Text();
            return Content("{ \"name\":\""+name+ "\",\"threat\":\"" + text.threat + "\",\"asset\":\"" + text.asset + "\",\"target\":\"" + text.target + "\" ,\"interval\":\"" + text.interval + "\" }");
        }
        public List<Text> GetInfo(List<NewNode> list, string name, List<Text> text)
        {
            foreach (var mm in list)
            {
                if (mm.name.Trim(' ') == name.Trim(' '))
                    {
                        Text node = new Text();
                        mm.text = mm.text.Replace("，", ",").Trim(' ');
                        string[] array = mm.text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        node.name = mm.name.Trim(' ');
                        node.threat = array.Length > 0 ? array[0].Replace(",", "").Replace("'", "").Trim(' ') : "";
                        node.asset = array.Length > 1 ? array[1].Replace(",", "").Replace("'", "").Trim(' ') : "";
                        node.target = array.Length > 2 ? array[2].Replace(",", "").Replace("'", "").Trim(' ') : "";
                        node.interval = array.Length > 3 ? array[3].Replace(",", "").Replace("'", "").Trim(' ') : "";
                        text.Add(node);
                        break;
                    }else
                    {
                        if (mm.children != null && mm.children.Count > 0)
                        {
                            GetInfo(mm.children, name, text);
                        }
                    }
            }
            return text;
        }
        public ActionResult savenodeinfo(Text model)
        {
            //保存指定结点的相关信息  可以理解为结点括号里面的 A  B  C  D
            DataTable tab = new DbHelper().GetTable("select * from myfile");
            string json = tab.Rows.Count > 0 ? tab.Rows[0][1].ToString() : "";
            List<NewNode> ListNode = GetNodeList();
            List<Text> listtext = new List<Text>();
            listtext = GetInfo(ListNode, model.oldname, listtext);
            Text text = listtext[0];
            Text tempnode = listtext[0];
            string tempstr =  tempnode.name + "("+tempnode.threat+","+tempnode.asset+","+tempnode.target+","+tempnode.interval+")";
            tempstr=tempstr.Trim(' ');
            string tempstr2 = model.name.Trim(' ') + "(" + model.threat.Trim(' ') + "," + model.asset.Trim(' ') + "," + model.target.Trim(' ') + "," + model.interval.Trim(' ') + ")";
            tempstr2 = tempstr2.Trim(' ');
            //最终转换为字符串 替换数据库保存中的匹配的字符串 替换后 保存到数据库中
            json = json.Replace(tempstr, tempstr2);
            //清空数据库
            new DbHelper().cleardb();
            SQLiteParameter[] par = new SQLiteParameter[] { (new SQLiteParameter("name", json)) };
            //重新写入数据库
            bool ret = new DbHelper().OperateChanges("insert into  myfile(name) values(@name)", par);
            return Content(ret? "true":"保存失败！");
        }
    }
}
