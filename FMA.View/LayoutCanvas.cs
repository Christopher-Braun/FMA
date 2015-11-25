using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FMA.View.Helpers;
using FMA.View.Models;

namespace FMA.View
{
    public class LayoutCanvas : Canvas
    {
        public static readonly DependencyProperty MaterialModelProperty = DependencyProperty.Register("MaterialModel", typeof(MaterialModel), typeof(LayoutCanvas),
            new PropertyMetadata(null, PropertyChangedCallback));

        public static readonly DependencyProperty CanManipulateTextsAndLogosProperty = DependencyProperty.Register(
            "CanManipulateTextsAndLogos", typeof (bool), typeof (LayoutCanvas), new PropertyMetadata(default(bool)));

        public bool CanManipulateTextsAndLogos
        {
            get { return (bool) GetValue(CanManipulateTextsAndLogosProperty); }
            set { SetValue(CanManipulateTextsAndLogosProperty, value); }
        }

        public static readonly DependencyProperty CanManipulateLogosProperty = DependencyProperty.Register(
            "CanManipulateLogos", typeof (bool), typeof (LayoutCanvas), new PropertyMetadata(default(bool)));

        public bool CanManipulateLogos
        {
            get { return (bool) GetValue(CanManipulateLogosProperty); }
            set { SetValue(CanManipulateLogosProperty, value); }
        }

        public MaterialModel MaterialModel
        {
            get { return (MaterialModel)GetValue(MaterialModelProperty); }
            set { SetValue(MaterialModelProperty, value); }
        }

