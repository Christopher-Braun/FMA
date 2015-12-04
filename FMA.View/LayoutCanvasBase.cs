using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FMA.View.Helpers;

namespace FMA.View
{
    public class LayoutCanvasBase : Canvas
    {
        private enum State
        {
            None,
            IsStartingSelection,
            IsDraggingSelectionRect,
            IsStartingDragging,
            IsDragging,
        }

        private State state = State.None;

        private AdornerLayer adornerLayer;
        private Border dragSelectionBorder;

        private Point origMouseDownPoint;

        private readonly List<SelectedElement> selectedElements = new List<SelectedElement>();

        static LayoutCanvasBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutCanvasBase), new FrameworkPropertyMetadata(typeof(LayoutCanvasBase)));
        }

        public LayoutCanvasBase()
        {
            this.ClipToBounds = true;
            this.Focus();
            this.Focusable = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            PreviewMouseDown += Canvas_MouseDown;
            PreviewMouseMove += Canvas_MouseMove;
            PreviewMouseUp += Canvas_MouseUp;
        }

        protected void CreateSelectionBorder()
        {
            dragSelectionBorder = new Border
            {
                Visibility = Visibility.Collapsed,
                BorderBrush = Brushes.Blue,
                BorderThickness = new Thickness(1),
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(2),
                Opacity = 0.5,
            };

            this.Children.Add(dragSelectionBorder);

            //TODO andere Stelle dafür suchen
            adornerLayer = AdornerLayer.GetAdornerLayer(this);
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CanManipulateElements == false)
            {
                return;
            }

            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            origMouseDownPoint = e.GetPosition(this);
            var hitTest = this.InputHitTest(origMouseDownPoint) as FrameworkElement;
            if (hitTest == null)
            {
                //Außerhalb des Bereiches oder falsches Element
                return;
            }

            if (IsBackground(hitTest))
            {
                //Klick auf Hintergrund - Beginne Selection
                state = State.IsStartingSelection;
                this.RemoveAllSelectedElements();
                this.CaptureMouse();
                e.Handled = true;
            }
            else if (CanManipulateElement(hitTest))
            {
                //Klick auf selektiertes Element - starte Bewegen / oder entfernen
                var selectedElementHit = selectedElements.FirstOrDefault(s => s.Element.Equals(hitTest));

                if (selectedElementHit != null)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        UnSelectElement(selectedElementHit);
                    }
                    else
                    {
                        state = State.IsStartingDragging;
                        selectedElements.ForEach(c => c.UpdateOrignalPosition());
                    }
                }
                else
                {
                    //Selektiere Element bzw füge zur Selektion hinzu
                    if (Keyboard.Modifiers != ModifierKeys.Control)
                    {
                        this.RemoveAllSelectedElements();
                    }

                    AddSelectedElement(hitTest);
                    state = State.IsStartingDragging;
                }

                e.Handled = true;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (CanManipulateElements == false)
            {
                return;
            }

            var mousePosition = e.GetPosition(this);

            switch (state)
            {
                case State.IsDraggingSelectionRect:
                    UpdateDragSelectionRect(origMouseDownPoint, mousePosition);
                    e.Handled = true;
                    break;
                case State.IsStartingSelection:
                    if (DragIsGreaterThanDragDelta(mousePosition))
                    {
                        state = State.IsDraggingSelectionRect;
                        RemoveAllSelectedElements();
                        InitDragSelectionRect(origMouseDownPoint, mousePosition);
                    }
                    e.Handled = true;
                    break;
                case State.IsStartingDragging:
                    if (DragIsGreaterThanDragDelta(mousePosition))
                    {
                        state = State.IsDragging;
                    }
                    e.Handled = true;
                    break;
            }

            if (state == State.IsDragging)
            {
                foreach (var selectedElement in selectedElements)
                {
                    var mouseDiff = mousePosition - origMouseDownPoint;

                    Canvas.SetTop(selectedElement.Element, selectedElement.OriginalTop + mouseDiff.Y);
                    Canvas.SetLeft(selectedElement.Element, selectedElement.OriginalLeft + mouseDiff.X);
                }
            }

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CanManipulateElements == false)
            {
                return;
            }

            if (e.ChangedButton != MouseButton.Left)
            {
                return;
            }

            switch (state)
            {
                case State.IsDraggingSelectionRect:
                    this.ReleaseMouseCapture();
                    ApplyDragSelectionRect();
                    e.Handled = true;
                    break;
                case State.IsStartingSelection:
                    this.ReleaseMouseCapture();
                    e.Handled = true;
                    break;
                case State.IsStartingDragging:
                    selectedElements.ForEach(c => c.UpdateOrignalPosition());
                    e.Handled = true;
                    break;
            }

            state = State.None;
        }

        //void Canvas_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
        //    {
        //        this.Children
        //            .OfType<FrameworkElement>()
        //            .Where(CanManipulateElement)
        //            .ToList()
        //            .ForEach(AddSelectedElement);
        //    }
        //}

        private bool DragIsGreaterThanDragDelta(Point mousePosition)
        {
            return ((Math.Abs(mousePosition.X - origMouseDownPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(mousePosition.Y - origMouseDownPoint.Y) > SystemParameters.MinimumVerticalDragDistance));
        }



        private void ApplyDragSelectionRect()
        {
            dragSelectionBorder.Visibility = Visibility.Collapsed;
            var x = Canvas.GetLeft(dragSelectionBorder);
            var y = Canvas.GetTop(dragSelectionBorder);
            var width = dragSelectionBorder.Width;
            var height = dragSelectionBorder.Height;
            var dragRect = new Rect(x, y, width, height);

            dragRect.Inflate(width / 10, height / 10);
            RemoveAllSelectedElements();
            foreach (var child in this.Children.OfType<FrameworkElement>().Where(CanManipulateElement))
            {
                var itemRect = new Rect(Canvas.GetLeft(child), Canvas.GetTop(child), child.ActualWidth, child.ActualHeight);
                if (!dragRect.Contains(itemRect))
                {
                    continue;
                }
                AddSelectedElement(child);
            }
        }

        private void InitDragSelectionRect(Point pt1, Point pt2)
        {
            UpdateDragSelectionRect(pt1, pt2);
            dragSelectionBorder.Visibility = Visibility.Visible;
        }

        private void UpdateDragSelectionRect(Point pt1, Point pt2)
        {
            double x, y, width, height;
            if (pt2.X < pt1.X)
            {
                x = pt2.X;
                width = pt1.X - pt2.X;
            }
            else
            {
                x = pt1.X;
                width = pt2.X - pt1.X;
            }
            if (pt2.Y < pt1.Y)
            {
                y = pt2.Y;
                height = pt1.Y - pt2.Y;
            }
            else
            {
                y = pt1.Y;
                height = pt2.Y - pt1.Y;
            }
            Canvas.SetLeft(dragSelectionBorder, x);
            Canvas.SetTop(dragSelectionBorder, y);

            dragSelectionBorder.Width = width;
            dragSelectionBorder.Height = height;

        }

        private void AddSelectedElement(FrameworkElement child)
        {
            if (!CanManipulateElement(child))
            {
                throw new InvalidOperationException("child");
            }
            selectedElements.Add(new SelectedElement(child));

            adornerLayer.Add(CreateAdornerForElement(child));
        }

        private void RemoveAllSelectedElements()
        {
            selectedElements.ForEach(RemoveAdorner);
            selectedElements.Clear();
        }

        private void UnSelectElement(SelectedElement selectedElement)
        {
            selectedElements.Remove(selectedElement);
            RemoveAdorner(selectedElement);
        }

        private void RemoveAdorner(SelectedElement selectedElement)
        {
            var adorners = adornerLayer.GetAdorners(selectedElement.Element);
            if (adorners != null) adornerLayer.Remove(adorners[0]);
        }

        protected void ClearChildren()
        {
            this.Children.Clear();
            this.selectedElements.Clear();
        }

        protected virtual Adorner CreateAdornerForElement(UIElement child)
        {
            return new ResizingAdorner(child);

        }

        protected virtual bool CanManipulateElement(UIElement child)
        {
            return true;
        }

        protected virtual bool CanManipulateElements
        {
            get
            {
                return true;
            }
        }


        protected virtual bool IsBackground(UIElement source)
        {
            return this.Children.Contains(source) == false;
        }

        private class SelectedElement
        {
            public SelectedElement(FrameworkElement element)
            {
                Element = element;
                UpdateOrignalPosition();
            }

            public FrameworkElement Element { get; private set; }

            public Double OriginalTop { get; private set; }
            public Double OriginalLeft { get; private set; }

            public void UpdateOrignalPosition()
            {
                OriginalLeft = Canvas.GetLeft(Element);
                OriginalTop = Canvas.GetTop(Element);
            }
        }

    }
}
