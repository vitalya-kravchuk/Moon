<%@ Application Language="C#" %>

<script runat="server">
    void Application_Start(object sender, EventArgs e) 
    {
        Logger.Init();
        Logger.Log.Info("OK");
        SqlDataAccess.StoredProcedure.ConnectionString = ConfigurationManager.ConnectionStrings["MoonCalendar"].ConnectionString;
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        Logger.Log.Info("OK");
    }
        
    void Application_Error(object sender, EventArgs e) 
    {
        Logger.Log.ErrorFormat("{0} \r\n {1} \r\n",
            Request.UserHostAddress, Server.GetLastError());
		
		/*
        string message = string.Empty;
        if (Server.GetLastError().InnerException != null)
            message = Server.GetLastError().InnerException.Message;
        new Mail().Send(
            ConfigurationManager.AppSettings["ContactEmail"],
            ConfigurationManager.AppSettings["WebSiteName"] + ": ошибка",
            message);
		*/
    }

    void Session_Start(object sender, EventArgs e) 
    {
        Logger.Log.Info(Request.UserHostAddress);
    }

    void Session_End(object sender, EventArgs e) 
    {
        Logger.Log.Info(Request.UserHostAddress);
    }
</script>
