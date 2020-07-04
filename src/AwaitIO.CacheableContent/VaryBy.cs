using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwaitIO.CacheableContent
{
    public static class VaryBy
    {
        /// <summary>
        /// Vary by the relvative URL
        /// </summary>
        public const string Url = nameof(Url);

        /// <summary>
        /// Vary by http headers
        /// </summary>
        public const string Headers = nameof(Headers);

        /// <summary>
        /// Vary by query string
        /// </summary>
        public const string Query = nameof(Query);

        /// <summary>
        /// Vary by cookies
        /// </summary>
        public const string Cookies = nameof(Cookies);

        /// <summary>
        /// Vary by visitor groups
        /// </summary>
        public const string VisitorGroups = nameof(VisitorGroups);
    }
}
