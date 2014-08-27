﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manager_list.aspx.cs" Inherits="DTcms.Web.admin.manager.manager_list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>管理员列表</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<link  href="../../css/pagination.css" rel="stylesheet" type="text/css" />
<link href="../../css/style.css" rel="stylesheet" />
<script type="text/javascript" charset="utf-8" src="../js/jquery.leanModal.min.js"></script>


</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span>管理员列表</span>
</div>
<!--/导航栏-->

<!--工具栏-->
<div class="toolbar-wrap">
  <div id="floatHead" class="toolbar">
    <div class="l-list">
      <ul class="icon-list">
        <li><a class="add" href="manager_edit.aspx?action=<%=DTEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
        <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
        <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
        <li><a href="#loginmodal" class="arrow" id="modaltrigger"><i></i><span>同步</span></a></li>
      </ul>
      <div class="menu-list">
          <div class="rule-single-select">
              <asp:DropDownList ID="ddlCategoryId" runat="server" AutoPostBack="True" onselectedindexchanged="ddlCategoryId_SelectedIndexChanged"></asp:DropDownList>
          </div>
      </div>
    </div>
    <div class="r-list">
      <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
      <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" onclick="btnSearch_Click">查询</asp:LinkButton>
    </div>
  </div>
</div>
<!--/工具栏-->

<!--列表-->
<asp:Repeater ID="rptList" runat="server">
<HeaderTemplate>
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
  <tr>
    <th width="8%">选择</th>
    <th align="left">用户名</th>
    <th align="left" width="12%">姓名</th>
    <th align="left" width="12%">部门</th>
    <th align="left" width="12%">电话</th>
    <th align="left" width="16%">添加时间</th>
    <th width="8%">状态</th>
    <th width="8%">操作</th>
  </tr>
</HeaderTemplate>
<ItemTemplate>
  <tr>
    <td align="center">
      <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
      <asp:HiddenField ID="hidId" Value='<%#Eval("id")%>' runat="server" />
    </td>
    <td><a href="manager_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>"><%# Eval("user_name") %></a></td>
    <td><%# Eval("real_name") %></td>
    <td><%#new DTcms.BLL.manager_role().GetTitle(Convert.ToInt32(Eval("role_id")))%></td>
    <td><%# Eval("telephone") %></td>
    <td><%#string.Format("{0:g}",Eval("add_time"))%></td>
    <td align="center"><%#Eval("is_lock").ToString().Trim() == "0" ? "正常" : "禁用"%></td>
    <td align="center"><a href="manager_edit.aspx?action=<%#DTEnums.ActionEnum.Edit %>&id=<%#Eval("id")%>">修改</a></td>
  </tr>
</ItemTemplate>
<FooterTemplate>
  <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
</table>
</FooterTemplate>
</asp:Repeater>
<!--/列表-->
<!--模态窗口-->
<div id="loginmodal" style="display:none;">
    <h1>域管理员登陆</h1>
    <form id="loginform" name="loginform" method="post" action="manager_list.aspx?connect=cc">
        <label for="username">域名:</label>
        <input type="text" name="domain" id="domain" class="txtfield" tabindex="1"/>

        <label for="username">账号:</label>
        <input type="text" name="username" id="username" class="txtfield" tabindex="1"/>

        <label for="password">密码:</label>
        <input type="password" name="password" id="password" class="txtfield" tabindex="2"/>

        <div class="center">
            <input type="submit" name="loginbtn" id="loginbtn" class="flatbtn-blu hidemodal" value="确定" tabindex="3" />
        </div>
    </form>
  </div>
<!--/模态窗口-->
<!--内容底部-->
<div class="line20"></div>
<div class="pagelist">
  <div class="l-btns">
    <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);" ontextchanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
  </div>
  <div id="PageContent" runat="server" class="default"></div>
</div>
<!--/内容底部-->
</form>
<script type="text/javascript">
    $(function () {
        $('#loginform').submit(function (e) {
            return false;
        });
        $('#modaltrigger').leanModal({ top: 110, overlay: 0.45, closeButton: ".hidemodal" });
    });
</script>
</body>
</html>
