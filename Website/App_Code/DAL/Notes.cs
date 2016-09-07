using System;
using SqlDataAccess;

namespace DAL
{
    public class Notes
    {
        #region Properties
        public int NoteID { get; set; }
        public int PersonID { get; set; }
        public int RhythmID { get; set; }
        public int ChapterID { get; set; }
        public string Text { get; set; }
        #endregion

        public static Notes Get(int personID, int rhythmID, int chapterID)
        {
            StoredProcedure sp = new StoredProcedure("Notes_Get");
            sp["PersonID"].Value = personID;
            sp["RhythmID"].Value = rhythmID;
            sp["ChapterID"].Value = chapterID;
            Notes note = sp.ExecuteSingle<Notes>();
            if (note != null)
                Logger.Log.Info(note.NoteID);
            return note;
        }

        public int Save()
        {
            StoredProcedure sp = new StoredProcedure("Notes_Save");
            sp["NoteID"].Value = NoteID;
            sp["PersonID"].Value = PersonID;
            sp["RhythmID"].Value = RhythmID;
            sp["ChapterID"].Value = ChapterID;
            sp["Text"].Value = Text;
            Logger.Log.Info(NoteID);
            return Convert.ToInt32(sp.ExecuteScalar());
        }
    }
}
