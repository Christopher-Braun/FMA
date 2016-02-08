using System.Globalization;
using System.Linq;
using System.Windows.Automation;
using FMA.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace FMA.UnitTests.UITests
{
    [TestClass]
    public class TestBase
    {
        protected Application Application;
        protected Window MainWindow;

        [TestInitialize]
        public void MyTestInitialize()
        {
            Application = Application.Launch("FMA.exe");
            MainWindow = Application.GetWindow(SearchCriteria.ByControlType(ControlType.Window), InitializeOption.NoCache);
        }

        [TestCleanup]
        public void MyTestCleanUp()
        {
            Application.KillAndSaveState();
        }

        protected void ClickButton(string automationId)
        {
            var button = MainWindow.Get<Button>(SearchCriteria.ByAutomationId(automationId));
            button.Click();
        }

        protected void ChangeMaterial(Material materialToSelect)
        {
            var comboBox = MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("MaterialComboBox"));
            comboBox.Select(materialToSelect.Title);
        }

        protected int GetValueFromNumericUpDown(IUIItem numericUpDown)
        {
            var logoWidthValue = numericUpDown.Get<TextBox>("Value");
            return int.Parse(logoWidthValue.Text);
        }

        protected void SetValueToNumericUpDown(IUIItem numericUpDown, int value)
        {
            var logoWidthValue = numericUpDown.Get<TextBox>("Value");
            logoWidthValue.Text = value.ToString(CultureInfo.InvariantCulture);
        }

        protected Window OpenExternalPreview()
        {
            var showExternalPreview = MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ShowExternalPreview"));
            showExternalPreview.Click();

            var windows = this.Application.GetWindows();
            windows.Remove(MainWindow);
            return windows.Last();
        }
    }
}
