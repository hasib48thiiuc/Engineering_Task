namespace NotesWebAPI.Models
{
    public enum NoteType
    {
        Regular,
        Reminder,
        Todo,
        Bookmark
    }

    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public NoteType Type { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsComplete { get; set; }
        public string Url { get; set; }
    }

}
