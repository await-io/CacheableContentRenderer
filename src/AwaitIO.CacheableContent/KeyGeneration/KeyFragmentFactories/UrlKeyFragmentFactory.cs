using AwaitIO.CacheableContent.Interfaces;
using AwaitIO.CacheableContent.Interfaces.Entities;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.KeyGeneration.KeyFragmentFactories
{
    [ServiceConfiguration(typeof(ICacheableKeyFragmentFactory), Lifecycle = ServiceInstanceScope.Singleton)]
    public class UrlKeyFragmentFactory : ICacheableKeyFragmentFactory
    {
        public string SupportedVaryBy => VaryBy.Url;

        public string CreateKeyFragment(HtmlHelper htmlHelper, IContent content, ICacheableSettings cacheableSettings)
        {
            return htmlHelper.ViewContext?.HttpContext?.Request?.Url.AbsoluteUri;
        }
    }
}
