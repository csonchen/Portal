using DTcms.DBUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DTcms.DAL
{
    public partial class department
    {
        private string databaseprefix; //数据库表名前缀
        public department(string _databaseprefix)
		{
            databaseprefix = _databaseprefix;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Model.department model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into " + databaseprefix + "department(");
            strSql.Append("id,name,typeId,parentId)");
            strSql.Append(" values (");
            strSql.Append("@id,@name,@typeId,@parentId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.NVarChar,50),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@typeId", SqlDbType.Int,4),
					new SqlParameter("@parentId", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.typeId;
            parameters[3].Value = model.parentId;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 取得所有部门列表
        /// </summary>
        /// <param name="parent_id">父ID</param>
        /// <returns></returns>
        public DataTable GetList(string parent_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,name,typeId,parentId from " + databaseprefix + "department");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataTable oldData = ds.Tables[0] as DataTable;
            if (oldData == null)
            {
                return null;
            }
            //复制结构
            DataTable newData = oldData.Clone();
            //调用迭代组合成DAGATABLE
            GetChilds(oldData, newData, parent_id);
            return newData;
        }

        /// <summary>
        /// 从内存中取得所有下级类别列表（自身迭代）
        /// </summary>
        private void GetChilds(DataTable oldData, DataTable newData, string parent_id)
        {
            DataRow[] dr = oldData.Select("parentId='" + parent_id + "'");
            for (int i = 0; i < dr.Length; i++)
            {
                //添加一行数据
                DataRow row = newData.NewRow();
                row["id"] = dr[i]["id"].ToString();
                row["name"] = dr[i]["name"].ToString();
                row["typeId"] = int.Parse(dr[i]["typeId"].ToString());
                row["parentId"] = dr[i]["parentId"].ToString();
                newData.Rows.Add(row);
                //调用自身迭代
                this.GetChilds(oldData, newData, dr[i]["id"].ToString());
            }
        }

        /// <summary>
        /// 删除所有记录
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("delete from " + databaseprefix + "department");
            strsql.Append(" where 1=1");
            int rows = DbHelperSQL.ExecuteSql(strsql.ToString());
            return rows;
        }

        /// <summary>
        /// 返回部门名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetTitle(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 name from " + databaseprefix + "department");
            strSql.Append(" where id='" + id + "'");
            string title = Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString()));
            if (string.IsNullOrEmpty(title))
            {
                return "";
            }
            return title;
        }
    }
}
