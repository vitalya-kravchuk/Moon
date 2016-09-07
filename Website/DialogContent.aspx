<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DialogContent.aspx.cs" Inherits="DialogContent" Culture="ru-Ru" UICulture="ru-RU" EnableViewStateMac="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1251" />
</head>
<body class="DialogContent">
    <form id="ContentForm" runat="server">
        <asp:AjaxScriptManager ID="AjaxScriptManager" runat="server" 
            EnableScriptGlobalization="true" EnableScriptLocalization="true">
            <Scripts>
                <asp:ScriptReference Path="~/App_Scripts/browser.js" />
                <asp:ScriptReference Path="~/App_Scripts/dimension.js" />
                <asp:ScriptReference Path="~/App_Scripts/keyevent.js" />
                <asp:ScriptReference Path="~/App_Scripts/tooltip.js" />
                <asp:ScriptReference Path="~/App_Scripts/validation.js" />
            </Scripts>
        </asp:AjaxScriptManager>
        <asp:PlaceHolder ID="phContent" runat="server" />
        <div id="divTooltip" class="Tooltip" style="position: absolute; z-index: 1000; visibility: hidden; white-space: nowrap;">
        </div>
    </form>
    <script type="text/javascript">
        KeyEvent.child = true;
        if (parent.Dialog)
        {
            parent.Dialog.LoadingComplete();
            
			if (parent.Dialog.params)
			{
				var focusControlID = parent.Dialog.params.focusControlID;
				if (focusControlID)
				{
					document.getElementById(focusControlID).select();
				}
			}
        }
    </script>
</body>
</html>
