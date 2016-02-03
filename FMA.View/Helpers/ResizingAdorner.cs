using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FMA.View.Helpers
{
    public class ResizingAdorner : Adorner
    {
        // Resizing adorner uses Thumbs for visual elements.  
        // The Thumbs have built-in mouse input handling.
        private readonly Thumb topLeft;
        private readonly Thumb topRight;
        private readonly Thumb bottomLeft;
        private readonly Thumb bottomRight;

        private readonly Thumb left;
        private readonly Thumb right;
        private readonly Thumb bottom;
        private readonly Thumb top;

        private readonly VisualCollection visualChildren;

        public ResizingAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsClipEnabled = true;
            ClipToBounds = true;

            visualChildren = new VisualCollection(this);

            topLeft = CreateAdornerCorner(Cursors.SizeNWSE);

            topRight = CreateAdornerCorner(Cursors.SizeNESW);
            bottomLeft = CreateAdornerCorner(Cursors.SizeNESW);
            bottomRight = CreateAdornerCorner(Cursors.SizeNWSE);

            left = CreateAdornerCorner(Cursors.SizeWE);
            right = CreateAdornerCorner(Cursors.SizeWE);
            top = CreateAdornerCorner(Cursors.SizeNS);
            bottom = CreateAdornerCorner(Cursors.SizeNS);

            bottomLeft.DragDelta += HandleDrag;
            bottomRight.DragDelta += HandleDrag;
            topLeft.DragDelta += HandleDrag;
            topRight.DragDelta += HandleDrag;

            left.DragDelta += HandleDrag;
            right.DragDelta += HandleDrag;
            top.DragDelta += HandleDrag;
            bottom.DragDelta += HandleDrag;
        }

        private void HandleDrag(object sender, DragDeltaEventArgs args)
        {
            var hitThumb = sender as Thumb;
            if (hitThumb == null) return;

            EnforceSize();

            if (ReferenceEquals(hitThumb, bottomRight))
            {
                HandleBottomRight(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, bottomLeft))
            {
                HandleBottomLeft(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, topLeft))
            {
                HandleTopLeft(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, topRight))
            {
                HandleTopRight(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, bottom))
            {
                HandleBottom(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, right))
            {
                HandleRight(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, top))
            {
                HandleTop(hitThumb, args);
            }
            else if (ReferenceEquals(hitThumb, left))
            {
                HandleLeft(hitThumb, args);
            }
        }

        private void HandleRight(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var newWidth = Math.Max(Adorned.Width + args.HorizontalChange, hitThumb.DesiredSize.Width);
            Adorned.Width = newWidth;
        }

        private void HandleBottom(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var newHeight = Math.Max(Adorned.Height + args.VerticalChange, hitThumb.DesiredSize.Width);
            Adorned.Height = newHeight;
        }

        private void HandleLeft(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var widthOld = Adorned.Width;
            var leftOld = Canvas.GetLeft(Adorned);

            var newWidth = Math.Max(Adorned.Width - args.HorizontalChange, hitThumb.DesiredSize.Width);
            Adorned.Width = newWidth;

            Canvas.SetLeft(Adorned, leftOld - (Adorned.Width - widthOld));
        }

        private void HandleTop(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var heightOld = Adorned.Height;
            var topOld = Canvas.GetTop(Adorned);

            var newHeight = Math.Max(Adorned.Height - args.VerticalChange, hitThumb.DesiredSize.Width);
            Adorned.Height = newHeight;

            Canvas.SetTop(Adorned, topOld - (Adorned.Height - heightOld));
        }

        private void HandleBottomRight(Thumb hitThumb, DragDeltaEventArgs args)
        {
            ApplySizeChange(new Point(args.HorizontalChange, args.VerticalChange), hitThumb);
        }

        private void HandleTopRight(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var heightOld = Adorned.Height;
            var topOld = Canvas.GetTop(Adorned);

            ApplySizeChange(new Point(args.HorizontalChange, -args.VerticalChange), hitThumb);

            Canvas.SetTop(Adorned, topOld - (Adorned.Height - heightOld));
        }

        private void HandleTopLeft(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var widthOld = Adorned.Width;
            var leftOld = Canvas.GetLeft(Adorned);

            var heightOld = Adorned.Height;
            var topOld = Canvas.GetTop(Adorned);

            ApplySizeChange(new Point(-args.HorizontalChange, -args.VerticalChange), hitThumb);

            Canvas.SetLeft(Adorned, leftOld - (Adorned.Width - widthOld));
            Canvas.SetTop(Adorned, topOld - (Adorned.Height - heightOld));
        }

        private void HandleBottomLeft(Thumb hitThumb, DragDeltaEventArgs args)
        {
            var widthOld = Adorned.Width;
            var leftOld = Canvas.GetLeft(Adorned);

            ApplySizeChange(new Point(-args.HorizontalChange, args.VerticalChange), hitThumb);

            Canvas.SetLeft(Adorned, leftOld - (Adorned.Width - widthOld));
        }

        private void ApplySizeChange(Point change, Thumb hitThumb)
        {
            var newHeight = Math.Max(Adorned.Height + change.Y, hitThumb.DesiredSize.Height);
            var newWidth = Math.Max(Adorned.Width + change.X, hitThumb.DesiredSize.Width);

            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                var scaleFactor = Math.Max(newWidth / Adorned.Width, newHeight / Adorned.Height);

                Adorned.Height *= scaleFactor;
                Adorned.Width *= scaleFactor;
            }
            else
            {
                Adorned.Height = newHeight;
                Adorned.Width = newWidth;
            }
        }

        private FrameworkElement Adorned
        {
            get { return AdornedElement as FrameworkElement; }
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            // desiredWidth and desiredHeight are the width and height of the element that's being adorned.  
            // These will be used to place the ResizingAdorner at the corners of the adorned element.  
            var desiredWidth = AdornedElement.DesiredSize.Width;
            var desiredHeight = AdornedElement.DesiredSize.Height;
            // adornerWidth & adornerHeight are used for placement as well.
            var adornerWidth = DesiredSize.Width;
            var adornerHeight = DesiredSize.Height;

            topLeft.Arrange(new Rect(-adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            topRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, -adornerHeight / 2, adornerWidth, adornerHeight));
            bottomLeft.Arrange(new Rect(-adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            bottomRight.Arrange(new Rect(desiredWidth - adornerWidth / 2, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));

            left.Arrange(new Rect(-adornerWidth / 2, 0, adornerWidth, adornerHeight));
            right.Arrange(new Rect(desiredWidth - adornerWidth / 2, 0, adornerWidth, adornerHeight));
            bottom.Arrange(new Rect(0, desiredHeight - adornerHeight / 2, adornerWidth, adornerHeight));
            top.Arrange(new Rect(0, -adornerHeight / 2, adornerWidth, adornerHeight));

            // Return the final size.
            return finalSize;
        }

        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        private Thumb CreateAdornerCorner(Cursor customizedCursor)
        {
            var cornerThumb = new Thumb
            {
                Cursor = customizedCursor,
                Height = 15,
                Width = 15,
                Opacity = 0.5,
                Background = new SolidColorBrush(Colors.Blue),
                BorderThickness = new Thickness(0),
                BorderBrush = Brushes.Silver
            };

            visualChildren.Add(cornerThumb);
            return cornerThumb;
        }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        private void EnforceSize()
        {
            if (Adorned.Width.Equals(double.NaN))
                Adorned.Width = Adorned.DesiredSize.Width;
            if (Adorned.Height.Equals(double.NaN))
                Adorned.Height = Adorned.DesiredSize.Height;

            var parent = Adorned.Parent as FrameworkElement;
            if (parent != null)
            {
                Adorned.MaxHeight = parent.ActualHeight;
                Adorned.MaxWidth = parent.ActualWidth;
            }
        }

        protected override int VisualChildrenCount
        {
            get { return visualChildren.Count; }
        }


        protected override Visual GetVisualChild(int index)
        {
            return visualChildren[index];
        }
    }
}