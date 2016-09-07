<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SendMailControl.ascx.cs" Inherits="Controls_Menu_SendMailControl" %>

<center>
    <asp:Panel ID="pSendMail" runat="server" DefaultButton="ibOK">
        <table>
            <tr>
                <td rowspan="5" align="left" valign="bottom">
                    <img src="<%=AppImagesPath%>iam.jpg" alt="" style="border: solid 1px black"
                        onmouseover="tooltip(this, 'Виталик');" onmouseout="tooltipHide(this);" />
                    &nbsp;&nbsp;
                </td>
                <td align="left" colspan="2">
                    <div id="divMsgError" class="MessageError" style="display: none;">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Ваше имя</b>
                </td>
                <td align="right">
                    <asp:TextBox ID="tbName" runat="server" Width="200px" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>Ваш email</b>
                </td>
                <td align="right">
                    <asp:TextBox ID="tbEmail" runat="server" Width="200px" MaxLength="30" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <b>Сообщение</b>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:TextBox ID="tbMessage" runat="server" TextMode="MultiLine" Height="70px" Width="280px" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibOK" runat="server" SkinID="OK"
                                    OnClientClick="return OnOK();"
                                    OnClick="ibOK_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibCancel" runat="server" SkinID="Cancel"
                                    OnClientClick="parent.Dialog.Hide(); return false;" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</center>

<script type="text/javascript">
    function Validate()
    {
        var divMsgError = document.getElementById('divMsgError');
        divMsgError.innerHTML = '';
    
        fields = new Array();
        fields.push('<%=tbName.ClientID%>');
        fields.push('<%=tbEmail.ClientID%>');
        fields.push('<%=tbMessage.ClientID%>');
		if (Validation.RequiredFields(fields))
		{
		    divMsgError.innerHTML = 'Все поля обязательны к заполнению <br/>';
		}
		else if (!Validation.EmailValidator('<%=tbEmail.ClientID%>'))
		{
		    divMsgError.innerHTML = 'Email введен с ошибками <br/>';
		}
		
		var result = (divMsgError.innerHTML == '');
		if (result)
		{
		    divMsgError.style.display = 'none';
		}
		else
		{
		    divMsgError.style.display = 'block';
		}
		
		var pSendMail = '<%=pSendMail.ClientID%>';
        parent.Dialog.params.width = Dimension.GetElementWidth(pSendMail);
        parent.Dialog.params.height = Dimension.GetElementHeight(pSendMail) + 10;
        parent.Dialog.Resize();
        
		return result;
    }

    function OnOK()
    {
        if (Validate())
        {
            parent.Dialog.HideContent();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    if (document.getElementById('<%=tbName.ClientID%>').value == '')
        parent.Dialog.params.focusControlID = '<%=tbName.ClientID%>';
    else
        parent.Dialog.params.focusControlID = '<%=tbMessage.ClientID%>';
</script>
