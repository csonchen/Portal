<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role_edit.aspx.cs" Inherits="DTcms.Web.admin.manager.role_edit" ValidateRequest="false" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>编辑管理角色</title>
<script type="text/javascript" src="../../scripts/jquery/jquery-1.10.2.min.js"></script>
<script type="text/javascript" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>
<script type="text/javascript" src="../js/layout.js"></script>
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
        //是否启用权限
        if ($("#ddlRoleType").find("option:selected").attr("value") == 1) {
            $(".border-table").find("input[type='checkbox']").prop("disabled", true);
        }
        $("#ddlRoleType").change(function () {
            if ($(this).find("option:selected").attr("value") == 1) {
                $(".border-table").find("input[type='checkbox']").prop("checked", false);
                $(".border-table").find("input[type='checkbox']").prop("disabled", true);
            } else {
                $(".border-table").find("input[type='checkbox']").prop("disabled", false);
            }
        });
        //权限全选
        $("input[name='checkAll']").click(function () {
            if ($(this).prop("checked") == true) {
                $(this).parent().siblings("td").find("input[type='checkbox']").prop("checked", true);
            } else {
                $(this).parent().siblings("td").find("input[type='checkbox']").prop("checked", false);
            }
        });
        //绑定“禁止”权限处理
        $(".ban > input").click(function () {
            if ($(this).prop("checked") == true) {
                $(this).parent().parent().parent().siblings("td").find("input[type='checkbox']").prop("disabled", true);
                $(this).parent().parent().children("input").prop("disabled", true);
                //同行选项取消选择
                $(this).parent().parent().children("input").prop("checked",false);
                //获取向下的tr元素数组
                var nextTrArrs = $(this).parent().parent().parent().parent().nextAll("tr");
                for (var i = 0; i < 3; i++) {
                    //DOM对象转为jquery对象
                    $(nextTrArrs.get(i)).children("td").find("input[type='checkbox']").prop("disabled", true);
                    //对下三行选项取消选择
                    $(nextTrArrs.get(i)).children("td").find("input[type='checkbox']").prop("checked", false);
                }
            } else {
                $(this).parent().parent().parent().siblings("td").find("input[type='checkbox']").prop("disabled", false);
                $(this).parent().parent().children("input").prop("disabled", false);
                //获取向下的tr元素数组
                var nextTrArrs = $(this).parent().parent().parent().parent().nextAll("tr");
                for (var i = 0; i < 3; i++) {
                    //DOM对象转为jquery对象
                    $(nextTrArrs.get(i)).children("td").find("input[type='checkbox']").prop("disabled", false);
                }
            }
        });
        
    });
</script>
</head>

<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="role_list.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="manager_list.aspx"><span>管理员</span></a>
  <i class="arrow"></i>
  <a href="role_list.aspx"><span>管理角色</span></a>
  <i class="arrow"></i>
  <span>编辑角色</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div class="content-tab-wrap">
  <div id="floatHead" class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a href="javascript:;" onclick="tabs(this);" class="selected">编辑角色信息</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>角色类型</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlRoleType" runat="server" datatype="*" errormsg="请选择角色类型！" sucmsg=" "></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
      <script type="text/javascript">
          $(function () {
              //本地角色去除角色选择下拉框
              var origon = $(".rule-multi-radio > .boxwrap > .selected").html();
              if ($.trim(origon) == "本地") {
                  $(".rule-single-select:last").css("display", "none");
              }
              //获取“来源”数组
              var l = $(".rule-multi-radio > .boxwrap > a");
              //获取角色下拉选择框
              var roleSel = $(".rule-single-select:last");
              //遍历，绑定点击事件
              l.each(function () {
                  //console.log("oo");
                  $(this).click(function () {
                      var value = $(this).html();
                      if ($.trim(value) == "本地") {
                          roleSel.css("display", "none");
                      } else if ($.trim(value) == "AD域") {
                          roleSel.css("display", "inline");
                      }
                  });
              });
          });
      </script>
    <dt>来源</dt>
    <dd>
      <div class="rule-multi-radio">
        <asp:RadioButtonList ID="txtIdentify" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">本地</asp:ListItem>
        <asp:ListItem Value="1">AD域</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>所属父类别</dt>
    <dd>
      <div class="rule-single-select">
        <asp:DropDownList id="ddlParentId" runat="server"></asp:DropDownList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>角色名称</dt>
    <dd><asp:TextBox ID="txtRoleName" runat="server" CssClass="input normal" datatype="*1-100" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*角色中文名称，100字符内</span></dd>
  </dl>   
  <dl>
    <dt>管理权限</dt>
    <dd>
      <table border="0" cellspacing="0" cellpadding="0" class="border-table" width="98%">
        <thead>
          <tr>
            <th width="30%">导航名称</th>
            <th>权限分配</th>
            <th width="10%">全选</th>
          </tr>
        </thead>
        <tbody>
          <asp:Repeater ID="rptList" runat="server" onitemdatabound="rptList_ItemDataBound">
          <ItemTemplate>
          <tr>
            <td style="white-space:nowrap;word-break:break-all;overflow:hidden;">
              <asp:HiddenField ID="hidName" Value='<%#Eval("name") %>' runat="server" />
              <asp:HiddenField ID="hidActionType" Value='<%#Eval("action_type") %>' runat="server" />
              <asp:HiddenField ID="hidLayer" Value='<%#Eval("class_layer") %>' runat="server" />
              <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
              <%#Eval("title")%>
            </td>
            <td>
              <asp:CheckBoxList ID="cblActionType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="cbllist"></asp:CheckBoxList>
            </td>
            <td align="center"><input name="checkAll" type="checkbox" /></td>
          </tr>
          </ItemTemplate>
          </asp:Repeater>
        </tbody>
      </table>
    </dd>
  </dl>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-list">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
  <div class="clear"></div>
</div>
<!--/工具栏-->

</form>
</body>
</html>
