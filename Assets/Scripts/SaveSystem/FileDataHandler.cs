using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;
    private bool encryptData;
    private string codeWord = "unity";

    public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
        this.encryptData = encryptData;
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            // Create Directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            
            // Convert GameData to JSON string
            string dataToSave = JsonUtility.ToJson(gameData, true);

            if (encryptData)
                dataToSave = EncryptDecrypt(dataToSave);
            
            // Open/Create a new File
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                // Write the JSON text to the File
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        // Check if the Save File exists
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                // Open the File
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    // Read File's text content
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                
                if (encryptData)
                    dataToLoad = EncryptDecrypt(dataToLoad);
                
                // Convert the JSON string back into a GameData object
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                // Log any Error that happens
                Debug.LogError("Error on trying to load data from file: " +  fullPath + "\n" + e);
            }
        }
        
        return loadData;
    }

    public void Delete()
    {
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;
    }
}
