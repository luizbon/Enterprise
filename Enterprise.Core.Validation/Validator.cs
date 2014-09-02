using System;
using System.Collections.Generic;
using System.Linq;
using Enterprise.Core.Model;
using Enterprise.Core.Validation.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Enterprise.Core.Validation
{
    public abstract class Validator<TEntity, TKey> : AbstractValidator<TEntity>, IEntityValidator<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        private Action _validationMethod;

        protected Validator()
        {
            SetupValidations();
        }

        public void ValidateAndThrow(TEntity instance)
        {
            ValidationResult validationResult = Validate(instance);
            if (!validationResult.IsValid)
                throw new ValidationException(Map(validationResult.Errors));
        }

        public void ValidateAndThrow(TEntity instance, string ruleset)
        {
            ValidationResult validationResult = Validate(instance, ruleset);
            if (!validationResult.IsValid)
                throw new ValidationException(Map(validationResult.Errors));
        }

        public void ValidateAndThrow(TEntity instance, string ruleset, bool cascade)
        {
            ValidateAndThrow(instance, ruleset);

            if (!cascade)
                return;

            ValidationResult validationResult = Validate(instance);
            if (!validationResult.IsValid)
                throw new ValidationException(Map(validationResult.Errors));
        }

        private void SetupValidations()
        {
            _validationMethod = () =>
            {
                CommonValidations();
                RuleSet(RuleSets.Create, ValidateOnCreate);
                RuleSet(RuleSets.Update, ValidateOnUpdate);
                RuleSet(RuleSets.Delete, ValidateOnDelete);
            };
        }

        protected abstract void CommonValidations();

        protected virtual void ValidateOnUpdate()
        {
        }

        protected virtual void ValidateOnCreate()
        {
        }

        protected virtual void ValidateOnDelete()
        {
        }

        public override ValidationResult Validate(TEntity instance)
        {
            ConfigureValidations();
            return base.Validate(instance);
        }

        public ValidationResult Validate(TEntity instance, string ruleset)
        {
            ConfigureValidations();
            return this.Validate(instance, ruleSet: ruleset);
        }

        protected void ConfigureValidations()
        {
            if (_validationMethod == null) return;

            _validationMethod();
            _validationMethod = null;
        }

        private static IEnumerable<ValidationFailure> Map(IEnumerable<ValidationFailure> source)
        {
            return
                source.Select(
                    validationFailure =>
                        new ValidationFailure(validationFailure.PropertyName, validationFailure.ErrorMessage,
                            validationFailure.AttemptedValue) {CustomState = validationFailure.CustomState});
        }
    }
}