using System;

using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    public static class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            return platform switch
            {
                Platform.Android => ConfigureApp.Android.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear),
                Platform.iOS => ConfigureApp.iOS.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear),
                _ => throw new NotSupportedException(),
            };
        }
    }
}

