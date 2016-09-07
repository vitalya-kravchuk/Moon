<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DialogControl.ascx.cs" Inherits="Controls_Dialog_DialogControl" %>

<div id="divDlgOverlay" 
    class="OpacityDialogOverlay"
    style=" display: none; 
            left: 0px; top: 0px; width: 100%; height: 100%; 
            background-color: Black; z-index: 900; position: absolute;">
</div>

<div id="divDlg"
    style=" display: none; 
            border: solid 1px black; background-color: White;
            left: 10px; top: 10px; width: 100px; height: 100px; 
            z-index: 901; position: absolute;">
    <table id="tblDlgTitle" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td style="height: 25px; width: 5px;" class="DialogTitleStart">
            </td>
            <td style="width: 5px;" align="left" valign="middle" class="DialogTitle">
            </td>
            <td align="left" valign="middle" class="DialogTitle">
                <span id="spanDlgTitle" class="DialogTitleCaption"></span>
            </td>
            <td align="right" valign="middle" class="DialogTitle">
                <img src="<%=ImagesPath%>Dialog/close.jpg" align="absmiddle" alt="" style="cursor: pointer;"
                    onmouseover="tooltip(this, 'Закрыть');" onmouseout="tooltipHide(this);"
                    onclick="Dialog.Hide();" />
            </td>
            <td style="width: 5px;" class="DialogTitleEnd">
            </td>
        </tr>
    </table>
    
    <table cellpadding="3" cellspacing="3">
        <tr>
            <td id="tdDlgContent" align="center" valign="middle">
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript">
    Dialog.LoadingImageURI = '<%=ImagesPath%>loading.gif';
</script>
