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
    [SerializeField] float _durationMergeHeroes = 2f;
    [SerializeField] float _durationWaitInRoom = 1f;
    [SerializeField] float _durationWaitBeforeDisplayLoss = 2f;
    [SerializeField] float _durationWaitBeforeDisplayWin = 2f;
    [SerializeField] DisplayUIOnMode _displayUI;
    [SerializeField] GameObject _winDisplayGO;
    [SerializeField] GameObject _lossDisplayGO;
    [SerializeField] ElementList _roomsInList;
    [SerializeField] UIMenu _uiMenu; // peut etre nul
    [SerializeField] bool _isInPlayMode = false;

    private bool _hasWon = false;
    private int _nbMoves = 0;
    private int _level;
    private Effect _currentRoomEffect = Effect.NONE;
    private Room _currentRoom = null;
    private static GameManager _instance;
    private int _nbMenuIn = 0;
    private Sequence _movementHeroesSequence;
    [SerializeField] private Language _languageChosen = Language.FR;

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
        get {
            return _currentRoom.Effects.Count > 0 &&
                (_currentRoom.Effects[0] == Effect.FOUDRE ||
            _currentRoom.Effects[0] == Effect.FEU ||
            _currentRoom.Effects[0] == Effect.GLACE);
        }
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

    public int[] GetListTrapsCurrentLevel()
    {
        return _levels[_level].TrapList;
    }
    public int NbMenuIn { 
        get => _nbMenuIn; 
        set => _nbMenuIn = value; 
    }
    public float SlotSize
    {
        get => _mapManager.SlotSize;
    }

    public bool IsPlayModeActive
    {
        get => _startButton.activeInHierarchy;
    }
    public Language LanguageChosen { 
        get => _languageChosen; 
        private set => _languageChosen = value; 
    }
    public bool IsInPlayMode { 
        get => _isInPlayMode;
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
    [SerializeField] private UnityEvent _onStartEditorMode;
    [SerializeField] private UnityEvent _onStartPlayMode;
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
        _languageChosen = (Language)PlayerPrefs.GetInt("language");
    }

    private void Start()
    {
        GameManager.Instance.SetPlayMode(false);
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

    public TutorialData GetTutorialData() //peut etre nul attention
    {
        return _levels[_level].Tutorial;
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

    public GameObject GetHeroesParentGameObject()
    {
        return _heroesManager.GroupParent;
    }

    public void SpawnHeroesOnScreen(Room room)
    {
        _heroesManager.GroupParent.transform.position = new Vector2(room.transform.position.x, room.transform.position.y);
    }
    public void MoveHeroesOnScreen(Room room)
    {
        MoveHeroesToRoom(room);
    }

    public void MoveHeroesToRoom(Room room)
    {
        int heroesNotAlive = _heroesManager.NbHeroesLeft;

        _heroesManager.HeroesInCurrentLevel.AffectedByPlants = false; //Enleve l'effet de la room des plantes
        
        if (room != null)
        {
            _currentRoom = room;
            if (room.IsActive && room.NbOfUpgrades > 0 && room.Effects.Count > 0)
            {
                ApplyCurrentRoomEffect(room.Effects[0]);
            }
            DecreaseRoomForEffectsList(room, _heroesManager.HeroesInCurrentLevel);
            _heroesManager.ApplyAbilities(room);
            if (room.IsActive)
            {
                room.IsActive = false;
                room.NbOfUsage = 1;
                room.SetIconEffect();
                if (room.TrapData.SoundWhenApplied != "")
                {
                    AudioManager.Instance.Play(room.TrapData.SoundWhenApplied);
                }
                if (room.Effects.Count > 0)
                {
                    _currentRoomEffect = room.Effects[0]; //On garde l'effet principal
                    OnEffectApplied?.Invoke(_currentRoomEffect);
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
                } else
                {
                    _currentRoomEffect = Effect.NONE;
                }
                if (room.TrapData.RoomType == RoomType.LEVER)
                {
                    _heroesManager.HeroesInCurrentLevel.NbKeysTaken++;
                }
            }
            _heroesManager.RemoveAbilities(room);
            _heroesManager.ApplyAfterRoomAbilities(room);

            if (room.TrapData.RoomType == RoomType.NORMAL)
            {
                _onHeroesAttackUnityEvent.Invoke();
            }
        } else
        {
            _currentRoomEffect = Effect.NONE;
            _currentRoom = null;
        }
        heroesNotAlive -= _heroesManager.NbHeroesLeft;
        if (heroesNotAlive == 3 && GooglePlayManager.Instance != null && GooglePlayManager.Instance.IsAuthenticated)
        {
            GooglePlayManager.Instance.HandleAchievement("Glue you back together, IN HELL");
        }
        _heroesManager.ChangeTurn();
    }

    public void ApplyCurrentRoomEffect(Effect effect)
    {
        if (effect == Effect.PLANTE)
        {
            _heroesManager.HeroesInCurrentLevel.AffectedByPlants = true;
            Debug.Log("PLANT EFFECT");
        }
    }

    public void DecreaseRoomForEffectsList(Room room, Group group)
    {
        Debug.Log("Is effect " + _heroesManager.HeroesInCurrentLevel.AffectedByPlants);
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
        _onStartEditorMode.Invoke();
        OnEffectApplied?.Invoke(Effect.NONE);
        _isInPlayMode = false;
        SetPlayMode(false);
        if (_movementHeroesSequence != null)
        {
            _movementHeroesSequence.Kill();
            _movementHeroesSequence = null;
        }
        _nbMenuIn = 0;
        _roomsInList.InitList(_levels[Level].TrapList);
        _winDisplayGO.SetActive(false);
        _lossDisplayGO.SetActive(false);
        _displayUI.EnterEditMode();
        _hasWon = false;
        OnEnterEditorMode?.Invoke(Level);
        _heroesManager.OnChangeLevel(Level);
        _mapManager.InitLevel(_levels[Level]);
        UIUpdateEditMode.Instance.Init(_levels[_level].NbMovesMax);
        if (_uiMenu != null && _levels[_level].Tutorial != null) { // peut etre nul
            _uiMenu.DisplayTutorial();
            ChangeNbMenuIn(1);
        }
    }

    [Button("Enter play mode")]
    public void StartPlayMode()
    {
        //Enter Play Mode
        _isInPlayMode = true;
        _mapManager.UpdateMapIconPlayMode();
        _onStartPlayMode.Invoke();
        OnEnterPlayMode?.Invoke(Level);
        _displayUI.EnterPlayMode();
        _startButton.SetActive(false);
    }
    #endregion

    public IEnumerator ChangeRoomFromPath(List<Room> path)
    {
        bool isBossRoomReached = false;
        int i = 0;
        bool movementComplete = false;
        while (path.Count > i && !_hasWon && !isBossRoomReached)
        {
            //Debug.Log($"{(i == 0 ? "Are at the room : " : "Move to ")} {path[i].name}");
            if (i == 0) //Waiting in entrance
            {
                SpawnHeroesOnScreen(path[i]);
                yield return new WaitForSeconds(0.5f);
            } else
            {
                //Move avec dotween puis onended
                _onHeroesMovementUnityEvent.Invoke();
                movementComplete = false;
                _heroesManager.HeroesInCurrentLevel.IsRunningInAnimator(true);
                _movementHeroesSequence = DOTween.Sequence();
                for (int j = 0; j < _heroesManager.HeroesInCurrentLevel.Heroes.Count; j++)
                {
                    if (i == 0)
                    {
                        if (_heroesManager.HeroesInCurrentLevel.Heroes[j].CanMove)
                        {
                            _movementHeroesSequence.Append(_heroesManager.HeroesInCurrentLevel.Heroes[j].transform.
                            DOLocalMoveX(0, _durationMergeHeroes));
                        }
                    }
                    else
                    {
                        if (_heroesManager.HeroesInCurrentLevel.Heroes[j].CanMove)
                        {
                            _movementHeroesSequence.Join(_heroesManager.HeroesInCurrentLevel.Heroes[j].transform.
                            DOLocalMoveX(0, _durationMergeHeroes));
                        }
                    }
                }
                float direction = _heroesManager.GroupParent.transform.position.x - path[i].transform.position.x;
                if (direction >= 0.2f || direction <= -0.2f)
                {
                    for (int j = 0; j < _heroesManager.HeroesInCurrentLevel.Heroes.Count; j++)
                    {
                        if (direction <= -0.2f)
                        {
                            _heroesManager.HeroesInCurrentLevel.Heroes[j].RotateHero(1);
                        } else
                        {
                            _heroesManager.HeroesInCurrentLevel.Heroes[j].RotateHero(-1);
                        }
                    }
                }

                _movementHeroesSequence.Append(_heroesManager.GroupParent.transform.DOMove(path[i].transform.position, _durationBetweenRoom));
                for (int j = 0; j < _heroesManager.HeroesInCurrentLevel.Heroes.Count; j++)
                {
                    float posOffset = ((j + 1) * (GameManager.Instance.SlotSize / (_heroesManager.HeroesInCurrentLevel.Heroes.Count + 1))) - 0.5f;
                    if (j == 0)
                    {
                        if (_heroesManager.HeroesInCurrentLevel.Heroes[j].CanMove)
                        {
                            _movementHeroesSequence.Append(_heroesManager.HeroesInCurrentLevel.Heroes[j].transform.
                            DOLocalMoveX(posOffset, _durationMergeHeroes));
                        }
                    }
                    else
                    {
                        if (_heroesManager.HeroesInCurrentLevel.Heroes[j].CanMove)
                        {
                            _movementHeroesSequence.Join(_heroesManager.HeroesInCurrentLevel.Heroes[j].transform.
                            DOLocalMoveX(posOffset, _durationMergeHeroes));
                        }
                    }
                }
                _movementHeroesSequence.OnComplete(() =>
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
                    _movementHeroesSequence = null;
                });
                yield return new WaitUntil(() => movementComplete);
                if (path[i].TrapData.RoomType != RoomType.BOSS) //Normal room
                {
                    yield return new WaitForSeconds(_durationWaitInRoom);
                    path[i].ClearIcon();
                } else
                {
                    yield return new WaitForSeconds(_durationWaitBeforeDisplayLoss);
                }
            }
            i++;
        }
    }

    #region VictoryConditions
    public IEnumerator PlayerWin()
    {
        if (GooglePlayManager.Instance != null && GooglePlayManager.Instance.IsAuthenticated)
        {
            if (Level == 0)
                GooglePlayManager.Instance.HandleAchievement("Here comes a new challenger");
            if (Level == _levels.Length - 1)
                GooglePlayManager.Instance.HandleAchievement("Enma no Danjon");
            if (MapManager.Instance.BuyableRoomCount == 0)
                GooglePlayManager.Instance.HandleAchievement("I've got balls of steel");
        }
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
        if (AllHeroesAreFullLife() && GooglePlayManager.Instance != null && GooglePlayManager.Instance.IsAuthenticated)
        {
            GooglePlayManager.Instance.HandleAchievement("How the hell?");
        }
        ChangeNbMenuIn(1);
        OnLoss?.Invoke();
    }
    #endregion

    private bool AllHeroesAreFullLife()
    {
        bool allHeroesAreFullLife = true;

        for (int i = 0; i < _heroesManager.HeroesInCurrentLevel.Heroes.Count; i++)
        {
            //Debug.Log($"Hero {_heroesManager.HeroesInCurrentLevel.Heroes[i].Health} {_heroesManager.HeroesInCurrentLevel.Heroes[i].MaxHealth}");
            if (_heroesManager.HeroesInCurrentLevel.Heroes[i].Health < _heroesManager.HeroesInCurrentLevel.Heroes[i].MaxHealth)
            {
                allHeroesAreFullLife = false;
                break;
            }
        }
        return allHeroesAreFullLife;
    }

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
