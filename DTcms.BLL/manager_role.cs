using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;

namespace DTcms.BLL
{
    /// <summary>
    /// 管理角色
    /// </summary>
    public partial class manager_role
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.manager_role dal;
        public manager_role()
        {
            dal = new DAL.manager_role(siteConfig.sysdatabaseprefix);
        }

        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string adID)
        {
            return dal.Exists(adID);
        }

        /// <summary>
        /// 更新一条数据（用于域用户同步：根据域id更新）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByAdID(Model.manager_role model)
        {
            return dal.UpdateByAdID(model);
        }

        /// <summary>
        /// 检查是否有权限
        /// </summary>
        public bool Exists(int role_id, string nav_name, string action_type)
        {
            Model.manager_role model = dal.GetModel(role_id);
            if (model != null)
            {
                if (model.role_type == 1)
                {
                    return true;
                }
                if (model.manager_role_values != null)
                {
                    Model.manager_role_value modelt = model.manager_role_values.Find(p => p.nav_name == nav_name && p.action_type == action_type);
                    if (modelt != null)
                    {
                        return true;
                    }
                }
            }
            //递归检查上层权限
            string parent_id = model.parent_id;
            while (!"0".Equals(parent_id.Trim()))
            {
                Model.manager_role parentModel = dal.GetModel(parent_id);
                //该上层父级的权限即可
                if (parentModel != null)
                {
                    if (parentModel.role_type == 1)
                    {
                        return true;
                    }
                    if (parentModel.manager_role_values != null)
                    {
                        Model.manager_role_value parentModelt = parentModel.manager_role_values.Find(p => p.nav_name == nav_name && p.action_type == action_type);
                        if (parentModelt != null)
                        {
                            return true;
                        }
                    }
                }
                parent_id = parentModel.parent_id;
            }
            return false;
        }

        /// <summary>
        /// 返回角色名称
        /// </summary>
        public string GetTitle(int id)
        {
            return dal.GetTitle(id);
        }

        /// <summary>
        /// 返回角色编号
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetRoleId(string title)
        {
            return dal.GetRoleId(title);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.manager_role model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.manager_role model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 删除一条数据，及字表所有相关数据（用于域用户同步多余角色删除处理）
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool Delete(string adID)
        {
            return dal.Delete(adID);
        }

        /// <summary>
        /// 删除所有角色记录，除超级用户（id为1）
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return dal.Delete();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.manager_role GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// 得到一个对象实体（查找上层ad域的父类对象）
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public Model.manager_role GetModel(string parent_id)
        {
            return dal.GetModel(parent_id);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表（用于ad域同步）
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DataTable GetListForAD(string parent_id)
        {
            return dal.GetListForAD(parent_id);
        }

        /// <summary>
        /// 获取数据列表（本地+ad域）
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DataTable GetListForAll(string parent_id)
        {
            return dal.GetListForAll(parent_id);
        }
        #endregion  Method
    }
}