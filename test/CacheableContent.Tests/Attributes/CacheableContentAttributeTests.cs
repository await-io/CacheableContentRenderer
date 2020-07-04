using AwaitIO.CacheableContent;
using AwaitIO.CacheableContent.Attributes;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CacheableContent.Tests.Attributes
{
    public class CacheableContentAttributeTests
    {
        [Fact]
        public void Headers_SetHeaders_HeadersAreAddedToParameters()
        {
            var attribute = new CacheableContentAttribute
            {
                Headers = "*"
            };

            attribute.Parameters.Should().Contain(VaryBy.Headers, "*", "Because the header property should store it's values ín Parameters");
        }

        [Fact]
        public void Cookies_SetCookies_CookiesAreAddedToParameters()
        {
            var attribute = new CacheableContentAttribute
            {
                Cookies = "*"
            };

            attribute.Parameters.Should().Contain(VaryBy.Cookies, "*", "Because the Cookies property should store it's values ín Parameters");
        }

        [Fact]
        public void Query_SetQuery_QueryAreAddedToParameters()
        {
            var attribute = new CacheableContentAttribute
            {
                Query = "*"
            };

            attribute.Parameters.Should().Contain(VaryBy.Query, "*", "Because the Query property should store it's values ín Parameters");
        }

        [Fact]
        public void VisitorGroups_SetVisitorGroups_VisitorGroupsAreAddedToParameters()
        {
            var attribute = new CacheableContentAttribute
            {
                VisitorGroups = "*"
            };

            attribute.Parameters.Should().Contain(VaryBy.VisitorGroups, "*", "Because the VisitorGroups property should store it's values ín Parameters");
        }
    }
}
