using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFilePath = "";
    private bool _useEncryption = false;
    private readonly string _encryptionCode = "goldproject";
    public string FullPath
    {
        get { return Path.Combine(_dataDirPath,_dataFilePath); }
    }
    public FileDataHandler(string dataDirPath, string dataFilePath, bool useEncryption)
    {
        _dataDirPath = dataDirPath;
        _dataFilePath = dataFilePath;
        _useEncryption = useEncryption;
    }

    public GameData Load()
    {
        GameData loadedData = null;
        if (File.Exists(FullPath)) 
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(FullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //Decrypt data (optional)
                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                //Deserialize the data from JSON
                loadedData =JsonUtility.FromJson<GameData>(dataToLoad);

            } catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data to file : " + FullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        try
        {
            //Create a directory for the file if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
        
            //Serialize the game data into JSON
            string dataToJSON = JsonUtility.ToJson(data,true);

            if (_useEncryption)
            {
                //Encrypt data (optional)
                dataToJSON = EncryptDecrypt(dataToJSON);
            }
            //Write the serialized file to the file

            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToJSON);
                }
            }
        } catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file : " + FullPath + "\n" + e);
        }
    }

    //XOR Encryption
    string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCode[i % -_encryptionCode.Length]);
        }
        return modifiedData;
    }
}
