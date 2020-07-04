using AwaitIO.CacheableContent.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace AwaitIO.CacheableContent.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class CacheableContentAttribute : Attribute, ICacheableSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheableContentAttribute"/> class.
        /// </summary>
        public CacheableContentAttribute() : this(TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheableContentAttribute"/> class.
        /// </summary>
        /// <param name="maxTimeInCache">The maximum time in cache.</param>
        private CacheableContentAttribute(TimeSpan maxTimeInCache)
        {

            _varyBy = new HashSet<string>();
            MaxTimeInCache = maxTimeInCache;
            IsCacheEnabled = true;
            IsCacheSettingEnabled = true;
        }

        /// <summary>
        /// Gets the vary by's for the cacheable.
        /// </summary>
        /// <value>
        /// The vary by.
        /// </value>
        private readonly HashSet<string> _varyBy;
        public IEnumerable<string> VaryBy => _varyBy;

        /// <summary>
        /// Gets the maximum time the cacheable is allowed to be in the cache before it must be evicted.
        /// </summary>
        /// <value>
        /// The maximum time in cache.
        /// </value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan MaxTimeInCache { get; private set; }

        public string MaxTimeInCacheString { get => MaxTimeInCache.ToString(); set => MaxTimeInCache = TimeSpan.Parse(value); }

        /// <summary>
        /// Gets a value indicating whether the Cacheable should be cached.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the cacheable can be cached; otherwise, <c>false</c>.
        /// </value>
        public bool IsCacheEnabled { get; set; }

        /// <summary>
        /// An comma separated list of header values that should be used for VaryBy.Headers, * means all
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public string Headers { get => GetValueFromParameters(CacheableContent.VaryBy.Headers); set => SetParameterValue(CacheableContent.VaryBy.Headers, value); }

        /// <summary>
        /// An comma separated list of Cookies values that should be used for VaryBy.Cookies, * means all
        /// </summary>
        /// <value>
        /// The cookies.
        /// </value>
        public string Cookies { get => GetValueFromParameters(CacheableContent.VaryBy.Cookies); set => SetParameterValue(CacheableContent.VaryBy.Cookies, value); }

        /// <summary>
        /// An comma separated list of Query values that should be used for VaryBy.Query, * means all
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        public string Query { get => GetValueFromParameters(CacheableContent.VaryBy.Query); set => SetParameterValue(CacheableContent.VaryBy.Query, value); }

        public bool CacheInCmsEditor { get; set; }

        /// <summary>
        /// An comma separated list of Visitor Groups values that should be used for VaryBy.VisitorGroups, * means all
        /// </summary>
        /// <value>
        /// The Visitor Groups.
        /// </value>
        public string VisitorGroups { get => GetValueFromParameters(CacheableContent.VaryBy.VisitorGroups); set => SetParameterValue(CacheableContent.VaryBy.VisitorGroups, value); }

        /// <summary>
        /// Gets the value from parameters or null if the key doesnt exist.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected string GetValueFromParameters(string key)
        {
            Parameters.TryGetValue(key, out string value);
            return value;
        }

        protected void SetParameterValue(string key, string value)
        {
            Parameters[key] = value;
            _varyBy.Add(key);
        }

        public IDictionary<string, string> Parameters { get; } = new Dictionary<string, string>();

        public bool IsCacheSettingEnabled { get; }
    }
}