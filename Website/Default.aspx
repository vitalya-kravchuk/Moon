<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Culture="ru-Ru" UICulture="ru-RU" EnableViewStateMac="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Мой Лунный Календарь</title>
    <link rel="shortcut icon" type="image/ico" href="/favicon.ico" />
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1251" />
    <asp:PlaceHolder runat="server" ID="phMeta" />
</head>
<body>
    <form id="AppForm" runat="server"> 
        <div id="divAppLoad">
            <table cellpadding="3">
                <tr>
                    <td valign="middle">
                        <img src="<%=ImagesPath%>loading.gif" alt="" />
                    </td>
                    <td valign="middle">
                        Пожалуйста, подождите
                    </td>
                </tr>
            </table>
        </div>
        <asp:AjaxScriptManager ID="AjaxScriptManager" runat="server" 
            EnableScriptGlobalization="true" EnableScriptLocalization="true">
            <Scripts>
                <asp:ScriptReference Path="~/App_Scripts/animation.js" />
                <asp:ScriptReference Path="~/App_Scripts/ballon.js" />
                <asp:ScriptReference Path="~/App_Scripts/browser.js" />
                <asp:ScriptReference Path="~/App_Scripts/dialog.js" />
                <asp:ScriptReference Path="~/App_Scripts/dimension.js" />
                <asp:ScriptReference Path="~/App_Scripts/event.js" />
                <asp:ScriptReference Path="~/App_Scripts/keyevent.js" />
                <asp:ScriptReference Path="~/App_Scripts/position.js" />
                <asp:ScriptReference Path="~/App_Scripts/request.js" />
                <asp:ScriptReference Path="~/App_Scripts/showdialog.js" />
                <asp:ScriptReference Path="~/App_Scripts/tooltip.js" />
            </Scripts>
        </asp:AjaxScriptManager>
        <div id="divAppContent" style="display:none;">
            <mc:MainControl ID="cMain" runat="server" />
        </div>
        <div id="divTooltip" class="Tooltip" style="position: absolute; z-index: 1000; visibility: hidden; white-space: nowrap;">
        </div>
    </form>
</body>
</html>
