namespace Medilive.Assessment.Core.Abstract.Data
{
    public interface IDataRepository : IDisposable
    {
        IQueryable<T> Get<T>() where T : class;
        T Save<T>(T entity) where T : class;
        IEnumerable<T> Save<T>(IEnumerable<T> entities) where T : class;
    }
}
