using AwaitIO.CacheableContent.Interfaces.Services;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.Renderer
{
    public class CacheContentRenderer : IContentRenderer
    {
        public CacheContentRenderer(ICacheableContentRendererService cacheableContentRendererService)
        {
            CacheableContentRendererService = cacheableContentRendererService;
        }

        public ICacheableContentRendererService CacheableContentRendererService { get; }

        public void Render(HtmlHelper helper, PartialRequest partialRequestHandler, IContentData contentData, TemplateModel templateModel)
        {
            CacheableContentRendererService.RenderFromCache(helper, partialRequestHandler, contentData, templateModel);
        }
    }
}
