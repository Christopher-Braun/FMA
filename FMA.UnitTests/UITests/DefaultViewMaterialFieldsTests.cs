using System;
using System.Linq;
using FMA.TestData;
using FMA.UnitTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace FMA.UnitTests.UITests
{
    [TestClass]
    public class DefaultViewMaterialFieldsTests : TestBase
    {

        [TestMethod]
        public void PerDefault_InTextInput_MaterialFieldValuesAreCorrect()
        {
            var textBoxes = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue")).OfType<TextBox>().ToList();
            var materialFields = DummyData.GetDefaultSelectedMaterial().MaterialFields;

            var materialTexts = materialFields.Select(f => f.Value).OrderBy(x => x);
            var textBoxTexts = textBoxes.Select(t => t.Text).OrderBy(x => x);

            Assert.IsTrue(materialTexts.SequenceEqual(textBoxTexts));
        }

        [TestMethod]
        public void PerDefault_InPreview_MaterialFieldValuesAreCorrect()
        {
            var textBoxesInPreview = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("CanvasTextBlock")).OfType<WPFLabel>().ToList();
            var materialFields = DummyData.GetDefaultSelectedMaterial().MaterialFields;

            var materialTexts = materialFields.Select(f => f.GetDisplayText()).OrderBy(x => x);
            var textBoxTexts = textBoxesInPreview.Select(t => t.Text).OrderBy(x => x);

            Assert.IsTrue(materialTexts.SequenceEqual(textBoxTexts));
        }

        [TestMethod]
        public void PerDefault_InExternalPreview_MaterialFieldValuesAreCorrect()
        {
            var externalPreview = OpenExternalPreview();
            var textBoxesInExternalPreview = externalPreview.GetMultiple(SearchCriteria.ByAutomationId("CanvasTextBlock")).OfType<WPFLabel>().ToList();
            var materialFields = DummyData.GetDefaultSelectedMaterial().MaterialFields;

            var materialTexts = materialFields.Select(f => f.GetDisplayText()).OrderBy(x => x);
            var textBoxTexts = textBoxesInExternalPreview.Select(t => t.Text).OrderBy(x => x);

            Assert.IsTrue(materialTexts.SequenceEqual(textBoxTexts));
        }

        [TestMethod]
        public void SelectAnotherMaterial_InTextInput_MaterialFieldValuesAreCorrect()
        {
            var materialToSelect = DummyData.GetNotDefaultSelectedMaterial();
            ChangeMaterial(materialToSelect);

            var materialFields = materialToSelect.MaterialFields;
            var materialTexts = materialFields.Select(f => f.Value).OrderBy(x => x);

            var textBoxes = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue")).OfType<TextBox>().ToList();
            var textBoxTexts = textBoxes.Select(t => t.Text).OrderBy(x => x);
            Assert.IsTrue(materialTexts.SequenceEqual(textBoxTexts));
        }

        [TestMethod]
        public void SelectAnotherMaterial_InPreview_AndExternalPreview_MaterialFieldValuesAreCorrect()
        {
            var materialToSelect = DummyData.GetNotDefaultSelectedMaterial();
            ChangeMaterial(materialToSelect);

            var materialFields = materialToSelect.MaterialFields;
            var materialTexts = materialFields.Select(f => f.GetDisplayText()).OrderBy(x => x);

            var textBoxesInPreview = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("CanvasTextBlock")).OfType<WPFLabel>().ToList();
            var textBoxesInPreviewTexts = textBoxesInPreview.Select(t => t.Text).OrderBy(x => x);
            Assert.IsTrue(materialTexts.SequenceEqual(textBoxesInPreviewTexts));

            var externalPreview = OpenExternalPreview();
            var textBoxesInExternalPreview = externalPreview.GetMultiple(SearchCriteria.ByAutomationId("CanvasTextBlock")).OfType<WPFLabel>().ToList();
            var textBoxesInExternalPreviewTexts = textBoxesInExternalPreview.Select(t => t.Text).OrderBy(x => x);
            Assert.IsTrue(materialTexts.SequenceEqual(textBoxesInExternalPreviewTexts));
        }

        [TestMethod]
        public void ChangeMaterialFieldValue_ViaTextBox_ChangesTextInPreview()
        {
            var textBoxes = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue")).OfType<TextBox>();

            var textsInPreview = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("CanvasTextBlock")).OfType<WPFLabel>().ToList();

            var externalPreview = OpenExternalPreview();
            var textsInExternalPreview = externalPreview.GetMultiple(SearchCriteria.ByAutomationId("CanvasTextBlock")).OfType<WPFLabel>().ToList();

            foreach (var textBox in textBoxes)
            {
                var matchingPreviewText = textsInPreview.First(t => t.Text.Equals(textBox.Text, StringComparison.CurrentCultureIgnoreCase));
                var matchingExternalPreviewText = textsInExternalPreview.First(t => t.Text.Equals(textBox.Text, StringComparison.CurrentCultureIgnoreCase));

                var matchingMaterialField =
                    DummyData.GetDummyMaterials()
                        .Single(m => m.Id.Equals(DummyData.DefaultSelectedMaterialId))
                        .MaterialFields.Single(f => f.Value.Equals(textBox.Text));


                var expected = matchingMaterialField.Uppper ? textBox.Text.ToUpper() : textBox.Text;
                Assert.AreEqual(expected, matchingPreviewText.Text);
                Assert.AreEqual(expected, matchingExternalPreviewText.Text);

                textBox.Text = textBox.Text + "_Modified";

                expected = matchingMaterialField.Uppper ? textBox.Text.ToUpper() : textBox.Text;
                Assert.AreEqual(expected, matchingPreviewText.Text);
                Assert.AreEqual(expected, matchingExternalPreviewText.Text);
            }
        }

        [TestMethod]
        public void ChangeMaterialFieldValue_ViaTextBoxWithTooLongText_DisablesCanCreate()
        {
            var createButton = MainWindow.Get<Button>("Create");
            Assert.IsTrue(createButton.Enabled);

            var textBoxes = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue")).OfType<TextBox>();

            foreach (var textBox in textBoxes)
            {
                var matchingMaterialField =
                    DummyData.GetDummyMaterials()
                        .Single(m => m.Id.Equals(DummyData.DefaultSelectedMaterialId))
                        .MaterialFields.Single(f => f.Value.Equals(textBox.Text));


                var newText = new string('X', matchingMaterialField.MaxLength);

                textBox.Text = newText;
                Assert.IsTrue(createButton.Enabled);

                textBox.Text += "Y";
                Assert.IsFalse(createButton.Enabled);

              //  var uiItems2 = mainWindow.GetMultiple(SearchCriteria.ByAutomationId("ErrorT"));
              //  var uiItems = mainWindow.GetMultiple(SearchCriteria.ByAutomationId("Error"));
             //   Assert.IsTrue(uiItems.Any());

                textBox.Text = "E";
            }
        }

        [TestMethod]
        public void Reset_ResetsCanCreateState()
        {
            var createButton = MainWindow.Get<Button>("Create");
            Assert.IsTrue(createButton.Enabled);

            var textBox =
                MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue"))
                .OfType<TextBox>()
                .First();

            var matchingMaterialField =
                DummyData.GetDummyMaterials()
                    .Single(m => m.Id.Equals(DummyData.DefaultSelectedMaterialId))
                    .MaterialFields.Single(f => f.Value.Equals(textBox.Text));


            var newText = new string('X', matchingMaterialField.MaxLength + 1);

            textBox.Text = newText;

            Assert.IsFalse(createButton.Enabled);

            ClickButton("Reset");

            Assert.IsTrue(createButton.Enabled);
        }

        [TestMethod]
        public void ChangeMaterial_ResetsCanCreateState()
        {
            var createButton = MainWindow.Get<Button>("Create");
            Assert.IsTrue(createButton.Enabled);

            var textBox =
                MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue"))
                .OfType<TextBox>()
                .First();

            var matchingMaterialField =
                DummyData.GetDummyMaterials()
                    .Single(m => m.Id.Equals(DummyData.DefaultSelectedMaterialId))
                    .MaterialFields.Single(f => f.Value.Equals(textBox.Text));


            var newText = new string('X', matchingMaterialField.MaxLength + 1);

            textBox.Text = newText;

            Assert.IsFalse(createButton.Enabled);

            ChangeMaterial(DummyData.GetNotDefaultSelectedMaterial());

            Assert.IsTrue(createButton.Enabled);
        }


        [TestMethod]
        public void ChangeMaterialFieldValue_ViaTextBoxWithTextWithTooManyLines_DisablesCanCreate()
        {
            var createButton = MainWindow.Get<Button>("Create");
            Assert.IsTrue(createButton.Enabled);

            var textBoxes = MainWindow.GetMultiple(SearchCriteria.ByAutomationId("MaterialFieldValue")).OfType<TextBox>();

            foreach (var textBox in textBoxes)
            {
                var matchingMaterialField =
                    DummyData.GetDummyMaterials()
                        .Single(m => m.Id.Equals(DummyData.DefaultSelectedMaterialId))
                        .MaterialFields.Single(f => f.Value.Equals(textBox.Text));


                var newText = "S";
                for (int i = 0; i < matchingMaterialField.MaxRows - 1; i++)
                {
                    newText += "\r\nR";
                }

                textBox.Text = newText;
                Assert.IsTrue(createButton.Enabled);

                textBox.Text += "\r\nR";
                Assert.IsFalse(createButton.Enabled);

                textBox.Text = "E";
            }
        }

    }
}
