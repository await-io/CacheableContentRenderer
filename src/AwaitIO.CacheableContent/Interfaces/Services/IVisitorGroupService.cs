using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Episerver.CacheableContentRenderer.Interfaces.Services
{
    public interface IVisitorGroupService
    {
        bool IsUserInVisitorGroup(IPrincipal principal, string virtualRole, HttpContextBase httpContext);
        
        bool IsUserInVisitorGroup(IPrincipal principal, string virtualRole);
    }
}
