using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DTcms.Common;
using System.DirectoryServices;
using DTcms.Model;
using System.IO;
using System.Collections;

namespace DTcms.Web.admin.manager
{
    public partial class manager_list : Web.UI.ManagePage
    {
        private List<AdModel> list = new List<AdModel>();
        private List<string> ADIdList = new List<string>();
        //private List<AdModel> adModelListForLay = new List<AdModel>();//存储用于计算层级数

        protected int totalCount;
        protected int page;
        protected int pageSize;

        protected string keywords = string.Empty;
        //部门编号
        protected int role_id;

        protected void Page_Load(object sender, EventArgs e)
        {
            string connect = DTRequest.GetQueryString("connect");
            string msg = DTRequest.GetQueryString("msg");
            this.role_id = DTRequest.GetQueryInt("role_id");
            //string connect1 = Request.Params["connect"];
            try
            {
                if (connect != null)
                {
                    //同步域信息
                    SyncADMessage();
                    
                }

            }
            catch (Exception)
            {
                JscriptMsg("同步域用户失败！", Utils.CombUrlTxt("manager_list.aspx", "keywords={0}", this.keywords), "Error");
            }

            this.keywords = DTRequest.GetQueryString("keywords");

            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                ChkAdminLevel("manager_list", DTEnums.ActionEnum.View.ToString()); //检查权限
                Model.manager model = GetAdminInfo(); //取得当前管理员信息
                //绑定角色
                TreeBind();
                RptBind(role_id,"role_type>=" + model.role_type + CombSqlTxt(keywords), "add_time asc,id desc");
            }
        }

