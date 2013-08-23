using BMA_WP.Model;
using BMA_WP.Resources;
using BMA_WP.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;


namespace BMA_WP.ViewModel
{
    public class MenuItem
    {

        public string Name{get;set;}
        public string SubName{get;set;}
        public string IconPath { get; set; }
        public string Description{get;set;}
        public string NavigateTo { get; set; }

    }
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        #region Private Members
        StaticServiceData.ServerStatus status;

        RelayCommand<SelectionChangedEventArgs> _goToCommand_SelectionChanged = null;
        RelayCommand<string> _goToCommand_NavigateTo = null;
        #endregion

        public bool IsLoading { get { return App.Instance.IsSyncing; } set { App.Instance.IsSyncing = value; } }
        public StaticServiceData.ServerStatus Status { get { return status; } set { status = value; RaisePropertyChanged("Status"); } }

        public RelayCommand<string> GoToCommand_NavigateTo
        {
            get
            {
                if (_goToCommand_NavigateTo == null)
                    _goToCommand_NavigateTo = new RelayCommand<string>(NavigateAwayParam);

                return _goToCommand_NavigateTo;
            }
        }

        public ICommand GoToCommand_SelectionChanged
        {
            get
            {
                if (_goToCommand_SelectionChanged == null)
                    _goToCommand_SelectionChanged = new RelayCommand<SelectionChangedEventArgs>((e) => 
                    { 
                        PageNavigationService.NavigateTo(typeof(MainPage), ((MenuItem)e.AddedItems[0]).NavigateTo); 
                        
                    });

                return _goToCommand_SelectionChanged;
            }
        }

        private void NavigateAway(SelectionChangedEventArgs e)
        {
            PageNavigationService.NavigateTo(typeof(MainPage), ((MenuItem)e.AddedItems[0]).NavigateTo);
        }

        private void NavigateAwayParam(string uri)
        {
            PageNavigationService.NavigateTo(typeof(MainPage), uri);
        }

        public ObservableCollection<MenuItem> MainMenuList { get; set; }
        public ObservableCollection<MenuItem> ReportsMenuList { get; set; }
        public ObservableCollection<MenuItem> AdminMenuList { get; set; }

        public MenuItem HubTileTransactions { get; set; }
        public MenuItem HubTileBudgets { get; set; }

        private bool IsLightThemeUsed()
        {
            //var theme = (System.Windows.Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
            var theme = (System.Windows.Visibility)Application.Current.Resources["PhoneLightThemeVisibility"];
            return theme == Visibility.Visible;
        }

        /// <summary>
        /// Initializes a new instance of the MainPageViewModel class.
        /// </summary>
        public MainPageViewModel()
        {
            var themAssetsDir = IsLightThemeUsed() ? "Light" : "Dark";


            HubTileTransactions = new MenuItem { Name = AppResources.MainMenuTransactions, SubName = "a11", Description = "a22", IconPath = "/Assets/TileTransactions.png", NavigateTo = "/View/Transactions.xaml" };
            HubTileBudgets = new MenuItem { Name = AppResources.MainMenuBudgets, SubName = "b11", Description = "b22", IconPath = "/Assets/TileBudgets.png", NavigateTo = "/View/Budgets.xaml" };

            MainMenuList = new ObservableCollection<MenuItem> { 
                new MenuItem{Name=AppResources.MainMenuTransactions, SubName="a11", Description="a22", NavigateTo="/View/Transactions.xaml"},
                new MenuItem{Name=AppResources.MainMenuBudgets, SubName="b11", Description="b22", NavigateTo="/View/Budgets.xaml"},
            };

            ReportsMenuList = new ObservableCollection<MenuItem> { 
                new MenuItem{Name=AppResources.ReportsMenuTransactionAmount, SubName="a11", IconPath=string.Format("/Assets/icons/{0}/reports.png", themAssetsDir), Description=AppResources.MenuDescriptionReportAmount, NavigateTo="/View/ReportsView/TransactionAmount.xaml"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionByPeriod, SubName="b11", IconPath=string.Format("/Assets/icons/{0}/reports.png", themAssetsDir), Description=AppResources.MenuDescriptionReportPeriod, NavigateTo="/View/ReportsView/TransactionByPeriod.xaml"},
                //new MenuItem{Name=AppResources.ReportsMenuTransactionBudget, SubName="b11", IconPath=string.Format("/Assets/icons/{0}/reports.png", themAssetsDir), Description="b22", NavigateTo="/View/ReportsView/TransactionBudget.xaml"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionCategory, SubName="c11", IconPath=string.Format("/Assets/icons/{0}/reports.png", themAssetsDir), Description=AppResources.MenuDescriptionReportCategory, NavigateTo="/View/ReportsView/TransactionCategory.xaml"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionReason, SubName="c11", IconPath=string.Format("/Assets/icons/{0}/reports.png", themAssetsDir), Description=AppResources.MenuDescriptionReportReason, NavigateTo="/View/ReportsView/TransactionReason.xaml"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionPlace, SubName="c11", IconPath=string.Format("/Assets/icons/{0}/reports.png", themAssetsDir), Description=AppResources.MenuDescriptionReportNameOfPlace, NavigateTo="/View/ReportsView/TransactionPlace.xaml"},
            };

            AdminMenuList = new ObservableCollection<MenuItem> { 
                new MenuItem{Name=AppResources.AdminMenuCategories, SubName=AppResources.MenuDescriptionCategory, Description=AppResources.MenuDescriptionCategory, IconPath=string.Format("/Assets/icons/{0}/category.png", themAssetsDir),NavigateTo="/View/AdminView/Category.xaml"},
                new MenuItem{Name=AppResources.AdminMenuInterval, SubName=AppResources.MenuDescriptionInterval, Description=AppResources.MenuDescriptionInterval, IconPath=string.Format("/Assets/icons/{0}/refresh.png", themAssetsDir),NavigateTo="/View/AdminView/Interval.xaml"},
                new MenuItem{Name=AppResources.AdminMenuNotifications, SubName=AppResources.MenuDescriptionNotification, Description=AppResources.MenuDescriptionNotification, IconPath=string.Format("/Assets/icons/{0}/feature.alarm.png", themAssetsDir),NavigateTo="/View/AdminView/Notification.xaml"},
                new MenuItem{Name=AppResources.AdminMenuTransactionReasons, SubName=AppResources.MenuDescriptionTransactionReason, Description=AppResources.MenuDescriptionTransactionReason, IconPath=string.Format("/Assets/icons/{0}/reason.png", themAssetsDir), NavigateTo="/View/AdminView/Reason.xaml"},
                new MenuItem{Name=AppResources.AdminMenuSecurity, SubName=AppResources.MenuDescriptionSecurity, Description=AppResources.MenuDescriptionSecurity, IconPath=string.Format("/Assets/icons/{0}/admin.png", themAssetsDir), NavigateTo="/View/AdminView/Security.xaml"},
            };
        }
    }
}