using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FriendStorage.Model;
using Moq;
using Prism.Events;

namespace FriendStorage.UITests.ViewModel
{
  public class NavigationViewModelTests
  {
        private NavigationViewModel viewModel;

        public NavigationViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(dp => dp.GetAllFriends())
                .Returns(new List<LookupItem>
                {
                    new LookupItem { Id = 1, DisplayMember = "Julia" },
                    new LookupItem { Id = 2, DisplayMember = "Thomas" }
                });
            viewModel = new NavigationViewModel(
              navigationDataProviderMock.Object,
        eventAggregatorMock.Object);
        }

    [Fact]
    public void ShouldLoadFriends()
    {
      viewModel.Load();

      Assert.Equal(2, viewModel.Friends.Count);

      var friend = viewModel.Friends.SingleOrDefault(f => f.Id == 1);
      Assert.NotNull(friend);
      Assert.Equal("Julia", friend.DisplayMember);

      friend = viewModel.Friends.SingleOrDefault(f => f.Id == 2);
      Assert.NotNull(friend);
      Assert.Equal("Thomas", friend.DisplayMember);
    }

    [Fact]
    public void ShouldLoadFriendsOnlyOnce()
    {
      viewModel.Load();
      viewModel.Load();

      Assert.Equal(2, viewModel.Friends.Count);
    }
  }
}
