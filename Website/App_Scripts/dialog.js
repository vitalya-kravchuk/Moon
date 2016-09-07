var Dialog =
{
    // init
    LoadingImageURI: '',
    params: null,

    // const
    headerHeight: 30,
    margin: 10,
    
    // properties
    GetDivDlgOverlay: function() 
    {
        return document.getElementById('divDlgOverlay');
    },
    GetDivDlg: function() 
    {
        return document.getElementById('divDlg');
    },
    GetSpanDlgTitle: function() 
    {
        return document.getElementById('spanDlgTitle');
    },
    GetTdDlgContent: function() 
    {
        return document.getElementById('tdDlgContent');
    },
    GetIFrameDlg: function() 
    {
        return document.getElementById('iframeDlgContent');
    },
    GetDivDlgLoading: function()
    {
        return document.getElementById('divDlgLoading');
    },

    // methods
    SetDisplay: function(display) 
    {
        this.GetDivDlgOverlay().style.display = display;
        this.GetDivDlg().style.display = display;
    },

    Resize: function() 
    {
        if (!this.params) return;
        if (this.GetIFrameDlg()) 
        {
            this.GetIFrameDlg().style.left = this.margin + 'px';
            this.GetIFrameDlg().style.top = this.headerHeight + 'px';
            this.GetIFrameDlg().style.width = this.params.width + 'px';
            this.GetIFrameDlg().style.height = this.params.height + 'px';
        }
        this.GetDivDlg().style.width = (this.params.width + (this.margin * 2)) + 'px';
        this.GetDivDlg().style.height = (this.params.height + this.headerHeight) + 'px';
        Position.Dialog();
    },

    Show: function(params) 
    {
        if (params) 
        {
            this.SetDisplay('block');
            this.Update(params);
        }
    },

    Hide: function() 
    {
        this.params = null;
        this.SetDisplay('none');
    },
    
    HideContent: function()
    {
        if (this.GetIFrameDlg()) 
        {
            this.GetIFrameDlg().style.visibility = 'hidden';
            this.Loading(true);
        }
    },
    
    ShowContent: function()
    {
        if (this.GetIFrameDlg()) 
        {
            this.Loading(false);
            this.GetIFrameDlg().style.visibility = 'visible';
        }
    },

    Update: function(params) 
    {
        if (params == null) return;
        this.params = params;
        
        this.GetSpanDlgTitle().innerHTML = params.title;
        this.Resize();

        if (params.path) 
        {
            if (this.GetIFrameDlg())
            {
                this.GetIFrameDlg().style.top = '10000px';
            }
            else
            {
                var iframe = document.createElement('iframe');
                iframe.style.zIndex = '1';
                iframe.style.position = 'absolute';
                iframe.setAttribute('id', 'iframeDlgContent');
                iframe.style.border = 'none';
				iframe.frameBorder = '0px';
                iframe.setAttribute('frameborder', '0');
                iframe.setAttribute('border', '0');
                iframe.setAttribute('cellspacing', '0');
                iframe.setAttribute('scrolling', 'no');
                iframe.setAttribute('marginwidth', '0px');
                iframe.setAttribute('marginheight', '0px');
                iframe.setAttribute('noresize', 'noresize');
                iframe.style.top = '10000px';
                this.GetTdDlgContent().appendChild(iframe);
            }
            
            this.Loading(true);
            this.GetIFrameDlg().style.visibility = 'visible';
            this.GetIFrameDlg().src = SitePath + 'DialogContent.aspx?path=' + params.path + '&' + params.params;
        }
    },

    Loading: function(process) 
    {
        if (this.GetDivDlgLoading())
            this.GetTdDlgContent().removeChild(this.GetDivDlgLoading());
        if (process) 
        {
            var img = document.createElement('img');
            img.src = this.LoadingImageURI;

            var div = document.createElement('div');
            div.setAttribute('id', 'divDlgLoading');
            div.style.zIndex = '0';
            div.style.position = 'absolute';
            div.style.textAlign = 'center';
            div.style.verticalAlign = 'middle';
            div.style.width = this.params.width + 'px';
            div.style.top = ((this.params.height / 2) + (this.headerHeight / 2)) + 'px';
            div.appendChild(img);

            this.GetTdDlgContent().style.height = this.params.height + 'px';
            this.GetTdDlgContent().appendChild(div);
        }
        else 
        {
            this.Resize();
        }
    },

    LoadingComplete: function() 
    {
        this.Loading(false);
    }
}
