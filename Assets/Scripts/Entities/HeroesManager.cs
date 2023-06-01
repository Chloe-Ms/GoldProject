using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroesManager : MonoBehaviour
{
    HeroData[] _heroesDataInCurrentLevel;
    Group _heroesInCurrentLevel;

    [SerializeField] float _spaceBetweenHeroes = 1f;
    [SerializeField] GameObject _heroPrefab;
    [SerializeField] HeroesSensibility _heroesSensibilities;
    [SerializeField] float _poisonDamageMultiplier = 3f;

    public event Action OnAnyHeroDeath;

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
        _heroesDataInCurrentLevel = GameManager.Instance.GetHeroesCurrentLevel();
        StartEditMode(level);
    }

    private void StartEditMode(int level)
    {
        RemoveHeroesGameObjects();

        for (int i = 0; i < _heroesDataInCurrentLevel.Length; i++)
        {
            HeroData hero = _heroesDataInCurrentLevel[i];
            Debug.Log("Nom : " + hero.heroName + "\n" + "MaxHealth : " + hero.maxHealth);
        }
    }

    
    void InstantiateHeroesInLevel(int level)
    {
        if (_heroesDataInCurrentLevel.Length >= level)
        {
            for(int i = 0;i < _heroesDataInCurrentLevel.Length; i++)
            {
                GameObject go = Instantiate(_heroPrefab);
                go.transform.position = go.transform.position + new Vector3(i * _spaceBetweenHeroes, 0,0);
                Hero hero = go?.GetComponent<Hero>();
                hero?.LoadHeroData(_heroesDataInCurrentLevel[i]);
                if (hero != null)
                {
                    _heroesInCurrentLevel.Heroes.Add(hero);
                }
            }
        }
    }
    public void RemoveHeroesGameObjects()
    {
        for (int i = _heroesInCurrentLevel.Heroes.Count - 1; i >= 0; i--)
        {
            Destroy(_heroesInCurrentLevel.Heroes[i].gameObject);
        }
        _heroesInCurrentLevel.Heroes.Clear();
    }

    public void ApplyDamageToEachHero(int damage)
    {
        if (_heroesInCurrentLevel.IsPoisoned)
        {
            damage *= 3;
        }
        foreach (Hero hero in _heroesInCurrentLevel.Heroes)
        {
            if (!hero.IsDead)
            {
                hero.TakeDamage(damage);
            }
        }
    }
}
