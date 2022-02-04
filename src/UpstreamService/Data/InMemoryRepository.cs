using System.Collections.Concurrent;

namespace UpstreamService.Data
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> Get(int id);

        Task<TEntity> Put(int id, TEntity entity);
    }

    internal class InMemoryRepository<TEntity> : IRepository<TEntity>
    {
        private readonly ConcurrentDictionary<int, TEntity> _entities = new();

        public async Task<TEntity> Get(int id) => _entities.TryGetValue(id, out var entity)
            ? entity
            : throw new EntityNotFoundException();

        public async Task<TEntity> Put(int id, TEntity entity) =>
            _entities.AddOrUpdate(id, entity, (_, __) => entity);
    }

    internal class EntityNotFoundException : Exception
    { }
}
