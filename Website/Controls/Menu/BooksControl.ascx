<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BooksControl.ascx.cs" Inherits="Controls_Menu_BooksControl" %>

<table width="100%">
    <tr>
        <td align="center">
            <asp:PlaceHolder ID="phCategory" runat="server" />
        </td>
    </tr>
</table>

<div style="width: 100%; height: 421px; overflow-y: scroll; overflow-x: hidden;">
<asp:ListView ID="lvBooks" runat="server">
    <GroupTemplate>
        <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
    </GroupTemplate>
    <LayoutTemplate>
        <asp:PlaceHolder ID="groupPlaceHolder" runat="server" />
    </LayoutTemplate>
    <ItemTemplate>
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td align="left" valign="top">
                    <img src='<%# Eval("CoverURI") %>' alt='' />
                </td>
                <td align="left" valign="top">
                    <table>
                        <tr>
                            <td align="left">
                                <%# Eval("Author") %>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <b><%# Eval("Name") %></b>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <%# GetShortText(Eval("Description")) %>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table cellpadding="6" cellspacing="6">
                                    <tr>
                                        <td>
                                            <%# GetDownloadButton(Eval("DownloadURI")) %>
                                        </td>
                                        <td>
                                            <%# GetOrderButton(Eval("OrderURL"))%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </ItemTemplate>
</asp:ListView>
</div>

<p />
<div class="DialogBottomLine" style="width: 100%;"></div>

<p />
<table width="100%">
    <tr>
        <td align="left">
            Для просмотра книг скачать программы:
            <a target="_blank" href="/Download.aspx?file=App_Storage\Soft\WinDjView.zip">DJVU</a>,
            <a target="_blank" href="/Download.aspx?file=App_Storage\Soft\FoxitReader.zip">PDF</a>
        </td>
    </tr>
    <tr>
        <td align="left">
            <b>Скачивание книг предоставляются исключительно для ознакомительных целей и защищены авторским правом.</b>
        </td>
    </tr>
</table>

<table width="100%">
    <tr>
        <td align="center">
            <asp:ImageButton ID="ibOK" runat="server" SkinID="OK"
                OnClientClick="parent.Dialog.Hide(); return false;" />
        </td>
    </tr>
</table>
