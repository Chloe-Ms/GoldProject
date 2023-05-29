using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    [SerializeField] Group[] _groupPerLevel;
    [SerializeField] float _spaceBetweenHeroes = 1f;
    List<Hero> _heroes;

    private void Awake()
    {
        _heroes = new List<Hero>();
    }

    private void Start()
    {
        
    }

    private void OnValidate()
    {
        for (int i = 0; i < _groupPerLevel.Length; i++)
        {
            _groupPerLevel[i].name = "Level " + (i + 1);
        }
    }
    void LoadHeroesInLevel(int level)
    {
        _heroes.Clear();
        if (_groupPerLevel.Length < (level - 1))
        {
            for(int i = 0;i < _groupPerLevel[level - 1].ListHeroesInGroup.Length; i++)
            {
                GameObject go = Instantiate(_groupPerLevel[level - 1].ListHeroesInGroup[i]);
                go.transform.position = go.transform.position + new Vector3(_spaceBetweenHeroes, 0,0);
                Hero hero = go?.GetComponent<Hero>();
                if (hero != null)
                {
                    _heroes.Add(hero);
                }
            }
        }
    }
}
