using System;
using UnityEngine;

[Serializable]
public class Level
{
    [HideInInspector] public string name; //public for the editor
    [SerializeField] int _golds;
    [SerializeField] GameObject[] _listHeroesInLevel;

    public GameObject[] ListHeroesInGroup {
        get => _listHeroesInLevel;
        private set => _listHeroesInLevel = value; 
    }
}
