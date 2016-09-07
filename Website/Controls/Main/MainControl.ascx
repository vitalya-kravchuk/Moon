<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MainControl.ascx.cs" Inherits="Controls_Main_MainControl" %>

<script type="text/javascript">
    var ClientID = "<%=this.ClientID%>";
    var SitePath = "<%=SitePath%>";
    var ImagesPath = "<%=ImagesPath%>";
 
    var bpcTimeID = "<%=bpcTime.ClientID%>";
    var tbTimeID = "<%=tbTime.ClientID%>";
    var bpcDateID = "<%=bpcDate.ClientID%>";
    var tbDateID = "<%=tbDate.ClientID%>";
    
    var imgMoonID = "<%=imgMoon.ClientID%>";
    var imgZodiacID = "<%=imgZodiac.ClientID%>";
    var imgSymbolID = "<%=imgSymbol.ClientID%>";
    
    var hfChapterNameID = "<%=hfChapterName.ClientID%>";
    var tbContentNoteID = "<%=tbContentNote.ClientID%>";

    function UpdateLocation()
    {
        document.getElementById("<%=btnUpdateLocation.ClientID%>").click();
    }
    function UpdateDateTime()
    {
        document.getElementById("<%=btnUpdateDateTime.ClientID%>").click();
    }
    function UpdateContent(chapterName)
    {
        document.getElementById("<%=hfChapterName.ClientID%>").value = chapterName;
        document.getElementById("<%=btnUpdateContent.ClientID%>").click();
    }
    function Recalc(dateTime)
    {
        document.getElementById("<%=hfRecalcDateTime.ClientID%>").value = dateTime;
        document.getElementById("<%=btnRecalc.ClientID%>").click();
    }
    function Now()
    {
        document.getElementById("<%=btnNow.ClientID%>").click();
    }
    function Logout()
    {
        document.getElementById("<%=btnLogout.ClientID%>").click();
    }
</script>

<asp:Panel ID="pMain" runat="server" DefaultButton="btnUpdateDateTime">

<asp:HiddenField ID="hfRecalcDateTime" runat="server" />
<asp:Button ID="btnRecalc" runat="server" Text="Recalc" style="display:none;" OnClick="btnRecalc_Click" />
<asp:Button ID="btnNow" runat="server" Text="Now" style="display:none;" OnClick="btnNow_Click" />
<asp:Button ID="btnLogout" runat="server" Text="Logout" style="display:none;" OnClick="btnLogout_Click" />

<%-- Звездное небо --%>
<div id="divStarSky" style="left: 0px; top: 0px; z-index: 0; position: absolute" onmousemove="Event.ShowMenu(false);">
<table id="tblStarSky" cellpadding="0" cellspacing="0">
<tr>
	<td align="left" valign="top"><img src="<%=ImagesPath%>StarSky/top_left.jpg" alt="" /></td>
	<%-- Луна --%>
	<td align="center" valign="bottom" class="StarSkyTop"><asp:Image ID="imgMoon" runat="server" /></td>
	<%-- Зодиак --%>
	<td align="right" valign="bottom"><asp:Image ID="imgZodiac" runat="server" /></td>
</tr>
<tr>
    <td id="tdStarSkyMiddle" colspan="3" class="StarSkyMiddle" style="width: 100%;"></td>
</tr>
<tr>
	<%-- Символ --%>
	<td align="left" valign="bottom" style="height: 370px" class="StarSkyBottom"><asp:Image ID="imgSymbol" runat="server" /></td>
	<td class="StarSkyBottom" style="width: 100%;"></td>
	<td align="right" valign="bottom"><img src="<%=ImagesPath%>StarSky/bottom_right.jpg" alt="" /></td>
</tr>
</table>
</div>

<%-- Комета --%>
<div id="divComet" style="display: none; z-index: 0; position: absolute;">
	<img src="<%=ImagesPath%>StarSky/comet.gif" alt="" />
</div>

<%-- Сияющая звезда --%>
<div id="divStarLight" style="display: none; z-index: 0; position: absolute;">
	<img src="<%=ImagesPath%>StarSky/star_light.gif" alt="" />
</div>

<%-- Желтая звезда --%>
<div id="divStarYellow" style="display: none; left: 1090px; top: 50px; z-index: 0; position: absolute;">
	<img src="<%=ImagesPath%>StarSky/star_yellow.jpg" alt="" />
</div>

<%-- Гора --%>
<div id="divMount" style="display: none; top: 177px; z-index: 51; position: absolute;">
    <img src="<%=ImagesPath%>StarSky/mount.jpg" alt="" />
