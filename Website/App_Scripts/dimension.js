var Dimension = 
{
    GetClientWidth: function() 
    {
        if (typeof window.innerWidth != 'undefined')
        {
            return window.innerWidth;
        }
        else if (typeof document.documentElement != 'undefined' &&
                 typeof document.documentElement.clientWidth != 'undefined' &&
                 document.documentElement.clientWidth != 0)
        {
            return document.documentElement.clientWidth;
        }
        else
        {
            return document.getElementsByTagName('body')[0].clientWidth;
        }
    },
    
    GetClientHeight: function()
    {
        if (typeof window.innerHeight != 'undefined')
        {
            return window.innerHeight;
        }
        else if (typeof document.documentElement != 'undefined' &&
                 typeof document.documentElement.clientHeight != 'undefined' &&
                 document.documentElement.clientHeight != 0)
        {
            return document.documentElement.clientHeight;
        }
        else
        {
            return document.getElementsByTagName('body')[0].clientHeight;
        }
    },
    
    GetObjNN4: function(obj, name)
    {
        var x = obj.layers;
        var foundLayer;
        for (var i = 0; i < x.length; i++) 
        {
            if (x[i].id == name)
                foundLayer = x[i];
            else if (x[i].layers.length)
                var tmp = this.GetObjNN4(x[i], name);
            if (tmp) foundLayer = tmp;
        }
        return foundLayer;
    },
    
    GetElementHeight: function(elem)
    {
        if (ns4) 
        {
            var elem = this.GetObjNN4(document, elem);
            return elem.clip.height;
        } 
        else 
        {
            if (document.getElementById) 
                var elem = document.getElementById(elem);
            else if (document.all) 
                var elem = document.all[elem];
            if (op5) 
                xPos = elem.style.pixelHeight;
            else 
                xPos = elem.offsetHeight;
            return xPos;
        }
    },
    
    GetElementWidth: function(elem)
    {
        if (ns4) 
        {
            var elem = this.GetObjNN4(document, elem);
            return elem.clip.width;
        } 
        else 
        {
            if (document.getElementById) 
                var elem = document.getElementById(elem);
            else if (document.all) 
                var elem = document.all[elem];
            if (op5) 
                xPos = elem.style.pixelWidth;
            else 
                xPos = elem.offsetWidth;
            return xPos;
        }
    },
    
    GetElementLeft: function(elem)
    {
        if (ns4) 
        {
            var elem = this.GetObjNN4(document, elem);
            return elem.pageX;
        } 
        else 
        {
            if (document.getElementById) 
                var elem = document.getElementById(elem);
            else if (document.all) 
                var elem = document.all[elem];
            xPos = elem.offsetLeft;
            tempEl = elem.offsetParent;
            while (tempEl != null) 
            {
                xPos += tempEl.offsetLeft;
                tempEl = tempEl.offsetParent;
            }
            return xPos;
        }
    },
    
    GetElementTop: function(elem)
    {
        if (ns4) 
        {
            var elem = this.GetObjNN4(document, elem);
            return elem.pageY;
        } 
        else 
        {
            if (document.getElementById) 
                var elem = document.getElementById(elem);
            else if (document.all) 
                var elem = document.all[elem];
            yPos = elem.offsetTop;
            tempEl = elem.offsetParent;
            while (tempEl != null) 
            {
                yPos += tempEl.offsetTop;
                tempEl = tempEl.offsetParent;
            }
            return yPos;
        }
    }
}
