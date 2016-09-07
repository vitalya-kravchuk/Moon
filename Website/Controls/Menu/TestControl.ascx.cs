using System;
using System.Web.UI;

public partial class Controls_Menu_TestControl : BaseControl
{
    const int QuestionsCount = 14;
    string[] TestData = Res.GetString("Test").Split('\r');

    int QuestionIndex
    {
        get
        {
            int questionIndex;
            int.TryParse(hfQuestionIndex.Value, out questionIndex);
            return questionIndex;
        }
        set
        {
            hfQuestionIndex.Value = value.ToString();
        }
    }
    int Points
    {
        get
        {
            int points;
            int.TryParse(hfPoints.Value, out points);
            return points;
        }
        set
        {
            hfPoints.Value = value.ToString();
        }
    }

    void ShowAbout()
    {
        HideButtons();
        lblTitle.Text = TestData[0];
        lblBody.Text = TestData[1];
        tblStartTest.Visible = true;
    }

    void NextQuestion()
    {
        QuestionIndex++;
        HideButtons();
        if (QuestionIndex <= QuestionsCount)
        {
            lblTitle.Text = QuestionIndex + " вопрос из " + QuestionsCount.ToString();
            lblBody.Text = TestData[QuestionIndex + 1];
            tblAnswers.Visible = true;
        }
        else
        {
            int resultIndex = 0;
            if (Points < 5)
            {
                resultIndex = 3;
            }
            else if (Points >= 5 && Points < 7)
            {
                resultIndex = 2;
            }
            else if (Points >= 7 && Points < 10)
            {
                resultIndex = 1;
            }
            else
            {
                resultIndex = 0;
            }
            lblTitle.Text = "Результат";
            lblBody.Text = TestData[QuestionsCount + 2 + resultIndex];
            btnOK.Visible = true;
            Logger.Log.InfoFormat("Person ID: {0}, points: {1}, result index: {2}", 
                Person.PersonID, Points, resultIndex);
        }
    }

    void HideButtons()
    {
        tblStartTest.Visible = false;
        tblAnswers.Visible = false;
        btnOK.Visible = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Books book = new Books(AppDataPathPhysical).Get("daily_life");
            hlBook.Text = book.Name + ". " + book.Author;
            hlBook.NavigateUrl = book.OrderURL;
            ShowAbout();
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
            "parent.Dialog.ShowContent();", true);
    }
    protected void btnStartTest_Click(object sender, EventArgs e)
    {
        NextQuestion();
    }
    protected void btnYes_Click(object sender, EventArgs e)
    {
        Points++;
        NextQuestion();
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        NextQuestion();
    }
}
