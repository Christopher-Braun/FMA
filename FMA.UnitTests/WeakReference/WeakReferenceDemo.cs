using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FMA.UnitTests.WeakReference
{
    [TestClass]
    public class WeakReferenceDemo
    {
        //Due to use of static fields, this Tests do NOT run parallel / all at once

        [TestMethod]
        public void WeakReference_ObjectIsDiposedWhenNotReferenced()
        {
            Action create = () =>
            {
                new GuiElement();
            };

            create();

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsTrue(GuiElement.IsDisposed);
        }


        [TestMethodAttribute]
        public void WeakReference_ObjectIsNotDiposedWhenReferenced()
        {
            Func<GuiElement> create = () => new GuiElement();

            var refence = create();

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsFalse(GuiElement.IsDisposed);
        }

        [TestMethodAttribute]
        public void WeakReference_ObjectIsDiposedWhenReferencedByWeakReference()
        {
            Func<GuiElement> create = () => new GuiElement();

            var reference = new System.WeakReference(create());

            Assert.IsTrue(reference.IsAlive);
            Assert.IsNotNull(reference.Target);
            var hash = reference.Target.GetHashCode();

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsTrue(GuiElement.IsDisposed);
            Assert.IsFalse(reference.IsAlive);
            Assert.IsNull(reference.Target);
        }

        [TestMethod]
        public void WeakReference_ObjectIsDiposedWhenNotReferencend_Copy()
        {
            Action create = () =>
            {
                var a = new GuiElement();
            };

            create();

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsTrue(GuiElement.IsDisposed);
        }

        [TestMethod]
        public void WeakReference_SubscriptionKeepsReceiverAlive()
        {
            var model = new Model();

            Action create = () =>
            {
                var a = new GuiElement();
                model.PropertyChanged += a.SomethingHappened;
            };

            create();

            model.FirePropertyChanged("Doesn`t matter");
            Assert.IsTrue(GuiElement.SomethingHappenedWasCalled);

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsFalse(GuiElement.IsDisposed);
        }

        [TestMethod]
        public void WeakReference_PropertyChangedEventManager()
        {
            var model = new Model();

            Action create = () =>
            {
                var a = new GuiElement();
                PropertyChangedEventManager.AddHandler(model, a.SomethingHappened, "");
            };

            create();

            model.FirePropertyChanged("Doesn`t matter");
            Assert.IsTrue(GuiElement.SomethingHappenedWasCalled);

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsTrue(GuiElement.IsDisposed);
        }


        [TestMethod]
        public void WeakReference_PropertyChangedEventManager_Filter()
        {
            var model = new Model();

            Action create = () =>
            {
                var a = new GuiElement();
                PropertyChangedEventManager.AddHandler(model, a.SomethingHappened, "IsSelected");
            };

            create();

            model.FirePropertyChanged("Doesn`t matter");
            Assert.IsFalse(GuiElement.SomethingHappenedWasCalled);

            model.FirePropertyChanged("IsSelected");
            Assert.IsTrue(GuiElement.SomethingHappenedWasCalled);

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsTrue(GuiElement.IsDisposed);
        }


        [TestMethod]
        public void WeakReference_ConditionalWeakTable()
        {
            var model = new Model();
            var dictionary = new ConditionalWeakTable<GuiElement, Model>();

            Action create = () =>
            {
                var a = new GuiElement();
                dictionary.Add(a, model);


                Model model2;
                dictionary.TryGetValue(a, out model2);

                Assert.IsNotNull(model2);
            };

            create();

            GC.Collect();
            Thread.Sleep(1000);

            Assert.IsTrue(GuiElement.IsDisposed);

            GC.Collect();
            Thread.Sleep(1000);

            var setYourBreakPointHere = 0;

        }

    }
}