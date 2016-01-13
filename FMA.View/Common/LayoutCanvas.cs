using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using FMA.View.Helpers;

namespace FMA.View.Common
{
    public class LayoutCanvas : Canvas
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

        private AdornerLayer AdornerLayer
        {
            get
            {
                return AdornerLayer.GetAdornerLayer(this);
            }
        }
        private Border dragSelectionBorder;

        private Point origMouseDownPoint;

        private readonly ObservableCollection<SelectedChild> selectedChilds = new ObservableCollection<SelectedChild>();

        static LayoutCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LayoutCanvas), new FrameworkPropertyMetadata(typeof(LayoutCanvas)));
        }

        public LayoutCanvas()
        {
            this.ClipToBounds = true;
            this.Focusable = true;
            this.Focus();
        }

        public void AlignLeft()
        {
            var position = this.SelectedChilds.Select(e => GetLeft(e.Element)).Min();
            this.SelectedChilds.ToList().ForEach(e => SetLeft(e.Element, position));
        }

        public void AlignRight()
        {
            var position = this.SelectedChilds.Select(e => GetLeft(e.Element) + e.Element.ActualWidth).Max();
            this.SelectedChilds.ToList().ForEach(e => SetLeft(e.Element, position - e.Element.ActualWidth));
        }

        public void AlignTop()
        {
            var position = this.SelectedChilds.Select(e => GetTop(e.Element)).Min();
            this.SelectedChilds.ToList().ForEach(e => SetTop(e.Element, position));
        }

        public void AlignBottom()
        {
            var position = this.SelectedChilds.Select(e => GetTop(e.Element) + e.Element.ActualHeight).Max();
            this.SelectedChilds.ToList().ForEach(e => SetTop(e.Element, position - e.Element.ActualHeight));
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
                BorderThickness = new Thickness(1.5),
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(2),
                Opacity = 0.6
            };

            this.Children.Add(dragSelectionBorder);
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
                this.UnSelectAllElements();
                this.CaptureMouse();
                e.Handled = true;
            }
            else if (CanManipulateElement(hitTest))
            {
                //Klick auf selektiertes Element - starte Bewegen / oder entfernen
                var selectedElementHit = SelectedChilds.FirstOrDefault(s => s.Element.Equals(hitTest));

                if (selectedElementHit != null)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        UnSelectElement(selectedElementHit);
                    }
                    else
                    {
                        state = State.IsStartingDragging;
                        SelectedChilds.ToList().ForEach(c => c.UpdateOrignalPosition());
                    }
                }
                else
                {
                    //Selektiere Element bzw füge zur Selektion hinzu
                    if (Keyboard.Modifiers != ModifierKeys.Control)
                    {
                        this.UnSelectAllElements();
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
                        UnSelectAllElements();
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
                foreach (var selectedElement in SelectedChilds)
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
                    SelectedChilds.ToList().ForEach(c => c.UpdateOrignalPosition());
                    e.Handled = true;
                    break;
            }

            state = State.None;
        }

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
            UnSelectAllElements();
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

        protected void AddSelectedElement(FrameworkElement child, bool focus = false)
        {
            if (!CanManipulateElement(child))
            {
                throw new InvalidOperationException("child");
            }

            if (AdornerLayer == null)
            {
                return;
            }

            if (focus)
            {
                child.Focus();
            }

            SelectedChilds.Add(new SelectedChild(child));

            AdornerLayer.Add(CreateAdornerForElement(child));
        }

        protected void UnSelectAllElements()
        {
            SelectedChilds.ToList().ForEach(RemoveAdorner);
            SelectedChilds.Clear();
        }

        protected void UnSelectElement(FrameworkElement child)
        {
            var selectedChild = this.selectedChilds.FirstOrDefault(s => s.Element == child);
            if (selectedChild == null)
            {
                return;
            }
            UnSelectElement(selectedChild);
        }

        private void UnSelectElement(SelectedChild selectedChild)
        {
            SelectedChilds.Remove(selectedChild);
            RemoveAdorner(selectedChild);
        }

        private void RemoveAdorner(SelectedChild selectedChild)
        {
            if (AdornerLayer == null) { return; }

            var adorners = AdornerLayer.GetAdorners(selectedChild.Element);
            if (adorners != null) AdornerLayer.Remove(adorners[0]);
        }

        protected void ClearChildren()
        {
            this.Children.Clear();
            this.SelectedChilds.Clear();
        }

        protected virtual Adorner CreateAdornerForElement(UIElement child)
        {
            return new ResizingAdorner(child);

        }

        protected virtual bool CanManipulateElement(UIElement child)
        {
            return this.Children.Contains(child) && !ReferenceEquals(child, dragSelectionBorder);
        }

        protected virtual bool CanManipulateElements
        {
            get
            {
                return true;
            }
        }

        protected IEnumerable<FrameworkElement> SelectedElements
        {
            get { return SelectedChilds.Select(s => s.Element); }
        }

        protected ObservableCollection<SelectedChild> SelectedChilds
        {
            get { return selectedChilds; }
        }


        protected virtual bool IsBackground(UIElement source)
        {
            return this.Children.Contains(source) == false;
        }

        protected class SelectedChild
        {
            public SelectedChild(FrameworkElement element)
            {
                Element = element;
                UpdateOrignalPosition();
            }

            public FrameworkElement Element { get; private set; }

            public double OriginalTop { get; private set; }
            public double OriginalLeft { get; private set; }

            public void UpdateOrignalPosition()
            {
                OriginalLeft = Canvas.GetLeft(Element);
                OriginalTop = Canvas.GetTop(Element);
            }
        }

    }
}
