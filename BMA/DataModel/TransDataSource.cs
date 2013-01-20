using BMA.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Web.Syndication;

namespace BMA.DataModel
{
    public class TransDataSource
    {
        private const string GROUP_FOLDER = "Groups";

        private static readonly string Utf8ByteOrderMark =
            Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble(), 0, Encoding.UTF8.GetPreamble().Length);

        public TransDataSource()
        {
            GroupList = new ObservableCollection<TransGroup>();

            if (DesignMode.DesignModeEnabled)
            {
                LoadTestGroups();
            }
        }

        public async Task LoadGroups()
        {
            if (Config.TestOnly)
            {
                LoadTestGroups();
            }
            else
            {
                await LoadAllGroups();
            }

        }

        public async Task LoadAllGroups()
        {
            var existing = await LoadCachedGroups();
            var live = await LoadLiveGroups();

            foreach (var liveGroup in live
                .Where(liveGroup => !existing.Contains(liveGroup, new BaseItemComparer())))
            {
                existing.Add(liveGroup);
                await StorageUtility.SaveItem(GROUP_FOLDER, liveGroup);
            }

            //foreach (var group in existing.OrderBy(e => e.Title))
                foreach (var group in live.OrderBy(e => e.Title))
            {
                GroupList.Add(group);
            }
        }

        private async Task<IList<TransGroup>> LoadCachedGroups()
        {
            var retVal = new List<TransGroup>();
            foreach (var item in await StorageUtility.ListItems(GROUP_FOLDER))
            {
                try
                {
                    var group = await StorageUtility.RestoreItem<TransGroup>(GROUP_FOLDER, item);
                    retVal.Add(group);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return retVal;
        }

        private static async Task<IList<TransGroup>> LoadLiveGroups()
        {

            var retVal = new List<TransGroup>();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            try
            {
                var client = new BMAService.MainClient();
                var result = await client.GetAllTransactionsAsync();

                foreach (var item in result)
                {
                    var group = new TransGroup
                    {
                        Id = item.TransactionId,
                        CategoryId = new Random().Next(1,3),
                        Title = item.Category.Name,
                        ImagePath = item.Amount.ToString()
                    };

                    retVal.Add(group);
                }
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(string.Format("There was an error accessing the weather service.\n\r{0}", ex.Message));
                //throw;
            }

            return retVal;
        }

        private static async Task<IList<TransGroup>> LoadLiveGroupsRSS()
        {
            var retVal = new List<TransGroup>();
            var info = NetworkInformation.GetInternetConnectionProfile();

            if (info == null || info.GetNetworkConnectivityLevel() != NetworkConnectivityLevel.InternetAccess)
            {
                return retVal;
            }

            var content = await PathIO.ReadTextAsync("ms-appx:///Assets/Transactions.js");
            content = content.Trim(Utf8ByteOrderMark.ToCharArray());
            var transactions = JsonArray.Parse(content);

            foreach (JsonValue item in transactions)
            {
                MessageDialog dialog = null;

                try
                {
                    var uri = item.GetObject()["TransUri"].GetString();
                    //var group = new BlogGroup
                    //{
                    //    Id = uri,
                    //    RssUri = new Uri(uri, UriKind.Absolute)
                    //};

                    //var client = GetSyndicationClient();
                    //var feed = await client.RetrieveFeedAsync(group.RssUri);

                    //group.Title = feed.Title.Text;

                    //retVal.Add(group);
                }
                catch (Exception ex)
                {
                    dialog = new MessageDialog(ex.Message);
                }
                if (dialog != null)
                {
                    await dialog.ShowAsync();
                }
            }

            return retVal;
        }

        private void LoadTestGroups()
        {
        }

        private static async Task<IList<TransItem>> LoadCachedItems(TransGroup group)
        {
            var retVal = new List<TransItem>();

            var groupFolder = group.Id.GetHashCode().ToString();

            foreach (var item in await StorageUtility.ListItems(groupFolder))
            {
                try
                {
                    var post = await StorageUtility.RestoreItem<TransItem>(groupFolder, item);
                    post.Group = group;
                    retVal.Add(post);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return retVal;
        }

        public async Task LoadAllItems(TransGroup group)
        {
            var cachedItems = await LoadCachedItems(group);
        }

        public ObservableCollection<TransGroup> GroupList { get; set; }

        private static SyndicationClient GetSyndicationClient()
        {
            var client = new SyndicationClient
            {
                BypassCacheOnRetrieve = false
            };
            //client.SetRequestHeader("user-agent", USER_AGENT);
            return client;
        }
        private static HttpClient GetClient()
        {
            var retVal = new HttpClient
            {
                MaxResponseContentBufferSize = 999999
            };
            //retVal.DefaultRequestHeaders.Add("user-agent", USER_AGENT);
            return retVal;
        }
        
    }
}
