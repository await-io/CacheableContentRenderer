using AwaitIO.CacheableContent.Interfaces.Entities;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwaitIO.CacheableContent.Interfaces.Services
{
    public interface ICacheableSettingsService
    {
        /// <summary>
        /// Tries to the get settings from content data.
        /// </summary>
        /// <param name="contentData">The content data.</param>
        /// <param name="cacheableSettings">The cacheable settings.</param>
        /// <returns>true if cacheable settings can be found, else false</returns>
        bool TryGetSettingsFromContentData(IContentData contentData, out ICacheableSettings cacheableSettings);
    }
}
