using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] LevelData[] _levels;
    [SerializeField] HeroesManager _heroesManager;
    [SerializeField] MapManager _mapManager;
    private static GameManager _instance;
    private bool _hasWon = false;
    private Coroutine _routineChangeRoom;

    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    private int _nbMoves = 0;
    private int _level = 0;
    private Effect _currentRoomEffect = Effect.NONE;
    public int NbMoves
    {
        get => _nbMoves;
        set => _nbMoves = value;
    }
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

    public bool IsCurrentRoomElementary
    {
        get => _currentRoomEffect == Effect.FOUDRE || 
            _currentRoomEffect == Effect.FEU || 
            _currentRoomEffect == Effect.GLACE;
    }
    public int CurrentLevelWidth
    {
        get => _levels[_level].MapWidth;
    }
    public int CurrentLevelHeight
    {
        get => _levels[_level].MapHeight;
    }

    public event Action<int> OnEnterEditorMode;
    public event Action<int> OnEnterPlayMode;

    #region test
    [SerializeField] private bool _updated;
    [SerializeField] private Room _room;
    [SerializeField] private Effect _effect;
    [Button]
    private void RoomTest()
    {
        GameObject room = Instantiate(_room.gameObject);
        Room t = room.GetComponent<Room>();
        if (_updated)
        {
            t.NbOfUpgrades = 1;
        }
        else
        {
            t.NbOfUpgrades = 0;
        }
        t.Effects.Add(_effect);
        MoveHeroesToRoom(t);
    }
    #endregion

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
        NbMoves = data.golds;
    }

    public void SaveData(ref GameData data)
    {
        data.golds = NbMoves;
    }

    public HeroData[] GetHeroesCurrentLevel()
    {
        return _levels[_level].ListHeroesInGroup;
    }
    public int GetHeroesSensibility(Effect effect, Role role)
    {
        return _heroesManager.GetSensibility(effect, role);
    }

    public void MoveHeroesOnScreen(Room room)
    {
        _heroesManager.GroupParent.transform.position = new Vector2(room.transform.position.x, room.transform.position.y);
    }

    public void MoveHeroesToRoom(Room room)
    {
        _heroesManager.HeroesInCurrentLevel.AffectedByPlants = false; //Enl�ve l'effet de la room des plantes

        if (room != null && room.Effects.Count > 0)
        {
            _currentRoomEffect = room.Effects[0]; //On garde l'effet principal
            DecreaseRoomForEffectsList(room, _heroesManager.HeroesInCurrentLevel);
            _heroesManager.ApplyAbilities(room);
            if (room.IsActive)
            {
                room.IsActive = false;
                Debug.Log($"Room number of effects : {room.Effects.Count }");
                if (room.Effects[0] == Effect.PLANTE) { _heroesManager.HeroesInCurrentLevel.AffectedByPlants = true; }
                for (int  j = 0; j < room.Effects.Count; j++)
                {
                    Debug.Log("Effect " +room.Effects[j]);
                    _heroesManager.ApplyDamageToEachHero(room.Effects[j]);
                    //Appliquer l'effet si la salle a au moins un upgrade et seulement pour l'effet de base
                }
                if (room.NbOfUpgrades > 0)
                {
                    Debug.Log("Apply effect of room" + _currentRoomEffect);
                    if (RoomEffectManager.EffectsOnRoom.ContainsKey(_currentRoomEffect))
                    {
                        RoomEffectManager.EffectsOnRoom[room.Effects[0]].OnRoomEnter.Invoke(room, _heroesManager.HeroesInCurrentLevel);
                    }
                }
            }
            _heroesManager.RemoveAbilities(room);
        } else
        {
            _currentRoomEffect = Effect.NONE;
        }
    }

    public void DecreaseRoomForEffectsList(Room room, Group group)
    {
        Debug.Log("Effects count "+RoomEffectManager.EffectsEvent.Count);
        for (int i = RoomEffectManager.EffectsEvent.Count - 1; i >= 0; i--)
        {
            Debug.Log("Decrease number for room effects list");
            RoomEffectManager.EffectsEvent[i].NbRoomBeforeApplied--;
            if (RoomEffectManager.EffectsEvent[i].NbRoomBeforeApplied == 0)
            {
                Debug.Log("Effect applied "+ RoomEffectManager.EffectsEvent[i].Effect);
                RoomEffectManager.EffectsAppliedAfterRoom[RoomEffectManager.EffectsEvent[i].Effect]?.Invoke(room, group, _heroesManager);
                RoomEffectManager.EffectsEvent.RemoveAt(i);
            }
        }
    }

    public void PlayerWin()
    {
        _hasWon = true;
        if (_routineChangeRoom != null)
        {
            StopCoroutine( _routineChangeRoom );
            _routineChangeRoom = null;
        }
        Debug.Log("Level cleared");
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
        _routineChangeRoom = null;
        _hasWon = false;
        OnEnterEditorMode?.Invoke(Level);
        _heroesManager.OnChangeLevel(Level);
        _mapManager.InitLevel(_levels[Level]);
    }

    [Button("Enter play mode")]
    public void StartPlayMode()
    {
        //Enter Play Mode
        OnEnterPlayMode?.Invoke(Level);
        List<Room> path = _mapManager.Pathfinding();
        if (path != null)
        {
            _routineChangeRoom = StartCoroutine(ChangeRoom(path));
        }
    }

    IEnumerator ChangeRoom(List<Room> path)
    {
        int i = 0;
        while (path.Count > i && !_hasWon)
        {
            MoveHeroesOnScreen(path[i]);
            if (i < path.Count - 1)
            {
                MoveHeroesToRoom(path[i]);
            }
            else
            {
                CheckWinLossContitions();
            }
            yield return new WaitForSeconds(1);
            i++;
        }
    }
    void CheckWinLossContitions()
    {
        Debug.Log("IN BOSS ROOM");
    }
}
