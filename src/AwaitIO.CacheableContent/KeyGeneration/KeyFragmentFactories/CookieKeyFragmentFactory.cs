﻿using AwaitIO.CacheableContent.Interfaces;
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
    public class CookieKeyFragmentFactory : ICacheableKeyFragmentFactory
    {
        public string SupportedVaryBy => VaryBy.Cookies;

        public string CreateKeyFragment(HtmlHelper htmlHelper, IContent content, ICacheableSettings cacheableSettings)
        {
            var cookies = htmlHelper.ViewContext?.HttpContext?.Request?.Cookies;

            if (cacheableSettings.Parameters.TryGetValue(SupportedVaryBy, out string value) && cookies != null)
            {
                var parameters = value.Split(',').Select(p => p.Trim());
                if (parameters.FirstOrDefault() == "*")
                {
                    return string.Join(",", GetCookieParameters(cookies.AllKeys));
                }

                return string.Join(",", GetCookieParameters(parameters));
            }

            return string.Empty;

            IEnumerable<string> GetCookieParameters(IEnumerable<string> keys)
            {
                foreach (var key in keys)
                {
                    yield return $"{key}:{cookies.Get(key)?.Value}";
                }
            }
        }
    }
}
