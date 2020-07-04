using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.Interfaces.Services
{
    public interface ICacheableContentRendererService
    {
        /// <summary>
        /// Tries to render the HTML from cache, falls back to the default renderer if the content is not cacheable.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialRequest">The partial request.</param>
        /// <param name="contentData">The content data.</param>
        /// <param name="templateModel">The template model.</param>
        void RenderFromCache(HtmlHelper htmlHelper, PartialRequest partialRequest, IContentData contentData, TemplateModel templateModel);
    }
}