<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="department_list.aspx.cs" Inherits="DTcms.Web.admin.manager.department_list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
    <script type="text/javascript" src="../js/layout.js"></script>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            var ids = $(".id");
            var parentids = $(".parent");
            //构建存储数组
            var array = new Array();
            var obj = function (id,parid) {
                this.id = id;
                this.parid = parid;
                //层级
                this.layer = 1;
            };

            //遍历
            for (var item = 0; item < ids.length; item++)
            {
                var k = new obj(ids[item].innerHTML, parentids[item].innerHTML);
                array.push(k);//添加到集合中      
            }
            //$.each(ids, function (i, value) {
            //    $.each(parentids, function (j, zhi) {
            //        var k = new obj(value.innerHTML, zhi.innerHTML);
            //        array.push(k);
            //        j++;
            //        //跳出当前循环
            //        return false;
            //    });
            //    i++;
            //});
            //遍历id数组,附层级数
            for (var j = 1; j < array.length; j++) {
                //父
                var parent = array[j];
                for (var a = j - 1; a >= 0; a--) {
                    //子
                    var child = array[a];
                    if (parent.parid == child.id) {//匹配到父编号
                        //层级+1
                        array[j].layer += 1;
                        if (array[a].parid == "0") {//匹配到根节点
                            break;
                        }
                        //子赋值给父
                        parent = child;
                    }
                }
            }

            var layers = $(".hidlayer");
            var litfirst = $(".litfirst");
            //遍历隐藏的层级数标签，并且赋值
            for (var u = 0; u < layers.length; u++)
            {
                layers[u].innerHTML = array[u].layer;
                var layer = layers[u].innerHTML;
                //var LitStyle = "<span style='display:inline-block;width:{0}px;'></span>{1}{2}";
                var LitImg1 = "<span class='folder-open'></span>";
                var LitImg2 = "<span class='folder-line'></span>";

                //偏移长度
                var width = 0;
                //层级数格式化为整数形式
                var laynum = parseInt(layer);
                width = (laynum - 2) * 24;
                var LitStyle = "<span style='display:inline-block;width:" + width + "px;'></span>";
                if (layer == "1") {
                    litfirst[u].innerHTML = LitImg1;
                } else {
                    litfirst[u].innerHTML = LitStyle + LitImg2 + LitImg1;
                }
            }

            //输出到控制台上调试
            //$.each(array, function (i, value) {
            //    console.log(value.id + "," + value.parid + "," + value.layer);
            //});
        });
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <!--导航栏-->
    <div class="location">
      <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
      <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
      <i class="arrow"></i>
      <span>内容类别</span>
    </div>
    <!--/导航栏-->

<%--    <!--工具栏-->
    <div class="toolbar-wrap">
      <div id="floatHead" class="toolbar">
        <div class="l-list">
          <ul class="icon-list">
            <li><a class="add" href="#"><i></i><span>新增</span></a></li>
            <li><asp:LinkButton ID="btnSave" runat="server" CssClass="save"><i></i><span>保存</span></asp:LinkButton></li>
            <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
            <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del"><i></i><span>删除</span></asp:LinkButton></li>
          </ul>
        </div>
      </div>
    </div>
    <!--/工具栏-->--%>
    <!--列表-->
    <asp:Repeater ID="rptList" runat="server">
    <HeaderTemplate>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
      <tr>
        <th width="6%">选择</th>
        <th align="left" width="30%">编号</th>
        <th align="left">组织名称</th>
      </tr>
    </HeaderTemplate>
        
    <ItemTemplate>
      <tr>
        <td align="center">
          <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
          <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
          <span style="display:none" class="hidlayer"></span>
          <span style="display:none" class="parent"><%#Eval("parentId") %></span>
        </td>
        <td>
            <span class="id"><%#Eval("id")%></span>
        </td>
        <td>
          <span class="litfirst"></span>
          <%#Eval("name")%>
        </td>
      </tr>
    </ItemTemplate>
    <FooterTemplate>
      <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"6\">暂无记录</td></tr>" : ""%>
    </table>
    </FooterTemplate>
    </asp:Repeater>
    <!--/列表-->
    </form>
</body>
</html>
