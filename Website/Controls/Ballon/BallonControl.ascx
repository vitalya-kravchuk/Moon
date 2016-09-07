<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BallonControl.ascx.cs" Inherits="Controls_Ballon_BallonControl" %>

<div id="divBallonArrow" style="position: absolute; z-index: 801; display: none">
    <img src="<%=ImagesPath %>Ballon/arrow.gif" alt="" />
</div>

<div id="divBallon" style="position: absolute; z-index: 800; padding: 5px; border: solid 1px black; background-color: White; display: none">
    <table>
        <tr>
            <td align="left" style="white-space: nowrap">
                <span id="spanBallon" style="cursor: default">
                </span>
            </td>
        </tr>
    </table>
</div>
