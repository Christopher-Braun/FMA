using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FMA.Contracts;
using FMA.View.Helpers;
using FMA.View.Models;
using FontStyleConverter = FMA.View.Helpers.FontStyleConverter;
using FontWeightConverter = FMA.View.Helpers.FontWeightConverter;

namespace FMA.View
{
    public class LayoutCanvas : LayoutCanvasBase
    {
        public static readonly DependencyProperty MaterialModelProperty = DependencyProperty.Register(
            "MaterialModel", typeof(MaterialModel), typeof(LayoutCanvas), new PropertyMetadata(null, (d, e) => ((LayoutCanvas)d).MaterialModelChanged()));
        public MaterialModel MaterialModel
        {
            get { return (MaterialModel)GetValue(MaterialModelProperty); }
            set { SetValue(MaterialModelProperty, value); }
        }

        public static readonly DependencyProperty SelectedMaterialChildProperty = DependencyProperty.Register(
            "SelectedMaterialChild", typeof(IMaterialChild), typeof(LayoutCanvas), new PropertyMetadata(default(IMaterialChild), (d, e) => ((LayoutCanvas)d).SelectedMaterialFieldChanged()));

        public IMaterialChild SelectedMaterialChild
        {
            get { return (IMaterialChild)GetValue(SelectedMaterialChildProperty); }
            set { SetValue(SelectedMaterialChildProperty, value); }
        }


        public static readonly DependencyProperty FontServiceProperty = DependencyProperty.Register(
            "FontService", typeof(FontService), typeof(LayoutCanvas), new PropertyMetadata(default(FontService)));

        public FontService FontService
        {
            get { return (FontService)GetValue(FontServiceProperty); }
            set { SetValue(FontServiceProperty, value); }
        }

        public static readonly DependencyProperty CanManipulateTextsAndLogosProperty = DependencyProperty.Register(
            "CanManipulateTextsAndLogos", typeof(bool), typeof(LayoutCanvas), new PropertyMetadata(default(bool)));

        public bool CanManipulateTextsAndLogos
        {
            get { return (bool)GetValue(CanManipulateTextsAndLogosProperty); }
            set { SetValue(CanManipulateTextsAndLogosProperty, value); }
        }

        public static readonly DependencyProperty CanManipulateLogosProperty = DependencyProperty.Register(
            "CanManipulateLogos", typeof(bool), typeof(LayoutCanvas), new PropertyMetadata(default(bool)));

        public bool CanManipulateLogos
        {
            get { return (bool)GetValue(CanManipulateLogosProperty); }
            set { SetValue(CanManipulateLogosProperty, value); }
        }

