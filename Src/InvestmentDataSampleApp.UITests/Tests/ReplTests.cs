using NUnit.Framework;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class ReplTests : BaseTest
    {

        public ReplTests(Platform platform) : base(platform)
        {
        }

        [Ignore("REPL used for manually exploring app")]
        [Test]
        public void ReplTest() => App.Repl();
    }
}

