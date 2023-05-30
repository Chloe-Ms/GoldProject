using System;
using UnityEngine;

[Serializable]
public class Group
{
    [HideInInspector] public string name; //public for the editor
    [SerializeField] GameObject[] _listHeroesInGroup;

    public GameObject[] ListHeroesInGroup {
        get => _listHeroesInGroup;
        private set => _listHeroesInGroup = value; 
    }
}
