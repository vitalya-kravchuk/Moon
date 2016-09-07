<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TestControl.ascx.cs" Inherits="Controls_Menu_TestControl" %>

<center>
<table width="480px">
    <tr>
        <td colspan="2" align="left">
            <asp:HyperLink ID="hlBook" runat="server" Target="_blank" /><br />
            &nbsp;
        </td>
    </tr>
    <tr>
        <td align="left" valign="top">
            <a href="http://ru.wikipedia.org/wiki/%D0%A1%D0%B5%D0%BB%D0%B5%D0%BD%D0%B0" target="_blank">
                <img src="<%=AppImagesPath%>Selena.jpg" alt=""
                    onmouseover="tooltip(this, 'Селена - Богиня Луны');" onmouseout="tooltipHide(this);" style="border: 0px" />
            </a>
        </td>
        <td align="center" valign="middle">
            <table cellpadding="3">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblTitle" runat="server" Font-Bold="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblBody" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table id="tblStartTest" runat="server">
                            <tr>
                                <td>
                                    <asp:Button ID="btnStartTest" runat="server" Text="Начать тест" 
                                        OnClientClick="parent.Dialog.HideContent();"
                                        onclick="btnStartTest_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnClose" runat="server" Text="Закрыть"
                                        OnClientClick="parent.Dialog.Hide(); return false;" />
                                </td>
                            </tr>
                        </table>
                        <table id="tblAnswers" runat="server">
                            <tr>
                                <td>
                                    <asp:Button ID="btnYes" runat="server" Text="Да" onclick="btnYes_Click"
                                        OnClientClick="parent.Dialog.HideContent();" />
                                </td>
                                <td>
                                    <asp:Button ID="btnNo" runat="server" Text="Нет" onclick="btnNo_Click" 
                                        OnClientClick="parent.Dialog.HideContent();" />
                                </td>
                            </tr>
                        </table>
                        <asp:Button ID="btnOK" runat="server" Text="OK"
                            OnClientClick="parent.Dialog.Hide(); return false;" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</center>

<asp:HiddenField ID="hfQuestionIndex" runat="server" Value="" />
<asp:HiddenField ID="hfPoints" runat="server" Value="" />
