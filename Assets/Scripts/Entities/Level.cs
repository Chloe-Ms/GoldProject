using System;
using UnityEngine;

[Serializable]
public class Level
{
    [HideInInspector] public string name; //public for the editor
    [SerializeField] int _golds;
    [SerializeField] GameObject[] _listHeroesInLevel;
    [SerializeField] int _mapWidth;
    [SerializeField] int _mapHeight;
    public GameObject[] ListHeroesInGroup {
        get => _listHeroesInLevel;
        private set => _listHeroesInLevel = value; 
    }
}
