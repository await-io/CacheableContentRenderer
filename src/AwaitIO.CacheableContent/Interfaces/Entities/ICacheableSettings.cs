using System;
using System.Collections.Generic;
using System.Text;

namespace AwaitIO.CacheableContent.Interfaces.Entities
{
    public interface ICacheableSettings
    {
        /// <summary>
        /// Gets a value indicating whether this instance is enabled, 
        /// used to fallback to the attribute config when using content to configure the caching
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cache setting enabled; otherwise, <c>false</c>.
        ///</value>
        bool IsCacheSettingEnabled { get; }
        /// <summary>
        /// Gets the vary by's for the cacheable.
        /// </summary>
        /// <value>
        /// The vary by.
        /// </value>
        IEnumerable<string> VaryBy { get; }

        /// <summary>
        /// Gets the maximum time the cacheable is allowed to be in the cache before it must be evicted.
        /// </summary>
        /// <value>
        /// The maximum time in cache.
        /// </value>
        TimeSpan MaxTimeInCache { get; }

        /// <summary>
        /// Gets a value indicating whether the Cacheable should be cached.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the cacheable can be cached; otherwise, <c>false</c>.
        /// </value>
        bool IsCacheEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether the Cacheable should be cached in the editor
        /// </summary>
        bool CacheInCmsEditor { get; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        IDictionary<string, string> Parameters { get; }

    }
}
