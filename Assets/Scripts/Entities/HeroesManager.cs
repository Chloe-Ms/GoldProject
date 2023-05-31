using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    GameObject[] _heroesInCurrentLevel;
    [SerializeField] float _spaceBetweenHeroes = 1f;
    List<Hero> _heroes;

    private void Awake()
    {
        _heroes = new List<Hero>();
    }

    private void Start()
    {
        GameManager.Instance.OnEnterEditorMode += OnChangeLevel;
        GameManager.Instance.OnEnterPlayMode += StartPlayMode;
    }

    private void StartPlayMode(int level)
    {
        InstantiateHeroesInLevel(level);
    }

    private void OnChangeLevel(int level)
    {
        _heroesInCurrentLevel = GameManager.Instance.GetHeroesCurrentLevel();
        StartEditMode(level);
    }

    private void StartEditMode(int level)
    {
        for (int i = 0; i < _heroesInCurrentLevel.Length; i++)
        {
            Hero hero = _heroesInCurrentLevel[i]?.GetComponent<Hero>();
            Debug.Log("Nom : " + hero.HeroName + "\n" + "MaxHealth : " + hero.MaxHealth);
        }
    }

    
    void InstantiateHeroesInLevel(int level)
    {
        for (int i = _heroes.Count - 1; i >= 0; i--)
        {
            Destroy(_heroes[i].gameObject);
        }
        _heroes.Clear();
        if (_heroesInCurrentLevel.Length >= level)
        {
            for(int i = 0;i < _heroesInCurrentLevel.Length; i++)
            {
                GameObject go = Instantiate(_heroesInCurrentLevel[i]);
                go.transform.position = go.transform.position + new Vector3(i * _spaceBetweenHeroes, 0,0);
                Hero hero = go?.GetComponent<Hero>();
                if (hero != null)
                {
                    _heroes.Add(hero);
                }
            }
        }
    }
}
