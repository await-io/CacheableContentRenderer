using AutoFixture.Xunit2;
using AwaitIO.CacheableContent;
using AwaitIO.CacheableContent.Interfaces.Entities;
using AwaitIO.CacheableContent.KeyGeneration.KeyFragmentFactories;
using CacheableContent.Tests.Infrastructure.Attributes;
using Episerver.CacheableContentRenderer.Interfaces.Services;
using EPiServer.Core;
using EPiServer.Personalization.VisitorGroups;
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
    public class VisitorGroupKeyFragmentFactoryTests
    {
        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromSpecificVisitorGroup_ReturnStringWithVisitorGroupValue(IContent content, ICacheableSettings cacheableSettings, IViewDataContainer viewDataContainer, [Frozen] IVisitorGroupService visitorGroupService, [Frozen] IVisitorGroupRepository visitorGroupRepository , [Frozen] IEnumerable<VisitorGroup> visitorGroups, VisitorGroupKeyFragmentFactory visitorGroupKeyFragmentFactory)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            var visitorGroup = visitorGroups.First();
            A.CallTo(() => visitorGroupRepository.List()).Returns(visitorGroups);
            A.CallTo(visitorGroupService)
                .Where(call => call.Method.Name == "IsUserInVisitorGroup" && (call.Arguments.ElementAtOrDefault(1) as string) == visitorGroup.Name)
                .WithReturnType<bool>()
                .Returns(true);

            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.VisitorGroups });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.VisitorGroups, string.Join(",",visitorGroups.Select(vg => vg.Name)) } });
            
            visitorGroupKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be($"{visitorGroup.Name}", "Because the returned value should be the specific role name from the visitor group ");
        }

        [Theory]
        [AutoFakeData]
        public void CreateKeyFragment_GenerateKeyFragmentFromAllVisitorGroup_ReturnStringWithAllVisitorGroupValue(IContent content, ICacheableSettings cacheableSettings, IViewDataContainer viewDataContainer, [Frozen] IVisitorGroupService visitorGroupService, [Frozen] IVisitorGroupRepository visitorGroupRepository, [Frozen] IEnumerable<VisitorGroup> visitorGroups, VisitorGroupKeyFragmentFactory visitorGroupKeyFragmentFactory)
        {
            var htmlHelper = new HtmlHelper(A.Fake<ViewContext>(), viewDataContainer);
            A.CallTo(() => visitorGroupRepository.List()).Returns(visitorGroups);
            A.CallTo(visitorGroupService)
                .Where(call => call.Method.Name == "IsUserInVisitorGroup" && visitorGroups.Any(vg => vg.Name == (call.Arguments.ElementAtOrDefault(1) as string)))
                .WithReturnType<bool>()
                .Returns(true);

            A.CallTo(() => cacheableSettings.VaryBy).Returns(new[] { VaryBy.VisitorGroups });
            A.CallTo(() => cacheableSettings.Parameters).Returns(new Dictionary<string, string> { { VaryBy.VisitorGroups, "*" } });

            string expectedResult = string.Join(",", visitorGroups.Select(vg => vg.Name));

            visitorGroupKeyFragmentFactory.CreateKeyFragment(htmlHelper, content, cacheableSettings).Should().Be(expectedResult, "Because the returned value should be the all the visitor groups role names");
        }
    }
}
