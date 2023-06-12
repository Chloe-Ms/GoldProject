using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] LevelData[] _levels;
    [SerializeField] HeroesManager _heroesManager;
    [SerializeField] MapManager _mapManager;
    [SerializeField] GameObject _startButton;
    [SerializeField] float _timeBetweenRoom = 10f;
    [SerializeField] DisplayUIOnMode _displayUI;
    [SerializeField] GameObject _winDisplayGO;
    [SerializeField] GameObject _lossDisplayGO;
    [SerializeField] ElementList _roomsInList;

    private bool _hasWon = false;
    private Coroutine _routineChangeRoom;
    private int _nbMoves = 0;
    private int _level = 0;
    private Effect _currentRoomEffect = Effect.NONE;
    private Room _currentRoom = null;
    private static GameManager _instance;

    [SerializeField] private GeneralData _generalData;
    
    #region Properties
    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }
    public int NbMoves
    {
        get => _nbMoves;
        set => _nbMoves = value;
    }
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
    public Room CurrentRoom { 
        get => _currentRoom;
    }

    public int[] MaxHealthCurrentLevel()
    {
        return _levels[_level].MaxHealth;
    }
    #endregion Properties

    #region Events
    public event Action<int> OnEnterEditorMode;
    public event Action<int> OnEnterPlayMode;
    public event Action OnWin;
    public event Action OnLoss;

    [SerializeField] private UnityEvent _onWinUnityEvent;
    [SerializeField] private UnityEvent _onLossUnityEvent;
    [SerializeField] private UnityEvent _onHeroesMovementUnityEvent;
    [SerializeField] private UnityEvent _onHeroesAttackUnityEvent;
    #endregion

    #region Test
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
        GameManager.Instance.SetPlayMode(false);
    }

    private void Start()
    {
        DOTween.Init();
        StartEditMode();
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

    public int GetDamageOnHero(Effect effect, Hero hero)
    {
        return _heroesManager.GetDamageOfEffectOnHero(effect, hero);
    }

    public void SpawnHeroesOnScreen(Room room)
    {
        _heroesManager.GroupParent.transform.position = new Vector2(room.transform.position.x, room.transform.position.y);
    }
    public void MoveHeroesOnScreen(Room room)
    {
        _onHeroesMovementUnityEvent.Invoke();
        _heroesManager.GroupParent.transform.position = new Vector2(room.transform.position.x, room.transform.position.y);
    }

    public void MoveHeroesToRoom(Room room)
    {
        _heroesManager.HeroesInCurrentLevel.AffectedByPlants = false; //Enl�ve l'effet de la room des plantes
        if (room.TrapData.SoundWhenApplied != "")
        {
            AudioManager.Instance.Play(room.TrapData.SoundWhenApplied);
        }
        if (room != null)
        {
            _currentRoom = room;
            DecreaseRoomForEffectsList(room, _heroesManager.HeroesInCurrentLevel);
            _heroesManager.ApplyAbilities(room);
            if (room.IsActive)
            {
                room.IsActive = false;
                if (room.Effects.Count > 0)
                {
                    _currentRoomEffect = room.Effects[0]; //On garde l'effet principal

                    ApplyCurrentRoomEffect(_currentRoomEffect);

                    for (int j = 0; j < room.Effects.Count; j++)
                    {
                        _heroesManager.ApplyDamageToEachHero(room.Effects[j]);
                    }
                    //Appliquer l'effet si la salle a au moins un upgrade et seulement pour l'effet de base
                    if (room.NbOfUpgrades > 0)
                    {
                        if (RoomEffectManager.EffectsOnRoom.ContainsKey(_currentRoomEffect))
                        {
                            RoomEffectManager.EffectsOnRoom[_currentRoomEffect].OnRoomEnter.Invoke(room, _heroesManager.HeroesInCurrentLevel);
                        }
                    }
                    if (_currentRoomEffect == Effect.MONSTRE)
                    {
                        ApplyDamageReduction();
                    }
                }
                if (room.TrapData.RoomType == RoomType.LEVER)
                {
                    _heroesManager.HeroesInCurrentLevel.NbKeysTaken++;
                }
            }
            _heroesManager.RemoveAbilities(room);
            if (room.TrapData.RoomType == RoomType.NORMAL)
            {
                _onHeroesAttackUnityEvent.Invoke();
            }
        } else
        {
            _currentRoomEffect = Effect.NONE;
            _currentRoom = null;
        }
        _heroesManager.ChangeTurn();
    }

    public void ApplyCurrentRoomEffect(Effect effect)
    {
        if (effect == Effect.PLANTE)
        {
            _heroesManager.HeroesInCurrentLevel.AffectedByPlants = true;
        }
    }
    public void ApplyDamageReduction()
    {
        Hero hero = _heroesManager.HeroesInCurrentLevel.GetHeroWithRole(Role.CHEVALIER);
        if (hero != null)
        {
            hero.HasDamageReduction = true;
        }
    }

    public void DecreaseRoomForEffectsList(Room room, Group group)
    {
        for (int i = RoomEffectManager.EffectsEvent.Count - 1; i >= 0; i--)
        {
            RoomEffectManager.EffectsEvent[i].NbRoomBeforeApplied--;
            if (RoomEffectManager.EffectsEvent[i].NbRoomBeforeApplied == 0)
            {
                RoomEffectManager.EffectsAppliedAfterRoom[RoomEffectManager.EffectsEvent[i].Effect]?.Invoke(room, group, _heroesManager);
                RoomEffectManager.EffectsEvent.RemoveAt(i);
            }
        }
    }

    [Button("Next level")]
    public void ChangeLevel()
    {
        _level++;
        StartEditMode();
    }

    [Button("Enter edit mode")]
    public void StartEditMode()
    {
        _roomsInList.InitList();
        _winDisplayGO.SetActive(false);
        _lossDisplayGO.SetActive(false);
        _displayUI.EnterEditMode();
        _routineChangeRoom = null;
        _hasWon = false;
        OnEnterEditorMode?.Invoke(Level);
        _heroesManager.OnChangeLevel(Level);
        _mapManager.InitLevel(_levels[Level]);
        UIUpdateEditMode.Instance.Init(_levels[_level].NbMovesMax);
    }

    [Button("Enter play mode")]
    public void StartPlayMode()
    {
        //Enter Play Mode
        if (_mapManager.IsEditComplete())
        {
            _displayUI.EnterPlayMode();
            _startButton.SetActive(false);
            OnEnterPlayMode?.Invoke(Level);
            // List<Room> path = _mapManager.Pathfinding();
            // if (path != null)
            // {
            //     _routineChangeRoom = StartCoroutine(ChangeRoomFromPath(path));
            // }
        }
    }

    public IEnumerator ChangeRoomFromPath(List<Room> path)
    {
        bool isBossRoomReached = false;
        int i = 0;
        bool movementComplete = false;
        while (path.Count > i && !_hasWon && !isBossRoomReached)
        {
            if (i == 0) //Waiting in entrance
            {
                SpawnHeroesOnScreen(path[i]);
            } else
            {
                //Move avec dotween puis onended
                _onHeroesMovementUnityEvent.Invoke();
                movementComplete = false;
                _heroesManager.GroupParent.transform.DOMove(path[i].transform.position, _timeBetweenRoom).OnComplete(() =>
                {
                    if (path[i].TrapData.RoomType != RoomType.BOSS) //Normal room
                    {
                        MoveHeroesToRoom(path[i]);
                    }
                    else
                    { // Boss room
                        PlayerLoss();
                        isBossRoomReached = true;
                    }
                    movementComplete = true;
                });
                yield return new WaitUntil(() => movementComplete);
            }
            i++;
        }
    }

    #region VictoryConditions
    public void PlayerWin()
    {
        _hasWon = true;
        if (MapManager.Instance.RoutineChangeRoom != null)
        {
            StopCoroutine(MapManager.Instance.RoutineChangeRoom);
            MapManager.Instance.RoutineChangeRoom = null;
        }
        OnWin?.Invoke();
        _onWinUnityEvent.Invoke();
        _winDisplayGO.SetActive(true);
    }
    void PlayerLoss()
    {
        OnLoss?.Invoke();
        _lossDisplayGO.SetActive(true);
        //Debug.Log("IN BOSS ROOM");
    }
    #endregion

    public void SetPlayMode(bool state)
    {
        Room boss = _mapManager.BossRoom;
        if(boss != null)
        {
            Vector2 positionBossRoom =  boss.gameObject.transform.position;
            _startButton.transform.position = new Vector2(positionBossRoom.x, positionBossRoom.y);
        }
        _startButton.SetActive(state);
    }
}
