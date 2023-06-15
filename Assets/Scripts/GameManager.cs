using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour//, IDataPersistence
{
    [SerializeField] LevelData[] _levels;
    [SerializeField] HeroesManager _heroesManager;
    [SerializeField] MapManager _mapManager;
    [SerializeField] GameObject _startButton;
    [SerializeField] float _durationBetweenRoom = 10f;
    [SerializeField] float _durationWaitInRoom = 1f;
    [SerializeField] float _durationWaitBeforeDisplayLoss = 2f;
    [SerializeField] float _durationWaitBeforeDisplayWin = 2f;
    [SerializeField] DisplayUIOnMode _displayUI;
    [SerializeField] GameObject _winDisplayGO;
    [SerializeField] GameObject _lossDisplayGO;
    [SerializeField] ElementList _roomsInList;

    private bool _hasWon = false;
    private int _nbMoves = 0;
    private int _level;
    private Effect _currentRoomEffect = Effect.NONE;
    private Room _currentRoom = null;
    private static GameManager _instance;
    private int _nbMenuIn = 0;
    private Coroutine _routineWaitInRoom;

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
        set => _level = value;
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
    public int NbMenuIn { 
        get => _nbMenuIn; 
        set => _nbMenuIn = value; 
    }
    #endregion Properties

    #region Events
    public event Action<int> OnEnterEditorMode;
    public event Action<int> OnEnterPlayMode;
    public event Action OnWin;
    public event Action OnLoss;
    public event Action<Effect> OnEffectApplied;

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
        //StartEditMode();
    }

    
    private void OnValidate()
    {
        for (int i = 0; i < _levels.Length; i++)
        {
            _levels[i].name = "Level " + (i + 1);
        }
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
        MoveHeroesToRoom(room);
    }

    public void MoveHeroesToRoom(Room room)
    {
        _heroesManager.HeroesInCurrentLevel.AffectedByPlants = false; //Enleve l'effet de la room des plantes
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
                    OnEffectApplied?.Invoke(_currentRoomEffect);
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

    #region Level
    public void LoadLevel(int level)
    {
        if (level < 1 && _levels.Length <= level)
        {
            Debug.LogWarning($"Num�ro de niveau invalide : le niveau doit �tre sup�rieur � 0 et inf�rieur � {_levels.Length + 1}");
        }
        else
        {
            _level = level - 1;
            StartEditMode();
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
        Debug.Log("NIVEAU " + _level);
        _nbMenuIn = 0;
        _roomsInList.InitList();
        _winDisplayGO.SetActive(false);
        _lossDisplayGO.SetActive(false);
        _displayUI.EnterEditMode();
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
    #endregion

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
                _heroesManager.HeroesInCurrentLevel.IsRunningInAnimator(true);
                _heroesManager.GroupParent.transform.DOMove(path[i].transform.position, _durationBetweenRoom).OnComplete(() =>
                {
                    _heroesManager.HeroesInCurrentLevel.IsRunningInAnimator(false);
                    if (path[i].TrapData.RoomType != RoomType.BOSS) //Normal room
                    {
                        MoveHeroesOnScreen(path[i]);
                    }
                    else
                    { // Boss room
                        PlayerLoss();
                        _lossDisplayGO.SetActive(true);
                        isBossRoomReached = true;
                    }
                    movementComplete = true;
                });
                yield return new WaitUntil(() => movementComplete);
                if (path[i].TrapData.RoomType != RoomType.BOSS) //Normal room
                {
                    yield return new WaitForSeconds(_durationWaitInRoom);
                } else
                {
                    yield return new WaitForSeconds(_durationWaitBeforeDisplayLoss);
                }
                OnEffectApplied?.Invoke(Effect.NONE);
            }
            i++;
        }
    }

    #region VictoryConditions
    public IEnumerator PlayerWin()
    {
        OnWin?.Invoke();
        _onWinUnityEvent.Invoke();
        if (MapManager.Instance.RoutineChangeRoom != null)
        {
            StopCoroutine(MapManager.Instance.RoutineChangeRoom);
            MapManager.Instance.RoutineChangeRoom = null;
        }
        _hasWon = true;
        yield return new WaitForSeconds(_durationWaitBeforeDisplayWin);
        ChangeNbMenuIn(1);
        _winDisplayGO.SetActive(true);
    }
    void PlayerLoss()
    {
        ChangeNbMenuIn(1);
        OnLoss?.Invoke();
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

    public void ChangeNbMenuIn(int offset)
    {
        _nbMenuIn += offset;
        Debug.Log("Nombre Menu" + _nbMenuIn);
    }
}
