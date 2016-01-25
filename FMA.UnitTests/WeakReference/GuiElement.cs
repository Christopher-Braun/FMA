using System.ComponentModel;

namespace FMA.UnitTests.WeakReference
{
    public class GuiElement
    {
        public static bool IsDisposed;
        public static bool SomethingHappenedWasCalled;

        public GuiElement()
        {
            IsDisposed = false;
            SomethingHappenedWasCalled = false;
        }

        public void SomethingHappened(object sender, PropertyChangedEventArgs e)
        {
            SomethingHappenedWasCalled = true;
        }

        ~GuiElement()
        {
            IsDisposed = true;
        }
    }
}