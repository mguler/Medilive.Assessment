namespace Medilive.Assessment.Core.Abstract.RuleEngine
{
    public class Message
    {
        public string Code { get; set; }
        public string Text { get; private set; }
        public string Type { get; set; }
        public Priority Priority { get; private set; }
        
        public Message(string messageText, string type, Priority priority = Priority.Error)
        {
            Text = messageText;
            Type = type;
            Priority = priority;
        }
        public Message(string messageText, string type, string code, Priority priority = Priority.Error)
        {
            Text = messageText;
            Type = type;
            Code = code;
            Priority = priority;
        }
    }
}
