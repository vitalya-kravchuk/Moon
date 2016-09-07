<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommonControl.ascx.cs" Inherits="Controls_Content_CommonControl" %>

<div>
    <table width="100%" cellspacing="3" cellpadding="3">
        <tr>
            <td colspan="2" align="left" valign="top" style="width: 100%">
                <asp:HyperLink ID="hlBook" runat="server" CssClass="ContentLink" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" valign="middle" style="width: 50%; padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParDay" runat="server" CssClass="ContentParagraph" />
            </td>
            <td align="left" valign="middle" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParDayInd" runat="server" CssClass="ContentParagraph" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:Label ID="lblTxtDay" runat="server" CssClass="ContentText" />
            </td>
            <td align="left" valign="top">
                <asp:LinkButton ID="lbLogin" runat="server" CssClass="ContentLink" />
                <asp:Label ID="lblTxtDayInd" runat="server" CssClass="ContentText" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td align="left" valign="middle" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParDreams" runat="server" CssClass="ContentParagraph" />
            </td>
            <td align="left" valign="middle" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParPhase" runat="server" CssClass="ContentParagraph" />
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:Label ID="lblTxtDreams" runat="server" CssClass="ContentText" />
            </td>
            <td align="left" valign="top">
                <asp:Label ID="lblTxtPhase" runat="server" CssClass="ContentText" />
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="left" valign="middle" style="padding-left: 15px" class="ContentLine">
                <asp:Label ID="lblParZodiac" runat="server" CssClass="ContentParagraph" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="justify" valign="top">
                <asp:Label ID="lblTxtZodiac" runat="server" CssClass="ContentText" />
            </td>
        </tr>
    </table>
</div>
