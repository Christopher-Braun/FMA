using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using static System.Windows.Interop.HwndSource;

namespace FMA.View.Helpers
{
    internal class WindowAspectRatio
    {
        private readonly double ratio;

        private WindowAspectRatio(FrameworkElement window)
        {
            ratio = window.Width / window.Height;
            // ReSharper disable once AccessToStaticMemberViaDerivedType
            var hwndSource = PresentationSource.FromVisual(window) as HwndSource;
            hwndSource?.AddHook(DragHook);
        }

        public static void Register(Window window)
        {
            // ReSharper disable once ObjectCreationAsStatement
            new WindowAspectRatio(window);
        }

        internal enum WindowsPos
        {
            WindowPosChanging = 0x0046,
        }

        [Flags]
        public enum Swp
        {
            NoMove = 0x2,
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowPosition
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;  
            public int cy;
            public int flags;
        }

        private IntPtr DragHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handeled)
        {
            if ((WindowsPos) msg != WindowsPos.WindowPosChanging) return IntPtr.Zero;

            var position = (WindowPosition)Marshal.PtrToStructure(lParam, typeof(WindowPosition));

            var hwndSource   = FromHwnd(hwnd);
            if (hwndSource != null && ((position.flags & (int)Swp.NoMove) != 0 || hwndSource.RootVisual == null)) return IntPtr.Zero;

            position.cx = (int)(position.cy * ratio);

            Marshal.StructureToPtr(position, lParam, true);
            handeled = true;

            return IntPtr.Zero;
        }
    }
}