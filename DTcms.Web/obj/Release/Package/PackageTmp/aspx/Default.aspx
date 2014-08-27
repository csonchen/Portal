<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/flat-ui.css" rel="Stylesheet" type="text/css"/>
    <link href="css/easydropdown.css" rel="Stylesheet" type="text/css"/>
    <link href="css/style.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="js/jquery.easydropdown.min.js" type="text/javascript"></script>
</head>
<body>
<table width="100%" style="height:100%;" border="0" cellspacing="0" cellpadding="0">
  <tr style="height:70px">
  </tr>
  <tr>
    <td><div class="con">
        <div class="login">
          <div class="b_left"></div>
          <div class="input">
            <div class="logo"><img alt="" src="images/apollo.png"></div>

			<form method=POST action="" name="form" target="_top">
			<table width="100%" border="0" cellspacing="0" cellpadding="0" class="logTb">
              <tr>
                <th>用户名</th>
                <td><input type="text" name="user" maxlength="20" class="input-border" />                </td>
                <td class="tishi">*</td>
              </tr>
              <tr>
                <th>密&nbsp;&nbsp;&nbsp;&nbsp;码</th>
                <td width="90"><input type="password" name="password" maxlength="20" class="input-border"></td>
                <td class="tishi">*</td>
              </tr>
              <tr>
                <th>部&nbsp;&nbsp;&nbsp;&nbsp;门</th>
                <td>
                    <select class="dropdown" tabindex="7">
						<option value="1">车辆部</option>
						<option value="2">信息管理部</option>
					</select>
                </td>
                <td class="tishi">*</td>
              </tr>
              <tr>
                <th>职&nbsp;&nbsp;&nbsp;&nbsp;位</th>
                <td>
                    <select class="dropdown" tabindex="7" >
						<option value="1">部门经理</option>
						<option value="2">项目经理</option>
					</select>
                </td>
                <td class="tishi">*</td>
              </tr>
              <tr>
                <th>&nbsp;</th>
                <td  colspan="2" style="line-height:25px; padding-top:2px; padding-left:180px; padding-bottom:10px;">
                    <div class="Sel_Ver">
                     <input type="submit" value="" class="login-b"  onMouseOver="this.className='login-b2'" onMouseDown="this.className='login-b3'" onMouseOut="this.className='login-b'"/>
                    </div>
                    <font style="line-height:60px;padding-left:8px;">
                        <a href="#">注册</a>
                    </font>
                </td>
              </tr>
            </table>
			</form>
          </div>
        </div>
      </div></td>
  </tr>
  <tr>
    <td>
      <div style=" width:100%; height:141px;clear: both;">
        <div class="foot">
          <ul>
            <li><a href="" target="_blank">关于太阳神</a></li>
            <li><a href="" target="_blank">帮助</a> </li>
          </ul>
          <div style="text-align:right;">太阳神公司版权所有　&copy; 1997-2009&nbsp; &nbsp; </div>
        </div>
      </div>
    </td>
  </tr>
</table>
</body>
</html>
