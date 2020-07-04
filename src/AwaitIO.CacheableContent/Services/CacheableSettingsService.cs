using AwaitIO.CacheableContent.Attributes;
using AwaitIO.CacheableContent.Interfaces.Entities;
using AwaitIO.CacheableContent.Interfaces.Services;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AwaitIO.CacheableContent.Services
{
    [ServiceConfiguration(typeof(ICacheableSettingsService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class CacheableSettingsService : ICacheableSettingsService
    {
        public bool TryGetSettingsFromContentData(IContentData contentData, out ICacheableSettings cacheableSettings)
        {
            if (contentData is ICacheableSettings settings && settings.IsCacheSettingEnabled)
            {
                cacheableSettings = settings;
            }
            else
            {
                cacheableSettings = contentData.GetOriginalType().GetCustomAttribute<CacheableContentAttribute>(true);
            }
            
            return cacheableSettings != null;
        }
    }
}
