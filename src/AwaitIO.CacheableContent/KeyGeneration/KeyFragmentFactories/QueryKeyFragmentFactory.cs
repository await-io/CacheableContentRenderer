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
    public class QueryKeyFragmentFactory : ICacheableKeyFragmentFactory
    {
        public string SupportedVaryBy => VaryBy.Query;

        public string CreateKeyFragment(HtmlHelper htmlHelper, IContent content, ICacheableSettings cacheableSettings)
        {
            var query = htmlHelper.ViewContext?.HttpContext?.Request?.QueryString;
            
            if(cacheableSettings.Parameters.TryGetValue(SupportedVaryBy, out string value) && query != null)
            {
                var parameters = value.Split(',').Select(p => p.Trim());
                if(parameters.FirstOrDefault() == "*")
                {
                    return string.Join(",", GetQueryParameters(query.AllKeys));
                }

                return string.Join(",", GetQueryParameters(parameters));
            }

            return string.Empty;

            IEnumerable<string> GetQueryParameters(IEnumerable<string> keys)
            {
                foreach (var key in keys)
                {
                   yield return $"{key}:{query.Get(key)}";
                }
            }
        }
    }
}
