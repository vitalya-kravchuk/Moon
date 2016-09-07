using System;

namespace DAL
{
    public enum eChapter
    {
        Common = 1,
        Health,
        Money,
        Beauty,
        Garden,
    }

    public class Chapters
    {
        public static int GetID(eChapter chapter)
        {
            return (int)chapter;
        }
    }
}
