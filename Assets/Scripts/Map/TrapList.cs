using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap List", menuName = "GoldProject/Trap List", order = 4)]
public class TrapList : ScriptableObject
{
    [SerializeField] private List<TrapData> _trapData = new List<TrapData>();

    public List<TrapData> TrapData
    {
        get { return _trapData; }
    }
}
