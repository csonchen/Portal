using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DTcms.BLL
{
    /// <summary>
    /// 组织结构
    /// </summary>
    public partial class department
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.department dal;

        public department()
        {
            dal = new DAL.department(siteConfig.sysdatabaseprefix);
        }

        #region 基本方法========
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Model.department model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 删除所有记录
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return dal.Delete();
        }

        /// <summary>
        /// 返回部门名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTitle(string id)
        {
            return dal.GetTitle(id);
        }

        /// <summary>
        /// 取得所有部门列表
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <returns></returns>
        public DataTable GetList(string parent_id)
        {
            return dal.GetList(parent_id);
        }
        #endregion
    }
}
