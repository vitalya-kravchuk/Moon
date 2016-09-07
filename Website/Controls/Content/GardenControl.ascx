<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GardenControl.ascx.cs" Inherits="Controls_Content_GardenControl" %>

<div>
    <table width="100%" cellspacing="0" cellpadding="0">
        <tr>
            <td colspan="2" align="left" valign="top" style="width: 100%">
                <asp:HyperLink ID="hlBook" runat="server" CssClass="ContentLink" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParDays" runat="server" CssClass="ContentParagraph" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="left">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>
                            <asp:Label ID="lblTxtDays" runat="server" CssClass="ContentText" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParFavorably" runat="server" CssClass="ContentParagraph" />
            </td>
            <td align="left" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParAdversely" runat="server" CssClass="ContentParagraph" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>
                            <asp:Label ID="lblTxtFavorably" runat="server" CssClass="ContentText" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" valign="top">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>
                            <asp:Label ID="lblTxtAdversely" runat="server" CssClass="ContentText" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