</div>

<%-- Панель --%>
<div id="divPanel" class="Panel" style="left: 0px; top: 0px; height: 38px; z-index: 100; position: absolute;">
<table id="tblPanel" width="100%" cellpadding="0" cellspacing="0" style="height: 38px;">
<tr>
	<td style="width: 10px; white-space: nowrap; cursor: default;">&nbsp;&nbsp;</td>
	<td style="width: 225px;" align="left" valign="middle"><img id="imgPanelMLC" src="<%=ImagesPath%>Panel/mlc.png" alt="" onmouseover="Animation.mlcStop = true; Event.SetMenuBtnActive(true);" onmouseout="Event.SetMenuBtnActive(false);" /></td>
	<td style="width: 10px; white-space: nowrap; cursor: default;">&nbsp;&nbsp;</td>
	<td style="white-space: nowrap; width: 90%;" align="center" valign="middle">
	    <table cellpadding="0" cellspacing="0">
	    <tr>
	        <td align="left" valign="middle" onmouseover="tooltip(this, 'Изменить местоположение');" onmouseout="tooltipHide(this);">
	            <%-- Местоположение --%>
	            <mc:ButtonPanelControl ID="bpcLocation" runat="server" OnClientClick="ShowDialog.Location();" />
	        </td>
	        <td align="left" valign="middle" onmouseover="tooltip(this, 'Сейчас');" onmouseout="tooltipHide(this);">
	            <%-- Сейчас --%>
	            <mc:ButtonPanelControl ID="bpcNow" runat="server" OnClientClick="Event.ShowLockOverlay(true); Now();" />
	        </td>
	        <td align="left" valign="middle" onmouseover="tooltip(this, 'Изменить время');" onmouseout="tooltipHide(this);">
	            <%-- Время --%>
	            <mc:ButtonPanelControl ID="bpcTime" runat="server" OnClientClick="Event.ShowTime(true);" />
                <asp:TextBox ID="tbTime" runat="server" Width="45px" MaxLength="5" style="text-align:center; display:none;" onBlur="Event.ShowTime(false);" />
	        </td>
	        <td align="left" valign="middle" onmouseover="tooltip(this, 'Изменить дату');" onmouseout="tooltipHide(this);">
	            <%-- Дата --%>
                <mc:ButtonPanelControl ID="bpcDate" runat="server" OnClientClick="Event.ShowDate(true);" />
                <asp:TextBox ID="tbDate" runat="server" Width="75px" MaxLength="10" style="text-align:center; display:none;" />
                <asp:CalendarExtender ID="CalendarEx" runat="server" TargetControlID="tbDate"
                    TodaysDateFormat="dd MMMM yyyy"
                    OnClientHidden="function onClientHidden(e) { Event.ShowDate(false); }"
                    OnClientDateSelectionChanged="function onChangeDate(e) { UpdateDateTime(); }" />
	        </td>
	        <td align="left" valign="middle" onmouseover="tooltip(this, 'Выход');" onmouseout="tooltipHide(this);">
	            <%-- Выход --%>
                <mc:ButtonPanelControl ID="bpcLogout" runat="server" OnClientClick="Event.ShowLockOverlay(true); Logout();" />
	        </td>
	        <td align="left" valign="middle" onmouseover="tooltip(this, 'Мои данные');" onmouseout="tooltipHide(this);">
	            <%-- Мои данные --%>
                <mc:ButtonPanelControl ID="bpcMyInfo" runat="server" OnClientClick="ShowDialog.Login();" />
	        </td>
	    </tr>
	    </table>
	    <%-- Обновить --%>
	    <asp:Button ID="btnUpdateLocation" runat="server" Text="UpdateLocation" style="display:none;" OnClick="btnUpdateLocation_Click" />
	    <asp:Button ID="btnUpdateDateTime" runat="server" Text="UpdateDateTime" style="display:none;" OnClick="btnUpdateDateTime_Click" />
	</td>
	<td style="width: 10px; white-space: nowrap; cursor: default;">&nbsp;&nbsp;</td>
	<td style="white-space: nowrap;" align="right" valign="middle">
	    <asp:Label ID="lPanelSlogan" runat="server" CssClass="PanelSloganLabel"><%=Res.GetSlogan()%></asp:Label>
	</td>
	<td style="width: 10px; white-space: nowrap; cursor: default;">&nbsp;&nbsp;</td>
	<%--
	<td style="width: 81px" align="right" valign="middle">
	    <a href="http://v-zakladki.ru/"> <script src="http://odnaknopka.ru/ok1.js" type="text/javascript"></script> </a>
	</td>
	--%>
	<td style="width: 10px; white-space: nowrap; cursor: default;">&nbsp;&nbsp;</td>
