using AwaitIO.CacheableContent.Interfaces;
using AwaitIO.CacheableContent.Interfaces.Entities;
using Episerver.CacheableContentRenderer.Interfaces.Services;
using EPiServer.Core;
using EPiServer.Personalization.VisitorGroups;
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
    public class VisitorGroupKeyFragmentFactory : ICacheableKeyFragmentFactory
    {
        public VisitorGroupKeyFragmentFactory(IVisitorGroupRepository visitorGroupRepository, IVisitorGroupService visitorGroupService)
        {
            VisitorGroupRepository = visitorGroupRepository ?? throw new ArgumentNullException(nameof(visitorGroupRepository));
            VisitorGroupService = visitorGroupService ?? throw new ArgumentNullException(nameof(visitorGroupService));
        }

        public string SupportedVaryBy => VaryBy.VisitorGroups;

        private IVisitorGroupRepository VisitorGroupRepository { get; }

        private IVisitorGroupService VisitorGroupService { get; }

        public string CreateKeyFragment(HtmlHelper htmlHelper, IContent content, ICacheableSettings cacheableSettings)
        {
            
            var user = htmlHelper.ViewContext?.HttpContext?.User;
            if (cacheableSettings.Parameters.TryGetValue(SupportedVaryBy, out string value) && user != null)
            {
                var parameters = value.Split(',').Select(p => p.Trim());
                if (parameters.FirstOrDefault() == "*")
                {
                    return string.Join(",", GetActiveRoles(VisitorGroupRepository.List().Select(vg => vg.Name)));
                }

                return string.Join(",", GetActiveRoles(parameters));
            }

            return string.Empty;

            IEnumerable<string> GetActiveRoles(IEnumerable<string> roles)
            {
                foreach (var role in roles.Where(r => VisitorGroupService.IsUserInVisitorGroup(user, r, htmlHelper.ViewContext.HttpContext)))
                {
                    yield return role;
                }
            }
        }
    }
}