        /// <summary>
        /// 同步域信息
        /// </summary>
        private void SyncADMessage()
        {
            string domain = Request.Params["domain"];
            string username = Request.Params["username"];
            string password = Request.Params["password"];

            DirectoryEntry de;
            DirectoryEntry ou;

            //返回连接域服务器标志
            bool flag = IsConnected(domain, username, password, out de);

            //创建组织结构实体类
            Model.department department = new Model.department();
            BLL.department debll = new BLL.department();
            //创建角色实体类
            Model.manager_role managerRole = new Model.manager_role();
            BLL.manager_role rolebll = new BLL.manager_role();
            //创建管理员实体类
            Model.manager manager = new Model.manager();
            BLL.manager managerbll = new BLL.manager();

            if (flag)
            {
                bool flag2 = IsExistOU(de, out ou, "组织机构");
                SyncAll(ou);

                //先清空表中已有部门记录
                int rows = debll.Delete();
                //清空已有管理员记录
                //bool isManagerDel = managerbll.Delete();

                //获取角色表中现有的记录
                DataTable dt = rolebll.GetList("").Tables[0];
                List<string> oldAdList = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    string adID = dr["ad_id"] + "";
                    oldAdList.Add(adID);
                }

                //获取管理员表中现有的记录项
                DataTable dt2 = managerbll.GetList("").Tables[0];
                List<string> oldManagerList = new List<string>();
                foreach (DataRow dr in dt2.Rows)
                {
                    string adID = dr["ad_id"] + "";
                    oldManagerList.Add(adID);
                }

                //遍历所有域用户信息
                //AdModel ad
                foreach (AdModel ad in list)
                {
                    AdModel ad2 = new AdModel();
                    ad2 = ad;
                    //获取类型
                    int typeid = ad.TypeId;
                    if (typeid == 1)//类型：1，组织结构
                    {
                        //部门对象赋值
                        department.id = ad.Id;
                        department.name = ad.Name;
                        department.typeId = ad.TypeId;
                        department.parentId = ad.ParentId;
                        //角色对象赋值
                        managerRole.role_name = ad.Name;
                        managerRole.role_type = 2;//默认初始化为普通系统用户
                        managerRole.adID = ad.Id;//添加域用户id
                        managerRole.identify = 1;//域角色：1；本地：0；
                        managerRole.parent_id = ad.ParentId;//添加父id
                        //计算层级数
                        for (int i = list.Count - 1; i >= 0; i--)
                        {
                            if ("0".Equals(ad2.ParentId.Trim()))
                            {
                                managerRole.class_layer = 1;
                                break;
                            }

                            if (list[i].Id.Trim().Equals(ad2.ParentId.Trim()))
                            {
                                managerRole.class_layer += 1;
                                if ("0".Equals(list[i].ParentId.Trim()))
                                {
                                    break;
                                }
                                ad2 = list[i];
                            }
                        }
                        
                        //部门清空成功，添加部门信息
                        if (rows >= 0)
                        {
                            //插入数据到部门表
                            debll.Add(department);
                        }

                        /*角色记录：查找域id是否存在，存在则进行update处理，
                         * 否则进行insert处理（解决角色删除后权限需要重新分配的问题）
                         * ps：需要对数据库表中多余角色进行删除处理
                         */
                        bool isRoleExists = rolebll.Exists(managerRole.adID.Trim());
                        //添加域用户id
                        ADIdList.Add(managerRole.adID.Trim());
                        if (isRoleExists)
                        {
                            rolebll.UpdateByAdID(managerRole);
                            //移除需要更新的记录项，剩余的即为多余项
                            oldAdList.Remove(managerRole.adID.Trim());
                        }
                        else
                        {
                            rolebll.Add(managerRole);
                        }
                        managerRole.class_layer = 1;
                    }

                    if (typeid == 2)//类型：2，用户
                    {
                        //根据父编号查找部门名称
                        string departName = debll.GetTitle(ad.ParentId);
                        //查找角色id
                        int roleid = rolebll.GetRoleId(departName);
                        //角色附初始值
                        manager.user_name = ad.Name;
                        manager.role_id = roleid;
                        //获得6位的salt加密字符串
                        manager.salt = Utils.GetCheckCode(6);
                        //以随机生成的6位字符串做为密钥加密
                        manager.password = DESEncrypt.Encrypt("123456", manager.salt);//密码初始化为123456
                        manager.is_lock = 1;//默认锁定用户
                        manager.real_name = ad.DisplayName;
                        manager.job = ad.Job;
                        manager.adID = ad.Id;
                        manager.identify = 1;//域同步：1；本地：0

                        /*管理员记录：查找域id是否存在，存在则进行update处理，
                         * 否则进行insert处理
                         * ps：需要对数据库表中多余管理员进行删除处理（排除掉本地用户）
                         */
                        bool isAdManager = managerbll.IsAdManagerExists(manager.adID.Trim());
                        if (isAdManager)
                        {
                            managerbll.UpdateByAdID(manager);
                            //移除需要更新的记录项，剩余的即为多余项
                            oldManagerList.Remove(manager.adID.Trim());
                        }
                        else
                        {
                            managerbll.Add(manager);
                        }
                    }
                }

                //优化数据库管理员表信息
                for (int i = 0; i < oldManagerList.Count; i++)
                {
                    if (oldManagerList[i].Trim() != "")//本地添加的管理员adid为""
                    {
                        managerbll.Delete(oldManagerList[i].Trim());
                    }
                    
                }

                //优化数据库角色表信息
                for (int i = 0; i < oldAdList.Count; i++)
                {
                    if (oldAdList[i].Trim() != "")
                    {
                        rolebll.Delete(oldAdList[i].Trim());
                    } 
                }
                JscriptMsg("同步域用户成功！", Utils.CombUrlTxt("manager_list.aspx", "keywords={0}", this.keywords), "Success");
            }
        }

        /// <summary>
        /// 判断是否存在根结点
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ou"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        private bool IsExistOU(DirectoryEntry entry, out DirectoryEntry ou,string root)
        {
            ou = new DirectoryEntry();
            try
            {
                ou = entry.Children.Find("OU=" + root);

                return (ou != null);
            }
            catch (Exception ex)
            {
                //LogRecord.WriteLog("[IsExistOU方法]错误信息：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 判断是否连接上域服务器
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private bool IsConnected(string domainName, string userName, string userPwd, out DirectoryEntry domain)
        {
            domain = new DirectoryEntry();
            try
            {
                domain.Path = string.Format("LDAP://{0}", domainName);
                domain.Username = userName;
                domain.Password = userPwd;
                domain.AuthenticationType = AuthenticationTypes.Secure;

                domain.RefreshCache();

                return true;
            }
            catch (Exception ex)
            {
                //LogRecord.WriteLog("[IsConnected方法]错误信息：" + ex.Message);
                return false;
            }
        }

        #region 数据绑定=================================
        private void RptBind(int role_id,string _strWhere, string _orderby)
        {
            this.page = DTRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            if (role_id > 0)
            {
                this.ddlCategoryId.SelectedValue = role_id.ToString();
            }
            BLL.manager bll = new BLL.manager();
            this.rptList.DataSource = bll.GetList(role_id,this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            this.rptList.DataBind();
            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("manager_list.aspx", "role_id={0}&keywords={1}&page={2}", this.role_id.ToString(),this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 绑定角色 =================================
        private void TreeBind()
        {
            BLL.manager_role bll = new BLL.manager_role();
            DataTable dt = bll.GetListForAD("0");

            this.ddlCategoryId.Items.Clear();
            this.ddlCategoryId.Items.Add(new ListItem("请选择部门...", ""));

            foreach (DataRow dr in dt.Rows)
            {
                string id = dr["id"].ToString();
                int classLayer = int.Parse(dr["class_layer"].ToString());
                string roleName = dr["role_name"].ToString();
                if (classLayer == 1)
                {
                    this.ddlCategoryId.Items.Add(new ListItem(roleName, id));
                }
                else
                {
                    roleName = "├ " + roleName;
                    roleName = Utils.StringOfChar(classLayer - 1, "　") + roleName;
                    this.ddlCategoryId.Items.Add(new ListItem(roleName, id));
                }
            }
        }
        #endregion

        //筛选角色
        protected void ddlCategoryId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("manager_list.aspx", "role_id={0}&keywords={1}",
                ddlCategoryId.SelectedValue, this.keywords));
        }

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and (user_name like  '%" + _keywords + "%' or real_name like '%" + _keywords + "%' or email like '%" + _keywords + "%')");
            }

            return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("manager_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("manager_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("manager_page_size", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("manager_list.aspx", "keywords={0}", this.keywords));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ChkAdminLevel("manager_list", DTEnums.ActionEnum.Delete.ToString()); //检查权限
            int sucCount = 0;
            int errorCount = 0;
            BLL.manager bll = new BLL.manager();
            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (bll.Delete(id))
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除管理员" + sucCount + "条，失败" + errorCount + "条"); //记录日志
            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("manager_list.aspx", "keywords={0}", this.keywords), "Success");
        }

        public void SyncAll(DirectoryEntry entryOU)
        {
            DirectorySearcher mySearcher = new DirectorySearcher(entryOU, "(objectclass=organizationalUnit)"); //查询组织单位                 

            DirectoryEntry root = mySearcher.SearchRoot;   //查找根OU

            SyncRootOU(root);

            StringBuilder sb = new StringBuilder();

            sb.Append("\r\nID\t帐号\t类型\t父ID\r\n");

            foreach (var item in list)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}\r\n", item.Id, item.Name, item.TypeId, item.ParentId);
            }

            string str = sb.ToString();
            //LogRecord.WriteLog(sb.ToString());

            //MessageBox.Show("同步成功", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Application.Exit();
        }

        private void SyncRootOU(DirectoryEntry entry)
        {
            if (entry.Properties.Contains("ou") && entry.Properties.Contains("objectGUID"))
            {
                string rootOuName = entry.Properties["ou"][0].ToString();

                byte[] bGUID = entry.Properties["objectGUID"][0] as byte[];

                string id = BitConverter.ToString(bGUID);

                list.Add(new AdModel(id, rootOuName, (int)TypeEnum.OU, "0"));

                SyncSubOU(entry, id);
            }
        }

        private void SyncSubOU(DirectoryEntry entry, string parentId)
        {
            foreach (DirectoryEntry subEntry in entry.Children)
            {
                string entrySchemaClsName = subEntry.SchemaClassName;

                string[] arr = subEntry.Name.Split('=');
                string categoryStr = arr[0];
                string nameStr = arr[1];
                string id = string.Empty;
                //string displayName = subEntry.Properties["displayName"].ToString();
                //string displayName2 = subEntry.Properties["displayName"][0].ToString();

                if (subEntry.Properties.Contains("objectGUID"))   //SID
                {
                    byte[] bGUID = subEntry.Properties["objectGUID"][0] as byte[];

                    id = BitConverter.ToString(bGUID);
                }

                bool isExist = list.Exists(d => d.Id == id);

                switch (entrySchemaClsName)
                {
                    case "organizationalUnit":

                        if (!isExist)
                        {
                            list.Add(new AdModel(id, nameStr, (int)TypeEnum.OU, parentId));
                        }

                        SyncSubOU(subEntry, id);
                        break;
                    case "user":
                        string accountName = string.Empty;
                        string displayName = string.Empty;
                        string job = string.Empty;
                        if (subEntry.Properties.Contains("samaccountName"))
                        {
                            accountName = subEntry.Properties["samaccountName"][0].ToString();
                            displayName = subEntry.Properties["displayName"][0].ToString();
                            job = (subEntry.Properties["title"].Count == 0) ? "" : subEntry.Properties["title"][0].ToString();
                        }

                        if (!isExist)
                        {
                            list.Add(new AdModel(id, accountName, (int)TypeEnum.USER, parentId,displayName,job));
                        }
                        break;
                }
            }
        }

        public enum TypeEnum : int
        {
            /// <summary>
            /// 组织单位
            /// </summary>
            OU = 1,

            /// <summary>
            /// 用户
            /// </summary>
            USER = 2
        }
    }
}