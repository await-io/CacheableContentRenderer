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
using System.Web.Mvc;
using Xunit;

namespace CacheableContent.Tests.KeyFragmentFactories
{
    public class UrlKeyFragmentFactoryTests
    {
        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromUrl_ReturnStringWithUrlValue(IContent content, ICacheableSettings cacheableSettings, Uri url, IViewDataContainer viewDataContainer)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            A.CallTo(() => htmlHelper.ViewContext.HttpContext.Request.Url).Returns(url);
            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.Url });

            var urlKeyFragmentFactory = new UrlKeyFragmentFactory();
            urlKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be(url.AbsoluteUri, "Because the returned value should be the absolute url");
        }
    }
}
