var Position =
{
    chaptersCount: 5,

    Panel: function() 
    {
        var divPanel = document.getElementById('divPanel');
        divPanel.style.width = Dimension.GetClientWidth() + 'px';
    },

    StarSky: function() 
    {
        var topHeight = 294;
        var bottomHeight = 371;

        var tableWidth = Dimension.GetClientWidth();
        var tableHeight = Dimension.GetClientHeight();
        var tdMiddleHeight = tableHeight - (topHeight + bottomHeight);

        var table = document.getElementById('tblStarSky');
        table.style.width = tableWidth + 'px';
        table.style.height = tableHeight + 'px';

        if (tdMiddleHeight >= 0) 
        {
            var tdMiddle = document.getElementById('tdStarSkyMiddle');
            tdMiddle.style.height = tdMiddleHeight + 'px';
        }
    },

    StarYellow: function() 
    {
        var zodiacLeft = Dimension.GetElementLeft(imgZodiacID);
        var moonLeft = Dimension.GetElementLeft(imgMoonID);
        var moonWidth = Dimension.GetElementWidth(imgMoonID);
        var areaWidth = zodiacLeft - (moonLeft + moonWidth);
        var starWidth = Dimension.GetElementWidth('divStarYellow');
        var div = document.getElementById('divStarYellow');
        if (areaWidth > starWidth) 
        {
            div.style.display = 'block';
            var starLeft = zodiacLeft - starWidth - 20;
            div.style.left = starLeft + 'px';
        }
        else 
        {
            div.style.display = 'none';
        }
    },

    Mount: function() 
    {
        var divMount = document.getElementById('divMount');
        if (Dimension.GetElementWidth('divContent') >= 1150)
        {
            divMount.style.left = '328px';
            divMount.style.top = '177px';
            divMount.style.display = 'block';
        }
        else
        {
            divMount.style.display = 'none';
        }
    },
    
    RhythmInfo: function()
    {
        var height = Dimension.GetElementHeight('divRhythmInfoElements');
        height -= 10;
        if (height > 0)
            document.getElementById('divRhythmInfoBackground').style.height = height + 'px';
    },
    
    QuickDaysGo: function()
    {
        var divQuickDaysGo = document.getElementById('divQuickDaysGo');
    
        var contentWidth = Dimension.GetElementWidth('divContent');
        //var width = Dimension.GetElementWidth('divQuickDaysGo');
        var width = 68;
        var contentLeft = Dimension.GetElementLeft('divContent');
        var chaptersMenuWidth = Dimension.GetElementWidth('divChaptersMenu');
        var left = contentLeft + ((contentWidth - width) / 2) - (chaptersMenuWidth / 2);
        
        var imgChapterOnLeft = Dimension.GetElementLeft('imgChapterOn');
        var imgChapterOnWidth = Dimension.GetElementWidth('imgChapterOn');
        if (left <= imgChapterOnLeft + imgChapterOnWidth)
        {
            divQuickDaysGo.style.display = 'none';
            return;
        }
        
        var contentTop = Dimension.GetElementTop('divContent');
        var top = contentTop + 9;
        
        divQuickDaysGo.style.display = 'block';
        divQuickDaysGo.style.left = left + 'px';
        divQuickDaysGo.style.top = top + 'px';
    },

    Chapters: function() 
    {
        var imgWidth = 104;
        var l = Dimension.GetElementLeft('divContent') - ((imgWidth / 2) | 0) - 3;
        var t = Dimension.GetElementTop('divContent') - 22;
        var w = Dimension.GetClientWidth() - l - 10;
        
        var divChapters = document.getElementById('divChapters');
        divChapters.style.left = l + 'px';
        divChapters.style.top = t + 'px';
        divChapters.style.width = w + 'px';
    },
    
    ContentLoad: function()
    {
        var contentLeft = Dimension.GetElementLeft('divContent');
        var contentTop = Dimension.GetElementTop('divContent');
        var contentWidth = Dimension.GetElementWidth('divContent')
        var contentHeight = Dimension.GetElementHeight('divContent');
        
        var contentLoadWidth = Dimension.GetElementWidth('divContentLoad');
        var contentLoadHeight = Dimension.GetElementHeight('divContentLoad');
        var contentLoadLeft = Math.ceil(contentLeft + ((contentWidth - contentLoadWidth) / 2));
        var contentLoadTop = Math.ceil(contentTop + ((contentHeight - contentLoadHeight) / 2));
        
        var contentLoad = document.getElementById('divContentLoad');
        contentLoad.style.left = contentLoadLeft + 'px';
        contentLoad.style.top = contentLoadTop + 'px';
    },
    
    Content: function() 
    {
        var divContent = document.getElementById('divContent');
        var divBorderTop = document.getElementById('divBorderTop');
        var divContentInner = document.getElementById('divContentInner');
        var divIContent = document.getElementById('divIContent');
        var tbContentNote = document.getElementById(tbContentNoteID);
        
        if (divContent == null)
            return;
    
        var l = 290;
        var t = 293;
        var w = Dimension.GetClientWidth() - l - 40;
        var h = Dimension.GetClientHeight() - t - 10;
        var min_w = 60 * this.chaptersCount + 50;
        var min_h = 400;

        if (w < min_w) w = min_w;
        if (h < min_h) h = min_h;

        divContent.style.left = l + 'px';
        divContent.style.top = t + 'px';
        divBorderTop.style.width = w + 'px';
        divContentInner.style.height = h + 'px';
        divIContent.style.width = (w - 50) + 'px';
        divIContent.style.height = (h - 130) + 'px';
        tbContentNote.style.width = (w - 80) + 'px';

        var divSubstrateID = 'divSubstrate';
        var divSubstrate = document.getElementById(divSubstrateID);
        var wSubstrate = 256;
        var hSubstrate = 256;
        divSubstrate.style.left = l + w - wSubstrate - 50 + 'px';
        divSubstrate.style.top = t + h - hSubstrate - 50 + 'px';
    },
    
    Dialog: function()
    {
        var id = 'divDlg';
        var clientWidth = Dimension.GetClientWidth();
        var clientHeight = Dimension.GetClientHeight();
        var width = Dimension.GetElementWidth(id);
        var height = Dimension.GetElementHeight(id);
        var left = (clientWidth - width) / 2;
        var top = (clientHeight - height) / 2;
        var divDlg = document.getElementById(id);
        divDlg.style.left = left + 'px';
        divDlg.style.top = top + 'px';
    }
}
