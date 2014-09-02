using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Enterprise.Core.Web.Html
{
    public static class HtmlHelperExtensions
    {
        public static ConditionalTag Conditional(this HtmlHelper helper, string tag, Func<bool> openCondition, Func<bool> closeCondition,
                                         object htmlAttributes)
        {
            return Conditional(helper, tag, openCondition, closeCondition, new RouteValueDictionary(htmlAttributes));
        }

        public static ConditionalTag Conditional(this HtmlHelper helper, string tag, Func<bool> openCondition, Func<bool> closeCondition,
                                                 IDictionary<string, object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder(tag);
            tagBuilder.MergeAttributes(htmlAttributes);
            return new ConditionalTag(helper.ViewContext, tagBuilder, openCondition, closeCondition);
        }

        public static ConditionalTag Conditional(this HtmlHelper helper, string tag, Func<bool> condition,
                                                 IDictionary<string, object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder(tag);
            tagBuilder.MergeAttributes(htmlAttributes);
            return new ConditionalTag(helper.ViewContext, tagBuilder, condition);
        }

        public static ConditionalTag Conditional(this HtmlHelper helper, string tag, Func<bool> condition,
                                                 object htmlAttributes)
        {
            return Conditional(helper, tag, condition, new RouteValueDictionary(htmlAttributes));
        }
    }
}
