using Enterprise.Core.Model;
using Enterprise.Core.Service.Interfaces;
using Enterprise.Core.Storage;

namespace Enterprise.Core.Service
{
    public class CRUDService<TEntity, TKey> : BaseReadOnlyService<TEntity, TKey>, ICRUDService<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        public CRUDService(IRepository<TEntity, TKey> repository)
            : base(repository)
        {
        }

        public virtual TEntity Add(TEntity entity)
        {
            OnBeforeAdd(entity);
            TKey id = Repository.Add(entity);
            entity = Repository.Get(id);
            OnAfterAdd(entity);
            return entity;
        }

        public virtual void Delete(TKey id)
        {
            Delete(Repository.Get(id));
        }

        public virtual void Delete(TEntity entity)
        {
            OnBeforeDelete(entity);
            Repository.Delete(entity);
            OnAfterDelete(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            OnBeforeUpdate(entity);
            Repository.Update(entity);
            OnAfterUpdate(entity);
            return entity;
        }

        protected void OnAfterAdd(TEntity entity)
        {
            if (AfterAdd != null)
                AfterAdd(entity);
        }

        protected void OnAfterDelete(TEntity entity)
        {
            if (AfterDelete != null)
                AfterDelete(entity);
        }

        protected void OnAfterUpdate(TEntity entity)
        {
            if (AfterUpdate != null)
                AfterUpdate(entity);
        }

        protected virtual void OnBeforeAdd(TEntity entity)
        {
            if (BeforeAdd != null)
                BeforeAdd(entity);
        }

        protected virtual void OnBeforeDelete(TEntity entity)
        {
            if (BeforeDelete != null)
                BeforeDelete(entity);
        }

        protected virtual void OnBeforeUpdate(TEntity entity)
        {
            if (BeforeUpdate != null)
                BeforeUpdate(entity);
        }

        protected event AfterHandler AfterAdd;

        protected event AfterHandler AfterDelete;

        protected event AfterHandler AfterUpdate;

        protected event BeforeHandler BeforeAdd;

        protected event BeforeHandler BeforeDelete;

        protected event BeforeHandler BeforeUpdate;

        protected delegate void AfterHandler(TEntity entity);

        protected delegate void BeforeHandler(TEntity entity);
    }
}