using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel;

using BMA.BusinessLogic;
using BMA.Common;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.Networking.Connectivity;

namespace BMA.DataModel
{
    public class StaticDataSource:LayoutAwarePage
    {
        #region Private members
        private const string STATIC_FOLDER = "Static";
        private const string STATIC_CATEGORY_FOLDER = "Static_Category";
        private const string STATIC_TYPETRANSACTION_FOLDER = "Static_TypeTransaction";
        private const string STATIC_TYPETRANSACTIONREASON_FOLDER = "Static_TypeTransactionReason";
        private const string STATIC_TYPESAVINGFREQUENCY_FOLDER = "Static_TypeSavingsFrequency";

        private static readonly string Utf8ByteOrderMark =
                    Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

        #endregion

        #region Public members
        public ObservableCollection<Category> CategoryList { get; set; }
        public ObservableCollection<TypeTransaction> TypeTransactionList { get; set; }
        public ObservableCollection<TypeTransactionReason> TypeTransactionReasonList { get; set; }
        public ObservableCollection<TypeSavingsDencity> TypeSavingsDencityList { get; set; }
        #endregion

        #region Constructor
        public StaticDataSource()
        {
            CategoryList = new ObservableCollection<Category>();
            TypeTransactionList = new ObservableCollection<TypeTransaction>();
            TypeTransactionReasonList = new ObservableCollection<TypeTransactionReason>();
            TypeSavingsDencityList = new ObservableCollection<TypeSavingsDencity>();
        }
        #endregion

        #region Public methods
        public async Task LoadStaticData()
        {
            StaticTypeList existing = new StaticTypeList();
            var info = NetworkInformation.GetInternetConnectionProfile();
            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                existing.TypeTransactions = await LoadCachedTypeTransaction(STATIC_TYPETRANSACTION_FOLDER);
                existing.Categories = await LoadCachedCategory(STATIC_CATEGORY_FOLDER);
                existing.TypeTransactionReasons = await LoadCachedTypeTransactionReason(STATIC_TYPETRANSACTIONREASON_FOLDER);
                existing.TypeSavingsDencities = await LoadCachedTypeSavingsDencity(STATIC_TYPESAVINGFREQUENCY_FOLDER);
            }
            else
            {
                existing = await LoadLiveStaticData();
                await UpdateCacheStaticData(existing);
            }
//            var live = await LoadLiveStaticData();

            LoadTypeTransactionData(existing.TypeTransactions);
            LoadTypeCategoryData(existing.Categories);
            LoadTypeSavingsDencityData(existing.TypeSavingsDencities);
            LoadTypeTransactionReasonData(existing.TypeTransactionReasons);
        }

        public void LoadTypeTransactionData(List<TypeTransaction> existing)
        {
            existing = existing ?? new List<TypeTransaction>();
            //live.TypeTransactions = live.TypeTransactions ?? new List<TypeTransaction>();

            foreach (var item in existing)
            {
                if (TypeTransactionList.Where(t => t.TypeTransactionId == item.TypeTransactionId).Count() == 0)
                    TypeTransactionList.Add(item);
            }
        }
        public void LoadTypeCategoryData(List<Category> existing)
        {
            existing = existing ?? new List<Category>();

            foreach (var item in existing)
            {
                if (CategoryList.Where(t => t.CategoryId == item.CategoryId).Count() == 0)
                    CategoryList.Add(item);
            }
        }

        public void LoadTypeSavingsDencityData(List<TypeSavingsDencity> existing)
        {
            existing = existing ?? new List<TypeSavingsDencity>();

            foreach (var item in existing)
            {
                if (TypeSavingsDencityList.Where(t => t.TypeSavingsDencityId== item.TypeSavingsDencityId).Count() == 0)
                    TypeSavingsDencityList.Add(item);
            }
        }

        public void LoadTypeTransactionReasonData(List<TypeTransactionReason> existing)
        {
            existing = existing ?? new List<TypeTransactionReason>();

            foreach (var item in existing)
            {
                if (TypeTransactionReasonList.Where(t => t.TypeTransactionReasonId == item.TypeTransactionReasonId).Count() == 0)
                    TypeTransactionReasonList.Add(item);
            }
        }
#endregion

        #region Private methods
        private static async Task<StaticTypeList> LoadLiveStaticData()
        {
            var retVal = new StaticTypeList();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAService.MainClient();
                var result = await client.GetAllStaticDataAsync();

                retVal = result;
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                throw;
            }

            return retVal;
        }
        private async Task<List<Category>> LoadCachedCategory(string folder)
        {
            var retVal = new List<Category>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<Category>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeTransaction>> LoadCachedTypeTransaction(string folder)
        {
            var retVal = new List<TypeTransaction>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransaction>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeTransactionReason>> LoadCachedTypeTransactionReason(string folder)
        {
            var retVal = new List<TypeTransactionReason>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeTransactionReason>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private async Task<List<TypeSavingsDencity>> LoadCachedTypeSavingsDencity(string folder)
        {
            var retVal = new List<TypeSavingsDencity>();
            foreach (var item in await StorageUtility.ListItems(folder))
            {
                try
                {
                    var staticType = await StorageUtility.RestoreItem<TypeSavingsDencity>(folder, item);
                    retVal.Add(staticType);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        #endregion
        #region Save
        public async Task SaveCategory(ObservableCollection<Category> categories)
        {
            var client = new BMAService.MainClient();
            var result = await client.SaveCategoriesAsync(categories);
        }
        #endregion

        private async Task UpdateCacheStaticData(StaticTypeList staticDataList)
        {
            await StorageUtility.Clear(STATIC_CATEGORY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPESAVINGFREQUENCY_FOLDER);
            await StorageUtility.Clear(STATIC_TYPETRANSACTION_FOLDER);
            await StorageUtility.Clear(STATIC_TYPETRANSACTIONREASON_FOLDER);

            foreach (var item in staticDataList.Categories)
                await StorageUtility.SaveItem(STATIC_CATEGORY_FOLDER, item, item.CategoryId);

            foreach (var item in staticDataList.TypeTransactions)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTION_FOLDER, item, item.TypeTransactionId);

            foreach (var item in staticDataList.TypeTransactionReasons)
                await StorageUtility.SaveItem(STATIC_TYPETRANSACTIONREASON_FOLDER, item, item.TypeTransactionReasonId);

            foreach (var item in staticDataList.TypeSavingsDencities)
                await StorageUtility.SaveItem(STATIC_TYPESAVINGFREQUENCY_FOLDER, item, item.TypeSavingsDencityId);
        }
    }
}
