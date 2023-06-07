using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    private Stack<MapAction> _mapActions = new Stack<MapAction>();
    private EditorState _editorState = EditorState.Select;
    private int _buyableRoomCount = 5;
    private int _currentRoomCount = 0;

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
    #endregion

    [SerializeField] private TMP_Text _roomText;
    [SerializeField, Required("RoomData required")] private RoomList _roomData;
    [SerializeField, Required("Slot required GameObject")] private GameObject _slot;
    [SerializeField] private List<GameObject> _slots = new List<GameObject>();
    [SerializeField, MinValue(2)] private int _heightSize = 8;
    [SerializeField, MinValue(2)] private int _widthSize = 15;
    [SerializeField, Range(1.1f, 1.5f)] private float _margin = 1.1f;

    private Room _start = null;
    private Room _boss = null;
    private Room _selectedSlot = null;
    private Room _lastestSelectedSlot = null;

    public Room SelectedSlot
    {
        get { return _selectedSlot; }
    }

    public Room BossRoom { 
        get => _boss;
    }

    [Button("Clear Map")]
    private void Clear()
    {
        if (_slots.Count > 0)
        {
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
        GameObject instantiateObject = null;

        if (_slots.Count > 0)
        {
            foreach (var slot in _slots)
            {
                DestroyImmediate(slot);
            }
            _slots.Clear();
        }
        transform.position = new Vector3(0, 0, 0);
        for (int i = 0; i < _widthSize; i++)
        {
            for (int j = 0; j < _heightSize; j++)
            {
                instantiateObject = Instantiate(_slot, transform);
                instantiateObject.name = "Slot_" + i + "_" + j;
                instantiateObject.transform.position = new Vector3(_margin * i, _margin * j, 0);
                instantiateObject.GetComponent<Room>().Init();
                _slots.Add(instantiateObject);
            }
        }
        transform.position = new Vector3(-_margin * ((_widthSize - 2f) / 2f + 0.5f), -_margin * ((_heightSize - 2f) / 2f + 0.5f), 0);
    }

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

    private int GetIndexOfRoom(Room room)
    {
        return _slots.IndexOf(room.gameObject);
    }

    private void SetBuyableAdjacent()
    {
        int index = _slots.IndexOf(_selectedSlot.gameObject);

        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
    }

    private void SetBuyableAdjacent(Room indexedRoom)
    {
        int index = _slots.IndexOf(indexedRoom.gameObject);

        //Debug.Log("SetBuyableAdjacent");
        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().RoomColor != RoomColor.Usable)
            _slots[index - 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().RoomColor != RoomColor.Usable)
            _slots[index + 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().RoomColor != RoomColor.Usable)
            _slots[index - _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().RoomColor != RoomColor.Usable)
            _slots[index + _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
    }

    private void SetUnBuyableAdjacent(Room indexedRoom)
    {
        int index = _slots.IndexOf(indexedRoom.gameObject);

        //Debug.Log("SetUnBuyableAdjacent");
        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().RoomColor == RoomColor.Buyable)
            _slots[index - 1].GetComponent<Room>().SetColor(RoomColor.NotBuyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().RoomColor == RoomColor.Buyable)
            _slots[index + 1].GetComponent<Room>().SetColor(RoomColor.NotBuyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().RoomColor == RoomColor.Buyable)
            _slots[index - _heightSize].GetComponent<Room>().SetColor(RoomColor.NotBuyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().RoomColor == RoomColor.Buyable)
            _slots[index + _heightSize].GetComponent<Room>().SetColor(RoomColor.NotBuyable);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this;
    }

    private void Start()
    {
    } 

    private void InitStart()
    {
        _start = FindRoom(_widthSize % 2 == 0 ? _widthSize / 2 - 1 : _widthSize / 2, 0);
        _start.SetColor(RoomColor.Buyable);
        _start.SetData(GameManager.Instance.GeneralData.RoomList.RoomData[15], GameManager.Instance.GeneralData.TrapList.TrapData[0]);
    }

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

        if (Input.GetKeyDown(KeyCode.Mouse0) && _editorState != EditorState.Play) {
            Debug.Log($"Click in {cursorPos} Camera in {cameraPos} position by Camera {cursorPos - cameraPos}");
            if (_editorState == EditorState.Select || (cursorPos.y - cameraPos.y > camOffset && _editorState == EditorState.Edit)) // change the offset by phone size
                room = FindRoom(cursorPos);

            if (_selectedSlot != null &&
                _selectedSlot.UpgradeIcon.gameObject.activeSelf && 
                _selectedSlot.UpgradeIcon.HasTouchedUpgradeButton(cursorPos))
            {
                mapAction = new MapAction();
                mapAction.SetAction(GetIndexOfRoom(_selectedSlot), ActionType.Upgrade);
                _mapActions.Push(mapAction);
                _selectedSlot.UpgradeRoom();
                _currentRoomCount++;
                UIUpdateEditMode.Instance.UpdateNbActionsLeft(BuyableRoomCount);
                return;
            }
            if (room != null && room == _boss)
            {
                Debug.Log($"Play Mode");
                _editorState = EditorState.Play;
                SetUnBuyableAdjacent(room);
                _selectedSlot.UnSelect();
                _selectedSlot = null;
                Debug.Log($"Selected Slot = {_selectedSlot}");
                GameManager.Instance.StartPlayMode();
                return;
            }
            if (room != null && room.RoomColor != RoomColor.NotBuyable) {
                _selectedSlot = room != _selectedSlot ? room : null;
            }
            if (room != null && room.RoomColor != RoomColor.NotBuyable) {
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
            if (_selectedSlot == null)
                EditorManager.Instance.CloseEditorMenu();
            else if (_selectedSlot.RoomData != null) {
                SetBuyableAdjacent();
            }
        }
    }

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

        if (_selectedSlot != null && _boss == null) {
            if (_selectedSlot.TrapData == null) {
                mapAction.SetAction(GetIndexOfRoom(_selectedSlot), ActionType.Add);
                FindRoomPatern();
            }
            if (_selectedSlot != _start) {
                if (mapAction.ActionType == ActionType.None)
                    mapAction.SetAction(GetIndexOfRoom(_selectedSlot), ActionType.Change, data);
                _selectedSlot.SetData(data);
            }
            if (data.Name == "Boss Room") {
                _boss = _selectedSlot;
                ElementList.Instance.RemoveBossRoom();
            }
            SetBuyableAdjacent(_selectedSlot);
            _selectedSlot.EnableUpgrade();
            _currentRoomCount++;
            UIUpdateEditMode.Instance.UpdateNbActionsLeft(BuyableRoomCount);
        }
        mapAction.PrintAction();
        _mapActions.Push(mapAction);
        if (IsEditComplete())
        {
            GameManager.Instance.SetPlayMode(true);
        }
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

    private RoomData FindRoomDataByDirections(Direction direction)
    {
        foreach (RoomData room in GameManager.Instance.GeneralData.RoomList.RoomData) {
            if ((int)room.Directions == -1 && (int)direction == 15)
                return room;
            if (room.Directions == direction)
                return room;
        }
        return null;
    }

    [Button("Pathfinding")]
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

        Debug.Log($"Room = {room.name} actualDirection = {PrintDirection(actualDirection)}");
        //pathfinding with recursion with using actualdirection
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

    private bool HaveDirection(ref Direction direction , Direction directionToCheck)
    {
        Direction tmp = direction;

        //Debug.Log($"{direction} & {directionToCheck}");
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

    public void InitLevel(LevelData data)
    {
        Clear();
        _editorState = EditorState.Select;
        _widthSize = data.MapWidth;
        _heightSize = data.MapHeight;
        _buyableRoomCount = data.NbMovesMax;
        _currentRoomCount = 0;
        _start = null;
        _boss = null;
        Generate();
        InitStart();
    }

    public bool IsRoomATrap(Room room)
    {
        return room != _start && room != _boss && room == _selectedSlot;
    }

    public bool IsEditComplete()
    {
        return _start != null && _boss != null;
    }

    public void Undo()
    {
        MapAction mapAction = _mapActions.Count > 0 ? _mapActions.Pop() : null;
        Room room = null;

        if (mapAction == null)
            return;
        room = FindRoom(mapAction.Index);
        Debug.Log("Room "+ room.RoomData);
        mapAction.PrintAction();
        if (mapAction.ActionType == ActionType.Add) {
            room.UndoData(null, null, RoomColor.NotBuyable);
            _selectedSlot = null;
            SetUnBuyableAdjacent(room);
            Debug.Log(room.TrapData);
            //Debug.Log(room.);
            /*if (room.TrapData.RoomType == RoomType.BOSS)
            {
                GameManager.Instance.SetPlayMode(false);
                //ElementList.Instance.
            }*/
        }
        else if (mapAction.ActionType == ActionType.Change) {
            room.UndoData(mapAction.TrapData);
        } else if (mapAction.ActionType == ActionType.Upgrade) {
            room.UndoUpgrade();
        }
        _currentRoomCount--;
        UIUpdateEditMode.Instance.UpdateNbActionsLeft(BuyableRoomCount);
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

    public MapAction()
    {
        _index = -1;
        _actionType = ActionType.None;
    }

    public void SetAction(int index, ActionType actionType, TrapData trapData = null, RoomData roomData = null)
    {
        _index = index;
        _actionType = actionType;
        _trapData = trapData;
        _roomData = roomData;
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