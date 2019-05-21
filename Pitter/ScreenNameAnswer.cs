namespace Pitter {
    public class ScreenNameAnswer
    {
        public enum AnswerType
        {
            User,
            Group
        }

        public AnswerType Type { get; set; }
        public long ObjectId { get; set; }
    }
}