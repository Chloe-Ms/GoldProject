using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Dictionary<Role,Action<Group,Trap>> _abilities = new Dictionary<Role, Action<Group, Trap>>()
    {
        {
            Role.HEALER,
            (Group group, Trap trap) =>
            {
                if (!trap.IsActive)
                {
                    group.Heroes.ForEach(hero =>
                    {
                        hero.UpdateHealth(1);
                    });
                }
            }
        },
        {
            Role.PALADIN,
            (Group group, Trap trap) =>
            {
                //group.Heroes
            }
        }

    };
}
