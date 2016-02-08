// Christopher Braun 2016

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using FMA.AdminView;
using FMA.Contracts;
using FMA.Core;
using FMA.TestData;
using FMA.View.Common;
using FMA.View.Helpers;

namespace FMA
{
    public class MainViewModelFactory
    {
        public static AdminViewModel CreateAdminViewModel(Window mainWindow)
        {
            var viewModel = new AdminViewModel(DummyData.GetDummyMaterials(), GetGetFontFunc(),
                new FontService(CustomFontsDir), new WindowService(mainWindow));

            viewModel.MaterialCreated += m => { };

            return viewModel;
        }

        public static FlyerMakerViewModel CreateFlyerViewModel(Window mainWindow)
        {
            var viewModel = new FlyerMakerViewModel(DummyData.GetDummyMaterials(), DummyData.DefaultSelectedMaterialId,
                GetGetFontFunc(), new FontService(CustomFontsDir), new WindowService(mainWindow));

            var flyerCreator = new FlyerCreator(CustomFontsDir);

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

        private static string CustomFontsDir
        {
            get
            {
                var exeDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var customFontsDir = $@"{exeDir}\CustomFonts\";
                return customFontsDir;
            }
        }

        private static Func<string, FontInfo> GetGetFontFunc()
        {
            //Dummy wird später ersetzt durch WCF Connection
            Func<string, FontInfo> getFont = name =>
            {
                if (name == "Signarita Anne")
                {
                    return new FontInfo("SignaritaAnne.ttf",
                        FileHelper.GetByteArrayFromFile("Fonts\\SignaritaAnneDemo.ttf"));
                }
                if (name == "Bakery")
                {
                    return new FontInfo("Bakery.ttf", FileHelper.GetByteArrayFromFile("Fonts\\bakery.ttf"));
                }

                return null;
            };
            return getFont;
        }
    }
}