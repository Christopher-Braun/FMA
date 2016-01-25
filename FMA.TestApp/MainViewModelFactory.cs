using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using FMA.Contracts;
using FMA.Core;
using FMA.TestData;
using FMA.View.Common;
using FMA.View.Helpers;

namespace FMA
{
    public class MainViewModelFactory
    {
        public static FlyerMakerViewModel CreateFlyerViewModel(Window mainWindow)
        {
            var dummyMaterials = DummyData.GetDummyMaterials();

            //Dummy wird später ersetzt durch WCF Connection
            Func<string, FontInfo> getFont = name =>
            {
                if (name == "Signarita Anne")
                {
                    return new FontInfo("SignaritaAnne.ttf", Helper.GetByteArrayFromFile("SignaritaAnneDemo.ttf"));
                }
                if (name == "Bakery")
                {
                    return new FontInfo("Bakery.ttf", Helper.GetByteArrayFromFile("bakery.ttf"));
                }

                return null;
            };

            var exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var customFontsDir = string.Format(@"{0}\CustomFonts\", exeDir);
            var viewModel = new FlyerMakerViewModel(dummyMaterials, DummyData.DefaultSelectedMaterialId, getFont,
                new FontService(customFontsDir), new WindowService(mainWindow));

            var flyerCreator = new FlyerCreator(customFontsDir);

            viewModel.FlyerCreated += cm =>
            {
                var flyer = flyerCreator.CreateFlyer(cm);

                using (var fileStream = new FileStream(FlyerTestAppSettings.TestappflyerJpg, FileMode.Create))
                {
                    flyer.WriteTo(fileStream);
                    Process.Start(FlyerTestAppSettings.TestappflyerJpg);
                }
            };
            return viewModel;
        }
    }
}