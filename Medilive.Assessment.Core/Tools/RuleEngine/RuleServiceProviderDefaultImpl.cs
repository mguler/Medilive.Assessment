using Medilive.Assessment.Core.Abstract.RuleEngine;

namespace Medilive.Assessment.Core.Tools.RuleEngine
{
    public class RuleServiceProviderDefaultImpl : IRuleServiceProvider
    {
        private readonly Dictionary<string, Type> _cache = new Dictionary<string, Type>();
        private Func<Type, object> _dependencyResolverCallback;

        public void AddRule<TRule>(string key) where TRule : class, IRule
        {
            _cache.Add(key, typeof(TRule));
        }
        public void AddRule(string key,Type ruleType) 
        {
            _cache.Add(key, ruleType);
        }
        public IRule ApplyRules(string key, params object[] args)
        {
            if (!_cache.ContainsKey(key))
            {
                return null;
            }

            var ctorArgs = new List<object>();
            var rule = _cache[key];
            var cancelRuleExecution = false;

            var ctor = rule.GetConstructors().FirstOrDefault();
            if (ctor == null)
            {
                return null;
            }

            var ctorArgTypes = ctor.GetParameters().ToList();
            var orderedArgs = new List<object>();

            if (ctorArgTypes.Count > 0 && _dependencyResolverCallback != null)
            {
                ctorArgTypes.ForEach(parameterInfo =>
                {
                    var argInstance = ctorArgs.SingleOrDefault(arg => arg.GetType() == parameterInfo.ParameterType);
                    if (argInstance == null)
                    {
                        argInstance = _dependencyResolverCallback(parameterInfo.ParameterType);
                        ctorArgs.Add(argInstance);
                    }
                    orderedArgs.Add(argInstance);
                });
            }

            var instance = ctor.Invoke(orderedArgs.ToArray()) as Rule;

            try
            {
                instance.Apply(args);
            }
            catch (Exception ex)
            {
                throw;//RuleEngineException(ex.InnerException, instance.GetType(), args);
            }

            return instance;
        }
        public void SetDependencyResolver(Func<Type, object> func) => _dependencyResolverCallback = func;
    }

}
