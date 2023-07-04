using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "General Data", menuName = "GoldProject/General Data", order = 0)]
public class GeneralData : ScriptableObject
{
    [SerializeField] private RoomList _roomList;
    [SerializeField] private TrapList _trapList;
    [SerializeField] private Sprite _square;
    [SerializeField] private Sprite _spriteMonsterUpgradeBottom;
    [SerializeField] private Sprite _spriteMonsterUpgradeTop;
    [SerializeField] private Sprite _spriteBossRoom;

    public RoomList RoomList
    {
        get { return _roomList; }
    }

    public TrapList TrapList
    {
        get { return _trapList; }
    }

    public Sprite Square
    {
        get { return _square; }
    }

    public Sprite SpriteMonsterUpgradeBottom {
        get => _spriteMonsterUpgradeBottom;
    }
    public Sprite SpriteMonsterUpgradeTop
    {
        get => _spriteMonsterUpgradeTop;
    }
    public Sprite SpriteBossRoom { 
        get => _spriteBossRoom;
    }
}
