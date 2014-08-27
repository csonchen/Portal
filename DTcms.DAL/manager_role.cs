using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// ���ݷ�����:�����ɫ
    /// </summary>
    public partial class manager_role
    {
        private string databaseprefix; //���ݿ����ǰ׺
        public manager_role(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region  Method
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "manager_role");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڼ�¼���������û�ͬ���ж��ϣ�
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool Exists(string adID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "manager_role");
            strSql.Append(" where ad_id=@ad_id ");
            SqlParameter[] parameters = {
					new SqlParameter("@ad_id", SqlDbType.NVarChar,50)};
            parameters[0].Value = adID;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���ؽ�ɫ����
        /// </summary>
        public string GetTitle(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 role_name from " + databaseprefix + "manager_role");
            strSql.Append(" where id=" + id);
            string title = Convert.ToString(DbHelperSQL.GetSingle(strSql.ToString()));
            if (string.IsNullOrEmpty(title))
            {
                return "";
            }
            return title;
        }

        /// <summary>
        /// ���ؽ�ɫ���
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetRoleId(string title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id from " + databaseprefix + "manager_role");
            strSql.Append(" where role_name='" + title + "'");
            int roleId = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            return roleId;
        }

        /// <summary>
        /// ����ad���ŷ��ؽ�ɫ���
        /// </summary>
        /// <returns></returns>
        public int GetRoleIdByADId(string adID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id from " + databaseprefix + "manager_role");
            strSql.Append(" where ad_id='" + adID + "'");
            int roleId = Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString()));
            return roleId;
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(Model.manager_role model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using(SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into " + databaseprefix + "manager_role(");
                        strSql.Append("role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer)");
                        strSql.Append(" values (");
                        strSql.Append("@role_name,@role_type,@is_sys,@ad_id,@identify,@parent_id,@class_layer)");
                        strSql.Append(";set @ReturnValue= @@IDENTITY");
                        SqlParameter[] parameters = {
					    new SqlParameter("@role_name", SqlDbType.NVarChar,100),
					    new SqlParameter("@role_type", SqlDbType.TinyInt,1),
                        new SqlParameter("@is_sys", SqlDbType.TinyInt,1),
                        new SqlParameter("@ad_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@identify",SqlDbType.Int,4),
                        new SqlParameter("@parent_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@class_layer",SqlDbType.Int,4),
                        new SqlParameter("@ReturnValue",SqlDbType.Int)};
                        parameters[0].Value = model.role_name;
                        parameters[1].Value = model.role_type;
                        parameters[2].Value = model.is_sys;
                        parameters[3].Value = model.adID;
                        parameters[4].Value = model.identify;
                        parameters[5].Value = model.parent_id;
                        parameters[6].Value = model.class_layer;
                        parameters[7].Direction = ParameterDirection.Output;
                        List<CommandInfo> sqllist = new List<CommandInfo>();
                        CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);

                        StringBuilder strSql2;
                        if (model.manager_role_values != null)
                        {
                            foreach (Model.manager_role_value modelt in model.manager_role_values)
                            {
                                strSql2 = new StringBuilder();
                                strSql2.Append("insert into " + databaseprefix + "manager_role_value(");
                                strSql2.Append("role_id,nav_name,action_type)");
                                strSql2.Append(" values (");
                                strSql2.Append("@role_id,@nav_name,@action_type)");
                                SqlParameter[] parameters2 = {
						        new SqlParameter("@role_id", SqlDbType.Int,4),
					            new SqlParameter("@nav_name", SqlDbType.NVarChar,100),
					            new SqlParameter("@action_type", SqlDbType.NVarChar,50)};
                                parameters2[0].Direction = ParameterDirection.InputOutput;
                                parameters2[1].Value = modelt.nav_name;
                                parameters2[2].Value = modelt.action_type;
                                cmd = new CommandInfo(strSql2.ToString(), parameters2);
                                sqllist.Add(cmd);
                            }
                        }
                        DbHelperSQL.ExecuteSqlTranWithIndentity(sqllist);

                        //���²㼶��
                        if (!"0".Equals(model.parent_id.Trim()))
                        {
                            Model.manager_role model2 = GetModel(conn, trans, model.parent_id);
                            model.class_layer = model2.class_layer + 1;
                        }
                        else 
                        {
                            model.class_layer = 1;
                        }
                        DbHelperSQL.ExecuteSql(conn, trans, "update " + databaseprefix + "manager_role set class_layer=" + model.class_layer + " where ad_id='" + model.adID.Trim() + "'");
                        trans.Commit();
                        return (int)parameters[7].Value;
                    }
                    catch
                    {
                        trans.Rollback();
                        return 0;
                    }
                }
            }


            //if (model.parent_id > 0)
            //{
            //    Model.article_category model2 = GetModel(conn, trans, model.parent_id); //������
            //    model.class_list = model2.class_list + model.id + ",";
            //    model.class_layer = model2.class_layer + 1;
            //}
            //else
            //{
            //    model.class_list = "," + model.id + ",";
            //    model.class_layer = 1;
            //}
            ////�޸Ľڵ��б�����
            //DbHelperSQL.ExecuteSql(conn, trans, "update " + databaseprefix + "article_category set class_list='" + model.class_list + "', class_layer=" + model.class_layer + " where id=" + model.id); //������
            //trans.Commit();
        }
        /// <summary>
        /// �õ�һ������ʵ��(���أ�������)
        /// </summary>
        public Model.manager_role GetModel(SqlConnection conn, SqlTransaction trans, string adID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer from " + databaseprefix + "manager_role ");
            strSql.Append(" where ad_id=@ad_id");
            SqlParameter[] parameters = {
					new SqlParameter("@ad_id", SqlDbType.NVarChar,50)};
            parameters[0].Value = adID;

            Model.manager_role model = new Model.manager_role();
            DataSet ds = DbHelperSQL.Query(conn, trans, strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region ������Ϣ
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                model.role_name = ds.Tables[0].Rows[0]["role_name"].ToString();
                if (ds.Tables[0].Rows[0]["role_type"].ToString() != "")
                {
                    model.role_type = int.Parse(ds.Tables[0].Rows[0]["role_type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_sys"].ToString() != "")
                {
                    model.is_sys = int.Parse(ds.Tables[0].Rows[0]["is_sys"].ToString());
                }
                model.adID = ds.Tables[0].Rows[0]["ad_id"].ToString();
                if (ds.Tables[0].Rows[0]["identify"].ToString() != "")
                {
                    model.identify = int.Parse(ds.Tables[0].Rows[0]["identify"].ToString());
                }
                model.parent_id = ds.Tables[0].Rows[0]["parent_id"].ToString();
                if (ds.Tables[0].Rows[0]["class_layer"].ToString() != "")
                {
                    model.class_layer = int.Parse(ds.Tables[0].Rows[0]["class_layer"].ToString());
                }
                #endregion
                return model;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// ����һ�����ݣ��������û�ͬ����������id���£�
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByAdID(Model.manager_role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "manager_role set ");
            strSql.Append("role_name=@role_name,");
            strSql.Append("role_type=@role_type,");
            strSql.Append("is_sys=@is_sys,");
            strSql.Append("identify=@identify,");
            strSql.Append("parent_id=@parent_id,");
            strSql.Append("class_layer=@class_layer");
            strSql.Append(" where ad_id=@ad_id");
            SqlParameter[] parameters = {
					new SqlParameter("@role_name", SqlDbType.NVarChar,100),
					new SqlParameter("@role_type", SqlDbType.TinyInt,1),
                    new SqlParameter("@is_sys", SqlDbType.TinyInt,1),
                    new SqlParameter("@identify",SqlDbType.Int,4),
                    new SqlParameter("@parent_id", SqlDbType.NVarChar,50),
                    new SqlParameter("@class_layer",SqlDbType.Int,4),
                    new SqlParameter("@ad_id", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.role_name;
            parameters[1].Value = model.role_type;
            parameters[2].Value = model.is_sys;
            parameters[3].Value = model.identify;
            parameters[4].Value = model.parent_id;
            parameters[5].Value = model.class_layer;
            parameters[6].Value = model.adID;

            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            //��ɾ���ý�ɫ����Ȩ��
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from " + databaseprefix + "manager_role_value where role_id=@role_id ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@role_id", SqlDbType.Int,4)};
            parameters2[0].Value = model.id;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //���Ȩ��
            if (model.manager_role_values != null)
            {
                StringBuilder strSql3;
                foreach (Model.manager_role_value modelt in model.manager_role_values)
                {
                    strSql3 = new StringBuilder();
                    strSql3.Append("insert into " + databaseprefix + "manager_role_value(");
                    strSql3.Append("role_id,nav_name,action_type)");
                    strSql3.Append(" values (");
                    strSql3.Append("@role_id,@nav_name,@action_type)");
                    SqlParameter[] parameters3 = {
						    new SqlParameter("@role_id", SqlDbType.Int,4),
					        new SqlParameter("@nav_name", SqlDbType.NVarChar,100),
					        new SqlParameter("@action_type", SqlDbType.NVarChar,50)};
                    parameters3[0].Value = model.id;
                    parameters3[1].Value = modelt.nav_name;
                    parameters3[2].Value = modelt.action_type;
                    cmd = new CommandInfo(strSql3.ToString(), parameters3);
                    sqllist.Add(cmd);
                }
            }

            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.manager_role model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update " + databaseprefix + "manager_role set ");
                        strSql.Append("role_name=@role_name,");
                        strSql.Append("role_type=@role_type,");
                        strSql.Append("is_sys=@is_sys,");
                        strSql.Append("ad_id=@ad_id,");
                        strSql.Append("identify=@identify,");
                        strSql.Append("parent_id=@parent_id,");
                        strSql.Append("class_layer=@class_layer");
                        strSql.Append(" where id=@id");
                        SqlParameter[] parameters = {
					    new SqlParameter("@role_name", SqlDbType.NVarChar,100),
					    new SqlParameter("@role_type", SqlDbType.TinyInt,1),
                        new SqlParameter("@is_sys", SqlDbType.TinyInt,1),
                        new SqlParameter("@ad_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@identify",SqlDbType.Int,4),
                        new SqlParameter("@parent_id",SqlDbType.NVarChar,50),
                        new SqlParameter("@class_layer",SqlDbType.Int,4),
                        new SqlParameter("@id", SqlDbType.Int,4)};
                        parameters[0].Value = model.role_name;
                        parameters[1].Value = model.role_type;
                        parameters[2].Value = model.is_sys;
                        parameters[3].Value = model.adID;
                        parameters[4].Value = model.identify;
                        parameters[5].Value = model.parent_id;

                        //����Ԫ�صĸ���Ų��Ҹ��Ĳ㼶��,�Ӳ㼶�� = ���㼶�� +����
                        //���²㼶��
                        if (!"0".Equals(model.parent_id.Trim()))
                        {
                            Model.manager_role model2 = GetModel(conn, trans, model.parent_id);
                            model.class_layer = model2.class_layer + 1;
                        }
                        parameters[6].Value = model.class_layer;
                        parameters[7].Value = model.id;

                        List<CommandInfo> sqllist = new List<CommandInfo>();
                        CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
                        sqllist.Add(cmd);

                        //��ɾ���ý�ɫ����Ȩ��
                        StringBuilder strSql2 = new StringBuilder();
                        strSql2.Append("delete from " + databaseprefix + "manager_role_value where role_id=@role_id ");
                        SqlParameter[] parameters2 = {
					    new SqlParameter("@role_id", SqlDbType.Int,4)};
                        parameters2[0].Value = model.id;
                        cmd = new CommandInfo(strSql2.ToString(), parameters2);
                        sqllist.Add(cmd);

                        //���Ȩ��
                        if (model.manager_role_values != null)
                        {
                            StringBuilder strSql3;
                            foreach (Model.manager_role_value modelt in model.manager_role_values)
                            {
                                strSql3 = new StringBuilder();
                                strSql3.Append("insert into " + databaseprefix + "manager_role_value(");
                                strSql3.Append("role_id,nav_name,action_type)");
                                strSql3.Append(" values (");
                                strSql3.Append("@role_id,@nav_name,@action_type)");
                                SqlParameter[] parameters3 = {
						    new SqlParameter("@role_id", SqlDbType.Int,4),
					        new SqlParameter("@nav_name", SqlDbType.NVarChar,100),
					        new SqlParameter("@action_type", SqlDbType.NVarChar,50)};
                                parameters3[0].Value = model.id;
                                parameters3[1].Value = modelt.nav_name;
                                parameters3[2].Value = modelt.action_type;
                                cmd = new CommandInfo(strSql3.ToString(), parameters3);
                                sqllist.Add(cmd);
                            }
                        }

                        int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// ɾ��һ�����ݣ����ֱ�����������ݣ��������û�ͬ�������ɫɾ������
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool Delete(string adID)
        {
            //����ɾ����ɫ���
            int id = GetRoleIdByADId(adID.Trim());

            return Delete(id);
        }

        /// <summary>
        /// ɾ��һ�����ݣ����ӱ������������
        /// </summary>
        public bool Delete(int id)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //ɾ�������ɫȨ��
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "manager_role_value ");
            strSql.Append(" where role_id=@role_id");
            SqlParameter[] parameters = {
					new SqlParameter("@role_id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);
            //ɾ�������ɫ
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from " + databaseprefix + "manager_role ");
            strSql2.Append(" where id=@id");
            SqlParameter[] parameters2 = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters2[0].Value = id;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ɾ�����н�ɫ��¼���������û���idΪ1��
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //ɾ�������ɫȨ��
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "manager_role_value ");
            strSql.Append(" where role_id!=@role_id");
            SqlParameter[] parameters = {
					new SqlParameter("@role_id", SqlDbType.Int,4)};
            parameters[0].Value = 1;//Ĭ�ϲ�ɾ�������û���ɫȨ��
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);
            //ɾ�������ɫ
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from " + databaseprefix + "manager_role ");
            strSql2.Append(" where id!=@id");
            SqlParameter[] parameters2 = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters2[0].Value = 1;//Ĭ�ϲ�ɾ�������û���ɫ
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            int rowsAffected = DbHelperSQL.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �õ�һ������ʵ�壨�����ϲ�ad��ĸ������
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public Model.manager_role GetModel(string parent_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer from " + databaseprefix + "manager_role ");
            strSql.Append(" where ad_id=@parent_id");
            SqlParameter[] parameters = {
					new SqlParameter("@parent_id", SqlDbType.NVarChar,50)};
            parameters[0].Value = parent_id;

            Model.manager_role model = new Model.manager_role();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region ������Ϣ
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                model.role_name = ds.Tables[0].Rows[0]["role_name"].ToString();
                if (ds.Tables[0].Rows[0]["role_type"].ToString() != "")
                {
                    model.role_type = int.Parse(ds.Tables[0].Rows[0]["role_type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_sys"].ToString() != "")
                {
                    model.is_sys = int.Parse(ds.Tables[0].Rows[0]["is_sys"].ToString());
                }
                model.adID = ds.Tables[0].Rows[0]["ad_id"].ToString();
                if (ds.Tables[0].Rows[0]["identify"].ToString() != "")
                {
                    model.identify = int.Parse(ds.Tables[0].Rows[0]["identify"].ToString());
                }
                model.parent_id = ds.Tables[0].Rows[0]["parent_id"].ToString();
                if (ds.Tables[0].Rows[0]["class_layer"].ToString() != "")
                {
                    model.class_layer = int.Parse(ds.Tables[0].Rows[0]["class_layer"].ToString());
                }
                #endregion

                //����ad_id���Ҹ�Ԫ��ʵ�ʱ��
                int id = GetRoleIdByADId(parent_id);

                #region �ӱ���Ϣ
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("select id,role_id,nav_name,action_type from dt_manager_role_value ");
                strSql2.Append(" where role_id=@role_id");
                SqlParameter[] parameters2 = {
					new SqlParameter("@role_id", SqlDbType.Int,4)};
                parameters2[0].Value = id;
                DataSet ds2 = DbHelperSQL.Query(strSql2.ToString(), parameters2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    List<Model.manager_role_value> models = new List<Model.manager_role_value>();
                    Model.manager_role_value modelt;
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        modelt = new Model.manager_role_value();
                        if (dr["id"].ToString() != "")
                        {
                            modelt.id = int.Parse(dr["id"].ToString());
                        }
                        if (dr["role_id"].ToString() != "")
                        {
                            modelt.role_id = int.Parse(dr["role_id"].ToString());
                        }
                        modelt.nav_name = dr["nav_name"].ToString();
                        modelt.action_type = dr["action_type"].ToString();
                        models.Add(modelt);
                    }
                    model.manager_role_values = models;
                }
                #endregion

                return model;
            }
            else
            {
                return null;
            }            
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.manager_role GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer from " + databaseprefix + "manager_role ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Model.manager_role model = new Model.manager_role();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                #region ������Ϣ
                if (ds.Tables[0].Rows[0]["id"].ToString() != "")
                {
                    model.id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                }
                model.role_name = ds.Tables[0].Rows[0]["role_name"].ToString();
                if (ds.Tables[0].Rows[0]["role_type"].ToString() != "")
                {
                    model.role_type = int.Parse(ds.Tables[0].Rows[0]["role_type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["is_sys"].ToString() != "")
                {
                    model.is_sys = int.Parse(ds.Tables[0].Rows[0]["is_sys"].ToString());
                }
                model.adID = ds.Tables[0].Rows[0]["ad_id"].ToString();
                if (ds.Tables[0].Rows[0]["identify"].ToString() != "")
                {
                    model.identify = int.Parse(ds.Tables[0].Rows[0]["identify"].ToString());
                }
                model.parent_id = ds.Tables[0].Rows[0]["parent_id"].ToString();
                if (ds.Tables[0].Rows[0]["class_layer"].ToString() != "")
                {
                    model.class_layer = int.Parse(ds.Tables[0].Rows[0]["class_layer"].ToString());
                }
                #endregion

                #region �ӱ���Ϣ
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("select id,role_id,nav_name,action_type from dt_manager_role_value ");
                strSql2.Append(" where role_id=@role_id");
                SqlParameter[] parameters2 = {
					new SqlParameter("@role_id", SqlDbType.Int,4)};
                parameters2[0].Value = id;
                DataSet ds2 = DbHelperSQL.Query(strSql2.ToString(), parameters2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    List<Model.manager_role_value> models = new List<Model.manager_role_value>();
                    Model.manager_role_value modelt;
                    foreach (DataRow dr in ds2.Tables[0].Rows)
                    {
                        modelt = new Model.manager_role_value();
                        if (dr["id"].ToString() != "")
                        {
                            modelt.id = int.Parse(dr["id"].ToString());
                        }
                        if (dr["role_id"].ToString() != "")
                        {
                            modelt.role_id = int.Parse(dr["role_id"].ToString());
                        }
                        modelt.nav_name = dr["nav_name"].ToString();
                        modelt.action_type = dr["action_type"].ToString();
                        models.Add(modelt);
                    }
                    model.manager_role_values = models;
                }
                #endregion

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer ");
            strSql.Append(" FROM " + databaseprefix + "manager_role ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// ��ȡ�����б�����+ad��
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DataTable GetListForAll(string parent_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer from " + databaseprefix + "manager_role");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataTable oldData = ds.Tables[0] as DataTable;
            if (oldData == null)
            {
                return null;
            }
            //���ƽṹ
            DataTable newData = oldData.Clone();
            //���õ�����ϳ�DAGATABLE
            GetChilds(oldData, newData, parent_id);
            return newData;            
        }
        /// <summary>
        /// ��ȡ�����б�����AD��ͬ����
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public DataTable GetListForAD(string parent_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer from " + databaseprefix + "manager_role");
            strSql.Append(" where identify != 0");
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            DataTable oldData = ds.Tables[0] as DataTable;
            if (oldData == null)
            {
                return null;
            }
            //���ƽṹ
            DataTable newData = oldData.Clone();
            //���õ�����ϳ�DAGATABLE
            GetChilds(oldData, newData, parent_id);

            //����������ӱ�����ӵĽ�ɫ
            //StringBuilder strSql2 = new StringBuilder();
            //strSql2.Append("select id,role_name,role_type,is_sys,ad_id,identify,parent_id,class_layer from " + databaseprefix + "manager_role");
            //strSql2.Append(" where identify=0");
            //DataSet ds2 = DbHelperSQL.Query(strSql2.ToString());
            //DataTable oldData2 = ds2.Tables[0] as DataTable;
            //foreach (DataRow dr in oldData2.Rows)
            //{ 
            //    ���һ������
            //    DataRow row = newData.NewRow();
            //    row["id"] = int.Parse(dr["id"].ToString());
            //    row["role_name"] = dr["role_name"].ToString();
            //    row["role_type"] = int.Parse(dr["role_type"].ToString());
            //    row["is_sys"] = int.Parse(dr["is_sys"].ToString());
            //    row["ad_id"] = dr["ad_id"].ToString();
            //    row["identify"] = int.Parse(dr["identify"].ToString());
            //    row["parent_id"] = dr["parent_id"].ToString();
            //    row["class_layer"] = int.Parse(dr["class_layer"].ToString());
            //    newData.Rows.Add(row);
            //}
            return newData;
        }
        /// <summary>
        /// ���ڴ���ȡ�������¼�����б����������
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="newData"></param>
        /// <param name="parent_id"></param>
        public void GetChilds(DataTable oldData, DataTable newData, string parent_id)
        {
            DataRow[] dr = oldData.Select("parent_id='" + parent_id + "'");
            for (int i = 0; i < dr.Length; i++)
            {
                //���һ������
                DataRow row = newData.NewRow();
                row["id"] = int.Parse(dr[i]["id"].ToString());
                row["role_name"] = dr[i]["role_name"].ToString();
                row["role_type"] = int.Parse(dr[i]["role_type"].ToString());
                row["is_sys"] = int.Parse(dr[i]["is_sys"].ToString());
                row["ad_id"] = dr[i]["ad_id"].ToString();
                row["identify"] = int.Parse(dr[i]["identify"].ToString());
                row["parent_id"] = dr[i]["parent_id"].ToString();
                row["class_layer"] = int.Parse(dr[i]["class_layer"].ToString());
                newData.Rows.Add(row);
                //�����������
                this.GetChilds(oldData, newData, dr[i]["ad_id"].ToString());
            }
        }

        #endregion  Method



        
    }
}