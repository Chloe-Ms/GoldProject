using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Trap Data", menuName = "GoldProject/Trap Data", order = 3)]
public class TrapData : ScriptableObject
{
    [SerializeField] private TextLanguage _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _spriteUI;
    [SerializeField] private Effect _effect;
    [SerializeField] private int _nbRoomsBeforeEffect;
    [SerializeField] private RoomType _roomType;
    [SerializeField] private Color _color;
    [ResizableTextArea,SerializeField] private TextLanguage _description;
    [SerializeField] private string _soundWhenApplied;
    [SerializeField] private Sprite _bgEffectUIImage;
    [SerializeField] private Sprite _roomEffectImage;
    [ShowIf("IsMonsterRoom")]
    [SerializeField] private Sprite[] _roomMonsterUpgradeImages = new Sprite[7];
    [SerializeField] private bool _isRoomEffectImageBehindHeroes;
    [SerializeField] private RuntimeAnimatorController _animatorBGEffectUI;
    private bool IsMonsterRoom()
    {
        return _effect == Effect.MONSTRE;
    }
    public TextLanguage Name
    {
        get { return _name; }
    }

    public string EnglishName
    {
        get { return _name.GetStringInLanguage(Language.EN); }
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
    public TextLanguage Description { 
        get => _description;
    }
    public Sprite BgEffectUIImage { 
        get => _bgEffectUIImage;
    }
    public Sprite RoomEffectImage { 
        get => _roomEffectImage;
    }
    public bool IsRoomEffectImageBehindHeroes {
        get => _isRoomEffectImageBehindHeroes;
    }
    public Sprite[] RoomMonsterImages { 
        get => _roomMonsterUpgradeImages; 
    }
    public RuntimeAnimatorController AnimatorBGEffectUI { 
        get => _animatorBGEffectUI;
        set => _animatorBGEffectUI = value; 
    }
}

public enum RoomType
{
    NORMAL,
    BOSS,
    LEVER,
    ENTRANCE
}
