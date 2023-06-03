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
                    //Nothing
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
                    group.AffectedByPlants = true;
                }
            )
        },
        {
            Effect.GLACE,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    _effectsEvent.Add(new EffectEvent(trap.TrapData.NbRoomsBeforeEffect,Effect.GLACE,_effectsAppliedAfterRoom[Effect.GLACE]));
                }
            )
        },
        {
            Effect.FEU,
            new UpdatedRoomEffect(
                (Trap trap,Group group) => {
                    _effectsEvent.Add(new EffectEvent(trap.TrapData.NbRoomsBeforeEffect,Effect.FEU,_effectsAppliedAfterRoom[Effect.FEU]));
                }
            )
        },

    };

    private static Dictionary<Effect, Action<Trap, Group,HeroesManager>> _effectsAppliedAfterRoom = new Dictionary<Effect, Action<Trap, Group, HeroesManager>>() {
        {
            Effect.GLACE,
            (Trap trap,Group group,HeroesManager manager) => {
                if (trap.IsActive)
                {
                    int j = 0;
                    while (j < trap.Effects.Count)
                    {
                        if (trap.Effects[j] != Effect.FEU)
                        {
                            manager.ApplyDamageToEachHero(trap.Effects[j]);
                            j++;
                        } else
                        {
                            trap.Effects.RemoveAt(j);
                        }
                    }
                }
            }
        },
        {
            Effect.FEU,
            (Trap trap,Group group,HeroesManager manager) => {
                if (trap.IsActive)
                {
                    //Enleve l'effet de glace
                    bool hasFireEffect = false;
                    for (int j = 0  ; j < trap.Effects.Count ; j++) 
                    {
                        if (trap.Effects[j] != Effect.GLACE)
                        {
                            if (trap.Effects[j] == Effect.FEU)
                            {
                                hasFireEffect = true;
                            }
                            j++;
                        } else
                        {
                            trap.Effects.RemoveAt(j);
                        }
                    }
                    if (!hasFireEffect)
                    {
                        trap.Effects.Add(Effect.FEU);
                    }
                }
            }
        }


    };

    public static Dictionary<Effect, Action<Trap, Group, HeroesManager>> EffectsAppliedAfterRoom {
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
    private int _turns;

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

    private Action<Trap, Group, HeroesManager> _onRoomEffectApplied;
    private Effect _effect;

    public EffectEvent(int nbRoomBeforeApplied, Effect effect, Action<Trap, Group,HeroesManager> onRoomEffectApplied)
    {
        _nbRoomBeforeApplied = nbRoomBeforeApplied;
        _effect = effect;
        _onRoomEffectApplied += onRoomEffectApplied;
    }
}