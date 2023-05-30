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

        if (index - 1 >= 0 && (index - 1) % _heightSize == (index % _heightSize) - 1 && _slots[index - 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + 1 < _slots.Count &&  (index + 1) % _heightSize == (index % _heightSize) + 1 && _slots[index + 1].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + 1].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index - _heightSize >= 0 && _slots[index - _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index - _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
        if (index + _heightSize < _slots.Count && _slots[index + _heightSize].GetComponent<Room>().RoomColor == RoomColor.NotBuyable)
            _slots[index + _heightSize].GetComponent<Room>().SetColor(RoomColor.Buyable);
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
        start.SetData(_roomData.RoomData[8]);
        SetBuyableAdjacent(start);
        _currentRoomCount++;
        UpdateText();
    }

    private void Update()
    {
        if (_editorState == EditorState.Select)
            SelectTiles();
    }

    private void SelectTiles()
    {
        Vector2 CursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Room room = null;

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (_selectedSlot != null)
                _selectedSlot.UnSelect();
            room = FindRoom(CursorPos);
            if (room != null && room.RoomColor != RoomColor.NotBuyable)
                _selectedSlot = room != _selectedSlot ? room : null;
            if (_selectedSlot != null && _selectedSlot.RoomColor != RoomColor.NotBuyable) {
                if (_selectedSlot.RoomColor == RoomColor.Buyable && _selectedSlot.RoomData != null) {
                    SetBuyableAdjacent();
                    _currentRoomCount++;
                    UpdateText();
                }
                _selectedSlot.SetColor(RoomColor.Selected);
            }
        }
    }

    private void UpdateText()
    {
        _roomText.text = $"You have {BuyableRoomCount} rooms buyable";
    }
}

public enum EditorState
{
    None = -1,
    Select = 0,
    Edit = 1,
}