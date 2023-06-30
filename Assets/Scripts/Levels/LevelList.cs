using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "GoldProject/Create list of levels")]
public class LevelList : ScriptableObject
{
    [SerializeField] LevelData[] _levels;
}
