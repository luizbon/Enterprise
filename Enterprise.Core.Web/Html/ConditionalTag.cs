using System;
using System.Web.Mvc;

namespace Enterprise.Core.Web.Html
{
    public class ConditionalTag : IDisposable
    {
        private readonly ViewContext _viewContext;
        private readonly TagBuilder _tag;
        private readonly Func<bool> _openCondition;
        private readonly Func<bool> _closeCondition;
        private bool _disposed;

        public ConditionalTag(ViewContext viewContext, TagBuilder tag, Func<bool> condition)
            : this(viewContext, tag, condition, condition)
        {
        }

        public ConditionalTag(ViewContext viewContext, TagBuilder tag, Func<bool> openCondition,
                              Func<bool> closeCondition)
        {
            _viewContext = viewContext;
            _tag = tag;
            _openCondition = openCondition;
            _closeCondition = closeCondition;

            if (_openCondition())
                viewContext.Writer.Write(tag.ToString(TagRenderMode.StartTag));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;
            if (_openCondition())
                _viewContext.Writer.Write(_tag.ToString(TagRenderMode.EndTag));
        }

        public void CloseTag()
        {
            if (_closeCondition())
                _viewContext.Writer.Write(_tag.ToString(TagRenderMode.EndTag));
        }
    }
}
