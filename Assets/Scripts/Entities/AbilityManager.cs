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
                    group.Heroes.ForEach(hero =>
                    {
                        if (!hero.IsDead)
                        {
                            hero.UpdateHealth(1);
                        }
                    });
                }
            }
        },
        {
            Role.PALADIN,
            (Group group, Room room) =>
            {
                //Search hero with only one pv
                int i = 0;
                bool foundHero = false;
                while (i < group.Heroes.Count && !foundHero)
                {
                    if (group.Heroes[i].Health == 1 && group.Heroes[i].Role != Role.PALADIN)
                    {
                        group.Heroes[i].Isinvulnerable = true;
                        foundHero = true;
                    }
                    i++;
                }
            }
        },
        {
            Role.MAGE,
            (Group group, Room room) =>
            {
                Hero hero = group.GetHeroWithRole(Role.MAGE);
                if (hero.NbDamageOnElementaryRoom == 3 && room.IsElementary)
                {
                    hero.NbDamageOnElementaryRoom = 0;
                    group.IsInvulnerable = true;
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
                int i = 0;
                bool foundHero = false;
                while (i < group.Heroes.Count && !foundHero)
                {
                    if (group.Heroes[i].Isinvulnerable)
                    {
                        group.Heroes[i].Isinvulnerable = false;
                        foundHero = true;
                    }
                    i++;
                }
            }
        },
        {
            Role.MAGE,
            (Group group) =>
            {

                group.IsInvulnerable = false;
            }
        }

    };

    public static Dictionary<Role, Action<Group, Room>> ActivateAbilities { 
        get => _activateAbilities; 
    }
    public static Dictionary<Role, Action<Group>> DeactivateAbilities { 
        get => _deactivateAbilities; 
    }
}
