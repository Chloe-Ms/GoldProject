using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;
using System.IO;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    private Stack<MapAction> _mapActions = new Stack<MapAction>();
    private EditorState _editorState = EditorState.Select;
    private int _buyableRoomCount = 5;
    private int _currentRoomCount = 0;
    private bool _isUpgradable = false;
    private Coroutine _routineChangeRoom;

    #region Properties
    public static MapManager Instance
    {
        get { return _instance; }
    }
    public EditorState EditorState
    {
        get { return _editorState; }
        set { _editorState = value; }
    }

    public int BuyableRoomCount
    {
        get { return _buyableRoomCount - _currentRoomCount; }
    }

    public bool IsUpgradable
    {
        get { return _isUpgradable; }
    }

    public Coroutine RoutineChangeRoom
    {
        get { return _routineChangeRoom; }
        set { _routineChangeRoom = value; }
    }

    public List<Room> ListOfLever
    {
        get {
            List<Room> leverList = new List<Room>();

            _slots.ForEach(slot => {
                if (slot.GetComponent<Room>().TrapData != null && slot.GetComponent<Room>().TrapData.RoomType == RoomType.LEVER)
                    leverList.Add(slot.GetComponent<Room>());
            });
            return leverList;
        }
    }
    public Room SelectedSlot
    {
        get { return _selectedSlot; }
    }

    public Room BossRoom { 
        get => _boss;
    }
    public float SlotSize { 
        get => _slotSize; 
    }
    public Room Start { 
        get => _start;
    }
    #endregion

    #region Events
    [SerializeField] private UnityEvent _onSetEffectOnRoomUnityEvent;
    #endregion

    [SerializeField, Required("RoomData required")] private RoomList _roomData;
    [SerializeField, Required("Background required GameObject")] private GameObject _background;
    [SerializeField, Required("Slot required GameObject")] private GameObject _slot;
    [SerializeField, Required("Grid required GameObject")] private GameObject _grid;
    [SerializeField] private List<GameObject> _slots = new List<GameObject>();
    [SerializeField, MinValue(2)] private int _heightSize = 8;
    [SerializeField, MinValue(2)] private int _widthSize = 15;
    [SerializeField, Range(1.1f, 1.5f)] private float _margin = 1.1f;
    private float _slotSize;
    private GameObject _grids;
    private GameObject _backgrounds;

    private Room _start = null;
    private Room _boss = null;
    private Room _selectedSlot = null;
    private Room _lastestSelectedSlot = null;
    private Coroutine _routineRoomMonster = null;
    private Coroutine _routineMoveHeroes = null;
    private Effect _effectRoomMonster = Effect.NONE;
    [SerializeField] private GameObject _menuEffectRoomMonster;

    [Button("Clear Map")]
    private void Clear()
    {
        if (_slots.Count > 0)
        {
            DestroyImmediate(_backgrounds);
            DestroyImmediate(_grids);
            foreach (var slot in _slots)
            {
                DestroyImmediate(slot);
            }
            _slots.Clear();
        }
    }

    [Button("Generate Map")]
    private void Generate()
    {
        GameObject instantiateBackground = null;
        GameObject instantiateObject = null;
        GameObject instantiateGrid = null;

        if (_slots.Count > 0)
        {
            DestroyImmediate(_backgrounds);
            DestroyImmediate(_grids);
            foreach (var slot in _slots)
            {
                DestroyImmediate(slot);
            }
            _slots.Clear();
        }
        transform.position = new Vector3(0, 0, 0);
        _grids = new GameObject("Grids");
        _grids.transform.parent = transform;
        GetComponent<Grid>().Init(_widthSize, _heightSize);
        _backgrounds = new GameObject("Backgrounds");
        _backgrounds.transform.parent = transform;
        GetComponent<BackgroundMap>().Init(_widthSize, _heightSize);
        for (int i = 0; i < _widthSize; i++)
        {
            for (int j = 0; j < _heightSize; j++)
            {
                instantiateBackground = Instantiate(_background, _backgrounds.transform);
                instantiateGrid = Instantiate(_grid, _grids.transform);
                instantiateObject = Instantiate(_slot, transform);
                instantiateBackground.name = "Background_" + i + "_" + j;
                instantiateObject.name = "Slot_" + i + "_" + j;
                instantiateGrid.name = "Grid_" + i + "_" + j;
                instantiateBackground.transform.position = new Vector3(_margin * i, _margin * j, 0.09f);
                instantiateObject.transform.position = new Vector3(_margin * i, _margin * j, 0);
                instantiateGrid.transform.position = instantiateObject.transform.position;
                instantiateBackground.GetComponent<SpriteRenderer>().sprite = GetComponent<BackgroundMap>().Sprite;
                instantiateObject.GetComponent<Room>().Init();
                instantiateGrid.GetComponent<SpriteRenderer>().sprite = GetComponent<Grid>().GetSprite(i, j);
                _slots.Add(instantiateObject);
            }
        }
        transform.position = new Vector3(-_margin * ((_widthSize - 2f) / 2f + 0.5f), -_margin * ((_heightSize - 2f) / 2f + 0.5f), 0);
        _grids.transform.position = transform.position;
    }

    #region FindFunctions
    private GameObject FindSlot(int x, int y)
    {
        return _slots[(x * (_heightSize)) + y ];
    }

    private GameObject FindSlot(Vector2 pos)
    {
        foreach (var slot in _slots) {
            if (slot.GetComponent<Room>().IsInBound(pos))
                return slot;
        }
        return null;
    }

    private Room FindRoom(int index)
    {
        return _slots[index].GetComponent<Room>();
    }

    private Room FindRoom(int x, int y)
    {
        return _slots[(x * (_heightSize)) + y ].GetComponent<Room>();
    }

    private Room FindRoom(Vector2 pos)
    {
        foreach (var slot in _slots) {
            if (slot.GetComponent<Room>().IsInBound(pos))
                return slot.GetComponent<Room>();
        }
        return null;
    }

    private Room FindRoom(Room room, Direction direction)
    {
        int index = _slots.IndexOf(room.gameObject);

        if (direction == Direction.Left && index - _heightSize >= 0)
            return _slots[index - _heightSize].GetComponent<Room>();
        if (direction == Direction.Right && index + _heightSize < _slots.Count)
            return _slots[index + _heightSize].GetComponent<Room>();
        if (direction == Direction.Up && index + 1 < _slots.Count && (index + 1) % _heightSize != 0)
            return _slots[index + 1].GetComponent<Room>();
        if (direction == Direction.Down && index - 1 >= 0 && (index - 1) % _heightSize != _heightSize - 1)
            return _slots[index - 1].GetComponent<Room>();
        return null;
    }
    #endregion

    private int GetIndexOfRoom(Room room)
    {
        return _slots.IndexOf(room.gameObject);
    }

    private void SetBuyableAdjacent()
    {
        int index = _slots.IndexOf(_selectedSlot.gameObject);

        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - 1].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + 1].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - _heightSize].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + _heightSize].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
    }

    private void SetBuyableAdjacent(Room indexedRoom)
    {
        int index = _slots.IndexOf(indexedRoom.gameObject);

        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().IsBuyable())
            _slots[index - 1].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().IsBuyable())
            _slots[index + 1].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().IsBuyable())
            _slots[index - _heightSize].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().IsBuyable())
            _slots[index + _heightSize].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.RoomList.RoomData[17], RoomColor.Buyable);
    }

    private void SetUnBuyableAdjacent(Room indexedRoom)
    {
        //Debug.Log($"Room unbuy {indexedRoom}");
        int index = _slots.IndexOf(indexedRoom.gameObject);

        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().IsNotBuy())
            _slots[index - 1].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.Square, RoomColor.NotBuyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().IsNotBuy())
            _slots[index + 1].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.Square, RoomColor.NotBuyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().IsNotBuy())
            _slots[index - _heightSize].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.Square, RoomColor.NotBuyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().IsNotBuy())
            _slots[index + _heightSize].GetComponent<Room>().SetData(GameManager.Instance.GeneralData.Square, RoomColor.NotBuyable);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this){
            Destroy(gameObject);
        }
        else
            _instance = this;
        if (_slot != null)
        {
            _slotSize = _slot.GetComponent<SpriteRenderer>().size.x;
        }
    }

    #region Init
    private void InitStart()
    {
        _start = FindRoom(_widthSize % 2 == 0 ? _widthSize / 2 - 1 : _widthSize / 2, 0);
        _start.SetColor(RoomColor.Usable, RoomColor.Usable);
        _start.SetData(GameManager.Instance.GeneralData.RoomList.RoomData[15], GameManager.Instance.GeneralData.TrapList.TrapData[0]);
    }
    public void InitLevel(LevelData data)
    {
        Clear();
        if (_routineChangeRoom != null)
        {
            StopCoroutine(_routineChangeRoom);
            _routineChangeRoom = null;
        }
        if (_routineRoomMonster != null)
        {
            StopCoroutine( _routineRoomMonster);
            _routineRoomMonster = null;
        }
        _isUpgradable = data.IsUpgradable;
        _editorState = EditorState.Select;
        _widthSize = data.MapWidth;
        _heightSize = data.MapHeight;
        _buyableRoomCount = data.NbMovesMax;
        _currentRoomCount = 0;
        _start = null;
        _boss = null;
        if (_grids != null)
            DestroyImmediate(_grids);
        _mapActions = new Stack<MapAction>();
        _routineChangeRoom = null;
        Generate();
        InitMap(data.PrePlacedElements);
        InitStart();
    }

    private void InitMap(PrePlacedElement ElementsToPlace)
    {
        if (ElementsToPlace == null)
            return;
        if (ElementsToPlace.PreplacedBoss != null) {
            _boss = FindRoom(ElementsToPlace.PreplacedBoss.x - 1, ElementsToPlace.PreplacedBoss.y - 1);
            _boss.SetData(GameManager.Instance.GeneralData.RoomList.RoomData[15], GameManager.Instance.GeneralData.TrapList.TrapData[9]);
        }
        if (ElementsToPlace.PreplacedObstacle != null) {
            foreach (Vector2Int Obstacle in ElementsToPlace.PreplacedObstacle) {
                FindRoom(Obstacle.x - 1, Obstacle.y - 1).SetData(GameManager.Instance.GeneralData.RoomList.RoomData[16], RoomColor.Unclickable);
            }
        }
    }
    #endregion

    private void Update()
    {
        SelectTiles();
    }

    private void SelectTiles()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cameraPos = CameraManager.Instance.Camera.transform.position;
        Room oldSelectedSlot = _selectedSlot;
        Room room = null;
        MapAction mapAction;
        float camOffset = -1.8f;

        if (Input.GetKeyDown(KeyCode.Mouse0) && _editorState != EditorState.Play && GameManager.Instance.NbMenuIn == 0 && !GameManager.Instance.IsInPlayMode) {
            if (_editorState == EditorState.Select || (cursorPos.y - cameraPos.y > camOffset && _editorState == EditorState.Edit)) // change the offset by phone size
                room = FindRoom(cursorPos);

            //UPGRADE BUTTON
            if (_selectedSlot != null &&
                _selectedSlot.UpgradeIcon.gameObject.activeSelf && 
                _selectedSlot.UpgradeIcon.HasTouchedUpgradeButton(cursorPos) && BuyableRoomCount > 0 &&
                _isUpgradable)
            {
                mapAction = new MapAction();
                mapAction.SetAction(GetIndexOfRoom(_selectedSlot), ActionType.Upgrade);
                _mapActions.Push(mapAction);
                if (_selectedSlot.TrapData != null && _selectedSlot.TrapData.Effect == Effect.MONSTRE) {
                    GameManager.Instance.NbMenuIn++;
                    _effectRoomMonster = Effect.NONE;
                    _routineRoomMonster = StartCoroutine(RoutineMonsterRoom());
                } else
                    _selectedSlot.UpgradeRoom();
                _currentRoomCount++;
                UIUpdateEditMode.Instance.UpdateNbActionsLeft(BuyableRoomCount);
                if (BuyableRoomCount <= 0)
                    SetUnBuyableAdjacent(_selectedSlot);
                return;
            }
            //START PLAY MODE (tap boss room)
            if (room != null && room == _boss && GameManager.Instance.IsPlayModeActive)
            {
                StartPlayMode(room);
            }
            //NORMAL ROOM
            if (room != null && room.IsClickable() && room != _boss) {
                //Debug.Log("SELECT");
                _selectedSlot = room != _selectedSlot ? room : null;
                _selectedSlot?.StartSelectionAnimation();
                if (_selectedSlot != null && _selectedSlot.RoomColor != RoomColor.NotBuyable) {
                    _selectedSlot.SetColor(RoomColor.Selected);
                    EditorManager.Instance.OpenEditorMenu();
                    _selectedSlot.EnableUpgrade();
                }
                if (oldSelectedSlot != null) {
                    _lastestSelectedSlot = oldSelectedSlot;
                    SetUnBuyableAdjacent(oldSelectedSlot);
                    oldSelectedSlot.UnSelect();
                }
            }
            /*if (_selectedSlot != null)
                Debug.Log($"Selected Slot = {_selectedSlot.IsUsable()}");*/
            if (_selectedSlot == null)
                EditorManager.Instance.CloseEditorMenu();
            else if (_selectedSlot.IsUsable()) {
                //Debug.Log($"BuyableRoomCount = {BuyableRoomCount}");
                if (BuyableRoomCount > 0)
                    SetBuyableAdjacent();
                else
                    SetUnBuyableAdjacent(_selectedSlot);
            }
        }
    }

    #region MonsterRoom
    private IEnumerator RoutineMonsterRoom()
    {
        _menuEffectRoomMonster.SetActive(true);
        yield return new WaitUntil(() => _effectRoomMonster != Effect.NONE);
        _menuEffectRoomMonster.SetActive(false);
        _selectedSlot.Effects.Add(_effectRoomMonster);

        _selectedSlot.UpgradeRoom(_effectRoomMonster);
        GameManager.Instance.NbMenuIn--;
    }

    public void ModifyRoomEffectMonster(int effect)
    {
        _effectRoomMonster = (Effect)effect;
    }
    #endregion
    public void SetDataOnSelectedRoom(RoomData data)
    {
        if (_selectedSlot != null) {
            _selectedSlot.SetData(data);
            SetBuyableAdjacent(_selectedSlot);
        }
    }

    public void SetDataOnSelectedTrap(TrapData data)
    {
        MapAction mapAction = new MapAction();

        if (_selectedSlot != null)
        { // && _boss != null pour stopper l'edition quand on a placé la salle du boss
            if (_selectedSlot.TrapData == null && BuyableRoomCount > 0)
            {
                //_selectedSlot.StopSelectionAnimation();
                mapAction.SetAction(GetIndexOfRoom(_selectedSlot), ActionType.Add);
                FindRoomPatern();
                _selectedSlot.PlayParticles();
                _selectedSlot.SetData(data);
                ElementList.Instance.ChangeUIElementValue(_selectedSlot.TrapData, -1);
                _currentRoomCount++;
                _onSetEffectOnRoomUnityEvent.Invoke();
            }
            if (_selectedSlot != _start && _selectedSlot.TrapData != data && _selectedSlot.TrapData != null)
            {
                if (mapAction.ActionType == ActionType.None)
                    mapAction.SetAction(GetIndexOfRoom(_selectedSlot), ActionType.Change, _selectedSlot.TrapData, _selectedSlot.RoomData, _selectedSlot.NbOfUpgrades);
                if (_selectedSlot.TrapData != null && (_selectedSlot.NbOfUpgrades > 0))
                { //si l'ancienne salle avait un upgrade on l'enlève
                    _selectedSlot.UndoUpgrade();
                    _currentRoomCount--;
                }
                _selectedSlot.PlayParticles();
                ElementList.Instance.ChangeUIElementValue(_selectedSlot.TrapData, 1);
                _selectedSlot.SetData(data);
                ElementList.Instance.ChangeUIElementValue(_selectedSlot.TrapData, -1);
                _onSetEffectOnRoomUnityEvent.Invoke();
            }
            if (BuyableRoomCount > 0)
            {
                SetBuyableAdjacent(_selectedSlot);
                _selectedSlot.EnableUpgrade();
            }
            UIUpdateEditMode.Instance.UpdateNbActionsLeft(BuyableRoomCount);
            if (BossIsAbove() && mapAction.ActionType == ActionType.Add)
            {
                FindRoomPatern(_selectedSlot, _boss);
                GameManager.Instance.SetPlayMode(true);
            }
        }
        if (mapAction.ActionType != ActionType.None)
            _mapActions.Push(mapAction);
    }

    private bool BossIsAbove()
    {
        int index = _slots.IndexOf(_selectedSlot.gameObject);

        if (index + 1 < _slots.Count && _slots[index + 1] != null && _slots[index + 1] == _boss.gameObject)
            return true;
        return false;
    }

    private bool BossIsAbove(Room room)
    {
        int index = _slots.IndexOf(room.gameObject);

        if (index + 1 < _slots.Count && _slots[index + 1] != null && _slots[index + 1] == _boss.gameObject)
            return true;
        return false;
    }

    private void FindRoomPatern()
    {
        Direction direction = _lastestSelectedSlot.RoomData.Directions;
        Direction newDirection = Direction.None;

        if (GetIndexOfRoom(_selectedSlot) - GetIndexOfRoom(_lastestSelectedSlot) == 1) {
            newDirection = Direction.Down;
            direction += (int)Direction.Up;
        } else if (GetIndexOfRoom(_selectedSlot) - GetIndexOfRoom(_lastestSelectedSlot) == -1) {
            newDirection = Direction.Up;
            direction += (int)Direction.Down;
        } else if (GetIndexOfRoom(_selectedSlot) - GetIndexOfRoom(_lastestSelectedSlot) == _heightSize) {
            newDirection = Direction.Left;
            direction += (int)Direction.Right;
        } else if (GetIndexOfRoom(_selectedSlot) - GetIndexOfRoom(_lastestSelectedSlot) == -_heightSize) {
            newDirection = Direction.Right;
            direction += (int)Direction.Left;
        }
        _lastestSelectedSlot.SetData(FindRoomDataByDirections(direction));
        _selectedSlot.SetData(FindRoomDataByDirections(newDirection));
    }

    private void FindRoomPatern(Room fromLink, Room toLink)
    {
        Direction direction = fromLink.RoomData.Directions;
        Direction newDirection = Direction.None;

        if (GetIndexOfRoom(toLink) - GetIndexOfRoom(fromLink) == 1) {
            newDirection = Direction.Down;
            direction += (int)Direction.Up;
        } else if (GetIndexOfRoom(toLink) - GetIndexOfRoom(fromLink) == -1) {
            newDirection = Direction.Up;
            direction += (int)Direction.Down;
        } else if (GetIndexOfRoom(toLink) - GetIndexOfRoom(fromLink) == _heightSize) {
            newDirection = Direction.Left;
            direction += (int)Direction.Right;
        } else if (GetIndexOfRoom(toLink) - GetIndexOfRoom(fromLink) == -_heightSize) {
            newDirection = Direction.Right;
            direction += (int)Direction.Left;
        }
        fromLink.SetData(FindRoomDataByDirections(direction));
        toLink.SetData(FindRoomDataByDirections(newDirection));
    }

    private RoomData FindRoomDataByDirections(Direction direction)
    {
        foreach (RoomData room in GameManager.Instance.GeneralData.RoomList.RoomData) {
            //Debug.Log($"direction = {direction} room.Directions = {room.Directions}");
            if ((int)room.Directions == -1 && (int)direction == 15)
                return room;
            if (room.Directions == direction)
                return room;
        }
        return null;
    }
    public void StartPlayMode(Room room)
    {
        _editorState = EditorState.Play;
        SetUnBuyableAdjacent(room);
        if (_selectedSlot != null)
        {
            SetUnBuyableAdjacent(_selectedSlot);
            _selectedSlot.UnSelect();
        }
        _selectedSlot = null;
        //Debug.Log($"Selected Slot = {_selectedSlot}");
        _grids.SetActive(false);
        GameManager.Instance.StartPlayMode();
        _routineChangeRoom = StartCoroutine(ImprovePathFinding());
    }

    #region Pathfinding
    public List<Room> Pathfinding()
    {
        List<Room> _travelList = new List<Room>();

        if (_start == null || _boss == null)
            return null;
        GetRoom(_start, _travelList);
        return _travelList;
    }

    private void GetRoom(Room room, List<Room> travelList = null)
    {
        Direction actualDirection = Direction.None;

        travelList.Add(room);
        actualDirection = room.RoomData.Directions;

        if (HaveDirection(ref actualDirection, Direction.Left) && !travelList.Contains(FindRoom(GetIndexOfRoom(room) - _heightSize))) {
            GetRoom(FindRoom(GetIndexOfRoom(room) - _heightSize), travelList);
            travelList.Add(room);
        }
        if (HaveDirection(ref actualDirection, Direction.Right) && !travelList.Contains(FindRoom(GetIndexOfRoom(room) + _heightSize))) {
            GetRoom(FindRoom(GetIndexOfRoom(room) + _heightSize), travelList);
            travelList.Add(room);
        }
        if (HaveDirection(ref actualDirection, Direction.Up) && !travelList.Contains(FindRoom(GetIndexOfRoom(room) + 1))) {
            GetRoom(FindRoom(GetIndexOfRoom(room) + 1), travelList);
            travelList.Add(room);
        }
        if (HaveDirection(ref actualDirection, Direction.Down) && !travelList.Contains(FindRoom(GetIndexOfRoom(room) - 1))) {
            GetRoom(FindRoom(GetIndexOfRoom(room) - 1), travelList);
            travelList.Add(room);
        }
    }

    private bool FindPathTo(Room actualRoom, List<Room> pathTo, Room roomToFind)
    {
        Direction actualDirection = Direction.None;
        bool find = false;

        pathTo.Add(actualRoom);
        actualDirection = actualRoom.RoomData.Directions;
        if (actualRoom == roomToFind)
            return true;
        if (HaveDirection(ref actualDirection, Direction.Left) && !pathTo.Contains(FindRoom(GetIndexOfRoom(actualRoom) - _heightSize)) && !find)
            find = FindPathTo(FindRoom(GetIndexOfRoom(actualRoom) - _heightSize), pathTo, roomToFind);
        if (HaveDirection(ref actualDirection, Direction.Right) && !pathTo.Contains(FindRoom(GetIndexOfRoom(actualRoom) + _heightSize)) && !find)
            find = FindPathTo(FindRoom(GetIndexOfRoom(actualRoom) + _heightSize), pathTo, roomToFind);
        if (HaveDirection(ref actualDirection, Direction.Up) && !pathTo.Contains(FindRoom(GetIndexOfRoom(actualRoom) + 1)) && !find)
            find = FindPathTo(FindRoom(GetIndexOfRoom(actualRoom) + 1), pathTo, roomToFind);
        if (HaveDirection(ref actualDirection, Direction.Down) && !pathTo.Contains(FindRoom(GetIndexOfRoom(actualRoom) - 1)) && !find)
            find = FindPathTo(FindRoom(GetIndexOfRoom(actualRoom) - 1), pathTo, roomToFind);
        if (find == false)
            pathTo.Remove(actualRoom);
        return find;
    }

    private List<List<Room>> FindObjectif()
    {
        List<List<Room>> travelLists;
        List<Room> leverList = new List<Room>();

        _slots.ForEach(slot => {
            if (slot.GetComponent<Room>().TrapData != null && slot.GetComponent<Room>().TrapData.RoomType == RoomType.LEVER)
                leverList.Add(slot.GetComponent<Room>());
        });
        travelLists = new List<List<Room>>();
        Debug.Log($"leverList.Count = {leverList.Count}");
        for (int i = 0; i < leverList.Count; i++) {
            travelLists.Add(new List<Room>());
            Debug.Log($"start = {_start.name} lever = {leverList[i].name} --------------------------------------------------------------------------------------");
            FindPathTo(_start, travelLists[i], leverList[i]);
        }
        return travelLists;
    }

    private List<List<Room>> FindObjectif(List<Room> leverList, Room actualRoom)
    {
        List<List<Room>> travelLists;

        if (leverList == null || leverList.Count == 0 || actualRoom == null)
            return null;
        travelLists = new List<List<Room>>();
        Debug.Log($"leverList.Count = {leverList.Count}");
        for (int i = 0; i < leverList.Count; i++) {
            travelLists.Add(new List<Room>());
            FindPathTo(actualRoom, travelLists[i], leverList[i]);
        }
        return travelLists;
    }

    private List<Room> FindObjectif(Room actualRoom, Room roomToFind)
    {
        List<Room> travelList = new List<Room>();

        if (actualRoom == null || roomToFind == null)
            return null;
        FindPathTo(actualRoom, travelList, roomToFind);
        return travelList;
    }

    [Button("Pathfinding")]
    public void PathfindingTest()
    {
        _editorState = EditorState.Play;
        GameManager.Instance.StartPlayMode();
        RoutineChangeRoom = StartCoroutine(ImprovePathFinding());
    }

    public IEnumerator ImprovePathFinding()
    {
        List<Room> leverList = ListOfLever;
        List<List<Room>> travelLists;
        List<Room> bestPath = new List<Room>();
        Room actualRoom = _start;
        Room lever = null;
        int lowestCount = _widthSize * _heightSize;

        // bestPath.Add(_start);
        // yield return GameManager.Instance.ChangeRoomFromPath(bestPath);
        //Debug.Log($"leverList.Count = {leverList.Count}");
        while (leverList != null && leverList.Count > 0) {
            //Debug.Log($"leverList.Count = {leverList.Count}");
            bestPath = null;
            travelLists = null;
            travelLists = FindObjectif(leverList, actualRoom);
            if (travelLists != null || travelLists.Count > 0) {
                lowestCount = GetLowestPathSize(travelLists);
                for (int i = 0; i < travelLists.Count; i++) {
                    if (travelLists[i].Count > lowestCount) {
                        travelLists.RemoveAt(i);
                        i--;
                    }
                }
                if (travelLists.Count > 1)
                    bestPath = MergeCommunSlot(travelLists);
                else 
                    bestPath = travelLists[0];
                // Debug.Log($"Actual Room : {actualRoom.name}");
                // PrintListOfRoom(bestPath);
                //bestPath.Insert(0, actualRoom);
                //bestPath.RemoveAt(0);
                yield return StartCoroutine(GameManager.Instance.ChangeRoomFromPath(bestPath));
                //Room lever = ask the player which path he want to take
                actualRoom = bestPath[bestPath.Count - 1];
                if (travelLists.Count > 1) {
                    List<List<Room>> keyRooms = travelLists.FindAll(path => {
                        Room lastRoom = path[path.Count - 1];
                        return lastRoom.TrapData != null && lastRoom.TrapData.RoomType == RoomType.LEVER;
                    });
                    foreach(List<Room> path in keyRooms)
                    {
                        path[path.Count - 1].StartLayerSelectionAnimation();
                    }
                    CameraManager.Instance.DezoomPlayMode();
                    yield return new WaitUntil(() => {
                        if (Input.GetKeyDown(KeyCode.Mouse0)) {
                            Room room = FindRoom(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                            if (room != null && room.TrapData != null && room.TrapData.RoomType == RoomType.LEVER) {
                                List<Room> foundedPath = travelLists.Find(path => path.Contains(room));
                                if (foundedPath != null) {
                                    //Debug.Log($"Room {room.name} selected");
                                    lever = room;
                                    return true;
                                } else
                                    return false;
                            } else
                                return false;
                        } else
                            return false;
                    });
                    //Enleve l'affichage des salles de clés
                    foreach (List<Room> path in keyRooms)
                    {
                        path[path.Count - 1].StopLayerSelectionAnimation();
                    }
                    CameraManager.Instance.Zoom();
                    // ici pour désafficher l'ui
                    bestPath = travelLists.Find(path => path.Contains(lever));
                    //Debug.Log($"Selected Path :");
                    bestPath.Insert(0, actualRoom);
                    //PrintListOfRoom(bestPath);
                    yield return StartCoroutine(GameManager.Instance.ChangeRoomFromPath(bestPath));
                }
                leverList.Remove(bestPath[bestPath.Count - 1]);
                actualRoom = bestPath[bestPath.Count - 1];
            }
        }
        bestPath = FindObjectif(actualRoom, _boss);
        yield return StartCoroutine(GameManager.Instance.ChangeRoomFromPath(bestPath));
    }

    private List<Room> MergeCommunSlot(List<List<Room>> path)
    {
        List<Room> newPath = new List<Room>();
        bool isIdentical = true;

        //Debug.Log($"path.Count = {path.Count} have a lenght of {path[0].Count}");
        for (int i = 0; i < path[0].Count && isIdentical;) {
            for (int j = 1; j < path.Count && isIdentical; j++) {
                if (path[j][0] == path[0][0]) {
                    isIdentical = true;
                } else {
                    isIdentical = false;
                    break;
                }
            }
            if (isIdentical) {
                newPath.Add(path[0][0]);
                for (int j = 0; j < path.Count; j++)
                    path[j].RemoveAt(0);
            }
        }
        return newPath;
    }

    private int GetLowestPathSize(List<List<Room>> paths)
    {
        int lowest = 0;

        foreach (List<Room> path in paths)
            if (lowest == 0 || path.Count < lowest)
                lowest = path.Count;
        return lowest;
    }

    private void PrintListOfRoom(List<Room> roomList)
    {
        string str = "";

        foreach (Room room in roomList) {
            str += room.name + " ";
        }
        Debug.Log(str);
    }

    #endregion
    public void UpdateMapIconPlayMode()
    {
        foreach (GameObject slot in _slots)
        {
            Room room = slot.GetComponent<Room>();
            if (room != null)
            {
                if (room.TrapData != null && room.TrapData.RoomType != RoomType.BOSS && room.TrapData.RoomType != RoomType.ENTRANCE)
                {
                    room.ClearIcon();
                }
                    
                if (room.TrapData != null && room.TrapData.RoomType == RoomType.LEVER)
                {
                    room.SetIconEffect();
                }
            }
        }
    }
    private bool HaveDirection(ref Direction direction , Direction directionToCheck)
    {
        Direction tmp = direction;

        if (tmp >= Direction.Down) {
            tmp -= Direction.Down;
            if (directionToCheck == Direction.Down) {
                direction -= Direction.Down;
                return true;
            }
        }
        if (tmp >= Direction.Up) {
            tmp -= Direction.Up;
            if (directionToCheck == Direction.Up) {
                direction -= Direction.Up;
                return true;
            }
        }
        if (tmp >= Direction.Left) {
            tmp -= Direction.Left;
            if (directionToCheck == Direction.Left) {
                direction -= Direction.Left;
                return true;
            }
        }
        if (tmp >= Direction.Right) {
            tmp -= Direction.Right;
            if (directionToCheck == Direction.Right) {
                direction -= Direction.Right;
                return true;
            }
        }
        return false;
    }

    public Direction GetRevertDirection(Direction direction)
    {
        Direction tmp = direction;
        Direction revertDirection = Direction.None;

        if (tmp >= Direction.Down) {
            tmp -= (int)Direction.Down;
            revertDirection = revertDirection | Direction.Up;
        }
        if (tmp >= Direction.Up) {
            tmp -= (int)Direction.Up;
            revertDirection = revertDirection | Direction.Down;
        }
        if (tmp >= Direction.Left) {
            tmp -= (int)Direction.Left;
            revertDirection = revertDirection | Direction.Right;
        }
        if (tmp >= Direction.Right) {
            tmp -= (int)Direction.Right;
            revertDirection = revertDirection | Direction.Left;
        }
        return revertDirection;
    }

    public void RemoveDirection(Room room, Direction direction)
    {
        Direction initialDirection = room.RoomData.Directions;

        initialDirection -= direction;
        room.SetData(FindRoomDataByDirections(initialDirection));
    }

    public string PrintDirection(Direction direction)
    {
        string str = "{";

        for (int i = (int) direction; i > 0; ) {
            if (i >= 8) {
                str += "Up";
                i -= 8;
            } else if (i >= 4) {
                str += "Down";
                i -= 4;
            } else if (i >= 2) {
                str += "Left";
                i -= 2;
            } else if (i >= 1) {
                str += "Right";
                i -= 1;
            }
            if (i > 0)
                str += ", ";
        }
        str += "}";
        return str;
    }


    public bool IsRoomATrap(Room room)
    {
        return room.TrapData != null && room.TrapData.RoomType == RoomType.NORMAL;
    }

    public bool IsEditComplete()
    {
        return _start != null && _boss != null;
    }

    public void Undo()
    {
        MapAction mapAction = _mapActions.Count > 0 ? _mapActions.Pop() : null;
        Direction direction = Direction.None;
        Room room = null;

        if (mapAction == null)
            return;
        room = FindRoom(mapAction.Index);
        if (mapAction.ActionType == ActionType.Add) {
            //Debug.Log($"Room {room} {BossIsAbove(room)} {room.RoomData.Directions}");
            if (BossIsAbove(room)) //Remove the link to the boss room, to get the right revert direction
            {
                Direction initialDirection = room.RoomData.Directions - (int)Direction.Up;
                room.SetData(FindRoomDataByDirections(initialDirection));
            }
            direction = GetRevertDirection(room.RoomData.Directions);
            RemoveDirection(FindRoom(room, room.RoomData.Directions), direction);
            ElementList.Instance.ChangeUIElementValue(room.TrapData, 1);
            room.UndoData(null, null, RoomColor.NotBuyable);
            if (_selectedSlot != null) {
                SetUnBuyableAdjacent(_selectedSlot);
                _selectedSlot.UnSelect();
            }
            _selectedSlot = null;
            SetUnBuyableAdjacent(room);
            _currentRoomCount--;
            if (BossIsAbove(room))
            {
                _boss.SetData(GameManager.Instance.GeneralData.RoomList.RoomData[15], GameManager.Instance.GeneralData.TrapList.TrapData[9]);
                GameManager.Instance.SetPlayMode(false);
            }
            EditorManager.Instance.CloseEditorMenu();
        } else if (mapAction.ActionType == ActionType.Change) {
            if (mapAction.Upgrade > 0) {
                room.UpgradeRoom();
                _currentRoomCount++;
            }
            ElementList.Instance.ChangeUIElementValue(room.TrapData, 1);
            room.UndoData(mapAction.TrapData);
            ElementList.Instance.ChangeUIElementValue(room.TrapData, -1);
        } else if (mapAction.ActionType == ActionType.Upgrade) {
            room.UndoUpgrade();
            _currentRoomCount--;
        }
        if (_selectedSlot != null && BuyableRoomCount > 0)
            SetBuyableAdjacent();
        
        UIUpdateEditMode.Instance.UpdateNbActionsLeft(BuyableRoomCount);
    }

    public void StopRoutines()
    {
        /*if (_routineMoveHeroes != null)
        {
            StopCoroutine(_routineMoveHeroes);
            _routineMoveHeroes = null;
        }

        if (_routineChangeRoom != null)
        {
            StopCoroutine( _routineChangeRoom);
            _routineChangeRoom = null;
        }
        if(_routineRoomMonster != null)
        {
            StopCoroutine(_routineRoomMonster);
            _routineRoomMonster = null;
        }*/
        StopAllCoroutines();
    }
}

