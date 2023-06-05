using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room List", menuName = "GoldProject/Room List", order = 2)]
public class RoomList : ScriptableObject
{
    [SerializeField] private List<RoomData> _roomData = new List<RoomData>();

    public List<RoomData> RoomData
    {
        get { return _roomData; }
        set { _roomData = value; }
    }
}
