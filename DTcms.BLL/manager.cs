using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;

namespace DTcms.BLL
{
    /// <summary>
    /// ����Ա��Ϣ��
    /// </summary>
    public partial class manager
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //���վ��������Ϣ
        private readonly DAL.manager dal;
        public manager()
        {
            dal = new DAL.manager(siteConfig.sysdatabaseprefix);
        }

        #region ��������
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// �����ж����û��Ƿ����
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool IsAdManagerExists(string adID)
        {
            return dal.IsAdManagerExists(adID);
        }



        /// <summary>
        /// ��ѯ�û����Ƿ����
        /// </summary>
        public bool Exists(string user_name)
        {
            return dal.Exists(user_name);
        }

        /// <summary>
        /// �����û���ȡ��Salt
        /// </summary>
        public string GetSalt(string user_name)
        {
            return dal.GetSalt(user_name);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(Model.manager model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(Model.manager model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// ����һ������,�������û�ͬ���ĸ���
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByAdID(Model.manager model)
        {
            return dal.UpdateByAdID(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// ɾ��һ�����ݣ��������û���ͬ��������
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool Delete(string adID)
        {
            return dal.Delete(adID);
        }

        /// <summary>
        /// ɾ���������м�¼������������Ա��idΪ1��
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return dal.Delete();
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.manager GetModel(int id)
        {
            return dal.GetModel(id);
        }
     
        /// <summary>
        /// �����û������뷵��һ��ʵ��
        /// </summary>
        public Model.manager GetModel(string user_name, string password)
        {
            return dal.GetModel(user_name, password);
        }

        /// <summary>
        /// �����û�������һ��ʵ��
        /// </summary>
        public Model.manager GetModel(string user_name)
        {
            return dal.GetModel(user_name);
        }

        /// <summary>
        /// �����û������뷵��һ��ʵ��
        /// </summary>
        public Model.manager GetModel(string user_name, string password, bool is_encrypt)
        {
            //���һ���Ƿ���Ҫ����
            if (is_encrypt)
            {
                //��ȡ�ø��û��������Կ
                string salt = dal.GetSalt(user_name);
                if (string.IsNullOrEmpty(salt))
                {
                    return null;
                }
                //�����Ľ��м������¸�ֵ
                password = DESEncrypt.Encrypt(password, salt);
            }
            return dal.GetModel(user_name, password);
        }

        /// <summary>
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataSet GetList(string strSql)
        {
            return dal.GetList(strSql);
        }

        /// <summary>
        /// ��ò�ѯ��ҳ����
        /// </summary>
        public DataSet GetList(int role_id,int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(role_id,pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        #endregion  Method
    }
}