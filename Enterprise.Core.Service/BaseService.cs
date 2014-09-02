using System;
using System.Collections.Generic;
using System.Linq;
using Core.Service.Interfaces;
using Enterprise.Core.Model;
using Enterprise.Core.Service.Interfaces;
using Enterprise.Core.Storage;
using Enterprise.Core.Validation;
using Enterprise.Core.Validation.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using PagedList;

namespace Enterprise.Core.Service
{
    public abstract class BaseService<TEntity, TKey> : CRUDService<TEntity, TKey>, IService<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        private readonly IEntityValidator<TEntity, TKey> _validator;

        protected BaseService(IRepository<TEntity, TKey> repository, IEntityValidator<TEntity, TKey> validator)
            : base(repository)
        {
            _validator = validator;
        }

        public IEntityValidator<TEntity, TKey> Validator
        {
            get { return _validator; }
        }

        public override TEntity Add(TEntity entity)
        {
            Validate(entity, RuleSets.Create, true);
            return base.Add(entity);
        }

        public override TEntity Update(TEntity entity)
        {
            Validate(entity, RuleSets.Update, true);
            return base.Update(entity);
        }

        public override void Delete(TKey id)
        {
            try
            {
                TEntity entity = Repository.Get(id);
                Delete(entity);
            }
            catch (InvalidOperationException)
            {
                var validatorFailures = new List<ValidationFailure>
                {
                    new ValidationFailure("id", "item não pertence à lista")
                };
                throw new ValidationException(validatorFailures);
            }
        }

        public override void Delete(TEntity entity)
        {
            try
            {
                Validate(entity, RuleSets.Delete, false);
                base.Delete(entity);
            }
            catch (InvalidOperationException)
            {
                var validatorFailures = new List<ValidationFailure>
                {
                    new ValidationFailure("id", "item não pertence à lista")
                };
                throw new ValidationException(validatorFailures);
            }
        }

        public override TEntity Get(TKey id)
        {
            try
            {
                return base.Get(id);
            }
            catch (InvalidOperationException)
            {
                var validatorFailures = new List<ValidationFailure>
                {
                    new ValidationFailure("id", "item não pertence à lista")
                };
                throw new ValidationException(validatorFailures);
            }
        }

        public virtual IPagedList<TEntity> GetByFilter(IPageFilter filter)
        {
            return filter.Paged(GetForReport(filter));
        }

        public virtual IQueryable<TEntity> GetForReport(IPageFilter filter)
        {
            IQueryable<TEntity> query = filter.Filter(Repository.Items);
            return filter.Sort(query);
        }

        protected override void OnBeforeAdd(TEntity entity)
        {
            if (BeforeAdd != null)
                BeforeAdd(entity, Validator);
        }

        protected override void OnBeforeDelete(TEntity entity)
        {
            if (BeforeDelete != null)
                BeforeDelete(entity, Validator);
        }

        protected override void OnBeforeUpdate(TEntity entity)
        {
            if (BeforeUpdate != null)
                BeforeUpdate(entity, Validator);
        }

        protected virtual void Validate(TEntity entity, string ruleSet, bool cascade)
        {
            try
            {
                Validator.ValidateAndThrow(entity, ruleSet, cascade);
            }
            catch
            {
                Repository.ClearCache(entity);
                throw;
            }
        }

        protected new event BeforeHandler BeforeAdd;

        protected new event BeforeHandler BeforeDelete;

        protected new event BeforeHandler BeforeUpdate;

        protected delegate void BeforeHandler(TEntity entity, IEntityValidator<TEntity, TKey> validator);
    }
}