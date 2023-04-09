using Medilive.Assessment.Core.Abstract.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Medilive.Assessment.Affiliate.Data
{
    /// <summary>
    /// The default implementation of "IDataRepository" interface that supplies database access and basic functionality
    /// </summary>
    public class DataRepositoryDefaultImpl : IDataRepository
    {
        private readonly DbContext _dataContext;
        private readonly Dictionary<string, IClrPropertyGetter> _primaryKeyCache = new Dictionary<string, IClrPropertyGetter>();

        public DataRepositoryDefaultImpl(DbContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IQueryable<T> Get<T>() where T : class => _dataContext.Set<T>();
        public T Save<T>(T entity) where T : class
        {
            var primaryKey = GetPrimaryKey<T>();

            if (primaryKey.HasDefaultValue(entity))
            {
                _dataContext.Add(entity);
            }
            else
            {
                _dataContext.Update(entity);
            }
            _dataContext.SaveChanges();
            return entity;
        }

        public IEnumerable<T> Save<T>(IEnumerable<T> entities) where T : class
        {
            foreach (var entity in entities)
            {
                Save(entity);
                yield return entity;
            }
        }
        private IClrPropertyGetter GetPrimaryKey<T>()
        {
            var key = typeof(T).FullName;
            var getter = _primaryKeyCache.ContainsKey(key) ? _primaryKeyCache[key]
                : _dataContext.Model.GetEntityTypes().FirstOrDefault(entityType => entityType.ClrType.FullName == key).FindPrimaryKey().Properties.FirstOrDefault()?.GetGetter();

            _primaryKeyCache.Add(key, getter);

            return getter;
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}
