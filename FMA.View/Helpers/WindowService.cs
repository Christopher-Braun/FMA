// Christopher Braun 2016

using System.Windows;
using FMA.Contracts;
using FMA.View.Common;
using FMA.View.ExternalView;

namespace FMA.View.Helpers
{
    public class WindowService : IWindowService
    {
        private readonly Window mainWindow;
        private ExternalPreviewView externalPreviewView;
        private ExternalEditView externalEditView;

        public WindowService(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void OpenExternalPreviewWindow(SelectedMaterialProvider selectedMaterialProvider,
            IFontService fontService)
        {
            if (externalPreviewView != null)
            {
                CloseExternalPreviewWindow();
            }

            externalPreviewView = new ExternalPreviewView
            {
                DataContext = new ExternalViewModel(selectedMaterialProvider, fontService),
                Owner = mainWindow
            };
            externalPreviewView.Show();
        }

        public void OpenExternalEditWindow(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService)
        {
            if (externalEditView != null)
            {
                CloseExternalEditWindow();
            }

            externalEditView = new ExternalEditView
            {
                DataContext = new ExternalViewModel(selectedMaterialProvider, fontService),
                Owner = mainWindow
            };
            externalEditView.Show();
        }

        public void CloseExternalPreviewWindow()
        {
            if (externalPreviewView == null)
            {
                return;
            }
            externalPreviewView.Close();
            externalPreviewView = null;
        }

        public void CloseExternalEditWindow()
        {
            if (externalEditView == null)
            {
                return;
            }
            externalEditView.Close();
            externalEditView = null;
        }
    }
}