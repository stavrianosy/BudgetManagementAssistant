using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BMA.DataModel
{
    public class StorageUtility
    {
        public static async Task<string[]> ListItems(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            var folder = await ApplicationData.Current.LocalFolder
                                             .CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            return (from file in await folder.GetFilesAsync() select file.DisplayName).ToArray();
        }

        public static async Task<T> RestoreItem<T>(string folderName, string hashCode)
            where T : BaseItem, new()
        {
            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException("folderName");
            }

            if (string.IsNullOrEmpty(hashCode))
            {
                throw new ArgumentNullException("hashCode");
            }

            var folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(folderName);
            var file = await folder.GetFileAsync(hashCode);
            var inStream = await file.OpenSequentialReadAsync();
            var serializer = new DataContractJsonSerializer(typeof(T));
            var retVal = (T)serializer.ReadObject(inStream.AsStreamForRead());
            return retVal;
        }
    }
}
