using DG.Tweening;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] LevelData[] _levels;
    [SerializeField] HeroesManager _heroesManager;
    private static GameManager _instance;

    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private int _nbMoves = 0;
    private int _level = 0;
    private Effect _currentRoomEffect = Effect.NONE;
    public int NbMoves { 
        get => _nbMoves; 
        set => _nbMoves = value; 
    [SerializeField] private GeneralData _generalData;

    public GeneralData GeneralData
    {
        get { return _generalData; }
    }
    
    public int Level {
        get => _level;
        private set => _level = value;
    }
    public Effect CurrentRoomEffect { 
        get => _currentRoomEffect; 
        private set => _currentRoomEffect = value; 
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
        return _levels[_level].ListHeroesInGroup;
    }

    [SerializeField]private Effect _effectRoomTest;
    [Button]
    private void RoomTest()
    {
        Trap trap = new Trap();
    }
    public void MoveHeroesToRoom(Room room)
    {
        //Move HeroesObjects

        Trap trap = room as Trap;
        if (trap != null)
        {
            _currentRoomEffect = trap.Effects[0]; //On garde l'effet principal
            //Activer RoomEffectManagerQueue 
            DecreaseRoomForEffectsList(trap, _heroesManager.HeroesInCurrentLevel);

            for (int  j = 0; j < trap.Effects.Length; j++)
            {   
                _heroesManager.ApplyDamageToEachHero(trap.Effects[j]);
                trap.IsActive = false;

                //Appliquer l'effet si la salle a au moins un upgrade et seulement pour l'effet de base
                if (trap.NbOfUpgrades > 0 && j == 0)
                {
                    RoomEffectManager.EffectsOnRoom[trap.Effects[j]].OnRoomEnter.Invoke(trap, _heroesManager.HeroesInCurrentLevel);
                    //UpdatedRoomEffect roomEffect = new UpdatedRoomEffect();
                }
            }
        } else
        {
            _currentRoomEffect = Effect.NONE;
        }
    }

    public static void DecreaseRoomForEffectsList(Trap trap, Group group)
    {
        for (int i = RoomEffectManager.EffectsEvent.Count - 1; i >= 0; i--)
        {
            RoomEffectManager.EffectsEvent[i].NbRoomBeforeApplied--;
            if (RoomEffectManager.EffectsEvent[i].NbRoomBeforeApplied == 0)
            {
                RoomEffectManager.EffectsAppliedAfterRoom[RoomEffectManager.EffectsEvent[i].Effect]?.Invoke(trap, group);
                RoomEffectManager.EffectsEvent.RemoveAt(i);
            }
        }
    }
}
