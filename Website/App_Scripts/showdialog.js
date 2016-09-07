var ShowDialog = 
{
    Location: function()
    {
        var params =
        {
            width: 490,
            height: 510,
            title: 'Местоположение',
            path: '~/Controls/Location/LocationControl.ascx'
        }
        Dialog.Show(params);
    },
    
    MyInfo: function(update) 
    {
        var params =
        {
            width: 950,
            height: 350,
            title: 'Мои данные',
            path: '~/Controls/MyInfo/MyInfoControl.ascx'
        }
        if (update)
            Dialog.Update(params);
        else
            Dialog.Show(params);
    },
    
    RhythmsView: function(RhythmType)
    {
        var params =
        {
            width: 230,
            height: 100,
            title: 'Выбрать лунный ритм',
            path: '~/Controls/RhythmsView/RhythmsViewControl.ascx',
            params: 'RhythmType=' + RhythmType
        }
        Dialog.Show(params);
    },
    
    // Login
    
    Login: function()
    {
        var params = 
        {
            width: 220,
            height: 150,
            title: 'Вход',
            path: '~/Controls/Login/LoginControl.ascx'
        }
        Dialog.Show(params);
    },
    
    RemindPassword: function(update)
    {
        var params = 
        {
            width: 230,
            height: 80,
            title: 'Напомнить пароль',
            path: '~/Controls/Login/RemindPasswordControl.ascx'
        }
        if (update)
            Dialog.Show(params);
        else
            Dialog.Update(params);
    },
    
    EmailConfirm: function()
    {
        var params =
        {
            width: 500,
            height: 100,
            title: 'Регистрация',
            path: '~/Controls/Login/EmailConfirmControl.ascx'
        }
        Dialog.Show(params);
    },
    
    // Menu
    
    SendMail: function()
    {
        var params =
        {
            width: 500,
            height: 215,
            title: 'Написать отзыв',
            path: '~/Controls/Menu/SendMailControl.ascx'
        }
        Dialog.Show(params);
    },
    
    Why: function()
    {
        var params =
        {
            width: 800,
            height: 355,
            title: 'Зачем Лунный календарь',
            path: '~/Controls/Menu/WhyControl.ascx'
        }
        Dialog.Show(params);
    },
    
    Test: function()
    {
        var params =
        {
            width: 480,
            height: 210,
            title: 'Тест',
            path: '~/Controls/Menu/TestControl.ascx'
        }
        Dialog.Show(params);
    },
    
    Books: function(update, category)
    {
        var params =
        {
            width: 700,
            height: 560,
            title: 'Источник информации',
            path: '~/Controls/Menu/BooksControl.ascx',
            params: 'CategoryID=' + category
        }
        if (update)
            Dialog.Update(params);
        else
            Dialog.Show(params);
    }
}
