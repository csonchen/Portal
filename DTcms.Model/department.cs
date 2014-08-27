using System;
using System.Collections.Generic;
using System.Text;

namespace DTcms.Model
{
    #region 组织结构
    /// <summary>
    /// department:组织结构实体类
    /// </summary>
    public class department
    {
        //无参构造函数
        public department() { }

        private string _id = "";
        private string _name = "";
        private int _typeId;
        private string _parentId = "";

        /// <summary>
        /// 编号
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 账号（组织结构名称）
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 类型：1：组织；2：用户
        /// </summary>
        public int typeId
        {
            set { _typeId = value; }
            get { return _typeId; }
        }

        /// <summary>
        /// 父编号
        /// </summary>
        public string parentId
        {
            set { _parentId = value; }
            get { return _parentId; }
        }
    }
    #endregion
}
