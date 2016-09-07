var Ballon = 
{
    // public
    targetID: '',
    target: null,
    
    // private
    id: 'divBallon',
    idArrow: 'divBallonArrow',
    spanID: 'spanBallon',
    
    Show: function(text, targetID)
    {
        setTimeout("Ballon.ShowNow('" + text + "', '" + targetID + "')", 15000);
    },
    
    ShowNow: function(text, targetID)
    {
        var spanObj = document.getElementById(this.spanID);
        spanObj.innerHTML = text;
        
        this.targetID = targetID;
        this.target = document.getElementById(this.targetID);
        
        var divBallon = document.getElementById(this.id);
        divBallon.style.display = 'block';
        if (ie)
        {
            divBallon.onclick = function()
            {
                Ballon.Hide();
                Ballon.target.click();
            }
        }
        else
        {
            divBallon.setAttribute('onclick', 
                'Ballon.Hide(); ' + this.target.getAttribute('onclick'));
        }
        
        var divBallonArrow = document.getElementById(this.idArrow);
        divBallonArrow.style.display = 'block';
        
        this.UpdatePos();
    },
    
    Hide: function()
    {
        document.getElementById(this.id).style.display = 'none';
        document.getElementById(this.idArrow).style.display = 'none';
    },
    
    UpdatePos: function()
    {
        if (document.getElementById(this.id).style.display == 'none')
            return;
    
        var tLeft = Dimension.GetElementLeft(this.targetID);
        var tTop = Dimension.GetElementTop(this.targetID);
        var tHeight = Dimension.GetElementHeight(this.targetID);
        
        var left = tLeft - 5;
        var top = tTop + tHeight + 3;
        
        var obj = document.getElementById(this.id);
        obj.style.left = left + 'px';
        obj.style.top = top + 'px';
        
        var objArrow = document.getElementById(this.idArrow);
        objArrow.style.left = left + 21 + 'px';
        objArrow.style.top = top - 9 + 'px';
    }
}
