using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Room Data", menuName = "GoldProject/Room Data", order = 1)]
public class RoomData : ScriptableObject
{
    [SerializeField, ShowAssetPreview] private Sprite _sprite;
    [SerializeField, EnumFlags] private Direction _directions;

    public Sprite Sprite {
        get { return _sprite; }
    }

    public Direction Directions {
        get { return (int)_directions != -1 ? _directions : Direction.Right | Direction.Left | Direction.Up | Direction.Down; }
        private set { _directions = value; }
    }
}

[System.Flags]
public enum Direction
{
    None = 0,
    Right = 1 << 0,
    Left = 1 << 1,
    Up = 1 << 2,
    Down = 1 << 3,
}