using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private RoomData _roomData;
    [SerializeField] private TrapData _trapData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private UIUpgradeButton _upgradeIcon;
    [SerializeField] private SpriteRenderer _borderRenderer;
    private GameObject _icon;

    public RoomData RoomData
    {
        get { return _roomData; }
        set { _roomData = value; }
    }

    public TrapData TrapData
    {
        get { return _trapData; }
        set { _trapData = value; }
    }

    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
        set { _spriteRenderer = value; }
    }

    private bool _isSelected;
    [ShowNonSerializedField] private RoomColor _roomColor = RoomColor.NotBuyable;

    public bool IsSelected
    {
        get { return _isSelected; }
        set { _isSelected = value; }
    }

    public RoomColor RoomColor
    {
        get { return _roomColor; }
        set {
            _roomColor = value;
            switch (_roomColor)
            {
                case RoomColor.Buyable:
                    _spriteRenderer.color = Color.green;
                    break;
                case RoomColor.NotBuyable:
                    _spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
                    break;
                case RoomColor.Usable:
                    _spriteRenderer.color = Color.white;
                    break;
                case RoomColor.Selected:
                    _spriteRenderer.color = Color.yellow;
                    break;
            }
        }
    }

    [ShowNonSerializedField] private RoomColor _oldState;

    List<Effect> _listEffects = new List<Effect>();
    bool _isActive = true;
    int _nbOfUpgrades = 0;

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    public List<Effect> Effects
    {
        get => _listEffects;
        private set => _listEffects = value;
    }
    public int NbOfUpgrades
    {
        get => _nbOfUpgrades;
        set => _nbOfUpgrades = value;
    }

    public bool IsElementary
    {
        get => _listEffects[0] == Effect.FOUDRE || _listEffects[0] == Effect.FEU || _listEffects[0] == Effect.GLACE;
    }
    public UIUpgradeButton UpgradeIcon { 
        get => _upgradeIcon;
    }

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        RoomColor = RoomColor.NotBuyable;
        _oldState = _roomColor;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CreateIconObject();
        _oldState = _roomColor;
    }
    
    private void CreateIconObject()
    {
        float offsetZ = 0.10f;

        _icon = new GameObject();
        _icon.name = transform.name + "_Icon";
        _icon.transform.parent = transform;
        _icon.transform.localPosition = new Vector3(0, 0, -offsetZ);
        _icon.AddComponent<SpriteRenderer>();
        _icon.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }

    public void SetData(RoomData roomData)
    {
        if (_roomColor != RoomColor.Selected)
            RoomColor = RoomColor.Usable;
        _oldState = RoomColor.Usable;
        _roomData = roomData;
        //Debug.Log($"RoomName = {transform.name} Selected = {MapManager.Instance.SelectedSlot} data = {roomData}");
        SetSprite(_roomData.Sprite);
    }

    public void SetData(TrapData trapData)
    {
        if (_roomColor != RoomColor.Selected)
            RoomColor = RoomColor.Usable;
        _oldState = RoomColor.Usable;
        _trapData = trapData;
        _listEffects.Add(trapData.Effect);
        SetIcon(trapData.Sprite);
    }

    public void SetData(RoomData roomData, TrapData trapData)
    {
        if (_roomColor != RoomColor.Selected)
            RoomColor = RoomColor.Usable;
        _oldState = RoomColor.Usable;
        _roomData = roomData;
        _trapData = trapData;
        SetIcon(_trapData.Sprite);
        SetSprite(_roomData.Sprite);
    }

    public void UndoData(TrapData trapData)
    {
        Debug.Log($"undo Data by trapData {trapData}");
        _trapData = trapData;
        if (trapData == null) {
            RoomColor = RoomColor.NotBuyable;
            SetIcon(null);
            SetSprite(GameManager.Instance.GeneralData.Square);
        }
        else {
            RoomColor = RoomColor.Usable;
            SetIcon(trapData.Sprite);
        }
        EnableUpgrade();
    }

    public void UndoData(RoomData roomData, TrapData trapData)
    {
        Debug.Log($"undo Data by roomData {roomData} && trapData {trapData}");
        _roomData = roomData;
        _trapData = trapData;
        if (roomData == null || trapData == null) {
            RoomColor = RoomColor.NotBuyable;
            SetIcon(null);
            SetSprite(GameManager.Instance.GeneralData.Square);
        }
        else {
            RoomColor = RoomColor.Usable;
            SetIcon(trapData.Sprite);
            SetSprite(roomData.Sprite);
        }
        EnableUpgrade();
    }

    public void UndoData(RoomData roomData, TrapData trapData, RoomColor roomColor = RoomColor.NotBuyable)
    {
        Debug.Log($"undo Data by roomData {roomData} && trapData {trapData} && roomColor {roomColor}");
        _roomData = roomData;
        _trapData = trapData;
        RoomColor = roomColor;
        _oldState = roomColor;
        SetIcon(trapData == null ? null : trapData.Sprite);
        SetSprite(roomData == null ? GameManager.Instance.GeneralData.Square : roomData.Sprite);
        EnableUpgrade();
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetIcon(Sprite sprite)
    {
        _icon.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        _icon.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetColor(RoomColor color)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_roomColor != RoomColor.Selected)
            _oldState = color != _oldState ? _roomColor : _oldState;
        RoomColor = color;
    }

    public bool IsInBound(Vector2 mousePosition)
    {
        Vector3 size = transform.localScale;
        Vector3 position = transform.position;

        float left = position.x - size.x / 2;
        float right = position.x + size.x / 2;
        float top = position.y + size.y / 2;
        float bottom = position.y - size.y / 2;

        return mousePosition.x > left && mousePosition.x < right && mousePosition.y > bottom && mousePosition.y < top;
    }

    public void UnSelect()
    {
        RoomColor = _oldState;
        _upgradeIcon.gameObject.SetActive(false);
        //Debug.Log($"RoomName = {transform.name} Selected = {MapManager.Instance.SelectedSlot} data = {_roomData}");
        if (_roomData == null) {
            if (MapManager.Instance.SelectedSlot == null)
                RoomColor = RoomColor.NotBuyable;
            _oldState = RoomColor.NotBuyable;
        }
    }

    public void EnableUpgrade()
    {
        if (NbOfUpgrades == 0 && _trapData != null && MapManager.Instance.IsRoomATrap(this)) // A AJOUTER check si le nombre d'actions est plus grand que 0
        {
            _upgradeIcon.gameObject.SetActive(true);
        } else
        {
            _upgradeIcon.gameObject.SetActive(false);
        }
    }

    public void UpgradeRoom()
    {
        _nbOfUpgrades++;
        EnableUpgrade();
    }

    public void UndoUpgrade()
    {
        _nbOfUpgrades--;
        EnableUpgrade();
    }
}

public enum RoomColor
{
    Buyable = 0,
    NotBuyable = 1,
    Usable = 2,
    Selected = 3
}