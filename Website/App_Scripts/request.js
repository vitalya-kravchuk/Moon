Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

var timeoutContentLoadID = 0;

function pageLoaded(sender, args)
{
    document.getElementById('divAppLoad').style.display = 'none';
    document.getElementById('divAppContent').style.display = 'block';
    Event.WindowResize();
}

function beginRequest(sender, args)
{
    postbackElement = args.get_postBackElement();
    var id = postbackElement.id;
    if (id.indexOf('Content') > 0)
    {
        timeoutContentLoadID = 
            setTimeout("document.getElementById('divContentLoad').style.display = 'block'", 300);
    }
}

function endRequest(sender, args)
{
    clearTimeout(timeoutContentLoadID);
    document.getElementById('divContentLoad').style.display = 'none';
}
