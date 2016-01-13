using FMA.Contracts;
using FMA.TestData;
using FMA.View.Common;
using FMA.View.DefaultView;
using FMA.View.Helpers;
using FMA.View.LayoutView;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FMA.UnitTests
{
    [TestClass]
    public class FlyerMakerViewModelTest
    {
        private FlyerMakerViewModel CreateSUT()
        {
            var fontServiceMock = new Mock<IFontService>();
            fontServiceMock.Setup(f => f.IsFontAvailable(It.IsAny<string>())).Returns(true);

            var windowServiceMock = new Mock<IWindowService>();
            return new FlyerMakerViewModel(DummyData.GetDummyMaterials(),1, s => default(FontInfo), fontServiceMock.Object, windowServiceMock.Object);
        }

        [TestMethod]
        public void FlyerMakerViewModel_SetLayoutMode_SetsFlyerViewModel_ToLayoutViewModel()
        {
            var sut = CreateSUT();

            sut.LayoutMode = true;
            Assert.AreEqual( typeof(LayoutViewModel) ,sut.FlyerViewModel.GetType());

            sut.LayoutMode = false;
            Assert.AreEqual( typeof(DefaultViewModel) ,sut.FlyerViewModel.GetType());
        }
    }
}
