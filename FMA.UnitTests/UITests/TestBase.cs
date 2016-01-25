using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using FMA.Contracts;
using FMA.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace FMA.UnitTests.CodedUITests
{
    [TestClass]
    public class TestBase
    {
        protected Application application;
        protected Window mainWindow;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            application = Application.Launch("FMA.exe");
            mainWindow = application.GetWindow(SearchCriteria.ByControlType(ControlType.Window), InitializeOption.NoCache);
        }

        [TestCleanup()]
        public void MyTestCleanUp()
        {
            application.KillAndSaveState();
        }

        protected void ClickButton(string automationId)
        {
            var button = mainWindow.Get<Button>(SearchCriteria.ByAutomationId(automationId));
            button.Click();
        }

        protected void ChangeMaterial(Material materialToSelect)
        {
            var comboBox = mainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("MaterialComboBox"));
            comboBox.Select(materialToSelect.Title);
        }

        protected int GetValueFromNumericUpDown(IUIItem numericUpDown)
        {
            var logoWidthValue = numericUpDown.Get<TextBox>("Value");
            return Int32.Parse(logoWidthValue.Text);
        }

        protected void SetValueToNumericUpDown(IUIItem numericUpDown, int value)
        {
            var logoWidthValue = numericUpDown.Get<TextBox>("Value");
            logoWidthValue.Text = value.ToString(CultureInfo.InvariantCulture);
        }

        protected Window OpenExternalPreview()
        {
            var showExternalPreview = mainWindow.Get<Button>(SearchCriteria.ByAutomationId("ShowExternalPreview"));
            showExternalPreview.Click();

            var windows = this.application.GetWindows();
            windows.Remove(mainWindow);
            return windows.Last();
        }
    }
}
