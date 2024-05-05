using FriendStorage.Model;
using System;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Command;
using System.Windows.Input;
using FriendStorage.UI.Wrapper;
using Prism.Events;
using FriendStorage.UI.Events;
using System.Windows;
using FriendStorage.UI.Dialogs;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int? friendId);
        FriendWrapper Friend { get; }
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider;
        private FriendWrapper _friend;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public FriendEditViewModel(IFriendDataProvider dataProvider,
          IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService)
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
        }

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public FriendWrapper Friend
        {
            get
            {
                return _friend;
            }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public void Load(int? friendId)
        {
            var friend = friendId.HasValue
              ? _dataProvider.GetFriendById(friendId.Value)
              : new Friend();

            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += Friend_PropertyChanged;

            InvalidateCommands();
        }

        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateCommands();
        }

        private void InvalidateCommands()
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
        }

        private void OnSaveExecute(object obj)
        {
            _dataProvider.SaveFriend(Friend.Model);
            Friend.AcceptChanges();
            _eventAggregator.GetEvent<FriendSavedEvent>().Publish(Friend.Model);
        }

        private bool OnSaveCanExecute(object arg)
        {
            return Friend != null && Friend.IsChanged;
        }

        private void OnDeleteExecute(object obj)
        {
            var result = _messageDialogService.ShowYesNoDialog("Delete Friend",
              $"Do you really want to delete the friend '{Friend.FirstName} {Friend.LastName}'");
            if (result == MessageDialogResult.Yes)
            {
                _dataProvider.DeleteFriend(Friend.Id);
                _eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private bool OnDeleteCanExecute(object arg)
        {
            return Friend != null && Friend.Id > 0;
        }
    }
}
