using UnityEngine;
using System;
using System.IO;
namespace ATBS.SavingSystem
{
    public class FileDataHandler {
        #region Variables
        private string dataDirPath;
        private readonly string EncryptionCodeWord = "YourCodeWordForEncryption"; // Provide your own encryption code word
        private string fileExtension;
        #endregion
        public FileDataHandler(string dataDirPath, string fileExtension)
        {
            this.dataDirPath = dataDirPath;
            this.fileExtension = fileExtension;
        }

        #region methods

        /// <summary>
        /// Loads data from the specified file in the save files directory.
        /// </summary>
        /// <param name="dataFileName">Name of the file to load.</param>
        /// <returns>Loaded data</returns>
        public SaveData Load(string dataFileName)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName) + "." + fileExtension;

            SaveData loadedData = null;
            if(File.Exists(fullPath))
            {   
                try
                {
                    // Load the serialized data from the file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }
                    dataToLoad = EncryptDecrypt(dataToLoad);

                    // Deserialize the data from JSON back into SaveData
                    loadedData = JsonUtility.FromJson<SaveData>(dataToLoad);
                }
                catch (Exception exc)
                {
                    Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + exc);
                    throw;
                }
            }
            return loadedData;
        }

        public void Save(SaveData saveData, string dataFileName)
        {
            // Path.Combine to account for different os path seperators
            string fullPath = Path.Combine(dataDirPath, dataFileName) + "." + fileExtension;
            try
            {
                // Create the directory the file will be written to if it doesn't already exist.
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                // Serialize the SaveData object into JSON
                string dataToStore = JsonUtility.ToJson(saveData, false);
                
                dataToStore = EncryptDecrypt(dataToStore);

                // Write the serialized data to a file
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
                Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + exc);
                throw;
            }
        }

        private string EncryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char) (data [i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);
            }
            return modifiedData;
        }
        #endregion
    }
}