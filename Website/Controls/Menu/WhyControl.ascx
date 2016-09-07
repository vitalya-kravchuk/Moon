<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WhyControl.ascx.cs" Inherits="Controls_Menu_WhyControl" %>

<table>
    <tr>
        <td align="left" valign="top">
            <a href="http://ru.wikipedia.org/wiki/%D0%A1%D0%B5%D0%BB%D0%B5%D0%BD%D0%B0" target="_blank">
                <img src="<%=AppImagesPath%>Selena_large.jpg" alt=""
                    onmouseover="tooltip(this, 'Селена - Богиня Луны');" onmouseout="tooltipHide(this);" style="border: 0px" />
            </a>
        </td>
        <td align="left" valign="top">
            <table>
                <tr>
                    <td align="left">
                        <b>Н</b>а Западе Лунный календарь получил распространение только в последнее столетие, сначала как интересная экзотика. Более подробное знакомство с его ритмами и информацией убедило и многих европейцев, что существуют закономерности, действующие в жизни не только восточного человека, но и любого человека на Земле.
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <b>М</b>ногочисленные опыты доказали увеличение скорости обмена веществ в организме во время новолуния и уменьшения его к полнолунию. Отмечено, что в новолуние и полнолуние вода поднимается гораздо выше, чем в другое время. То же касается и человека, в основном состоящего из воды на 70%.
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Б</b>лагодаря календарю Вы узнаете, какое время самое благоприятное для тех или иных действий. Это, во-первых, помогает вам уберечься от многих неприятностей, а во-вторых, вы начинаете действовать более эффективно.
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:HyperLink ID="hlBook" runat="server" Target="_blank" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <b>Д</b>ополнительная информация:<br />
                        <a href="http://rutube.ru/tracks/707400.html?v=06097069bb821cbd5114ddddd86c52f2" target="_blank">О Луне. Вступление. С музыкой. Почему лучше жить по лунному циклу</a><br />
                        <a href="http://www.homeomed.ru/opros-moon-results.htm" target="_blank">Результаты опроса о влиянии Луны на состояние здоровья людей</a><br />
                        <a href="http://www.ametist-e.ru/consult_parapsy/parapsy_art008.htm" target="_blank">Влияние Луны на человека: вопросы и ответы</a><br />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibOK" runat="server" SkinID="OK"
                            OnClientClick="parent.Dialog.Hide(); return false;" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