</tr>
</table>
</div>

<%-- Меню --%>
<div id="divMenu" style="left: 10px; top: 38px; display: none; z-index: 100; position: absolute;">
    <asp:PlaceHolder ID="phMenu" runat="server" />
</div>

<%-- Просмотр текущего ритма --%>
<div id="divRhythmInfo">
    <div id="divRhythmInfoBackground" class="OpacityRhythmInfoBackground" style="left: 0px; top: 60px; width: 285px; background-color: #ffffff; z-index: 75; position: absolute;">
    </div>
    <div id="divRhythmInfoElements" style="left: -2px; top: 60px; z-index: 76; position: absolute;">
        <table width="100%">
            <tr>
	            <td colspan="3" style="height: 10px"></td>
	        </tr>
	        <tr>
	            <td style="width: 10px"></td>
	            <td align="left" valign="top">
	                <table runat="server">
	                    <tr>
                            <td align="left">
                                <asp:Label ID="lblLunarDayTime" runat="server" CssClass="RhythmInfoTime" /><br />
                                <asp:Label ID="lblLunarDay" runat="server" CssClass="RhythmInfoText" 
                                    onmouseover="tooltip(this, 'Выбрать лунные сутки');" onmouseout="tooltipHide(this);"
                                    onclick="ShowDialog.RhythmsView('LunarDay');" /><br />
                                <asp:Label ID="lblIndividualLunarDay" runat="server" CssClass="RhythmInfoText" style="border-top-width: 0px;" 
                                    onclick="ShowDialog.RhythmsView('IndividualLunarDay');" />
                            </td>
                        </tr>
                        <tr><td class="RhythmInfoSep"></td></tr>
                        
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblZodiacTime" runat="server" CssClass="RhythmInfoTime" /><br />
                                <asp:Label ID="lblZodiac" runat="server" CssClass="RhythmInfoText" 
                                    onmouseover="tooltip(this, 'Выбрать Зодиак');" onmouseout="tooltipHide(this);" 
                                    onclick="ShowDialog.RhythmsView('Zodiac');" /><br />
                            </td>
                        </tr>
                        <tr><td class="RhythmInfoSep"></td></tr>
                        
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblPhaseTime" runat="server" CssClass="RhythmInfoTime" /><br />
                                <asp:Label ID="lblPhase" runat="server" CssClass="RhythmInfoText" 
                                    onmouseover="tooltip(this, 'Выбрать фазу Луны');" onmouseout="tooltipHide(this);" 
                                    onclick="ShowDialog.RhythmsView('Phase');" />
                            </td>
                        </tr>
                        <tr><td class="RhythmInfoSep"></td></tr>
                        
                        <tr id="trRhythmInfoEclipse" runat="server">
                            <td align="left">
                                <asp:Label ID="lblEclipseTime" runat="server" CssClass="RhythmInfoTime" /><br />
                                <asp:Label ID="lblEclipse" runat="server" CssClass="RhythmInfoText" 
                                    onmouseover="tooltip(this, 'Выбрать затмение');" onmouseout="tooltipHide(this);" 
                                    onclick="ShowDialog.RhythmsView('Eclipse');" />
                            </td>
                        </tr>
                        <tr id="trRhythmInfoEclipseSep" runat="server"><td class="RhythmInfoSep"></td></tr>
                        
                        <tr id="trRhythmInfoNewYear" runat="server">
                            <td align="left">
                                <asp:Label ID="lblNewYearTime" runat="server" CssClass="RhythmInfoTime" /><br />
                                <asp:Label ID="lblNewYear" runat="server" CssClass="RhythmInfoText" 
                                    onmouseover="tooltip(this, 'Выбрать Новый Лунный Год');" onmouseout="tooltipHide(this);" 
                                    onclick="ShowDialog.RhythmsView('NewYear');" />
                            </td>
                        </tr>
                        <tr id="trRhythmInfoNewYearSep" runat="server"><td class="RhythmInfoSep"></td></tr>
	                </table>
	            </td>
	            <td style="width: 10px"></td>
	        </tr>
	        <tr>
	            <td colspan="3" style="height: 10px"></td>
            </tr>
        </table>
    </div>
</div>

<%-- Подложка --%>
<div id="divSubstrate" class="OpacitySubstrate" style="z-index: 49; position: absolute">
    <img id="imgSubstrate" alt="" src="" />
</div>

