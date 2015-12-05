using System.Windows;

namespace FMA.View
{
    public class WindowService
    {
        private readonly Window mainWindow;
        private ExternalPreviewView externalPreviewView;
        private ExternalEditView externalEditView;

        public WindowService(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void OpenExternalPreviewWindow(SelectedMaterialProvider selectedMaterialProvider)
        {
            if (externalPreviewView != null)
            {
                CloseExternalPreviewWindow();
            }

            externalPreviewView = new ExternalPreviewView
            {
                DataContext = new ExternalPreviewViewModel(selectedMaterialProvider),
                Owner = mainWindow
            };
            externalPreviewView.Show();
        }

        public void OpenExternalEditWindow(SelectedMaterialProvider selectedMaterialProvider)
        {
            if (externalEditView != null)
            {
                CloseExternalEditWindow();
            }

            externalEditView = new ExternalEditView
            {
                DataContext = new ExternalPreviewViewModel(selectedMaterialProvider),
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
