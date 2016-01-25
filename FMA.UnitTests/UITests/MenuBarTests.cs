using System.IO;
using System.Linq;
using FMA.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;

namespace FMA.UnitTests.CodedUITests
{
    [TestClass]
    public class MenuBarTests : TestBase
    {
        [TestMethod]
        public void CreateFlyer_CreatesFlyerJpg()
        {
            if (File.Exists(FlyerTestAppSettings.TestappflyerJpg))
            {
                File.Delete(FlyerTestAppSettings.TestappflyerJpg);
            }

            var create = mainWindow.Get<Button>(SearchCriteria.ByAutomationId("Create"));

            create.Click();

            Assert.IsTrue(File.Exists(FlyerTestAppSettings.TestappflyerJpg));
        }


        [TestMethod]
        public void ShowFlyerBackSide_MakesBackSideVisible()
        {
            var image = mainWindow.Get<Image>(SearchCriteria.ByAutomationId("CanvasBackSideImage"));

            Assert.IsFalse(image.Visible);

            var showBackSide = mainWindow.Get<Button>(SearchCriteria.ByAutomationId("ShowBackSide"));
            showBackSide.Click();

            Assert.IsTrue(image.Visible);
        }

        [TestMethod]
        public void OpenExternalView_OpensExternalView()
        {
            Assert.AreEqual(1, application.GetWindows().Count);

            OpenExternalPreview();

            var windows = application.GetWindows();
            Assert.AreEqual(2, windows.Count);

            var previewWindow = windows.Last();
            Assert.AreEqual("ExternalPreview", previewWindow.Id);
            //Difference?
            Assert.AreEqual("ExternalPreview", previewWindow.PrimaryIdentification);

            Assert.IsFalse(previewWindow.IsModal);
            Assert.IsFalse(previewWindow.IsClosed);
            Assert.IsTrue(previewWindow.Enabled);
            Assert.AreEqual(string.Empty, previewWindow.Title);
        }

        [TestMethod]
        public void SelectMaterialComboBox_ContainsCorretValues_AndHasCorrectSelectedItemText()
        {
            var comboBox = mainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("MaterialComboBox"));

            var itemTexts = comboBox.Items.AsEnumerable().Select(i => i.Text);
            var materialTitles = DummyData.GetDummyMaterials().Select(m => m.Title);

            Assert.IsTrue(itemTexts.SequenceEqual(materialTitles));

            Assert.AreEqual(DummyData.GetDefaultSelectedMaterial().Title, comboBox.SelectedItemText);

        }
    }
}