<%-- Быстрый переход по дням --%>
<div id="divQuickDaysGo" style="z-index: 61; position: absolute;">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:LinkButton ID="lbQuickDaysGoLeft" runat="server" 
                    OnClick="lbQuickDaysGoLeft_Click"
                    OnClientClick="Event.ShowLockOverlay(true)">
                    <img src="<%=ImagesPath%>Content/arrow_left.jpg" alt="" style="border: 0px"
                        onmouseover="this.src = '<%=ImagesPath%>Content/arrow_left_on.jpg'"
                        onmouseout="this.src = '<%=ImagesPath%>Content/arrow_left.jpg'" />
                </asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="lbQuickDaysGoRight" runat="server" 
                    OnClick="lbQuickDaysGoRight_Click"
                    OnClientClick="Event.ShowLockOverlay(true)">
                    <img src="<%=ImagesPath%>Content/arrow_right.jpg" alt="" style="border: 0px"
                        onmouseover="this.src = '<%=ImagesPath%>Content/arrow_right_on.jpg'"
                        onmouseout="this.src = '<%=ImagesPath%>Content/arrow_right.jpg'" />
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>

<%-- Разделы --%>
<div id="divChapters" style="z-index: 60; position: absolute;">
    <asp:HiddenField ID="hfChapterName" runat="server" />
    <asp:Button ID="btnUpdateContent" runat="server" Text="UpdateContent" style="display:none;" OnClick="btnUpdateContent_Click" />    
    <table width="100%" cellpadding="0">
        <tr>
            <td align="left" style="width: 100%">
                <img src="<%=ImagesPath%>Chapter/commonon.jpg" id="imgChapterOn" alt="" />
            </td>
            <td align="right" style="white-space: nowrap">
                <div id="divChaptersMenu">
                <img src="<%=ImagesPath%>Chapter/common.jpg" id="imgChapterCommon" alt=""
                    style="cursor: pointer; display: none;"
                    onmouseover="Event.SetChapterActive(this, true); tooltip(this, 'Общее');" 
                    onmouseout="Event.SetChapterActive(this, false); tooltipHide(this);"
                    onclick="Event.ChapterClick('Common');" />
                    
                <img src="<%=ImagesPath%>Chapter/health.jpg" id="imgChapterHealth" alt=""
                    style="cursor: pointer;"
                    onmouseover="Event.SetChapterActive(this, true); tooltip(this, 'Здоровье');" 
                    onmouseout="Event.SetChapterActive(this, false); tooltipHide(this);"
                    onclick="Event.ChapterClick('Health');" />
            
                <img src="<%=ImagesPath%>Chapter/money.jpg" id="imgChapterMoney" alt=""
                    style="cursor: pointer;"
                    onmouseover="Event.SetChapterActive(this, true); tooltip(this, 'Деньги');" 
                    onmouseout="Event.SetChapterActive(this, false); tooltipHide(this);"
                    onclick="Event.ChapterClick('Money');" />
                
                <img src="<%=ImagesPath%>Chapter/beauty.jpg" id="imgChapterBeauty" alt=""
                    style="cursor: pointer;"
                    onmouseover="Event.SetChapterActive(this, true); tooltip(this, 'Красота');" 
                    onmouseout="Event.SetChapterActive(this, false); tooltipHide(this);"
                    onclick="Event.ChapterClick('Beauty');" />
                    
                <img src="<%=ImagesPath%>Chapter/garden.jpg" id="imgChapterGarden" alt=""
                    style="cursor: pointer;"
                    onmouseover="Event.SetChapterActive(this, true); tooltip(this, 'Сад');" 
                    onmouseout="Event.SetChapterActive(this, false); tooltipHide(this);"
                    onclick="Event.ChapterClick('Garden');" />
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        Event.ChapterChange(document.getElementById(hfChapterNameID).value);
    </script>
</div>

<div id="divContentLoad" style="display: none; position: absolute; z-index: 51; background-color: White; border: solid 1px black; padding: 10px">
    <img src="<%=ImagesPath%>loading.gif" alt="" />
</div>

