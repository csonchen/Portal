using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;

namespace DTcms.BLL
{
    /// <summary>
    /// �����ɫ
    /// </summary>
    public partial class manager_role
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //���վ��������Ϣ
        private readonly DAL.manager_role dal;
        public manager_role()
        {
            dal = new DAL.manager_role(siteConfig.sysdatabaseprefix);
        }

        #region  Method
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(string adID)
        {
            return dal.Exists(adID);
        }

        /// <summary>
        /// ����һ�����ݣ��������û�ͬ����������id���£�
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByAdID(Model.manager_role model)
        {
            return dal.UpdateByAdID(model);
        }

        /// <summary>
        /// ����Ƿ���Ȩ��
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
            //�ݹ����ϲ�Ȩ��
            string parent_id = model.parent_id;
            while (!"0".Equals(parent_id.Trim()))
            {
                Model.manager_role parentModel = dal.GetModel(parent_id);
                //���ϲ㸸����Ȩ�޼���
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
        /// ���ؽ�ɫ����
        /// </summary>
        public string GetTitle(int id)
        {
            return dal.GetTitle(id);
        }

        /// <summary>
        /// ���ؽ�ɫ���
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public int GetRoleId(string title)
        {
            return dal.GetRoleId(title);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(Model.manager_role model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.manager_role model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// ɾ��һ�����ݣ����ֱ�����������ݣ��������û�ͬ�������ɫɾ������
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool Delete(string adID)
        {
            return dal.Delete(adID);
        }

        /// <summary>
        /// ɾ�����н�ɫ��¼���������û���idΪ1��
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return dal.Delete();
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.manager_role GetModel(int id)
        {
            return dal.GetModel(id);
        }

        /// <summary>
        /// �õ�һ������ʵ�壨�����ϲ�ad��ĸ������
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public Model.manager_role GetModel(string parent_id)
        {
            return dal.GetModel(parent_id);
        }

        /// <summary>
        /// ��������б�
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// ��������б�����ad��ͬ����
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public DataTable GetListForAD(string parent_id)
        {
            return dal.GetListForAD(parent_id);
        }

        /// <summary>
        /// ��ȡ�����б�����+ad��
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