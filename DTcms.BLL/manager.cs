using System;
using System.Data;
using System.Collections.Generic;
using DTcms.Common;

namespace DTcms.BLL
{
    /// <summary>
    /// 管理员信息表
    /// </summary>
    public partial class manager
    {
        private readonly Model.siteconfig siteConfig = new BLL.siteconfig().loadConfig(); //获得站点配置信息
        private readonly DAL.manager dal;
        public manager()
        {
            dal = new DAL.manager(siteConfig.sysdatabaseprefix);
        }

        #region 基本方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return dal.Exists(id);
        }

        /// <summary>
        /// 用于判定域用户是否存在
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool IsAdManagerExists(string adID)
        {
            return dal.IsAdManagerExists(adID);
        }



        /// <summary>
        /// 查询用户名是否存在
        /// </summary>
        public bool Exists(string user_name)
        {
            return dal.Exists(user_name);
        }

        /// <summary>
        /// 根据用户名取得Salt
        /// </summary>
        public string GetSalt(string user_name)
        {
            return dal.GetSalt(user_name);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.manager model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.manager model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据,用于域用户同步的更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateByAdID(Model.manager model)
        {
            return dal.UpdateByAdID(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            return dal.Delete(id);
        }

        /// <summary>
        /// 删除一条数据，用于域用户的同步更新上
        /// </summary>
        /// <param name="adID"></param>
        /// <returns></returns>
        public bool Delete(string adID)
        {
            return dal.Delete(adID);
        }

        /// <summary>
        /// 删除表中所有记录，除超级管理员（id为1）
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            return dal.Delete();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.manager GetModel(int id)
        {
            return dal.GetModel(id);
        }
     
        /// <summary>
        /// 根据用户名密码返回一个实体
        /// </summary>
        public Model.manager GetModel(string user_name, string password)
        {
            return dal.GetModel(user_name, password);
        }

        /// <summary>
        /// 根据用户名返回一个实体
        /// </summary>
        public Model.manager GetModel(string user_name)
        {
            return dal.GetModel(user_name);
        }

        /// <summary>
        /// 根据用户名密码返回一个实体
        /// </summary>
        public Model.manager GetModel(string user_name, string password, bool is_encrypt)
        {
            //检查一下是否需要加密
            if (is_encrypt)
            {
                //先取得该用户的随机密钥
                string salt = dal.GetSalt(user_name);
                if (string.IsNullOrEmpty(salt))
                {
                    return null;
                }
                //把明文进行加密重新赋值
                password = DESEncrypt.Encrypt(password, salt);
            }
            return dal.GetModel(user_name, password);
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataSet GetList(string strSql)
        {
            return dal.GetList(strSql);
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int role_id,int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(role_id,pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }

        #endregion  Method
    }
}