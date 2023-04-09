namespace Medilive.Assessment.Core.Abstract.RuleEngine
{
    public interface IRuleCollection
    {
        bool HasErrors { get; }
        bool HasMessages { get; }
        IEnumerable<Message> GetMessages();
    }
}
