using DTcms.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DTcms.Web.admin.manager
{
    public partial class department_list : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //绑定表格,显示数据
            RptBind();
        }

        //数据绑定
        private void RptBind()
        {
            BLL.department bll = new BLL.department();
            DataTable dt = bll.GetList("0");
            this.rptList.DataSource = dt;
            this.rptList.DataBind();
        }
    }
}