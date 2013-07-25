﻿using BMA.BusinessLogic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Shell;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.Generic;

namespace BMA_WP.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TransactionViewModel : ViewModelBase
    {
        #region Private Members
        private bool _isEnabled;
        private bool _isLoading;
        private Transaction _currTransaction;
        private TransactionImageList _currTransactionImages;
        private int pivotIndex;
        #endregion

        #region Public Properties
        public bool IsEnabled { get { return _isEnabled; } set { _isEnabled = value; RaisePropertyChanged("IsEnabled"); } }
        public bool IsLoading { get { return _isLoading; } set { _isLoading = value; RaisePropertyChanged("IsLoading"); } }
        public Transaction CurrTransaction { get { return _currTransaction; } 
            set 
            {
                _currTransaction = value;
                RaisePropertyChanged("CurrTransaction");
            } }


        public TransactionList Transactions { get { return App.Instance.ServiceData.TransactionList; } }
        //public TransactionImageList CurrTransactionImages { get { return _currTransactionImages; } set { _currTransactionImages = value; RaisePropertyChanged("CurrTransactionImages"); } }
        public ObservableCollection<TypeTransaction> TransactionTypeList { get { return App.Instance.StaticServiceData.TypeTransactionList; } }
        public ObservableCollection<TypeTransactionReason> TransactionReasonTypeList { get { return App.Instance.StaticServiceData.TypeTransactionReasonList; } }
        public ObservableCollection<Category> CategoryList { get { return App.Instance.StaticServiceData.CategoryList; } }
        
        public int PivotIndex { get { return pivotIndex; } set { pivotIndex = value; RaisePropertyChanged("PivotIndex"); } }
        

                #endregion

        #region Event To Commands
        public ICommand Transactions_SelectionChanged
        {
            get
            {
                return new RelayCommand<object>((param) =>
                {
                    var selectedItem = (SelectionChangedEventArgs)param;
                    if (selectedItem.AddedItems[0] != null)
                        PivotIndex = 0;
                });
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the TransactionViewModel class.
        /// </summary>
        public TransactionViewModel()
        {
            IsEnabled = false;
            App.Instance.ServiceData.LoadTransactions();
            PivotIndex = 0;
        }


    }
}