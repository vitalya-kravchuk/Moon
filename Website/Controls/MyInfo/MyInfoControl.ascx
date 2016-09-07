<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyInfoControl.ascx.cs" Inherits="Controls_MyInfo_MyInfoControl" %>

<center>
<asp:Panel ID="pMyInfo" runat="server" DefaultButton="ibOK">
<table cellspacing="3" cellpadding="3">
    <tr>
        <td align="left" valign="top">
            <table id="tblMyInfoEdit">
                <tr>
                    <td align="left">
                        <div id="divMsgError" class="MessageError" style="display: none;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td align="left">
                                    <b>Имя</b>
                                </td>
                                <td align="left">
                                    <b>Email</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="tbName" runat="server" Width="192px" MaxLength="30" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="tbEmail" runat="server" Width="192px" MaxLength="30" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td align="left">
                                    <b>Пароль</b>
                                </td>
                                <td align="left">
                                    <b>Подтверждение пароля</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="tbPassword" runat="server" Width="192px" TextMode="Password" MaxLength="30" />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="tbPasswordConfirm" runat="server" Width="192px" TextMode="Password" MaxLength="30" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td align="left">
                                    <b>День рождения</b>
                                </td>
                                <td align="left">
                                    <b>Время рождения</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:UpdatePanel ID="upDate" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 198px;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlDay" runat="server" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:DropDownList ID="ddlMonth" runat="server" AutoPostBack="true" 
                                                            onselectedindexchanged="ddlMonth_SelectedIndexChanged" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" 
                                                            onselectedindexchanged="ddlMonth_SelectedIndexChanged" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlHour" runat="server" />
                                    &nbsp;:&nbsp;
                                    <asp:DropDownList ID="ddlMinute" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Panel ID="pBirthPlace" runat="server" DefaultButton="btnGoToAddress">
                        <table>
                            <tr>
                                <td colspan="2" align="left">
                                    <b>Место рождения (например: </b><a href="#" onclick="showAddress(this.innerHTML); tbBirthPlace.value=this.innerHTML;">Хмельницкий Курчатова 1</a><b>)</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:TextBox ID="tbBirthPlace" runat="server" Width="290px" MaxLength="30" />
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnGoToAddress" runat="server" Text="Перейти" Width="100px" OnClientClick="showAddress(tbBirthPlace.value); return false;" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="addressResult" style="width: 98%; border: solid 1px black; padding: 3px; display: none; overflow-y: scroll; overflow-x: hidden; height: 50px;"></div>
                                </td>
                            </tr>
                        </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table>
                            <tr>
                                <td align="left">
                                    <b>Часовой пояс</b><br />
                                    <asp:DropDownList ID="ddlTimeZone" runat="server" Width="400px" /><br />
                                    <asp:CheckBox ID="cbDST" runat="server" Checked="true" Text="Определять летнее/зимнее время автоматически" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="bottom">
                        <p />
                        <div class="DialogBottomLine" style="width: 100%;"></div>
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:ImageButton ID="ibOK" runat="server" SkinID="OK"
                                        OnClientClick="return OnOK();"
                                        OnClick="ibOK_Click" />
                                </td>
                                <td align="left">
                                    <asp:ImageButton ID="ibCancel" runat="server" SkinID="Cancel"
                                        OnClientClick="parent.Dialog.Hide(); return false;" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        
        <td align="left" valign="top">
            <table>
                <tr>
                    <td>
                        <div id="map" style="width: 510px; height: 300px"></div>
                        <asp:HiddenField ID="hfLat" runat="server" />
                        <asp:HiddenField ID="hfLng" runat="server" />
                        <asp:HiddenField ID="hfMapType" runat="server" />
                        <asp:HiddenField ID="hfMapZoom" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
</center>

<script src="http://maps.google.com/maps?file=api&v=2&hl=ru&key=<%=GoogleAPIKey%>" type="text/javascript">
</script>

