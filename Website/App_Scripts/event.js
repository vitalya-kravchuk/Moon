var Event =
{
    WindowResize: function() 
    {
        Position.Panel();
        Position.StarSky();
        Position.StarYellow();
        Position.Content();
        Position.ContentLoad();
        Position.RhythmInfo();
        Position.Chapters();
        Position.QuickDaysGo();
        Position.Mount();
        Position.Dialog();
        Ballon.UpdatePos();
    },
    
    Reload: function()
    {
        window.location = window.location;
    },
    
    SetImageActive: function(imgObj, active, extOn, extOff) 
    {
        var src = imgObj.src;
        var pointPos = src.lastIndexOf('.');
        var ext = src.substr(pointPos);
        if (active) {
            src = src.substr(0, pointPos);
            src += 'a';
            if (extOn) src += extOn; else src += ext;
        }
        else {
            src = src.substr(0, pointPos - 1);
            if (extOff) src += extOff; else src += ext;
        }
        imgObj.src = src;
    },

    // Меню

    SetMenuBtnActive: function(active) 
    {
        var imgPanelMLC = document.getElementById('imgPanelMLC');
        if (active) 
        {
            imgPanelMLC.src = ImagesPath + 'Panel/mlca.png';
            this.ShowMenu(true);    
        }
        else
        {
            imgPanelMLC.src = ImagesPath + 'Panel/mlc.png';
        }
    },

    ShowMenu: function(show) 
    {
        var divMenu = document.getElementById('divMenu');
        var divRhythmInfo = document.getElementById('divRhythmInfo');

        if (show) 
        {
            divMenu.style.display = 'block';
            divRhythmInfo.style.display = 'none';
        }
        else 
        {
            divMenu.style.display = 'none';
            divRhythmInfo.style.display = 'block';
        }
    },

    MenuClick: function(itemName) 
    {
        this.ShowMenu(false);
        switch (itemName)
        {
            case "Blog":
                window.open("http://twitter.com/moyaluna");
                break;
            case "Why":
                ShowDialog.Why();
                break;
            case "Test":
                ShowDialog.Test();
                break;
            case "Books":
                ShowDialog.Books();
                break;
            case "Comment":
                ShowDialog.SendMail();
                break;
        }
    },

    MenuMouseMove: function(tdObj, itemName, active) 
    {
        var imgObj = document.getElementById(itemName);
        this.SetImageActive(imgObj, active, '.gif', '.png');
        if (active) 
        {
            tdObj.setAttribute('class', 'MenuPanelActive');
            tdObj.className = 'MenuPanelActive';
        }
        else 
        {
            tdObj.setAttribute('class', 'MenuPanel');
            tdObj.className = 'MenuPanel';
        }
    },

    // Панель

    SetButtonPanelActive: function(active, root) 
    {
        var left = document.getElementById('td' + root + 'Left');
        var center = document.getElementById('td' + root);
        var right = document.getElementById('td' + root + 'Right');
        if (active) 
        {
            left.setAttribute('class', 'PanelButtonLeftActive');
            center.setAttribute('class', 'PanelButtonActive');
            right.setAttribute('class', 'PanelButtonRightActive');

            left.className = 'PanelButtonLeftActive';
            center.className = 'PanelButtonActive';
            right.className = 'PanelButtonRightActive';
        }
        else 
        {
            left.removeAttribute('class');
            center.removeAttribute('class');
            right.removeAttribute('class');

            left.className = '';
            center.className = '';
            right.className = '';
        }
    },

    lastTime: '',
    ShowTime: function(show) 
    {
        var bpcTime = document.getElementById(bpcTimeID);
        var tbTime = document.getElementById(tbTimeID);
        if (show) 
        {
            bpcTime.style.display = 'none';
            tbTime.style.display = 'block';
            tbTime.focus();
            tbTime.select();
            this.lastTime = tbTime.value;
        }
        else 
        {
            bpcTime.style.display = 'block';
            tbTime.style.display = 'none';
            if (this.lastTime != tbTime.value)
                UpdateDateTime();
        }
    },

    lastDate: '',
    ShowDate: function(show) 
    {
        var bpcDate = document.getElementById(bpcDateID);
        var tbDate = document.getElementById(tbDateID);
        if (show) 
        {
            bpcDate.style.display = 'none';
            tbDate.style.display = 'block';
            tbDate.focus();
            tbDate.select();
            this.lastDate = tbDate.value;
        }
        else 
        {
            bpcDate.style.display = 'block';
            tbDate.style.display = 'none';
            if (this.lastDate != tbDate.value)
                UpdateDateTime();
        }
    },

    // Контент
    
    SetChapterActive: function(imgObj, active) 
    {
        this.SetImageActive(imgObj, active);
    },
    
    ChapterChange: function(chapterName)
    {
        if (chapterName == '')
            return;
    
        var imgChapterOn = document.getElementById("imgChapterOn");
        var imgChapter = document.getElementById("imgChapter" + chapterName);
        imgChapterOn.src = ImagesPath + "Chapter/" + chapterName + "on.jpg";
        
        var divChaptersMenu = document.getElementById('divChaptersMenu');
        var images = divChaptersMenu.getElementsByTagName('IMG');
        for (var i = 0; i < images.length; i++)
        {
            images[i].style.display = '';
        }
        imgChapter.style.display = 'none';
        
        var imgSubstrate = document.getElementById('imgSubstrate');
        imgSubstrate.src = ImagesPath + "Chapter/" + chapterName + ".gif";
        
        Position.Content();
    },
    
    ChapterClick: function(chapterName)
    {
        if (chapterName == '')
            return;
        this.ChapterChange(chapterName);
        UpdateContent(chapterName);
    },

    ShowNote: function(show) 
    {
        var divContentNoteEdit = document.getElementById('divContentNoteEdit');
        var divContentNote = document.getElementById('divContentNote');
        var tbContentNote = document.getElementById(tbContentNoteID);
        
        var text = tbContentNote.value;
        var textLen = text.length;
        text = text.replace("<", "");
        text = text.replace(">", "");
        tbContentNote.value = text;
        if (textLen != text.length)
            return false;
        
        if (show)
        {
            divContentNoteEdit.style.display = 'none';
            divContentNote.style.display = 'block';
            tbContentNote.focus();
            tbContentNote.scrollTop = tbContentNote.scrollHeight;
        }
        else
        {
            divContentNoteEdit.style.display = 'block';
            divContentNote.style.display = 'none';
        }
        
        return true;
    },
    
    // Дополнительно
    
    ShowLockOverlay: function(show)
    {
        var divLockOverlay = document.getElementById('divLockOverlay');
        if (show)
            divLockOverlay.style.display = 'block';
        else
            divLockOverlay.style.display = 'none';
    }
}

// Инициализация
if (document.all)
{
    document.body.scroll = "no";
    document.body.onload = Event.WindowResize;
    document.body.onresize = Event.WindowResize;
}
else
{
    document.body.style.overflow = "hidden";
    if (window.addEventListener)
    {
        window.addEventListener("load", Event.WindowResize, false);
        window.addEventListener("resize", Event.WindowResize, false);
    }
    else
    {
        window.attachEvent("onload", Event.WindowResize);
        window.attachEvent("onresize", Event.WindowResize);
    }
    window.onload = function()
    {
        Event.WindowResize();
    }
}
