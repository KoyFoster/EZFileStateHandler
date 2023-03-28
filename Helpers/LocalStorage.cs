using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using System.IO;
using Newtonsoft.Json;
using EZFileStateHandler.Models;

namespace EZFileStateHandler.Helpers
{
    public class LocalStorage
    {
        public static bool SetLocalStorage<T>(string storageName, T data)
        {
            if (data != null)
            {
                try
                {
                    // Write data to local storage
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForAssembly();
                    using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(storageName, FileMode.Create, storage))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.WriteLine(JsonConvert.SerializeObject(data));
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static T? GetLocalStorage<T>(string storageName) where T : class
        {
            // Read data from local storage
            try
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForAssembly();
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(storageName, FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string data = reader.ReadToEnd();
                        Console.WriteLine(data);
                        return JsonConvert.DeserializeObject<T>(data);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool SaveSettings(Settings data)
        {
            return SetLocalStorage<Settings>("Settings.config", data);
        }

        public static Settings? GetSettings()
        {
            return GetLocalStorage<Settings>("Settings.config");
        }
    }
}
