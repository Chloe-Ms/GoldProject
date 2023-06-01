using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEffectManager : MonoBehaviour
{
    private Dictionary<Effect, UpdatedRoomEffect> _effects;
}

public class UpdatedRoomEffect
{
    private Action _onRoomEnter;
}