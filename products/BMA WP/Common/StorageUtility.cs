using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

using BMA.BusinessLogic;
using System.Runtime.Serialization;

namespace BMA_WP.Common
{
    public class StorageUtility
    {
        public static async Task<string[]> ListItems(string folderName, string userName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            var files = new List<string>();
            var folder = await ApplicationData.Current.LocalFolder
                                             .CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            //var userId = 0;

            if (userName != null && userName.Length > 0)
            {
                var folderUser = await folder.GetFolderAsync(userName);
                files.AddRange((from file in await folderUser.GetFilesAsync() select file.Name).ToList());
            }
            else
            {
                var folderList = await folder.GetFoldersAsync();

                foreach (var item in folderList)
                {
                    files.AddRange((from file in await item.GetFilesAsync() select file.Name).ToList());
                }
            }
            return files.ToArray();
        }

        public static async Task<T> RestoreItem<T>(string folderName, string hash, string userName)
            where T : BaseItem, new()
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            var folderUsersAll = await folder.GetFoldersAsync();

            //StorageFile file = await folder.GetFileAsync(hash);
            StorageFile file = null;

            if (userName != null && userName.Length > 0)
            {
                var folderUser = folderUsersAll.FirstOrDefault(x => x.Name == (userName));
                file = await folderUser.GetFileAsync(hash);
            }
            else
            {
                foreach (var item in folderUsersAll)
                {
                    file = (await item.GetFilesAsync()).FirstOrDefault(x => x.Name == hash);
                    if (file != null)
                        break;
                }
            }

            //var inStream = await file.OpenSequentialReadAsync();
            var inStream = await file.OpenStreamForReadAsync();
            var serializer = new DataContractSerializer(typeof(T));
            var retVal = (T)serializer.ReadObject(inStream);
            return retVal;
        }

        public static async Task DeleteItem<T>(string folderName, T item, int id)
            where T : BaseItem, new()
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }


            try
            {
                var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
                var file = await folder.GetFileAsync(id.GetHashCode().ToString());

                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static async Task Clear(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            try
            {
                await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.ReplaceExisting);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public static async Task SaveItem<T>(string folderName, T item, int id, string userName)
            where T : BaseItem
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            try
            {
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

                folder = await folder.CreateFolderAsync(userName, CreationCollisionOption.OpenIfExists);

                var file = await folder.CreateFileAsync(id.GetHashCode().ToString(), CreationCollisionOption.ReplaceExisting);

                var outStream = await file.OpenStreamForWriteAsync();
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(outStream, item);
                await outStream.FlushAsync();
                outStream.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

    }
}
