using AutoFixture.Xunit2;
using AwaitIO.CacheableContent;
using AwaitIO.CacheableContent.Interfaces.Entities;
using AwaitIO.CacheableContent.KeyGeneration.KeyFragmentFactories;
using CacheableContent.Tests.Infrastructure.Attributes;
using EPiServer.Core;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;
using CacheableContent.Tests.TestExtensions;

namespace CacheableContent.Tests.KeyFragmentFactories
{
    public class CookieKeyFragmentFactoryTests
    {
        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromSpecificCookie_ReturnStringWithCookieValue(IContent content, ICacheableSettings cacheableSettings, IEnumerable<HttpCookie> cookies, HttpCookie varyByCookie, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake< ViewContext>(), viewDataContainer);
            htmlHelper.AddCookies(cookies, varyByCookie);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Cookies });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.Cookies, varyByCookie.Name } });
            
            var cookieKeyFragmentFactory = new CookieKeyFragmentFactory();
            cookieKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be($"{varyByCookie.Name}:{varyByCookie.Value}", "Because the returned value should be the cookie name and value");
        }

        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromAllCookie_ReturnStringWithCookieValue(IContent content, ICacheableSettings cacheableSettings, IEnumerable<HttpCookie> cookies, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            htmlHelper.AddCookies(cookies);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Cookies });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.Cookies, "*" } });

            var cookieKeyFragmentFactory = new CookieKeyFragmentFactory();
            var expectedResult = string.Join(",", cookies.Select(cookie => $"{cookie.Name}:{cookie.Value}"));

            cookieKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be(expectedResult, "Because the returned value should contain all the cookies names and values");
        }
    }
}
