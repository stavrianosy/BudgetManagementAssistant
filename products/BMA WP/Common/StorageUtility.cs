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
        public static async Task<string[]> ListItems(string folderName, int userId)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            var files = new List<string>();
            var folder = await ApplicationData.Current.LocalFolder
                                             .CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            //var userId = 0;

            if (userId > 0)
            {
                var folderUser = await folder.GetFolderAsync(userId.ToString());
                files.AddRange((from file in await folderUser.GetFilesAsync() select file.Name).ToList());
            }
            else
            {
                var folderList = await folder.GetFoldersAsync();

                foreach (var item in folderList)
                    files.AddRange((from file in await item.GetFilesAsync() select file.Name).ToList());
            }
            return files.ToArray();
        }

        public static async Task<T> RestoreItem<T>(string folderName, string hash, int userId)
            where T : BaseItem, new()
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            var folderUsersAll = await folder.GetFoldersAsync();

            //StorageFile file = await folder.GetFileAsync(hash);
            //StorageFile file = null;

            var folderUser = folderUsersAll.FirstOrDefault(x => x.Name == (userId > 0 ? userId.ToString() : hash));
            var  file = await folderUser.GetFileAsync(hash);
            
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

        public static async Task SaveItem<T>(string folderName, T item, int id, int userId)
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

                //var userId = 4;
                //if(userId > 0)
                folder = await folder.CreateFolderAsync(userId.ToString(), CreationCollisionOption.OpenIfExists);

                var file = await folder.CreateFileAsync(id.GetHashCode().ToString(), CreationCollisionOption.ReplaceExisting);

                //var stream = await file.OpenAsync(FileAccessMode.ReadWrite);

                //using (var outStream = stream.GetOutputStreamAt(0))
                //{
                //    var serializer = new DataContractJsonSerializer(typeof(T));
                //    serializer.WriteObject(outStream.AsStreamForWrite(), item);
                //    await outStream.FlushAsync();
                //    stream.Dispose();
                //}

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
