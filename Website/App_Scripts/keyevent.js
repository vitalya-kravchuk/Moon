var KeyEvent = 
{
    child: false
}

if (document.addEventListener)
{
    document.addEventListener("keypress", OnKeyPress, false);
}
else if (document.attachEvent)
{
    document.attachEvent("onkeypress", OnKeyPress);
}
else
{
    document.onkeypress = OnKeyPress;
}

function OnKeyPress(e)
{
    if (!e) e = event;
    var code = 0;
    if (e.keyCode) 
        code = e.keyCode;
    else if (e.which)
        code = e.which;
    else if (e.charCode)
        code = e.charCode;
    switch (code)
    {
        case 27:
            if (KeyEvent.child)
                parent.Dialog.Hide();
            else
                Dialog.Hide();
            break;
    }
    return true;
}
