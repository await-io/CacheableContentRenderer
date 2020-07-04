using AwaitIO.CacheableContent.Interfaces.Entities;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.Interfaces
{
    public interface ICacheableKeyFragmentFactory
    {

        /// <summary>
        /// Tries to create a fragment of the cache key.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        string CreateKeyFragment(HtmlHelper htmlHelper, IContent content, ICacheableSettings cacheableSettings);

        /// <summary>
        /// Gets the supported vary by.
        /// </summary>
        /// <value>
        /// The supported vary by.
        /// </value>
        string SupportedVaryBy { get; }
    }
}
