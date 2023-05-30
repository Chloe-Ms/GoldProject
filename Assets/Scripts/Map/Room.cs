using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class Room : MonoBehaviour
{
    [SerializeField] private RoomData _roomData;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public RoomData RoomData
    {
        get { return _roomData; }
        set { _roomData = value; }
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
                    _spriteRenderer.color = Color.black;
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

    public void Init()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        RoomColor = RoomColor.NotBuyable;
        _oldState = _roomColor;
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _oldState = _roomColor;
    }

    public void SetData(RoomData roomData)
    {
        RoomColor = RoomColor.Usable;
        _roomData = roomData;
        SetSprite(_roomData.Sprite);
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetColor(RoomColor color)
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        _oldState = _roomColor != _oldState ? _roomColor : _oldState;
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
    }
}

public enum RoomColor
{
    Buyable = 0,
    NotBuyable = 1,
    Usable = 2,
    Selected = 3
}