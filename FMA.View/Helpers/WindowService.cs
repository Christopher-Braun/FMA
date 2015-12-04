using System.Windows;

namespace FMA.View
{
    public class WindowService
    {
        private readonly Window mainWindow;
        private ExternalPreviewView externalPreviewView;

        public WindowService(Window mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void OpenExternalPreviewWindow(SelectedMaterialProvider selectedMaterialProvider, bool canEdit)
        {
            if (externalPreviewView != null)
            {
                CloseExternalPreviewWindow();
            }

            externalPreviewView = new ExternalPreviewView
            {
                DataContext = new ExternalPreviewViewModel(selectedMaterialProvider, canEdit),
                Owner = mainWindow
            };
            externalPreviewView.Show();
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
    }
}
