﻿using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Management;
using System.Runtime.InteropServices;   //必要引用
using System.Security.Principal;    //必要引用

namespace DTcms.Web.aspx
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取域登陆信息
            string userName = Request.Params["user"];
            string password = Request.Params["password"];
            //域名
            string domainName = "apollo.com.cn";

            UserLoginForDomain CheckUserLogin = new UserLoginForDomain();

            try
            {
                //如果用户名或密码错误，跳转回登录页面
                if (Session["loginUser"] == null && (userName.Trim() == "" || password.Trim() == ""))
                {
                    Response.Redirect("Default.aspx");
                    return;
                }
                else if (Session["loginUser"] != null && (userName == null || password == null)) 
                {
                    userName = Session["loginUser"].ToString();
                    password = Session["loginPassword"].ToString();
                }

                //域用户判断
                if (!CheckUserLogin.impersonateValidUser(userName, domainName, password))
                {
                    Response.Redirect("error.aspx");
                    return;
                }
                else
                {
                    //登录通过
                    Session["loginUser"] = userName;
                    Session["loginPassword"] = password;
                }
            }
            catch (Exception)
            {
                Response.Redirect("Default.aspx");
                return;
            }

            if (!Page.IsPostBack)
            {
                //查找用户信息
                BLL.manager bll = new BLL.manager();
                BLL.manager_role rolebll = new BLL.manager_role();
                int roleId = 0;
                try
                {
                    Model.manager manager = bll.GetModel(userName);

                    //查询部门名和真实姓名
                    string department = rolebll.GetTitle(manager.role_id);
                    string realName = manager.real_name;

                    //绑定登陆基本信息
                    loginMsgBind(userName, realName, department);

                    //获取部门id
                    roleId = manager.role_id;
                }
                catch (Exception) {
                    return;
                }
                
                BLL.manager_role_value mrbll = new BLL.manager_role_value();
                BLL.channel chanbll = new BLL.channel();
                BLL.article artibll = new BLL.article();


                //构造查询条件
                string sqlWhere = "";
                sqlWhere = "role_id = " + roleId + " and action_type = 'Audit'";

                //返回查询子权限集合
                DataTable oldData = mrbll.GetList(sqlWhere).Tables[0];
                //频道id
                int channelId = 0;
                List<int> chidList = new List<int>();
                StringBuilder ids = new StringBuilder();
                foreach (DataRow dr in oldData.Rows)
                {
                    //获取频道名称拼音形式
                    string navName = dr["nav_name"] + "";
                    navName = navName.Split('_')[1];
                    //返回频道id
                    channelId = chanbll.GetChannelId(navName);
                    chidList.Add(channelId);
                }

                //遍历父级角色
                Model.manager_role child = rolebll.GetModel(roleId);
                string parent_id = child.parent_id;
                while (!"0".Equals(parent_id.Trim()))
                {
                    Model.manager_role parent = rolebll.GetModel(parent_id);
                    //父角色编号
                    int parentRoleId = parent.id;
                    string sqlWhere2 = "";
                    sqlWhere2 = "role_id = " + parentRoleId + " and action_type = 'Audit'";
                    //返回查询父权限集合
                    DataTable newData = mrbll.GetList(sqlWhere2).Tables[0];

                    foreach (DataRow dr in newData.Rows)
                    {
                        string navName = dr["nav_name"] + "";
                        navName = navName.Split('_')[1];
                        channelId = chanbll.GetChannelId(navName);
                        chidList.Add(channelId);
                    }
                    parent_id = parent.parent_id;
                }
                

                //构造频道id集合
                for (int i = 0; i < chidList.Count; i++)
                {
                    ids.Append(chidList[i] + ",");
                }

                string channelid = Utils.DelLastComma(ids.ToString());

                //查找现有系统信息
                BLL.article artbll = new BLL.article();
                //查找所有
                DataTable dt = artbll.GetList(0, "", "ID DESC").Tables[0];
                //DataTable dt2 = artbll.GetList(0, "channel_id in (" + channelid.ToString() + ")", "ID DESC").Tables[0];

                //对集合元素反转排序
                chidList.Reverse();

                //创建元素
                foreach (DataRow dr in dt.Rows)
                {
                    //外围
                    HtmlGenericControl sys = new HtmlGenericControl("div");
                    sys.Attributes.Add("class", "sys");
                    //内置div1
                    HtmlGenericControl sysphoto = new HtmlGenericControl("div");
                    sysphoto.Attributes.Add("class", "sys_photo");
                    HtmlAnchor aElement = new HtmlAnchor();
                    //设置a标签属性
                    //aElement.HRef = urlCreate(dr["zhaiyao"].ToString().Trim());
                    HtmlImage image = new HtmlImage();
                    image.Src = dr["img_url"].ToString().Trim();
                    //子标签添加到父标签
                    aElement.Controls.Add(image);
                    sysphoto.Controls.Add(aElement);
                    //内置div2
                    HtmlGenericControl systitle = new HtmlGenericControl("div");
                    systitle.Attributes.Add("class", "sys_title");
                    HtmlAnchor aElement2 = new HtmlAnchor();
                    //aElement2.HRef = urlCreate(dr["zhaiyao"].ToString().Trim());
                    aElement2.InnerHtml = dr["title"].ToString();

                    
                    //获取产品频道id
                    string channelID = dr["channel_id"] + "";
                    if (chidList.Count > 0)
                    {
                        for (int i = 0; i < chidList.Count; i++)
                        {
                            if (chidList[i] == int.Parse(channelID))//匹配频道id
                            {
                                //设置图片跳转地址
                                aElement.HRef = urlCreate(dr["zhaiyao"].ToString().Trim());
                                //设置文字跳转地址
                                aElement2.HRef = urlCreate(dr["zhaiyao"].ToString().Trim());
                                //删除匹配上的频道id
                                chidList.RemoveAt(i);
                                break;
                            }
                            else //若匹配不上该频道id,绑定点击事件处理函数class属性
                            {
                                aElement.Style.Add("cursor", "pointer");
                                aElement.Attributes.Add("class", "nobelong");
                                aElement2.Style.Add("cursor", "pointer");
                                aElement2.Attributes.Add("class", "nobelong");
                            }
                        }
                    }
                    else 
                    {
                        aElement.Style.Add("cursor", "pointer");
                        aElement.Attributes.Add("class", "nobelong");
                        aElement2.Style.Add("cursor", "pointer");
                        aElement2.Attributes.Add("class", "nobelong");
                    }
                    

                    //子标签添加到父标签
                    systitle.Controls.Add(aElement2);

                    sys.Controls.Add(sysphoto);
                    sys.Controls.Add(systitle);

                    //添加到页面标签
                    this.main.Controls.Add(sys);
                }
            }
        }

        //绑定登陆的基本信息
        public void loginMsgBind(string userName,string realName,string department) 
        {
            this.userName.InnerText = userName;
            this.realName.InnerText = realName;
            this.department.InnerText = department;
        }

        /// <summary>
        /// 用于构造跳转地址格式
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string urlCreate(string url) 
        {
            string[] values = url.Split(':');
            if ("http".Equals(values[0].Trim()))
            {
                return url;
            }
            else 
            {
                return "http://" + url;
            }
        }

        public class UserLoginForDomain
        {
            public UserLoginForDomain()
            {
                //
                // TODO: 在此处添加构造函数逻辑
                //
            }

            //【用户登录域】方法#region【用户登录域】方法

            public const int LOGON32_LOGON_INTERACTIVE = 2;
            public const int LOGON32_PROVIDER_DEFAULT = 0;

            WindowsImpersonationContext impersonationContext;

            [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
            public static extern int LogonUser(String lpszUserName,
                                              String lpszDomain,
                                              String lpszPassword,
                                              int dwLogonType,
                                              int dwLogonProvider,
                                              ref IntPtr phToken);
            [DllImport("advapi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
            public extern static int DuplicateToken(IntPtr hToken,
                                              int impersonationLevel,
                                              ref IntPtr hNewToken);
            /**/
            /// <summary>
            /// 输入用户名、密码、登录域判断是否成功
            /// </summary>
            /// <example>
            /// if (impersonateValidUser(UserName, Domain, Password)){}
            /// </example>
            /// <param name="userName">账户名称，如：string UserName = UserNameTextBox.Text;</param>
            /// <param name="domain">要登录的域，如：string Domain   = DomainTextBox.Text;</param>
            /// <param name="password">账户密码, 如：string Password = PasswordTextBox.Text;</param>
            /// <returns>成功返回true,否则返回false</returns>
            public bool impersonateValidUser(String userName, String domain, String password)
            {
                WindowsIdentity tempWindowsIdentity;
                IntPtr token = IntPtr.Zero;
                IntPtr tokenDuplicate = IntPtr.Zero;

                if (LogonUser(userName, domain, password, LOGON32_LOGON_INTERACTIVE,
                LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            public void undoImpersonation()
            {
                impersonationContext.Undo();
            }
            // #endregion
        }
    }
}