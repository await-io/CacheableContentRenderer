using Episerver.CacheableContentRenderer.Interfaces.Services;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Episerver.CacheableContentRenderer.Services
{
    [ServiceConfiguration(typeof(IVisitorGroupService), Lifecycle = ServiceInstanceScope.Singleton)]
    public class VisitorGroupService : IVisitorGroupService
    {
        public VisitorGroupService(IVisitorGroupRoleRepository visitorGroupRoleRepository)
        {
            VisitorGroupHelper = new VisitorGroupHelper(visitorGroupRoleRepository);
        }

        private VisitorGroupHelper VisitorGroupHelper { get; }

        public bool IsUserInVisitorGroup(IPrincipal principal, string virtualRole, HttpContextBase httpContext)
        {
            if(httpContext is null)
            {
                return false;
            }

            return VisitorGroupHelper.IsPrincipalInGroup(principal, virtualRole, httpContext);
        }

        public bool IsUserInVisitorGroup(IPrincipal principal, string virtualRole)
        {
            return VisitorGroupHelper.IsPrincipalInGroup(principal, virtualRole);
        }
    }
}
