using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomEffectManager
{
    private List<EffectEvent> _effectsEvent = new List<EffectEvent>();
    private Dictionary<Effect, UpdatedRoomEffect> _effects;

}

public class UpdatedRoomEffect
{
    private Action<Trap> _onRoomEnter;
}

public class EffectEvent
{
    private int _nbRoomBeforeApplied;
    public int NbRoomBeforeApplied { 
        get => _nbRoomBeforeApplied; 
        private set => _nbRoomBeforeApplied = value; 
    }

    private Effect _effect;

    public EffectEvent(int nbRoomBeforeApplied, Effect effect)
    {
        _nbRoomBeforeApplied = nbRoomBeforeApplied;
        _effect = effect;
    }
}