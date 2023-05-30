using NaughtyAttributes;
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

    private int _golds = 0;
    private int _level = 1;
    public int Golds { 
        get => _golds; 
        set => _golds = value; 
    }
    
    public int Level {
        get => _level;
        private set => _level = value;
    }

    public event Action<int> OnEnterEditorMode;
    public event Action<int> OnEnterPlayMode;

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
    }

    public void LoadData(GameData data)
    {
        Golds = data.golds;
        Level = data.level;
    }

    public void SaveData(ref GameData data)
    {
        data.golds = Golds;
        data.level = Level;
    }
    [Button]
    public void ChangeLevel()
    {
        _level++;
        Debug.Log("Level " + _level);

        //Enter Editor Mode
        OnEnterEditorMode?.Invoke(Level);
    }

    public void StartLevel()
    {
        //Enter Play Mode
        OnEnterPlayMode?.Invoke(Level);
    }
}
