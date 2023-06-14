using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Trap Data", menuName = "GoldProject/Trap Data", order = 3)]
public class TrapData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _spriteUI;
    [SerializeField] private Effect _effect;
    [SerializeField] private int _nbRoomsBeforeEffect;
    [SerializeField] private RoomType _roomType;
    [SerializeField] private Color _color;
    [ResizableTextArea,SerializeField] private string _description;
    [SerializeField] private string _soundWhenApplied;
    public string Name
    {
        get { return _name; }
    }

    public Sprite Sprite
    {
        get { return _sprite; }
    }

    public Effect Effect { 
        get => _effect; 
    }
    public Sprite SpriteUI { 
        get => _spriteUI; 
    }
    public int NbRoomsBeforeEffect { 
        get => _nbRoomsBeforeEffect;
    }
    public RoomType RoomType { 
        get => _roomType;
    }
    public Color Color
    {
        get { return _color; }
    }

    public string SoundWhenApplied { 
        get => _soundWhenApplied;
    }
    public string Description { 
        get => _description;
    }
}

public enum RoomType
{
    NORMAL,
    BOSS,
    LEVER,
    ENTRANCE
}
