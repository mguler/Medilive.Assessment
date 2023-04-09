namespace Medilive.Assessment.Core.Abstract.RuleEngine
{
    public interface IRule
    {
        void Apply(params object[] args);
    }
}
