﻿namespace Medilive.Assessment.Core.Abstract.RuleEngine
{
    public abstract class Rule : IRule
    {
        public bool CancelRuleExecution { get; protected set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool HasErrors => _messages.Any(message => message.Priority == Priority.Error);
        public bool HasMessages => _messages.Any();
        private List<Message> _messages { get; set; } = new List<Message>();
        private Delegate _rule;
        private void Register(Delegate func) => _rule = func;
        public void Define(Action func) => Register(func);
        public void Define<TArgument1>(Action<TArgument1> func) => Register(func);
        public void Define<TArgument1, TArgument2>(Action<TArgument1, TArgument2> func) => Register(func);
        public void Define<TArgument1, TArgument2, TArgument3>(Action<TArgument1, TArgument2, TArgument3> func) => Register(func);
        public void Define<TArgument1, TArgument2, TArgument3, TArgument4>(Action<TArgument1, TArgument2, TArgument3, TArgument4> func) => Register(func);
        public void Define<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5>(Action<TArgument1, TArgument2, TArgument3, TArgument4> func) => Register(func);
        public void Define<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6>(Action<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6> func) => Register(func);
        public void Define<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7>(Action<TArgument1, TArgument2, TArgument3, TArgument4, TArgument5, TArgument6, TArgument7> func) => Register(func);
        public void Apply(params object[] args) => _rule.DynamicInvoke(args);
        public void AddMessage(string message,string type, Priority priority = Priority.Error) => _messages.Add(new Message(message, type, priority));
        public void AddMessage(string message,string type, string code, Priority priority = Priority.Error) => _messages.Add(new Message(message,type, code, priority));
        public IEnumerable<Message> GetMessages() => _messages;

    }
}
