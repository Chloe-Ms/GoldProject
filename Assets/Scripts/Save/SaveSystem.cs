using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    [SerializeField] static string _saveName = "data.savedata";

    public static void SaveData(){
        string savePath = Application.persistentDataPath + "/" + _saveName;
        Debug.Log(savePath);
        FileStream stream;
        BinaryFormatter formatter = new BinaryFormatter();

        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        stream = new FileStream(savePath, FileMode.Create);
        GameData data = new GameData();
        formatter.Serialize(stream,data);
        stream.Close();
    }

    public static GameData LoadData()
    {
        string savePath = Application.persistentDataPath + "/" + _saveName;
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);
            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + savePath);
            return null;
        }
    }
}
