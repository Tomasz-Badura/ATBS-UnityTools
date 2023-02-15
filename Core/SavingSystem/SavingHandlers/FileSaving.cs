using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using ATBS.Extensions;

namespace ATBS.Core.SavingSystem
{
    [CreateAssetMenu(menuName = "SavingSystem/FileSaving")]
    public class FileSaving : SavingHandler
    {
        private readonly string EncryptionCodeWord = "YourCodeWordForEncryption";
        public override SaveData Load(string dataFileName)
        {
            string fullPath = Path.Combine(SavePath, dataFileName) + "." + fileExtension;
            SaveData loadedData = null;
            if(File.Exists(fullPath))
            {   
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    dataToLoad = EncryptDecrypt(dataToLoad);
                    loadedData = JsonConvert.DeserializeObject<SaveData>(dataToLoad);
                }
                catch (Exception exc)
                {
                    this.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + exc);
                }
            }
            return loadedData;
        }

        public override void Save(SaveData saveData, string dataFileName)
        {
            string fullPath = Path.Combine(SavePath, dataFileName) + "." + fileExtension;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonConvert.SerializeObject(saveData);
                dataToStore = EncryptDecrypt(dataToStore);
                using(FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using(StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception exc)
            {
                this.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + exc);
            }
        }

        /// <summary>
        /// Encrypts / decrypts string using EncryptionCodeWord
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char) (data [i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);
            }
            return modifiedData;
        }
    }
}