using System;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using Enterprise.Core.Model;
using Enterprise.Core.Service.Interfaces;
using Enterprise.Core.Web.Extensions;
using Enterprise.Core.Web.Resources;
using Enterprise.Core.Web.ViewModels;
using FluentValidation;
using IPageFilter = Enterprise.Core.Web.Filters.Interfaces.IPageFilter;

namespace Enterprise.Core.Web.Controllers
{
    public abstract class CrudController<TEntity, TViewModel, TFilter, TKey> : BaseController
        where TEntity : Entity<TKey>
        where TViewModel : EntityViewModel<TKey>, new()
        where TFilter : IPageFilter
    {
        protected CrudController(IService<TEntity, TKey> service)
        {
            Service = service;
        }

        protected IService<TEntity, TKey> Service { get; private set; }

        protected event EntityHandler Success;

        protected event EntityHandler EntityAdd;

        protected virtual void OnSuccess(TEntity entity)
        {
            if (Success != null) Success(entity);
        }

        protected virtual void OnEntityAdd(TEntity entity)
        {
            if (EntityAdd != null) EntityAdd(entity);
        }

        protected event EntityHandler EntityUpdate;

        protected virtual void OnEntityUpdate(TEntity entity)
        {
            if (EntityUpdate != null) EntityUpdate(entity);
        }

        public virtual ActionResult Index(TFilter filter)
        {
            if (filter.Export)
                return Report(filter);

            var indexViewModel = new IndexViewModel<TViewModel, TKey>
            {
                Filter = filter,
                Items = Service.GetByFilter(filter).ToViewModel<TViewModel, TEntity, TKey>()
            };

            if (Request.IsAjaxRequest() || filter.IsPartial)
                return PartialView("Grid", indexViewModel);

            return View(indexViewModel);
        }

        public virtual ActionResult Report(TFilter filter)
        {
            var serializer = new XmlSerializer(typeof (TEntity));

            var sw = new StringWriter();

            serializer.Serialize(sw, Service.GetForReport(filter));

            return Content(sw.ToString(), "application/xml");
        }

        public virtual ActionResult Create()
        {
            OnEntityAdd(null);
            if (Request != null && Request.IsAjaxRequest())
                return PartialView(CreateViewModel(null));
            return View(CreateViewModel(null));
        }

        [HttpPost]
        public virtual ActionResult Create(TViewModel model)
        {
            if (ModelState.IsValid)
            {
                TEntity entity = model.ToEntity<TEntity, TKey>();
                try
                {
                    Service.Add(entity);

                    Flash.Success(Messages.RecordCreated);

                    OnSuccess(entity);

                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    IncludeValidationErrorsInModelState(ex);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            OnEntityAdd(model.ToEntity<TEntity, TKey>());
            CreateViewModel(model);
            return View(model);
        }

        public virtual ActionResult Edit(TKey id)
        {
            TEntity entity = Service.Get(id);
            OnEntityUpdate(entity);
            TViewModel viewModel = CreateViewModel(entity.ToViewModel<TViewModel, TKey>());
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Edit(TViewModel model)
        {
            if (ModelState.IsValid)
            {
                TEntity entity = model.ToEntity<TEntity, TKey>();
                OnEntityUpdate(entity);
                try
                {
                    Service.Update(entity);

                    Flash.Success(Messages.RecordUpdated);

                    OnSuccess(entity);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            CreateViewModel(model);
            return View(model);
        }

        public virtual ActionResult Details(TKey id)
        {
            TViewModel model = Service.Get(id).ToViewModel<TViewModel, TKey>();

            if (Request.IsAjaxRequest())
                return PartialView("DetailsModal", model);

            return View(model);
        }

        public virtual ActionResult Delete(TKey id, TFilter filter)
        {
            try
            {
                Service.Delete(id);

                Flash.Success(Messages.RecordDeleted);
            }
            catch (Exception ex)
            {
                Flash.Error(ex.Message);
            }

            return RedirectToAction("Index", filter);
        }

        protected virtual TViewModel CreateViewModel(TViewModel viewModel)
        {
            return viewModel ?? new TViewModel();
        }

        protected virtual void IncludeValidationErrorsInModelState(ValidationException ex)
        {
            foreach (var validatorFailure in ex.Errors)
            {
                ModelState.AddModelError(validatorFailure.PropertyName, validatorFailure.ErrorMessage);
            }
        }

        protected delegate void EntityHandler(TEntity entity);
    }
}