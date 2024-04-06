using System;
using FriendStorage.UI.ViewModel;
using Moq;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
  public class MainViewModelTests
  {
    private Mock<INavigationViewModel> navigationViewModelMock;
    private MainViewModel viewModel;

    public MainViewModelTests()
    {
        navigationViewModelMock = new Mock<INavigationViewModel>();
        viewModel = new MainViewModel(navigationViewModelMock.Object);

    }

    [Fact]
    public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
    {
      viewModel.Load();
        navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
    }
  }
}
