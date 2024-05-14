namespace _4._01_Hw.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Answer> Answers { get; set; }
        public List<QuestionTags> QuestionTags { get; set; }
    }
}