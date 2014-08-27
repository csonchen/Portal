using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTcms.DAL
{
    public partial class manager_role_value
    {
        private string databaseprefix; //数据库表名前缀
        public manager_role_value(string _databaseprefix)
		{
            databaseprefix = _databaseprefix;
        }

        /// <summary>
        /// 返回数据列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public System.Data.DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_id,nav_name,action_type ");
            strSql.Append("FROM " + databaseprefix + "manager_role_value ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
    }
}
