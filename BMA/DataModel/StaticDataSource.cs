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
            var existing = await LoadCachedStaticData();
            var live = await LoadLiveStaticData();

            await LoadTypeTransactionData(existing, live);
            await LoadTypeCategoryData(existing, live);
            await LoadTypeSavingsDencityData(existing, live);
            await LoadTypeTransactionReasonData(existing, live);
        }

        public async Task LoadTypeTransactionData(StaticTypeList existing, StaticTypeList live)
        {
            existing.TypeTransactions = existing.TypeTransactions ?? new List<TypeTransaction>();
            live.TypeTransactions = live.TypeTransactions ?? new List<TypeTransaction>();

            foreach (var item in live.TypeTransactions
                .Where(i => !existing.TypeTransactions.Contains(i, new BaseItemComparer())))
            {
                existing.TypeTransactions.Add(item);
                await StorageUtility.SaveItem(STATIC_FOLDER, item);
            }

            foreach (var itemExisting in existing.TypeTransactions.OrderBy(e => e.TypeTransactionId))
                foreach (var itemLive in live.TypeTransactions.OrderBy(e => e.Name))
                {
                    TypeTransactionList.Add(itemLive);
                }
        }
        public async Task LoadTypeCategoryData(StaticTypeList existing, StaticTypeList live)
        {
            existing.Categories = existing.Categories ?? new List<Category>();
            live.Categories = live.Categories ?? new List<Category>();

            foreach (var item in live.Categories
                .Where(i => !existing.Categories.Contains(i, new BaseItemComparer())))
            {
                existing.Categories.Add(item);
                await StorageUtility.SaveItem(STATIC_FOLDER, item);
            }

            foreach (var itemExisting in existing.Categories.OrderBy(e => e.CategoryId))
                foreach (var itemLive in live.Categories.OrderBy(e => e.Name))
                {
                    CategoryList.Add(itemLive);
                }
        }

        public async Task LoadTypeSavingsDencityData(StaticTypeList existing, StaticTypeList live)
        {
            existing.TypeSavingsDencities = existing.TypeSavingsDencities ?? new List<TypeSavingsDencity>();
            live.TypeSavingsDencities = live.TypeSavingsDencities ?? new List<TypeSavingsDencity>();

            foreach (var item in live.TypeSavingsDencities
                .Where(i => !existing.TypeSavingsDencities.Contains(i, new BaseItemComparer())))
            {
                existing.TypeSavingsDencities.Add(item);
                await StorageUtility.SaveItem(STATIC_FOLDER, item);
            }

            foreach (var itemExisting in existing.TypeSavingsDencities.OrderBy(e => e.TypeSavingsDencityId))
                foreach (var itemLive in live.TypeSavingsDencities.OrderBy(e => e.Name))
                {
                    TypeSavingsDencityList.Add(itemLive);
                }
        }

        public async Task LoadTypeTransactionReasonData(StaticTypeList existing, StaticTypeList live)
        {
            existing.TypeTransactionReasons = existing.TypeTransactionReasons ?? new List<TypeTransactionReason>();
            live.TypeTransactionReasons = live.TypeTransactionReasons ?? new List<TypeTransactionReason>();

            foreach (var item in live.TypeTransactionReasons
                .Where(i => !existing.TypeTransactionReasons.Contains(i, new BaseItemComparer())))
            {
                existing.TypeTransactionReasons.Add(item);
                await StorageUtility.SaveItem(STATIC_FOLDER, item);
            }

            foreach (var itemExisting in existing.TypeTransactionReasons.OrderBy(e => e.TypeTransactionReasonId))
                foreach (var itemLive in live.TypeTransactionReasons.OrderBy(e => e.Name))
                {
                    TypeTransactionReasonList.Add(itemLive);
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
        private async Task<StaticTypeList> LoadCachedStaticData()
        {
            var retVal = new StaticTypeList();
            foreach (var item in await StorageUtility.ListItems(STATIC_FOLDER))
            {
                try
                {
                    //var staticType = await StorageUtility.RestoreItem<StaticTypeList>(STATIC_FOLDER, item);
                    //retVal = staticType;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        #endregion
    }
}
