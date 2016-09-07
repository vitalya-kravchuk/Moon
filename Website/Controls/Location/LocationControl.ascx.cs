using System;
using System.Data;

public partial class Controls_Location_LocationControl : BaseControl
{
    #region Properties
    protected double Latitude
    {
        get
        {
            return Helper.StringToDouble(hfLat.Value);
        }
        set
        {
            hfLat.Value = Helper.DoubleToString(value);
        }
    }
    protected double Longitude
    {
        get
        {
            return Helper.StringToDouble(hfLng.Value);
        }
        set
        {
            hfLng.Value = Helper.DoubleToString(value);
        }
    }
    protected int MapType
    {
        get
        {
            return Convert.ToInt32(hfMapType.Value);
        }
        set
        {
            hfMapType.Value = value.ToString();
        }
    }
    protected int MapZoom
    {
        get
        {
            return Convert.ToInt32(hfMapZoom.Value);
        }
        set
        {
            hfMapZoom.Value = value.ToString();
        }
    }
    #endregion

    void BindTimeZones()
    {
        DataSet ds = new DataSet();
        ds.ReadXml(AppDataPathPhysical + "TimeZones.xml");
        DataView dv = ds.Tables["TimeZone"].DefaultView;
        ddlTimeZone.DataTextField = "Text";
        ddlTimeZone.DataValueField = "Value";
        ddlTimeZone.DataSource = dv;
        ddlTimeZone.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindTimeZones();

            ddlTimeZone.SelectedValue = PersonLocation.TimeZone.ToString();
            cbDST.Checked = PersonLocation.DST;
            tbAddress.Text = PersonLocation.PlaceName;
            Latitude = PersonLocation.Latitude;
            Longitude = PersonLocation.Longitude;
            MapType = PersonLocation.MapType;
            MapZoom = PersonLocation.MapZoom;
        }
    }

    protected void ibOK_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        Location location = new Location()
        {
            Latitude = Latitude,
            Longitude = Longitude,
            PlaceName = tbAddress.Text.Trim(),
            DST = cbDST.Checked,
            TimeZone = Helper.StringToDouble(ddlTimeZone.SelectedValue),
            MapType = MapType,
            MapZoom = MapZoom
        };
        PersonLocation = location;

        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
            "parent.UpdateLocation(); parent.Dialog.Hide();", true);
    }
}
