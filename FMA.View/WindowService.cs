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

        public void OpenExternalPreviewWindow(SelectedMaterialProvider selectedMaterialProvider)
        {
            externalPreviewView = new ExternalPreviewView();
            externalPreviewView.DataContext = new ExternalPreviewViewModel(selectedMaterialProvider);
            externalPreviewView.Owner = mainWindow;
            externalPreviewView.Show();
        }

        public void CloseExternalPreviewWindow()
        {
            externalPreviewView.Close();
            externalPreviewView = null;
        }
    }
}
