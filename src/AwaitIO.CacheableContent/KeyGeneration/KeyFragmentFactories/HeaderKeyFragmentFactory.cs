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
    public class HeaderKeyFragmentFactory : ICacheableKeyFragmentFactory
    {
        public string SupportedVaryBy => VaryBy.Headers;

        public string CreateKeyFragment(HtmlHelper htmlHelper, IContent content, ICacheableSettings cacheableSettings)
        {
            var headers = htmlHelper.ViewContext?.HttpContext?.Request?.Headers;

            if (cacheableSettings.Parameters.TryGetValue(SupportedVaryBy, out string value) && headers != null)
            {
                var parameters = value.Split(',').Select(p => p.Trim());
                if (parameters.FirstOrDefault() == "*")
                {
                    return string.Join(",", GetHeadersParameters(headers.AllKeys));
                }

                return string.Join(",", GetHeadersParameters(parameters));
            }

            return string.Empty;

            IEnumerable<string> GetHeadersParameters(IEnumerable<string> keys)
            {
                foreach (var key in keys)
                {
                    yield return $"{key}:{headers.Get(key)}";
                }
            }
        }
    }
}