<script type="text/javascript">
    var tbBirthPlace = document.getElementById('<%=tbBirthPlace.ClientID%>');
    var hfLat = document.getElementById('<%=hfLat.ClientID%>');
    var hfLng = document.getElementById('<%=hfLng.ClientID%>');
    var hfMapType = document.getElementById('<%=hfMapType.ClientID%>');
    var hfMapZoom = document.getElementById('<%=hfMapZoom.ClientID%>');
    
    var map = null;
    var marker = null;
    
    function setPoint(point)
    {
        hfLat.value = point.lat();
        hfLng.value = point.lng();
    }
    function getPoint()
    {
        return new GLatLng(hfLat.value, hfLng.value);
    }
    
    function setAddressResult(address)
    {
        var addressResult = document.getElementById("addressResult");
        if (address == null || address == '')
        {
            addressResult.innerHTML = '';
            addressResult.style.display = 'none';
        }
        else
        {
            addressResult.innerHTML = address;
            addressResult.style.display = '';
        }
        Resize();
    }
    
    function showAddress(search)
    {
        if (search == '') return;
        var geo = new GClientGeocoder();
        setAddressResult("Поиск...");
        geo.getLocations(search, function(result)
        {
            if (result.Status.code == G_GEO_SUCCESS)
            {
                var address = "";
                for (var i = 0; i < result.Placemark.length; i++)
                {
                    var lat = result.Placemark[i].Point.coordinates[1];
                    var lng = result.Placemark[i].Point.coordinates[0];
                    var lnkStyle = "";
                    if (i > 0) lnkStyle = "style='font-weight: bold;'";
                    address += 
                        "<small>" + (i+1) + "</small>. " +
                        "<a href='javascript:void(0);' onclick='goLocation(" + lat + ", " + lng + "); this.style.fontWeight = \"normal\"; return false;' " + lnkStyle + ">" + 
                        result.Placemark[i].address + "</a>" + 
                        "<br/>";
                }
                if (result.Placemark.length > 0)
                {
                    var lat = result.Placemark[0].Point.coordinates[1];
                    var lng = result.Placemark[0].Point.coordinates[0];
                    goLocation(lat, lng);
                }
                setAddressResult(address);
            }
            else
            {
                setAddressResult(null);
                alert(search + " не найдено");
            }
        });
    }
    
    function goLocation(lat, lng)
    {
        var point = new GLatLng(lat, lng);
        map.panTo(point);
        marker.setPoint(point);
        setPoint(point);
    }
    
    function getMapType()
    {
        var type = 1;
        switch (map.getCurrentMapType())
        {
            case G_NORMAL_MAP:
                type = 0;
                break; 
            case G_SATELLITE_MAP:
                type = 1;
                break; 
            case G_PHYSICAL_MAP:
                type = 2;
                break; 
        }
        return type;
    }
    
    function setMapType(type)
    {
        switch (type)
        {
            case 0:
                map.setMapType(G_NORMAL_MAP);
                break;
            case 1:
                map.setMapType(G_SATELLITE_MAP);
                break;
            case 2:
                map.setMapType(G_PHYSICAL_MAP);
                break;
            default:
                map.setMapType(G_NORMAL_MAP);
                break;
        }
    }

    function initGMap()
    {
        map = new GMap2(document.getElementById("map"));
        map.setUIToDefault();
        setMapType(parseInt(hfMapType.value));
        map.setCenter(getPoint(), parseInt(hfMapZoom.value));
        
        GEvent.addListener(map, "zoomend", function()
        { 
            hfMapZoom.value = map.getZoom();
        });
        
        GEvent.addListener(map, "click", function(overlay, point)
        {
            marker.setPoint(point);
            setPoint(point);
        });
        
        GEvent.addListener(map, "maptypechanged", function()
        {
            hfMapType.value = getMapType();
        }); 
        
        marker = new GMarker(getPoint(), {draggable: true});
        GEvent.addListener(marker, "dragend", function()
        {
            var point = marker.getPoint();
            map.panTo(point);
            setPoint(point);
        });
        map.addOverlay(marker);
    }
    
    function resizeMap()
    {
        var mieHeight = Dimension.GetElementHeight('tblMyInfoEdit');
        document.getElementById('map').style.height = (mieHeight - 6) + 'px';
    }
    
    function Resize()
    {
        resizeMap();
        var pMyInfo = '<%=pMyInfo.ClientID%>';
        parent.Dialog.params.width = Dimension.GetElementWidth(pMyInfo);
        parent.Dialog.params.height = Dimension.GetElementHeight(pMyInfo);
        parent.Dialog.Resize();
    }
    
    function Validate()
    {
        var divMsgError = document.getElementById('divMsgError');
        divMsgError.innerHTML = '';
    
        fields = new Array();
        fields.push('<%=tbName.ClientID%>');
        fields.push('<%=tbEmail.ClientID%>');
        fields.push('<%=tbPassword.ClientID%>');
        fields.push('<%=tbPasswordConfirm.ClientID%>');
        fields.push('<%=ddlDay.ClientID%>');
		fields.push('<%=ddlMonth.ClientID%>');
		fields.push('<%=ddlYear.ClientID%>');
		if (Validation.RequiredFields(fields))
		{
		    divMsgError.innerHTML = 'Введите обязательные поля <br/>';
		}
		else if (!Validation.EmailValidator('<%=tbEmail.ClientID%>'))
		{
		    divMsgError.innerHTML = 'Email введен с ошибками <br/>';
		}
		else if (!Validation.CompareValidator('<%=tbPassword.ClientID%>', '<%=tbPasswordConfirm.ClientID%>'))
		{
		    divMsgError.innerHTML = 'Пароли не совпадают <br/>';
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
		Resize();
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
    
    resizeMap();
    initGMap();
    parent.Dialog.params.focusControlID = "<%=tbName.ClientID%>";
</script>