        static LayoutCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutCanvas), new FrameworkPropertyMetadata(typeof(LayoutCanvas)));
        }


        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (LayoutCanvas)d;
            me.MaterialModelChanged();
        }

        private bool mouseDown;
        private bool isDragging;
        private Point startPoint;
        private AdornerLayer adornerLayer;
        private UIElement selectedElement;
        private double realtiveY;
        private double relativeX;
        private Image backgroundImage;
        private Image logoImage;

        public LayoutCanvas()
        {
            SetEvents();
            this.ClipToBounds = true;
            this.AllowDrop = true;
        }

        private void MaterialModelChanged()
        {
            this.selectedElement = null;

            if (MaterialModel == null) return;

            MaterialModel.PropertyChanged += MaterialModel_PropertyChanged;

            this.Children.Clear();
            backgroundImage = new Image { Source = MaterialModel.FlyerFrontSideImage };
            this.Children.Add(backgroundImage);

            foreach (var textBlock in MaterialModel.MaterialFields.Select(CreateTextBlock))
            {
                this.Children.Add(textBlock);
            }

            logoImage = CreateLogoImage();

            this.Children.Add(logoImage);
        }

  
        //TODO Hack nicht so gut
        private void MaterialModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Logo"))
            {
                if (MaterialModel == null)
                {
                    return;
                }

                if (this.Children.Contains(logoImage))
                {
                    this.Children.Remove(logoImage);
                }

                logoImage = CreateLogoImage();
                this.Children.Add(logoImage);
            }
        }

        private static TextBlock CreateTextBlock(MaterialFieldModel materialFieldModel)
        {
            var fontStyle = materialFieldModel.Italic ? FontStyles.Italic : FontStyles.Normal;
            var fontWeight = materialFieldModel.Bold ? FontWeights.Bold : FontWeights.Normal;
            var fontFamily = new FontFamily(materialFieldModel.FontName);

            var textBlock = new TextBlock
            {
                FontFamily = fontFamily,
                FontStyle = fontStyle,
                FontWeight = fontWeight,
                DataContext = materialFieldModel
            };

            var fontSizeBinding = new Binding("FontSize") { Mode = BindingMode.OneWay };
            textBlock.SetBinding(TextBlock.FontSizeProperty, fontSizeBinding);

            var textBinding = new Binding("Value") { Mode = BindingMode.OneWay };
            textBlock.SetBinding(TextBlock.TextProperty, textBinding);

            var topBinding = new Binding("TopMargin") { Mode = BindingMode.TwoWay };

            textBlock.SetBinding(Canvas.TopProperty, topBinding);

            var leftBinding = new Binding("LeftMargin") { Mode = BindingMode.TwoWay };
            textBlock.SetBinding(Canvas.LeftProperty, leftBinding);

            return textBlock;
        }

        private Image CreateLogoImage()
        {
            var logoImage = new Image { DataContext = MaterialModel.LogoModel, Stretch = Stretch.Fill };

            var sourceBinding = new Binding("LogoImage") { Mode = BindingMode.OneWay };
            logoImage.SetBinding(Image.SourceProperty, sourceBinding);

            var heightBinding = new Binding("Height") { Mode = BindingMode.TwoWay };
            logoImage.SetBinding(HeightProperty, heightBinding);

            var widthBinding = new Binding("Width") { Mode = BindingMode.TwoWay };
            logoImage.SetBinding(WidthProperty, widthBinding);

            var topBinding = new Binding("TopMargin") { Mode = BindingMode.TwoWay };
            logoImage.SetBinding(Canvas.TopProperty, topBinding);

            var leftBinding = new Binding("LeftMargin") { Mode = BindingMode.TwoWay };
            logoImage.SetBinding(Canvas.LeftProperty, leftBinding);
            return logoImage;
        }


        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            if (InternalChildren.Count == 0)
            {
                return new Size();
            }

            var image = MaterialModel.FlyerFrontSideImage;
            return new Size(image.Width, image.Height);
        }


        private void SetEvents()
        {
            MouseLeftButtonDown += (s, e) => UnselectElement();
            MouseMove += LayoutCanvas_MouseMove;

            PreviewMouseLeftButtonDown += LayoutCanvas_PreviewMouseLeftButtonDown;

            MouseLeftButtonUp += (s, e) => DragFinished(e);
            MouseLeave += (s, e) => DragFinished(e);
            PreviewMouseLeftButtonUp += (s, e) => DragFinished(e);

            Drop += LayoutCanvas_Drop;
        }

        private void LayoutCanvas_Drop(object sender, DragEventArgs e)
        {
            this.DropLogo(this.MaterialModel, e);
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


        // Handler for providing drag operation with selected element
        private void LayoutCanvas_MouseMove(object sender, MouseEventArgs e)
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

            Canvas.SetTop(selectedElement, topMargin);
            Canvas.SetLeft(selectedElement, leftMargin);
        }

        private void UnselectElement()
        {
            if (selectedElement == null)
            {
                return;
            }
            var adorners = adornerLayer.GetAdorners(selectedElement);

            if (adorners != null) adornerLayer.Remove(adorners[0]);

            selectedElement = null;
        }

        // Handler for element selection on the canvas providing resizing adorner
        private void LayoutCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Remove selection on clicking anywhere the window
            UnselectElement();

            if (e.Source == this)
            {
                return;
            }

            var source = (UIElement)e.Source;
            if (source == backgroundImage)
            {
                return;
            }

            var logoClicked = source == logoImage;
            if (this.CanManipulateTextsAndLogos == false &&  this.CanManipulateLogos == false )
            {
                //In PreviewMode Nothing can be selected
                return;
            }

            if (this.CanManipulateTextsAndLogos == false && logoClicked == false)
            {
                //Is DefaultMode only Logo can be manipulated
                return;
            }

            // If any element except canvas is clicked, 
            // assign the selected element and add the adorner
            mouseDown = true;
            startPoint = e.GetPosition(this);

            selectedElement = source;

            realtiveY = startPoint.Y - Canvas.GetTop(selectedElement);
            relativeX = (startPoint.X - Canvas.GetLeft(selectedElement));

            adornerLayer = AdornerLayer.GetAdornerLayer(selectedElement);

            if (selectedElement is TextBlock)
            {
                adornerLayer.Add(new SelectionAdorner(selectedElement));
            }
            else if (selectedElement is Image)
            {
                adornerLayer.Add(new ResizingAdorner(selectedElement));
            }

            e.Handled = true;
        }
    }
}