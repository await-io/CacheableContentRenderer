using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CacheableContent.Tests.TestExtensions
{
    public static class CookieExtensions
    {
        public static void AddCookies(this HtmlHelper htmlHelper, IEnumerable<HttpCookie> httpCookies, params HttpCookie[] additionalCookies)
        {
            var cookies = httpCookies.Concat(additionalCookies ?? Enumerable.Empty<HttpCookie>());

            foreach (var cookie in cookies)
            {
                htmlHelper.ViewContext.HttpContext.Request.Cookies.Add(cookie);
            }
        }
    }
}
