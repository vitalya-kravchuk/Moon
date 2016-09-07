<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RhythmsViewControl.ascx.cs" Inherits="Controls_RhythmsView_RhythmsViewControl" %>

<center>
<asp:Panel ID="pRhythmsView" runat="server" DefaultButton="btnUpdate">
<table id="tblRhythmsView">
    <tr>
        <td align="center">
            <table id="tblSet" width="100%" runat="server">
                <tr>
                    <td align="left">
                        <asp:DropDownList ID="ddlRhythmType" runat="server" onchange="Update();" />
                    </td>
                    <td align="center" style="width: 90%; white-space: nowrap;">
                        <asp:DropDownList ID="ddlMonth" runat="server" onchange="Update();" />
                        <asp:TextBox ID="tbYear" runat="server" Width="40px" MaxLength="4"
                            style="text-align: center;" onblur="UpdateYear();" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" style="display: none;" OnClientClick="return false;" />
                    </td>
                    <td align="right">
                        <img src="<%=ImagesPath%>print.png" alt="" style="cursor: pointer"
                            onmouseover="tooltip(this, 'Печать')" onmouseout="tooltipHide(this)"
                            onclick="Print()" />
                    </td>
                </tr>
            </table>
            
            <table id="tblTitle" runat="server" width="100%">
                <tr>
                    <td align="center">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" /><br />
                        <asp:Label ID="lblPlace" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="center">
            <div id="divRhythms">
                <asp:PlaceHolder ID="phRhythms" runat="server" />
            </div>
        </td>
    </tr>
</table>
</asp:Panel>
</center>

<script type="text/javascript">
    var tblRhythmsViewID = 'tblRhythmsView';
    var ddlRhythmType = document.getElementById('<%=ddlRhythmType.ClientID%>');
    var ddlMonth = document.getElementById('<%=ddlMonth.ClientID%>');
    var tbYear = document.getElementById('<%=tbYear.ClientID%>');
    
    var year;
    if (tbYear) year = tbYear.value;
    
    function getSelVal(objSel)
    {
        if (objSel)
            return objSel.options[objSel.selectedIndex].value;
        else
            return '';
    }
    function GetParams()
    {
        return 'rhythmType=' + getSelVal(ddlRhythmType) +
               '&month=' + getSelVal(ddlMonth) +
               '&year=' + tbYear.value;
    }
    function GetPath()
    {
        return '~/Controls/RhythmsView/RhythmsViewControl.ascx';
    }
    
    function Update()
    {
        var params =
        {
            width: Dimension.GetElementWidth(tblRhythmsViewID),
            height: Dimension.GetElementHeight(tblRhythmsViewID),
            title: 'Выбрать лунный ритм',
            path: GetPath(),
            params: GetParams()
        }
        parent.Dialog.Update(params);
    }
    function UpdateYear()
    {
        if (year != tbYear.value)
            Update();
    }
    function Print()
    {
        var url = 
            '<%=SitePath%>DialogContent.aspx?' +
            'path=' + GetPath() + 
            '&' + GetParams() + 
            '&print=1';
        window.open(url, '_blank');
    }
    function Resize()
    {
        var height = Dimension.GetElementHeight(tblRhythmsViewID) + 10;
        if (height > 450)
        {
            var divRhythms = document.getElementById('divRhythms');
            divRhythms.style.height = '450px';
            divRhythms.style.overflowX = 'hidden';
            divRhythms.style.overflowY = 'scroll';
        }
        parent.Dialog.params.width = Dimension.GetElementWidth(tblRhythmsViewID) + 30;
        parent.Dialog.params.height = Dimension.GetElementHeight(tblRhythmsViewID) + 10;
        parent.Dialog.Resize();
    }
    
    if (parent.Dialog)
    {
        Resize();
    }
</script>
