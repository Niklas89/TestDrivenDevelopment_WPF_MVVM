﻿using FriendStorage.DataAccess;
using FriendStorage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendStorage.UI.DataProvider
{
    class NavigationDataProvider : INavigationDataProvider
    {
        private Func<IDataService> _dataServiceCreator;

        public NavigationDataProvider(Func<IDataService> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }
        public IEnumerable<LookupItem> GetAllFriends()
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetAllFriends();
            }
        }
    }
}
