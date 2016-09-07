<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LocationControl.ascx.cs" Inherits="Controls_Location_LocationControl" %>

<center>
<asp:Panel ID="pLocation" runat="server" DefaultButton="btnGoToAddress">
<table cellpadding="3" cellspacing="0">
    <tr>
        <td colspan="2" align="left">
            <b>Часовой пояс</b><br />
            <asp:DropDownList ID="ddlTimeZone" runat="server" Width="100%" /><br />
            <asp:CheckBox ID="cbDST" runat="server" Checked="true" Text="Определять летнее/зимнее время автоматически" />
        </td>
    </tr>
    <tr>
        <td align="left">
            <b>Введите адрес (например: </b><a href="#" onclick="showAddress(this.innerHTML); tbAddress.value=this.innerHTML;">Хмельницкий Курчатова 1</a><b>)</b><br />
            <asp:TextBox ID="tbAddress" runat="server" Width="390px" MaxLength="30" />
        </td>
        <td align="right" valign="bottom">
            <asp:Button ID="btnGoToAddress" runat="server" Text="Перейти" OnClientClick="showAddress(tbAddress.value); return false;" />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="left">
            <div id="addressResult" style="width: 98%; border: solid 1px black; padding: 3px; display: none; overflow-y: scroll; overflow-x: hidden; height: 50px;"></div>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="left">
            <div id="map" style="width: 480px; height: 330px"></div>
            <asp:HiddenField ID="hfLat" runat="server" />
            <asp:HiddenField ID="hfLng" runat="server" />
            <asp:HiddenField ID="hfMapType" runat="server" />
            <asp:HiddenField ID="hfMapZoom" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
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
</asp:Panel>
</center>

<script src="http://maps.google.com/maps?file=api&v=2&hl=ru&key=<%=GoogleAPIKey%>" type="text/javascript">
</script>

<script type="text/javascript">
    var tbAddress = document.getElementById('<%=tbAddress.ClientID%>');
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
        var pLocation = '<%=pLocation.ClientID%>';
        parent.Dialog.params.width = Dimension.GetElementWidth(pLocation);
        parent.Dialog.params.height = Dimension.GetElementHeight(pLocation) + 10;
        parent.Dialog.Resize();
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
    
    function OnOK()
    {
        if (tbAddress.value == '')
        {
            alert('Введите адрес');
            tbAddress.focus();
            return false;
        }
        else
        {
            parent.Dialog.HideContent();
            return true;
        }
    }
    
    initGMap();
    parent.Dialog.params.focusControlID = "<%=tbAddress.ClientID%>";
</script>
