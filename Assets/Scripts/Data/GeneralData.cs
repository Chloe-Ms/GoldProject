using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "General Data", menuName = "GoldProject/General Data", order = 0)]
public class GeneralData : ScriptableObject
{
    [SerializeField] private RoomList _roomList;
    [SerializeField] private TrapList _trapList;
    [SerializeField] private Sprite _square;
    [SerializeField] private Sprite _spriteMonsterUpgrade;

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

    public Sprite SpriteMonsterUpgrade {
        get => _spriteMonsterUpgrade;
    }
}
