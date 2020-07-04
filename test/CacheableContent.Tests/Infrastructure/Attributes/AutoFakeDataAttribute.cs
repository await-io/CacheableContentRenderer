using AutoFixture;
using AutoFixture.AutoFakeItEasy;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CacheableContent.Tests.Infrastructure.Attributes
{
    public class AutoFakeDataAttribute : AutoDataAttribute
    {
        public AutoFakeDataAttribute()
            : base(() => new Fixture().Customize(new AutoFakeItEasyCustomization()))
        {
        }
    }

    public class InlineAutoFakeDataAttribute : CompositeDataAttribute
    {
        public InlineAutoFakeDataAttribute(params object[] values) : base(new InlineDataAttribute(values), new AutoFakeDataAttribute())
        {
        }
    }
}
