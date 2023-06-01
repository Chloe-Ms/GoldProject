using System;
using System.Collections.Generic;

public class RoomEffectManager
{
    private static List<EffectEvent> _effectsEvent = new List<EffectEvent>();
    private static Dictionary<Effect, UpdatedRoomEffect> _effectsOnRoom = new Dictionary<Effect, UpdatedRoomEffect>() {
        {
            Effect.PHYSIQUE,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    trap.IsActive = true;
                }
            )
        },
        {
            Effect.MONSTRE,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    //Display
                }
            )
        },
        {
            Effect.POISON,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    group.IsPoisoned = true;
                }
            )
        },
        {
            Effect.PLANTE,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    //Ajouter un event pour la mort 
                }
            )
        },
        {
            Effect.GLACE,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    _effectsEvent.Add(new EffectEvent(1,Effect.GLACE,_effectsAppliedAfterRoom[Effect.GLACE]));
                }
            )
        },

    };

    private static Dictionary<Effect, Action<Trap, Group>> _effectsAppliedAfterRoom = new Dictionary<Effect, Action<Trap, Group>>() {
        {
            Effect.GLACE,
            (Trap trap,Group group) => {
                //HIHIHIHIHi
            }
        },

    };

    public static Dictionary<Effect, Action<Trap, Group>> EffectsAppliedAfterRoom {
        get => _effectsAppliedAfterRoom; 
        set => _effectsAppliedAfterRoom = value; 
    }
    public static Dictionary<Effect, UpdatedRoomEffect> EffectsOnRoom {
        get => _effectsOnRoom; 
        private set => _effectsOnRoom = value; 
    }
    public static List<EffectEvent> EffectsEvent {
        get => _effectsEvent; 
        set => _effectsEvent = value; 
    }
}

public class UpdatedRoomEffect
{
    private Action<Trap,Group> _onRoomEnter;

    public UpdatedRoomEffect(Action<Trap,Group> onRoomEnter)
    {
        _onRoomEnter += onRoomEnter;
    }
    public UpdatedRoomEffect(UpdatedRoomEffect room)
    {
        _onRoomEnter += room.OnRoomEnter;
    }


    public Action<Trap, Group> OnRoomEnter { 
        get => _onRoomEnter; 
        set => _onRoomEnter = value; 
    }
}

public class EffectEvent
{
    private int _nbRoomBeforeApplied;
    public int NbRoomBeforeApplied { 
        get => _nbRoomBeforeApplied; 
        set => _nbRoomBeforeApplied = value; 
    }
    public Effect Effect {
        get => _effect; 
        set => _effect = value; 
    }

    private Action<Trap, Group> _onRoomEffectApplied;
    private Effect _effect;

    public EffectEvent(int nbRoomBeforeApplied, Effect effect, Action<Trap, Group> onRoomEffectApplied)
    {
        _nbRoomBeforeApplied = nbRoomBeforeApplied;
        _effect = effect;
        _onRoomEffectApplied += onRoomEffectApplied;
    }
}