<%-- Контент --%>
<div id="divContent" class="OpacityContent" style="background-color: #d9d9e1; z-index: 50; position: absolute">
	<div id="divBorderTop" class="ContentBorderTop"><div class="ContentBorderBottom">
		<div id="divContentInner" style="height: 520px">
			<table>
				<tr>
					<td colspan="3">&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/>&nbsp;<br/></td>
				</tr>
				<tr>
					<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
					<td>
						<div id="divIContent" style="height: 520px; width: 300px; overflow-x: hidden; overflow-y: scroll; border: 0px; padding: 5px;">
						    <%-- Мысль --%>
						    <table width="100%">
						        <tr>
							        <td align="right" style="width: 100%">
								        <asp:Label ID="lContentTextMind" runat="server" CssClass="ContentTextMind" />
							        </td>
							        <td style="white-space: nowrap">
							            &nbsp;&nbsp;&nbsp;
							        </td>
						        </tr>
						    </table>
						    
                            <asp:UpdatePanel ID="upContent" runat="server" UpdateMode="Conditional" EnableViewState="false">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnUpdateContent" />
                            </Triggers>
                            <ContentTemplate>
                            <table width="100%">
                                <%-- Заметки --%>
						        <tr>
							        <td align="left">
								        <table id="tblTitleNote" cellpadding="0" cellspacing="0">
								            <tr>
								                <td><img src="<%=ImagesPath%>Content/title_note.jpg" alt="" /></td>
									            <td class="ContentTitleRibbonR">
                                                    <asp:Label ID="lContentTitleNote" runat="server" CssClass="ContentTitle" Text="Заметки" />
                                                </td>
									            <td class="ContentTitleRibbonR">&nbsp;&nbsp;&nbsp;</td>
                                                <td class="ContentTitleRibbonEnd">&nbsp;&nbsp;&nbsp;</td>
								            </tr>
								        </table>
							        </td>
						        </tr>
						        <tr>
							        <td align="left">
							            <table>
							                <tr>
							                    <td align="left">
							                        <div id="divContentNoteEdit">
							                            <asp:LinkButton ID="lbContentNoteShow" runat="server" CssClass="ContentLink"
                                                            OnClientClick="Event.ShowNote(true); return false;"
                                                            Text="Нажмите сюда для записи своих наблюдений ритмов жизни" />
                                                        <asp:Label ID="lblContentNoteEdit" runat="server" CssClass="ContentEdit"
                                                            style="cursor: pointer;" onclick="Event.ShowNote(true);" />
							                        </div>
								                    <div id="divContentNote" style="display: none;">
                                                        <asp:TextBox ID="tbContentNote" runat="server" TextMode="MultiLine" CssClass="ContentNote" Height="150px" />
									                    <br/>
									                    <asp:LinkButton ID="lbContentNoteSave" runat="server" CssClass="ContentLink"
                                                            OnClientClick="return Event.ShowNote(false);"
                                                            OnClick="lbContentNoteSave_Click"
                                                            Text="Сохранить заметки" />
									                    <asp:Label ID="lContentNoteSep" runat="server" CssClass="ContentText">&nbsp;|&nbsp;</asp:Label>
									                    <asp:LinkButton ID="lbContentNoteCancel" runat="server" CssClass="ContentLink"
                                                            OnClientClick="Event.ShowNote(false);"
                                                            OnClick="lbContentNoteCancel_Click"
                                                            Text="Отмена" />
                                                        <asp:HiddenField ID="hfNoteID" runat="server" />
								                    </div>
							                    </td>
							                </tr>
							            </table>
							        </td>
						        </tr>
						        <tr>
							        <td>&nbsp;</td>
						        </tr>
                                <%-- Описание --%>
                                <tr>
                                    <td align="left">
                                        <table id="tblTitleDesc" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td><img src="<%=ImagesPath%>Content/title_desc.jpg" alt="" /></td>
                                                <td class="ContentTitleRibbonR">
                                                    <asp:Label ID="lContentTitleDesc" runat="server" CssClass="ContentTitle" />
                                                </td>
                                                <td class="ContentTitleRibbonR">&nbsp;&nbsp;&nbsp;</td>
                                                <td class="ContentTitleRibbonEnd">&nbsp;&nbsp;&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
						            <td align="left">
                                        <asp:PlaceHolder ID="phContent" runat="server" />
						            </td>
						        </tr>
                            </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
						</div>
					</td>
					<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
				</tr>
			</table>
		</div>
	</div></div>
</div>

<%-- Диалог --%>
<mc:DialogControl ID="cDialog" runat="server" />

<%-- Баллон --%>
<mc:BallonControl ID="cBallon" runat="server" />

<%-- Анимация кнопки меню. Кеш картинок --%>
<asp:Literal ID="litAnimationCacheMLC" runat="server" EnableViewState="false" />

<div id="divLockOverlay" class="OpacityLockOverlay"
    style=" display: none; position: absolute; z-index: 10000; 
            left: 0px; top: 0px; width: 100%; height: 100%; 
            background-color: Black">
</div>

</asp:Panel>
