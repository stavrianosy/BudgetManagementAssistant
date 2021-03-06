﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BMA_WP.Resources;
using BMA_WP.ViewModel.Admin;
using BMA.BusinessLogic;

namespace BMA_WP.View.AdminView
{
    public partial class TypeFrequency : PhoneApplicationPage
    {
        public TypeFrequencyViewModel vm
        {
            get { return (TypeFrequencyViewModel)DataContext; }
        }

        public TypeFrequency()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!App.Instance.IsAuthorized)
                NavigationService.Navigate(new Uri("/View/Login.xaml", UriKind.Relative));
        }


        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string piName = (e.AddedItems[0] as PivotItem).Name;

            switch (piName)
            {
                case "piTypeFrequency":
                    SetupAppBar_TypeFrequency();
                   svItem.ScrollToVerticalOffset(0d);
                    break;
                case "piTypeFrequencyList":
                    SetupAppBar_TypeFrequencyList();
                    break;
            }
        }

        private void SetupAppBar_TypeFrequencyList()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = false;
        }

        private void SetupAppBar_TypeFrequency()
        {
            ApplicationBar = new ApplicationBar();
            ApplicationBar.IsVisible = true;

            ApplicationBarIconButton save = new ApplicationBarIconButton();
            save.IconUri = new Uri("/Assets/icons/Dark/save.png", UriKind.Relative);
            save.Text = AppResources.AppBarButtonSave;
            ApplicationBar.Buttons.Add(save);
            save.Click += new EventHandler(Save_Click);

            ApplicationBarIconButton saveContinute = new ApplicationBarIconButton();
            saveContinute.IconUri = new Uri("/Assets/icons/Dark/refresh.png", UriKind.Relative);
            saveContinute.Text = AppResources.AppBarButtonContinue;
            ApplicationBar.Buttons.Add(saveContinute);
            save.Click += new EventHandler(Continue_Click);

            ApplicationBarIconButton delete = new ApplicationBarIconButton();
            delete.IconUri = new Uri("/Assets/icons/Dark/delete.png", UriKind.Relative);
            delete.Text = AppResources.AppBarButtonDelete;
            ApplicationBar.Buttons.Add(delete);
            save.Click += new EventHandler(Delete_Click);

            ApplicationBarIconButton add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/icons/Dark/add.png", UriKind.Relative);
            add.Text = AppResources.AppBarButtonAdd;
            ApplicationBar.Buttons.Add(add);
            add.Click += new EventHandler(Add_Click);

            ApplicationBarMenuItem mainMenu = new ApplicationBarMenuItem();
            mainMenu.Text = AppResources.AppBarButtonMainMenu;
            ApplicationBar.MenuItems.Add(mainMenu);
            mainMenu.Click += new EventHandler(MainMenu_Click);

        }

        private void MainMenu_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/View/MainPage.xaml", UriKind.Relative));
        }

        private void Continue_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            svItem.ScrollToVerticalOffset(0d);
            //var item = new TypeTransaction();
            //vm.TypeFrequencies.Add(item);
            //TypeFrequenciesMultiSelect.SelectedItem = item;
        }
    }
}