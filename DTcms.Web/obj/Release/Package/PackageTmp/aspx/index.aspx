<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DTcms.Web.aspx.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(function () {
            //找到class属性为“nobelong”的元素,绑定点击触发函数
            $(".nobelong").each(function () {
                $(this).click(function () {
                    alert("亲，你没有权限访问该系统！");
                });
            });

            function date(type, obj, update) {
                function timeset() {
                    var date = new Date();
                    Y = date.getFullYear();
                    M = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                    D = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                    h = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
                    m = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
                    s = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
                    var time = type.replace("Y", Y); time = time.replace("M", M); time = time.replace("D", D);
                    time = time.replace("h", h); time = time.replace("m", m); time = time.replace("s", s);
                    if (obj) { $("#" + obj).html(time); }
                }

                if (update == 1) { setInterval(timeset, 1000); }
            }

            date("Y年M月D日 h时m分s秒", "currentTime", 1);

            //高度自适应
            var containHeight = $("#contain").height();
            var mainHeight = $("#main").height();
            var topHeight = $("#top").height();
            //设置高度
            var val = $("#contain").height(containHeight);
            //单个元素高度
            var sysHeight = $(".sys").first().height();
            var sysSize = $(".sys").length;
            var height = 0;
            //求行高元素个数
            if (sysSize % 4 == 0) {
                height = (sysSize / 4) * sysHeight + (sysSize / 4) * 30;
            } else {
                height = (parseInt(sysSize / 4) + 1) * sysHeight + (parseInt(sysSize / 4) + 1) * 30;
            }
            //设置主体高度
            $("#main").height(height);
            $("#contain").height(height + containHeight);
        });
    </script>
</head>
<body>
    <div id="manageMsg">
            <a href="../admin/login.aspx">后台登陆</a>
    </div>
    <div id="contain">
        <div id="top">
            <div class="left_msg">
                <span>用户名：</span>
                <span id="userName" runat="server"></span>
                <span>姓名：</span>
                <span id="realName" runat="server"></span>
                <span>部门：</span>
                <span id="department" runat="server"></span>
            </div>
            <div id="currentTime" class="right_msg">

            </div>
        </div>
        
        <div id="main" runat="server">
            
        </div>
        <div class="foot">
            <ul>
                <li><a href="" target="_blank">关于太阳神</a></li>
                <li><a href="" target="_blank">帮助</a> </li>
            </ul>
          <div style="text-align:right;">太阳神公司版权所有　&copy; 1988-2014&nbsp; &nbsp; </div>
        </div>
    </div>  
</body>
</html>
