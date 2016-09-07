var Animation = 
{
    Comet: function(show)
    {
        var id = 'divComet';
        var obj = document.getElementById(id);
        if (show)
        {
            var maxLeft = Dimension.GetElementLeft(imgMoonID) - 150;
            var maxTop = Dimension.GetElementTop(imgSymbolID) - 150;
            var left = Math.floor(Math.random() * maxLeft);
            var top = Math.floor(Math.random() * maxTop);
            obj.style.left = left + 'px';
            obj.style.top = top + 'px';
            obj.style.display = 'block';
            setTimeout('Animation.Comet(false)', 5500);
        }
        else
        {
            obj.style.display = 'none';
            setTimeout('Animation.Comet(true)', 10000);
        }
    },
    
    StarLight: function(show)
    {
        var id = 'divStarLight';
        var obj = document.getElementById(id);
        if (show)
        {
            var maxLeft = Dimension.GetElementLeft(imgMoonID) - 150;
            var maxTop = Dimension.GetElementTop(imgSymbolID) - 150;
            var left = Math.floor(Math.random() * maxLeft);
            var top = Math.floor(Math.random() * maxTop);
            obj.style.left = left + 'px';
            obj.style.top = top + 'px';
            obj.style.display = 'block';
            setTimeout('Animation.StarLight(false)', 1000);
        }
        else
        {
            obj.style.display = 'none';
            setTimeout('Animation.StarLight(true)', 30000);
        }
    },
    
    mlcIndex: 0,
    mlcStop: false,
    MLC: function()
    {
        if (!document.getElementById('divAnimationCacheMLC'))
            return;
        imgPanelMLC = document.getElementById('imgPanelMLC');
        if (this.mlcStop)
        {
            imgPanelMLC.src = ImagesPath + 'Panel/mlc.png';
            return;
        }
        imgPanelMLC.src = ImagesPath + 'Panel/mlc/' + this.mlcIndex + '.jpg';
        this.mlcIndex++;
        if (this.mlcIndex > 11)
            this.mlcIndex = 0;
        setTimeout('Animation.MLC()', 100);
    }
}

setTimeout('Animation.Comet(true)', 10000);
setTimeout('Animation.StarLight(true)', 10000);
setTimeout('Animation.MLC()', 5000);
