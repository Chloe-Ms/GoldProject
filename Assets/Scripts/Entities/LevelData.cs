using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [HideInInspector] public string name; //public for the editor
    [SerializeField] int _nbMovesMax;
    [SerializeField] HeroData[] _listHeroesInLevel;
    [SerializeField] int[] _maxHealth;
    [SerializeField] int _mapWidth;
    [SerializeField] int _mapHeight;
    public HeroData[] ListHeroesInGroup {
        get => _listHeroesInLevel;
        private set => _listHeroesInLevel = value; 
    }
    public int MapWidth { 
        get => _mapWidth; 
    }
    public int MapHeight { 
        get => _mapHeight;
    }
    public int NbMovesMax { 
        get => _nbMovesMax;
    }
    public int[] MaxHealth { 
        get => _maxHealth;
    }
}
