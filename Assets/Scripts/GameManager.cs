using System;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private int _golds;
    private int _level;
    public int Golds { 
        get => _golds; 
        set => _golds = value; 
    }
    
    public int Level {
        get => _level;
        private set => _level = value;
    }

    public event Action<int> OnLevelChanges;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of Game Manager in the scene.");
            Destroy(gameObject);
        }
    }

    public void IncrementGold()
    {
        Golds++;
        Debug.Log(Golds);
    }

    public void LoadData(GameData data)
    {
        Golds = data.golds;
    }

    public void SaveData(ref GameData data)
    {
        data.golds = Golds;
    }

    public void ChangeLevel()
    {
        _level++;
        Debug.Log("Level " + _level);
        OnLevelChanges?.Invoke(Level);
        //Enter Editor Mode
    }

    public void StartLevel()
    {
        //Enter Play Mode
    }
}
