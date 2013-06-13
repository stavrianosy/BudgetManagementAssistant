﻿/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:BMA_WP.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using BMA_WP.ViewModel.Admin;
using BMA_WP.ViewModel.ReportsView;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace BMA_WP.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                 //SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                 //SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<TransactionViewModel>();
            SimpleIoc.Default.Register<BudgetViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();

            #region Admin
            //## Admin ##//
            SimpleIoc.Default.Register<BudgetThresholdViewModel>();
            SimpleIoc.Default.Register<CategoryViewModel>();
            SimpleIoc.Default.Register<IntervalViewModel>();
            SimpleIoc.Default.Register<NotificationViewModel>();
            SimpleIoc.Default.Register<ReasonViewModel>();
            SimpleIoc.Default.Register<SecurityViewModel>();
            SimpleIoc.Default.Register<TypeFrequencyViewModel>();
            #endregion

            #region Reports
            //## Reports ##//
            SimpleIoc.Default.Register<TransactionAmountViewModel>();
            SimpleIoc.Default.Register<TransactionBudgetViewModel>();
            SimpleIoc.Default.Register<TransactionCategoryViewModel>();
            SimpleIoc.Default.Register<TransactionPeriodViewModel>();
            SimpleIoc.Default.Register<TransactionPlaceViewModel>();
            #endregion
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public TransactionViewModel TransactionViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionViewModel>();
            }
        }
        public BudgetViewModel BudgetViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BudgetViewModel>();
            }
        }
        public MainPageViewModel MainPageViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainPageViewModel>();
            }
        }
        public LoginViewModel LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

        #region Admin
        public BudgetThresholdViewModel BudgetThresholdViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<BudgetThresholdViewModel>();
            }
        }
        public CategoryViewModel CategoryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CategoryViewModel>();
            }
        }
        public IntervalViewModel IntervalViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IntervalViewModel>();
            }
        }
        public NotificationViewModel NotificationViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NotificationViewModel>();
            }
        }
        public ReasonViewModel ReasonViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReasonViewModel>();
            }
        }
        public SecurityViewModel SecurityViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SecurityViewModel>();
            }
        }
        public TypeFrequencyViewModel TypeFrequencyViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TypeFrequencyViewModel>();
            }
        }
        
        #endregion

        #region Reports
        public TransactionAmountViewModel TransactionAmountViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionAmountViewModel>();
            }
        }
        public TransactionBudgetViewModel TransactionBudgetViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionBudgetViewModel>();
            }
        }

        public TransactionCategoryViewModel TransactionCategoryViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionCategoryViewModel>();
            }
        }

        public TransactionPeriodViewModel TransactionperiodViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionPeriodViewModel>();
            }
        }

        public TransactionPlaceViewModel TransactionPlaceViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionPlaceViewModel>();
            }
        }
        #endregion
    }
}