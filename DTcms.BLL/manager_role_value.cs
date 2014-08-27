using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DTcms.BLL
{
    public partial class manager_role_value
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig();
        private readonly DAL.manager_role_value dal;
        public manager_role_value()
        {
            dal = new DAL.manager_role_value(siteConfig.sysdatabaseprefix);
        }

        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <returns></returns>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

    }
}
