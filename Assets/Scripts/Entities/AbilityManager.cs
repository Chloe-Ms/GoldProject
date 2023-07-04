using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityManager
{
    private static Dictionary<Role,Action<Group,Room>> _activateAbilities = new Dictionary<Role, Action<Group, Room>>()
    {
        {
            Role.HEALER,
            (Group group, Room room) =>
            {
                if ((room.TrapData.RoomType == RoomType.NORMAL && !room.IsActive) ||
                room.TrapData.RoomType == RoomType.LEVER ||
                room.TrapData.RoomType == RoomType.ENTRANCE)
                {
                    GameManager.Instance.HealGroup();
                }
            }
        },
        {
            Role.PALADIN,
            (Group group, Room room) =>
            {
                
            }
        },
        {
            Role.MAGE,
            (Group group, Room room) =>
            {
                Hero hero = group.GetHeroWithRole(Role.MAGE);
                if (hero.NbDamageOnElementaryRoom == 3 && room.IsActive && room.IsElementary)
                {
                    hero.NbDamageOnElementaryRoom = 0;
                    group.IsInvulnerable = true;
                }
            }
        }

    };
    private static Dictionary<Role, Action<Group, Room>> _activateAfterRoomAbilities = new Dictionary<Role, Action<Group, Room>>() 
    {
        {
            Role.CHEVALIER,
            (Group group, Room room) =>
            {
                if (room.Effects.Count > 0 && room.Effects[0] == Effect.MONSTRE)
                {
                    for (int i = 0; i < group.Heroes.Count; i++)
                    {
                        if (!group.Heroes[i].IsDead)
                        {
                            group.Heroes[i].HasDamageReduction = true;
                        }
                    }
                }
            }
        }
    };
    private static Dictionary<Role, Action<Group, Room,Effect>> _activateDuringRoomAbilities = new Dictionary<Role, Action<Group, Room,Effect>>()
    {
        {
            Role.PALADIN,
            (Group group, Room room,Effect roomEffect) =>
            {
                //Search hero with fatal attack

                for (int i = 0; i < group.Heroes.Count; i++)
                {
                    group.Heroes[i].IsInvulnerable = false;
                    int damage = GameManager.Instance.GetDamageOnHero(roomEffect,group.Heroes[i]);
                    if (group.Heroes[i].Health + damage <= 0 && group.Heroes[i].Role != Role.PALADIN)
                    {
                        group.Heroes[i].IsInvulnerable = true;
                    }
                }
            }
        }
    };
    private static Dictionary<Role, Action<Group>> _deactivateAbilities = new Dictionary<Role, Action<Group>>()
    {
        {
            Role.HEALER,
            (Group group) =>
            {
                
            }
        },
        {
            Role.PALADIN,
            (Group group) =>
            {
                //Remove invulnerability
                for (int i = 0; i < group.Heroes.Count; i++)
                {
                    if (group.Heroes[i].IsInvulnerable)
                    {
                        group.Heroes[i].IsInvulnerable = false;
                    }
                }
            }
        },
        {
            Role.MAGE,
            (Group group) =>
            {
                group.IsInvulnerable = false;
            }
        },
        {
            Role.CHEVALIER,
            (Group group) =>
            {
                for (int i = 0; i < group.Heroes.Count; i++)
                {
                    if (group.Heroes[i].HasDamageReduction)
                    {
                        group.Heroes[i].HasDamageReduction = false;
                    }
                }
            }
        }

    };

    public static Dictionary<Role, Action<Group, Room>> ActivateAbilities { 
        get => _activateAbilities; 
    }
    public static Dictionary<Role, Action<Group>> DeactivateAbilities { 
        get => _deactivateAbilities; 
    }
    public static Dictionary<Role, Action<Group, Room>> ActivateAfterRoomAbilities { 
        get => _activateAfterRoomAbilities;
    }
    public static Dictionary<Role, Action<Group, Room, Effect>> ActivateDuringRoomAbilities { 
        get => _activateDuringRoomAbilities;
    }
}