public enum EditorState
{
    None = -1,
    Select = 0,
    Edit = 1,
    Play = 2,
}


public class MapAction
{
    private int _index;
    private ActionType _actionType;
    private TrapData _trapData;
    private RoomData _roomData;
    private int _upgrade;

    public int Index
    {
        get { return _index; }
        set { _index = value; }
    }

    public ActionType ActionType
    {
        get { return _actionType; }
        set { _actionType = value; }
    }

    public TrapData TrapData
    {
        get { return _trapData; }
    }

    public RoomData RoomData
    {
        get { return _roomData; }
    }

    public int Upgrade
    {
        get { return _upgrade; }
    }

    public MapAction()
    {
        _index = -1;
        _actionType = ActionType.None;
    }

    public void SetAction(int index, ActionType actionType, TrapData trapData = null, RoomData roomData = null, int upgrade = 0)
    {
        _index = index;
        _actionType = actionType;
        _trapData = trapData;
        _roomData = roomData;
        _upgrade = upgrade;
    }

    public void PrintAction()
    {
        Debug.Log($"Index = {_index} ActionType = {_actionType} TrapData = {_trapData} RoomData = {_roomData}");
    }
}

public enum ActionType
{
    None = -1,
    Add = 0,
    Remove = 1,
    Change = 2,
    Upgrade = 3,
}