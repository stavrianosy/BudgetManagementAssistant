using BMA_WP.Model;
using BMA_WP.Resources;
using BMA_WP.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        StaticServiceData.ServerStatus status;

        private RelayCommand<SelectionChangedEventArgs> _goToCommand_SelectionChanged = null;
        private RelayCommand<string> _goToCommand_NavigateTo = null;
        private RelayCommand<string> _goToCommand_SelectionChangedParam = null;

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
                    //_goToCommand_SelectionChanged = new RelayCommand<SelectionChangedEventArgs>(NavigateAway);
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
        /// <summary>
        /// Initializes a new instance of the MainPageViewModel class.
        /// </summary>
        public MainPageViewModel()
        {
            HubTileTransactions = new MenuItem { Name = AppResources.MainMenuTransactions, SubName = "a11", Description = "a22", IconPath = "/Assets/SplashScreen.png", NavigateTo = "/View/Transactions.xaml" };
            HubTileBudgets = new MenuItem { Name = AppResources.MainMenuBudgets, SubName = "b11", Description = "b22", IconPath = "/Assets/SplashScreen.png", NavigateTo = "/View/Budgets.xaml" };

            MainMenuList = new ObservableCollection<MenuItem> { 
                new MenuItem{Name=AppResources.MainMenuTransactions, SubName="a11", Description="a22", NavigateTo="/View/Transactions.xaml"},
                new MenuItem{Name=AppResources.MainMenuBudgets, SubName="b11", Description="b22", NavigateTo="/View/Budgets.xaml"},
            };

            ReportsMenuList = new ObservableCollection<MenuItem> { 
                new MenuItem{Name=AppResources.ReportsMenuTransactionAmount, SubName="a11", IconPath="/Assets/icons/Dark/delete.png", Description="a22", NavigateTo="/View/Transactions.xaml"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionBudget, SubName="b11", IconPath="/Assets/icons/Dark/delete.png", Description="b22", NavigateTo="/View/Budgets.xaml"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionCategory, SubName="c11", IconPath="/Assets/icons/Dark/delete.png", Description="c22", NavigateTo="3"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionReason, SubName="c11", IconPath="/Assets/icons/Dark/delete.png", Description="c22", NavigateTo="4"},
                new MenuItem{Name=AppResources.ReportsMenuTransactionPlace, SubName="c11", IconPath="/Assets/icons/Dark/delete.png", Description="c22", NavigateTo="5"},
            };

            AdminMenuList = new ObservableCollection<MenuItem> { 
                new MenuItem{Name=AppResources.AdminMenuBudgetThreshold, SubName=AppResources.MenuDescriptionBudgetThreshold, Description=AppResources.MenuDescriptionBudgetThreshold, IconPath="/Assets/icons/Dark/edit.png", NavigateTo="/View/AdminView/BudgetThreshold.xaml"},
                new MenuItem{Name=AppResources.AdminMenuCategories, SubName=AppResources.MenuDescriptionCategory, Description=AppResources.MenuDescriptionCategory, IconPath="/Assets/icons/Dark/edit.png",NavigateTo="/View/AdminView/Category.xaml"},
                new MenuItem{Name=AppResources.AdminMenuInterval, SubName=AppResources.MenuDescriptionInterval, Description=AppResources.MenuDescriptionInterval, IconPath="/Assets/icons/Dark/edit.png",NavigateTo="/View/AdminView/Interval.xaml"},
                new MenuItem{Name=AppResources.AdminMenuFrequencies, SubName=AppResources.MenuDescriptionFrequency, Description=AppResources.MenuDescriptionFrequency, IconPath="/Assets/icons/Dark/edit.png", NavigateTo="/View/AdminView/TypeFrequency.xaml"},
                new MenuItem{Name=AppResources.AdminMenuNotifications, SubName=AppResources.MenuDescriptionNotification, Description=AppResources.MenuDescriptionNotification, IconPath="/Assets/icons/Dark/edit.png",NavigateTo="/View/AdminView/Notification.xaml"},
                new MenuItem{Name=AppResources.AdminMenuTransactionReasons, SubName=AppResources.MenuDescriptionTransactionReason, Description=AppResources.MenuDescriptionTransactionReason, IconPath="/Assets/icons/Dark/edit.png", NavigateTo="/View/AdminView/Reason.xaml"},
                new MenuItem{Name=AppResources.AdminMenuSecurity, SubName=AppResources.MenuDescriptionSecurity, Description=AppResources.MenuDescriptionSecurity, IconPath="/Assets/icons/Dark/edit.png", NavigateTo="/View/AdminView/Security.xaml"},
            };
        }
    }
}