using AwaitIO.CacheableContent.Interfaces;
using AwaitIO.CacheableContent.Interfaces.Entities;
using AwaitIO.CacheableContent.Interfaces.Services;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Editor;
using EPiServer.Framework.Cache;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.Services
{
    public class CacheableContentRendererService : ICacheableContentRendererService
    {
        public CacheableContentRendererService(
            IContentRenderer defaultRenderer, 
            ICacheableSettingsService settingsService, 
            ISynchronizedObjectInstanceCache cache, 
            ICacheableKeyCreator keyCreator,
            IContentCacheKeyCreator contentCacheKeyCreator)
        {
            DefaultRenderer = defaultRenderer ?? throw new ArgumentNullException(nameof(defaultRenderer));
            SettingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            Cache = cache ?? throw new ArgumentNullException(nameof(cache));
            KeyCreator = keyCreator ?? throw new ArgumentNullException(nameof(keyCreator));
            ContentCacheKeyCreator = contentCacheKeyCreator ?? throw new ArgumentNullException(nameof(contentCacheKeyCreator));
            MasterCacheKeys = new[] { ContentCacheKeyCreator.VersionKey };
        }

        private IContentRenderer DefaultRenderer { get; }
        private ICacheableSettingsService SettingsService { get; }
        private ISynchronizedObjectInstanceCache Cache { get; }
        private ICacheableKeyCreator KeyCreator { get; }
        private IContentCacheKeyCreator ContentCacheKeyCreator { get; }

        /// <summary>
        /// Gets the master cache key.
        /// </summary>
        /// <value>
        /// The master cache key.
        /// </value>
        protected virtual IEnumerable<string> MasterCacheKeys { get; }

        /// <summary>
        /// Tries to render the HTML from cache, falls back to the default renderer if the content is not cacheable.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialRequest">The partial request.</param>
        /// <param name="contentData">The content data.</param>
        /// <param name="templateModel">The template model.</param>
        public void RenderFromCache(HtmlHelper htmlHelper, PartialRequest partialRequest, IContentData contentData, TemplateModel templateModel)
        {
            if(SettingsService.TryGetSettingsFromContentData(contentData, out ICacheableSettings settings) 
                && IsCacheEnabled()
                && contentData is IContent content)
            {
                RenderFromOrAddToCache(htmlHelper, partialRequest, content, templateModel, settings);
            }
            else
            {
                DefaultRenderer.Render(htmlHelper, partialRequest, contentData, templateModel);
            }

            bool IsCacheEnabled()
            {
                if(PageEditing.PageIsInEditMode)
                {
                    return settings.IsCacheEnabled && settings.CacheInCmsEditor;
                }
                return settings.IsCacheEnabled;
            }
        }

        /// <summary>
        /// Renders from the cache or adds the result to the cache.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="partialRequest">The partial request.</param>
        /// <param name="content">The content.</param>
        /// <param name="templateModel">The template model.</param>
        /// <param name="settings">The settings.</param>
        protected virtual void RenderFromOrAddToCache(HtmlHelper helper, PartialRequest partialRequest, IContent content, TemplateModel templateModel, ICacheableSettings settings)
        {
            string cacheKey = KeyCreator.GenerateCacheKey(settings, helper, content);

            if(Cache.TryGet(cacheKey, ReadStrategy.Immediate, out string cachedHtml))
            {
                helper.ViewContext.Writer.Write(cachedHtml);
            }
            else
            {
                RenderAndAddToCache(helper, partialRequest, content, templateModel, settings, cacheKey);
            }
        }

        /// <summary>
        /// Render the html and adds it to the cache.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="partialRequest">The partial request.</param>
        /// <param name="content">The content.</param>
        /// <param name="templateModel">The template model.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="cacheKey">The cache key.</param>
        protected virtual void RenderAndAddToCache(HtmlHelper helper, PartialRequest partialRequest, IContent content, TemplateModel templateModel, ICacheableSettings settings, string cacheKey)
        {
            using (var cacheWriter = new StringWriter())
            {
                var currentWriter = helper.ViewContext.Writer;
                helper.ViewContext.Writer = cacheWriter;
                DefaultRenderer.Render(helper, partialRequest, content, templateModel);
                var html = cacheWriter.ToString();
                currentWriter.Write(html);
                helper.ViewContext.Writer = currentWriter;

                if (helper.ViewContext.HttpContext.Error == null)
                {
                    InsertToCache(html);
                }
            }

            void InsertToCache(string html)
            {
                if(settings.MaxTimeInCache == TimeSpan.Zero)
                {
                    Cache.Insert(cacheKey, html, new CacheEvictionPolicy(null, MasterCacheKeys));
                }
                else
                {
                    Cache.Insert(cacheKey, html, new CacheEvictionPolicy(settings.MaxTimeInCache, CacheTimeoutType.Absolute, null, MasterCacheKeys));
                }
            }
        }
    }
}