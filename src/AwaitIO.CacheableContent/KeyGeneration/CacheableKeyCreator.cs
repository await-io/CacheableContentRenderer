using AwaitIO.CacheableContent.Interfaces;
using AwaitIO.CacheableContent.Interfaces.Entities;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AwaitIO.CacheableContent.KeyGeneration
{
    [ServiceConfiguration(typeof(ICacheableKeyCreator), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CacheableKeyCreator : ICacheableKeyCreator
    {
        private readonly IReadOnlyDictionary<string, ICacheableKeyFragmentFactory> _keyFragmentFactories;

        public CacheableKeyCreator(IEnumerable<ICacheableKeyFragmentFactory> keyFragmentFactories, IContentCacheKeyCreator contentCacheKeyCreator)
        {
            var factories = keyFragmentFactories?.ToDictionary(x => x.SupportedVaryBy, StringComparer.OrdinalIgnoreCase) ?? throw new ArgumentNullException(nameof(keyFragmentFactories));
            _keyFragmentFactories = new ReadOnlyDictionary<string, ICacheableKeyFragmentFactory>(factories);
            ContentCacheKeyCreator = contentCacheKeyCreator;
        }

        public string RootCacheKey { get; } = "668FD9F2DB51402DBB52B0CA8F194F81";

        private IContentCacheKeyCreator ContentCacheKeyCreator { get; }

        public string GenerateCacheKey(ICacheableSettings cacheableSettings, HtmlHelper helper, IContent content)
        {
            return string.Join("&", CreateKeyFragments());

            IEnumerable<string> CreateKeyFragments()
            {
                yield return "rootKey=" + ContentCacheKeyCreator.CreateLanguageCacheKey(content.ContentLink, (content as ILocalizable)?.Language.Name);
                foreach (string varyBy in cacheableSettings.VaryBy.OrderBy(v => v, StringComparer.OrdinalIgnoreCase))
                {
                    if (_keyFragmentFactories.TryGetValue(varyBy, out var factory))
                    {
                        yield return factory.SupportedVaryBy + "=" + factory.CreateKeyFragment(helper, content, cacheableSettings);
                    }
                }
            }
        }
    }
}
