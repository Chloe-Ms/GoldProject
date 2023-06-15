using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager _instance;

    public static DataPersistenceManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private GameData _gameData;
    private FileDataHandler _dataHandler;
    private List<IDataPersistence> dataPersistenceObjects;
    [SerializeField] private string _fileName;
    [SerializeField] private bool _useEncryption;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of Data Persistence Manager in the scene.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        this._gameData = _dataHandler.Load();

        if (this._gameData == null)
        {
            Debug.Log("No data was found.");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in this.dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in this.dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref _gameData);
        }

        _dataHandler.Save(_gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
