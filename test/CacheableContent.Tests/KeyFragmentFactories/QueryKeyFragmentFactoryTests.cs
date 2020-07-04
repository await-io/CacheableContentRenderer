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
using System.Web.Mvc;
using Xunit;

namespace CacheableContent.Tests.KeyFragmentFactories
{
    public class QueryKeyFragmentFactoryTests
    {
        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromSpecificQueryParameter_ReturnStringWithQueryParameterValue(IContent content, ICacheableSettings cacheableSettings, IEnumerable<(string name, string value)> queryParameters, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            var (name, value) = queryParameters.First();
            AddQueryParameters(htmlHelper, queryParameters);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Query });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.Query, name } });

            var queryKeyFragmentFactory = new QueryKeyFragmentFactory();
            queryKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be($"{name}:{value}", "Because the returned value should be the specific query parameter name and value");
        }

        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromAllQueryParameter_ReturnStringWithAllQueryParameterValues(IContent content, ICacheableSettings cacheableSettings, IEnumerable<(string name, string value)> queryParameters, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            AddQueryParameters(htmlHelper, queryParameters);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Query });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.Query, "*" } });

            var queryKeyFragmentFactory = new QueryKeyFragmentFactory();
            var expectedResult = string.Join(",", queryParameters.Select(queryParam => $"{queryParam.name}:{queryParam.value}"));
            queryKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be(expectedResult, "Because the returned value should be the specific query parameter name and value");
        }

        private static void AddQueryParameters(HtmlHelper htmlHelper, IEnumerable<(string name, string value)> queryParameters)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();

            foreach (var (name, value) in queryParameters)
            {
                nameValueCollection.Add(name, value);
            }
            A.CallTo(() => htmlHelper.ViewContext.HttpContext.Request.QueryString).Returns(nameValueCollection);
        }
    }
}
