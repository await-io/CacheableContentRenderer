using AwaitIO.CacheableContent.Interfaces;
using AwaitIO.CacheableContent.Interfaces.Services;
using AwaitIO.CacheableContent.Renderer;
using AwaitIO.CacheableContent.Services;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Cache;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwaitIO.CacheableContent.Initialization
{
    [InitializableModule]
    public class InitializationModule : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += Context_ConfigurationComplete;
        }

        private void Context_ConfigurationComplete(object sender, ServiceConfigurationEventArgs e)
        {
            e.Services.Intercept<IContentRenderer>((locator, contentRender) => new CacheContentRenderer(ContentRendererFactory(locator, contentRender)));
        }

        public void Initialize(InitializationEngine context)
        {
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        private ICacheableContentRendererService ContentRendererFactory(IServiceLocator serviceLocator, IContentRenderer contentRenderer)
        {
            return new CacheableContentRendererService(
                contentRenderer,
                serviceLocator.GetInstance<ICacheableSettingsService>(),
                serviceLocator.GetInstance<ISynchronizedObjectInstanceCache>(),
                serviceLocator.GetInstance<ICacheableKeyCreator>(),
                serviceLocator.GetInstance<IContentCacheKeyCreator>());
        }
    }
}
