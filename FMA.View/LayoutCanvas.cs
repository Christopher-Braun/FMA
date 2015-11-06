using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FMA.View
{
    /// <summary>
    ///     Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///     Step 1a) Using this custom control in a XAML file that exists in the current project.
    ///     Add this XmlNamespace attribute to the root element of the markup file where it is
    ///     to be used:
    ///     xmlns:MyNamespace="clr-namespace:FMA.View"
    ///     Step 1b) Using this custom control in a XAML file that exists in a different project.
    ///     Add this XmlNamespace attribute to the root element of the markup file where it is
    ///     to be used:
    ///     xmlns:MyNamespace="clr-namespace:FMA.View;assembly=FMA.View"
    ///     You will also need to add a project reference from the project where the XAML file lives
    ///     to this project and Rebuild to avoid compilation errors:
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///     Step 2)
    ///     Go ahead and use your control in the XAML file.
    ///     <MyNamespace:LayoutCanvas />
    /// </summary>
    public class LayoutCanvas : Canvas
    {
        public static readonly DependencyProperty MaterialModelProperty = DependencyProperty.Register(
            "MaterialModel", typeof (MaterialModel), typeof (LayoutCanvas),
            new PropertyMetadata(null, PropertyChangedCallback));

        private bool mouseDown;
        private bool isDragging;
        private Point startPoint;
        private AdornerLayer adornerLayer;
        private UIElement selectedElement;
        private double realtiveY;
        private double relativeX;

        static LayoutCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (LayoutCanvas),
                new FrameworkPropertyMetadata(typeof (LayoutCanvas)));
        }

        public MaterialModel MaterialModel
        {
            get { return (MaterialModel) GetValue(MaterialModelProperty); }
            set { SetValue(MaterialModelProperty, value); }
        }


        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (LayoutCanvas) d;
            me.MaterialModelChanged();
        }

        public LayoutCanvas()
        {
            SetEvents();
            this.AllowDrop = true;
        }

        private void MaterialModelChanged()
        {
            this.selectedElement = null;

            if (MaterialModel == null) return;

            this.Children.Clear();
            var backgroundImage = new Image {Source = MaterialModel.FlyerFrontSideImage};
            this.Children.Add(backgroundImage);

            foreach (var materialFieldModel in MaterialModel.MaterialFields)
            {
                var fontStyle = materialFieldModel.Italic ? FontStyles.Italic : FontStyles.Normal;
                var fontWeight = materialFieldModel.Bold ? FontWeights.Bold : FontWeights.Normal;
                var fontFamily = new FontFamily(materialFieldModel.FontName);

                var textBlock = new TextBlock
                {
                    Text = materialFieldModel.Value,
                    FontSize = materialFieldModel.FontSize,
                    FontFamily = fontFamily,
                    FontStyle = fontStyle,
                    FontWeight = fontWeight,
                    Tag = materialFieldModel
                };

                this.Children.Add(textBlock);

                Canvas.SetTop(textBlock, materialFieldModel.TopMargin);
                Canvas.SetLeft(textBlock, materialFieldModel.LeftMargin);
            }

            if (MaterialModel.LogoModel.HasLogo)
            {
                var logoImage = new Image
                {
                    Source = MaterialModel.LogoModel.LogoImage,
                    Tag = MaterialModel.LogoModel
                };
                Canvas.SetTop(logoImage, MaterialModel.LogoModel.TopMargin);
                Canvas.SetLeft(logoImage, MaterialModel.LogoModel.LeftMargin);
                this.Children.Add(logoImage);

                logoImage.SizeChanged += logoImage_SizeChanged;
            }
        }

        private void logoImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var tag = ((FrameworkElement) sender).Tag;

            var logoModel = tag as LogoModel;
            if (logoModel != null)
            {
                logoModel.Size = e.NewSize;
            }

        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            if (InternalChildren.Count == 0)
            {
             return new Size();   
            }

           var image = MaterialModel.FlyerFrontSideImage;
            return new Size(image.Width,image.Height);
        }


        private void SetEvents()
        {
            MouseLeftButtonDown += (s,e)=> UnselectElement();
            MouseMove += Window1_MouseMove;

            PreviewMouseLeftButtonDown += Canvas_PreviewMouseLeftButtonDown;

            MouseLeftButtonUp += (s,e)=> DragFinished(e);
            MouseLeave += (s,e)=> DragFinished(e);
            PreviewMouseLeftButtonUp +=  (s,e)=> DragFinished(e);

            Drop += LayoutCanvas_Drop;
        }

        void LayoutCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop);

                // Assuming you have one file that you care about, pass it off to whatever
                // handling code you have defined.

                var logoFile = files.First();

                byte[] logoData;

                using (var fileStream = new FileStream(logoFile, FileMode.Open))
                {
                    using (var ms = new MemoryStream())
                    {
                        fileStream.CopyTo(ms);
                        logoData = ms.ToArray();
                    }
                }

                this.MaterialModel.LogoModel.Logo = logoData;

                var mousePosition = e.GetPosition(this);
                this.MaterialModel.LogoModel.LeftMargin = (int)mousePosition.X;
                this.MaterialModel.LogoModel.TopMargin = (int)mousePosition.Y;

                //TODO Weg statt dessenauf Event hören
                this.MaterialModelChanged();
            }
        }

        private void DragFinished(MouseEventArgs e)
        {
            if (!mouseDown)
            {
                return;
            }
            mouseDown = false;
            isDragging = false;
            e.Handled = true;
        }

      
        // Hanler for providing drag operation with selected element
        private void Window1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
            {
                return;
            }

            var mousePosition = e.GetPosition(this);
            if ((isDragging == false) &&
                ((Math.Abs(mousePosition.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                 (Math.Abs(mousePosition.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
            {
                isDragging = true;
            }

            if (!isDragging)
            {
                return;
            }

            var position = Mouse.GetPosition(this);
            var topMargin = position.Y - realtiveY;
            var leftMargin = position.X - relativeX;

            SetMargins(topMargin, leftMargin);
        }

        private void SetMargins(double topMargin, double leftMargin)
        {
            Canvas.SetTop(selectedElement, topMargin);
            Canvas.SetLeft(selectedElement, leftMargin);

            //TODO HACK
            var tag = ((FrameworkElement) selectedElement).Tag;
            var model = tag as MaterialFieldModel;
            if (model != null)
            {
                //TODO Cast nicht gut
                model.TopMargin = (int) topMargin;
                model.LeftMargin = (int) leftMargin;
            }
            else
            {
                var logoModel = tag as LogoModel;
                if (logoModel != null)
                {
                    //TODO Cast nicht gut
                    logoModel.TopMargin = (int)topMargin;
                    logoModel.LeftMargin = (int)leftMargin;
                }
            }
        }

        private void UnselectElement()
        {
            if (selectedElement == null)
            {
                return;
            }
            adornerLayer.Remove(adornerLayer.GetAdorners(selectedElement)[0]);

            selectedElement = null;
        }

        // Handler for element selection on the canvas providing resizing adorner
        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Remove selection on clicking anywhere the window
           UnselectElement();

            if (e.Source == this)
            {
                return;
            }

            // If any element except canvas is clicked, 
            // assign the selected element and add the adorner

            mouseDown = true;
            startPoint = e.GetPosition(this);

            selectedElement = (UIElement)e.Source;
            realtiveY = startPoint.Y - Canvas.GetTop(selectedElement);
            relativeX = (startPoint.X - Canvas.GetLeft(selectedElement));

            adornerLayer = AdornerLayer.GetAdornerLayer(selectedElement);
            adornerLayer.Add(new ResizingAdorner(selectedElement));
            e.Handled = true;
        }
    }
}