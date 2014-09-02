using Enterprise.Core.Model;

namespace Enterprise.Core.Validation.Interfaces
{
    public interface IEntityValidator<in TEntity, TKey> where TEntity : Entity<TKey>
    {
        void ValidateAndThrow(TEntity instance);
        void ValidateAndThrow(TEntity instance, string ruleset);
        void ValidateAndThrow(TEntity instance, string ruleset, bool cascade);
    }
}
