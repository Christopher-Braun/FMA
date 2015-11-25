using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using FMA.Core;
using FMA.View.Models;

namespace FMA.View
{
    public class SelectedMaterialProvider : NotifyPropertyChangedBase
    {
        private MaterialModel materialModel;

        public MaterialModel MaterialModel
        {
            get { return materialModel; }
            set
            {
                if (materialModel != null)
                {
                    this.materialModel.PropertyChanged -= MaterialModelPropertyChanged;
                }
                if (value == null)
                {
                    this.materialModel = null;
                    return;
                }

                this.materialModel = value;

                this.materialModel.PropertyChanged += MaterialModelPropertyChanged;
                MaterialModelPropertyChanged(null, null);
                OnPropertyChanged();
            }
        }

        private void MaterialModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (suspendRefreshPreview)
            {
                return;
            }

            UpdatePreview();
        }

        private CancellationTokenSource tokenSource;


        private async Task UpdatePreview()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }

            tokenSource = new CancellationTokenSource();

            var preview = await Task.Run(() => FlyerCreator.CreateImage(MaterialModel.ToCustomMaterial(), tokenSource.Token), tokenSource.Token);
            tokenSource = null;
            FlyerPreview = preview;

            //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            //{
            //    FlyerPreview = preview;
            //}));

            base.OnPropertyChanged("FlyerPreview");
        }


        private ImageSource flyerPreview;

        public ImageSource FlyerPreview
        {
            get { return flyerPreview; }
            private set
            {
                if (Equals(value, flyerPreview)) return;
                flyerPreview = value;
                OnPropertyChanged();
            }
        }

        private bool suspendRefreshPreview;

        public void SuspendRefreshPreview()
        {
            this.suspendRefreshPreview = true;
        }

        public void ResumeRefreshPreview()
        {
            this.suspendRefreshPreview = false;
            UpdatePreview();
        }
    }
}