using System;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace FMA.View.View.Common
{
    public class MaterialCanvas : LayoutCanvas
    {
        public static readonly DependencyProperty MaterialModelProperty = DependencyProperty.Register(
            "MaterialModel", typeof(MaterialModel), typeof(MaterialCanvas), new PropertyMetadata(null, (d, e) => ((MaterialCanvas)d).MaterialModelChanged(e)));
        
        public MaterialModel MaterialModel
        {
            get { return (MaterialModel)GetValue(MaterialModelProperty); }
            set { SetValue(MaterialModelProperty, value); }
        }

        public static readonly DependencyProperty FontServiceProperty = DependencyProperty.Register(
            "FontService", typeof(FontService), typeof(MaterialCanvas), new PropertyMetadata(default(FontService)));

        public FontService FontService
        {
            get { return (FontService)GetValue(FontServiceProperty); }
            set { SetValue(FontServiceProperty, value); }
        }

        public static readonly DependencyProperty CanManipulateTextsProperty = DependencyProperty.Register(
            "CanManipulateTexts", typeof(bool), typeof(MaterialCanvas), new PropertyMetadata(default(bool)));

        public bool CanManipulateTexts
        {
            get { return (bool)GetValue(CanManipulateTextsProperty); }
            set { SetValue(CanManipulateTextsProperty, value); }
        }

        public static readonly DependencyProperty CanManipulateLogosProperty = DependencyProperty.Register(
            "CanManipulateLogos", typeof(bool), typeof(MaterialCanvas), new PropertyMetadata(true));

        public bool CanManipulateLogos
        {
            get { return (bool)GetValue(CanManipulateLogosProperty); }
            set { SetValue(CanManipulateLogosProperty, value); }
        }

        static MaterialCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MaterialCanvas), new FrameworkPropertyMetadata(typeof(MaterialCanvas)));
        }

        private Image backgroundImage;
        private Image logoImage;

        public MaterialCanvas()
        {
            this.AllowDrop = true;
            this.Drop += (sender, e) => this.DropLogo(this.MaterialModel, e);
            SelectedChilds.CollectionChanged += SelectedElements_CollectionChanged;
        }

        private void SelectedElements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (MaterialModel == null) { return; }

            var selectedMaterialChilds = SelectedElements.Select(s => s.Tag).OfType<MaterialChildModel>().ToList();

            foreach (var materialField in MaterialModel.MaterialFields)
            {
                materialField.IsSelected = selectedMaterialChilds.Contains(materialField);
            }

            MaterialModel.LogoModel.IsSelected = selectedMaterialChilds.Contains(MaterialModel.LogoModel);
        }

        private void MaterialModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (MaterialModel == null)
            {
                //this means view is killed
                RemoveEvents(e.OldValue as MaterialModel);
                return;
            }
           
            CreateChildren();
            AddEvents();
        }

        
        private void CreateChildren()
        {
            ClearChildren();

            backgroundImage = new Image { Source = MaterialModel.FlyerFrontSideImage, Cursor = Cursors.Arrow };
            Children.Add(backgroundImage);

            logoImage = CreateLogoImage(MaterialModel.LogoModel);

            foreach (var materialField in MaterialModel.MaterialFields)
            {
                CreateTextBlock(materialField);
            }

            CreateSelectionBorder();
        }

        private void CreateTextBlock(MaterialFieldModel materialField)
        {
            var textBlock = new TextBlock
            {
                Tag = materialField,
                DataContext = materialField,
                Margin = new Thickness(0),
                Focusable = true
            };

            if (CanManipulateTexts)
            {
                textBlock.Cursor = Cursors.Hand;
                textBlock.PreviewKeyUp += element_PreviewKeyUp;
            }

            var fontFamilyBinding = new Binding("FontFamilyWithName.FontFamily") { Mode = BindingMode.OneWay };
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
            textBlock.SetBinding(TopProperty, topBinding);

            var leftBinding = new Binding("LeftMargin") { Mode = BindingMode.TwoWay };
            textBlock.SetBinding(LeftProperty, leftBinding);

            Children.Add(textBlock);
        }

        private Image CreateLogoImage(LogoModel logoModel)
        {
            var createdLogoImage = new Image
            {
                DataContext = logoModel,
                Stretch = Stretch.Fill,
                Focusable = true,
                Tag = logoModel
            };

            if (CanManipulateLogos)
            {
                createdLogoImage.Cursor = Cursors.Hand;
                createdLogoImage.PreviewKeyUp += element_PreviewKeyUp;
            }

            var sourceBinding = new Binding("LogoImage") { Mode = BindingMode.OneWay };
            createdLogoImage.SetBinding(Image.SourceProperty, sourceBinding);

            var heightBinding = new Binding("Height") { Mode = BindingMode.TwoWay };
            createdLogoImage.SetBinding(HeightProperty, heightBinding);

            var widthBinding = new Binding("Width") { Mode = BindingMode.TwoWay };
            createdLogoImage.SetBinding(WidthProperty, widthBinding);

            var topBinding = new Binding("TopMargin") { Mode = BindingMode.TwoWay };
            createdLogoImage.SetBinding(TopProperty, topBinding);

            var leftBinding = new Binding("LeftMargin") { Mode = BindingMode.TwoWay };
            createdLogoImage.SetBinding(LeftProperty, leftBinding);

            Children.Add(createdLogoImage);

            return createdLogoImage;
        }

      
        private void AddEvents()
        {
            PropertyChangedEventManager.AddHandler(MaterialModel, MaterialModel_PropertyChanged, "");

            if (CanManipulateLogos)
            {
                PropertyChangedEventManager.AddHandler(MaterialModel.LogoModel, MaterialChild_IsSelectedChanged, "IsSelected");
            }
            if (CanManipulateTexts)
            {
                foreach (var materialField in MaterialModel.MaterialFields)
                {
                    PropertyChangedEventManager.AddHandler(materialField, MaterialChild_IsSelectedChanged, "IsSelected");
                }

                CollectionChangedEventManager.AddHandler(MaterialModel.MaterialFields, MaterialFields_CollectionChanged);
            }
        }
       
        private void RemoveEvents(MaterialModel materialModel)
        {
            PropertyChangedEventManager.RemoveHandler(materialModel, MaterialModel_PropertyChanged, "");
            if (CanManipulateLogos)
            {
                PropertyChangedEventManager.RemoveHandler(materialModel.LogoModel, MaterialChild_IsSelectedChanged, "IsSelected");
            }
            if (CanManipulateTexts)
            {
                foreach (var materialField in materialModel.MaterialFields)
                {
                    PropertyChangedEventManager.RemoveHandler(materialField, MaterialChild_IsSelectedChanged, "IsSelected");
                }

                CollectionChangedEventManager.RemoveHandler(materialModel.MaterialFields, MaterialFields_CollectionChanged);
            }
        }

        private void MaterialFields_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (MaterialModel == null) return;
            CreateChildren();
            HighlightCollisions();
        }

        private void MaterialModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            HighlightCollisions();
        }

        private void MaterialChild_IsSelectedChanged(object sender, PropertyChangedEventArgs e)
        {
            var materialChildModel = sender as MaterialChildModel;
            if (materialChildModel != null )
            {
                MaterialChildIsSelectedChanged(materialChildModel);
            }
        }
 
        private void MaterialChildIsSelectedChanged(MaterialChildModel materialChild)
        {
            var frameworkElement = this.Children.OfType<FrameworkElement>().FirstOrDefault(f => f.Tag == materialChild);
            if (frameworkElement == null)
            {
                return;
            }
            if (CanManipulateElement(frameworkElement) == false)
            {
                return;
            }

            if (materialChild.IsSelected)
            {
                AddSelectedElement(frameworkElement, true);
            }
            else
            {
                UnSelectElement(frameworkElement);
            }
        }


        private void HighlightCollisions()
        {
            if (CanManipulateTexts == false)
            {
                return;
            }

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

                if (childRect.Top < 0 || childRect.Left < 0 || childRect.Right > ActualWidth ||
                    childRect.Bottom > ActualHeight)
                {
                    childItem.Foreground = Brushes.Red;
                }
            }
        }

        private static Rect GetRectFromElement(UIElement element)
        {
            return new Rect(new Point(GetLeft(element), GetTop(element)), element.RenderSize);
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


        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            HighlightCollisions();
        }

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
            get { return CanManipulateLogos || CanManipulateTexts; }
        }

        protected override bool CanManipulateElement(UIElement source)
        {
            if (IsBackground(source))
            {
                return false;
            }

            var isLogo = ReferenceEquals(source, logoImage);
            if (CanManipulateTexts == false && CanManipulateLogos == false)
            {
                //In PreviewMode Nothing can be selected
                return false;
            }

            if (CanManipulateTexts == false && isLogo == false)
            {
                //In DefaultMode only Logo can be manipulated
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