<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default1.aspx.cs" Inherits="WebApplication4._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
    <title>ASP.NET模拟登录验证</title>
    </head>
    <body>
    <form id="form1" runat="server">
    <div style="text-align:center">
        账户：<asp:TextBox ID="UserNameTextBox" runat="server"></asp:TextBox><br />
        密码：<asp:TextBox ID="PasswordTextBox" runat="server"></asp:TextBox><br />
        域名：<asp:TextBox ID="DomainTextBox" runat="server"></asp:TextBox><br />
        <asp:Button ID="OKButton" runat="server" OnClick="OKButton_Click" Text="Button" /></div>
    </form>
    </body>
