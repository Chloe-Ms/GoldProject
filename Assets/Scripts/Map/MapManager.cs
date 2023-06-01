using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;

    public static MapManager Instance
    {
        get { return _instance; }
    }

    private EditorState _editorState = EditorState.Select;
    private int _buyableRoomCount = 5;
    private int _currentRoomCount = 0;

    public EditorState EditorState
    {
        get { return _editorState; }
        set { _editorState = value; }
    }

    public int BuyableRoomCount
    {
        get { return _buyableRoomCount - _currentRoomCount; }
    }

    [SerializeField] private TMP_Text _roomText;
    [SerializeField, Required("RoomData required")] private RoomList _roomData;
    [SerializeField, Required("Slot required GameObject")] private GameObject _slot;
    [SerializeField] private List<GameObject> _slots = new List<GameObject>();
    [SerializeField, MinValue(2)] private int _heightSize = 8;
    [SerializeField, MinValue(2)] private int _widthSize = 15;
    [SerializeField, Range(1.1f, 1.5f)] private float _margin = 1.1f;

    private Room _selectedSlot = null;
    private Room _lastestSelectedSlot = null;

    public Room SelectedSlot
    {
        get { return _selectedSlot; }
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

        Debug.Log("SetBuyableAdjacent");
        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
    }

    private void SetUnBuyableAdjacent(Room indexedRoom)
    {
        int index = _slots.IndexOf(indexedRoom.gameObject);

        Debug.Log("SetUnBuyableAdjacent");
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
        InitStart();
    }

    private void InitStart()
    {
        Room start = null;

        start = FindRoom(_widthSize % 2 == 0 ? _widthSize / 2 - 1 : _widthSize / 2, 0);
        start.SetColor(RoomColor.Buyable);
        start.SetData(GameManager.Instance.GeneralData.RoomList.RoomData[0]);
        UpdateText();
        Debug.Log($"Start in {start.RoomColor}");
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
        float camOffset = 1.8f;

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            //Debug.Log($"Click in {cursorPos} Camera in {cameraPos} position by Camera {cursorPos - cameraPos}");
            if (_editorState == EditorState.Select || (cursorPos.y - cameraPos.y < camOffset && _editorState == EditorState.Edit)) // change the offset by phone size
                room = FindRoom(cursorPos);
            if (room != null && room.RoomColor != RoomColor.NotBuyable) {
                _selectedSlot = room != _selectedSlot ? room : null;
            }
            if (_selectedSlot != null && _selectedSlot.RoomColor != RoomColor.NotBuyable) {
                if (_selectedSlot.RoomColor == RoomColor.Usable && _selectedSlot.RoomData != null) {
                    SetBuyableAdjacent();
                }
                _selectedSlot.SetColor(RoomColor.Selected);
            }
            if (oldSelectedSlot != null) {
                _lastestSelectedSlot = oldSelectedSlot;
                SetUnBuyableAdjacent(oldSelectedSlot);
                oldSelectedSlot.UnSelect();
            }
        }
    }

    private void UpdateText()
    {
        _roomText.text = $"You have {BuyableRoomCount} rooms buyable";
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
        if (_selectedSlot != null) {
            _selectedSlot.SetData(data);
            FindRoomPatern();
            SetBuyableAdjacent(_selectedSlot);
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
        } else
            direction = Direction.None;
        _lastestSelectedSlot.SetData(FindRoomDataByDirections(newDirection));
        _selectedSlot.SetData(FindRoomDataByDirections(direction));
    }

    private RoomData FindRoomDataByDirections(Direction direction)
    {
        foreach (RoomData room in GameManager.Instance.GeneralData.RoomList.RoomData) {
            if (room.Directions == direction)
                return room;
        }
        return null;
    }

}

public enum EditorState
{
    None = -1,
    Select = 0,
    Edit = 1,
}