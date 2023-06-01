using DG.Tweening;
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

    [SerializeField] private GeneralData _generalData;

    public GeneralData GeneralData
    {
        get { return _generalData; }
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

    [SerializeField] Level[] _levels;

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

    private void Start()
    {
        DOTween.Init();
    }

    private void OnValidate()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].name = "Level " + (i + 1);
        }
    }

    public void LoadData(GameData data)
    {
        Golds = data.golds;
        //Level = data.level;
    }

    public void SaveData(ref GameData data)
    {
        data.golds = Golds;
        //data.level = Level;
    }
    [Button("Next level")]
    public void ChangeLevel()
    {
        _level++;
        Debug.Log("Level " + _level);
    }

    [Button("Enter edit mode")]
    public void StartEditMode()
    {
        //Enter Play Mode
        OnEnterEditorMode?.Invoke(Level);
    }

    [Button("Enter play mode")]
    public void StartPlayMode()
    {
        //Enter Play Mode
        OnEnterPlayMode?.Invoke(Level);
    }

    public GameObject[] GetHeroesCurrentLevel()
    {
        return _levels[_level-1].ListHeroesInGroup;
    }
}
