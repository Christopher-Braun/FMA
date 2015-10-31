﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FMA.TestData;

namespace FMA.View
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FMA.View"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FMA.View;assembly=FMA.View"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:LayoutCanvas/>
    ///
    /// </summary>
    public class LayoutCanvas : Control
    {
        public static readonly DependencyProperty MaterialModelProperty = DependencyProperty.Register(
            "MaterialModel", typeof(MaterialModel), typeof(LayoutCanvas),
            new PropertyMetadata(DummyData.GetDummyMaterials().First().ToMaterialModel(), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (LayoutCanvas) d;
            me.MaterialModelChanged();
        }


        public MaterialModel MaterialModel
        {
            get { return (MaterialModel) GetValue(MaterialModelProperty); }
            set { SetValue(MaterialModelProperty, value); }
        }


        static LayoutCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (LayoutCanvas),
                new FrameworkPropertyMetadata(typeof (LayoutCanvas)));
        }

        private Canvas canvas;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var child = this.GetTemplateChild("Canvas");
            this.canvas = (Canvas) child;
            
            SetEvents();

            MaterialModelChanged();
        }

        private void MaterialModelChanged()
        {
            if (canvas == null || MaterialModel == null) return;

            this.canvas.Children.Clear();
            this.canvas.Children.Add(new Image {Source = this.MaterialModel.FlyerFrontSideImage});

            foreach (var materialFieldModel in MaterialModel.MaterialFields)
            {
                var textBlock = new TextBlock
                {
                    Text = materialFieldModel.Value,
                    FontSize = materialFieldModel.FontSize,
                    Tag = materialFieldModel
                };

                this.canvas.Children.Add(textBlock);

                Canvas.SetTop(textBlock, materialFieldModel.TopMargin);
                Canvas.SetLeft(textBlock, materialFieldModel.LeftMargin);
            }
        }



          AdornerLayer aLayer;

        bool _isDown;
        bool _isDragging;
        bool selected = false;
        UIElement selectedElement = null;

        Point _startPoint;
        private double _originalLeft;
        private double _originalTop;


        private void SetEvents()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Window1_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
            this.MouseMove += new MouseEventHandler(Window1_MouseMove);
            this.MouseLeave += new MouseEventHandler(Window1_MouseLeave);

            canvas.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(myCanvas_PreviewMouseLeftButtonDown);
            canvas.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
        }

        // Handler for drag stopping on leaving the window
        void Window1_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Handler for drag stopping on user choise
        void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Method for stopping dragging
        private void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;
                _isDragging = false;
            }
        }

        // Hanler for providing drag operation with selected element
        void Window1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(canvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(canvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    _isDragging = true;

                if (_isDragging)
                {
                    Point position = Mouse.GetPosition(canvas);
                    var topMargin = position.Y - (_startPoint.Y - _originalTop);
                    Canvas.SetTop(selectedElement, topMargin);
                    var LeftMargin = position.X - (_startPoint.X - _originalLeft);
                    Canvas.SetLeft(selectedElement, LeftMargin);

                    //TODO HACK
                    var model = (selectedElement as FrameworkElement).Tag as MaterialFieldModel;

                    //TODO Cast nicht gut
                    model.TopMargin = (int)topMargin;
                    model.LeftMargin = (int)LeftMargin;
                }
            }
        }

        // Handler for clearing element selection, adorner removal
        void Window1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
        }

        // Handler for element selection on the canvas providing resizing adorner
        void myCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Remove selection on clicking anywhere the window
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    // Remove the adorner from the selected element
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }

            // If any element except canvas is clicked, 
            // assign the selected element and add the adorner
            if (e.Source != canvas)
            {
                _isDown = true;
                _startPoint = e.GetPosition(canvas);

                selectedElement = e.Source as UIElement;

                _originalLeft = Canvas.GetLeft(selectedElement);
                _originalTop = Canvas.GetTop(selectedElement);

                aLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                aLayer.Add(new ResizingAdorner(selectedElement));
                selected = true;
                e.Handled = true;
            }
        }
    }
}
