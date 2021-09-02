using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;

namespace DevnotMentor.Test.Base
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public static AutoMoqCustomization DefaultAutoMoqCustomization { get; private set; } = new();
        public static Func<int, IFixture> FixtureFactory { get; set; } = (count) =>
         {
             var fixture = new Fixture
             {
                 RepeatCount = count,
             }
             .Customize(DefaultAutoMoqCustomization);

             fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
             fixture.Behaviors.Add(new OmitOnRecursionBehavior());

             return fixture;
         };

        public AutoMoqDataAttribute(int count = 3)
           : base(() => FixtureFactory(count))
        {
        }
    }
}
