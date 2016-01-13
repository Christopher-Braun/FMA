using FMA.Contracts;
using FMA.View.Common;

namespace FMA.View.Helpers
{
    public interface IWindowService
    {
        void OpenExternalPreviewWindow(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService);
        void OpenExternalEditWindow(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService);
        void CloseExternalPreviewWindow();
        void CloseExternalEditWindow();
    }
}