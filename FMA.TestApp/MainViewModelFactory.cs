using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using FMA.Contracts;
using FMA.Core;
using FMA.TestData;
using FMA.View.AdminView;
using FMA.View.Common;
using FMA.View.Helpers;

namespace FMA
{
    public class MainViewModelFactory
    {
        public static AdminViewModel CreateAdminViewModel(Window mainWindow)
        {
            var viewModel = new AdminViewModel(DummyData.GetDummyMaterials(), GetGetFontFunc(), new FontService(CustomFontsDir), new WindowService(mainWindow));

            viewModel.MaterialCreated += m =>
            {

            };

            return viewModel;
        }

        public static FlyerMakerViewModel CreateFlyerViewModel(Window mainWindow)
        {
            var viewModel = new FlyerMakerViewModel(DummyData.GetDummyMaterials(), DummyData.DefaultSelectedMaterialId, GetGetFontFunc(), new FontService(CustomFontsDir), new WindowService(mainWindow));

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
                var customFontsDir = string.Format(@"{0}\CustomFonts\", exeDir);
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
                    return new FontInfo("SignaritaAnne.ttf", Helper.GetByteArrayFromFile("SignaritaAnneDemo.ttf"));
                }
                if (name == "Bakery")
                {
                    return new FontInfo("Bakery.ttf", Helper.GetByteArrayFromFile("bakery.ttf"));
                }

                return null;
            };
            return getFont;
        }
    }
}