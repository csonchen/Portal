using System;
using System.Collections.Generic;

namespace DTcms.Model
{
    /// <summary>
    /// ������ɫ:ʵ����
    /// </summary>
    [Serializable]
    public partial class manager_role
    {
        public manager_role()
        { }
        #region Model
        private int _id;
        private string _role_name;
        private int _role_type = 1;
        private int _is_sys = 0;
        private string _adID = "";
        private int _identify = 0;//��ɫ��Դ��0�����أ�1,AD��
        private string _parent_id = "";
        private int _class_layer = 1;
        /// <summary>
        /// ����ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// ��ɫ��Դ��ʶ����0�����أ�1��AD��
        /// </summary>
        public int identify
        {
            set { _identify = value; }
            get { return _identify; }
        }
        /// <summary>
        /// ��id
        /// </summary>
        public string adID
        {
            set { _adID = value; }
            get { return _adID; }
        }

        /// <summary>
        /// ���û��ĸ�id
        /// </summary>
        public string parent_id
        {
            set { _parent_id = value; }
            get { return _parent_id; }
        }
        /// <summary>
        /// �㼶��
        /// </summary>
        public int class_layer
        {
            set { _class_layer = value; }
            get { return _class_layer; }
        }
        /// <summary>
        /// ��ɫ����
        /// </summary>
        public string role_name
        {
            set { _role_name = value; }
            get { return _role_name; }
        }
        /// <summary>
        /// ��ɫ����
        /// </summary>
        public int role_type
        {
            set { _role_type = value; }
            get { return _role_type; }
        }
        /// <summary>
        /// �Ƿ�ϵͳĬ��0��1��
        /// </summary>
        public int is_sys
        {
            set { _is_sys = value; }
            get { return _is_sys; }
        }
        #endregion Model

        private List<manager_role_value> _manager_role_values;
        /// <summary>
        /// Ȩ������ 
        /// </summary>
        public List<manager_role_value> manager_role_values
        {
            set { _manager_role_values = value; }
            get { return _manager_role_values; }
        }
    }
}