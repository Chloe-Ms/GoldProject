using NaughtyAttributes;
using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [HideInInspector] public string name; //public for the editor
    [SerializeField] int _nbMovesMax;
    [SerializeField] HeroData[] _listHeroesInLevel;
    [SerializeField] int[] _maxHealth;
    [SerializeField, MinValue(2)] int _mapWidth;
    [SerializeField, MinValue(2)] int _mapHeight;
    [SerializeField] PrePlacedElement _prePlacedElements;
    [SerializeField] TutorialData _tutorial; //peut etre nul attention

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

    public PrePlacedElement PrePlacedElements { 
        get => _prePlacedElements;
    }

    public TutorialData Tutorial //peut etre nul attention
    {
        get => _tutorial;
    }
}
