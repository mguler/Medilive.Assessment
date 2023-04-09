using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medilive.Assessment.Core.Abstract.RuleEngine
{
    public interface IRuleServiceProvider 
    {

        void AddRule<TRule>(string key) where TRule : class, IRule;

        IRule ApplyRules(string key, params object[] args);
        void SetDependencyResolver(Func<Type, object> func);
    }
}
