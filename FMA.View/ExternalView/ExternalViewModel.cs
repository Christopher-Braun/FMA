// Christopher Braun 2016

using FMA.Contracts;
using FMA.View.Common;
using FMA.View.Models;

namespace FMA.View.ExternalView
{
    public class ExternalViewModel : NotifyPropertyChangedBase
    {
        public SelectedMaterialProvider SelectedMaterialProvider { get; }
        public IFontService FontService { get; }

        public ExternalViewModel(SelectedMaterialProvider selectedMaterialProvider, IFontService fontService)
        {
            SelectedMaterialProvider = selectedMaterialProvider;
            FontService = fontService;
        }
    }
}