        static LayoutCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutCanvas), new FrameworkPropertyMetadata(typeof(LayoutCanvas)));
        }

        public LayoutCanvas()
        {
            this.AllowDrop = true;
            this.Drop += (sender, e) => this.DropLogo(this.MaterialModel, e);
            SelectedChilds.CollectionChanged += SelectedElements_CollectionChanged;
        }

        private bool selectedMaterialChangedInternal = false;

        private void SelectedElements_CollectionChanged(object sender,System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            selectedMaterialChangedInternal = true;
            try
            {
                if (SelectedElements.Count() != 1)
                {
                    this.SelectedMaterialChild = null;
                    return;
                }

                var selectedMaterialFieldModel = SelectedElements.Single().Tag as IMaterialChild;

                SelectedMaterialChild = selectedMaterialFieldModel;
            }
            finally
            {
                selectedMaterialChangedInternal = false;
            }
        }

        private void SelectedMaterialFieldChanged()
       {
           if (selectedMaterialChangedInternal)
           {
               return;
           }

           if (this.SelectedMaterialChild == null)
           {
               UnSelectAllElements();
               return;
           }

           var elementToSelect = Children.OfType<FrameworkElement>().First(t => SelectedMaterialChild.Equals(t.Tag));

           SetSelectedElement(elementToSelect);
       }

       private Image backgroundImage;
        private Image logoImage;

        private void MaterialModelChanged()
        {
            if (MaterialModel == null) return;

            CreateChildren();

            MaterialModel.PropertyChanged += MaterialModel_PropertyChanged;
            MaterialModel.MaterialFields.CollectionChanged += MaterialFields_CollectionChanged;
        }

        private void CreateChildren()
        {
            this.ClearChildren();

            backgroundImage = new Image { Source = MaterialModel.FlyerFrontSideImage, Cursor = Cursors.Arrow };
            this.Children.Add(backgroundImage);

            logoImage = CreateLogoImage();
            this.Children.Add(logoImage);

            foreach (var textBlock in MaterialModel.MaterialFields.Select(CreateTextBlock))
            {
                this.Children.Add(textBlock);
            }

            CreateSelectionBorder();
       
        }

       private void MaterialFields_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (MaterialModel == null) return;
            CreateChildren();
            HighlightCollisions();
        }


        private void MaterialModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            HighlightCollisions();
        }

        private void HighlightCollisions()
        {
            var children = this.Children.OfType<TextBlock>().Where(CanManipulateElement).ToList();
            children.ForEach(c => c.Foreground = Brushes.Black);

            var childrenWithRects = children.Select(c => new Tuple<TextBlock, Rect>(c, GetRectFromElement(c))).ToArray();
            var childrenWithRectsToCompare = childrenWithRects.ToList();

            foreach (var child in childrenWithRects)
            {
                childrenWithRectsToCompare.Remove(child);
                var childRect = child.Item2;
                var childItem = child.Item1;

                foreach (var otherChild in childrenWithRectsToCompare)
                {
                    if (childRect.IntersectsWith(otherChild.Item2))
                    {
                        childItem.Foreground = Brushes.Red;
                        otherChild.Item1.Foreground = Brushes.Red;
                    }
                }

                if (childRect.Top < 0 || childRect.Left < 0 || childRect.Right > this.ActualWidth ||
                    childRect.Bottom > this.ActualHeight)
                {
                    childItem.Foreground = Brushes.Red;
                }
            }
        }

        private static Rect GetRectFromElement(UIElement element)
        {
            return new Rect(new Point(Canvas.GetLeft(element), Canvas.GetTop(element)), element.RenderSize);
        }

        private TextBlock CreateTextBlock(MaterialFieldModel materialFieldModel)
        {
            var textBlock = new TextBlock
            {
                Tag = materialFieldModel,
                DataContext = materialFieldModel,
                Margin = new Thickness(0),
                Focusable = true
            };

            textBlock.PreviewKeyUp += element_PreviewKeyUp;

            if (CanManipulateTextsAndLogos)
            {
                textBlock.Cursor = Cursors.Hand;
            }

            var fontFamilyBinding = new Binding("FontFamily") { Mode = BindingMode.OneWay };
            textBlock.SetBinding(TextBlock.FontFamilyProperty, fontFamilyBinding); 
            
            var fontStyleBinding = new Binding("Italic") { Mode = BindingMode.OneWay, Converter = new FontStyleConverter() };
            textBlock.SetBinding(TextBlock.FontStyleProperty, fontStyleBinding);

            var fontWeightBinding = new Binding("Bold") { Mode = BindingMode.OneWay, Converter = new FontWeightConverter() };
            textBlock.SetBinding(TextBlock.FontWeightProperty, fontWeightBinding);

            var fontSizeBinding = new Binding("FontSize") { Mode = BindingMode.OneWay };
            textBlock.SetBinding(TextBlock.FontSizeProperty, fontSizeBinding);

            var textBinding = new Binding("DisplayValue") { Mode = BindingMode.OneWay };
            textBlock.SetBinding(TextBlock.TextProperty, textBinding);

            var topBinding = new Binding("TopMargin") { Mode = BindingMode.TwoWay };
            textBlock.SetBinding(Canvas.TopProperty, topBinding);

            var leftBinding = new Binding("LeftMargin") { Mode = BindingMode.TwoWay };
            textBlock.SetBinding(Canvas.LeftProperty, leftBinding);

            return textBlock;
        }

        void element_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSelectedElements();
            }
        }

        public void DeleteSelectedElements()
        {
            var materialFieldModels = SelectedElements.Select(s => s.Tag).OfType<MaterialFieldModel>().ToArray();

            if (SelectedElements.Contains(logoImage))
            {
                MaterialModel.LogoModel.DeleteLogo();
            }

            foreach (var materialFieldModel in materialFieldModels)
            {
                MaterialModel.MaterialFields.Remove(materialFieldModel);
            }

            UnSelectAllElements();
        }

        private Image CreateLogoImage()
        {
            var logo = new Image
            {
                DataContext = MaterialModel.LogoModel,
                Stretch = Stretch.Fill,
                Focusable = true,
                Tag = MaterialModel.LogoModel
            };

            logo.PreviewKeyUp += element_PreviewKeyUp;

            if (CanManipulateTextsAndLogos || CanManipulateLogos)
            {
                logo.Cursor = Cursors.Hand;
            }

            var sourceBinding = new Binding("LogoImage") { Mode = BindingMode.OneWay };
            logo.SetBinding(Image.SourceProperty, sourceBinding);

            var heightBinding = new Binding("Height") { Mode = BindingMode.TwoWay };
            logo.SetBinding(HeightProperty, heightBinding);

            var widthBinding = new Binding("Width") { Mode = BindingMode.TwoWay };
            logo.SetBinding(WidthProperty, widthBinding);

            var topBinding = new Binding("TopMargin") { Mode = BindingMode.TwoWay };
            logo.SetBinding(Canvas.TopProperty, topBinding);

            var leftBinding = new Binding("LeftMargin") { Mode = BindingMode.TwoWay };
            logo.SetBinding(Canvas.LeftProperty, leftBinding);
            return logo;
        }

        //protected override void OnRender(DrawingContext dc)
        //{
        //    base.OnRender(dc);
        //    HighlightCollisions();
        //}

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint);

            if (Children.Count == 0)
            {
                return new Size();
            }

            var image = MaterialModel.FlyerFrontSideImage;
            return new Size(image.Width, image.Height);
        }

        protected override bool CanManipulateElements
        {
            get { return CanManipulateLogos || CanManipulateTextsAndLogos; }
        }

        protected override bool CanManipulateElement(UIElement source)
        {
            if (IsBackground(source))
            {
                return false;
            }

            var isLogo = ReferenceEquals(source, logoImage);
            if (CanManipulateTextsAndLogos == false && CanManipulateLogos == false)
            {
                //In PreviewMode Nothing can be selected
                return false;
            }

            if (CanManipulateTextsAndLogos == false && isLogo == false)
            {
                //Is DefaultMode only Logo can be manipulated
                return false;
            }
            return base.CanManipulateElement(source);
        }

        protected override bool IsBackground(UIElement source)
        {
            return ReferenceEquals(source, backgroundImage);
        }

        protected override Adorner CreateAdornerForElement(UIElement element)
        {
            if (element is TextBlock)
            {
                return new SelectionAdorner(element);
            }
            if (element is Image)
            {
                return new ResizingAdorner(element);
            }

            return new ResizingAdorner(element);
        }
    }
}