using AwaitIO.CacheableContent;
using AwaitIO.CacheableContent.Interfaces.Entities;
using AwaitIO.CacheableContent.KeyGeneration.KeyFragmentFactories;
using CacheableContent.Tests.Infrastructure.Attributes;
using EPiServer.Core;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xunit;

namespace CacheableContent.Tests.KeyFragmentFactories
{
    public class HeaderKeyFragmentFactoryTests
    {
        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromSpecificHeader_ReturnStringWithHeaderValue(IContent content, ICacheableSettings cacheableSettings, IEnumerable<(string name, string value)> headers, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            var (headerName, headerValue) = headers.First();
            AddHeaders(htmlHelper, headers);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Headers });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.Headers, headerName } });

            var headerKeyFragmentFactory = new HeaderKeyFragmentFactory();
            headerKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be($"{headerName}:{headerValue}", "Because the returned value should be the specific header name and value");
        }

        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromAllHeaders_ReturnStringWithAllHeaderValues(IContent content, ICacheableSettings cacheableSettings, IEnumerable<(string name, string value)> headers, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            AddHeaders(htmlHelper, headers);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Headers });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.Headers, "*" } });

            var headerKeyFragmentFactory = new HeaderKeyFragmentFactory();
            var expectedResult = string.Join(",", headers.Select(header => $"{header.name}:{header.value}"));
            headerKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be(expectedResult, "Because the returned value should be the specific header name and value");
        }

        private static void AddHeaders(HtmlHelper htmlHelper, IEnumerable<(string name, string value)> headers)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            
            foreach (var (name, value) in headers)
            {
                nameValueCollection.Add(name, value);
            }
            A.CallTo(() => htmlHelper.ViewContext.HttpContext.Request.Headers).Returns(nameValueCollection);
        }
    }
}
