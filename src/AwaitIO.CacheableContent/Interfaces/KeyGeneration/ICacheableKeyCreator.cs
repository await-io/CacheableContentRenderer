using AwaitIO.CacheableContent.Interfaces.Entities;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.Interfaces
{
    public interface ICacheableKeyCreator
    {
        /// <summary>
        /// Generates the cache key.
        /// </summary>
        /// <param name="cacheableSettings">The cacheable settings.</param>
        /// <param name="helper">The helper.</param>
        /// <param name="content">The content.</param>
        /// <returns>
        /// the cache key for the Cacheable object
        /// </returns>
        string GenerateCacheKey(ICacheableSettings cacheableSettings, HtmlHelper helper, IContent content);

        /// <summary>
        /// Gets the root cache key.
        /// everything in the content renderer cache has a dependency on this key
        /// </summary>
        /// <value>
        /// The root cache key.
        /// </value>
        string RootCacheKey { get; }
    }
}
